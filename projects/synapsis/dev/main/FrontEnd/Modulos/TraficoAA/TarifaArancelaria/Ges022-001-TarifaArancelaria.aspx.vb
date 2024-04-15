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
Imports Syn.Nucleo.RecursosComercioExterior.CamposTarifaArancelaria
Imports System.Xml.Serialization
Imports gsol
Imports System.IO

#End Region

Public Class Ges022_001_TarifaArancelaria
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

    Public Property Tratados As List(Of List(Of TratadoItem))

    'Public Property Cupos As List(Of CupoItem)
    Public Property IEPS As List(Of IepsItem)
    Public Property Cuotas As List(Of CuotaItem)
    Public Property Precios As List(Of PrecioItem)
    Public Property Permisos As List(Of PermisoItem)
    Public Property Normas As List(Of NormaItem)
    'Public Property Anexos As List(Of AnexoItem)
    'Public Property Embargos As List(Of EmbargoItem)
    'Public Property CuposMinimos As List(Of CupoMinimoItem)
    Public Property Padrones As List(Of PadronItem)

#End Region


#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████



    Public Overrides Sub Inicializa()

        With Buscador

            .DataObject = New ConstructorTIGIE(True)

            .addFilter(SeccionesTarifaArancelaria.TIGIE1, CA_NUMERO_FRACCION_ARANCELARIA, "Fraccion")
            .addFilter(SeccionesTarifaArancelaria.TIGIE1, CA_FRACCION_ARANCELARIA, "Descripción")

        End With

        If Not Page.IsPostBack Then

            Session("IEPS") = Nothing
            Session("Cuotas") = Nothing
            Session("Precios") = Nothing
            Session("Permisos") = Nothing
            Session("Normas") = Nothing
            Session("Padrones") = Nothing
            Session("catTLCAN") = Nothing
            Session("catTLCUE") = Nothing

            '_fsCupo.Visible = False
            _fsImpuestoEspecial.Visible = False
            _fsCuotasCompensatorias.Visible = False
            _fsPreciosEstimados.Visible = False
            _fsPermisos.Visible = False
            _fsNormas.Visible = False
            '_fsAnexos.Visible = False
            '_fsEmbargos.Visible = False
            '_fsCuposMinimos.Visible = False
            _fsPadronSctorial.Visible = False

            cat_tlcan.DataSource = Nothing

            cat_tlcue.DataSource = Nothing

        Else

            IEPS = IIf(Session("IEPS") IsNot Nothing, Session("IEPS"), New List(Of IepsItem))
            Cuotas = IIf(Session("Cuotas") IsNot Nothing, Session("Cuotas"), New List(Of CuotaItem))
            Precios = IIf(Session("Precios") IsNot Nothing, Session("Precios"), New List(Of PrecioItem))
            Permisos = IIf(Session("Permisos") IsNot Nothing, Session("Permisos"), New List(Of PermisoItem))
            Normas = IIf(Session("Normas") IsNot Nothing, Session("Normas"), New List(Of NormaItem))
            Padrones = IIf(Session("Padrones") IsNot Nothing, Session("Padrones"), New List(Of PadronItem))

            cat_tlcan.DataSource = Session("catTLCAN")
            cat_tlcue.DataSource = Session("catTLCUE")

            _fsImpuestoEspecial.Visible = IIf(IEPS.Count = 0, False, True)
            _fsCuotasCompensatorias.Visible = IIf(Cuotas.Count = 0, False, True)
            _fsPreciosEstimados.Visible = IIf(Precios.Count = 0, False, True)
            _fsPermisos.Visible = IIf(Permisos.Count = 0, False, True)
            _fsNormas.Visible = IIf(Normas.Count = 0, False, True)
            _fsPadronSctorial.Visible = IIf(Padrones.Count = 0, False, True)

        End If


    End Sub


    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        [Set](db_FraccionArancelaria, CA_NUMERO_FRACCION_ARANCELARIA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](db_FraccionArancelaria, CA_NUMERO_NICO, propiedadDelControl_:=PropiedadesControl.ValueDetail)

        [Set](sw_TipoMaterialPeligroso, CA_MATERIAL_PELIGROSO, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](sw_TipoMaterialVulnerable, CA_MATERIAL_VULNERABLE, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](sw_TipoMaterialSensible, CA_MATERIAL_SENSIBLE, propiedadDelControl_:=PropiedadesControl.Checked)

        [Set](txt_Fraccion, CA_FRACCION_ARANCELARIA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](txt_Nico, CA_NICO, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](txt_Publicacion, CA_FECHA_PUBLICACION, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](txt_EntradaVigor, CA_FECHA_ENTRADA_VIGOR, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](t_Seccion, CA_SECCION, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](t_Capitulo, CA_CAPITULO, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](t_Partida, CA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](t_Subpartida, CA_SUBPARTIDA, propiedadDelControl_:=PropiedadesControl.Valor)

        [Set](txt_UnidadMedida, CA_UNIDAD_MEDIDA, propiedadDelControl_:=PropiedadesControl.Valor)

        Return New TagWatcher(1)

    End Function


    Public Overrides Sub BotoneraClicNuevo()

        'ClonarTarifaSysExpert()

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        'ProcesarOperacion(Of Something)()
        'If Not ProcesarTransaccion(Of ConstructorTIGIE)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

    End Sub

    Public Overrides Sub BotoneraClicBorrar()

    End Sub

    Public Overrides Sub BotoneraClicOtros(IndexSelected_ As Integer)


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

            .FolioOperacion = ""

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

        sw_TipoOperacion.Checked = True

        FillForm(documentoElectronico_)

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralConDatos()


    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

        Session("IEPS") = Nothing
        Session("Cuotas") = Nothing
        Session("Precios") = Nothing
        Session("Permisos") = Nothing
        Session("Normas") = Nothing
        Session("Padrones") = Nothing
        Session("catTLCAN") = Nothing
        Session("catTLCUE") = Nothing

        '_fsCupo.Visible = False
        _fsImpuestoEspecial.Visible = False
        _fsCuotasCompensatorias.Visible = False
        _fsPreciosEstimados.Visible = False
        _fsPermisos.Visible = False
        _fsNormas.Visible = False
        '_fsAnexos.Visible = False
        '_fsEmbargos.Visible = False
        '_fsCuposMinimos.Visible = False
        _fsPadronSctorial.Visible = False

        cat_tlcan.DataSource = Nothing

        cat_tlcue.DataSource = Nothing

    End Sub


    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()


    End Sub

    Public Overrides Sub Limpiar()

        txt_ValorIVA.Value = Nothing
        txt_ValorImpuestoGeneral.Value = Nothing

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

    Public Sub FillForm(ByRef documentoElectronico_ As DocumentoElectronico)

        Session("IEPS") = Nothing
        Session("Cuotas") = Nothing
        Session("Precios") = Nothing
        Session("Permisos") = Nothing
        Session("Normas") = Nothing
        Session("Padrones") = Nothing
        Session("catTLCAN") = Nothing
        Session("catTLCUE") = Nothing

        Dim seccionTarifaArancelaria_ As SeccionesTarifaArancelaria = IIf(sw_TipoOperacion.Checked = True, SeccionesTarifaArancelaria.TIGIE2, SeccionesTarifaArancelaria.TIGIE3)

        'Encabezado
        txt_Fraccion.Value = documentoElectronico_.Seccion(SeccionesTarifaArancelaria.TIGIE1).Attribute(CA_FRACCION_ARANCELARIA).Valor
        txt_Nico.Value = documentoElectronico_.Seccion(SeccionesTarifaArancelaria.TIGIE1).Attribute(CA_NICO).Valor
        db_FraccionArancelaria.Value = documentoElectronico_.Seccion(SeccionesTarifaArancelaria.TIGIE1).Attribute(CA_NUMERO_FRACCION_ARANCELARIA).Valor
        db_FraccionArancelaria.ValueDetail = documentoElectronico_.Seccion(SeccionesTarifaArancelaria.TIGIE1).Attribute(CA_NUMERO_NICO).Valor
        txt_Publicacion.Value = documentoElectronico_.Seccion(SeccionesTarifaArancelaria.TIGIE1).Attribute(CA_FECHA_PUBLICACION).Valor
        txt_EntradaVigor.Value = documentoElectronico_.Seccion(SeccionesTarifaArancelaria.TIGIE1).Attribute(CA_FECHA_ENTRADA_VIGOR).Valor
        'documentoElectronico_.Seccion(SeccionesTarifaArancelaria.TIGIE1).Attribute(CA_FECHA_FIN).Valor

        'txt_FranjaRegionFonteriza
        'Impuestos
        Dim impuestos = documentoElectronico_.Seccion(SeccionesTarifaArancelaria.TIGIE19)

        For indice_ As Int32 = 1 To impuestos.CantidadPartidas

            With impuestos.Partida(indice_)

                If .Attribute(CA_NOMBRE_IMPUESTO).Valor = "IMPUESTO GENERAL DE IMPORTACION/EXPORTACION." Then

                    txt_ValorImpuestoGeneral.Value = .Attribute(CA_VALOR_IMPUESTO).Valor

                ElseIf .Attribute(CA_NOMBRE_IMPUESTO).Valor = "IMPUESTO AL VALOR AGREGADO." Then

                    txt_ValorIVA.Value = .Attribute(CA_VALOR_IMPUESTO).Valor

                End If

            End With

        Next

        'Unidad de medida
        txt_UnidadMedida.Value = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Attribute(CA_UNIDAD_MEDIDA).Valor

        'Tratados
        Dim tratados = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE4).Seccion(SeccionesTarifaArancelaria.TIGIE6)

        Dim catTLCAN As New List(Of Dictionary(Of String, String))
        Dim catTLCUE As New List(Of Dictionary(Of String, String))

        For indice_ As Int32 = 1 To tratados.CantidadPartidas

            With tratados.Partida(indice_)

                Dim tratado = .Attribute(CA_NOMBRE_CORTO_TRATADO).Valor

                Dim paises = .Seccion(SeccionesTarifaArancelaria.TIGIE7)

                For indice2_ As Int32 = 1 To paises.CantidadPartidas

                    With paises.Partida(indice2_)

                        If tratado = "TLCAN" Then

                            catTLCAN.Add(New Dictionary(Of String, String) From {
                               {"indice", indice2_},
                               {"c1_txt_Paises", .Attribute(CamposTarifaArancelaria.CA_PAIS).Valor},
                               {"c1_txt_Sector", ""},
                               {"c1_txt_Arancel", .Attribute(CA_ARANCEL).Valor},
                               {"c1_txt_Observacion", .Attribute(CA_OBSERVACION).Valor},
                               {"c1_txt_Publicacion", .Attribute(CA_FECHA_PUBLICACION).Valor},
                               {"c1_txt_EntradaVigor", .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor}
                           })
                            '{"c1_txt_Preferencia", ""},

                        End If

                        If tratado = "TLCUE" Then

                            catTLCUE.Add(New Dictionary(Of String, String) From {
                               {"indice", indice2_},
                               {"c2_txt_Paises", .Attribute(CamposTarifaArancelaria.CA_PAIS).Valor},
                               {"c2_txt_Sector", ""},
                               {"c2_txt_Arancel", .Attribute(CA_ARANCEL).Valor},
                               {"c2_txt_Observacion", .Attribute(CA_OBSERVACION).Valor},
                               {"c2_txt_Publicacion", .Attribute(CA_FECHA_PUBLICACION).Valor},
                               {"c2_txt_EntradaVigor", .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor}
                           })
                            '{"c2_txt_Preferencia", ""},
                        End If

                    End With

                Next

            End With

        Next

        Session("catTLCAN") = catTLCAN

        Session("catTLCUE") = catTLCUE

        cat_tlcan.DataSource = Session("catTLCAN")

        cat_tlcue.DataSource = Session("catTLCUE")

        'Cupos
        'IEPS
        Dim dataIpes_ As New List(Of IepsItem)
        Dim ieps_ = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE4).Seccion(SeccionesTarifaArancelaria.TIGIE9)

        For indice_ As Int32 = 1 To ieps_.CantidadPartidas

            With ieps_.Partida(indice_)

                Dim iepsitem_ = New IepsItem

                iepsitem_.Nota = .Attribute(CA_OBSERVACION).Valor

                dataIpes_.Add(iepsitem_)

            End With

        Next

        Session("IEPS") = dataIpes_

        'Cuotas
        Dim dataCuotas_ As New List(Of CuotaItem)
        Dim cuotas_ = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE4).Seccion(SeccionesTarifaArancelaria.TIGIE10)

        For indice_ As Int32 = 1 To cuotas_.CantidadPartidas

            With cuotas_.Partida(indice_)

                Dim cuota_ = New CuotaItem

                cuota_.NombreEmpresa = .Attribute(CA_EMPRESA).Valor
                cuota_.NombrePais = .Attribute(CamposTarifaArancelaria.CA_PAIS).Valor
                cuota_.Tasa = .Attribute(CA_CUOTA).Valor
                cuota_.TipoCuota = .Attribute(CA_TIPO).Valor
                cuota_.Nota = .Attribute(CA_ACOTACION).Valor
                cuota_.FechaPublicacion = .Attribute(CA_FECHA_PUBLICACION).Valor
                cuota_.FechaInicioVigencia = .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor
                cuota_.FechaFinVigencia = .Attribute(CA_FECHA_FIN).Valor

                dataCuotas_.Add(cuota_)

            End With

        Next

        Session("Cuotas") = dataCuotas_

        'Precios Estimados
        Dim dataPrecios_ As New List(Of PrecioItem)
        Dim precios_ = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE4).Seccion(SeccionesTarifaArancelaria.TIGIE11)

        For indice_ As Int32 = 1 To precios_.CantidadPartidas

            With precios_.Partida(indice_)

                Dim precio_ = New PrecioItem

                precio_.PrecioEstimado = .Attribute(CA_PRECIO).Valor
                precio_.DescripcionUM = .Attribute(CA_UNIDAD_MEDIDA).Valor
                precio_.DetalleProducto = .Attribute(CA_DESCRIPCION).Valor
                precio_.FechaPublicacion = .Attribute(CA_FECHA_PUBLICACION).Valor
                precio_.FechaInicioVigencia = .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor
                precio_.FechaFinVigencia = .Attribute(CA_FECHA_FIN).Valor

                dataPrecios_.Add(precio_)

            End With

        Next

        Session("Precios") = dataPrecios_

        'Permisos
        Dim dataPermisos_ As New List(Of PermisoItem)
        Dim permisos_ = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE5).Seccion(SeccionesTarifaArancelaria.TIGIE13)

        For indice_ As Int32 = 1 To permisos_.CantidadPartidas

            With permisos_.Partida(indice_)

                Dim permiso_ = New PermisoItem

                permiso_.ClavePermiso = .Attribute(CA_CLAVE).Valor
                permiso_.Descripcion = .Attribute(CA_PERMISO).Valor
                permiso_.Particularidad = .Attribute(CA_ACOTACION).Valor
                permiso_.FechaPublicacion = .Attribute(CA_FECHA_PUBLICACION).Valor
                permiso_.FechaInicioVigencia = .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor
                permiso_.FechaFinVigencia = .Attribute(CA_FECHA_FIN).Valor

                dataPermisos_.Add(permiso_)

            End With

        Next

        Session("Permisos") = dataPermisos_

        'Normas
        Dim dataNorms_ As New List(Of NormaItem)
        Dim normas_ = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE5).Seccion(SeccionesTarifaArancelaria.TIGIE14)

        For indice_ As Int32 = 1 To normas_.CantidadPartidas

            With normas_.Partida(indice_)

                Dim norma_ = New NormaItem

                norma_.NOM = .Attribute(CA_NORMA).Valor
                norma_.Descripcion = .Attribute(CA_DESCRIPCION).Valor
                norma_.FechaPublicacion = .Attribute(CA_FECHA_PUBLICACION).Valor
                norma_.FechaInicioVigencia = .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor
                norma_.FechaFinVigencia = .Attribute(CA_FECHA_FIN).Valor

                dataNorms_.Add(norma_)

            End With

        Next

        Session("Normas") = dataNorms_

        'Anexos
        'Embargos
        'Cupos Mínimos
        'Padrones
        Dim dataPadrones_ As New List(Of PadronItem)
        Dim padrones_ = documentoElectronico_.Seccion(seccionTarifaArancelaria_).Seccion(SeccionesTarifaArancelaria.TIGIE5).Seccion(SeccionesTarifaArancelaria.TIGIE18)

        For indice_ As Int32 = 1 To padrones_.CantidadPartidas

            With padrones_.Partida(indice_)

                Dim padron_ = New PadronItem

                padron_.Sector = .Attribute(CA_SECTOR).Valor
                padron_.Notas = .Attribute(CA_DESCRIPCION).Valor

                dataPadrones_.Add(padron_)

            End With

        Next

        Session("Padrones") = dataPadrones_


        IEPS = IIf(Session("IEPS") IsNot Nothing, Session("IEPS"), New List(Of IepsItem))
        Cuotas = IIf(Session("Cuotas") IsNot Nothing, Session("Cuotas"), New List(Of CuotaItem))
        Precios = IIf(Session("Precios") IsNot Nothing, Session("Precios"), New List(Of PrecioItem))
        Permisos = IIf(Session("Permisos") IsNot Nothing, Session("Permisos"), New List(Of PermisoItem))
        Normas = IIf(Session("Normas") IsNot Nothing, Session("Normas"), New List(Of NormaItem))
        Padrones = IIf(Session("Padrones") IsNot Nothing, Session("Padrones"), New List(Of PadronItem))

        _fsImpuestoEspecial.Visible = IIf(IEPS.Count = 0, False, True)
        _fsCuotasCompensatorias.Visible = IIf(Cuotas.Count = 0, False, True)
        _fsPreciosEstimados.Visible = IIf(Precios.Count = 0, False, True)
        _fsPermisos.Visible = IIf(Permisos.Count = 0, False, True)
        _fsNormas.Visible = IIf(Normas.Count = 0, False, True)
        _fsPadronSctorial.Visible = IIf(Padrones.Count = 0, False, True)

    End Sub

    Private Sub ClonarTarifaSysExpert()

        'exportare la lista de las fracciones para ejecutar un loop de esas una a una e ir cargado los datos desde mi db local, hay que cargar una copia que me dara rosa de sysexpert
        'VTIGIE_Nico_Fraccion_Relacion  de aqui jalare las id de fraccion para ejecutar el query uno a uno

        Dim fracciones As DataTable = ConsultaLibre("select top(10) * from [SysExpert].[dbo].[VTIGIE_Nico_Fraccion_Relacion]")

        For Each row As DataRow In fracciones.Rows

            Dim query = My.Computer.FileSystem.ReadAllText("C:\temp\test.txt")

            query = query.Replace("{{idFraccion}}", row.Item("idFraccion"))
            'query = query.Replace("{{idFraccion}}", "10789")

            Dim data = XMLDecode(Of List(Of NicoItem))("TIGIE", queryString:=query)

            Dim nico As NicoItem = data(0)

            Dim doc_ As DocumentoElectronico = New ConstructorTIGIE()

            With doc_

                .FolioDocumento = nico.Importacion.Fraccion

                .FolioOperacion = nico.Nico

                .IdCliente = 0

                .NombreCliente = ""

                'Encabezado
                With .Seccion(SeccionesTarifaArancelaria.TIGIE1)
                    .Attribute(CA_NUMERO_FRACCION_ARANCELARIA).Valor = nico.Importacion.Fraccion
                    .Attribute(CA_NUMERO_NICO).Valor = nico.Nico
                    .Attribute(CA_FRACCION_ARANCELARIA).Valor = nico.Importacion.DescripcionFraccion
                    .Attribute(CA_NICO).Valor = nico.DescripcionNico
                    .Attribute(CA_FECHA_PUBLICACION).Valor = nico.Importacion.FechaPublicacion
                    .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor = nico.Importacion.FechaInicioVigencia
                    .Attribute(CA_FECHA_FIN).Valor = nico.Importacion.FechaFinVigencia
                End With
                'Importacion
                With .Seccion(SeccionesTarifaArancelaria.TIGIE2)
                    'Unidad de Medida [tiene datos extras]
                    If nico.Importacion.UnidadesDeMedida IsNot Nothing Then
                        .Attribute(CA_UNIDAD_MEDIDA).Valor = nico.Importacion.UnidadesDeMedida.Value.Unidad
                    End If
                    'Impuestos
                    With .Seccion(SeccionesTarifaArancelaria.TIGIE19)

                        Dim impuestos = nico.Importacion.Impuestos

                        If impuestos.Count > 0 Then

                            For Each impuesto As ImpuestoItem In impuestos

                                With .Partida(doc_)
                                    .Attribute(CA_NOMBRE_IMPUESTO).Valor = impuesto.Contribucion
                                    .Attribute(CA_VALOR_IMPUESTO).Valor = impuesto.Tasa
                                    .Attribute(CA_FECHA_PUBLICACION).Valor = impuesto.FechaPublicacion
                                    .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor = impuesto.FechaInicioVigencia
                                    .Attribute(CA_FECHA_FIN).Valor = impuesto.FechaFinVigencia
                                End With

                            Next

                        End If

                    End With
                    'Regulaciones Arancelarias
                    With .Seccion(SeccionesTarifaArancelaria.TIGIE4)

                        'Tratados
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE6)

                            Dim tratados = nico.Importacion.Tratados

                            If tratados.Count > 0 Then

                                Dim listTratados As New Dictionary(Of String, List(Of TratadoItem))

                                For Each tratado As TratadoItem In tratados

                                    If listTratados.ContainsKey(tratado.ClaveTratado) Then

                                        listTratados(tratado.ClaveTratado).Add(tratado)

                                    Else

                                        listTratados.Add(tratado.ClaveTratado, New List(Of TratadoItem) From {tratado})

                                    End If

                                Next

                                For Each kvp As KeyValuePair(Of String, List(Of TratadoItem)) In listTratados

                                    With .Partida(doc_)

                                        .Attribute(CA_NOMBRE_CORTO_TRATADO).Valor = kvp.Key

                                        With .Seccion(SeccionesTarifaArancelaria.TIGIE7)

                                            For Each t As TratadoItem In kvp.Value

                                                With .Partida(doc_)

                                                    .Attribute(CamposTarifaArancelaria.CA_PAIS).Valor = t.NombrePais
                                                    .Attribute(CA_ARANCEL).Valor = t.Tasa
                                                    .Attribute(CA_PREFERENCIA).Valor = t.Tasa
                                                    .Attribute(CA_OBSERVACION).Valor = t.Nota
                                                    .Attribute(CA_FECHA_PUBLICACION).Valor = t.FechaPublicacion
                                                    .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor = t.FechaIncioVigencia
                                                    .Attribute(CA_FECHA_FIN).Valor = t.FechaFinVigencia

                                                End With

                                            Next

                                        End With

                                    End With

                                Next

                            End If

                        End With

                        'Cupos Arancel [sin tablas]
                        'IEPS
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE9)

                            Dim ipes = nico.Importacion.IEPS

                            If ipes.Count > 0 Then

                                For Each iepsItem As IepsItem In ipes

                                    With .Partida(doc_)
                                        .Attribute(CA_OBSERVACION).Valor = iepsItem.Nota
                                    End With

                                Next

                            End If

                        End With
                        'Cuotas Compensatorias
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE10)

                            Dim cuotas = nico.Importacion.CuotasCompensatorias

                            If cuotas.Count > 0 Then

                                For Each cuota As CuotaItem In cuotas

                                    With .Partida(doc_)
                                        .Attribute(CA_EMPRESA).Valor = cuota.NombreEmpresa
                                        .Attribute(CamposTarifaArancelaria.CA_PAIS).Valor = cuota.NombrePais
                                        .Attribute(CA_CUOTA).Valor = cuota.Tasa
                                        .Attribute(CA_TIPO).Valor = cuota.TipoCuota
                                        .Attribute(CA_ACOTACION).Valor = cuota.Nota
                                        .Attribute(CA_FECHA_PUBLICACION).Valor = cuota.FechaPublicacion
                                        .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor = cuota.FechaInicioVigencia
                                        .Attribute(CA_FECHA_FIN).Valor = cuota.FechaFinVigencia
                                    End With

                                Next

                            End If

                        End With
                        'Precios Estimados
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE11)

                            Dim precios = nico.Importacion.PreciosEstimados

                            If precios.Count > 0 Then

                                For Each precio As PrecioItem In precios

                                    With .Partida(doc_)
                                        .Attribute(CA_PRECIO).Valor = precio.PrecioEstimado
                                        .Attribute(CA_UNIDAD_MEDIDA).Valor = precio.DescripcionUM
                                        .Attribute(CA_DESCRIPCION).Valor = precio.DetalleProducto
                                        .Attribute(CA_FECHA_PUBLICACION).Valor = precio.FechaPublicacion
                                        .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor = precio.FechaInicioVigencia
                                        .Attribute(CA_FECHA_FIN).Valor = precio.FechaFinVigencia
                                    End With

                                Next

                            End If

                        End With
                    End With
                    'Regulaciones no Arancelarias
                    With .Seccion(SeccionesTarifaArancelaria.TIGIE5)
                        'Permisos
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE13)

                            Dim permisos = nico.Importacion.Permisos

                            If permisos.Count > 0 Then

                                For Each permiso As PermisoItem In permisos

                                    With .Partida(doc_)
                                        .Attribute(CA_CLAVE).Valor = permiso.ClavePermiso
                                        .Attribute(CA_PERMISO).Valor = permiso.Descripcion
                                        .Attribute(CA_ACOTACION).Valor = permiso.Particularidad
                                        .Attribute(CA_FECHA_PUBLICACION).Valor = permiso.FechaPublicacion
                                        .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor = permiso.FechaInicioVigencia
                                        .Attribute(CA_FECHA_FIN).Valor = permiso.FechaFinVigencia
                                    End With

                                Next

                            End If

                        End With
                        'Normas
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE14)

                            Dim normas = nico.Importacion.Normas

                            If normas.Count > 0 Then

                                For Each norma As NormaItem In normas

                                    With .Partida(doc_)
                                        .Attribute(CA_NORMA).Valor = norma.NOM
                                        .Attribute(CA_DESCRIPCION).Valor = norma.Descripcion
                                        .Attribute(CA_FECHA_PUBLICACION).Valor = norma.FechaPublicacion
                                        .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor = norma.FechaInicioVigencia
                                        .Attribute(CA_FECHA_FIN).Valor = norma.FechaFinVigencia
                                    End With

                                Next

                            End If

                        End With
                        'Anexos [todo esta metido en un campo y una sola tabla]
                        'Embargos [todo esta metido en un campo y una sola tabla]
                        'Cupos Mínimos [sin tablas]
                        'Padron Sectoreal
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE18)

                            Dim padrones = nico.Importacion.PadronSectorial

                            If padrones.Count > 0 Then

                                For Each padron As PadronItem In padrones

                                    With .Partida(doc_)
                                        .Attribute(CA_SECTOR).Valor = padron.Sector
                                        .Attribute(CA_DESCRIPCION).Valor = padron.Notas
                                    End With

                                Next

                            End If

                        End With

                    End With
                End With
                '----------------------------------------------------------------------------------------------------------------------------------------------------------------
                'Exportacion
                With .Seccion(SeccionesTarifaArancelaria.TIGIE3)

                    'Unidad de Medida [tiene datos extras]
                    If nico.Exportacion.UnidadesDeMedida IsNot Nothing Then
                        .Attribute(CA_UNIDAD_MEDIDA).Valor = nico.Exportacion.UnidadesDeMedida.Value.Unidad
                    End If
                    'Impuestos
                    With .Seccion(SeccionesTarifaArancelaria.TIGIE19)

                        Dim impuestos = nico.Exportacion.Impuestos

                        If impuestos.Count > 0 Then

                            For Each impuesto As ImpuestoItem In impuestos

                                With .Partida(doc_)
                                    .Attribute(CA_NOMBRE_IMPUESTO).Valor = impuesto.Contribucion
                                    .Attribute(CA_VALOR_IMPUESTO).Valor = impuesto.Tasa
                                    .Attribute(CA_FECHA_PUBLICACION).Valor = impuesto.FechaPublicacion
                                    .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor = impuesto.FechaInicioVigencia
                                    .Attribute(CA_FECHA_FIN).Valor = impuesto.FechaFinVigencia
                                End With

                            Next

                        End If

                    End With
                    'Regulaciones Arancelarias
                    With .Seccion(SeccionesTarifaArancelaria.TIGIE4)

                        'Tratados
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE6)

                            Dim tratados = nico.Exportacion.Tratados

                            If tratados.Count > 0 Then

                                Dim listTratados As New Dictionary(Of String, List(Of TratadoItem))

                                For Each tratado As TratadoItem In tratados

                                    If listTratados.ContainsKey(tratado.ClaveTratado) Then

                                        listTratados(tratado.ClaveTratado).Add(tratado)

                                    Else

                                        listTratados.Add(tratado.ClaveTratado, New List(Of TratadoItem) From {tratado})

                                    End If

                                Next

                                For Each kvp As KeyValuePair(Of String, List(Of TratadoItem)) In listTratados

                                    With .Partida(doc_)

                                        .Attribute(CA_NOMBRE_CORTO_TRATADO).Valor = kvp.Key

                                        With .Seccion(SeccionesTarifaArancelaria.TIGIE7)

                                            For Each t As TratadoItem In kvp.Value

                                                With .Partida(doc_)

                                                    .Attribute(CamposTarifaArancelaria.CA_PAIS).Valor = t.NombrePais
                                                    .Attribute(CA_ARANCEL).Valor = t.Tasa
                                                    .Attribute(CA_PREFERENCIA).Valor = t.Tasa
                                                    .Attribute(CA_OBSERVACION).Valor = t.Nota
                                                    .Attribute(CA_FECHA_PUBLICACION).Valor = t.FechaPublicacion
                                                    .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor = t.FechaIncioVigencia
                                                    .Attribute(CA_FECHA_FIN).Valor = t.FechaFinVigencia

                                                End With

                                            Next

                                        End With

                                    End With

                                Next

                            End If

                        End With

                        'Cupos Arancel [sin tablas]
                        'IEPS
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE9)

                            Dim ipes = nico.Exportacion.IEPS

                            If ipes.Count > 0 Then

                                For Each iepsItem As IepsItem In ipes

                                    With .Partida(doc_)
                                        .Attribute(CA_OBSERVACION).Valor = iepsItem.Nota
                                    End With

                                Next

                            End If

                        End With
                        'Cuotas Compensatorias
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE10)

                            Dim cuotas = nico.Exportacion.CuotasCompensatorias

                            If cuotas.Count > 0 Then

                                For Each cuota As CuotaItem In cuotas

                                    With .Partida(doc_)
                                        .Attribute(CA_EMPRESA).Valor = cuota.NombreEmpresa
                                        .Attribute(CamposTarifaArancelaria.CA_PAIS).Valor = cuota.NombrePais
                                        .Attribute(CA_CUOTA).Valor = cuota.Tasa
                                        .Attribute(CA_TIPO).Valor = cuota.TipoCuota
                                        .Attribute(CA_ACOTACION).Valor = cuota.Nota
                                        .Attribute(CA_FECHA_PUBLICACION).Valor = cuota.FechaPublicacion
                                        .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor = cuota.FechaInicioVigencia
                                        .Attribute(CA_FECHA_FIN).Valor = cuota.FechaFinVigencia
                                    End With

                                Next

                            End If

                        End With
                        'Precios Estimados
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE11)

                            Dim precios = nico.Exportacion.PreciosEstimados

                            If precios.Count > 0 Then

                                For Each precio As PrecioItem In precios

                                    With .Partida(doc_)
                                        .Attribute(CA_PRECIO).Valor = precio.PrecioEstimado
                                        .Attribute(CA_UNIDAD_MEDIDA).Valor = precio.DescripcionUM
                                        .Attribute(CA_DESCRIPCION).Valor = precio.DetalleProducto
                                        .Attribute(CA_FECHA_PUBLICACION).Valor = precio.FechaPublicacion
                                        .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor = precio.FechaInicioVigencia
                                        .Attribute(CA_FECHA_FIN).Valor = precio.FechaFinVigencia
                                    End With

                                Next

                            End If

                        End With
                    End With
                    'Regulaciones no Arancelarias
                    With .Seccion(SeccionesTarifaArancelaria.TIGIE5)
                        'Permisos
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE13)

                            Dim permisos = nico.Exportacion.Permisos

                            If permisos.Count > 0 Then

                                For Each permiso As PermisoItem In permisos

                                    With .Partida(doc_)
                                        .Attribute(CA_CLAVE).Valor = permiso.ClavePermiso
                                        .Attribute(CA_PERMISO).Valor = permiso.Descripcion
                                        .Attribute(CA_ACOTACION).Valor = permiso.Particularidad
                                        .Attribute(CA_FECHA_PUBLICACION).Valor = permiso.FechaPublicacion
                                        .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor = permiso.FechaInicioVigencia
                                        .Attribute(CA_FECHA_FIN).Valor = permiso.FechaFinVigencia
                                    End With

                                Next

                            End If

                        End With
                        'Normas
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE14)

                            Dim normas = nico.Exportacion.Normas

                            If normas.Count > 0 Then

                                For Each norma As NormaItem In normas

                                    With .Partida(doc_)
                                        .Attribute(CA_NORMA).Valor = norma.NOM
                                        .Attribute(CA_DESCRIPCION).Valor = norma.Descripcion
                                        .Attribute(CA_FECHA_PUBLICACION).Valor = norma.FechaPublicacion
                                        .Attribute(CA_FECHA_ENTRADA_VIGOR).Valor = norma.FechaInicioVigencia
                                        .Attribute(CA_FECHA_FIN).Valor = norma.FechaFinVigencia
                                    End With

                                Next

                            End If

                        End With
                        'Anexos [todo esta metido en un campo y una sola tabla]
                        'Embargos [todo esta metido en un campo y una sola tabla]
                        'Cupos Mínimos [sin tablas]
                        'Padron Sectoreal
                        With .Seccion(SeccionesTarifaArancelaria.TIGIE18)

                            Dim padrones = nico.Exportacion.PadronSectorial

                            If padrones.Count > 0 Then

                                For Each padron As PadronItem In padrones

                                    With .Partida(doc_)
                                        .Attribute(CA_SECTOR).Valor = padron.Sector
                                        .Attribute(CA_DESCRIPCION).Valor = padron.Notas
                                    End With

                                Next

                            End If

                        End With

                    End With

                End With

            End With

            CloneDocumentoInMongo(doc_)

        Next row


    End Sub

    Private Async Sub CloneDocumentoInMongo(object1_ As Object)

        Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        Using session_ = Await iEnlace_.GetMongoClient().StartSessionAsync().ConfigureAwait(False)

            Using enlaceDatos_ As IEnlaceDatos =
                New EnlaceDatos With {.EspacioTrabajo = Session("EspacioTrabajoExtranet")}

                Using _entidadDatos As IEntidadDatos = object1_

                    Dim tagWatcher_ As TagWatcher = enlaceDatos_.AgregarDatos(_entidadDatos, Nothing, session_)

                    If tagWatcher_.Status = Ok Then



                    Else


                    End If

                End Using

            End Using

        End Using


    End Sub

    Protected Sub sw_TipoOperacion_CheckedChanged(sender As Object, e As EventArgs)

        If OperacionGenerica IsNot Nothing Then

            FillForm(OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente)

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

