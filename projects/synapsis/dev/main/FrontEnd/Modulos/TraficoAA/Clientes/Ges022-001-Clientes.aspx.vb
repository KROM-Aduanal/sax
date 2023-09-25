
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports gsol.krom
Imports MongoDB.Bson
Imports Syn.Documento
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports Syn.Operaciones
Imports gsol.Web.Components
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.Recursos.CamposClientes
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus
Imports Syn.Nucleo.RecursosComercioExterior
Imports gsol.Web.Components.FormControl.ButtonbarModality
Imports Syn.Nucleo.Recursos.CamposDomicilio

'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web
Imports Syn.Utils.Organismo
Imports Sax.Web.ControladorBackend.Datos
Imports Sax.Web.ControladorBackend.Cookies

'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals
Imports Rec.Globals.Empresa
Imports Rec.Globals.Controllers

'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports gsol.krom.Anexo22.Vt022AduanaSeccionA01
Imports System.Globalization
Imports Syn.Documento.Componentes.Campo
Imports Rec.Globals.Utils
Imports System.IO
Imports Syn.CustomBrokers.Controllers

#End Region

Public Class Ges022_001_Clientes
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _empresa As Empresa

    Private _sistema As New Syn.Utils.Organismo

    Private _controladorEmpresas As ControladorEmpresas

    Private _tipoObjeto As Type

    Private _cdocumentos As New ControladorDocumento

