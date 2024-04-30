
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports gsol.Web.Components
Imports gsol.Web.Components.PillboxControl.ToolbarActions
Imports gsol.Web.Components.PillboxControl.ToolbarModality
Imports MongoDB.Driver
'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals.Controllers
'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports Rec.Globals.Utils
'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web
Imports Syn.Documento
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Operaciones
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus

'Imports MongoDB.Driver.GridFS

#End Region

Public Class Ges022_001_RegistroProductos
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

#End Region


#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Public Overrides Sub Inicializa()

        With Buscador

            .DataObject = New ConstructorProducto(True)

            .addFilter(SeccionesProducto.SPTO1, CamposProducto.CP_NOMBRE_COMERCIAL, "Nombre Comercial")

        End With

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        [Set](icNombreComercial, CamposProducto.CP_NOMBRE_COMERCIAL)
        [Set](swcEstadoProducto, CamposProducto.CP_HABILITADO, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](fbcFraccionArancelaria, CamposProducto.CP_FRACCION_ARANCELARIA)
        [Set](fbcFraccionArancelaria, CamposProducto.CP_FRACCION_ARANCELARIA, asignarA_:=TiposAsignacion.ValorPresentacion, propiedadDelControl_:=PropiedadesControl.Text)
        [Set](icDescripcionFraccion, CamposProducto.CP_DESCRIPCION_FRACCION_ARANCELARIA)
        [Set](scNico, CamposProducto.CP_NICO)
        [Set](icDescripcionNico, CamposProducto.CP_DESCRIPCION_NICO)
        '[Set](fbcNico, CamposProducto.CP_NICO)
        '[Set](fbCNico, CamposProducto.CP_NICO, asignarA_:=TiposAsignacion.ValorPresentacion, propiedadDelControl_:=PropiedadesControl.Text)
        [Set](icFechaRegistro, CamposProducto.CP_FECHA_REGISTRO)

        [Set](scEstatus, CamposProducto.CP_ESTATUS)
        [Set](icObservaciones, CamposProducto.CP_OBSERVACION)
        '[Set](icMotivo, CamposProducto.CP_MOTIVO)

        [Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](fbcProveedor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icIdKrom, CamposProducto.CP_IDKROM, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumeroParte, CamposProducto.CP_NUMERO_PARTE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icAlias, CamposProducto.CP_ALIAS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scTipoAlias, CamposProducto.CP_TIPO_ALIAS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scTipoAlias, CamposProducto.CP_TIPO_ALIAS, asignarA_:=TiposAsignacion.ValorPresentacion, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icDescripcion, CamposProducto.CP_DESCRIPCION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](swcAplicaCove, CamposProducto.CP_APLICACOVE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icDescripcionCove, CamposProducto.CP_DESCRIPCION_COVE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccDescipcionesFacturas, Nothing, seccion_:=SeccionesProducto.SPTO5, propiedadDelControl_:=PropiedadesControl.Ninguno)


        [Set](pbcDescipcionesFacturas, Nothing, seccion_:=SeccionesProducto.SPTO3)
        Return New TagWatcher(1)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        PreparaTarjetero(Simple, pbcDescipcionesFacturas)

        btnRestaurar.Enabled = False

        btnArchivar.Enabled = False

        swcEstadoProducto.Checked = True

        icFechaRegistro.Value = Convert.ToDateTime(Now).Date.ToString("yyyy-MM-dd")

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        'ProcesarOperacion(Of Something)()
        If Not ProcesarTransaccion(Of ConstructorProducto)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        PreparaTarjetero(Advanced, pbcDescipcionesFacturas)

        btnRestaurar.Enabled = False

        btnArchivar.Enabled = True

        fscHistoriales.Visible = True

    End Sub

    Public Overrides Sub BotoneraClicBorrar()


    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████



            '  ████████fin█████████       Logica de negocios local       ███████████████████████

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        'SincronizarCatalogo()

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        Dim secuencia_ As New Secuencia _
                  With {.anio = 0,
                        .environment = 0,
                        .mes = 0,
                        .nombre = "Productos",
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

        With documentoElectronico_

            .FolioDocumento = 0

            .FolioOperacion = sec_

            .IdCliente = 0

            .NombreCliente = Nothing

        End With

    End Sub

    Public Overrides Sub DespuesOperadorDatosProcesar(ByRef documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            'HISTORICO CLASIFICACIÓN
            Dim clasificacionArchivados_ = GetVars("_clasificacionArchivados")

            If clasificacionArchivados_ IsNot Nothing Then
                'Dictionary(Of String, String)
                For Each item_ As Object In clasificacionArchivados_

                    Dim claIndice = Convert.ToInt32(item_.Item(ccHistorialClasificacion.KeyField))

                    If claIndice > 0 Then

                        With .Seccion(SeccionesProducto.SPTO4).Partida(numeroSecuencia_:=claIndice)

                            .Attribute(CamposProducto.CP_FRACCION_ARANCELARIA).Valor = item_.Item("icHistoricoFraccion")
                            .Attribute(CamposProducto.CP_NICO).Valor = item_.Item("icHistoricoNico")
                            .Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor = item_.Item("icHistoricoFechaModificacion")
                            .Attribute(CamposProducto.CP_MOTIVO).Valor = item_.Item("icHistoricoMotivo")

                        End With

                    Else

                        With .Seccion(SeccionesProducto.SPTO4).Partida(documentoElectronico_)

                            .Attribute(CamposProducto.CP_FRACCION_ARANCELARIA).Valor = item_.Item("icHistoricoFraccion")
                            .Attribute(CamposProducto.CP_NICO).Valor = item_.Item("icHistoricoNico")
                            .Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor = item_.Item("icHistoricoFechaModificacion")
                            .Attribute(CamposProducto.CP_MOTIVO).Valor = item_.Item("icHistoricoMotivo")

                        End With

                    End If

                Next

            End If

        End With

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

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        icFechaRegistro.Value = Convert.ToDateTime(Now).Date.ToString("yyyy-MM-dd")

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)


    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        Return New TagWatcher(Ok)

    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        'LLENADO DEL HISTORICO DE CLASIFICACIONES

        Dim seccionUnicaClasificacion_ = documentoElectronico_.Seccion(SeccionesProducto.SPTO4)

        With seccionUnicaClasificacion_

            ccHistorialClasificacion.ClearRows()

            For indice_ As Int32 = 1 To .CantidadPartidas

                ccHistorialClasificacion.SetRow(Sub(ByVal catalogRow_ As CatalogRow)

                                                    With .Partida(indice_)

                                                        catalogRow_.SetIndice(ccHistorialClasificacion.KeyField, indice_)
                                                        catalogRow_.SetColumn(icHistoricoFraccion, .Attribute(CamposProducto.CP_FRACCION_ARANCELARIA).Valor)
                                                        catalogRow_.SetColumn(icHistoricoNico, .Attribute(CamposProducto.CP_NICO).Valor)
                                                        catalogRow_.SetColumn(icHistoricoMotivo, .Attribute(CamposProducto.CP_MOTIVO).Valor)
                                                        catalogRow_.SetColumn(icHistoricoFechaModificacion, .Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor)

                                                    End With

                                                End Sub)

            Next

            ccHistorialClasificacion.CatalogDataBinding()

            SetVars("_clasificacionArchivados", ccHistorialClasificacion.DataSource)

        End With


        'LLENADO DEL HISTORICO DE DESCRIPCIONES

        Dim seccionUnicaDescripciones_ = documentoElectronico_.Seccion(SeccionesProducto.SPTO3)

        With seccionUnicaDescripciones_

            ccHistorialDescripciones.ClearRows()

            For indice_ As Int32 = 1 To .CantidadPartidas

                If .Partida(indice_).archivado = True Then

                    Dim cliente_ = .Attribute(CamposClientes.CA_RAZON_SOCIAL).ValorPresentacion

                    Dim proveedor_ = .Attribute(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).ValorPresentacion

                    Dim subseccionUnicaDescripciones_ = documentoElectronico_.Seccion(SeccionesProducto.SPTO3).Partida(indice_).Seccion(SeccionesProducto.SPTO5)

                    With subseccionUnicaDescripciones_

                        For subindice_ As Int32 = 1 To .CantidadPartidas

                            ccHistorialDescripciones.SetRow(Sub(ByVal catalogRow_ As CatalogRow)

                                                                With .Partida(subindice_)

                                                                    catalogRow_.SetIndice(ccHistorialDescripciones.KeyField, subindice_)
                                                                    catalogRow_.SetColumn(icHistoricoCliente, cliente_)
                                                                    catalogRow_.SetColumn(icHistoricoProveedor, proveedor_)
                                                                    catalogRow_.SetColumn(icHistoricoNumeroParte, .Attribute(CamposProducto.CP_NUMERO_PARTE).Valor)
                                                                    catalogRow_.SetColumn(icHistoricoDescripcion, .Attribute(CamposProducto.CP_DESCRIPCION).Valor)
                                                                    'catalogRow_.SetColumn(icHistoricoFechaArchivado, .Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor)

                                                                End With

                                                            End Sub)

                        Next

                    End With

                End If

            Next

            ccHistorialDescripciones.CatalogDataBinding()

        End With

        fscHistoriales.Visible = True

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralConDatos()



    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

        PreparaTarjetero([Default], pbcDescipcionesFacturas)

    End Sub


    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        SetVars("_CatalogsData", Nothing)

        SetVars("_clasificacionArchivados", Nothing)

        SetVars("_fraccionesArancelarias", Nothing)

        SetVars("_nicos", Nothing)

    End Sub

    Public Overrides Sub Limpiar()

        ccDescipcionesFacturas.DataSource = Nothing

        ccHistorialClasificacion.DataSource = Nothing

        ccHistorialDescripciones.DataSource = Nothing

        scNico.Value = Nothing

        scNico.DataSource = Nothing

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

#End Region

    Protected Sub fbx_Cliente_TextChanged(sender As Object, e As EventArgs)

        Dim controlBusqueda_ = New ControladorBusqueda(Of ConstructorCliente)

        Dim references_ = controlBusqueda_.Buscar(fbcCliente.Text, New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        fbcCliente.DataSource = references_

    End Sub

    Protected Sub fbx_Proveedor_TextChanged(sender As Object, e As EventArgs)

        Dim controlBusqueda_ = New ControladorBusqueda(Of ConstructorProveedoresOperativos)

        Dim references_ = controlBusqueda_.Buscar(fbcProveedor.Text, New Filtro With {.IdSeccion = SeccionesProvedorOperativo.SPRO1, .IdCampo = CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR})

        fbcProveedor.DataSource = references_

    End Sub

    Protected Sub fbx_FraccionArancelaria_TextChanged(sender As Object, e As EventArgs)

        Dim controlador_ = New ControladorTIGIE()

        Dim tagwacher_ = controlador_.EnlistarFracciones(fbcFraccionArancelaria.Text)

        If tagwacher_.Status = TypeStatus.Ok Then

            Dim fracciones_ As List(Of FraccionArancelaria) = tagwacher_.ObjectReturned

            SetVars("_fraccionesArancelarias", fracciones_)

            Dim fraccionesData_ = New List(Of SelectOption)
            '& " | " & fraccion_.DescripcionFraccion
            fracciones_.ForEach(Sub(ByVal fraccion_ As FraccionArancelaria) fraccionesData_.Add(New SelectOption With {.Value = fraccion_.Fraccion, .Text = fraccion_.Fraccion}))

            fbcFraccionArancelaria.DataSource = fraccionesData_

        End If

    End Sub

    Protected Sub fbx_FraccionArancelaria_Click(sender As Object, e As EventArgs)


        If Not String.IsNullOrEmpty(fbcFraccionArancelaria.Text) Then

            Dim controlador_ = New ControladorTIGIE()

            Dim tagwacher_ = controlador_.EnlistarNicosFraccion(fbcFraccionArancelaria.Value)

            If tagwacher_.Status = TypeStatus.Ok Then

                Dim nicos_ As List(Of NicoFraccionArancelaria) = tagwacher_.ObjectReturned

                SetVars("_nicos", nicos_)

                Dim nicosData_ = New List(Of SelectOption)
                '& " | " & nico_.DescripcionNico
                nicos_.ForEach(Sub(ByVal nico_ As NicoFraccionArancelaria) nicosData_.Add(New SelectOption With {.Value = nico_.Nico, .Text = nico_.Nico}))

                scNico.DataSource = nicosData_

                Dim fracciones_ As List(Of FraccionArancelaria) = GetVars("_fraccionesArancelarias")

                Dim fraccion_ As String = fracciones_.Where(Function(u) u.Fraccion = fbcFraccionArancelaria.Value).Select(Function(u) u.DescripcionFraccion).FirstOrDefault()

                icDescripcionFraccion.Value = fraccion_

            End If

        Else

            scNico.DataSource = Nothing

            scNico.Value = Nothing

            icDescripcionFraccion.Value = Nothing

            icDescripcionNico.Value = Nothing

        End If

    End Sub

    Protected Sub sl_Nico_Selected(sender As Object, e As EventArgs)

        Dim nicos_ As List(Of NicoFraccionArancelaria) = GetVars("_nicos")

        Dim nico_ As String = nicos_.Where(Function(u) u.Nico = scNico.Value).Select(Function(u) u.DescripcionNico).FirstOrDefault()

        icDescripcionNico.Value = nico_

    End Sub

    Protected Sub ConfigurarControlesClasificacion_Click(sender As Object, e As EventArgs)

        btnArchivar.Enabled = Not btnArchivar.Enabled

        btnRestaurar.Enabled = Not btnRestaurar.Enabled

        fbcFraccionArancelaria.Visible = Not fbcFraccionArancelaria.Visible

        scNico.Visible = Not scNico.Visible

        icFechaRegistro.Visible = Not icFechaRegistro.Visible

        scEstatus.Visible = Not scEstatus.Visible

        icObservaciones.Visible = Not icObservaciones.Visible

        icMotivo.Visible = Not icMotivo.Visible

        btn_ConfirmarArchivado.Visible = Not btn_ConfirmarArchivado.Visible

    End Sub

    Protected Sub btn_ConfirmarArchivado_Click(sender As Object, e As EventArgs)

        Dim clasificacionArchivados As Object() = GetVars("_clasificacionArchivados")

        Array.Resize(clasificacionArchivados, clasificacionArchivados.Length + 1)

        clasificacionArchivados(clasificacionArchivados.Length - 1) = New Dictionary(Of String, String) From {
            {ccHistorialClasificacion.KeyField, 0},
            {"icHistoricoFraccion", fbcFraccionArancelaria.Text},
            {"icHistoricoNico", scNico.Text},
            {"icHistoricoMotivo", icMotivo.Value},
            {"icHistoricoFechaModificacion", Date.Now().ToString("yyyy-MM-dd")}
        }

        SetVars("_clasificacionArchivados", clasificacionArchivados)

        ccHistorialClasificacion.DataSource = clasificacionArchivados

        fscHistoriales.Visible = True

        fbcFraccionArancelaria.Text = Nothing

        fbcFraccionArancelaria.Value = Nothing

        scNico.Value = Nothing

        scNico.DataSource = Nothing

        icFechaRegistro.Value = Nothing

        scEstatus.Value = Nothing

        icObservaciones.Value = Nothing

        icMotivo.Value = Nothing

        scEstatus.DataSource = Nothing

        scEstatus.Value = Nothing

        ConfigurarControlesClasificacion_Click(Nothing, EventArgs.Empty)

    End Sub

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorEmpresas                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


#End Region

End Class