<Serializable(), XmlRoot("NicoItem")>
Public Structure NicoItem
    <XmlElement("Nico")>
    Public Property Nico As String
    <XmlElement("DescripcionNico")>
    Public Property DescripcionNico As String
    <XmlElement("FechaPublicacion")>
    Public Property FechaPublicacion As String
    <XmlElement("FechaInicioVigencia")>
    Public Property FechaInicioVigencia As String
    <XmlElement("FechaFinVigencia")>
    Public Property FechaFinVigencia As String
    <XmlElement("Importacion")>
    Public Property Importacion As Importacion
    <XmlElement("Exportacion")>
    Public Property Exportacion As Exportacion
End Structure

<Serializable(), XmlRoot("Importacion")>
Public Structure Importacion
    <XmlElement("idFraccion")>
    Public Property idFraccion As String
    <XmlElement("Fraccion")>
    Public Property Fraccion As String
    <XmlElement("DescripcionFraccion")>
    Public Property DescripcionFraccion As String
    <XmlElement("ImpExp")>
    Public Property ImpExp As String
    <XmlElement("FechaPublicacion")>
    Public Property FechaPublicacion As String
    <XmlElement("FechaInicioVigencia")>
    Public Property FechaInicioVigencia As String
    <XmlElement("FechaFinVigencia")>
    Public Property FechaFinVigencia As String
    <XmlElement("UnidadesDeMedida", IsNullable:=True)>
    Public Property UnidadesDeMedida As UnidadesDeMedida?
    <XmlArray("Impuestos", IsNullable:=True)>
    Public Property Impuestos As List(Of ImpuestoItem?)
    <XmlArray("Tratados", IsNullable:=True)>
    Public Property Tratados As List(Of TratadoItem?)
    <XmlArray("Normas", IsNullable:=True)>
    Public Property Normas As List(Of NormaItem?)
    <XmlArray("Permisos", IsNullable:=True)>
    Public Property Permisos As List(Of PermisoItem?)
    <XmlArray("CuotasCompensatorias", IsNullable:=True)>
    Public Property CuotasCompensatorias As List(Of CuotaItem?)
    <XmlArray("PreciosEstimados", IsNullable:=True)>
    Public Property PreciosEstimados As List(Of PrecioItem?)
    <XmlArray("Restricciones", IsNullable:=True)>
    Public Property Restricciones As List(Of RestriccionItem?)
    <XmlArray("IEPS", IsNullable:=True)>
    Public Property IEPS As List(Of IepsItem?)
    <XmlArray("PadronSectorial", IsNullable:=True)>
    Public Property PadronSectorial As List(Of PadronItem?)
