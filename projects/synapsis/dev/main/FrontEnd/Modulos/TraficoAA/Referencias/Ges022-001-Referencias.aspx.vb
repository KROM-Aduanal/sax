
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
Imports Rec.Globals.Controllers.ControladorRecursosAduanales
Imports Syn.CustomBrokers.Controllers.ControladorRecursosAduanales
Imports Syn.CustomBrokers.Controllers

#End Region

Public Class Ges022_001_Referencia
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

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

            .DataObject = New ConstructorReferencia(True)


            .addFilter(SeccionesReferencias.SREF1, CamposReferencia.CP_REFERENCIA, "Referencia")
            .addFilter(SeccionesReferencias.SREF1, CamposReferencia.CP_PEDIMENTO, "Pedimento")
            .addFilter(SeccionesReferencias.SREF2, CamposReferencia.CA_RAZON_SOCIAL_IOE, "Cliente")

        End With

        'If Not Page.IsPostBack Then

        scRegimen.DataEntity = New krom.Anexo22()

        scClaveDocumento.DataEntity = New krom.Anexo22()

        scAduanaEntradaSalida.DataEntity = New krom.Anexo22

        'scAduanaDespacho.DataEntity = New krom.Anexo22

        scDestinoMercancia.DataEntity = New krom.Anexo22

        scEjecutivoCuenta.DataEntity = New krom.Ejecutivos

        scEjecutivoCuenta.FreeClauses = " and i_Cve_DivisionMiEmpresa = " & Statements.GetOfficeOnline()._id

        'End If
    End Sub

    Public Overrides Sub BotoneraClicNuevo()

        If OperacionGenerica IsNot Nothing Then

            '_empresa = Nothing

        End If

        swcTipoOperacion.Checked = True
        swcTipoOperacion.Enabled = True
        swcRectificacion.Enabled = True
        icFechaATA.Visible = False
        icFechaRevalidacion.Visible = False
        icFechaRegistro.Visible = False
        icFechaPagoPedimento.Visible = False
        icFechaDespacho.Visible = False
        icFechaPrevio.Visible = False
        scTipoPrevio.Visible = False

        PreparaControles()

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        If Not ProcesarTransaccion(Of ConstructorReferencia)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        InicializaPrefijo()

        icFechaATA.Visible = False
        icFechaRevalidacion.Visible = False
        icFechaRegistro.Visible = False
        icFechaPagoPedimento.Visible = False
        icFechaDespacho.Visible = False
        icFechaPrevio.Visible = False
        scTipoPrevio.Visible = False

        swcTipoOperacion.Enabled = False
        swcRectificacion.Enabled = False

        scTipoReferencia.Enabled = False
        scTipoDocumento.Enabled = False
        scEjecutivoCuenta.Enabled = False


    End Sub

    Public Overrides Sub BotoneraClicBorrar()


    End Sub


    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        [Set](dbcReferencia, CP_REFERENCIA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](dbcReferencia, CP_PEDIMENTO, propiedadDelControl_:=PropiedadesControl.ValueDetail)
        [Set](icOriginal, CP_ORIGINAL)
        [Set](swcTipoOperacion, CP_TIPO_OPERACION)
        [Set](scTipoReferencia, CP_TIPO_REFERENCIA)
        [Set](swcMaterialPeligroso, CP_MATERIAL_PELIROSO)
        [Set](swcRectificacion, CP_RECTIFICACION)
        [Set](scTipoCarga, CP_TIPO_CARGA)
        [Set](scPatente, CP_MODALIDAD_ADUANA_PATENTE2)
        [Set](scRegimen, CP_REGIMEN)
        [Set](scTipoDocumento, CA_TIPO_DOCUMENTO)
        [Set](scClaveDocumento, CP_CLAVE_DOCUMENTO)
        [Set](scAduanaEntradaSalida, CP_ADUANA_ENTRADA_SALIDA)
        [Set](scAduanaDespacho, CP_ADUANA_DESPACHO)
        [Set](scDestinoMercancia, CP_DESTINO_MERCANCIA)
        [Set](scEjecutivoCuenta, CP_EJECUTIVO_CUENTA)

        [Set](fbcCliente, CamposReferencia.CP_ID_IOE)
        [Set](fbcCliente, CamposReferencia.CA_RAZON_SOCIAL_IOE, propiedadDelControl_:=PropiedadesControl.Text)
        [Set](icRFC, CamposReferencia.CA_RFC_DEL_IOE)
        [Set](icCURP, CamposReferencia.CA_CURP_DEL_IOE)
        [Set](icRFCFacturacion, CamposReferencia.CP_RFC_FACTURACION_IOE)
        [Set](icBancoPago, CamposReferencia.CA_BANCO_PAGO_IOE)

        [Set](icNumero, CamposReferencia.CP_DESCRIPCION_DETALLE)
        [Set](scTipoDato, CamposReferencia.CP_TIPO_DETALLE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccCDetallesliente, Nothing, seccion_:=SeccionesReferencias.SREF3)

        [Set](swcEntrada, CP_ES_ENTRADA)
        [Set](scPagoAnticipado, CP_ES_PAGO_ANTICIPADO)
        [Set](icFechaETA, CP_FECHA_ETA)

        [Set](icFechaRecepcionDocumentos, CP_FECHA_RECEPCION_DOCUMENTOS)

        Return New TagWatcher(1)

    End Function

    Protected Sub PreparaControles()
        'Inicializa Tipo de Referencia
        Dim infoprefijolocal_ As New List(Of SelectOption) From {New SelectOption With {.Value = 1, .Text = "Operativa"}}
        scTipoReferencia.DataSource = infoprefijolocal_
        scTipoReferencia.Value = 1

        'Inicizliza prefijo
        InicializaPrefijo()

        'GeneraPrefijoReferencia
        dbcReferencia.Value = GeneraReferenciaPedimento(True, False, TipoReferenciaPedimento.Prefijo)

    End Sub


    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        dbcReferencia.Value = dbcReferencia.Value & GeneraSecuencia("Referencias", Statements.GetOfficeOnline._id, Year(Now), 0, 0, 0, scPrefijo.Value).ToString.PadLeft(8, "0")

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

        Return sec_

    End Function

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


    Protected Sub scTipoReferencia_SelectedIndexChanged(sender As Object, e As EventArgs)

        InicializaPrefijo()

    End Sub

    Protected Sub scPrefijo_SelectedIndexChanged(sender As Object, e As EventArgs)

        dbcReferencia.Value = GeneraReferenciaPedimento(True, False, TipoReferenciaPedimento.Prefijo)

    End Sub

    Protected Sub scTipoDocumento_SelectedIndexChanged(sender As Object, e As EventArgs)

        If scTipoDocumento.Value IsNot Nothing And scTipoDocumento.Value = 4 Then

            icOriginal.Visible = True

            swcRectificacion.Checked = True

        Else

            icOriginal.Visible = False

            swcRectificacion.Checked = False

        End If

    End Sub

    Protected Sub swcTipoOperacion_CheckedChanged(sender As Object, e As EventArgs)

        If swcTipoOperacion.Checked Then

            icFechaETD.Visible = False

            icFechaETA.Visible = True

        Else

            icFechaETD.Visible = True

            icFechaETA.Visible = False

        End If

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

    Protected Sub scPatente_Click(sender As Object, e As EventArgs)

        scPatente.DataSource = CargaPatente()

    End Sub

    Protected Sub scAduanaDespacho_Click(sender As Object, e As EventArgs)

        scAduanaDespacho.DataSource = AduanasSeccion()

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

        Select Case scTipoReferencia.Value

            Case ControladorRecursosAduanales.TiposReferenciasOperativas.Operativas

                tipoPrefijo_ = ControladorRecursosAduanales.TiposPrefijosEnviroment.ReferenciaOperativaNormal

            Case ControladorRecursosAduanales.TiposReferenciasOperativas.Corresponsalias

                tipoPrefijo_ = ControladorRecursosAduanales.TiposPrefijosEnviroment.ReferenciaOperativaCorresponsalia

            Case ControladorRecursosAduanales.TiposReferenciasOperativas.CorresponsaliasTerceros

                tipoPrefijo_ = ControladorRecursosAduanales.TiposPrefijosEnviroment.ReferenciaOperativaCorresponsaliasTerceros

            Case Else

                tipoPrefijo_ = ControladorRecursosAduanales.TiposPrefijosEnviroment.SinDefinir

        End Select

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

    Public Function CargaPatente() As List(Of SelectOption)

        Dim recursos_ As ControladorRecursosAduanales = BuscarRecursosAduanales(ControladorRecursosAduanales.TiposRecurso.Generales)

        Dim patentes_ = From data In recursos_.patentes
                        Where data.archivado = False And data.estado = 1
                        Select data._idpatente, data.agenteaduanal

        If patentes_.Count > 0 Then

            Dim soPatentes_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To patentes_.Count - 1

                soPatentes_.Add(New SelectOption With
                             {.Value = patentes_(index_)._idpatente,
                              .Text = patentes_(index_)._idpatente.ToString & " - " & patentes_(index_).agenteaduanal})

            Next

            Return soPatentes_

        End If

        Return Nothing

    End Function

    Protected Sub InicializaCliente(ByVal datosCliente_ As OperacionGenerica)

        icRFC.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CA_RFC_CLIENTE).Valor

        icCURP.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CA_CURP_CLIENTE).Valor

        'icBancoPago.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CP_CVE_BANCO_PAGO).Valor

        icRFCFacturacion.Value = datosCliente_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Campo(CamposClientes.CA_RFC_CLIENTE).Valor

    End Sub

#End Region

End Class