#End Region
#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Public Overrides Sub Inicializa()

        With Buscador

            .DataObject = New ConstructorCliente(True)

            .addFilter(SeccionesClientes.SCS1, CamposClientes.CA_RAZON_SOCIAL, "Cliente")
            .addFilter(SeccionesClientes.SCS1, CamposClientes.CA_RFC_CLIENTE, "RFC")
            .addFilter(SeccionesClientes.SCS1, CamposClientes.CA_TAX_ID, "TaxID")

        End With

        'icRutaCertificado.Value = "647f42e48f3c19d9cf5ea6c5"
        'icRutaCertificado.Text = "algo.pdf"

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        [Set](i_Cve_Empresa, CP_CVE_EMPRESA, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](i_Cve_Empresa, CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text)

        [Set](0, CP_CVE_DIVISION_MI_EMPRESA, TiposDato.Entero)

        [Set](t_RFC, CA_RFC_CLIENTE)

        [Set](t_TaxID, CA_TAX_ID)

        [Set](t_CURP, CA_CURP_CLIENTE)

        [Set](Convert.ToInt32(IIf(s_tipoPersona.Checked, TiposPersona.Moral, TiposPersona.Fisica)), CP_TIPO_PERSONA, TiposDato.Entero)

        [Set](s_Habilitado, CP_CLIENTE_HABILITADO)

        [Set](Convert.ToInt32(IIf(s_Extranjero.Checked, TiposEmpresa.Nacional, TiposEmpresa.Extranjera)), CP_CLIENTE_EXTRANJERO, TiposDato.Entero)

        'Datos del domicilio

        [Set](t_Calle.Value & " #" & t_NumeroExt.Value & ", " & t_Ciudad.Value & " CP:" & t_CP.Value, CamposDomicilio.CA_DOMICILIO_FISCAL)

        [Set](t_Calle, CamposDomicilio.CA_CALLE, permisoConsulta_:=582)

        [Set](t_NumeroExt, CamposDomicilio.CA_NUMERO_EXTERIOR)

        [Set](t_NumeroInt, CamposDomicilio.CA_NUMERO_INTERIOR)

        [Set](t_CP, CamposDomicilio.CA_CODIGO_POSTAL)

        [Set](t_Colonia, CamposDomicilio.CA_COLONIA)

        [Set](t_Ciudad, CamposDomicilio.CA_CIUDAD)

        [Set](t_Pais, CamposDomicilio.CA_CVE_PAIS)

        [Set](t_Pais, CamposDomicilio.CA_PAIS)

        [Set](icRutaCertificado, CP_RUTA_ARCHIVO_SER_SELLOS)

        [Set](icRutaLlave, CP_RUTA_ARCHIVO_KEY_SELLOS)

        [Set](icFechaVigencia, CP_FECHA_VIGENCIA_SELLOS)

        [Set](icContraseniaCertificado, CP_CONTRASENIA_SELLOS, soloLectura_:=True, permisoConsulta_:=651)

        [Set](icCveWebServices, CP_CVE_WEB_SERVICES_SELLOS)

        'CatConf [_cataduanasdefecto]
        [Set](sc_claveAduanaSeccion, CP_CVE_ADUANA_SECCION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](sc_patenteAduanal, CP_CVE_PATENTE_ADUANAL, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](sc_tipoOperacion, CP_CVE_TIPO_OPERACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](_cataduanasdefecto, Nothing, seccion_:=SeccionesClientes.SCS2)

        'CatConf [ccContactos]
        [Set](icNombreContacto, CP_NOMBRE_CONTACTO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icInfoContacto, CP_INFO_CONTACTO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icTelefono1, CP_TELEFONO1_CONTACTO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icMovil, CP_MOVIL_CONTACTO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCorreoElectronico, CP_EMAIL_CONTACTO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](ccContactos, Nothing, seccion_:=SeccionesClientes.SCS3)

        'CatConf [ccEncargosConferidos]
        [Set](scPatenteAduanaEncargo, CP_CVE_PATENTE_ADUANAL_ENCARGO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icFechaInicialEncargo, CP_FECHA_INICIO_ENCARGO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icFechaFinalEncargo, CP_FECHA_FIN_ENCARGO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scEncargoActivo, CP_ACTIVO_ENCARGO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](ccEncargosConferidos, Nothing, seccion_:=SeccionesClientes.SCS6)

        'CatConf [ccPagoElectronico]
        [Set](scTipoOperacionPago, CP_CVE_TIPO_OPERACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scPatentePago, CP_PATENTE_ADUANA_SECCION_PAGO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scClaveDocumentoPago, CP_CVE_DOCUMENTO_PAGO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scBancoPago, CP_CVE_BANCO_PAGO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icCuentaPago, CP_ID_CUENTA_PAGO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icRangoCuentaPago, CP_RANGO_PAGO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](scEstatusPago, CP_ACTIVO_PAGO, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](ccPagoElectronico, Nothing, seccion_:=SeccionesClientes.SCS7)

        Return New TagWatcher(1)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        If OperacionGenerica IsNot Nothing Then

            _empresa = Nothing

        End If

        s_SeleccionarDomicilio.Checked = True : VerificaCheckDomicilio()

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        'ProcesarOperacion(Of Something)()
        If Not ProcesarTransaccion(Of ConstructorCliente)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        s_EditarDomicilio.Enabled = True

    End Sub

    Public Overrides Sub BotoneraClicBorrar()

        If OperacionGenerica IsNot Nothing Then

            _empresa = Nothing

        End If

    End Sub

    Public Overrides Sub BotoneraClicOtros(IndexSelected_ As Integer)

    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            _controladorEmpresas = New ControladorEmpresas

            With _controladorEmpresas

                .t_CURP = t_CURP.Value

                .t_Calle = t_Calle.Value

                .t_NumeroExt = t_NumeroExt.Value

                .t_NumeroInt = t_NumeroInt.Value

                .t_Colonia = t_Colonia.Value

                .t_Ciudad = t_Ciudad.Value

                .t_Estado = t_Estado.Value

                .t_Pais = t_Pais.Value

                .i_Cve_Empresa = i_Cve_Empresa.Text

                .t_RFC = t_RFC.Value

                .s_tipoPersona = s_tipoPersona.Checked

                .s_Extranjero = s_Extranjero.Checked

                .esNuevoDomicilio = Not s_Domicilios.Visible

                If GetVars("_empresa") IsNot Nothing Then

                    _empresa = GetVars("_empresa")

                    tagwatcher_ = .ActualizaEmpresa(_empresa, session_)

                    If s_Domicilios.Visible = True Then

                        SetVars("_secDomicilio", Convert.ToInt32(s_Domicilios.Value) - 1)

                    End If

                Else

                    tagwatcher_ = .NuevaEmpresa(session_)

                    If tagwatcher_.Status = TypeStatus.Ok Then

                        _empresa = tagwatcher_.ObjectReturned

                        'Grabamos la instancia en la session
                        SetVars("_empresa", _empresa)

                    End If

                End If

                '  ████████fin█████████       Logica de negocios local       ███████████████████████

            End With


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        '************** TEMPORAL ********** (Este segmento se colocará al interior de DocomentoElectronico y como una propiedad en el CMF) ***********
        Dim secuencia_ As New Secuencia _
                  With {.anio = 2022,
                        .environment = 0,
                        .mes = 0,
                        .nombre = "Clientes",
                        .tiposecuencia = 1,
                        .subtiposecuencia = 0
                        }

        Dim respuesta_ As TagWatcher = secuencia_.Generar().Result

        Dim sec_ As Int32 = 0

        Select Case respuesta_.Status

            Case TypeStatus.Ok

                sec_ = respuesta_.ObjectReturned.sec

            Case Else

        End Select

        _empresa = GetVars("_empresa")

        If _empresa IsNot Nothing Then : [Set](_empresa._id, CP_ID_EMPRESA) : End If

        Dim domicilio_ As Int32 = GetVars("_secDomicilio")

        [Set](domicilio_, CamposDomicilio.CP_SEC_DOMICILIO, tipoDato_:=TiposDato.Entero)
        '.Attribute(CamposDomicilio.CP_SEC_DOMICILIO).Valor = GetVars("_secDomicilio")

        If _empresa IsNot Nothing Then : [Set](_empresa.domicilios(GetVars("_secDomicilio"))._iddomicilio, CamposDomicilio.CP_ID_DOMICILIO) : End If
        ''.Attribute(CamposDomicilio.CP_ID_DOMICILIO).Valor = _empresa.domicilios(GetVars("_secDomicilio"))._iddomicilio

        If s_Domicilios.Visible = True Then

            With _empresa.domicilios(GetVars("_secDomicilio"))
                [Set](.calle, CA_CALLE)
                [Set](.numeroexterior, CA_NUMERO_EXTERIOR)
                [Set](.numerointerior, CA_NUMERO_INTERIOR)
                [Set](.cp, CA_CODIGO_POSTAL)
                [Set](.colonia, CA_COLONIA)
                [Set](.ciudad, CA_CIUDAD)
                [Set](.pais, CA_PAIS)
            End With

        End If

        '************** TEMPORAL ********** (Este segmento requiere ser cargado una sola vez y bajo demanda) ***********

        With documentoElectronico_

            .FolioDocumento = _empresa._idempresa

            .FolioOperacion = sec_

            .IdCliente = _empresa._idempresa

            .NombreCliente = _empresa.razonsocial

        End With

        'operacion = referencia

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        Return New TagWatcher(Ok)

    End Function


    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '     ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒

        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            'tagwatcher_ = New TagWatcher

            '.SetOK()

            'Actualizamos los datos del objeto empresa en session
            _controladorEmpresas = New ControladorEmpresas

            With _controladorEmpresas

                .t_CURP = t_CURP.Value

                .t_Calle = t_Calle.Value

                .t_NumeroExt = t_NumeroExt.Value

                .t_NumeroInt = t_NumeroInt.Value

                .t_Colonia = t_Colonia.Value

                .t_Ciudad = t_Ciudad.Value

                .t_Estado = t_Estado.Value

                .t_Pais = t_Pais.Value

                .i_Cve_Empresa = i_Cve_Empresa.Text

                .t_RFC = t_RFC.Value

                .s_tipoPersona = s_tipoPersona.Checked

                .s_Extranjero = s_Extranjero.Checked

                .esNuevoDomicilio = Not s_Domicilios.Visible

                tagwatcher_ = .ActualizaEmpresa(GetVars("_empresa"), session_)

                '  ████████fin█████████        Logica de negocios local      ███████████████████████

            End With

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
            .Attribute(CamposClientes.CP_ID_EMPRESA).Valor = _empresa._id

            ''Datos del domicilio
            .Attribute(CamposDomicilio.CP_ID_DOMICILIO).Valor = _empresa.domicilios(GetVars("_secDomicilio"))._iddomicilio

        End With

    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        's_EditarDomicilio.Checked = False

        'If s_Domicilios.Value IsNot Nothing Then

        '    s_EditarDomicilio.Enabled = True

        'End If

        's_SeleccionarDomicilio.Checked = True
        Return New TagWatcher(Ok)
    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            Dim domicilio_ As New domicilio

            Dim listOptionsDomicilios_ As New List(Of SelectOption)

            SetVars("_empresa", ControladorEmpresas.BuscarEmpresa(.Attribute(CamposClientes.CP_ID_EMPRESA).Valor,
                                                .Attribute(CamposDomicilio.CP_ID_DOMICILIO).Valor,
                                                listOptionsDomicilios_,
                                                domicilio_))
            _empresa = GetVars("_empresa")

            'If .Attribute(CamposClientes.CP_ID_EMPRESA).Valor IsNot Nothing Then

            '    _empresa._id = .Attribute(CamposClientes.CP_ID_EMPRESA).Valor

            'End If

            'Datos del domicilio
            SetVars("_secDomicilio", .Attribute(CamposDomicilio.CP_SEC_DOMICILIO).Valor)
            'SetVars("_secDomicilio", _empresa.domicilios.Last.sec)

        End With

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralConDatos()

        s_SeleccionarDomicilio.Checked = True

        VerificaCheckDomicilio(2)

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

    End Sub


    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        SetVars("_empresa", Nothing)
        SetVars("_empresasTemporal", Nothing)
        SetVars("_secDomicilio", Nothing)

    End Sub

    Public Overrides Sub Limpiar()

        _cataduanasdefecto.DataSource = Nothing
        ccContactos.DataSource = Nothing
        ccEncargosConferidos.DataSource = Nothing
        ccExpedienteLegal.DataSource = Nothing
        ccPagoElectronico.DataSource = Nothing

    End Sub