End Structure

<Serializable(), XmlRoot("Exportacion")>
Public Structure Exportacion
    <XmlElement("idFraccion")>
    Public Property idFraccion As String
    <XmlElement("Fraccion")>
    Public Property Fraccion As String
    <XmlElement("DescripcionFraccion")>
    Public Property DescripcionFraccion As String
    <XmlElement("ImpExp")>
    Public Property ImpExp As String
    <XmlElement("FechaPublicacion")>
    Public Property FechaPublicacion As String
    <XmlElement("FechaInicioVigencia")>
    Public Property FechaInicioVigencia As String
    <XmlElement("FechaFinVigencia")>
    Public Property FechaFinVigencia As String
    <XmlElement("UnidadesDeMedida", IsNullable:=True)>
    Public Property UnidadesDeMedida As UnidadesDeMedida?
    <XmlArray("Impuestos", IsNullable:=True)>
    Public Property Impuestos As List(Of ImpuestoItem?)
    <XmlArray("Tratados", IsNullable:=True)>
    Public Property Tratados As List(Of TratadoItem?)
    <XmlArray("Normas", IsNullable:=True)>
    Public Property Normas As List(Of NormaItem?)
    <XmlArray("Permisos", IsNullable:=True)>
    Public Property Permisos As List(Of PermisoItem?)
    <XmlArray("CuotasCompensatorias", IsNullable:=True)>
    Public Property CuotasCompensatorias As List(Of CuotaItem?)
    <XmlArray("PreciosEstimados", IsNullable:=True)>
    Public Property PreciosEstimados As List(Of PrecioItem?)
    <XmlArray("Restricciones", IsNullable:=True)>
    Public Property Restricciones As List(Of RestriccionItem?)
    <XmlArray("IEPS", IsNullable:=True)>
    Public Property IEPS As List(Of IepsItem?)
    <XmlArray("PadronSectorial", IsNullable:=True)>
    Public Property PadronSectorial As List(Of PadronItem?)
