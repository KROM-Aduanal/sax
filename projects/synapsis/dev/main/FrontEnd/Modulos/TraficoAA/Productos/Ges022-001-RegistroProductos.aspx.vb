
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

        [Set](txt_NombreComercial, CamposProducto.CP_NOMBRE_COMERCIAL)
        [Set](sw_EstadoProducto, CamposProducto.CP_HABILITADO)
        [Set](fbx_FraccionArancelaria, CamposProducto.CP_FRACCION_ARANCELARIA)
        [Set](fbx_FraccionArancelaria, CamposProducto.CP_FRACCION_ARANCELARIA, asignarA_:=TiposAsignacion.ValorPresentacion, propiedadDelControl_:=PropiedadesControl.Text)
        [Set](sl_Nico, CamposProducto.CP_NICO)
        '[Set](fbx_Nico, CamposProducto.CP_NICO)
        '[Set](fbx_Nico, CamposProducto.CP_NICO, asignarA_:=TiposAsignacion.ValorPresentacion, propiedadDelControl_:=PropiedadesControl.Text)
        [Set](txtFechaRegistro, CamposProducto.CP_FECHA_REGISTRO)
        [Set](sl_Estatus, CamposProducto.CP_ESTATUS)
        [Set](txt_Observaciones, CamposProducto.CP_OBSERVACION)
        '[Set](txt_Motivo, CamposProducto.CP_MOTIVO)
        [Set](fbx_Cliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](fbx_Proveedor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](pbx_DescipcionesFacturas, Nothing, seccion_:=SeccionesProducto.SPTO3)
        Return New TagWatcher(1)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        PreparaTarjetero(Simple, pbx_DescipcionesFacturas)

        btn_Restaurar.Enabled = False

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        'ProcesarOperacion(Of Something)()
        If Not ProcesarTransaccion(Of ConstructorProducto)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        PreparaTarjetero(Advanced, pbx_DescipcionesFacturas)

        btn_Restaurar.Enabled = False

        '_fshistoriales.Visible = True

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

        SincronizarCatalogo()

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

            Dim copyDocument_ = documentoElectronico_

            Dim data_ As List(Of CatalogDataSource) = GetVars("_CatalogsData", New List(Of CatalogDataSource))

            Dim indice_ = 1

            'Historico Facturas
            pbx_DescipcionesFacturas.ForEach(Sub(pillbox_ As PillBox)

                                                 If pillbox_.IsFiled = False And pillbox_.IsDeleted = False Then

                                                     Dim source_ = From item_ As CatalogDataSource In data_ Where item_.identity = pillbox_.GetIdentity() Select item_.source

                                                     If source_.Count > 0 Then

                                                         If source_(0) IsNot Nothing Then

                                                             With .Seccion(SeccionesProducto.SPTO3).Partida(indice_)

                                                                 For Each item_ As Dictionary(Of String, Object) In source_(0)

                                                                     Dim catIndice = Convert.ToInt32(item_.Item(cat_DescipcionesFacturas.KeyField))

                                                                     If catIndice > 0 Then

                                                                         With .Seccion(SeccionesProducto.SPTO5).Partida(numeroSecuencia_:=catIndice)

                                                                             Dim rowId_ = cat_DescipcionesFacturas.DeleteRowsId

                                                                             If rowId_ IsNot Nothing Then

                                                                                 If cat_DescipcionesFacturas.DeleteRowsId.Contains(catIndice) Then

                                                                                     .estado = 0

                                                                                 End If

                                                                             End If

                                                                             .Attribute(CamposProducto.CP_IDKROM).Valor = item_.Item("txt_IdKrom")
                                                                             .Attribute(CamposProducto.CP_NUMERO_PARTE).Valor = item_.Item("txt_NumeroParte")
                                                                             .Attribute(CamposProducto.CP_ALIAS).Valor = item_.Item("txt_Alias")
                                                                             .Attribute(CamposProducto.CP_TIPO_ALIAS).Valor = item_.Item("sl_TipoAlias").Item("Value")
                                                                             .Attribute(CamposProducto.CP_TIPO_ALIAS).ValorPresentacion = item_.Item("sl_TipoAlias").Item("Text")
                                                                             .Attribute(CamposProducto.CP_DESCRIPCION).Valor = item_.Item("txt_Descripcion")
                                                                             .Attribute(CamposProducto.CP_APLICACOVE).Valor = item_.Item("sw_AplicaCove")
                                                                             .Attribute(CamposProducto.CP_DESCRIPCION_COVE).Valor = item_.Item("txt_DescripcionCove")

                                                                         End With

                                                                     Else

                                                                         With .Seccion(SeccionesProducto.SPTO5).Partida(copyDocument_)

                                                                             .Attribute(CamposProducto.CP_IDKROM).Valor = item_.Item("txt_IdKrom")
                                                                             .Attribute(CamposProducto.CP_NUMERO_PARTE).Valor = item_.Item("txt_NumeroParte")
                                                                             .Attribute(CamposProducto.CP_ALIAS).Valor = item_.Item("txt_Alias")
                                                                             .Attribute(CamposProducto.CP_TIPO_ALIAS).Valor = item_.Item("sl_TipoAlias").Item("Value")
                                                                             .Attribute(CamposProducto.CP_TIPO_ALIAS).ValorPresentacion = item_.Item("sl_TipoAlias").Item("Text")
                                                                             .Attribute(CamposProducto.CP_DESCRIPCION).Valor = item_.Item("txt_Descripcion")
                                                                             .Attribute(CamposProducto.CP_APLICACOVE).Valor = item_.Item("sw_AplicaCove")
                                                                             .Attribute(CamposProducto.CP_DESCRIPCION_COVE).Valor = item_.Item("txt_DescripcionCove")

                                                                         End With

                                                                     End If

                                                                 Next

                                                             End With

                                                         End If

                                                     End If

                                                 End If

                                                 indice_ += 1

                                             End Sub)

            'Historio Clasificación
            Dim clasificacionArchivados_ = GetVars("_clasificacionArchivados")

            If clasificacionArchivados_ IsNot Nothing Then

                For Each item_ As Dictionary(Of String, String) In clasificacionArchivados_

                    Dim claIndice = Convert.ToInt32(item_.Item(cat_HistorialClasificacion.KeyField))

                    If claIndice > 0 Then

                        With .Seccion(SeccionesProducto.SPTO4).Partida(numeroSecuencia_:=claIndice)

                            .Attribute(CamposProducto.CP_FRACCION_ARANCELARIA).Valor = item_.Item("txt_HistoricoFraccion")
                            .Attribute(CamposProducto.CP_NICO).Valor = item_.Item("txt_HistoricoNico")
                            .Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor = item_.Item("txt_HistoricoFechaModificacion")
                            .Attribute(CamposProducto.CP_MOTIVO).Valor = item_.Item("txt_HistoricoMotivo")

                        End With

                    Else

                        With .Seccion(SeccionesProducto.SPTO4).Partida(documentoElectronico_)

                            .Attribute(CamposProducto.CP_FRACCION_ARANCELARIA).Valor = item_.Item("txt_HistoricoFraccion")
                            .Attribute(CamposProducto.CP_NICO).Valor = item_.Item("txt_HistoricoNico")
                            .Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor = item_.Item("txt_HistoricoFechaModificacion")
                            .Attribute(CamposProducto.CP_MOTIVO).Valor = item_.Item("txt_HistoricoMotivo")

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

        SincronizarCatalogo()

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

        Dim clasificacionArchivados As List(Of Object) = GetVars("_clasificacionArchivados", New List(Of Object))

        For indice_ As Int32 = 1 To seccionUnicaClasificacion_.CantidadPartidas

            clasificacionArchivados.Add(New Dictionary(Of String, String) From {
                {cat_HistorialClasificacion.KeyField, indice_},
                {"txt_HistoricoFraccion", seccionUnicaClasificacion_.Partida(indice_).Attribute(CamposProducto.CP_FRACCION_ARANCELARIA).Valor},
                {"txt_HistoricoNico", seccionUnicaClasificacion_.Partida(indice_).Attribute(CamposProducto.CP_NICO).Valor},
                {"txt_HistoricoMotivo", seccionUnicaClasificacion_.Partida(indice_).Attribute(CamposProducto.CP_MOTIVO).Valor},
                {"txt_HistoricoFechaModificacion", seccionUnicaClasificacion_.Partida(indice_).Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor}
            })

        Next

        If clasificacionArchivados.Count > 0 Then

            cat_HistorialClasificacion.DataSource = clasificacionArchivados

        End If

        'LLENADO MANUAL DEL CATALOGO DE LA PRIMERA TARJETA

        Dim seccionUnicaFacturasHasFiled = False

        Dim data_ As List(Of CatalogDataSource) = GetVars("_CatalogsData", New List(Of CatalogDataSource))

        Dim seccionUnicaFactura_ = documentoElectronico_.Seccion(SeccionesProducto.SPTO3)

        For indice_ As Int32 = 1 To seccionUnicaFactura_.CantidadPartidas

            Dim catalogDataSource_ = New CatalogDataSource

            catalogDataSource_.identity = seccionUnicaFactura_.Partida(indice_).Attribute(CamposGlobales.CP_IDENTITY).Valor

            catalogDataSource_.source = New List(Of Object)

            For indice2_ As Int32 = 1 To seccionUnicaFactura_.Partida(indice_).Seccion(SeccionesProducto.SPTO5).CantidadPartidas

                With seccionUnicaFactura_.Partida(indice_).Seccion(SeccionesProducto.SPTO5).Partida(indice2_)

                    If .estado = 1 Then

                        Dim row_ As New Dictionary(Of String, Object)

                        Dim valor_ = .Attribute(CamposProducto.CP_TIPO_ALIAS).Valor
                        Dim text_ = .Attribute(CamposProducto.CP_TIPO_ALIAS).ValorPresentacion

                        row_.Add(cat_DescipcionesFacturas.KeyField, indice2_)
                        row_.Add("txt_IdKrom", .Attribute(CamposProducto.CP_IDKROM).Valor)
                        row_.Add("txt_NumeroParte", .Attribute(CamposProducto.CP_NUMERO_PARTE).Valor)
                        row_.Add("txt_Alias", .Attribute(CamposProducto.CP_ALIAS).Valor)
                        row_.Add("sl_TipoAlias", New SelectOption With {.Value = valor_, .Text = text_})
                        row_.Add("txt_Descripcion", .Attribute(CamposProducto.CP_DESCRIPCION).Valor)
                        row_.Add("sw_AplicaCove", .Attribute(CamposProducto.CP_APLICACOVE).Valor)
                        row_.Add("txt_DescripcionCove", .Attribute(CamposProducto.CP_DESCRIPCION_COVE).Valor)

                        catalogDataSource_.source.Add(row_)
                    Else

                        If .archivado = True Then

                            seccionUnicaFacturasHasFiled = True

                        End If

                    End If

                End With

            Next

            data_.Add(catalogDataSource_)

        Next

        If data_.Count Then

            cat_DescipcionesFacturas.DataSource = data_(0).source

        End If

        If clasificacionArchivados.Count > 0 Or seccionUnicaFacturasHasFiled Then

            _fshistoriales.Visible = True

        Else

            _fshistoriales.Visible = False

        End If

        PreparaHistoricoDescipciones()

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralConDatos()



    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

        PreparaTarjetero([Default], pbx_DescipcionesFacturas)

    End Sub


    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        SetVars("_CatalogsData", Nothing)

        SetVars("_clasificacionArchivados", Nothing)

    End Sub

    Public Overrides Sub Limpiar()

        cat_DescipcionesFacturas.DataSource = Nothing

        cat_HistorialClasificacion.DataSource = Nothing

        cat_HistorialDescripciones.DataSource = Nothing

        sl_Nico.Value = Nothing

        sl_Nico.DataSource = Nothing

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

        Dim references_ = controlBusqueda_.Buscar(fbx_Cliente.Text, New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        fbx_Cliente.DataSource = references_

    End Sub

    Protected Sub fbx_Proveedor_TextChanged(sender As Object, e As EventArgs)

        Dim controlBusqueda_ = New ControladorBusqueda(Of ConstructorProveedoresOperativos)

        Dim references_ = controlBusqueda_.Buscar(fbx_Proveedor.Text, New Filtro With {.IdSeccion = SeccionesProvedorOperativo.SPRO1, .IdCampo = CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR})

        fbx_Proveedor.DataSource = references_

    End Sub

    Protected Sub fbx_FraccionArancelaria_TextChanged(sender As Object, e As EventArgs)

        fbx_FraccionArancelaria.DataSource = New List(Of SelectOption) From {
            New SelectOption With {.Value = "1", .Text = "2208.90.03 - Tequila"},
            New SelectOption With {.Value = "2", .Text = "2208.90.04 - Sotol"},
            New SelectOption With {.Value = "3", .Text = "2208.90.05 - Mezcal"},
            New SelectOption With {.Value = "4", .Text = "2208.90.06 - Charanda"}
        }

    End Sub

    Protected Sub fbx_FraccionArancelaria_Click(sender As Object, e As EventArgs)

        FillNicos()

    End Sub

    Protected Sub sl_Nico_Click(sender As Object, e As EventArgs)

        FillNicos()

    End Sub

    Private Sub FillNicos()

        If fbx_FraccionArancelaria.Value = "1" Then

            sl_Nico.DataSource = New List(Of SelectOption) From {
                New SelectOption With {.Value = "1", .Text = "01 - Tequila contenido en envases con capacidad inferior o igual a 5 litros"},
                New SelectOption With {.Value = "2", .Text = "91 - Los demástequilas."}
            }

        End If

        If fbx_FraccionArancelaria.Value = "2" Then

            sl_Nico.DataSource = New List(Of SelectOption) From {
                New SelectOption With {.Value = "3", .Text = "00 - Sotol"}
            }

        End If

        If fbx_FraccionArancelaria.Value = "3" Then

            sl_Nico.DataSource = New List(Of SelectOption) From {
                New SelectOption With {.Value = "3", .Text = "00 - Mezcal"}
            }

        End If

        If fbx_FraccionArancelaria.Value = "4" Then

            sl_Nico.DataSource = New List(Of SelectOption) From {
                New SelectOption With {.Value = "3", .Text = "00 - Charanda"}
            }

        End If

    End Sub

    Protected Sub pbx_DescipcionesFacturas_BeforeClick(sender As Object, e As EventArgs)

        SincronizarCatalogo()

    End Sub

    Protected Sub pbx_DescipcionesFacturas_Click(sender As Object, e As EventArgs)

        Select Case pbx_DescipcionesFacturas.ToolbarAction

            Case Nuevo

                cat_DescipcionesFacturas.DataSource = Nothing

            Case Borrar

                ColocarDatosCatalogo()

            Case Archivar

                ColocarDatosCatalogo()

                PreparaHistoricoDescipciones()

            Case Else

        End Select

    End Sub

    Protected Sub pbx_DescipcionesFacturas_CheckedChange(sender As Object, e As EventArgs)

        SincronizarCatalogo()

        ColocarDatosCatalogo()

    End Sub

    Private Sub ColocarDatosCatalogo()

        pbx_DescipcionesFacturas.GetPillbox(Sub(pillbox As PillBox)

                                                Dim data_ As List(Of CatalogDataSource) = GetVars("_CatalogsData", New List(Of CatalogDataSource))

                                                Dim source_ = From item_ As CatalogDataSource In data_ Where item_.identity = pillbox.GetIdentity() Select item_

                                                If source_.Count > 0 Then

                                                    cat_DescipcionesFacturas.DataSource = source_(0).source

                                                    cat_DescipcionesFacturas.DeleteRowsId = source_(0).deletedIds

                                                End If

                                            End Sub)

    End Sub

    Private Sub SincronizarCatalogo()

        Dim page_ = 0

        If pbx_DescipcionesFacturas.ToolbarAction = Anterior Then

            page_ = pbx_DescipcionesFacturas.PageIndex + 1

        ElseIf pbx_DescipcionesFacturas.ToolbarAction = Siguiente Then

            page_ = pbx_DescipcionesFacturas.PageIndex - 1

        End If

        pbx_DescipcionesFacturas.GetPillbox(Sub(pillbox As PillBox)

                                                Dim data_ As List(Of CatalogDataSource) = GetVars("_CatalogsData", New List(Of CatalogDataSource))

                                                Dim source_ = From item_ As CatalogDataSource In data_ Where item_.identity = pillbox.GetIdentity() Select item_ '.source

                                                If source_.Count = 0 Then

                                                    Dim catalogDataSource_ = New CatalogDataSource

                                                    With catalogDataSource_

                                                        .identity = pillbox.GetIdentity()

                                                        .source = cat_DescipcionesFacturas.DataSource

                                                        .deletedIds = cat_DescipcionesFacturas.DeleteRowsId

                                                        If pbx_DescipcionesFacturas.ToolbarAction = Borrar Then

                                                            .deleted = True

                                                        ElseIf pbx_DescipcionesFacturas.ToolbarAction = Archivar Then

                                                            .filed = True

                                                        End If

                                                    End With

                                                    data_.Add(catalogDataSource_)

                                                Else

                                                    With source_(0)

                                                        .source = cat_DescipcionesFacturas.DataSource

                                                        .deletedIds = cat_DescipcionesFacturas.DeleteRowsId

                                                        If pbx_DescipcionesFacturas.ToolbarAction = Borrar Then

                                                            .deleted = True

                                                        ElseIf pbx_DescipcionesFacturas.ToolbarAction = Archivar Then

                                                            .filed = True

                                                        End If

                                                    End With

                                                End If

                                            End Sub, page_)

    End Sub

    Private Sub PreparaHistoricoDescipciones()

        Dim data_ As List(Of CatalogDataSource) = GetVars("_CatalogsData", New List(Of CatalogDataSource))

        Dim dataSource_ As New List(Of Object)

        pbx_DescipcionesFacturas.ForEach(Sub(pillbox_ As PillBox)

                                             If pillbox_.IsFiled Then

                                                 Dim source_ = From item_ As CatalogDataSource In data_ Where item_.identity = pillbox_.GetIdentity() Select item_.source

                                                 If source_.Count > 0 Then

                                                     If source_(0) IsNot Nothing Then

                                                         For Each item_ As Dictionary(Of String, Object) In source_(0)

                                                             Dim row_ As New Dictionary(Of String, Object)

                                                             row_.Add("txt_HistoricoCliente", pillbox_.GetControlValue(fbx_Cliente).Text)

                                                             row_.Add("txt_HistoricoProveedor", pillbox_.GetControlValue(fbx_Proveedor).Text)

                                                             row_.Add("txt_HistoricoFechaArchivado", Date.Now().ToString("yyyy-MM-dd"))

                                                             row_.Add("txt_HistoricoNumeroParte", item_.Item("txt_NumeroParte"))

                                                             row_.Add("txt_HistoricoDescripcion", item_.Item("txt_Descripcion"))

                                                             row_.Add("indice", 0)

                                                             dataSource_.Add(row_)

                                                         Next

                                                     End If

                                                 End If

                                             End If

                                         End Sub)

        If dataSource_.Count > 0 Then

            cat_HistorialDescripciones.DataSource = dataSource_

            _fshistoriales.Visible = True

        End If

    End Sub

    Protected Sub ConfigurarControlesClasificacion_Click(sender As Object, e As EventArgs)

        btn_Archivar.Enabled = Not btn_Archivar.Enabled

        btn_Restaurar.Enabled = Not btn_Restaurar.Enabled

        fbx_FraccionArancelaria.Visible = Not fbx_FraccionArancelaria.Visible

        sl_Nico.Visible = Not sl_Nico.Visible

        txtFechaRegistro.Visible = Not txtFechaRegistro.Visible

        sl_Estatus.Visible = Not sl_Estatus.Visible

        txt_Observaciones.Visible = Not txt_Observaciones.Visible

        txt_Motivo.Visible = Not txt_Motivo.Visible

        btn_ConfirmarArchivado.Visible = Not btn_ConfirmarArchivado.Visible

    End Sub

    Protected Sub btn_ConfirmarArchivado_Click(sender As Object, e As EventArgs)

        Dim clasificacionArchivados As List(Of Object) = GetVars("_clasificacionArchivados", New List(Of Object))

        clasificacionArchivados.Add(New Dictionary(Of String, String) From {
            {cat_HistorialClasificacion.KeyField, 0},
            {"txt_HistoricoFraccion", fbx_FraccionArancelaria.Text},
            {"txt_HistoricoNico", sl_Nico.Text},
            {"txt_HistoricoMotivo", txt_Motivo.Value},
            {"txt_HistoricoFechaModificacion", Date.Now().ToString("yyyy-MM-dd")}
        })

        cat_HistorialClasificacion.DataSource = clasificacionArchivados

        fbx_FraccionArancelaria.Text = Nothing

        fbx_FraccionArancelaria.Value = Nothing

        sl_Nico.Value = Nothing

        sl_Nico.DataSource = Nothing

        txtFechaRegistro.Value = Nothing

        sl_Estatus.Value = Nothing

        txt_Observaciones.Value = Nothing

        txt_Motivo.Value = Nothing

        sl_Estatus.DataSource = Nothing

        sl_Estatus.Value = Nothing

        ConfigurarControlesClasificacion_Click(Nothing, EventArgs.Empty)

        If clasificacionArchivados.Count > 0 Then

            _fshistoriales.Visible = True

        End If

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

Public Class CatalogDataSource

    Public Property identity As Integer

    Public Property source As Object

    Public Property deletedIds As List(Of Object)

    Public Property deleted As Boolean = False

    Public Property filed As Boolean = False

End Class

'Property ObjectID As ObjectId

'Property Name As String


'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

'End Sub

'Protected Sub Unnamed_CheckedChanged(sender As Object, e As EventArgs)

'    'FileStream Stream
'    'Dim ofd = New OpenFileDialog()
'    'Dim operationsDB_ = iEnlace_.GetMongoClient()
'    'Dim database = operationsDB_.GetDatabase("SynDocs")
'    'Dim fs = New GridFSBucket(database)

'End Sub

'Protected Sub icMate_CheckedChanged(sender As Object, e As EventArgs)

'    'Conexion a MongoDB
'    Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos
'    Dim operationsDB_ = iEnlace_.GetMongoClient()
'    Dim database_ = operationsDB_.GetDatabase("SynDocs")

'    'Nuget GridFS
'    'Dim fs As New GridFSBucket(database_)

'    'Manipulación de atrchivo
'    Dim stream As FileStream = File.Open("C:\Users\guadalupesc\Downloads\curso.pdf", FileMode.Open)
'    Dim buff As Byte() = streamToByteArray(stream)
'    stream.Close()

'    'Agregar opciones al GridFS
'    'Dim opciones = New GridFSUploadOptions With {
'    '        .ChunkSizeBytes = 16000001,
'    '        .BatchSize = Convert.ToInt32(buff.Length),
'    '        .Metadata = New BsonDocument From {{"filename", stream.Name}, {"contentType", ".pdf"}, {"version", 1.0}}
'    '    }

'    'Actualizar el archivo en mongo y regresar su id
'    'ObjectID = fs.UploadFromBytesAsync("3108-curso", buff, opciones).Result

'    'Debe regresar un id
'    MsgBox("¡Terminamos! " & ObjectID.ToString())

'End Sub

'Protected Sub swcHabilitar_CheckedChanged(sender As Object, e As EventArgs)

'    'Conexión a MongoDB
'    Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos
'    Dim operationsDB_ = iEnlace_.GetMongoClient()
'    Dim database_ = operationsDB_.GetDatabase("SynDocs")

'    'Nuget GridFS
'    'Dim fs As New GridFSBucket(database_)

'    'Object ID ----Buscaba como usar Find para pasar el nombre y retornar el id
'    Dim id_ As New ObjectId("630f90eb2c5590d46e46fed1")

'    'Mandar a llamr el GridFs para regresar el arreglo de bytes
'    'Dim test = fs.DownloadAsBytesAsync(id_, Nothing, Nothing)

'    'Mandamos a guardar el archivo
'    Dim rutaPDF = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
'                        "PruebaCurso.pdf")

'    'Sobreescribimos con el arreglo de bytes
'    'File.WriteAllBytes(rutaPDF, test.Result)


'End Sub

'Public Shared Function streamToByteArray(ByVal stream As Stream) As Byte()

'    Using ms As New MemoryStream()
'        stream.CopyTo(ms)
'        Return ms.ToArray()
'    End Using

'End Function