#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██    Pendientes ( al 07/05/2022 )                                                                ██
    '    ██      1. Mejorar la carga de los dropdowns ( hace las consultas en cada postback)               ██
    '    ██      2. Completar el caso de uso de inserción cuando se reutilice una empresa y domicilio      ██
    '    ██      3. Completar la carga de datos del CRUD ( el resto de las secciones )                     ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Protected Sub CambioTipoEmpresa(ByVal sender As Object, ByVal e As EventArgs)

        If sender.Checked Then 'Extranjera

            t_CURP.Enabled = False

        Else 'Nacional

            t_CURP.Enabled = True

        End If

    End Sub

    Protected Sub s_EditarDomicilio_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        VerificaCheckDomicilio(3)

        If s_EditarDomicilio.Checked Then

            Dim empresasTemporales_ As List(Of Empresa) = GetVars("_empresasTemporal")

            If empresasTemporales_ IsNot Nothing And
                        Not IsNumeric(empresasTemporales_) Then

                If i_Cve_Empresa.Value <> "" Then

                    If IsNumeric(i_Cve_Empresa.Value) Then

                        If i_Cve_Empresa.Value <> -1 Then

                            Dim result_ = From data In empresasTemporales_
                                          Where data._idempresa = i_Cve_Empresa.Value And data.estado = 1
                                          Select data.rfc, data.curp

                            If result_.Count > 0 Then

                                Dim domicilio_ = ControladorEmpresas.BuscarDomicilio(i_Cve_Empresa.Value,
                                                                                         Convert.ToInt32(s_Domicilios.Value),
                                                                                         empresasTemporales_)

                                t_Calle.Value = domicilio_.calle
                                t_NumeroExt.Value = domicilio_.numeroexterior
                                t_NumeroInt.Value = domicilio_.numerointerior
                                t_CP.Value = domicilio_.cp
                                t_Colonia.Value = domicilio_.colonia
                                t_Ciudad.Value = domicilio_.ciudad
                                t_Pais.Value = domicilio_.pais

                            End If

                        End If

                    End If

                End If

            End If

        End If

    End Sub

    Protected Sub s_SeleccionarDomicilio_CheckedChanged(sender As Object, e As EventArgs)

        VerificaCheckDomicilio()

        If GetVars("isEditing") = True Then

            If s_SeleccionarDomicilio.Checked = True Then

                t_Calle.Value = [Get](Of String)(CamposDomicilio.CA_CALLE)
                t_NumeroExt.Value = [Get](Of String)(CamposDomicilio.CA_NUMERO_EXTERIOR)
                t_NumeroInt.Value = [Get](Of String)(CamposDomicilio.CA_NUMERO_INTERIOR)
                t_CP.Value = [Get](Of String)(CamposDomicilio.CA_CODIGO_POSTAL)
                t_Colonia.Value = [Get](Of String)(CamposDomicilio.CA_COLONIA)
                t_Ciudad.Value = [Get](Of String)(CamposDomicilio.CA_CIUDAD)
                t_Pais.Value = [Get](Of String)(CamposDomicilio.CA_PAIS)

            End If

        End If

    End Sub

    Sub VerificaCheckDomicilio(Optional ByVal opcion_ As Int32 = 1)

        Select Case opcion_

            Case 1 'Defaults

                If s_SeleccionarDomicilio.Checked Then

                    If GetVars("isEditing") = False Then

                        t_Calle.Visible = False
                        t_NumeroExt.Visible = False
                        t_NumeroInt.Visible = False
                        t_CP.Visible = False
                        t_Colonia.Visible = False
                        t_Ciudad.Visible = False
                        t_Pais.Visible = False
                        s_Domicilios.Visible = True
                        If s_Domicilios.Value IsNot Nothing And Not String.IsNullOrEmpty(s_Domicilios.Value) Then

                            s_EditarDomicilio.Visible = True

                        End If
                    End If

                Else
                    t_Calle.Visible = True
                    t_Calle.Value = Nothing
                    t_NumeroExt.Visible = True
                    t_NumeroExt.Value = Nothing
                    t_NumeroInt.Visible = True
                    t_NumeroInt.Value = Nothing
                    t_CP.Visible = True
                    t_CP.Value = Nothing
                    t_Colonia.Visible = True
                    t_Colonia.Value = Nothing
                    t_Ciudad.Visible = True
                    t_Ciudad.Value = Nothing
                    t_Pais.Visible = True
                    t_Pais.Value = Nothing
                    s_Domicilios.Visible = False
                    s_EditarDomicilio.Visible = False
                End If

            Case 2

                If s_SeleccionarDomicilio.Checked Then
                    t_Calle.Visible = True
                    t_NumeroExt.Visible = True
                    t_NumeroInt.Visible = True
                    t_CP.Visible = True
                    t_Colonia.Visible = True
                    t_Ciudad.Visible = True
                    t_Pais.Visible = True
                    s_Domicilios.Visible = False
                    s_EditarDomicilio.Visible = False
                Else
                    t_Calle.Visible = False
                    t_NumeroExt.Visible = False
                    t_NumeroInt.Visible = False
                    t_CP.Visible = False
                    t_Colonia.Visible = False
                    t_Ciudad.Visible = False
                    t_Pais.Visible = False
                    s_Domicilios.Visible = True
                    If s_Domicilios.Value IsNot Nothing And Not String.IsNullOrEmpty(s_Domicilios.Value) Then
                        s_EditarDomicilio.Visible = True
                    End If
                End If

            Case 3

                If s_EditarDomicilio.Checked = True Then

                    t_Calle.Visible = True
                    t_NumeroExt.Visible = True
                    t_NumeroInt.Visible = True
                    t_CP.Visible = True
                    t_Colonia.Visible = True
                    t_Ciudad.Visible = True
                    t_Pais.Visible = True
                    s_Domicilios.Visible = False
                    s_SeleccionarDomicilio.Enabled = False

                Else

                    t_Calle.Visible = False
                    t_NumeroExt.Visible = False
                    t_NumeroInt.Visible = False
                    t_CP.Visible = False
                    t_Colonia.Visible = False
                    t_Ciudad.Visible = False
                    t_Pais.Visible = False
                    s_Domicilios.Visible = True
                    s_SeleccionarDomicilio.Enabled = True

                End If

        End Select

    End Sub

    Protected Sub s_Habilitado_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub i_Cve_Empresa_TextChanged(sender As Object, e As EventArgs)

        Dim empresasTemporales_ As New List(Of Empresa)

        Dim lista_ As List(Of SelectOption) = ControladorEmpresas.BuscarEmpresas(empresasTemporales_, i_Cve_Empresa.Text)

        SetVars("_empresasTemporal", empresasTemporales_)

        i_Cve_Empresa.DataSource = lista_

    End Sub

    Protected Sub i_Cve_Empresa_Click(sender As Object, e As EventArgs)

        With s_Domicilios

            .Options.Clear()

            Dim empresasTemporales_ As List(Of Empresa) = GetVars("_empresasTemporal")

            If empresasTemporales_ IsNot Nothing And
                Not IsNumeric(empresasTemporales_) Then

                If i_Cve_Empresa.Value <> "" Then

                    If IsNumeric(i_Cve_Empresa.Value) Then

                        If i_Cve_Empresa.Value <> -1 Then

                            Dim result_ = From data In empresasTemporales_
                                          Where data._idempresa = i_Cve_Empresa.Value And data.estado = 1
                            'Select data.rfc, data.curp

                            If result_.Count > 0 Then

                                SetVars("_empresa", result_(0))

                                t_RFC.Value = result_(0).rfc

                                t_CURP.Value = result_(0).curp

                                Dim domicilios_ = ControladorEmpresas.BuscarDomicilios(i_Cve_Empresa.Value,
                                                               empresasTemporales_)

                                .DataSource = domicilios_

                                If domicilios_.Count > 0 Then

                                    .Value = domicilios_(0).Value

                                    s_EditarDomicilio.Visible = True

                                    s_SeleccionarDomicilio.Checked = True

                                    VerificaCheckDomicilio()

                                End If

                            End If

                        End If

                    End If
                Else

                    t_RFC.Value = Nothing

                    t_CURP.Value = Nothing

                    s_EditarDomicilio.Checked = False

                    s_Domicilios.DataSource = Nothing

                    VerificaCheckDomicilio(3)

                End If

            Else

                .Options.Clear()

                .DataSource = Nothing

                t_RFC.Value = Nothing

                t_CURP.Value = Nothing

            End If

        End With

    End Sub