End Structure

<Serializable(), XmlRoot("UnidadesDeMedida")>
Public Structure UnidadesDeMedida

    <XmlElement("idUnidad")>
    Public Property idUnidad As String
    <XmlElement("ClaveUnidad")>
    Public Property ClaveUnidad As String
    <XmlElement("ClaveCOVE")>
    Public Property ClaveCOVE As String
    <XmlElement("Unidad")>
    Public Property Unidad As String
    <XmlElement("FechaPublicacion")>
    Public Property FechaPublicacion As String
    <XmlElement("FechaInicioVigencia")>
    Public Property FechaInicioVigencia As String
    <XmlElement("FechaFinVigencia")>
    Public Property FechaFinVigencia As String

End Structure

<Serializable(), XmlRoot("ImpuestoItem")>
Public Structure ImpuestoItem
    <XmlElement("idTipoTasa")>
    Public Property idTipoTasa As String
    <XmlElement("Tasa")>
    Public Property Tasa As String
    <XmlElement("DescripcionTasa")>
    Public Property DescripcionTasa As String
    <XmlElement("idTipoTarifa")>
    Public Property idTipoTarifa As String
    <XmlElement("Tarifa")>
    Public Property Tarifa As String
    <XmlElement("idContribucion")>
    Public Property idContribucion As String
    <XmlElement("Contribucion")>
    Public Property Contribucion As String
    <XmlElement("ClaveContribucion")>
    Public Property ClaveContribucion As String
    <XmlElement("FechaPublicacion")>
    Public Property FechaPublicacion As String
    <XmlElement("FechaInicioVigencia")>
    Public Property FechaInicioVigencia As String
    <XmlElement("FechaFinVigencia")>
    Public Property FechaFinVigencia As String
