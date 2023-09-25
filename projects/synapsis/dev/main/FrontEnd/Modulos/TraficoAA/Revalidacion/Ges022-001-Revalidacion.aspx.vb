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

Public Class Ges022_001_Revalidacion
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

            .DataObject = New ConstructorRevalidacion(True)

            .addFilter(SeccionesRevalidacion.SREV1, CamposReferencia.CP_REFERENCIA, "Referencia")
            .addFilter(SeccionesRevalidacion.SREV1, CamposClientes.CA_RAZON_SOCIAL, "Cliente")
            .addFilter(SeccionesRevalidacion.SREV2, CamposRevalidacion.CP_NO_GUIA_MASTER, "No. Guia")

        End With

    End Sub

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        [Set](fbx_Referencia, CamposReferencia.CP_REFERENCIA)
        [Set](fbx_Referencia, CamposReferencia.CP_REFERENCIA, propiedadDelControl_:=PropiedadesControl.Text)
        [Set](txt_Cliente, CamposClientes.CA_RAZON_SOCIAL)
        [Set](txt_GuiaMaster, CamposRevalidacion.CP_NO_GUIA_MASTER)
        [Set](sw_Revalidado, CamposRevalidacion.CP_REVALIDADO)
        [Set](txt_FechaRevalidacion, CamposRevalidacion.CP_FECHA_REVALIDACION)
        [Set](sl_TipoCarga, CamposRevalidacion.CP_TIPO_CARGA)
        '[Set](fl_BLRevalidado, CamposRevalidacion.CP_ID_BLREVALIDADO)

        [Set](sltest, CamposRevalidacion.CP_CLASE_CARGA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](txt_CantidadCarga, CamposRevalidacion.CP_CANTIDAD_CARGA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](txt_PesoCarga, CamposRevalidacion.CP_PESO_CARGA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](cat_CargaSuelta, Nothing, seccion_:=SeccionesRevalidacion.SREV3)

        [Set](txt_NumeroContenedor, CamposRevalidacion.CP_CONTENEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](sl_TamanoContenedor, CamposRevalidacion.CP_TAMANO_CONTENEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](txt_PesoContenedor, CamposRevalidacion.CP_PESO_CONTENEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](cat_Contenedores, Nothing, seccion_:=SeccionesRevalidacion.SREV4)

        Return New TagWatcher(1)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        sl_TipoCarga.DataSource = Nothing

        sl_TipoCarga.Value = 1

        cat_CargaSuelta.Visible = True

        cat_Contenedores.Visible = False

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        'ProcesarOperacion(Of Something)()
        If Not ProcesarTransaccion(Of ConstructorRevalidacion)().Status = TypeStatus.Errors Then : End If

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

            .FolioOperacion = txt_GuiaMaster.Value

            .IdCliente = 0

            .NombreCliente = ""

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

        sl_TipoCarga.Value = Nothing

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

        If sl_TipoCarga.Value = "1" Then

            cat_CargaSuelta.Visible = True

            cat_Contenedores.Visible = False

        ElseIf sl_TipoCarga.Value = "2" Then

            cat_CargaSuelta.Visible = False

            cat_Contenedores.Visible = True

        End If

        txt_FechaRevalidacion.Visible = sw_Revalidado.Checked

    End Sub

    Protected Sub sl_TipoCarga_SelectedIndexChanged(sender As Object, e As EventArgs)

        PrepararSecciones()

    End Sub

    Protected Sub sw_Revalidado_CheckedChanged(sender As Object, e As EventArgs)

        PrepararSecciones()

    End Sub

    Protected Sub fbx_Referencia_TextChanged(sender As Object, e As EventArgs)

        Dim controlBusqueda_ = New ControladorBusqueda(Of ConstructorReferencia)

        Dim references_ = controlBusqueda_.Buscar("", New Filtro With {.IdSeccion = SeccionesReferencias.SREF1, .IdCampo = CamposReferencia.CP_REFERENCIA})

        fbx_Referencia.DataSource = references_


    End Sub

    Protected Sub fbx_Referencia_Click(sender As Object, e As EventArgs)

        Dim controlBusqueda_ = New ControladorBusqueda(Of ConstructorReferencia)

        Dim tagwacher_ = controlBusqueda_.ObtenerDocumento(fbx_Referencia.Value)

        If tagwacher_.Status = TypeStatus.Ok Then

            Dim documento_ As DocumentoElectronico = tagwacher_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            txt_Cliente.Value = documento_.Attribute(CamposReferencia.CA_RAZON_SOCIAL_IOE).Valor

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


#End Region


End Class