
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports System.Drawing
Imports System.IO
Imports System.Web.Script.Serialization
Imports Cube.Validators
Imports gsol.documento
Imports gsol.Web.Components
Imports gsol.Web.Components.PillboxControl.ToolbarModality
Imports MongoDB.Bson
Imports MongoDB.Driver
'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals.Controllers
'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports Rec.Globals.Utils
Imports Rec.Globals.Utils.Secuencias
'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web
Imports SharpCompress.Common
Imports Syn.CustomBrokers.Controllers
Imports Syn.Documento
Imports Syn.Documento.Componentes
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

    Private _cdocumentos As New ControladorDocumento

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
        [Set](fcImagenProducto, CamposProducto.CP_RUTA_ARCHIVO_MUESTRA)
        [Set](fcImagenProducto, CamposProducto.CP_RUTA_ARCHIVO_MUESTRA, asignarA_:=TiposAsignacion.ValorPresentacion, propiedadDelControl_:=PropiedadesControl.Text)
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

        '[Set](icHistoricoFraccion, CamposProducto.CP_FRACCION_ARANCELARIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](icHistoricoNico, CamposProducto.CP_NICO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](icHistoricoMotivo, CamposProducto.CP_MOTIVO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '[Set](icHistoricoFechaModificacion, CamposProducto.CP_FECHA_MODIFICACION, propiedadDelControl_:=PropiedadesControl.Ninguno)

        '[Set](ccHistorialClasificacion, Nothing, seccion_:=SeccionesProducto.SPTO4)

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

        If Not ProcesarTransaccion(Of ConstructorProducto)().Status = TypeStatus.Errors Then



        End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        PreparaTarjetero(Advanced, pbcDescipcionesFacturas)



        btnRestaurar.Enabled = False

        btnArchivar.Enabled = True

        fscHistoriales.Visible = True

        icMotivo.Visible = True



    End Sub

    Public Overrides Sub BotoneraClicBorrar()


    End Sub

    Public Overrides Sub BotoneraClicOtros(ByVal IndexSelected_ As Integer)


    End Sub

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Esta metodo se manda llamar al dar clic en cualquiera de las opciones del      '
    ' dropdown en la botonera; recibe el valor indice del boton al que se le ha dado '
    ' clic                                                                           '
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


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

        Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")

        Dim userName_ As String = loginUsuario_("WebServiceUserID")

        With documentoElectronico_

            'HISTORICO CLASIFICACIÓN

            With .Seccion(SeccionesProducto.SPTO1)

                If fcImagenProducto.Value = "" Then

                Else

                    Dim valores_ = fcImagenProducto.Value.Replace("[{", "").Replace("}]", "").Replace(Chr(34), "").Split(",")

                    Dim valor_ = valores_(0).Split(":")

                    If valor_.Count > 1 Then

                        Dim valorPresentacion_ = valores_(1).Split(":")

                        .Attribute(CamposProducto.CP_RUTA_ARCHIVO_MUESTRA).Valor = ObjectId.Parse(valor_(1))

                        .Attribute(CamposProducto.CP_RUTA_ARCHIVO_MUESTRA).ValorPresentacion = valorPresentacion_(1)

                    Else

                        .Attribute(CamposProducto.CP_RUTA_ARCHIVO_MUESTRA).Valor = ObjectId.Parse(valor_(0))

                        .Attribute(CamposProducto.CP_RUTA_ARCHIVO_MUESTRA).ValorPresentacion = ""

                    End If

                End If

            End With

            Dim hayHistoricoClasificacion_ = If(ccHistorialClasificacion.DataSource Is Nothing, True, If(ccHistorialClasificacion.DataSource.length = 0, True, False))

            If hayHistoricoClasificacion_ Then

                If fbcFraccionArancelaria.Text <> "" Then

                    With .Seccion(SeccionesProducto.SPTO4).Partida(documentoElectronico_)

                        .Attribute(CamposProducto.CP_FRACCION_ARANCELARIA).Valor = fbcFraccionArancelaria.Text

                        .Attribute(CamposProducto.CP_NICO).Valor = scNico.Text

                        .Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor = DateTime.Now

                        .Attribute(CamposProducto.CP_MOTIVO).Valor = "ALTA"

                        .Attribute(CamposProducto.CP_LOGIN_USUARIO).Valor = userName_

                        .Attribute(CamposProducto.CP_ENVIRONMENT).Valor = __SYSTEM_ENVIRONMENT.Value

                        .Attribute(CamposProducto.CP_ENVIRONMENT).ValorPresentacion = __SYSTEM_ENVIRONMENT.Text.ToUpper

                    End With

                End If

            Else


                Dim encontrado_ = False


                Dim item_ = ccHistorialClasificacion.DataSource(0)

                If fbcFraccionArancelaria.Text & "-" & scNico.Text = item_("icHistoricoFraccion") Then
                    encontrado_ = True

                End If


                If Not encontrado_ Then

                    If fbcFraccionArancelaria.Text <> "" Then

                        With .Seccion(SeccionesProducto.SPTO4).Partida(documentoElectronico_)

                            .Attribute(CamposProducto.CP_FRACCION_ARANCELARIA).Valor = fbcFraccionArancelaria.Text

                            .Attribute(CamposProducto.CP_NICO).Valor = scNico.Text

                            .Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor = DateTime.Now



                            .Attribute(CamposProducto.CP_MOTIVO).Valor = If(icMotivo.Value = "", "ACTUALIZACIÓN", icMotivo.Value)

                            .Attribute(CamposProducto.CP_LOGIN_USUARIO).Valor = userName_



                            .Attribute(CamposProducto.CP_ENVIRONMENT).Valor = __SYSTEM_ENVIRONMENT.Value

                            .Attribute(CamposProducto.CP_ENVIRONMENT).ValorPresentacion = __SYSTEM_ENVIRONMENT.Text.ToUpper

                        End With

                    End If


                End If


            End If

            Dim Nodos_ = .Seccion(SeccionesProducto.SPTO3)

            For Each nodo_ In Nodos_.Nodos

                Dim nodoDescripciones_ = nodo_.Seccion(SeccionesProducto.SPTO5)


                For Each nodoDescripcion_ In nodoDescripciones_.Nodos

                    Dim secuencia_ As New Syn.Operaciones.Secuencia _
                          With {.anio = 0,
                        .environment = 0,
                        .mes = 0,
                        .nombre = "ProductosidKrom",
                        .tiposecuencia = 1,
                        .subtiposecuencia = 0
                        }

                    With nodoDescripcion_

                        Dim idKrom_ = nodoDescripcion_.Campo(CamposProducto.CP_IDKROM).Valor

                        Dim estado_ = nodoDescripcion_.Campo(CamposProducto.CP_IDKROM).estado


                        If idKrom_ = 0 Then

                            .Campo(CamposProducto.CP_IDKROM).Valor = secuencia_.Generar().Result.ObjectReturned.sec

                            .Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor = Date.Now

                            Dim partida_ = documentoElectronico_.Seccion(SeccionesProducto.SPTO6).Partida(documentoElectronico_)

                            partida_.Campo(CamposProducto.CP_IDKROM).Valor = .Campo(CamposProducto.CP_IDKROM).Valor

                            partida_.Campo(CamposProducto.CP_NUMERO_PARTE).Valor = .Campo(CamposProducto.CP_NUMERO_PARTE).Valor

                            partida_.Campo(CamposProducto.CP_TIPO_ALIAS).Valor = .Campo(CamposProducto.CP_TIPO_ALIAS).Valor

                            partida_.Campo(CamposProducto.CP_APLICACOVE).Valor = .Campo(CamposProducto.CP_APLICACOVE).Valor

                            partida_.Campo(CamposProducto.CP_DESCRIPCION).Valor = .Campo(CamposProducto.CP_DESCRIPCION).Valor

                            partida_.Campo(CamposClientes.CA_RAZON_SOCIAL).Valor = nodo_.Campo(CamposClientes.CA_RAZON_SOCIAL).ValorPresentacion

                            partida_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor = nodo_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).ValorPresentacion

                            partida_.Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor = .Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor

                            partida_.Campo(CamposProducto.CP_LOGIN_USUARIO).Valor = userName_

                            partida_.Campo(CamposProducto.CP_ENVIRONMENT).Valor = __SYSTEM_ENVIRONMENT.Value

                            partida_.Campo(CamposProducto.CP_ENVIRONMENT).ValorPresentacion = __SYSTEM_ENVIRONMENT.Text.ToUpper


                        Else

                            Dim diccionarioDescrionciones_ = GetVars("diccionarioDescrionciones_")

                            For Each descripcion_ In diccionarioDescrionciones_


                                If descripcion_("icIdKrom") = idKrom_ And estado_ = 1 Then

                                    Dim tipoalias_ = descripcion_("scTipoAlias")

                                    If descripcion_("icNumeroParte") <> .Campo(CamposProducto.CP_NUMERO_PARTE).Valor OrElse
                                       descripcion_("icDescripcion") <> .Campo(CamposProducto.CP_DESCRIPCION).Valor OrElse
                                       descripcion_("swcAplicaCove") <> .Campo(CamposProducto.CP_APLICACOVE).Valor OrElse
                                       descripcion_("icDescripcionCove") <> .Campo(CamposProducto.CP_DESCRIPCION_COVE).Valor OrElse
                                       descripcion_("icAlias") <> .Campo(CamposProducto.CP_ALIAS).Valor Then


                                        Dim partida_ = documentoElectronico_.Seccion(SeccionesProducto.SPTO6).Partida(documentoElectronico_)

                                        partida_.Campo(CamposProducto.CP_IDKROM).Valor = .Campo(CamposProducto.CP_IDKROM).Valor

                                        partida_.Campo(CamposProducto.CP_NUMERO_PARTE).Valor = .Campo(CamposProducto.CP_NUMERO_PARTE).Valor

                                        partida_.Campo(CamposProducto.CP_TIPO_ALIAS).Valor = .Campo(CamposProducto.CP_TIPO_ALIAS).Valor

                                        partida_.Campo(CamposProducto.CP_APLICACOVE).Valor = .Campo(CamposProducto.CP_APLICACOVE).Valor

                                        partida_.Campo(CamposProducto.CP_DESCRIPCION).Valor = .Campo(CamposProducto.CP_DESCRIPCION).Valor

                                        partida_.Campo(CamposClientes.CA_RAZON_SOCIAL).Valor = nodo_.Campo(CamposClientes.CA_RAZON_SOCIAL).ValorPresentacion

                                        partida_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor = nodo_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).ValorPresentacion

                                        partida_.Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor = .Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor

                                        partida_.Campo(CamposProducto.CP_LOGIN_USUARIO).Valor = userName_

                                        partida_.Campo(CamposProducto.CP_ENVIRONMENT).Valor = __SYSTEM_ENVIRONMENT.Value

                                        partida_.Campo(CamposProducto.CP_ENVIRONMENT).ValorPresentacion = __SYSTEM_ENVIRONMENT.Text.ToUpper

                                    End If
                                End If

                            Next

                        End If

                    End With

                Next

            Next

        End With

        ColocaHistóricoClasificaciones(documentoElectronico_)

        ColocaHistóricoDescripciones(documentoElectronico_)

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

        'Dim seccionUnicaClasificacion_ = documentoElectronico_.Seccion(SeccionesProducto.SPTO4)

        'With seccionUnicaClasificacion_

        '    ccHistorialClasificacion.ClearRows()

        '    For indice_ As Int32 = 1 To .CantidadPartidas

        '        ccHistorialClasificacion.SetRow(Sub(ByVal catalogRow_ As CatalogRow)

        '                                            With .Partida(indice_)

        '                                                catalogRow_.SetIndice(ccHistorialClasificacion.KeyField, indice_)
        '                                                catalogRow_.SetColumn(icHistoricoFraccion, .Attribute(CamposProducto.CP_FRACCION_ARANCELARIA).Valor)
        '                                                catalogRow_.SetColumn(icHistoricoNico, .Attribute(CamposProducto.CP_NICO).Valor)
        '                                                catalogRow_.SetColumn(icHistoricoMotivo, .Attribute(CamposProducto.CP_MOTIVO).Valor)
        '                                                catalogRow_.SetColumn(icHistoricoFechaModificacion, .Attribute(CamposProducto.CP_FECHA_MODIFICACION).Valor)

        '                                            End With

        '                                        End Sub)

        '    Next

        '    ccHistorialClasificacion.CatalogDataBinding()

        '    SetVars("_clasificacionArchivados", ccHistorialClasificacion.DataSource)

        'End With


        'LLENADO DEL HISTORICO DE DESCRIPCIONES

        'Dim seccionUnicaDescripciones_ = documentoElectronico_.Seccion(SeccionesProducto.SPTO6)

        'With seccionUnicaDescripciones_

        '    ccHistorialDescripciones.ClearRows()

        '    Dim cuenta_ = 1

        '    For Each nodoDescripcion_ In .Nodos

        '        ccHistorialDescripciones.SetRow(Sub(catalogRow_ As CatalogRow)

        '                                            'Define el valor Llave de tu fila

        '                                            catalogRow_.SetIndice(ccHistorialDescripciones.KeyField, cuenta_)

        '                                            'Define el valor de una columna de la fila



        '                                            Dim icHistoricoNumeroParteT_ As New InputControl With {.ID = "icHistoricoNumeroParte",
        '                                                                               .Value = nodoDescripcion_.Campo(CamposProducto.CP_NUMERO_PARTE).Valor,
        '                                                                               .Type = InputControl.InputType.Text}

        '                                            Dim icHistoricoDescripcionT_ As New InputControl With {.ID = "icHistoricoDescripcion",
        '                                                                               .Value = nodoDescripcion_.Campo(CamposProducto.CP_DESCRIPCION).Valor,
        '                                                                               .Type = InputControl.InputType.Text}

        '                                            Dim icHistoricoProveedorT_ As New InputControl With {.ID = "icHistoricoProveedor",
        '                                                                               .Value = nodoDescripcion_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor,
        '                                                                               .Type = InputControl.InputType.Text}

        '                                            Dim icHistoricoClienteT_ As New InputControl With {.ID = "icHistoricoCliente",
        '                                                                               .Value = nodoDescripcion_.Campo(CamposClientes.CA_RAZON_SOCIAL).Valor,
        '                                                                               .Type = InputControl.InputType.Text}

        '                                            catalogRow_.SetColumn(icHistoricoNumeroParteT_, nodoDescripcion_.Campo(CamposProducto.CP_NUMERO_PARTE).Valor)

        '                                            catalogRow_.SetColumn(icHistoricoDescripcionT_, nodoDescripcion_.Campo(CamposProducto.CP_DESCRIPCION).Valor)

        '                                            catalogRow_.SetColumn(icHistoricoProveedorT_, nodoDescripcion_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor)

        '                                            catalogRow_.SetColumn(icHistoricoClienteT_, nodoDescripcion_.Campo(CamposClientes.CA_RAZON_SOCIAL).Valor)

        '                                            'de esta manera agregamos todas las columnas de nuestra fila 
        '                                            'usando el control asociado a la columna y el valor que se asignara

        '                                        End Sub)
        '        cuenta_ += 1


        '    Next

        '    ccHistorialDescripciones.CatalogDataBinding()

        'End With

        fscHistoriales.Visible = True

        With documentoElectronico_.Seccion(SeccionesProducto.SPTO1)

            If fcImagenProducto.Value = "" Or fcImagenProducto.Value = "000000000000000000000000" Then

            Else

                Dim doc As Byte() = (New ControladorDocumento).GetDocument(.Campo(CamposProducto.CP_RUTA_ARCHIVO_MUESTRA).Valor).ObjectReturned

                Dim memoryStream_ As New MemoryStream(doc)

                Randomize()

                Dim filename_ = "prueba" & Rnd(10000) & ".jpg"

                Dim filePath As String = "C:/inetpub/wwwroot/saxtest/sax/projects/synapsis/dev/main/FrontEnd/Recursos/Imgs/" & filename_ ' Ruta del archivo

                Dim fileMode As FileMode = FileMode.Create ' Modo de acceso (crear en este caso)

                Dim fileStream As FileStream = New FileStream(filePath, fileMode)

                memoryStream_.WriteTo(fileStream)

                fileStream.Close()

                fcImagenProducto.CssClass = "col-xs-12 col-md-4"

                icMuestraProducto.Source = "/FrontEnd/Recursos/Imgs/" & filename_

                SetVars("PATH", icMuestraProducto.Source)

                icMuestraProducto.Visible = True

                'fcImagenProducto.Visible = False


            End If

        End With



    End Sub

    Public Overrides Sub DespuesBuquedaGeneralConDatos()

        ColocaHistóricoClasificaciones(OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente)

        ColocaHistóricoDescripciones(OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente)



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

        fcImagenProducto.CssClass = "col-xs-12 col-md-6"

        scNico.Value = Nothing

        scNico.DataSource = Nothing

        icMotivo.Visible = False

        icMuestraProducto.Visible = False


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

    Protected Sub fbc_Cliente_TextChanged(sender As Object, e As EventArgs)

        Dim controlBusqueda_ = New ControladorBusqueda(Of ConstructorCliente)

        Dim references_ = controlBusqueda_.Buscar(fbcCliente.Text, New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

        fbcCliente.DataSource = references_

    End Sub

    Protected Sub fbc_Proveedor_TextChanged(sender As Object, e As EventArgs)

        Dim controlBusqueda_ = New ControladorBusqueda(Of ConstructorProveedoresOperativos)

        Dim references_ = controlBusqueda_.Buscar(fbcProveedor.Text, New Filtro With {.IdSeccion = SeccionesProvedorOperativo.SPRO1, .IdCampo = CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR})

        fbcProveedor.DataSource = references_

    End Sub

    Protected Sub fbc_FraccionArancelaria_TextChanged(sender As Object, e As EventArgs)

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

    Protected Sub fbc_FraccionArancelaria_Click(sender As Object, e As EventArgs)


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

        Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")

        Dim userName_ As String = loginUsuario_("WebServiceUserID")

        Dim clasificacionArchivados As Object() = GetVars("_clasificacionArchivados")

        Array.Resize(clasificacionArchivados, clasificacionArchivados.Length + 1)

        clasificacionArchivados(clasificacionArchivados.Length - 1) = New Dictionary(Of String, String) From {
            {ccHistorialClasificacion.KeyField, 0},
            {"icHistoricoFraccion", fbcFraccionArancelaria.Text & "-" & scNico.Text},
            {"icHistoricoMotivo", icMotivo.Value},
            {"icHistoricoFechaModificacion", Date.Now().ToString("yyyy-MM-dd")},
            {"icHistoricoUsuario", userName_}
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

    Public Function GeneraSecuencia(ByVal nombre_ As String,
                                   Optional ByVal enviroment_ As Int16 = 0,
                                   Optional ByVal anio_ As Int16 = 0,
                                   Optional ByVal mes_ As Int16 = 0,
                                   Optional ByVal tipoSecuencia_ As Integer = 0,
                                   Optional ByVal subTipoSecuencia_ As Integer = 0,
                                   Optional ByVal prefijo As String = Nothing
                                   ) As Int32

        Dim _controladorSecuencia = New ControladorSecuencia

        Dim _secuencia = New Syn.Operaciones.Secuencia With {.nombre = nombre_,
            .environment = enviroment_,
            .anio = anio_,
            .mes = mes_,
            .tiposecuencia = tipoSecuencia_,
            .subtiposecuencia = subTipoSecuencia_,
            .prefijo = prefijo
        }

        Dim respuesta_ = _controladorSecuencia.Generar(_secuencia)

        Dim sec_ As Int32 = 0

        Select Case respuesta_.Status

            Case TypeStatus.Ok

                sec_ = respuesta_.ObjectReturned.sec

            Case Else

        End Select

        Return sec_

    End Function

    Protected Sub fcImagenProducto_ChooseFile(sender As PropiedadesDocumento, e As EventArgs)

        Dim id = ObjectId.GenerateNewId().ToString

        With sender
            ._idpropietario = id
            .nombrepropietario = "ZERG"
            .tipovinculacion = PropiedadesDocumento.TiposVinculacion.AgenciaAduanal
            .datosadicionales = New InformacionDocumento With {
                          .foliodocumento = "00000007",
                          .tipodocumento = InformacionDocumento.TiposDocumento.SinDefinir,
                          .datospropietario = New InformacionPropietario With {
                              .nombrepropietario = "ZERG",
                              ._id = id
                          }
                         }
            .formatoarchivo = PropiedadesDocumento.FormatosArchivo.jpg
        End With

        Dim _idDocumento = ObjectId.Parse(id)

        PROBANDOO()

    End Sub

    Sub ColocaHistóricoDescripciones(documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            With .Seccion(SeccionesProducto.SPTO6)

                Dim cantidadPartidas_ = .Nodos.Count

                ccHistorialDescripciones.ClearRows()

                For indice_ = 1 To cantidadPartidas_

                    Dim partida_ = .Partida(cantidadPartidas_ - indice_ + 1)

                    ccHistorialDescripciones.SetRow(Sub(catalogRow_ As CatalogRow)

                                                        'Define el valor Llave de tu fila

                                                        catalogRow_.SetIndice(ccHistorialDescripciones.KeyField, indice_)

                                                        'de esta manera agregamos todas las columnas de nuestra fila 
                                                        'usando el control asociado a la columna y el valor que se asignara
                                                        catalogRow_.SetColumn(icHistoricoNumeroParte, partida_.Campo(CamposProducto.CP_NUMERO_PARTE).Valor)

                                                        catalogRow_.SetColumn(icHistoricoDescripcion, partida_.Campo(CamposProducto.CP_DESCRIPCION).Valor)

                                                        catalogRow_.SetColumn(icHistoricoCliente, partida_.Campo(CamposClientes.CA_RAZON_SOCIAL).Valor)

                                                        catalogRow_.SetColumn(icHistoricoProveedor, partida_.Campo(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor)

                                                        catalogRow_.SetColumn(icHistoricoFechaModificacionDescripciones, partida_.Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor)

                                                        catalogRow_.SetColumn(icHistoricoUsuarioDescripciones, partida_.Campo(CamposProducto.CP_LOGIN_USUARIO).Valor)

                                                        catalogRow_.SetColumn(icHistoricoOficinaDescripciones, partida_.Campo(CamposProducto.CP_ENVIRONMENT).ValorPresentacion)

                                                    End Sub)


                Next

                ccHistorialDescripciones.CatalogDataBinding()

            End With


        End With

        Dim haycolumnas_ As Boolean = If(ccDescipcionesFacturas.DataSource Is Nothing, False, If(ccDescipcionesFacturas.DataSource.length = 0, False, True))

        If haycolumnas_ Then



            SetVars("diccionarioDescrionciones_", ccDescipcionesFacturas.DataSource)

        End If

    End Sub

    Sub ColocaHistóricoClasificaciones(documentoElectronico_ As DocumentoElectronico)



        With documentoElectronico_



            With .Seccion(SeccionesProducto.SPTO4)

                Dim cantidadPartidas_ = .Nodos.Count

                ccHistorialClasificacion.ClearRows()

                For indice_ = 1 To cantidadPartidas_

                    Dim partida_ = .Partida(cantidadPartidas_ - indice_ + 1)

                    ccHistorialClasificacion.SetRow(Sub(catalogRow_ As CatalogRow)

                                                        'Define el valor Llave de tu fila

                                                        catalogRow_.SetIndice(ccHistorialClasificacion.KeyField, indice_)

                                                        'de esta manera agregamos todas las columnas de nuestra fila 
                                                        'usando el control asociado a la columna y el valor que se asignara
                                                        catalogRow_.SetColumn(icHistoricoFraccion, partida_.Campo(CamposProducto.CP_FRACCION_ARANCELARIA).Valor &
                                                                                                   "-" &
                                                                                                   partida_.Campo(CamposProducto.CP_NICO).Valor)

                                                        catalogRow_.SetColumn(icHistoricoMotivo, partida_.Campo(CamposProducto.CP_MOTIVO).Valor)

                                                        Dim fechaModificacion_ As String = partida_.Campo(CamposProducto.CP_FECHA_MODIFICACION).Valor.ToString.Replace("-", "/")

                                                        If fechaModificacion_.IndexOf("/") = 4 Then

                                                            fechaModificacion_ = DateTime.ParseExact(fechaModificacion_, "dd/MM/yyyy hh:mm tt", Nothing)

                                                        End If

                                                        catalogRow_.SetColumn(icHistoricoFechaModificacion, fechaModificacion_)

                                                        catalogRow_.SetColumn(icHistoricoUsuario, partida_.Campo(CamposProducto.CP_LOGIN_USUARIO).Valor)

                                                        catalogRow_.SetColumn(icHistoricoOficina, partida_.Campo(CamposProducto.CP_ENVIRONMENT).ValorPresentacion)


                                                    End Sub)


                Next

                ccHistorialClasificacion.CatalogDataBinding()

            End With

        End With




    End Sub
    Sub ActualizaImagen()

        icMuestraProducto.Source = GetVars("PATH")

        Dim algo_ = fcImagenProducto

        Dim algo_2 = 0

    End Sub

    Sub PROBANDOO()

        Dim algo_ = fcImagenProducto

        Dim algo_2 = 0

    End Sub

#End Region

End Class