End Structure

<Serializable(), XmlRoot("TratadoItem")>
Public Structure TratadoItem
    <XmlElement("idPais")>
    Public Property idPais As String
    <XmlElement("NombrePais")>
    Public Property NombrePais As String
    <XmlElement("ClaveTratado")>
    Public Property ClaveTratado As String
    <XmlElement("idTipoTasa")>
    Public Property idTipoTasa As String
    <XmlElement("Tasa")>
    Public Property Tasa As String
    <XmlElement("DescripcionTasa")>
    Public Property DescripcionTasa As String
    <XmlElement("idIdentificador")>
    Public Property idIdentificador As String
    <XmlElement("ClaveIdentificador")>
    Public Property ClaveIdentificador As String
    <XmlElement("Identificador")>
    Public Property Identificador As String
    <XmlElement("idNota")>
    Public Property idNota As String
    <XmlElement("Nota")>
    Public Property Nota As String
    <XmlElement("FechaPublicacion")>
    Public Property FechaPublicacion As String
    <XmlElement("FechaIncioVigencia")>
    Public Property FechaIncioVigencia As String
    <XmlElement("FechaFinVigencia")>
    Public Property FechaFinVigencia As String
End Structure

<Serializable(), XmlRoot("NormaItem")>
Public Structure NormaItem
    <XmlElement("NOM")>
    Public Property NOM As String
    <XmlElement("Descripcion")>
    Public Property Descripcion As String
    <XmlElement("idAcuerdo")>
    Public Property idAcuerdo As String
    <XmlElement("Fraccion")>
    Public Property Fraccion As String
    <XmlElement("Acuerdo")>
    Public Property Acuerdo As String
    <XmlElement("idArticulo")>
    Public Property idArticulo As String
    <XmlElement("NumeroArticulo")>
    Public Property NumeroArticulo As String
    <XmlElement("Articulo")>
    Public Property Articulo As String
    <XmlElement("FechaPublicacion")>
    Public Property FechaPublicacion As String
    <XmlElement("FechaInicioVigencia")>
    Public Property FechaInicioVigencia As String
    <XmlElement("FechaFinVigencia")>
    Public Property FechaFinVigencia As String
