
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

        [Set](txt_IdKrom, CamposProducto.CP_IDKROM, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](txt_NumeroParte, CamposProducto.CP_NUMERO_PARTE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](txt_Alias, CamposProducto.CP_ALIAS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sl_TipoAlias, CamposProducto.CP_TIPO_ALIAS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sl_TipoAlias, CamposProducto.CP_TIPO_ALIAS, asignarA_:=TiposAsignacion.ValorPresentacion, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](txt_Descripcion, CamposProducto.CP_DESCRIPCION, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sw_AplicaCove, CamposProducto.CP_APLICACOVE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](txt_DescripcionCove, CamposProducto.CP_DESCRIPCION_COVE, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](cat_DescipcionesFacturas, Nothing, seccion_:=SeccionesProducto.SPTO5, propiedadDelControl_:=PropiedadesControl.Ninguno)


        [Set](pbx_DescipcionesFacturas, Nothing, seccion_:=SeccionesProducto.SPTO3)
        Return New TagWatcher(1)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        PreparaTarjetero(Simple, pbx_DescipcionesFacturas)

        btn_Restaurar.Enabled = False

        btn_Archivar.Enabled = False

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        'ProcesarOperacion(Of Something)()
        If Not ProcesarTransaccion(Of ConstructorProducto)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        PreparaTarjetero(Advanced, pbx_DescipcionesFacturas)

        btn_Restaurar.Enabled = False

        btn_Archivar.Enabled = True

        _fshistoriales.Visible = True

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

        Dim secuencia_ As New Syn.Operaciones.Secuencia _
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

            cat_HistorialClasificacion.ClearRows()

            For indice_ As Int32 = 1 To .CantidadPartidas

                cat_HistorialClasificacion.SetRow(Sub(ByVal catalogRow_ As CatalogRow)

                                                      With .Partida(indice_)

                                                          catalogRow_.SetIndice(cat_HistorialClasificacion.KeyField, indice_)
                                                          catalogRow_.SetColumn(txt_HistoricoFraccion, .Attribute(CamposProducto.CP_FRACCION_ARANCELARIA).Valor)
                                                          catalogRow_.SetColumn(txt_HistoricoNico, .Attribute(CamposProducto.CP_NICO).Valor)
                                                          catalogRow_.SetColumn(txt_HistoricoMotivo, .Attribute(CamposProducto.CP_MOTIVO).Valor)
                                                          catalogRow_.SetColumn(txt_HistoricoFechaModificacion, .Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor)

                                                      End With

                                                  End Sub)

            Next

            cat_HistorialClasificacion.CatalogDataBinding()

            SetVars("_clasificacionArchivados", cat_HistorialClasificacion.DataSource)

        End With


        'LLENADO DEL HISTORICO DE DESCRIPCIONES

        Dim seccionUnicaDescripciones_ = documentoElectronico_.Seccion(SeccionesProducto.SPTO3)

        With seccionUnicaDescripciones_

            cat_HistorialDescripciones.ClearRows()

            For indice_ As Int32 = 1 To .CantidadPartidas

                If .Partida(indice_).archivado = True Then

                    Dim cliente_ = .Attribute(CamposClientes.CA_RAZON_SOCIAL).ValorPresentacion

                    Dim proveedor_ = .Attribute(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).ValorPresentacion

                    Dim subseccionUnicaDescripciones_ = documentoElectronico_.Seccion(SeccionesProducto.SPTO3).Partida(indice_).Seccion(SeccionesProducto.SPTO5)

                    With subseccionUnicaDescripciones_

                        For subindice_ As Int32 = 1 To .CantidadPartidas

                            cat_HistorialDescripciones.SetRow(Sub(ByVal catalogRow_ As CatalogRow)

                                                                  With .Partida(subindice_)

                                                                      catalogRow_.SetIndice(cat_HistorialDescripciones.KeyField, subindice_)
                                                                      catalogRow_.SetColumn(txt_HistoricoCliente, cliente_)
                                                                      catalogRow_.SetColumn(txt_HistoricoProveedor, proveedor_)
                                                                      catalogRow_.SetColumn(txt_HistoricoNumeroParte, .Attribute(CamposProducto.CP_NUMERO_PARTE).Valor)
                                                                      catalogRow_.SetColumn(txt_HistoricoDescripcion, .Attribute(CamposProducto.CP_DESCRIPCION).Valor)
                                                                      'catalogRow_.SetColumn(txt_HistoricoFechaArchivado, .Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor)

                                                                  End With

                                                              End Sub)

                        Next

                    End With

                End If

            Next

            cat_HistorialDescripciones.CatalogDataBinding()

        End With

        _fshistoriales.Visible = True

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

        Dim controlador_ = New ControladorTIGIE()

        Dim tagwacher_ = controlador_.EnlistarFracciones(fbx_FraccionArancelaria.Text)

        If tagwacher_.Status = TypeStatus.Ok Then

            Dim fracciones_ As List(Of FraccionArancelaria) = tagwacher_.ObjectReturned

            Dim fraccionesData_ = New List(Of SelectOption)

            fracciones_.ForEach(Sub(ByVal fraccion_ As FraccionArancelaria) fraccionesData_.Add(New SelectOption With {.Value = fraccion_.Fraccion, .Text = fraccion_.Fraccion & " | " & fraccion_.DescripcionFraccion}))

            fbx_FraccionArancelaria.DataSource = fraccionesData_

        End If

    End Sub

    Protected Sub fbx_FraccionArancelaria_Click(sender As Object, e As EventArgs)

        Dim controlador_ = New ControladorTIGIE()

        Dim tagwacher_ = controlador_.EnlistarNicosFraccion(fbx_FraccionArancelaria.Value)

        If tagwacher_.Status = TypeStatus.Ok Then

            Dim nicos_ As List(Of NicoFraccionArancelaria) = tagwacher_.ObjectReturned

            Dim nicosData_ = New List(Of SelectOption)

            nicos_.ForEach(Sub(ByVal nico_ As NicoFraccionArancelaria) nicosData_.Add(New SelectOption With {.Value = nico_.Nico, .Text = nico_.Nico & " | " & nico_.DescripcionNico}))

            sl_Nico.DataSource = nicosData_

        End If

    End Sub

    Protected Sub sl_Nico_Click(sender As Object, e As EventArgs)



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

        Dim clasificacionArchivados As Object() = GetVars("_clasificacionArchivados")

        Array.Resize(clasificacionArchivados, clasificacionArchivados.Length + 1)

        clasificacionArchivados(clasificacionArchivados.Length - 1) = New Dictionary(Of String, String) From {
            {cat_HistorialClasificacion.KeyField, 0},
            {"txt_HistoricoFraccion", fbx_FraccionArancelaria.Text},
            {"txt_HistoricoNico", sl_Nico.Text},
            {"txt_HistoricoMotivo", txt_Motivo.Value},
            {"txt_HistoricoFechaModificacion", Date.Now().ToString("yyyy-MM-dd")}
        }

        SetVars("_clasificacionArchivados", clasificacionArchivados)

        cat_HistorialClasificacion.DataSource = clasificacionArchivados

        _fshistoriales.Visible = True

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
