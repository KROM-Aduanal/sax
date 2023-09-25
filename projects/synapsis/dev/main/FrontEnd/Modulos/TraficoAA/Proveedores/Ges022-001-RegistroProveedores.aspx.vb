
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports Syn.Documento
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports Syn.Operaciones
Imports gsol.Web.Components
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior.CamposProveedorOperativo
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.Recursos.CamposDomicilio

'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web

'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals
Imports Rec.Globals.Empresa
Imports Rec.Globals.Controllers

'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports Syn.Documento.Componentes.Campo
Imports Rec.Globals.Utils
Imports gsol
Imports Syn.CustomBrokers.Controllers

#End Region

Public Class Ges022_001_RegistroProveedores
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _empresa As Empresa

    Private _controladorEmpresas As New ControladorEmpresas

    Private _sistema As New Syn.Utils.Organismo

    Private _cantidadDetalles As New Int32

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
            .addFilter(SeccionesProvedorOperativo.SPRO2, CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR, "TaxID")

        End With
        scIncoterm.DataEntity = New krom.Anexo22()
        scMetodoValoracion.DataEntity = New krom.Anexo22()

        'If Not Page.IsPostBack Then

        '    Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Default

        'End If

        'pbDetalleProveedor.Modality = Session("_tbDetalleProveedor")

        If OperacionGenerica IsNot Nothing Then

            'aquí se consulta el número de partidas
            _cantidadDetalles = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas

        End If

    End Sub

    Public Overrides Sub BotoneraClicNuevo()

        If OperacionGenerica IsNot Nothing Then

            _empresa = Nothing

        End If


        'LimpiaSesion()
        'Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Simple : pbDetalleProveedor.Modality = Session("_tbDetalleProveedor")
        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbDetalleProveedor)

        fsVinculaciones.Visible = False
        fsConfiguracionAdicional.Visible = False
        fsHistorialDomicilios.Visible = False
        scDomicilios.DataSource = Nothing

        If pbDetalleProveedor.PageIndex > 0 Then

            lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()

        End If

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        Dim Algo = OperacionGenerica



        If Not ProcesarTransaccion(Of ConstructorProveedoresOperativos)().Status = TypeStatus.Errors Then : End If

        ''Parche temporal para arreglar problema con secciones de partida si se le coloca campos en falso
        'Dim Organismo_ = New Syn.Utils.Organismo

        'Organismo_.RemplazaNodo(OperacionGenerica.Id, New ConstructorProveedoresOperativos, SeccionesProvedorOperativo.SPRO4)

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        icClave.Enabled = False
        swcTipoUso.Enabled = False
        fcRazonSocial.Enabled = False
        swcEditarDomicilio.Enabled = True
        swcEditarDomicilio.Visible = True

        VerificaCheckDomicilio(4)

        'Session("_tbDetalleProveedor") = PillboxControl.ToolbarModality.Advanced : pbDetalleProveedor.Modality = Session("_tbDetalleProveedor")
        PreparaTarjetero(PillboxControl.ToolbarModality.Advanced, pbDetalleProveedor)

    End Sub

    Public Overrides Sub BotoneraClicBorrar()

        swcUtilizarDatos.Checked = True : VerificaCheckDomicilio()

    End Sub


    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        'Datos generales (SeccionesProveedorOperativo.SPRO1)
        [Set](icClave, CP_CVE_PROVEEDOR)
        [Set](fcRazonSocial, CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text)
        [Set](IIf(swcTipoUso.Checked, 2, 1), CP_TIPO_USO, tipoDato_:=TiposDato.Entero)

        'Detalle proveedor
        If pbDetalleProveedor.PageIndex > 0 Then

            lbNumero.Text = pbDetalleProveedor.PageIndex.ToString()

        End If

        If Not swcTipoUso.Checked Then

            [Set](icTaxId, CA_TAX_ID_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        Else

            [Set](icRFC, CA_RFC_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
            [Set](icCURP, CA_CURP_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        End If

        [Set](scDomicilios, CamposDomicilio.CA_DOMICILIO_FISCAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](icCalle.Value & " #" & icNumeroExterior.Value, CamposDomicilio.CA_DOMICILIO_FISCAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCalle, CA_CALLE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroExterior, CA_NUMERO_EXTERIOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroInterior, CA_NUMERO_INTERIOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCodigoPostal, CA_CODIGO_POSTAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icColonia, CA_COLONIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icLocalidad, CA_LOCALIDAD, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCiudad, CA_CIUDAD, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icMunicipio, CA_MUNICIPIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icEntidadFederativa, CA_ENTIDAD_FEDERATIVA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPais, CA_PAIS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](pbDetalleProveedor, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO2)

        'Historial domicilios físcales (SeccionesProvedorOperativo.SPRO3)
        '[Set](icTaxIDRFC, CP_TAX_ID_DOMICILIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](icDomicilio, CamposProveedorOperativo.CA_DOMICILIO_FISCAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](swcArchivarDomicilio, CP_ARCHIVADO_DOMICILIO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](ccDomiciliosFiscales, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO3)

        ' Vinculaciones con clientes (SeccionesProvedorOperativo.SPRO4)
        [Set](scClienteVinculacion, CP_ID_CLIENTE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](scTaxIdVinculacion, CP_TAX_ID_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scTaxIdVinculacion, CP_RFC_PROVEEDOR_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scVinculacion, CA_CVE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPorcentajeVinculacion, CP_PORCENTAJE_VINCULACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccVinculaciones, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO4)

        'Configuración adicional (SeccionesProvedorOperativo.SPRO5)
        [Set](scClienteConfiguracion, CP_ID_CLIENTE_CONFIGURACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](scTaxIdConfiguracion, CP_TAX_ID_CONFIGURACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scTaxIdConfiguracion, CP_RFC_PROVEEDOR_CONFIGURACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scMetodoValoracion, CA_CVE_METODO_VALORACION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scIncoterm, CA_CVE_INCOTERM, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccConfiguracionAdicional, Nothing, seccion_:=SeccionesProvedorOperativo.SPRO5)

        Return New TagWatcher(1)

    End Function

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher


        '      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            _controladorEmpresas = New ControladorEmpresas

            With _controladorEmpresas

                .t_CURP = icCURP.Value

                .t_Calle = icCalle.Value

                .t_NumeroExt = icNumeroExterior.Value

                .t_NumeroInt = icNumeroInterior.Value

                .t_Colonia = icColonia.Value

                .t_CodigoPostal = icCodigoPostal.Value

                .t_Ciudad = icCiudad.Value

                .t_localidad = icLocalidad.Value

                .t_municipio = icMunicipio.Value

                .t_entidadfederativa = icEntidadFederativa.Value

                .t_Pais = icPais.Value

                .i_Cve_Empresa = fcRazonSocial.Text

                .t_RFC = icRFC.Value

                .s_tipoPersona = TiposPersona.Fisica

                .s_Extranjero = IIf(swcTipoUso.Checked, TiposEmpresa.Nacional, TiposEmpresa.Extranjera)

                .esNuevoDomicilio = IIf(swcEditarDomicilio.Checked, True, IIf(swcUtilizarDatos.Checked = False, True, False))

                If GetVars("_empresa") IsNot Nothing Then

                    _empresa = GetVars("_empresa")

                    tagwatcher_ = .ActualizaEmpresa(_empresa, session_)

                    If scDomicilios.Visible = True And scDomicilios.Value <> "" Then

                        SetVars("_secDomicilio", Convert.ToInt32(scDomicilios.Value) - 1)

                    End If

                Else

                    tagwatcher_ = .NuevaEmpresa(session_)

                    If tagwatcher_.Status = TypeStatus.Ok Then

                        _empresa = tagwatcher_.ObjectReturned

                        'Grabamos la instancia en la session
                        SetVars("_empresa", _empresa)

                    End If

                End If

            End With

            'guardar domicilios e historial

            Dim domicilios_ As New List(Of Int32)

            If _empresa IsNot Nothing Then : [Set](_empresa._id, CP_ID_EMPRESA) : End If

            [Set](Convert.ToInt32(_empresa._idempresa), CP_CVE_EMPRESA, tipoDato_:=TiposDato.Entero)

            pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)

                                           If pillbox_.GetControlValue(scDomicilios) IsNot Nothing And pillbox_.GetControlValue(scDomicilios).Value <> "" Then

                                               domicilios_.Add(Convert.ToInt32(pillbox_.GetControlValue(scDomicilios).Value))

                                           Else

                                               _controladorEmpresas = New ControladorEmpresas

                                               With _controladorEmpresas

                                                   .t_CURP = pillbox_.GetControlValue(icCURP)
                                                   .t_RFC = pillbox_.GetControlValue(icRFC)
                                                   .t_Calle = pillbox_.GetControlValue(icCalle)
                                                   .t_NumeroExt = pillbox_.GetControlValue(icNumeroExterior)
                                                   .t_NumeroInt = pillbox_.GetControlValue(icNumeroInterior)
                                                   .t_Colonia = pillbox_.GetControlValue(icColonia)
                                                   .t_CodigoPostal = pillbox_.GetControlValue(icCodigoPostal)
                                                   .t_Ciudad = pillbox_.GetControlValue(icCiudad)
                                                   .t_localidad = pillbox_.GetControlValue(icLocalidad)
                                                   .t_municipio = pillbox_.GetControlValue(icMunicipio)
                                                   .t_entidadfederativa = pillbox_.GetControlValue(icEntidadFederativa)
                                                   .t_Pais = pillbox_.GetControlValue(icPais)
                                                   .i_Cve_Empresa = _empresa.razonsocial
                                                   .esNuevoDomicilio = True

                                                   _empresa = GetVars("_empresa")

                                                   tagwatcher_ = .ActualizaEmpresa(_empresa, session_)

                                               End With

                                           End If

                                       End Sub)

            '  ████████fin█████████       Logica de negocios local       ███████████████████████

        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        '************** TEMPORAL ********** (Este segmento se colocará al interior de DocomentoElectronico y como una propiedad en el CMF) ***********
        Dim secuencia_ As New Secuencia _
                 With {.anio = 0,
                       .environment = 0,
                       .mes = 0,
                       .nombre = "ProveedoresOperativos",
                       .tiposecuencia = 1,
                       .subtiposecuencia = 0
                       }

        Dim respuesta_ As TagWatcher = secuencia_.Generar().Result

        Dim sec_ As Int32 = 0

        Select Case respuesta_.Status

            Case TypeStatus.Ok

                sec_ = respuesta_.ObjectReturned.sec
                icClave.Value = sec_

            Case Else

        End Select

        '**************
        'circulo rojo mis ofertas 
        With documentoElectronico_

            .FolioDocumento = _empresa._idempresa

            .FolioOperacion = sec_

            .IdCliente = _empresa._idempresa

            .NombreCliente = _empresa.razonsocial

        End With

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        CargarHistorialDomicilios()
        fsHistorialDomicilios.Visible = True
        fsVinculaciones.Visible = True
        fsConfiguracionAdicional.Visible = True
        swcEditarDomicilio.Checked = False
        VerificaCheckDomicilio(4)
        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        If _empresa Is Nothing Then

            _empresa = GetVars("_empresa")

        End If

        Dim tagwatcher_ As TagWatcher

        '     ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒

        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            'Actualizamos los datos del objeto empresa en session
            _controladorEmpresas = New ControladorEmpresas

            With _controladorEmpresas

                .t_CURP = icCURP.Value

                .t_Calle = icCalle.Value

                .t_NumeroExt = icNumeroExterior.Value

                .t_NumeroInt = icNumeroInterior.Value

                .t_Colonia = icColonia.Value

                .t_CodigoPostal = icCodigoPostal.Value

                .t_Ciudad = icCiudad.Value

                .t_Pais = icPais.Value

                .i_Cve_Empresa = fcRazonSocial.Text

                .t_RFC = icRFC.Value

                .s_tipoPersona = TiposPersona.Fisica

                .s_Extranjero = IIf(swcTipoUso.Checked, TiposEmpresa.Nacional, TiposEmpresa.Extranjera)

                .esNuevoDomicilio = IIf(swcEditarDomicilio.Checked, True, IIf(swcUtilizarDatos.Checked = False, True, False))

                tagwatcher_ = .ActualizaEmpresa(GetVars("_empresa"), session_)

            End With

            'Guardar domicilios
            Dim domicilios_ As New List(Of Int32)

            If _empresa IsNot Nothing Then : [Set](_empresa._id, CP_ID_EMPRESA) : End If

            pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)

                                           If pillbox_.GetControlValue(scDomicilios) IsNot Nothing And pillbox_.GetControlValue(scDomicilios).Value <> "" Then

                                               domicilios_.Add(Convert.ToInt32(pillbox_.GetControlValue(scDomicilios).Value))

                                           Else

                                               _controladorEmpresas = New ControladorEmpresas

                                               With _controladorEmpresas

                                                   .t_CURP = pillbox_.GetControlValue(icCURP)
                                                   .t_RFC = pillbox_.GetControlValue(icRFC)
                                                   .t_Calle = pillbox_.GetControlValue(icCalle)
                                                   .t_NumeroExt = pillbox_.GetControlValue(icNumeroExterior)
                                                   .t_NumeroInt = pillbox_.GetControlValue(icNumeroInterior)
                                                   .t_Colonia = pillbox_.GetControlValue(icColonia)
                                                   .t_CodigoPostal = pillbox_.GetControlValue(icCodigoPostal)
                                                   .t_Ciudad = pillbox_.GetControlValue(icCiudad)
                                                   .t_localidad = pillbox_.GetControlValue(icLocalidad)
                                                   .t_municipio = pillbox_.GetControlValue(icMunicipio)
                                                   .t_entidadfederativa = pillbox_.GetControlValue(icEntidadFederativa)
                                                   .t_Pais = pillbox_.GetControlValue(icPais)
                                                   .i_Cve_Empresa = _empresa.razonsocial
                                                   .esNuevoDomicilio = True

                                                   _empresa = GetVars("_empresa")

                                                   tagwatcher_ = .ActualizaEmpresa(_empresa, session_)

                                               End With

                                           End If

                                       End Sub)

            '  ████████fin█████████        Logica de negocios local      ███████████████████████


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        _empresa = GetVars("_empresa")

        With documentoElectronico_

            'Datos generales
            .Attribute(CP_ID_EMPRESA).Valor = _empresa._id

            ''Datos del domicilio
            .Attribute(CamposDomicilio.CP_ID_DOMICILIO).Valor = _empresa.domicilios(GetVars("_secDomicilio"))._iddomicilio

        End With

    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        CargarHistorialDomicilios()
        fsHistorialDomicilios.Visible = True
        fsVinculaciones.Visible = True
        fsConfiguracionAdicional.Visible = True

        swcEditarDomicilio.Checked = False
        VerificaCheckDomicilio(4)



        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        If documentoElectronico_ IsNot Nothing Then

            _cantidadDetalles = documentoElectronico_.Seccion(SeccionesProvedorOperativo.SPRO2).CantidadPartidas

        End If

        With documentoElectronico_

            Dim domicilio_ As domicilio

            Dim listOptionsDomicilios_ As New List(Of SelectOption)

            SetVars("_empresa", ControladorEmpresas.BuscarEmpresa(.Attribute(CamposProveedorOperativo.CP_ID_EMPRESA).Valor,
                                                .Attribute(CamposDomicilio.CP_ID_DOMICILIO).Valor,
                                                listOptionsDomicilios_,
                                                domicilio_))
            _empresa = GetVars("_empresa")

            'Datos del domicilio
            SetVars("_secDomicilio", .Attribute(CamposDomicilio.CP_SEC_DOMICILIO).Valor)

        End With

        CargarHistorialDomicilios()

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

        ''* ** * ** Generador de secuencias proveedor operativo ** * ** *
        Dim secuencia_ As New Secuencia _
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
        ''* ** * ** Generador de secuencias ** * ** *
        Return sec_

    End Function

    Public Overrides Sub DespuesBuquedaGeneralConDatos()

        swcUtilizarDatos.Checked = True

        VerificaCheckDomicilio(4)

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

        SetVars("_empresa", Nothing)
        SetVars("_empresasTemporal", Nothing)
        SetVars("_secDomicilio", Nothing)

        GetVars("Parche", Nothing)

    End Sub

    Public Overrides Sub Limpiar()

        ccDomiciliosFiscales.DataSource = Nothing
        ccVinculaciones.DataSource = Nothing
        ccConfiguracionAdicional.DataSource = Nothing

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
                If GetVars("_empresasTemporal") IsNot Nothing Then

                    If GetVars("_empresasTemporal").Count >= 1 Then

                        VerificaCheckDomicilio(4)

                    Else

                        swcUtilizarDatos.Checked = False
                        swcUtilizarDatos.Enabled = False
                        scDomicilios.Enabled = False
                        swcEditarDomicilio.Enabled = False

                    End If

                End If
            Else

                Select Case opcion_

                    Case 1 'Solo mover tarjeta
                        VerificaCheckDomicilio(4)
                        swcUtilizarDatos.Checked = True
                        scDomicilios.Enabled = False
                        swcEditarDomicilio.Enabled = True
                        icRFC.Enabled = icRFC.Value Is ""
                        icCURP.Enabled = icCURP.Value Is ""

                    Case 2 'Cuando es nuevo o copiar
                        swcUtilizarDatos.Checked = False
                        scDomicilios.Enabled = False
                        swcEditarDomicilio.Enabled = False
                        VerificaCheckDomicilio(5)

                End Select

            End If

        End If

    End Sub

    'EVENTOS PARA CONTROLAR QUE PASA CON EL TIPO DE USO
    Protected Sub swcTipoUso_CheckedChanged(sender As Object, e As EventArgs)

        VerificaCheckTipoUso()

    End Sub

    Sub VerificaCheckTipoUso(Optional ByVal opcion_ As Int32 = 1)

        Select Case opcion_

            Case 1 'Defaults

                If Not swcTipoUso.Checked Then
                    icTaxId.Enabled = True
                    icTaxId.Visible = True
                    icRFC.Visible = False
                    icRFC.Enabled = False
                    icCURP.Visible = False
                    icCURP.Enabled = False
                Else
                    icTaxId.Visible = False
                    icTaxId.Enabled = False
                    icRFC.Visible = True
                    icRFC.Enabled = True
                    icCURP.Visible = True
                    icCURP.Enabled = True
                End If

            Case 2

                If swcTipoUso.Checked Then
                    icTaxId.Enabled = False
                    icTaxId.Visible = False
                    icRFC.Visible = True
                    icRFC.Enabled = True
                    icCURP.Visible = True
                    icCURP.Enabled = True

                Else
                    icTaxId.Visible = True
                    icTaxId.Enabled = True
                    icRFC.Visible = False
                    icRFC.Enabled = False
                    icCURP.Visible = False
                    icCURP.Enabled = False

                End If

        End Select

    End Sub

    'EVENTOS PARA CONTROLAR QUE PASA CON LA EDICIÓN DE DOMICILIO
    Protected Sub swcEditarDomicilio_CheckedChanged(sender As Object, e As EventArgs)

        If swcEditarDomicilio.Checked Then

            VerificaCheckDomicilio(5)

        Else

            VerificaCheckDomicilio(4)

        End If

        GuardaCamposDomicilio()

    End Sub

    Protected Sub swcUtilizarDatos_CheckedChanged(sender As Object, e As EventArgs)

        If swcUtilizarDatos.Checked = True Then

            swcEditarDomicilio.Visible = True
            swcEditarDomicilio.Enabled = True
            scDomicilios.Enabled = True

            If swcEditarDomicilio.Checked Then

                VerificaCheckDomicilio(5)

            Else

                VerificaCheckDomicilio(4)

            End If

            GuardaCamposDomicilio()

        Else

            scDomicilios.Enabled = False
            VerificaCheckDomicilio(1)

        End If

    End Sub

    Protected Sub scDomicilios_Click(sender As Object, e As EventArgs)

        GuardaCamposDomicilio()

    End Sub

    Protected Sub scDomicilios_SelectedIndexChanged(sender As Object, e As EventArgs)

        GuardaCamposDomicilio()

    End Sub

    Sub VerificaCheckDomicilio(Optional ByVal opcion_ As Int32 = 1)

        Select Case opcion_

            Case 1 'Defaults

                If swcUtilizarDatos.Checked Then

                    If GetVars("isEditing") = False Then

                        icCalle.Visible = False
                        icNumeroExterior.Visible = False
                        icNumeroInterior.Visible = False
                        icCodigoPostal.Visible = False
                        icColonia.Visible = False
                        icCiudad.Visible = False
                        icLocalidad.Visible = False
                        icMunicipio.Visible = False
                        icEntidadFederativa.Visible = False
                        icPais.Visible = False
                        scDomicilios.Visible = True
                        scDomicilios.Enabled = True
                        If scDomicilios.Value IsNot Nothing And Not String.IsNullOrEmpty(scDomicilios.Value) Then

                            swcEditarDomicilio.Visible = True

                        End If

                    End If

                Else

                    icCalle.Enabled = True
                    icNumeroExterior.Enabled = True
                    icNumeroInterior.Enabled = True
                    icCodigoPostal.Enabled = True
                    icColonia.Enabled = True
                    icCiudad.Enabled = True
                    icLocalidad.Enabled = True
                    icMunicipio.Enabled = True
                    icEntidadFederativa.Enabled = True
                    icPais.Enabled = True
                    icCalle.Visible = True
                    icNumeroExterior.Visible = True
                    icNumeroInterior.Visible = True
                    icCodigoPostal.Visible = True
                    icColonia.Visible = True
                    icCiudad.Visible = True
                    icLocalidad.Visible = True
                    icMunicipio.Visible = True
                    icEntidadFederativa.Visible = True
                    icPais.Visible = True
                    icCalle.Value = Nothing
                    icNumeroExterior.Value = Nothing
                    icNumeroInterior.Value = Nothing
                    icCodigoPostal.Value = Nothing
                    icColonia.Value = Nothing
                    icCiudad.Value = Nothing
                    icLocalidad.Value = Nothing
                    icMunicipio.Value = Nothing
                    icEntidadFederativa.Value = Nothing
                    icPais.Value = Nothing
                    'scDomicilios.Visible = False
                    scDomicilios.Enabled = False
                    'swcEditarDomicilio.Visible = False
                    swcEditarDomicilio.Enabled = False

                End If

            Case 2

                If swcUtilizarDatos.Checked Then

                    icCalle.Enabled = True
                    icNumeroExterior.Enabled = True
                    icNumeroInterior.Enabled = True
                    icCodigoPostal.Enabled = True
                    icColonia.Enabled = True
                    icCiudad.Enabled = True
                    icLocalidad.Enabled = True
                    icMunicipio.Enabled = True
                    icEntidadFederativa.Enabled = True
                    icPais.Enabled = True
                    icCalle.Visible = True
                    icNumeroExterior.Visible = True
                    icNumeroInterior.Visible = True
                    icCodigoPostal.Visible = True
                    icColonia.Visible = True
                    icCiudad.Visible = True
                    icLocalidad.Visible = True
                    icMunicipio.Visible = True
                    icEntidadFederativa.Visible = True
                    icPais.Visible = True
                    scDomicilios.Visible = False
                    scDomicilios.Enabled = False
                    swcEditarDomicilio.Visible = False

                Else

                    icCalle.Enabled = False
                    icNumeroExterior.Enabled = False
                    icNumeroInterior.Enabled = False
                    icCodigoPostal.Enabled = False
                    icColonia.Enabled = False
                    icCiudad.Enabled = False
                    icLocalidad.Enabled = False
                    icMunicipio.Enabled = False
                    icEntidadFederativa.Enabled = False
                    icPais.Enabled = False
                    icCalle.Visible = False
                    icNumeroExterior.Visible = False
                    icNumeroInterior.Visible = False
                    icCodigoPostal.Visible = False
                    icColonia.Visible = False
                    icCiudad.Visible = False
                    icLocalidad.Visible = False
                    icMunicipio.Visible = False
                    icEntidadFederativa.Visible = False
                    icPais.Visible = False
                    scDomicilios.Visible = True
                    scDomicilios.Enabled = True
                    If scDomicilios.Value IsNot Nothing And Not String.IsNullOrEmpty(scDomicilios.Value) Then

                        swcEditarDomicilio.Visible = True

                    End If

                End If

            Case 3

                If swcEditarDomicilio.Checked = True Then

                    icCalle.Visible = True
                    icNumeroExterior.Visible = True
                    icNumeroInterior.Visible = True
                    icCodigoPostal.Visible = True
                    icColonia.Visible = True
                    icCiudad.Visible = True
                    icLocalidad.Visible = True
                    icMunicipio.Visible = True
                    icEntidadFederativa.Visible = True
                    icPais.Visible = True
                    scDomicilios.Visible = False
                    scDomicilios.Enabled = False
                    swcUtilizarDatos.Enabled = False

                Else

                    icCalle.Enabled = False
                    icNumeroExterior.Enabled = False
                    icNumeroInterior.Enabled = False
                    icCodigoPostal.Enabled = False
                    icColonia.Enabled = False
                    icCiudad.Enabled = False
                    icLocalidad.Enabled = False
                    icMunicipio.Enabled = False
                    icEntidadFederativa.Enabled = False
                    icPais.Enabled = False
                    icCalle.Visible = False
                    icNumeroExterior.Visible = False
                    icNumeroInterior.Visible = False
                    icCodigoPostal.Visible = False
                    icColonia.Visible = False
                    icCiudad.Visible = False
                    icLocalidad.Visible = False
                    icMunicipio.Visible = False
                    icEntidadFederativa.Visible = False
                    icPais.Visible = False
                    scDomicilios.Visible = True
                    scDomicilios.Enabled = True
                    swcUtilizarDatos.Enabled = True

                End If

            Case 4 'Desactivar controles

                icCalle.Enabled = False
                icNumeroExterior.Enabled = False
                icNumeroInterior.Enabled = False
                icCodigoPostal.Enabled = False
                icColonia.Enabled = False
                icCiudad.Enabled = False
                icLocalidad.Enabled = False
                icMunicipio.Enabled = False
                icEntidadFederativa.Enabled = False
                icPais.Enabled = False

            Case 5 'Activar controles

                icCalle.Enabled = True
                icNumeroExterior.Enabled = True
                icNumeroInterior.Enabled = True
                icCodigoPostal.Enabled = True
                icColonia.Enabled = True
                icCiudad.Enabled = True
                icLocalidad.Enabled = True
                icMunicipio.Enabled = True
                icEntidadFederativa.Enabled = True
                icPais.Enabled = True

        End Select

    End Sub

    Protected Sub fcRazonSocial_TextChanged(sender As Object, e As EventArgs)

        Dim empresasTemporales_ As New List(Of Empresa)

        Dim lista_ As List(Of SelectOption) = ControladorEmpresas.BuscarEmpresas(empresasTemporales_, fcRazonSocial.Text)

        SetVars("_empresasTemporal", empresasTemporales_)

        fcRazonSocial.DataSource = lista_
        icRFC.Enabled = True
        icCURP.Enabled = True

        If lista_.Count = 0 Then

            swcUtilizarDatos.Checked = False
            swcUtilizarDatos.Enabled = False
            scDomicilios.Enabled = False
            swcEditarDomicilio.Enabled = False
            'Limpiar el tarjetero
            icRFC.Value = Nothing
            icCURP.Value = Nothing
            icTaxId.Value = Nothing
            icCalle.Value = Nothing
            icNumeroExterior.Value = Nothing
            icNumeroInterior.Value = Nothing
            icCodigoPostal.Value = Nothing
            icColonia.Value = Nothing
            icCiudad.Value = Nothing
            icLocalidad.Value = Nothing
            icMunicipio.Value = Nothing
            icEntidadFederativa.Value = Nothing
            icPais.Value = Nothing
            scDomicilios.DataSource = Nothing
            VerificaCheckDomicilio(5)

        Else

            swcUtilizarDatos.Checked = True
            swcUtilizarDatos.Enabled = True
            scDomicilios.Enabled = True
            swcEditarDomicilio.Enabled = True

        End If

    End Sub

    Protected Sub fcRazonSocial_Click(sender As Object, e As EventArgs)

        GuardaCamposDomicilio()

    End Sub

    Sub GuardaCamposDomicilio()

        '**********************
        If GetVars("isEditing") = True Then

            Dim empresasTemporales_ As New List(Of Empresa)

            Dim lista_ As List(Of SelectOption) = ControladorEmpresas.BuscarEmpresas(empresasTemporales_, fcRazonSocial.Text)

            SetVars("_empresasTemporal", empresasTemporales_)

        End If

        If GetVars("_empresasTemporal") IsNot Nothing Then

            Dim empresasTemporales_ As List(Of Empresa) = GetVars("_empresasTemporal")

            If empresasTemporales_ IsNot Nothing And
                    Not IsNumeric(empresasTemporales_) Then

                If empresasTemporales_(0)._idempresa > 0 Then

                    If IsNumeric(empresasTemporales_(0)._idempresa) Then

                        If empresasTemporales_(0)._idempresa <> -1 Then

                            Dim domPresentar_ As Int32 = 0

                            '***********
                            Dim empresa_ = From data In empresasTemporales_
                                           Where data._idempresa = empresasTemporales_(0)._idempresa And data.estado = 1

                            'If empresa_.Count > 0 Then

                            '    SetVars("_empresa", empresa_(0))

                            'End If

                            '***********
                            If empresa_.Count > 0 Then

                                SetVars("_empresa", empresa_(0))

                                If icRFC.Value = "" Or empresa_(0).rfc <> icRFC.Value Then

                                    icRFC.Value = empresa_(0).rfc
                                    icCURP.Value = empresa_(0).curp

                                End If

                                Dim domicilios_ = ControladorEmpresas.BuscarDomicilios(empresasTemporales_(0)._idempresa,
                                                                empresasTemporales_)

                                If domicilios_.Count > 0 And scDomicilios.DataSource Is Nothing Then

                                    scDomicilios.DataSource = domicilios_
                                    scDomicilios.Value = domicilios_(0).Value

                                ElseIf scDomicilios.DataSource.Count <> domicilios_.Count Then

                                    scDomicilios.DataSource = domicilios_
                                    scDomicilios.Value = domicilios_(0).Value

                                End If

                                If scDomicilios.Value <> "" Then

                                    domPresentar_ = Convert.ToInt32(scDomicilios.Value)

                                Else

                                    domPresentar_ = 1

                                End If

                                Dim domicilio_ = ControladorEmpresas.BuscarDomicilio(empresasTemporales_(0)._idempresa,
                                                                                        domPresentar_,
                                                                                        empresasTemporales_)
                                If domicilio_.calle <> "" Then

                                    icCalle.Value = domicilio_.calle
                                    icNumeroExterior.Value = domicilio_.numeroexterior
                                    icNumeroInterior.Value = domicilio_.numerointerior
                                    icCodigoPostal.Value = domicilio_.cp
                                    icColonia.Value = domicilio_.colonia
                                    icLocalidad.Value = domicilio_.localidad
                                    icMunicipio.Value = domicilio_.municipio
                                    icEntidadFederativa.Value = domicilio_.entidadfederativa
                                    icCiudad.Value = domicilio_.ciudad
                                    icPais.Value = domicilio_.pais
                                    swcUtilizarDatos.Checked = True

                                    If swcEditarDomicilio.Checked Then

                                        VerificaCheckDomicilio(5)

                                    Else

                                        VerificaCheckDomicilio(4)

                                    End If

                                    scDomicilios.Visible = True
                                End If

                            End If

                        End If

                    End If

                End If

            End If

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

    'EVENTOS PARA CARGAR LOS CLIENTES EN LAS LISTAS
    Protected Sub scClienteVinculacion_Click(sender As Object, e As EventArgs)

        scClienteVinculacion.DataSource = CargaTopClientes()

    End Sub

    Protected Sub scClienteVinculacion_TextChanged(sender As Object, e As EventArgs)

        Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        Dim lista_ As List(Of SelectOption) = controlador_.Buscar(scClienteVinculacion.SuggestedText, New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        scClienteVinculacion.DataSource = lista_

    End Sub

    Protected Sub scClienteConfiguracion_Click(sender As Object, e As EventArgs)

        scClienteConfiguracion.DataSource = CargaTopClientes()

    End Sub

    Protected Sub scClienteConfiguracion_TextChanged(sender As Object, e As EventArgs)

        Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        Dim lista_ As List(Of SelectOption) = controlador_.Buscar(scClienteConfiguracion.SuggestedText, New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        scClienteConfiguracion.DataSource = lista_

    End Sub

    Private Function CargaTopClientes() As List(Of SelectOption)

        Dim controlador_ As New ControladorBusqueda(Of ConstructorCliente)

        controlador_.AgregarFiltro(SeccionesClientes.SCS1, CamposClientes.CA_RAZON_SOCIAL)
        controlador_.Limit = 5
        controlador_.Buscar("")
        Dim tagwatcher_ = controlador_.ObtenerResultadosBusqueda(CamposClientes.CA_RAZON_SOCIAL)

        If tagwatcher_.Count > 0 Then

            Return tagwatcher_

        End If

        Return Nothing

    End Function

    'EVENTOS PARA CARGAR LAS VINCULACIONES
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

    'EVENTOS PARA CARGAR LOS TAX-ID/RFC 
    Protected Sub scTaxIdVinculacion_Click(sender As Object, e As EventArgs)

        scTaxIdVinculacion.DataSource = IdentificadoresProveedor()

    End Sub

    Private Function IdentificadoresProveedor() As List(Of SelectOption)

        Dim listaIdentificadores_ As New List(Of SelectOption)
        pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)

                                       listaIdentificadores_.Add(New SelectOption With {.Text = pillbox_.GetControlValue(icRFC), .Value = pillbox_.GetIndice(pbDetalleProveedor.KeyField)})

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

    'EVENTO PARA LEER EL HISTORIAL DE DOMICILIO
    Protected Sub CargarHistorialDomicilios()

        Dim i = 1
        pbDetalleProveedor.ForEach(Sub(pillbox_ As PillBox)
                                       ccDomiciliosFiscales.SetRow(Sub(catalogRow_ As CatalogRow)
                                                                       catalogRow_.SetIndice(ccDomiciliosFiscales.KeyField, i)
                                                                       catalogRow_.SetColumn(icTaxIDRFC, pillbox_.GetControlValue(icRFC))
                                                                       catalogRow_.SetColumn(icDomicilio, pillbox_.GetControlValue(icCalle) & " #" & pillbox_.GetControlValue(icNumeroExterior) & " C.P. " & pillbox_.GetControlValue(icCodigoPostal) & " " & pillbox_.GetControlValue(icPais))
                                                                       catalogRow_.SetColumn(swcArchivarDomicilio, False)

                                                                   End Sub)

                                       i += 1

                                   End Sub)

        ccDomiciliosFiscales.CatalogDataBinding()

    End Sub
#End Region

End Class