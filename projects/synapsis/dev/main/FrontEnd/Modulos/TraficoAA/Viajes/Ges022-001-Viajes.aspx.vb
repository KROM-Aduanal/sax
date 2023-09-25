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

#End Region

Public Class Ges022_001_Viajes
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

#End Region

#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    Public Overrides Sub Inicializa()


        With Buscador

            .DataObject = New ConstructorViajes(True)

            .addFilter(SeccionesViajes.SVIA1, CamposViajes.CP_NUMERO_VIAJE, "No. Viaje")
            .addFilter(SeccionesViajes.SVIA1, CamposViajes.CP_NAVE_BUQUE, "Nave/Buque")
            .addFilter(SeccionesViajes.SVIA2, CamposViajes.CP_FECHA_ETA, "Fecha ETA")

        End With

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        [Set](sl_TipoTransporte, CamposViajes.CP_TIPO_TRANSPORTE)
        [Set](sw_Operacion, CamposViajes.CP_TIPO_OPERACION)

        [Set](fbx_NaveBuqueOtros, CamposViajes.CP_NAVE_BUQUE, propiedadDelControl_:=PropiedadesControl.Valor, asignarA_:=TiposAsignacion.Valor)
        [Set](fbx_NaveBuqueOtros, CamposViajes.CP_NAVE_BUQUE, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)

        [Set](fbx_NavieraAereolineaOtros, CamposViajes.CP_NAVIERA_AEREOLINEA, propiedadDelControl_:=PropiedadesControl.Valor, asignarA_:=TiposAsignacion.Valor)
        [Set](fbx_NavieraAereolineaOtros, CamposViajes.CP_NAVIERA_AEREOLINEA, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)

        [Set](fbx_ReexpedidoraForwarding, CamposViajes.CP_REEXPEDIDORA_FORWARDING, propiedadDelControl_:=PropiedadesControl.Valor, asignarA_:=TiposAsignacion.Valor)
        [Set](fbx_ReexpedidoraForwarding, CamposViajes.CP_REEXPEDIDORA_FORWARDING, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)

        [Set](fbx_PuertoExtrangero, CamposViajes.CP_PUERTO_EXTRANGERO, propiedadDelControl_:=PropiedadesControl.Valor, asignarA_:=TiposAsignacion.Valor)
        [Set](fbx_PuertoExtrangero, CamposViajes.CP_PUERTO_EXTRANGERO, propiedadDelControl_:=PropiedadesControl.Text, asignarA_:=TiposAsignacion.ValorPresentacion)

        '[Set](fbx_NavieraAereolineaOtros, CamposViajes.CP_NAVIERA_AEREOLINEA)
        '[Set](fbx_ReexpedidoraForwarding, CamposViajes.CP_REEXPEDIDORA_FORWARDING)
        [Set](txt_FolioCapitania, CamposViajes.CP_FOLIO_CAPITANIA)
        [Set](txt_NumeroViaje, CamposViajes.CP_NUMERO_VIAJE)
        '[Set](fbx_PuertoExtrangero, CamposViajes.CP_PUERTO_EXTRANGERO)
        [Set](txt_FechaSalidaOrigen, CamposViajes.CP_FECHA_SALIDA_ORIGEN)
        [Set](txt_FechaETA, CamposViajes.CP_FECHA_ETA)
        [Set](txt_FechaETD, CamposViajes.CP_FECHA_ETD)
        [Set](txt_FechaFondeo, CamposViajes.CP_FECHA_FONDEO)
        [Set](txt_FechaAtraque, CamposViajes.CP_FECHA_ATRAQUE)
        [Set](txt_FechaCierreDocumentos, CamposViajes.CP_FECHA_CIERRE_DOCUMENTO)
        [Set](txt_FechaPresentacion, CamposViajes.CP_FECHA_PRESENTACION)
        [Set](lbx_Refencias, CamposReferencia.CP_REFERENCIA, seccion_:=SeccionesViajes.SVIA4)

        Return New TagWatcher(1)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        sl_TipoTransporte.DataSource = Nothing

        sl_TipoTransporte.Value = 1

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        'ProcesarOperacion(Of Something)()
        If Not ProcesarTransaccion(Of ConstructorViajes)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()



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

        Return tagwatcher_

    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

        With documentoElectronico_

            .FolioDocumento = 0

            .FolioOperacion = txt_FolioCapitania.Value

            .IdCliente = 0

            .NombreCliente = fbx_NavieraAereolineaOtros.Value

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

        PrepararSecciones()

    End Sub

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
    '    ██    Pendientes ( al 07/05/2022 )                                                                ██
    '    ██      1. Mejorar la carga de los dropdowns ( hace las consultas en cada postback)               ██
    '    ██      2. Completar el caso de uso de inserción cuando se reutilice una empresa y domicilio      ██
    '    ██      3. Completar la carga de datos del CRUD ( el resto de las secciones )                     ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private Sub PrepararSecciones()

        If sw_Operacion.Checked = True Then

            txt_FechaETA.Visible = False

            txt_FechaETD.Visible = True

            txt_FechaFondeo.Visible = False

            txt_FechaAtraque.Visible = False

            txt_FechaCierreDocumentos.Visible = True

            txt_FechaPresentacion.Visible = True

            fbx_PuertoExtrangero.Visible = False

        Else

            txt_FechaETA.Visible = True

            txt_FechaETD.Visible = False

            txt_FechaFondeo.Visible = True

            txt_FechaAtraque.Visible = True

            txt_FechaCierreDocumentos.Visible = False

            txt_FechaPresentacion.Visible = False

            fbx_PuertoExtrangero.Visible = True

        End If

    End Sub

    Protected Sub sw_Operacion_CheckedChanged(sender As Object, e As EventArgs)

        PrepararSecciones()

    End Sub

    Protected Sub lbx_Refencias_TextChanged(sender As Object, e As EventArgs)

        Dim controlBusqueda_ = New ControladorBusqueda(Of ConstructorReferencia)

        Dim references_ = controlBusqueda_.Buscar("", New Filtro With {.IdSeccion = SeccionesReferencias.SREF1, .IdCampo = CamposReferencia.CP_REFERENCIA}, campoDetalle_:=ControladorBusqueda(Of ConstructorReferencia).CampoDetalles.Propietario)

        lbx_Refencias.DataSource = references_

    End Sub

    Protected Sub fbx_NaveBuqueOtros_TextChanged(sender As Object, e As EventArgs)

        fbx_NaveBuqueOtros.DataSource = New List(Of SelectOption) From {
            New SelectOption With {.Value = "1", .Text = "LEO PERDANA"},
            New SelectOption With {.Value = "2", .Text = "CMA CGM J.ADAMS"},
            New SelectOption With {.Value = "3", .Text = "CMA CGM PELLEAS"},
            New SelectOption With {.Value = "4", .Text = "OCEAN ADORE"},
            New SelectOption With {.Value = "5", .Text = "EVER LEGACY"}
        }

    End Sub

    Protected Sub fbx_NavieraAereolineaOtros_TextChanged(sender As Object, e As EventArgs)

        fbx_NavieraAereolineaOtros.DataSource = New List(Of SelectOption) From {
            New SelectOption With {.Value = "1", .Text = "CMA - CGM"},
            New SelectOption With {.Value = "2", .Text = "COSCO SHIPPING CO LTD"},
            New SelectOption With {.Value = "3", .Text = "HAPAG - LLOVYD MÉXICO S.A. DE C.V."}
        }

    End Sub

    Protected Sub fbx_ReexpedidoraForwarding_TextChanged(sender As Object, e As EventArgs)

        fbx_ReexpedidoraForwarding.DataSource = New List(Of SelectOption) From {
            New SelectOption With {.Value = "1", .Text = "MEXSHIPPING AGENCIA S.A. DE C.V."},
            New SelectOption With {.Value = "2", .Text = "TRANSMARINE NAVIGATION"},
            New SelectOption With {.Value = "3", .Text = "NAOS MARITIMA S.A. DE C.V."},
            New SelectOption With {.Value = "4", .Text = "MERITUS DE MÉXICO S.A. DE C.V."}
        }

    End Sub

    Protected Sub fbx_PuertoExtrangero_TextChanged(sender As Object, e As EventArgs)

        fbx_PuertoExtrangero.DataSource = New List(Of SelectOption) From {
            New SelectOption With {.Value = "1", .Text = "HO CHI MINH CITY"},
            New SelectOption With {.Value = "2", .Text = "HONG KONG"},
            New SelectOption With {.Value = "3", .Text = "SIHANOUKVILLE"}
        }

    End Sub

#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorEmpresas                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


#End Region



End Class