End Structure

<Serializable(), XmlRoot("PermisoItem")>
Public Structure PermisoItem
    <XmlElement("idPermiso")>
    Public Property idPermiso As String
    <XmlElement("ClavePermiso")>
    Public Property ClavePermiso As String
    <XmlElement("Permiso")>
    Public Property Permiso As String
    <XmlElement("idIncisoPermiso")>
    Public Property idIncisoPermiso As String
    <XmlElement("Inciso")>
    Public Property Inciso As String
    <XmlElement("Descripcion")>
    Public Property Descripcion As String
    <XmlElement("Particularidad")>
    Public Property Particularidad As String
    <XmlElement("FechaPublicacion")>
    Public Property FechaPublicacion As String
    <XmlElement("FechaInicioVigencia")>
    Public Property FechaInicioVigencia As String
    <XmlElement("FechaFinVigencia")>
    Public Property FechaFinVigencia As String
End Structure

<Serializable(), XmlRoot("CuotaItem")>
Public Structure CuotaItem
    <XmlElement("Producto")>
    Public Property Producto As String
    <XmlElement("idPais")>
    Public Property idPais As String
    <XmlElement("NombrePais")>
    Public Property NombrePais As String
    <XmlElement("idClaveEmpresa")>
    Public Property idClaveEmpresa As String
    <XmlElement("NombreEmpresa")>
    Public Property NombreEmpresa As String
    <XmlElement("Tasa")>
    Public Property Tasa As String
    <XmlElement("TipoCuota")>
    Public Property TipoCuota As String
    <XmlElement("idUnidad")>
    Public Property idUnidad As String
    <XmlElement("ClaveUnidad")>
    Public Property ClaveUnidad As String
    <XmlElement("ClaveCOVE")>
    Public Property ClaveCOVE As String
    <XmlElement("Unidad")>
    Public Property Unidad As String
    <XmlElement("idCCNota")>
    Public Property idCCNota As String
    <XmlElement("Nota")>
    Public Property Nota As String
    <XmlElement("Regimen")>
    Public Property Regimen As String
    <XmlElement("TasaReferencia")>
    Public Property TasaReferencia As String
    <XmlElement("FechaPublicacion")>
    Public Property FechaPublicacion As String
    <XmlElement("FechaInicioVigencia")>
    Public Property FechaInicioVigencia As String
    <XmlElement("FechaFinVigencia")>
    Public Property FechaFinVigencia As String