#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorEmpresas                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


    Protected Sub sc_claveAduanaSeccion_Click()

        sc_claveAduanaSeccion.DataSource = AduanasSeccion()

    End Sub

    Protected Sub sc_patenteAduanal_Click()

        sc_patenteAduanal.DataSource = PatentesAduanales()

    End Sub

    Protected Sub scPatenteAduanaEncargo_Click(sender As Object, e As EventArgs)

        scPatenteAduanaEncargo.DataSource = PatentesAduanales()

    End Sub
    Protected Sub scPatentePago_Click(sender As Object, e As EventArgs)

        scPatentePago.DataSource = PatentesAduanales()

    End Sub

    Private Function PatentesAduanales() As List(Of SelectOption)

        Dim recursos_ As ControladorRecursosAduanales = ControladorRecursosAduanales.BuscarRecursosAduanales(ControladorRecursosAduanales.TiposRecurso.Generales)

        Dim aduanasSecciones_ = From data In recursos_.aduanasmodalidad
                                Where data.archivado = False And data.estado = 1
                                Select data.modalidad, data.ciudad, data._idaduanaseccion

        Dim patentes_ = From data In recursos_.patentes
                        Where data.archivado = False And data.estado = 1
                        Select data.agenteaduanal, data._idpatente

        If patentes_.Count > 0 Then

            Dim dataSource1_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To patentes_.Count - 1

                dataSource1_.Add(New SelectOption With
                             {.Value = patentes_(index_)._idpatente,
                              .Text = patentes_(index_)._idpatente.ToString & "|" & patentes_(index_).agenteaduanal})

            Next

            Return dataSource1_

        End If

        Return Nothing

    End Function

    Private Function AduanasSeccion() As List(Of SelectOption)

        Dim recursos_ As ControladorRecursosAduanales = ControladorRecursosAduanales.BuscarRecursosAduanales(ControladorRecursosAduanales.TiposRecurso.Generales)

        Dim aduanasSecciones_ = From data In recursos_.aduanasmodalidad
                                Where data.archivado = False And data.estado = 1
                                Select data.modalidad, data.ciudad, data._idaduanaseccion

        If aduanasSecciones_.Count > 0 Then

            Dim dataSource1_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To aduanasSecciones_.Count - 1

                dataSource1_.Add(New SelectOption With
                             {.Value = aduanasSecciones_(index_)._idaduanaseccion,
                              .Text = aduanasSecciones_(index_).modalidad.ToString & "|" & aduanasSecciones_(index_).ciudad & "|" & aduanasSecciones_(index_)._idaduanaseccion.ToString})

            Next

            Return dataSource1_

        End If

        Return Nothing

    End Function

    Protected Sub icRutaCertificado_ChooseFile(sender As PropiedadesDocumento, e As EventArgs)

        Dim id = ObjectId.GenerateNewId().ToString

        With sender
            ._idpropietario = id
            .nombrearchivo = "Test2.pdf"
            .nombrepropietario = "Yo Merengues Dos"
            .tipovinculacion = PropiedadesDocumento.TiposVinculacion.AgenciaAduanal
            .datosadicionales = New InformacionDocumento With {
                          .foliodocumento = "00000002",
                          .tipodocumento = InformacionDocumento.TiposDocumento.BL,
                          .datospropietario = New InformacionPropietario With {
                              .nombrepropietario = "Yo Merengues Dos",
                              ._id = id
                          }
                         }
            .formatoarchivo = PropiedadesDocumento.FormatosArchivo.pdf
        End With

    End Sub

#End Region


End Class