End Structure

<Serializable(), XmlRoot("PrecioItem")>
Public Structure PrecioItem
    <XmlElement("Producto")>
    Public Property Producto As String
    <XmlElement("DetalleProducto")>
    Public Property DetalleProducto As String
    <XmlElement("idUnidadMedidaPE")>
    Public Property idUnidadMedidaPE As String
    <XmlElement("DescripcionUM")>
    Public Property DescripcionUM As String
    <XmlElement("PrecioEstimado")>
    Public Property PrecioEstimado As String
    <XmlElement("FechaPublicacion")>
    Public Property FechaPublicacion As String
    <XmlElement("FechaInicioVigencia")>
    Public Property FechaInicioVigencia As String
    <XmlElement("FechaFinVigencia")>
    Public Property FechaFinVigencia As String
End Structure

<Serializable(), XmlRoot("RestriccionItem")>
Public Structure RestriccionItem
    <XmlElement("idRestriccion")>
    Public Property idRestriccion As String
    <XmlElement("ClaveRestriccion")>
    Public Property ClaveRestriccion As String
    <XmlElement("Restriccion")>
    Public Property Restriccion As String
    <XmlElement("FechaPublicacion")>
    Public Property FechaPublicacion As String
    <XmlElement("FechaInicioVigencia")>
    Public Property FechaInicioVigencia As String
    <XmlElement("FechaFinVigencia")>
    Public Property FechaFinVigencia As String
End Structure

<Serializable(), XmlRoot("IepsItem")>
Public Structure IepsItem
    <XmlElement("idNotaIEPS")>
    Public Property idNotaIEPS As String
    <XmlElement("idFraccion")>
    Public Property idFraccion As String
    <XmlElement("Nota")>
    Public Property Nota As String
End Structure

<Serializable(), XmlRoot("PadronItem")>
Public Structure PadronItem
    <XmlElement("idTIGIEAnexo10")>
    Public Property idTIGIEAnexo10 As String
    <XmlElement("idFraccion")>
    Public Property idFraccion As String
    <XmlElement("Sector")>
    Public Property Sector As String
    <XmlElement("Notas")>
    Public Property Notas As String
End Structure