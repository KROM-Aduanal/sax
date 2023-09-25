Imports gsol.krom
Imports gsol
Imports gsol.BaseDatos.Operaciones
Imports System.IO
Imports System.Web
Imports Wma.Exceptions
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports Wma.Exceptions.TagWatcher
Imports gsol.krom.ReglaAccesoKrom
Imports Wma.Reports
Imports System.Text
Imports NPOI.HSSF.UserModel

Namespace gsol.krom.controladores

    Public Class ControladorWeb

#Region "Enums"

        'JC - Agregar
        Enum FiltrosDashBoard
            ClavesCliente
            RazonesSocialesCliente
        End Enum

        Enum TipoSeccion
            Operativo
            Administrativo
        End Enum

#End Region

#Region "Attributes"

        Private _iOperations As IOperacionesCatalogo

        Public Shared _sesion As ISesion

        Private _sessionStatus As Boolean

        Private _reglasAccesoKrom As List(Of ReglaAccesoKrom)

        Private _clausulasSQLReglasAcceso As String

        Private _enlaceDatos As IEnlaceDatos

        Public _sistema As New Organismo

        Private _resultados As List(Of IEntidadDatos)

        Private _sesionWebIniciada As Boolean = False

        Private _tipoReporte As IReportw.Extensions

        Private _listaAtributos As List(Of CampoVirtual)

        Private _espacioTrabajo As EspacioTrabajo

        Private _datosBusquedaPrincipal As Dictionary(Of String, String)

        'JC - Agregar
        Private _componentesOperacionesUsuario As Dictionary(Of FiltrosDashBoard, Object)

#End Region

#Region "Properties"

        Public Property TipoReporte As IReportw.Extensions

            Get
                Return _tipoReporte

            End Get

            Set(ByVal value As IReportw.Extensions)

                _tipoReporte = value

            End Set

        End Property

        Public Property FiltrosAvanzados As String

            Get

                Return _enlaceDatos.FiltrosAvanzados

            End Get

            Set(value As String)

                _enlaceDatos.FiltrosAvanzados = value

            End Set

        End Property

        Public ReadOnly Property Estatus As TagWatcher

            Get

                Return _enlaceDatos.MensajeTagWatcher

            End Get

        End Property

        Public ReadOnly Property SesionUsuario As ISesion

            Get

                Return _sesion

            End Get

        End Property

        Public ReadOnly Property SesionWebIniciada As Boolean

            Get

                Return _sesionWebIniciada

            End Get

        End Property

        Public ReadOnly Property ReglasAccesoKrom As List(Of ReglaAccesoKrom)

            Get

                Return _reglasAccesoKrom

            End Get

        End Property

        Public ReadOnly Property ClausulasSQLReglasAcceso As String

            Get

                Return _clausulasSQLReglasAcceso

            End Get

        End Property

        Public Property EnlaceDatos As IEnlaceDatos

            Get

                Return _enlaceDatos

            End Get

            Set(value As IEnlaceDatos)

                _enlaceDatos = value

            End Set

        End Property

        Public Property ListaAtributos As List(Of CampoVirtual)

            Get

                Return _listaAtributos

            End Get

            Set(value As List(Of CampoVirtual))

                _listaAtributos = value

            End Set

        End Property

        Public ReadOnly Property ComponentesOperacionesUsuario As Dictionary(Of FiltrosDashBoard, Object)

            Get

                Return _componentesOperacionesUsuario

            End Get

        End Property

#End Region

#Region "Builders"

        'Aqui llega ya con un espacio de trabajo válido, es decir ya inicio sesión.

        Sub New(ByVal espacioTrabajo_ As IEspacioTrabajo)

            _enlaceDatos = New EnlaceDatos

            _tipoReporte = IReportw.Extensions.und

            If Not espacioTrabajo_ Is Nothing And Not espacioTrabajo_.MisCredenciales Is Nothing Then

                _resultados = New List(Of IEntidadDatos)

                With _enlaceDatos

                    .EspacioTrabajo = espacioTrabajo_

                    .LimiteResultados = 1000

                    .Granularidad = IEnlaceDatos.TiposDimension.SinDefinir

                    .ModalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo

                    .TipoRespuestaRequerida = IEnlaceDatos.FormatosRespuesta.IOperaciones

                    .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                    .ClausulasLibres = Nothing

                    .FiltrosAvanzados = Nothing

                End With

                _sesion = New SesionWcf

                _iOperations = New OperacionesCatalogo

                _reglasAccesoKrom = New List(Of ReglaAccesoKrom)

                If Not _sesionWebIniciada Then

                    If Not espacioTrabajo_ Is Nothing Then

                        Dim mensajeTagWatcher_ As New TagWatcher

                        _componentesOperacionesUsuario = New Dictionary(Of FiltrosDashBoard, Object)

                        'Cuando llega aquí ya no se verifican credenciales de nuevo, solo se toman las reglas, MOP 17102019

                        LogIn(espacioTrabajo_.MisCredenciales.CredencialUsuario,
                                                   "brouniews@brounie.com",
                                                   "AB24rsdAQ54", 4, 1, False)

                        _enlaceDatos.ClausulasLibres = _clausulasSQLReglasAcceso

                        _datosBusquedaPrincipal = New Dictionary(Of String, String)

                        _espacioTrabajo = espacioTrabajo_

                    Else

                        _clausulasSQLReglasAcceso = Nothing

                    End If

                End If

            Else

                _enlaceDatos.MensajeTagWatcher.SetError(ErrorTypes.WS007,
                                                          "4 This mobile user doesn't have client rules to access profile,  although was authenticated")

            End If

        End Sub

#End Region

#Region "Methods"

        Public Function ObtenerBusquedaPrincipal() As Dictionary(Of String, String)

            Dim iOperacionesCatalogo_ As IOperacionesCatalogo

            iOperacionesCatalogo_ = New OperacionesCatalogo

            iOperacionesCatalogo_.EspacioTrabajo = _espacioTrabajo

            With _datosBusquedaPrincipal

                If iOperacionesCatalogo_.EspacioTrabajo.BuscaPermiso(IRecursosSistemas.ClavePermisos.KBWBusquedaGeneralPO,
                                                                 IEspacioTrabajo.TipoModulo.Abstracto) Then

                    If Not .ContainsKey("id") Then
                        
                        .Add("label", "Buscar orden de compra")

                        .Add("id", "t_OrdenCompra")

                        .Add("placeholder", "Ejemplo: 1234567890")

                    End If

                Else

                    If Not .ContainsKey("id") Then

                        If Not _listaAtributos Is Nothing Then

                            For Each campo_ As CampoVirtual In _listaAtributos

                                If campo_.AtributoLLave Then

                                    .Add("label", "Buscar " & campo_.Descripcion)

                                    .Add("id", campo_.Atributo.ToString)

                                    .Add("placeholder", "")

                                End If

                            Next

                        Else

                            .Add("label", "Buscar pedimento")

                            .Add("id", "t_Pedimento")

                            .Add("placeholder", "Ejemplo: 20 43 3945 0000000")

                        End If

                    End If

                End If

            End With

            Return _datosBusquedaPrincipal

        End Function

        Public Function ExportarReporte(Optional ByVal i_TipoReporte_ As IReportw.Extensions = IReportw.Extensions.und) As Byte()

            Dim o_Reporteador_ As IReportw = Nothing

            Dim o_StringCSV_ As StringBuilder = Nothing

            Dim o_ObjectNPOI_ As HSSFWorkbook = Nothing

            Dim o_MemoryStream_ As New MemoryStream

            Dim data_ As New DataSet

            Dim o_Stream_ As Byte() = Nothing

            Dim ioperacionesCopia_ As IOperacionesCatalogo = New OperacionesCatalogo

            If i_TipoReporte_ = IReportw.Extensions.und Then

                If _sistema.TieneResultados(_enlaceDatos.IOperaciones) Then

                    If _enlaceDatos.IOperaciones.Vista.Tables(0).Rows.Count <= 1500 Then

                        i_TipoReporte_ = IReportw.Extensions.xls

                    Else
                        i_TipoReporte_ = IReportw.Extensions.csv

                    End If

                End If

            End If

            _tipoReporte = i_TipoReporte_

            If Not _enlaceDatos.IOperaciones Is Nothing Then

                ioperacionesCopia_ = _enlaceDatos.IOperaciones.Clone

                data_.Tables.Add(_enlaceDatos.Tabla.Copy)

                ioperacionesCopia_.SetVista = data_

                ioperacionesCopia_.EspacioTrabajo = _enlaceDatos.IOperaciones.EspacioTrabajo

                o_Reporteador_ = New DesktopReports

                With o_Reporteador_

                    .MakerMode = IReportw.MakerModes.ThroughPackage

                    .PackageIOperations = ioperacionesCopia_

                    .setReportTheme = ExcelApplicationReports.WorksheetThemes.OliveWorld

                    .FileExtension = i_TipoReporte_

                    .GenerateReport()

                    If Not .getReportObject Is Nothing Then

                        Select Case i_TipoReporte_

                            Case IReportw.Extensions.xls, IReportw.Extensions.xlsx

                                o_ObjectNPOI_ = .getReportObject

                                o_ObjectNPOI_.Write(o_MemoryStream_)

                                o_Stream_ = o_MemoryStream_.GetBuffer()

                            Case IReportw.Extensions.csv

                                o_StringCSV_ = .getReportObject

                                o_Stream_ = Encoding.Unicode.GetBytes(o_StringCSV_.ToString())

                        End Select

                    End If

                End With

            End If

            Return o_Stream_

        End Function

        Public Function ObtenerFuncionesJavaScript(ByVal rutaAPSXCorta_ As String,
                                           Optional ByVal botonCopiar_ As Boolean = True,
                                           Optional ByVal botonPDF_ As Boolean = True,
                                           Optional ByVal botonImprimir_ As Boolean = True,
                                           Optional ByVal botonDescargar_ As Boolean = True,
                                           Optional ByVal activarMensaje_ As Boolean = True,
                                           Optional ByVal mensaje_ As String = "aCookie") As StringBuilder

            Dim jsPrincipal_ = New System.Text.StringBuilder()

            Dim configuracionBotones_ As String = ""

            If Not botonCopiar_ And
               Not botonDescargar_ And
               Not botonImprimir_ And
               Not botonPDF_ Then

                'configuracionBotones_ = "rtip"
                configuracionBotones_ = "rti"

            Else

                'configuracionBotones_ = "Brtip"
                configuracionBotones_ = "Brti"

            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''Includes de JavaScript''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            jsPrincipal_.Append("    <script src=""/FrontEnd/Librerias/JQuery/jquery-3.3.1.js""></script>" & vbCrLf)

            jsPrincipal_.Append("    <script src=""/FrontEnd/Librerias/DataTables/js/jquery-dataTables.js""></script>" & vbCrLf)

            jsPrincipal_.Append("    <script src=""/FrontEnd/Librerias/DataTables/js/dataTables-buttons.js""></script>" & vbCrLf)

            jsPrincipal_.Append("    <script src=""/FrontEnd/Librerias/DataTables/js/buttons-print.js""></script>" & vbCrLf)

            jsPrincipal_.Append("    <script src=""/FrontEnd/Librerias/DataTables/js/buttons-flash.js""></script>" & vbCrLf)

            jsPrincipal_.Append("    <script src=""/FrontEnd/Librerias/DataTables/js/jszip.js""></script>" & vbCrLf)

            jsPrincipal_.Append("    <script src=""/FrontEnd/Librerias/DataTables/js/pdfmake.js""></script>" & vbCrLf)

            jsPrincipal_.Append("    <script src=""/FrontEnd/Librerias/DataTables/js/vfs-fonts.js""></script>" & vbCrLf)

            jsPrincipal_.Append("    <script src=""/FrontEnd/Librerias/DataTables/js/buttons-html5.js""></script>" & vbCrLf)

            jsPrincipal_.Append("    <script src=""/FrontEnd/Librerias/Krom/js/KromComponentes.js""></script>" & vbCrLf)

            jsPrincipal_.Append("    <script src=""/FrontEnd/Librerias/Krom/js/KROM-Plugins.js""></script>" & vbCrLf)

            'jsPrincipal_.Append("    <script src=""/FrontEnd/Librerias/Krom/js/KROM-Eventos.js""></script>" & vbCrLf)

            jsPrincipal_.Append("    <script src=""/FrontEnd/Librerias/JQuery/jquery.cookie.js""></script>" & vbCrLf)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''Includes de JavaScript''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            jsPrincipal_.Append("<script language='javascript'>" & vbCrLf)

            jsPrincipal_.Append("const _kromComponents = new KromComponentes();" & vbCrLf)

            jsPrincipal_.Append("(function () {" & vbCrLf)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''Crear Tabla'''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim jsTabla_ = New System.Text.StringBuilder()

            jsTabla_.Append("<thead><tr>")

            For Each campo_ As CampoVirtual In _listaAtributos

                jsTabla_.Append("<th>" & campo_.Descripcion & "</th>")

            Next

            jsTabla_.Append("</tr>)")

            jsTabla_.Append("<tbody id=""tbl_body_table"">")

            jsTabla_.Append("</tbody>")

            jsPrincipal_.Append("    $('#dashBoardFilter').attr('value', localStorage.getItem('filtroDashBoard')); localStorage.removeItem('filtroDashBoard');" & vbCrLf)

            jsPrincipal_.Append("    var dashBoardFilter_ = $('#dashBoardFilter').val();" & vbCrLf)

            jsPrincipal_.Append("    $('table').html('" & jsTabla_.ToString & "');" & vbCrLf)
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''Crear Tabla'''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''Render''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'jsPrincipal_.Append("    var clausulasLibres_ = '{clausulasLibres_: """" }';" & vbCrLf)
            'jsPrincipal_.Append("    var clausulasLibres_ = '{clausulasLibres_: """", limiteResultados_: ' + limiteResultados_ + '}';" & vbCrLf)

            jsPrincipal_.Append("    var clausulasLibres_ = (typeof dashBoardFilter_ != 'undefined' && dashBoardFilter_ != '') ? '{clausulasLibres_: ' + dashBoardFilter_ + ' }' : '{""clausulasLibres_"": """" }';" & vbCrLf)

            jsPrincipal_.Append("    $.kromDataTable({" & vbCrLf)

            jsPrincipal_.Append("        url: ""/FrontEnd/" & rutaAPSXCorta_ & "/ConsultarControladorWeb""" & vbCrLf)

            jsPrincipal_.Append("        , conditions: clausulasLibres_" & vbCrLf)

            jsPrincipal_.Append("        , label: ""#tbl_catalogo""" & vbCrLf)

            jsPrincipal_.Append("        , buttons: ['print', 'copy', 'csv', 'excel', 'pdf']" & vbCrLf)

            'jsPrincipal_.Append("       , elements: 'Brtip' " & vbCrLf)

            jsPrincipal_.Append("        , elements: '" & configuracionBotones_ & "' " & vbCrLf)

            If activarMensaje_ = True Then

                If mensaje_ = "aCookie" Then

                    jsPrincipal_.Append("        , message: {activate: true, message: 'aCookie'}" & vbCrLf)

                Else

                    jsPrincipal_.Append("        , message: {activate: true, message: '" & mensaje_ & "'}" & vbCrLf)

                End If

            End If

            jsPrincipal_.Append("        , render: {" & vbCrLf)

            Dim valorTarjet_ = 0

            For Each campo_ As CampoVirtual In _listaAtributos

                If valorTarjet_ = 0 Then

                    jsPrincipal_.Append("            0: {" & vbCrLf)

                Else

                    jsPrincipal_.Append("            ," & valorTarjet_ & ": {" & vbCrLf)

                End If

                jsPrincipal_.Append("                targets: " & valorTarjet_ & "" & vbCrLf)

                jsPrincipal_.Append("                , data: """ & campo_.Descripcion & """" & vbCrLf)

                jsPrincipal_.Append("                , render: function (data, type, row, meta) {" & vbCrLf)

                jsPrincipal_.Append("                    if (type === 'display')" & vbCrLf)

                jsPrincipal_.Append("                    {" & vbCrLf)

                jsPrincipal_.Append(EvaluarClaseCSS(campo_).ToString & vbCrLf)

                jsPrincipal_.Append("                    }" & vbCrLf)

                jsPrincipal_.Append("                    else {" & vbCrLf)

                jsPrincipal_.Append("                        return ''" & vbCrLf)

                jsPrincipal_.Append("                    }" & vbCrLf)

                jsPrincipal_.Append("                }" & vbCrLf)

                jsPrincipal_.Append("            }" & vbCrLf)

                valorTarjet_ = valorTarjet_ + 1

            Next

            jsPrincipal_.Append("        }" & vbCrLf) 'Final Render

            jsPrincipal_.Append("     }" & vbCrLf) 'Cerrar KromDataTable

            jsPrincipal_.Append("   );" & vbCrLf) 'Cerrar KromDataTable
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''Render''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''Filtros avanzados'''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            jsPrincipal_.Append("    $('#btnBuscar').click(function () {" & vbCrLf)

            jsPrincipal_.Append("    var filtrosactivos = $("".kb-btn-ba"").hasClass(""fa-plus"");" & vbCrLf)

            jsPrincipal_.Append("    if (filtrosactivos) {" & vbCrLf)

            Dim contadorCampoLlave As Int16 = 0

            For Each campo_ As CampoVirtual In _listaAtributos

                If campo_.AtributoLLave Then

                    If campo_.AtributoFiltro Is Nothing Then

                        ' JCCS: Se comento esta linea por que ahora tomara la busqueda principal del metodo "ObtenerBusquedaPrincipal" que esta en este mismo proyecto
                        'jsPrincipal_.Append("             var busqueda_ = ($(""#" & campo_.Atributo.ToString & """).val() != """") ? ""\"" and " & campo_.Atributo.ToString & " like '%"" + $(""#" & campo_.Atributo.ToString & """).val().trim() + ""%' \"""" : '""""';" & vbCrLf)

                        If contadorCampoLlave < 1 Then

                            If _datosBusquedaPrincipal.Count > 0 Then

                                jsPrincipal_.Append("             var busqueda_ = ($(""#" & _datosBusquedaPrincipal.Item("id").ToString & """).val() != """") ? ""\"" and " & _datosBusquedaPrincipal.Item("id").ToString & " like '%"" + $(""#" & _datosBusquedaPrincipal.Item("id").ToString & """).val().trim() + ""%' \"""" : '""""';" & vbCrLf)

                                contadorCampoLlave += 1

                            Else

                                jsPrincipal_.Append("             var busqueda_ = ($(""#" & campo_.Atributo.ToString & """).val() != """") ? ""\"" and " & campo_.Atributo.ToString & " like '%"" + $(""#" & campo_.Atributo.ToString & """).val().trim() + ""%' \"""" : '""""';" & vbCrLf)

                            End If

                        End If

                    Else

                        jsPrincipal_.Append("             var busqueda_ = ($(""#" & DirectCast(campo_.AtributoFiltro, CampoVirtual).Atributo.ToString() & """).val() != """") ? ""\"" and " & DirectCast(campo_.AtributoFiltro, CampoVirtual).Atributo.ToString() & " like '%"" + $(""#" & DirectCast(campo_.AtributoFiltro, CampoVirtual).Atributo.ToString() & """).val().trim() + ""%' \"""" : '""""';" & vbCrLf)

                    End If

                End If

            Next

            jsPrincipal_.Append("             var clausulasLibres_ = ""{clausulasLibres_: "" + busqueda_ + ""}"";" & vbCrLf)

            jsPrincipal_.Append("             $.kromDataTable({" & vbCrLf)

            jsPrincipal_.Append("                    url: ""/FrontEnd/" & rutaAPSXCorta_ & "/ConsultarControladorWeb""" & vbCrLf)

            jsPrincipal_.Append("                    , conditions: clausulasLibres_" & vbCrLf)

            jsPrincipal_.Append("                    , label: ""#tbl_catalogo""" & vbCrLf)

            jsPrincipal_.Append("                    , buttons: ['print', 'copy', 'csv', 'excel', 'pdf']" & vbCrLf)

            'jsPrincipal_.Append("                    , elements: 'Brtip' " & vbCrLf)

            jsPrincipal_.Append("                    , elements: '" & configuracionBotones_ & "' " & vbCrLf)

            If activarMensaje_ = True Then

                If mensaje_ = "aCookie" Then

                    jsPrincipal_.Append("        , message: {activate: true, message: 'aCookie'}" & vbCrLf)

                Else

                    jsPrincipal_.Append("        , message: {activate: true, message: '" & mensaje_ & "'}" & vbCrLf)

                End If

            End If

            jsPrincipal_.Append("                    , render: {" & vbCrLf)

            valorTarjet_ = 0

            For Each campo_ As CampoVirtual In _listaAtributos

                If valorTarjet_ = 0 Then

                    jsPrincipal_.Append("                           0: {" & vbCrLf)

                Else

                    jsPrincipal_.Append("                                  ," & valorTarjet_ & ": {" & vbCrLf)

                End If

                jsPrincipal_.Append("                                       targets: " & valorTarjet_ & "" & vbCrLf)

                jsPrincipal_.Append("                                     , data: """ & campo_.Descripcion & """" & vbCrLf)

                jsPrincipal_.Append("                                     , render: function (data, type, row, meta) {" & vbCrLf)

                jsPrincipal_.Append("                                     if (type === 'display')" & vbCrLf)

                jsPrincipal_.Append("                                           {" & vbCrLf)

                jsPrincipal_.Append(EvaluarClaseCSS(campo_).ToString & vbCrLf)

                jsPrincipal_.Append("                                           }" & vbCrLf)

                jsPrincipal_.Append("                                     else {" & vbCrLf)

                jsPrincipal_.Append("                                             return ''" & vbCrLf)

                jsPrincipal_.Append("                                          }" & vbCrLf)

                jsPrincipal_.Append("                                   }" & vbCrLf)

                jsPrincipal_.Append("                          }" & vbCrLf)

                valorTarjet_ = valorTarjet_ + 1

            Next
            jsPrincipal_.Append("                       }" & vbCrLf) 'final render

            jsPrincipal_.Append("                }" & vbCrLf) 'cerrar kromdatatable

            jsPrincipal_.Append("           );" & vbCrLf) 'cerrar kromdatatable

            jsPrincipal_.Append("   } else {" & vbCrLf)

            'Petición para los filtros externos
            jsPrincipal_.Append(EjecutarFiltroExterno(rutaAPSXCorta_))

            jsPrincipal_.Append(vbCrLf)

            'jsPrincipal_.Append("function EjecutarFiltrosAvanzados() {" & vbCrLf)

            jsPrincipal_.Append("     $.KromClausulasLibres({" & vbCrLf)

            valorTarjet_ = 0

            If _datosBusquedaPrincipal.Count > 0 Then

                'Esta lista de atributos modificada se crea en caso de que se tengan algún permiso con busqueda personalizada
                Dim listaAtributosFiltros_ As New List(Of CampoVirtual)

                Dim listaAtributosAdicionales_ As New CampoVirtual With {.EsFiltro = True,
                                                                        .Atributo = _datosBusquedaPrincipal.Item("id"),
                                                                        .TipoFiltro = 2,
                                                                        .TipoDatoHTML = IEntidadDatos.TiposDatosHTML.Texto}

                listaAtributosFiltros_.AddRange(_listaAtributos)

                listaAtributosFiltros_.Add(listaAtributosAdicionales_)

                For Each campo_ As CampoVirtual In listaAtributosFiltros_

                    If campo_.TipoFiltro = 2 Then

                        If valorTarjet_ = 0 Then

                            jsPrincipal_.Append("            0: {" & vbCrLf)

                        Else

                            jsPrincipal_.Append("            ," & valorTarjet_ & ": {" & vbCrLf)

                        End If

                        jsPrincipal_.Append("                    label: """ & campo_.Atributo.ToString & """" & vbCrLf)

                        If campo_.AtributoFiltro Is Nothing Then

                            jsPrincipal_.Append("                    , tipo: """ & _sistema.GetEnumDescription(DirectCast(Convert.ToInt32(campo_.TipoDatoHTML), IEntidadDatos.TiposDatosHTML)) & """" & vbCrLf)

                            jsPrincipal_.Append("                    , clausula: """ & campo_.Atributo.ToString & """" & vbCrLf)

                        Else

                            jsPrincipal_.Append("                    , tipo: """ & _sistema.GetEnumDescription(DirectCast(Convert.ToInt32(DirectCast(campo_.AtributoFiltro, CampoVirtual).TipoDatoHTML), IEntidadDatos.TiposDatosHTML)) & """" & vbCrLf)

                            jsPrincipal_.Append("                    , clausula: """ & DirectCast(campo_.AtributoFiltro, CampoVirtual).Atributo.ToString() & """" & vbCrLf)

                        End If

                        jsPrincipal_.Append("                }" & vbCrLf)

                        valorTarjet_ = valorTarjet_ + 1

                    End If

                Next

            Else

                For Each campo_ As CampoVirtual In _listaAtributos

                    If campo_.EsFiltro Then

                        If valorTarjet_ = 0 Then

                            jsPrincipal_.Append("            0: {" & vbCrLf)

                        Else

                            jsPrincipal_.Append("            ," & valorTarjet_ & ": {" & vbCrLf)

                        End If

                        jsPrincipal_.Append("                    label: """ & campo_.Atributo.ToString & """" & vbCrLf)

                        If campo_.AtributoFiltro Is Nothing Then

                            jsPrincipal_.Append("                    , tipo: """ & _sistema.GetEnumDescription(DirectCast(Convert.ToInt32(campo_.TipoDatoHTML), IEntidadDatos.TiposDatosHTML)) & """" & vbCrLf)

                            jsPrincipal_.Append("                    , clausula: """ & campo_.Atributo.ToString & """" & vbCrLf)

                        Else

                            jsPrincipal_.Append("                    , tipo: """ & _sistema.GetEnumDescription(DirectCast(Convert.ToInt32(DirectCast(campo_.AtributoFiltro, CampoVirtual).TipoDatoHTML), IEntidadDatos.TiposDatosHTML)) & """" & vbCrLf)

                            jsPrincipal_.Append("                    , clausula: """ & DirectCast(campo_.AtributoFiltro, CampoVirtual).Atributo.ToString() & """" & vbCrLf)

                        End If

                        jsPrincipal_.Append("                }" & vbCrLf)

                        valorTarjet_ = valorTarjet_ + 1

                    End If

                Next


            End If

            _datosBusquedaPrincipal = New Dictionary(Of String, String)

            jsPrincipal_.Append("            }, function (clausulasLibres_) {" & vbCrLf)

            jsPrincipal_.Append("    var clausulasLibres_ = clausulasLibres_.split(""}"")[0] + ""}""" & vbCrLf)

            'jsPrincipal_.Append("    console.log(clausulasLibres_);" & vbCrLf)

            jsPrincipal_.Append("    $.kromDataTable({" & vbCrLf)

            jsPrincipal_.Append("        url: ""/FrontEnd/" & rutaAPSXCorta_ & "/ConsultarControladorWeb""" & vbCrLf)

            jsPrincipal_.Append("        , conditions: clausulasLibres_" & vbCrLf)

            jsPrincipal_.Append("        , label: ""#tbl_catalogo""" & vbCrLf)

            jsPrincipal_.Append("        , buttons: ['print', 'copy', 'csv', 'excel', 'pdf']" & vbCrLf)

            'jsPrincipal_.Append("        , elements: 'Brtip' " & vbCrLf)

            jsPrincipal_.Append("        , elements: '" & configuracionBotones_ & "' " & vbCrLf)

            If activarMensaje_ = True Then

                If mensaje_ = "aCookie" Then

                    jsPrincipal_.Append("        , message: {activate: true, message: 'aCookie'}" & vbCrLf)

                Else

                    jsPrincipal_.Append("        , message: {activate: true, message: '" & mensaje_ & "'}" & vbCrLf)

                End If

            End If

            jsPrincipal_.Append("        , render: {" & vbCrLf)

            valorTarjet_ = 0

            For Each campo_ As CampoVirtual In _listaAtributos

                If valorTarjet_ = 0 Then

                    jsPrincipal_.Append("            0: {" & vbCrLf)

                Else

                    jsPrincipal_.Append("            ," & valorTarjet_ & ": {" & vbCrLf)

                End If

                jsPrincipal_.Append("                targets: " & valorTarjet_ & "" & vbCrLf)

                jsPrincipal_.Append("                , data: """ & campo_.Descripcion & """" & vbCrLf)

                jsPrincipal_.Append("                , render: function (data, type, row, meta) {" & vbCrLf)

                jsPrincipal_.Append("                    if (type === 'display')" & vbCrLf)

                jsPrincipal_.Append("                    {" & vbCrLf)

                jsPrincipal_.Append(EvaluarClaseCSS(campo_).ToString & vbCrLf)

                jsPrincipal_.Append("                    }" & vbCrLf)

                jsPrincipal_.Append("                    else {" & vbCrLf)

                jsPrincipal_.Append("                        return ''" & vbCrLf)

                jsPrincipal_.Append("                    }" & vbCrLf)

                jsPrincipal_.Append("                }" & vbCrLf)

                jsPrincipal_.Append("            }" & vbCrLf)

                valorTarjet_ = valorTarjet_ + 1

            Next

            jsPrincipal_.Append("                   }" & vbCrLf) 'final render

            jsPrincipal_.Append("               });" & vbCrLf) 'Cierrar kromdatatable

            jsPrincipal_.Append("            });" & vbCrLf) 'cerrar KromClausulasLibres

            'jsPrincipal_.Append("}" & vbCrLf)

            jsPrincipal_.Append("        }" & vbCrLf) ' Cierra if filtrosactivos

            jsPrincipal_.Append("    });" & vbCrLf) ' Cierra evento click
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''Filtros avanzados'''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''''''''Floating Buttons'''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If Not botonCopiar_ And
               Not botonDescargar_ And
               Not botonImprimir_ And
               Not botonPDF_ Then

            Else

                Dim jsFloatingButtons_ = New System.Text.StringBuilder()

                jsFloatingButtons_.Append("<div class='zoom' style='z-index: 999'>")

                jsFloatingButtons_.Append("   <a class='zoom-fab zoom-btn-large' id='zoomBtn'><i class='fa fa-download' style='font-size: 25px; line-height: 60px;'></i></a>")

                jsFloatingButtons_.Append("   <ul class='zoom-menu'>")

                If botonCopiar_ Then

                    jsFloatingButtons_.Append("        <li><a class='zoom-fab zoom-btn-sm zoom-btn-download scale-transition scale-out' id='zoom-btn-copy'><i class='fa fa-copy'></i></a></li>")

                End If

                If botonPDF_ Then

                    jsFloatingButtons_.Append("        <li><a class='zoom-fab zoom-btn-sm zoom-btn-download scale-transition scale-out' id='zoom-btn-pdf'><i class='fa fa-file-pdf-o'></i></a></li>")

                End If

                If botonImprimir_ Then

                    jsFloatingButtons_.Append("        <li><a class='zoom-fab zoom-btn-sm zoom-btn-download scale-transition scale-out' id='zoom-btn-print'><i class='fa fa-print'></i></a></li>")

                End If

                If botonDescargar_ Then

                    jsFloatingButtons_.Append("        <li><a class='zoom-fab zoom-btn-sm zoom-btn-download scale-transition scale-out' id='zoom-btn-download'><i class='fa fa-file-excel-o'></i></a></li>")

                End If

                jsFloatingButtons_.Append("   </ul>")

                jsFloatingButtons_.Append("</div>")

                jsPrincipal_.Append("    $(""" & jsFloatingButtons_.ToString & """).insertBefore('section.content');" & vbCrLf)

                jsPrincipal_.Append("    $('#zoom-btn-copy').click(function (event) {" & vbCrLf)

                jsPrincipal_.Append("        $('.buttons-copy').trigger(""click"");" & vbCrLf)

                jsPrincipal_.Append("    });" & vbCrLf)

                jsPrincipal_.Append("    $('#zoom-btn-download').click(function (event) {" & vbCrLf)

                jsPrincipal_.Append("        self.location = ""/FrontEnd/NameSpaces/DescargarDocumentos.aspx?mod=repcom"";" & vbCrLf)

                jsPrincipal_.Append("    });" & vbCrLf)

                jsPrincipal_.Append("    $('#zoom-btn-print').click(function (event) {" & vbCrLf)

                jsPrincipal_.Append("        $('.buttons-print').trigger(""click"");" & vbCrLf)

                jsPrincipal_.Append("    });" & vbCrLf)

                jsPrincipal_.Append("    $('#zoom-btn-pdf').click(function (event) {" & vbCrLf)

                jsPrincipal_.Append("        $('.buttons-pdf').trigger(""click"");" & vbCrLf)

                jsPrincipal_.Append("    });" & vbCrLf)

            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''''''''Floating Buttons'''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            jsPrincipal_.Append("})();" & vbCrLf) 'Cerrar función mágica

            jsPrincipal_.Append("</script>" & vbCrLf)

            Return jsPrincipal_

        End Function

        Private Function EjecutarFiltroExterno(ByVal vista_ As String)

            Dim jsFiltroExterno_ = New System.Text.StringBuilder()

            jsFiltroExterno_.Append("$('[filtro-externo]').each(function(index_, label_) {" & vbCrLf)

            jsFiltroExterno_.Append("   var valor_ = $(this).val(), granularidad_ = $(this).attr('granularidad'), localizacion_ = $(this).attr('localizacion'), busqueda_ = $(this).attr('busqueda');" & vbCrLf)

            jsFiltroExterno_.Append("   var data_ = JSON.stringify({data: {valor: valor_, granularidad: granularidad_, localizacion: localizacion_, busqueda: busqueda_}}) " & vbCrLf)

            jsFiltroExterno_.Append("   if(valor_ != '') {" & vbCrLf)

            jsFiltroExterno_.Append("	    $.ajax({ " & vbCrLf)
            jsFiltroExterno_.Append("	        type: 'POST' " & vbCrLf)
            jsFiltroExterno_.Append("	        , url: '../../../../../../../../../../CapaPresentacion/" & vista_ & "/FiltroExterno' " & vbCrLf)
            jsFiltroExterno_.Append("	        , data: data_ " & vbCrLf)
            jsFiltroExterno_.Append("	        , contentType: 'application/json; charset=utf-8' " & vbCrLf)
            jsFiltroExterno_.Append("	        , dataType: 'JSON' " & vbCrLf)
            jsFiltroExterno_.Append("	        , error: function (xhr, ajaxOptions, thrownError) { " & vbCrLf)
            jsFiltroExterno_.Append("	            console.log(xhr.status + ' \n' + xhr.responseText, '\n' + thrownError); " & vbCrLf)
            jsFiltroExterno_.Append("	        } " & vbCrLf)
            'jsFiltroExterno_.Append("	        , success: function (response) { " & vbCrLf)
            'jsFiltroExterno_.Append("               EjecutarFiltrosAvanzados()" & vbCrLf)
            'jsFiltroExterno_.Append("	        } " & vbCrLf)
            jsFiltroExterno_.Append("	    }); " & vbCrLf)

            jsFiltroExterno_.Append("   }" & vbCrLf)

            jsFiltroExterno_.Append("});" & vbCrLf)

            'jsFiltroExterno_.Append("EjecutarFiltrosAvanzados()" & vbCrLf)

            Return jsFiltroExterno_

        End Function

        Private Function EvaluarClaseCSS(ByVal campo_ As CampoVirtual) As StringBuilder

            Dim jsAuxiliar_ = New System.Text.StringBuilder()

            Select Case campo_.EstiloCCS

                Case IEntidadDatos.EstilossCCS.Check

                    jsAuxiliar_.Append("                        switch(data) {" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'No':" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'NO':" & vbCrLf)

                    jsAuxiliar_.Append("                                var svg_ = _kromComponents.ObtenerSVG(""close"", ""#f44336"", 18, ""d-block m-auto"")" & vbCrLf)

                    jsAuxiliar_.Append("                                return svg_" & vbCrLf)

                    jsAuxiliar_.Append("                            break;" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'Si':" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'SI':" & vbCrLf)

                    jsAuxiliar_.Append("                                var svg_ = _kromComponents.ObtenerSVG(""check"", ""#4caf50"", 18, ""d-block m-auto"")" & vbCrLf)

                    jsAuxiliar_.Append("                                return svg_" & vbCrLf)

                    jsAuxiliar_.Append("                            break;" & vbCrLf)

                    jsAuxiliar_.Append("                            default:" & vbCrLf)

                    jsAuxiliar_.Append("                                    return '<span class="""">' + data + '</span>'" & vbCrLf)

                    jsAuxiliar_.Append("                        }" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.EstatusSeguimiento

                    jsAuxiliar_.Append("                        switch(data) {" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'CERRADO':" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'Cerrado':" & vbCrLf)

                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-EstatusSeguimiento-Cerrado d-table px-3 py-3 m-auto br-3"">' + data + '</span>'" & vbCrLf)

                    jsAuxiliar_.Append("                            break;" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'ABIERTO':" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'Abierto':" & vbCrLf)

                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-EstatusSeguimiento-Abierto d-table px-3 py-3 m-auto br-3"">' + data + '</span>'" & vbCrLf)

                    jsAuxiliar_.Append("                            break;" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'SEGUIMIENTO':" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'Seguimiento':" & vbCrLf)

                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-EstatusSeguimiento-Seguimiento d-table px-3 py-3 m-auto br-3"">' + data + '</span>'" & vbCrLf)

                    jsAuxiliar_.Append("                            break;" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'SUSPENDIDO':" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'Suspendido':" & vbCrLf)

                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-EstatusSeguimiento-Suspendido d-table px-3 py-3 m-auto br-3"">' + data + '</span>'" & vbCrLf)

                    jsAuxiliar_.Append("                            break;" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'DESCARTADO':" & vbCrLf)

                    jsAuxiliar_.Append("                            case 'Descartado':" & vbCrLf)

                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-EstatusSeguimiento-Descartado d-table px-3 py-3 m-auto br-3"">' + data + '</span>'" & vbCrLf)

                    jsAuxiliar_.Append("                            break;" & vbCrLf)

                    jsAuxiliar_.Append("                            default:" & vbCrLf)

                    jsAuxiliar_.Append("                                    return '<span class=""SinDefinir"">' + data + '</span>'" & vbCrLf)

                    jsAuxiliar_.Append("                        }" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.MostrarDatos

                    jsAuxiliar_.Append("                        if (data) {" & vbCrLf)

                    jsAuxiliar_.Append("                            let svg_ = _kromComponents.ObtenerSVG(""listNote"", ""#14937b"", 25, ""ver-datos d-block m-auto cur-pointer"", ""data-id="" + data + "" "")" & vbCrLf)

                    jsAuxiliar_.Append("                            return svg_" & vbCrLf)

                    jsAuxiliar_.Append("                         }else{")

                    jsAuxiliar_.Append("                         return """"}")

                Case IEntidadDatos.EstilossCCS.EditarDatos

                    jsAuxiliar_.Append("                        if (data) {" & vbCrLf)

                    jsAuxiliar_.Append("                            let svg_ = _kromComponents.ObtenerSVG(""editRow"", ""#14937b"", 25, ""editar-fila d-block m-auto cur-pointer"", ""data-id="" + data + "" "")" & vbCrLf)

                    jsAuxiliar_.Append("                            return svg_" & vbCrLf)

                    jsAuxiliar_.Append("                         }else{")

                    jsAuxiliar_.Append("                         return """"}")

                Case IEntidadDatos.EstilossCCS.ClaveDocumentoPDF
                    jsAuxiliar_.Append("                        if (data != 0) {" & vbCrLf)
                    jsAuxiliar_.Append("                            $('.iEntidadDatos-ArchivoEspecial_PDF').parent().addClass(""text-center"")" & vbCrLf)
                    jsAuxiliar_.Append("                            return '<a class=""iEntidadDatos-ArchivoEspecial_PDF"" href=""/FrontEnd/NameSpaces/DescargarDocumentos.aspx?mod=opeadmin&doc=cmd&bus=' + data + '&ext=pdf&cla=clv""><img src=""/FrontEnd/Recursos/imgs/pdf.png"" target=""icon"" style=""width: 35px;"" title=""Soportes"" /></a>'; " & vbCrLf)
                    jsAuxiliar_.Append("                         }else{")
                    jsAuxiliar_.Append("                         return """"}")

                Case IEntidadDatos.EstilossCCS.SeparacionMiles
                    jsAuxiliar_.Append("                        if (data) {")
                    jsAuxiliar_.Append("                            var numero = parseFloat(data).toLocaleString('en-US')" & vbCrLf)
                    jsAuxiliar_.Append("                            return '<span class=""iEntidadDatos-EstilossCCS-SeparacionMiles"">' + numero + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                        }")

                Case IEntidadDatos.EstilossCCS.MostrarDocumentos
                    jsAuxiliar_.Append("                         if (data != 0) {")
                    jsAuxiliar_.Append("                              $('.mostrar-documentos').parent().addClass(""text-center"");" & vbCrLf)
                    jsAuxiliar_.Append("                              if(data != 3) {" & vbCrLf)
                    jsAuxiliar_.Append("                                return """" " & vbCrLf)
                    jsAuxiliar_.Append("                              } else {" & vbCrLf)
                    jsAuxiliar_.Append("                                return '<span class=""mostrar-documentos""><i class=""fa kb-btn-ba fa-plus-circle"" style=""font-size: 20px;color: #14937b;cursor: pointer;""></i></span>'" & vbCrLf)
                    jsAuxiliar_.Append("                              }" & vbCrLf)
                    jsAuxiliar_.Append("                         } else {")
                    jsAuxiliar_.Append("                         return """"}")

                Case IEntidadDatos.EstilossCCS.TipoOperacion

                    jsAuxiliar_.Append("                        switch(data) {" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Importación':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-tipoOperacion-impo"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Exportación':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-tipoOperacion-expo"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Triangulación':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-tipoOperacion-triangulacion"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            default:" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""SinDefinir"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                        }" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.Modalidad

                    jsAuxiliar_.Append("                        switch(data) {" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'MARÍTIMO':" & vbCrLf)
                    jsAuxiliar_.Append("                                    $('.iEntidadDatos-modalidad-maritimo').parent().addClass(""text-center"")" & vbCrLf)
                    'jsAuxiliar_.Append("                                    return '<img class=""iEntidadDatos-modalidad-maritimo"" src=""../../../../../../../../../../CapaPresentacion/Componentes/IMAGENES/trafico_maritimo.png""><span style = "" display: none ""> ' + data + ' </span> </img>'" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class=""iEntidadDatos-modalidad-maritimo"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><title>Marítimo</title><g id=""layer101"" fill=""#77af15"" stroke=""none""><path d=""M278 1885 c-21 -14 -42 -34 -48 -44 -30 -57 -230 -634 -230 -666 0 -76 -30 -59 548 -310 361 -156 364 -157 425 -153 53 4 119 30 454 176 452 197 469 205 483 231 22 42 8 99 -100 398 -61 168 -115 314 -120 324 -6 10 -27 30 -48 44 l-37 25 -645 0 -645 0 -37 -25z""/><path d=""M332 878 c-8 -8 -12 -67 -12 -199 0 -187 0 -188 26 -218 l26 -31 588 0 588 0 26 31 26 31 -2 190 c-3 166 -5 192 -20 201 -13 9 -71 -13 -280 -103 -258 -113 -264 -115 -338 -115 -71 0 -82 3 -220 66 -213 98 -359 159 -378 159 -10 0 -23 -5 -30 -12z m996 -350 l-3 -23 -365 0 -365 0 -3 23 -3 22 371 0 371 0 -3 -22z""/><path d=""M588 364 c-38 -20 -51 -71 -46 -190 2 -74 8 -107 21 -127 l17 -27 380 0 380 0 17 27 c22 34 31 207 14 265 -21 67 -26 68 -413 68 -283 -1 -348 -3 -370 -16z m173 -103 c29 -23 31 -81 4 -111 -27 -30 -78 -27 -104 6 -28 35 -26 69 4 99 29 30 64 32 96 6z m244 -6 c66 -65 -16 -163 -93 -109 -16 11 -22 25 -22 52 0 42 6 54 35 70 31 18 54 14 80 -13z m243 5 c31 -29 31 -93 0 -114 -31 -22 -68 -20 -95 6 -31 29 -30 72 2 103 29 30 64 32 93 5z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'AÉREO':" & vbCrLf)
                    jsAuxiliar_.Append("                                    $('.iEntidadDatos-modalidad-aereo').parent().addClass(""text-center"")" & vbCrLf)
                    'jsAuxiliar_.Append("                                    return '<img class=""iEntidadDatos-modalidad-aereo"" src=""../../../../../../../../../../CapaPresentacion/Componentes/IMAGENES/trafico_aereo.png""><span style = "" display: none ""> ' + data + ' </span> </img>'" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class=""iEntidadDatos-modalidad-aereo"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><title>Aéreo</title><g id=""layer101"" fill=""#77af15"" stroke=""none""><path d=""M640 1901 c-13 -25 -13 -100 1 -137 6 -17 38 -50 77 -79 l67 -51 3 -222 c2 -200 1 -222 -14 -222 -9 0 -176 52 -371 116 -195 63 -364 114 -376 112 -21 -3 -22 -9 -25 -105 l-3 -101 33 -25 c18 -13 184 -113 368 -222 184 -109 347 -207 362 -217 l26 -18 4 -293 c3 -279 4 -294 25 -333 25 -47 73 -91 109 -100 35 -9 88 3 118 27 15 12 39 44 54 72 l27 52 3 287 3 287 27 19 c15 10 178 108 362 217 184 109 350 209 368 222 l33 25 -3 101 c-3 96 -4 102 -25 105 -12 2 -181 -49 -376 -112 -195 -64 -362 -116 -371 -116 -15 0 -16 22 -14 222 l3 222 67 51 c72 54 88 82 88 152 0 50 -14 83 -35 83 -8 0 -77 -18 -155 -41 l-140 -41 -140 41 c-78 23 -147 41 -155 41 -8 0 -19 -9 -25 -19z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'TERRESTRE':" & vbCrLf)
                    jsAuxiliar_.Append("                                    $('.iEntidadDatos-modalidad-terrestre').parent().addClass(""text-center"")" & vbCrLf)
                    'jsAuxiliar_.Append("                                    return '<img class=""iEntidadDatos-modalidad-terrestre"" src=""../../../../../../../../../../CapaPresentacion/Componentes/IMAGENES/trafico_terrestre.png""><span style = "" display: none ""> ' + data + ' </span> </img>'" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class=""iEntidadDatos-modalidad-terrestre"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><title>Terrestre</title><g id=""layer101"" fill=""#77af15"" stroke=""none""><path d=""M313 1903 c-19 -13 -23 -25 -23 -68 0 -28 52 -438 115 -911 63 -472 115 -861 115 -865 0 -3 11 -18 25 -33 l24 -26 159 0 158 0 -3 123 c-2 67 -5 150 -6 185 l-3 62 90 0 89 0 -6 -185 -6 -185 163 0 c159 0 163 1 185 24 21 23 30 82 131 883 60 472 112 881 115 910 7 48 5 54 -20 77 l-28 26 -253 0 -254 0 -2 -192 -3 -193 -116 -3 -116 -3 -6 122 c-4 67 -7 155 -7 196 l0 73 -247 -1 c-209 0 -252 -3 -270 -16z m756 -655 c0 -84 -3 -171 -7 -193 l-7 -40 -90 -3 c-50 -1 -95 1 -102 6 -8 5 -12 62 -13 187 -1 99 -1 183 -1 188 1 4 51 7 111 7 l110 0 -1 -152z m-9 -492 c0 -73 -3 -161 -6 -195 l-7 -61 -83 0 c-45 0 -85 3 -87 8 -3 4 -8 91 -12 195 l-8 187 102 0 101 0 0 -134z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            default:" & vbCrLf)
                    jsAuxiliar_.Append("                                    if(row.AduanaES == 651) {" & vbCrLf)
                    jsAuxiliar_.Append("                                        $('.iEntidadDatos-modalidad-terrestre').parent().addClass(""text-center"")" & vbCrLf)
                    'jsAuxiliar_.Append("                                        return '<img class=""iEntidadDatos-modalidad-terrestre"" src=""../../../../../../../../../../CapaPresentacion/Componentes/IMAGENES/trafico_terrestre.png""><span style = "" display: none ""> ' + data + ' </span> </img>'" & vbCrLf)
                    jsAuxiliar_.Append("                                        return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class=""iEntidadDatos-modalidad-terrestre"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><title>Ferroviario</title><g id=""layer101"" fill=""#77af15"" stroke=""none""><path d=""M313 1903 c-19 -13 -23 -25 -23 -68 0 -28 52 -438 115 -911 63 -472 115 -861 115 -865 0 -3 11 -18 25 -33 l24 -26 159 0 158 0 -3 123 c-2 67 -5 150 -6 185 l-3 62 90 0 89 0 -6 -185 -6 -185 163 0 c159 0 163 1 185 24 21 23 30 82 131 883 60 472 112 881 115 910 7 48 5 54 -20 77 l-28 26 -253 0 -254 0 -2 -192 -3 -193 -116 -3 -116 -3 -6 122 c-4 67 -7 155 -7 196 l0 73 -247 -1 c-209 0 -252 -3 -270 -16z m756 -655 c0 -84 -3 -171 -7 -193 l-7 -40 -90 -3 c-50 -1 -95 1 -102 6 -8 5 -12 62 -13 187 -1 99 -1 183 -1 188 1 4 51 7 111 7 l110 0 -1 -152z m-9 -492 c0 -73 -3 -161 -6 -195 l-7 -61 -83 0 c-45 0 -85 3 -87 8 -3 4 -8 91 -12 195 l-8 187 102 0 101 0 0 -134z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                                    } else {" & vbCrLf)
                    jsAuxiliar_.Append("                                        return '<span class=""SinDefinir"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                                    }" & vbCrLf)
                    jsAuxiliar_.Append("                        }" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.Aduana

                    jsAuxiliar_.Append("                        switch(data) {" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ALTAMIRA':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-altamira"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'LÁZARO CÁRDENAS':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-lazaroCardenas"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'MANZANILLO':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-manzanillo"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'MÉXICO':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-mexico"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'TOLUCA':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-toluca"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'VERACRUZ':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-veracruz"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'TUXPAN':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-veracruz"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'NUEVO LAREDO':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-nuevoLaredo"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            default:" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""SinDefinir"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                        }" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.DivisionEmpresa

                    jsAuxiliar_.Append("                        switch(data) {" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Altamira':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-altamira"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Lázaro':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-lazaroCardenas"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Manzanillo':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-manzanillo"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'México':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-mexico"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Toluca':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-toluca"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Veracruz':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-veracruz"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Laredo':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-nuevoLaredo"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Forwarder':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-aduana-mexico"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            default:" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""SinDefinir"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                        }" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.ArchivoEspecial_PDF
                    jsAuxiliar_.Append("                         if (data != 0) {")
                    jsAuxiliar_.Append("                              $('.iEntidadDatos-ArchivoEspecial_PDF').parent().addClass(""text-center"");" & vbCrLf)
                    'jsAuxiliar_.Append("                              var anio = row['Fecha Pago'].split('/')[2];" & vbCrLf)
                    'jsAuxiliar_.Append("                              var mes = row['Fecha Pago'].split('/')[1];" & vbCrLf)
                    'jsAuxiliar_.Append("                              var dia = row['Fecha Pago'].split('/')[0];" & vbCrLf)
                    'jsAuxiliar_.Append("                              let fechaPago = new Date(anio, mes, dia), fechaCorte = new Date(2019, 05, 01);" & vbCrLf)
                    'jsAuxiliar_.Append("                              if(fechaPago < fechaCorte) {" & vbCrLf)
                    jsAuxiliar_.Append("                              if(data != 3) {" & vbCrLf)
                    jsAuxiliar_.Append("                                return '<a class=""iEntidadDatos-ArchivoEspecial_PDF"" href=""/FrontEnd/NameSpaces/DescargarDocumentos.aspx?mod=opeadmin&doc=ope&bus=' + row['Referencia'] + '&ext=pdf""><img src=""/FrontEnd/Recursos/imgs/pdf.png"" target=""icon"" style=""width: 35px;"" title="""" /></a>' ; " & vbCrLf)
                    jsAuxiliar_.Append("                              } else {" & vbCrLf)
                    jsAuxiliar_.Append("                                return '<a class=""iEntidadDatos-ArchivoEspecial_PDF"" href=""/FrontEnd/NameSpaces/DescargarDocumentos.aspx?mod=opeadmin&doc=mdz&bus=' + row['Referencia'] + '&ext=pdf""><img src=""/FrontEnd/Recursos/imgs/zip.png"" target=""icon"" style=""width: 35px;"" title="""" /></a>' ; " & vbCrLf)
                    jsAuxiliar_.Append("                              }" & vbCrLf)
                    jsAuxiliar_.Append("                         } else {")
                    jsAuxiliar_.Append("                         return """"}")

                Case IEntidadDatos.EstilossCCS.CG_PDF
                    jsAuxiliar_.Append("                        $('.iEntidadDatos-CG_PDF').parent().addClass(""text-center"")" & vbCrLf)
                    jsAuxiliar_.Append("                        return '<a class=""iEntidadDatos-CG_PDF"" href=""/FrontEnd/NameSpaces/DescargarDocumentos.aspx?mod=opeadmin&doc=admi&bus=' + data + '&ext=pdf""><img src=""/FrontEnd/Recursos/imgs/pdf.png"" target=""icon"" style=""width: 35px;"" title=""Cuenta de gatos"" /></a>'; " & vbCrLf)

                Case IEntidadDatos.EstilossCCS.CG_XML
                    jsAuxiliar_.Append("                        $('.iEntidadDatos-CG_XML').parent().addClass(""text-center"")" & vbCrLf)
                    jsAuxiliar_.Append("                        return '<a class=""iEntidadDatos-CG_XML"" href=""/FrontEnd/NameSpaces/DescargarDocumentos.aspx?mod=opeadmin&doc=admi&bus=' + data + '&ext=xml""><img src=""/FrontEnd/Recursos/imgs/xml.png"" target=""icon"" style=""width: 35px;"" title=""CFDI"" /></a>'; " & vbCrLf)

                Case IEntidadDatos.EstilossCCS.Soportes_CG_ZIP
                    jsAuxiliar_.Append("                        if (data != """") {" & vbCrLf)
                    jsAuxiliar_.Append("                            $('.iEntidadDatos-Soportes_CG_ZIP').parent().addClass(""text-center"")" & vbCrLf)
                    jsAuxiliar_.Append("                            return '<a class=""iEntidadDatos-Soportes_CG_ZIP"" href=""/FrontEnd/NameSpaces/DescargarDocumentos.aspx?mod=opeadmin&doc=admi&bus=' + data + '&ext=zip&cla=sop""><img src=""/FrontEnd/Recursos/imgs/zip.png"" target=""icon"" style=""width: 35px;"" title=""Soportes"" /></a>'; " & vbCrLf)
                    jsAuxiliar_.Append("                         }else{")
                    jsAuxiliar_.Append("                         return """"}")

                Case IEntidadDatos.EstilossCCS.ComplementosPago_ZIP
                    jsAuxiliar_.Append("                        if (data != """") {" & vbCrLf)
                    jsAuxiliar_.Append("                            $('.iEntidadDatos-ComplementosPago_ZIP').parent().addClass(""text-center"")" & vbCrLf)
                    jsAuxiliar_.Append("                            return '<a class=""iEntidadDatos-ComplementosPago_ZIP"" href=""/FrontEnd/NameSpaces/DescargarDocumentos.aspx?mod=opeadmin&doc=admi&bus=' + data + '&ext=zip&cla=cpg""><img src=""/FrontEnd/Recursos/imgs/zip.png"" target=""icon"" style=""width: 35px;"" title=""Complementos de pago KROM"" /></a>'; " & vbCrLf)
                    jsAuxiliar_.Append("                         }else{")
                    jsAuxiliar_.Append("                         return """"}")

                Case IEntidadDatos.EstilossCCS.ComplementosPagoTerceros_ZIP
                    jsAuxiliar_.Append("                        if (data != """") {" & vbCrLf)
                    jsAuxiliar_.Append("                            $('.iEntidadDatos-ComplementosPagoTerceros_ZIP').parent().addClass(""text-center"")" & vbCrLf)
                    jsAuxiliar_.Append("                            return '<a class=""iEntidadDatos-ComplementosPagoTerceros_ZIP"" href=""/FrontEnd/NameSpaces/DescargarDocumentos.aspx?mod=opeadmin&doc=admi&bus=' + data + '&ext=zip&cla=cpt""><img src=""/FrontEnd/Recursos/imgs/zip.png"" target=""icon"" style=""width: 35px;"" title=""Complementos de pago terceros"" /></a>'; " & vbCrLf)
                    jsAuxiliar_.Append("                         }else{")
                    jsAuxiliar_.Append("                         return """"}")

                Case IEntidadDatos.EstilossCCS.AgenciaAduanal

                    jsAuxiliar_.Append("                        data = (typeof data == 'undefined') ? data : data.toUpperCase();" & vbCrLf)
                    jsAuxiliar_.Append("                        switch(data) {" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'COMERCIO EXTERIOR DEL GOLFO S.C.':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""IEntidadDatos-AgenciaAduanal-CEG"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'DESPACHOS AEREOS INTEGRADOS S.C':" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'DESPACHOS AEREOS INTEGRADOS S. C.':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""IEntidadDatos-AgenciaAduanal-DAI"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'GRUPO REYES KURI S.C.':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""IEntidadDatos-AgenciaAduanal-GRK"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'SERVICIOS ADUANALES DEL PACIFICO S.C.':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""IEntidadDatos-AgenciaAduanal-SAP"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'KROM FORWARDING INC.':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""IEntidadDatos-AgenciaAduanal-GRK"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            default:" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""SinDefinir"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                        }" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.EstatusFactura
                    jsAuxiliar_.Append("                        switch(data) {" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'CFDi CANCELADA':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""IEntidadDatos-EstatusFactura-Cancelada"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'CFDi EMITIDA':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""IEntidadDatos-EstatusFactura-Emitida"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            default:" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""SinDefinir"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                        }" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.AtlasReferenciaAgenciaAduanal

                    jsAuxiliar_.Append("                        data = data.split("","");" & vbCrLf)
                    jsAuxiliar_.Append("                        var result = """"; " & vbCrLf)
                    jsAuxiliar_.Append("                        $.each(data, function(i,v){" & vbCrLf)
                    jsAuxiliar_.Append("                            if (i==0){" & vbCrLf)
                    jsAuxiliar_.Append("                                if ((v.indexOf(""RKU"") > -1 ) || (v.indexOf(""SAP"") > -1 )  || (v.indexOf(""TUX"") > -1 ) || (v.indexOf(""ALC"") > -1 ) || (v.indexOf(""DAI"") > -1) || (v.indexOf(""CEG"") > -1 ) || (v.indexOf(""TOL"") > -1 ) || (v.indexOf(""SFC"") > -1 ) || (v.indexOf(""SFI"") > -1 ))" & vbCrLf)
                    jsAuxiliar_.Append("                                {" & vbCrLf)
                    jsAuxiliar_.Append("                                    result = result + ' ' + '<a href=""/FrontEnd/Modulos/TrackingOperaciones/Ges003-001-Consultas.Operaciones.aspx?referencia=' + v + '"">' + v + '</a>'; " & vbCrLf)
                    jsAuxiliar_.Append("                                }" & vbCrLf)
                    jsAuxiliar_.Append("                                else " & vbCrLf)
                    jsAuxiliar_.Append("                                {" & vbCrLf)
                    jsAuxiliar_.Append("                                    result = result + ' ' + '<span class=""SinDefinir"">' + v + '</span>'; " & vbCrLf)
                    jsAuxiliar_.Append("                                }" & vbCrLf)
                    jsAuxiliar_.Append("                            }" & vbCrLf)
                    jsAuxiliar_.Append("                            else" & vbCrLf)
                    jsAuxiliar_.Append("                            {" & vbCrLf)
                    jsAuxiliar_.Append("                                if ((v.indexOf(""RKU"") > -1 ) || (v.indexOf(""SAP"") > -1 )  || (v.indexOf(""TUX"") > -1 ) || (v.indexOf(""ALC"") > -1 ) || (v.indexOf(""DAI"") > -1) || (v.indexOf(""CEG"") > -1 ) || (v.indexOf(""TOL"") > -1 ) || (v.indexOf(""SFC"") > -1 ) || (v.indexOf(""SFI"") > -1 ))" & vbCrLf)
                    jsAuxiliar_.Append("                                {" & vbCrLf)
                    jsAuxiliar_.Append("                                    result = result + ' ' + '<a href=""/FrontEnd/Modulos/TrackingOperaciones/Ges003-001-Consultas.Operaciones.aspx?referencia=' + v + '"">, ' + v + '</a>'; " & vbCrLf)
                    jsAuxiliar_.Append("                                }" & vbCrLf)
                    jsAuxiliar_.Append("                                else " & vbCrLf)
                    jsAuxiliar_.Append("                                {" & vbCrLf)
                    jsAuxiliar_.Append("                                    result = result + ' ' + '<span class=""SinDefinir"">, ' + v + '</span>'; " & vbCrLf)
                    jsAuxiliar_.Append("                                }" & vbCrLf)
                    jsAuxiliar_.Append("                            }" & vbCrLf)
                    jsAuxiliar_.Append("                        });" & vbCrLf)
                    jsAuxiliar_.Append("                        return result" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.TipoComprobante
                    jsAuxiliar_.Append("                        switch(data) {" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Factura Operativa':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""IEntidadDatos-TipoComprobante-Operativa"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Factura Administrativa':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""IEntidadDatos-TipoComprobante-Administrativa"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Nota Credito Bonificacion':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""IEntidadDatos-TipoComprobante-Bonificacion"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Nota Credito Con Cargo':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""IEntidadDatos-TipoComprobante-Cargo"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            default:" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""SinDefinir"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                        }" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.TipoMovimientoLogistica
                    jsAuxiliar_.Append("                        switch(data) {" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Sin definir':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-TipoMovimientoLogistica_SinDefinir"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Door to Port':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-TipoMovimientoLogistica_DoortoPort"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Port to Door':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-TipoMovimientoLogistica_PorttoDoor"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Port to Port':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-TipoMovimientoLogistica_PorttoPort"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'Door to Door':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-TipoMovimientoLogistica_DoortoDoor"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            default:" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""SinDefinir"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                        }" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.OficinaLogistica

                    jsAuxiliar_.Append("                        switch(data) {" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'SIN DEFINIR':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-OficinaLogistica_SinDefinir"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'MONTERREY':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-OficinaLogistica_Monterrey"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'MEXICO':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-OficinaLogistica_Mexico"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'VERACRUZ':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-OficinaLogistica_Veracruz"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'BARQUIN':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""iEntidadDatos-OficinaLogistica_Barquien"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            default:" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""SinDefinir"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                        }" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.EstatusTracking

                    jsAuxiliar_.Append("                        $('.iEntidadDatos-EstatusTracking_AgenciaAduanal').parent().addClass(""text-center"")" & vbCrLf)
                    jsAuxiliar_.Append("                        switch(data) {" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'give-money':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 354.796 354.796""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""><g><g id=""Layer_5_58_""><g><g><path d=""M265.442,109.092c-10.602-4.25-13.665-6.82-13.665-11.461c0-3.714,2.813-8.053,10.744-8.053c7.015,0,12.395,2.766,12.443,2.79c0.566,0.302,1.201,0.463,1.83,0.463c1.535,0,2.893-0.929,3.456-2.367l1.927-4.926c0.671-1.795-0.347-3.359-1.645-3.92c-4.319-1.88-12.76-3.335-12.846-3.35c-0.136-0.024-0.609-0.125-0.609-0.678l-0.027-7.146c0-2.152-1.797-3.904-4.003-3.904h-3.457c-2.204,0-4,1.751-4,3.905l0.009,7.513c0,0.576-0.624,0.826-0.852,0.879c-10.655,2.538-17.314,10.343-17.314,20.188c0,12.273,10.145,17.819,21.099,21.982c8.757,3.438,12.329,6.924,12.329,12.037c0,5.564-5.059,9.45-12.307,9.45c-6.189,0-14.565-3.923-14.648-3.963c-0.536-0.254-1.104-0.382-1.688-0.382c-1.594,0-2.982,0.964-3.537,2.457l-1.84,4.982c-0.654,1.86,0.353,3.37,1.642,4.042c5.144,2.679,15.098,4.249,15.541,4.318c0.119,0.017,0.725,0.23,0.725,0.784v7.48c0,2.152,1.797,3.904,4.004,3.904h3.572c2.208,0,4.005-1.751,4.005-3.904v-7.872c0-0.736,0.543-0.801,0.655-0.828c11.351-2.55,18.343-10.855,18.343-21.283C285.325,121.518,279.377,114.597,265.442,109.092z""/><path d=""M260.979,22.509c-51.816,0-93.818,42.005-93.818,93.818c0,51.814,42.002,93.82,93.818,93.82c51.814,0,93.817-42.006,93.817-93.82C354.796,64.514,312.793,22.509,260.979,22.509z M260.979,188.404c-39.808,0-72.076-32.271-72.076-72.076s32.268-72.075,72.076-72.075c39.806,0,72.073,32.27,72.073,72.075S300.785,188.404,260.979,188.404z""/></g><g><path d=""M335.733,255.61c-19.95,11.011-47.389,21.192-74.753,25.484c-24.346,3.818-70.148-5.39-70.148-16.265c0-4.121,40.17,10.154,64.469,3.671c18.633-4.971,15.988-22.401,5.853-24.7c-10.076-2.287-69.108-23.913-94.323-24.659c-11.878-0.351-41.203,4.131-55.393,6.442c-4.861,0.791-7.909,0.704-8.213,5.356c-1.412,21.62-4.195,65.832-5.712,88.926c-0.032,0.488,0.646,7.05,6.061,2.432c5.927-5.054,14.24-10.656,21.929-8.912c12.063,2.737,116.424,21.856,130.819,18.51c20.593-4.787,78.888-39.334,90.065-50.072C363.711,265.176,350.244,247.601,335.733,255.61z""/><path d=""M74.426,224.74l-54.672-2.694c-4.221-0.208-8.532,2.973-9.581,7.066l-9.941,90.255c-1.048,4.094,1.55,7.578,5.773,7.741l60.59-0.006c4.224,0.163,7.942-3.151,8.266-7.365l6.654-86.958C81.837,228.566,78.647,224.948,74.426,224.74z M42.24,315.145c-8.349,0-15.116-6.768-15.116-15.116c0-8.349,6.768-15.116,15.116-15.116s15.116,6.768,15.116,15.116C57.356,308.378,50.589,315.145,42.24,315.145z""/></g></g></g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'FAC':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 354.796 354.796""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""><title>Emisión de cuenta de gasto</title><g><g id=""Layer_5_58_""><g><g><path d=""M265.442,109.092c-10.602-4.25-13.665-6.82-13.665-11.461c0-3.714,2.813-8.053,10.744-8.053c7.015,0,12.395,2.766,12.443,2.79c0.566,0.302,1.201,0.463,1.83,0.463c1.535,0,2.893-0.929,3.456-2.367l1.927-4.926c0.671-1.795-0.347-3.359-1.645-3.92c-4.319-1.88-12.76-3.335-12.846-3.35c-0.136-0.024-0.609-0.125-0.609-0.678l-0.027-7.146c0-2.152-1.797-3.904-4.003-3.904h-3.457c-2.204,0-4,1.751-4,3.905l0.009,7.513c0,0.576-0.624,0.826-0.852,0.879c-10.655,2.538-17.314,10.343-17.314,20.188c0,12.273,10.145,17.819,21.099,21.982c8.757,3.438,12.329,6.924,12.329,12.037c0,5.564-5.059,9.45-12.307,9.45c-6.189,0-14.565-3.923-14.648-3.963c-0.536-0.254-1.104-0.382-1.688-0.382c-1.594,0-2.982,0.964-3.537,2.457l-1.84,4.982c-0.654,1.86,0.353,3.37,1.642,4.042c5.144,2.679,15.098,4.249,15.541,4.318c0.119,0.017,0.725,0.23,0.725,0.784v7.48c0,2.152,1.797,3.904,4.004,3.904h3.572c2.208,0,4.005-1.751,4.005-3.904v-7.872c0-0.736,0.543-0.801,0.655-0.828c11.351-2.55,18.343-10.855,18.343-21.283C285.325,121.518,279.377,114.597,265.442,109.092z""/><path d=""M260.979,22.509c-51.816,0-93.818,42.005-93.818,93.818c0,51.814,42.002,93.82,93.818,93.82c51.814,0,93.817-42.006,93.817-93.82C354.796,64.514,312.793,22.509,260.979,22.509z M260.979,188.404c-39.808,0-72.076-32.271-72.076-72.076s32.268-72.075,72.076-72.075c39.806,0,72.073,32.27,72.073,72.075S300.785,188.404,260.979,188.404z""/></g><g><path d=""M335.733,255.61c-19.95,11.011-47.389,21.192-74.753,25.484c-24.346,3.818-70.148-5.39-70.148-16.265c0-4.121,40.17,10.154,64.469,3.671c18.633-4.971,15.988-22.401,5.853-24.7c-10.076-2.287-69.108-23.913-94.323-24.659c-11.878-0.351-41.203,4.131-55.393,6.442c-4.861,0.791-7.909,0.704-8.213,5.356c-1.412,21.62-4.195,65.832-5.712,88.926c-0.032,0.488,0.646,7.05,6.061,2.432c5.927-5.054,14.24-10.656,21.929-8.912c12.063,2.737,116.424,21.856,130.819,18.51c20.593-4.787,78.888-39.334,90.065-50.072C363.711,265.176,350.244,247.601,335.733,255.61z""/><path d=""M74.426,224.74l-54.672-2.694c-4.221-0.208-8.532,2.973-9.581,7.066l-9.941,90.255c-1.048,4.094,1.55,7.578,5.773,7.741l60.59-0.006c4.224,0.163,7.942-3.151,8.266-7.365l6.654-86.958C81.837,228.566,78.647,224.948,74.426,224.74z M42.24,315.145c-8.349,0-15.116-6.768-15.116-15.116c0-8.349,6.768-15.116,15.116-15.116s15.116,6.768,15.116,15.116C57.356,308.378,50.589,315.145,42.24,315.145z""/></g></g></g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'check-box':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 459 459""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""> <g> <g id=""check-box-outline""> <path d=""M124.95,181.05l-35.7,35.7L204,331.5l255-255l-35.7-35.7L204,260.1L124.95,181.05z M408,408H51V51h255V0H51 C22.95,0,0,22.95,0,51v357c0,28.05,22.95,51,51,51h357c28.05,0,51-22.95,51-51V204h-51V408z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'LFAC':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 459 459""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""> <title>Pago de cuenta de gasto</title> <g> <g id=""check-box-outline""> <path d=""M124.95,181.05l-35.7,35.7L204,331.5l255-255l-35.7-35.7L204,260.1L124.95,181.05z M408,408H51V51h255V0H51 C22.95,0,0,22.95,0,51v357c0,28.05,22.95,51,51,51h357c28.05,0,51-22.95,51-51V204h-51V408z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'DSP':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><title>Despacho aduanal</title><g id=""layer101""  stroke=""none""><path d=""M170 1675 l0 -35 -85 0 -85 0 0 -670 0 -670 43 0 c36 0 44 -4 59 -30 29 -51 106 -80 213 -80 79 0 98 4 152 29 47 22 64 36 69 55 6 23 11 26 55 26 l49 0 0 670 0 670 -90 0 -90 0 0 35 0 35 -145 0 -145 0 0 -35z m206 -194 c125 -57 119 -227 -9 -276 -57 -22 -110 -8 -156 39 -58 60 -57 141 2 204 49 51 100 62 163 33z m0 -400 c89 -40 119 -149 62 -223 -60 -79 -159 -85 -227 -14 -58 60 -57 141 2 204 49 51 100 62 163 33z m4 -393 c82 -43 110 -119 71 -196 -50 -98 -165 -115 -242 -36 -26 27 -33 43 -37 88 -6 69 20 115 81 145 53 27 72 26 127 -1z""/><path d=""M1379 1407 c-38 -25 -56 -76 -45 -123 4 -16 46 -68 99 -121 l91 -93 -305 0 c-182 0 -319 -4 -342 -11 -77 -21 -105 -130 -48 -187 17 -16 44 -32 62 -36 17 -3 167 -6 333 -6 l301 0 -88 -88 c-90 -91 -111 -129 -100 -183 12 -63 92 -104 152 -80 15 7 118 103 229 215 l202 202 0 54 0 54 -197 199 c-235 237 -267 256 -344 204z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'validated-box':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M170 1675 l0 -35 -85 0 -85 0 0 -670 0 -670 43 0 c36 0 44 -4 59 -30 29 -51 106 -80 213 -80 79 0 98 4 152 29 47 22 64 36 69 55 6 23 11 26 55 26 l49 0 0 670 0 670 -90 0 -90 0 0 35 0 35 -145 0 -145 0 0 -35z m206 -194 c125 -57 119 -227 -9 -276 -57 -22 -110 -8 -156 39 -58 60 -57 141 2 204 49 51 100 62 163 33z m0 -400 c89 -40 119 -149 62 -223 -60 -79 -159 -85 -227 -14 -58 60 -57 141 2 204 49 51 100 62 163 33z m4 -393 c82 -43 110 -119 71 -196 -50 -98 -165 -115 -242 -36 -26 27 -33 43 -37 88 -6 69 20 115 81 145 53 27 72 26 127 -1z""/><path d=""M1379 1407 c-38 -25 -56 -76 -45 -123 4 -16 46 -68 99 -121 l91 -93 -305 0 c-182 0 -319 -4 -342 -11 -77 -21 -105 -130 -48 -187 17 -16 44 -32 62 -36 17 -3 167 -6 333 -6 l301 0 -88 -88 c-90 -91 -111 -129 -100 -183 12 -63 92 -104 152 -80 15 7 118 103 229 215 l202 202 0 54 0 54 -197 199 c-235 237 -267 256 -344 204z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'SAR':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 94.477 94.477"" style=""fill: #000;"" xml:space=""preserve""> <g> <path d=""M60.936,7.325C60.936,3.279,54.803,0,47.238,0c-7.564,0-13.697,3.279-13.697,7.325h-6.053v82.553h11.15v4.599h17.2v-4.599 h11.15V7.325H60.936z M47.238,81.52c-5.41,0-9.795-4.385-9.795-9.795c0-5.408,4.385-9.794,9.795-9.794s9.795,4.386,9.795,9.794 C57.033,77.135,52.648,81.52,47.238,81.52z M47.238,56.676c-5.41,0-9.795-4.387-9.795-9.796c0-5.41,4.385-9.795,9.795-9.795 s9.795,4.385,9.795,9.795C57.033,52.289,52.648,56.676,47.238,56.676z M47.238,32.545c-5.41,0-9.795-4.385-9.795-9.794 c0-5.41,4.385-9.795,9.795-9.795s9.795,4.385,9.795,9.795C57.033,28.16,52.648,32.545,47.238,32.545z""/> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'traffic-light':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 94.477 94.477"" style=""fill: #000;"" xml:space=""preserve""> <g> <path d=""M60.936,7.325C60.936,3.279,54.803,0,47.238,0c-7.564,0-13.697,3.279-13.697,7.325h-6.053v82.553h11.15v4.599h17.2v-4.599 h11.15V7.325H60.936z M47.238,81.52c-5.41,0-9.795-4.385-9.795-9.795c0-5.408,4.385-9.794,9.795-9.794s9.795,4.386,9.795,9.794 C57.033,77.135,52.648,81.52,47.238,81.52z M47.238,56.676c-5.41,0-9.795-4.387-9.795-9.796c0-5.41,4.385-9.795,9.795-9.795 s9.795,4.385,9.795,9.795C57.033,52.289,52.648,56.676,47.238,56.676z M47.238,32.545c-5.41,0-9.795-4.385-9.795-9.794 c0-5.41,4.385-9.795,9.795-9.795s9.795,4.385,9.795,9.795C57.033,28.16,52.648,32.545,47.238,32.545z""/> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'RAD':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><title>Reconocimiento aduanero</title><g id=""layer101""  stroke=""none""><path d=""M1595 1843 c-39 -43 -89 -97 -111 -121 -38 -41 -40 -42 -72 -31 -56 19 -188 7 -247 -23 -193 -100 -241 -362 -95 -521 55 -60 114 -93 194 -108 152 -28 311 63 367 210 30 81 24 196 -16 275 l-29 57 55 57 c141 147 149 158 149 206 0 54 -24 76 -84 76 -36 0 -45 -6 -111 -77z m-161 -300 c46 -32 96 -118 96 -166 0 -150 -154 -257 -288 -201 -171 71 -176 305 -7 386 58 27 144 19 199 -19z""/><path d=""M189 1745 c-35 -19 -46 -50 -64 -179 -33 -237 -31 -239 180 -302 83 -24 166 -49 186 -54 31 -9 37 -15 42 -46 6 -33 2 -40 -49 -89 -64 -63 -108 -151 -123 -245 -13 -76 -14 -153 -3 -148 4 2 41 20 82 41 94 47 169 61 290 54 102 -5 179 -27 248 -71 23 -14 44 -26 47 -26 11 0 4 129 -10 185 -23 88 -54 143 -112 204 -43 44 -53 62 -53 89 0 30 4 35 40 49 36 14 39 18 32 42 -4 14 -7 71 -7 126 0 92 3 106 32 167 30 65 100 146 168 195 l30 22 -465 0 c-383 0 -470 -2 -491 -14z""/><path d=""M645 723 c-65 -6 -142 -27 -186 -49 -57 -28 -109 -86 -109 -120 0 -15 -11 -34 -26 -47 -14 -12 -40 -34 -57 -48 l-30 -26 27 -109 c15 -60 34 -119 42 -132 19 -29 344 -192 384 -192 43 0 365 163 386 195 22 34 67 213 60 241 -3 12 -26 35 -51 51 -25 15 -45 34 -45 42 0 36 -42 98 -87 128 -67 46 -209 76 -308 66z m104 -358 l53 -56 -7 -52 c-4 -29 -9 -55 -12 -59 -2 -5 -44 -8 -93 -8 -85 0 -90 1 -94 23 -12 60 -13 83 -4 102 12 23 89 105 99 105 3 0 30 -25 58 -55z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'inspector':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M1595 1843 c-39 -43 -89 -97 -111 -121 -38 -41 -40 -42 -72 -31 -56 19 -188 7 -247 -23 -193 -100 -241 -362 -95 -521 55 -60 114 -93 194 -108 152 -28 311 63 367 210 30 81 24 196 -16 275 l-29 57 55 57 c141 147 149 158 149 206 0 54 -24 76 -84 76 -36 0 -45 -6 -111 -77z m-161 -300 c46 -32 96 -118 96 -166 0 -150 -154 -257 -288 -201 -171 71 -176 305 -7 386 58 27 144 19 199 -19z""/><path d=""M189 1745 c-35 -19 -46 -50 -64 -179 -33 -237 -31 -239 180 -302 83 -24 166 -49 186 -54 31 -9 37 -15 42 -46 6 -33 2 -40 -49 -89 -64 -63 -108 -151 -123 -245 -13 -76 -14 -153 -3 -148 4 2 41 20 82 41 94 47 169 61 290 54 102 -5 179 -27 248 -71 23 -14 44 -26 47 -26 11 0 4 129 -10 185 -23 88 -54 143 -112 204 -43 44 -53 62 -53 89 0 30 4 35 40 49 36 14 39 18 32 42 -4 14 -7 71 -7 126 0 92 3 106 32 167 30 65 100 146 168 195 l30 22 -465 0 c-383 0 -470 -2 -491 -14z""/><path d=""M645 723 c-65 -6 -142 -27 -186 -49 -57 -28 -109 -86 -109 -120 0 -15 -11 -34 -26 -47 -14 -12 -40 -34 -57 -48 l-30 -26 27 -109 c15 -60 34 -119 42 -132 19 -29 344 -192 384 -192 43 0 365 163 386 195 22 34 67 213 60 241 -3 12 -26 35 -51 51 -25 15 -45 34 -45 42 0 36 -42 98 -87 128 -67 46 -209 76 -308 66z m104 -358 l53 -56 -7 -52 c-4 -29 -9 -55 -12 -59 -2 -5 -44 -8 -93 -8 -85 0 -90 1 -94 23 -12 60 -13 83 -4 102 12 23 89 105 99 105 3 0 30 -25 58 -55z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'payment':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 427.479 427.479""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""> <g> <path d=""M287.006,299.735c-6.383,0-11.576-5.193-11.576-11.576s5.193-11.575,11.576-11.575h19.075v-15h-11.58v-18.475h-15v19.561 c-11.01,3.247-19.071,13.441-19.071,25.489c0,14.654,11.922,26.576,26.576,26.576c6.383,0,11.575,5.192,11.575,11.575 s-5.192,11.576-11.575,11.576H267.93v15h11.571v18.479h15v-19.562c11.014-3.245,19.08-13.441,19.08-25.492 C313.581,311.657,301.66,299.735,287.006,299.735z""/> <rect x=""121.397"" y=""337.886"" width=""94.495"" height=""15""/> <rect x=""121.397"" y=""313.484"" width=""94.495"" height=""15""/> <rect x=""150.089"" y=""64.497"" width=""127.3"" height=""15""/> <rect x=""121.397"" y=""137.993"" width=""184.684"" height=""15""/> <rect x=""121.397"" y=""164.991"" width=""184.684"" height=""15""/> <rect x=""121.397"" y=""191.99"" width=""184.684"" height=""15""/> <path d=""M347.982,0v11.999h-16.499V0h-15v11.999h-16.499V0h-15v11.999h-16.498V0h-15v11.999h-16.499V0h-15v11.999H205.49V0h-15 v11.999h-16.499V0h-15v11.999h-16.498V0h-15v11.999h-16.499V0h-15v11.999H79.496V0h-15v427.479h15v-11.999h16.499v11.999h15 v-11.999h16.499v11.999h15v-11.999h16.498v11.999h15v-11.999h16.499v11.999h15v-11.999h16.499v11.999h15v-11.999h16.499v11.999h15 v-11.999h16.498v11.999h15v-11.999h16.499v11.999h15v-11.999h16.499v11.999h15V0H347.982z M79.496,26.999h268.486v373.48H79.496 V26.999z""/> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'PAG':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><title>Pago de pedimento</title><g id=""layer101""  stroke=""none""><path d=""M210 960 l0 -960 606 0 606 0 134 128 134 127 0 833 0 832 -740 0 -740 0 0 -960z m1340 105 l0 -725 -105 0 -104 0 -3 -102 -3 -103 -492 -3 -493 -2 0 830 0 830 600 0 600 0 0 -725z""/><path d=""M880 1366 c0 -33 -2 -35 -55 -51 -67 -19 -129 -76 -144 -132 -20 -72 -19 -73 59 -73 l70 0 14 34 c8 19 24 39 35 46 21 10 21 9 21 -72 l0 -83 -59 -17 c-96 -28 -141 -89 -141 -188 1 -82 69 -166 153 -190 47 -13 47 -13 47 -56 l0 -44 70 0 70 0 0 44 0 43 65 32 c71 35 107 81 120 154 l7 37 -74 0 -74 0 -13 -35 c-20 -57 -32 -42 -29 36 l3 71 58 18 c78 24 105 44 128 93 56 124 -19 261 -158 292 -29 6 -33 10 -33 41 l0 34 -70 0 -70 0 0 -34z m180 -186 c27 -27 26 -75 -2 -94 -12 -9 -26 -16 -30 -16 -5 0 -8 29 -8 65 0 69 7 78 40 45z m-182 -386 l-3 -36 -27 22 c-17 12 -28 31 -28 45 0 14 11 33 28 45 l27 22 3 -31 c2 -17 2 -47 0 -67z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'payment2':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M210 960 l0 -960 606 0 606 0 134 128 134 127 0 833 0 832 -740 0 -740 0 0 -960z m1340 105 l0 -725 -105 0 -104 0 -3 -102 -3 -103 -492 -3 -493 -2 0 830 0 830 600 0 600 0 0 -725z""/><path d=""M880 1366 c0 -33 -2 -35 -55 -51 -67 -19 -129 -76 -144 -132 -20 -72 -19 -73 59 -73 l70 0 14 34 c8 19 24 39 35 46 21 10 21 9 21 -72 l0 -83 -59 -17 c-96 -28 -141 -89 -141 -188 1 -82 69 -166 153 -190 47 -13 47 -13 47 -56 l0 -44 70 0 70 0 0 44 0 43 65 32 c71 35 107 81 120 154 l7 37 -74 0 -74 0 -13 -35 c-20 -57 -32 -42 -29 36 l3 71 58 18 c78 24 105 44 128 93 56 124 -19 261 -158 292 -29 6 -33 10 -33 41 l0 34 -70 0 -70 0 0 -34z m180 -186 c27 -27 26 -75 -2 -94 -12 -9 -26 -16 -30 -16 -5 0 -8 29 -8 65 0 69 7 78 40 45z m-182 -386 l-3 -36 -27 22 c-17 12 -28 31 -28 45 0 14 11 33 28 45 l27 22 3 -31 c2 -17 2 -47 0 -67z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'PRE':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><title>Previo</title><g id=""layer101""  stroke=""none""><path d=""M177 1852 c-10 -10 -17 -35 -17 -55 0 -30 10 -48 58 -104 32 -38 66 -74 76 -82 9 -8 26 -27 37 -43 l20 -28 -28 -58 c-36 -74 -39 -190 -6 -264 32 -69 94 -131 164 -163 56 -25 70 -27 148 -23 102 5 151 27 217 94 113 117 113 321 0 438 -65 66 -116 89 -213 94 -45 2 -95 0 -112 -6 -29 -10 -32 -8 -136 104 -104 111 -108 114 -149 114 -29 0 -48 -6 -59 -18z m523 -327 c35 -18 87 -75 99 -108 14 -36 14 -108 0 -144 -17 -47 -79 -102 -132 -118 -147 -44 -288 95 -247 244 15 55 83 124 136 139 44 12 107 6 144 -13z""/><path d=""M978 1468 c29 -87 27 -171 -8 -260 -25 -67 -100 -154 -164 -192 -50 -29 -143 -56 -196 -56 -56 0 -151 28 -202 60 l-48 29 0 -188 c0 -139 -3 -191 -12 -198 -7 -5 -79 -33 -160 -62 -80 -29 -156 -60 -167 -68 -37 -25 -36 -28 129 -423 23 -57 30 -65 56 -68 16 -1 103 22 194 53 l164 55 396 0 396 0 164 -55 c91 -31 178 -54 195 -53 28 3 32 10 118 218 58 141 87 225 85 244 -3 29 -8 32 -168 90 -91 34 -171 65 -177 69 -10 6 -13 103 -13 426 0 392 -1 419 -18 434 -16 15 -52 17 -304 17 l-284 0 24 -72z m388 -1020 c-22 -79 -45 -149 -49 -155 -12 -19 -702 -19 -714 0 -4 6 -27 76 -49 155 l-41 142 447 0 447 0 -41 -142z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'previo':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M177 1852 c-10 -10 -17 -35 -17 -55 0 -30 10 -48 58 -104 32 -38 66 -74 76 -82 9 -8 26 -27 37 -43 l20 -28 -28 -58 c-36 -74 -39 -190 -6 -264 32 -69 94 -131 164 -163 56 -25 70 -27 148 -23 102 5 151 27 217 94 113 117 113 321 0 438 -65 66 -116 89 -213 94 -45 2 -95 0 -112 -6 -29 -10 -32 -8 -136 104 -104 111 -108 114 -149 114 -29 0 -48 -6 -59 -18z m523 -327 c35 -18 87 -75 99 -108 14 -36 14 -108 0 -144 -17 -47 -79 -102 -132 -118 -147 -44 -288 95 -247 244 15 55 83 124 136 139 44 12 107 6 144 -13z""/><path d=""M978 1468 c29 -87 27 -171 -8 -260 -25 -67 -100 -154 -164 -192 -50 -29 -143 -56 -196 -56 -56 0 -151 28 -202 60 l-48 29 0 -188 c0 -139 -3 -191 -12 -198 -7 -5 -79 -33 -160 -62 -80 -29 -156 -60 -167 -68 -37 -25 -36 -28 129 -423 23 -57 30 -65 56 -68 16 -1 103 22 194 53 l164 55 396 0 396 0 164 -55 c91 -31 178 -54 195 -53 28 3 32 10 118 218 58 141 87 225 85 244 -3 29 -8 32 -168 90 -91 34 -171 65 -177 69 -10 6 -13 103 -13 426 0 392 -1 419 -18 434 -16 15 -52 17 -304 17 l-284 0 24 -72z m388 -1020 c-22 -79 -45 -149 -49 -155 -12 -19 -702 -19 -714 0 -4 6 -27 76 -49 155 l-41 142 447 0 447 0 -41 -142z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'previo2':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 512 512""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xmlns:xlink=""http://www.w3.org/1999/xlink"" enable-background=""new 0 0 512 512""> <g> <g> <polygon points=""256,11 33.7,76.6 256,152.1 478.3,76.6   ""/> <path d=""M13.1,350.3c0,8.7,5.5,16.4,13.7,19.2l208.9,71V188L13.1,112.3V350.3z""/> <path d=""m465.7,431.1c-4.9,4.7-10.2,8.9-16.1,12.5-5.9,3.6-12.1,6.3-18.5,8.5l20.2,39.2c8.7,13.7 23.1,9.6 27.8,6.6 9.5-5.9 12.5-18.3 6.6-27.8l-20-39z""/> <circle cx=""400.5"" cy=""363.7"" r=""56.7""/> <path d=""m276.2,188v252.5c0,0 44.7-15.8 46.6-17.6-12.6-16.4-20-37-20-59.2 0-53.9 43.8-97.7 97.7-97.7 51.4,0 93.6,39.9 97.4,90.3 0.6-1.9 1-3.9 1-6v-238l-222.7,75.7z""/> </g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'checklist':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M500 1420 l0 -50 40 0 c29 0 40 4 40 15 0 13 43 15 330 15 l330 0 0 -395 0 -395 -60 0 -60 0 0 -55 0 -55 -270 0 -270 0 0 115 0 115 -40 0 -40 0 0 -150 0 -150 337 0 338 0 72 72 73 72 0 448 0 448 -410 0 -410 0 0 -50z""/><path d=""M315 1320 c-45 -5 -108 -18 -140 -30 -33 -12 -80 -25 -105 -31 -25 -5 -51 -17 -57 -26 -9 -12 -13 -70 -13 -204 0 -183 1 -188 22 -203 12 -9 34 -16 48 -16 18 0 68 -31 150 -92 105 -79 129 -93 161 -93 65 0 110 64 88 122 l-9 23 121 0 c85 0 128 4 148 15 30 15 55 62 49 91 -2 11 9 30 27 45 24 22 30 35 30 69 0 34 -6 47 -30 68 -21 18 -29 32 -27 52 4 31 -40 90 -66 90 -13 0 -17 9 -18 36 0 24 -9 43 -27 62 -27 26 -31 27 -150 29 -67 0 -158 -3 -202 -7z m303 -87 c-3 -15 -18 -19 -87 -23 -65 -4 -86 -9 -95 -22 -8 -13 -8 -23 0 -35 9 -15 31 -19 135 -23 107 -4 124 -7 127 -22 3 -16 -10 -18 -124 -20 -108 -3 -128 -6 -138 -20 -8 -13 -8 -23 0 -35 10 -16 33 -19 165 -23 136 -4 154 -7 157 -22 3 -17 -9 -18 -147 -18 -123 0 -152 -3 -165 -16 -19 -19 -20 -38 -3 -52 7 -7 66 -13 133 -14 111 -3 119 -4 119 -23 0 -19 -8 -20 -175 -25 -170 -5 -175 -6 -178 -27 -2 -12 9 -35 27 -56 17 -19 31 -40 31 -46 0 -26 -38 -6 -149 79 -92 70 -124 90 -149 90 l-33 0 3 152 3 153 35 5 c19 4 46 12 58 19 48 26 150 39 300 40 141 1 153 0 150 -16z""/><path d=""M1360 1291 l0 -39 148 -4 c119 -4 158 -9 207 -27 33 -12 76 -26 95 -29 l35 -7 3 -153 3 -152 -33 0 c-25 0 -57 -20 -149 -90 -111 -85 -149 -105 -149 -79 0 6 14 27 31 46 18 21 29 44 27 56 -3 21 -9 22 -110 25 l-108 3 0 -36 0 -35 50 0 c48 0 49 -1 41 -23 -22 -58 23 -122 88 -122 32 0 56 14 161 93 82 61 132 92 150 92 14 0 36 7 48 16 21 15 22 20 22 203 0 134 -4 192 -12 204 -7 9 -33 21 -58 26 -25 6 -71 19 -103 31 -60 21 -203 39 -319 40 l-68 0 0 -39z""/><path d=""M734 1255 c-16 -41 -4 -44 203 -47 109 -2 206 1 216 5 11 5 17 17 15 30 -3 22 -6 22 -216 25 -183 2 -213 0 -218 -13z""/><path d=""M1360 1170 l0 -41 56 3 c64 3 88 23 68 56 -8 12 -27 18 -68 20 l-56 3 0 -41z""/><path d=""M830 1110 l0 -30 169 0 c104 0 172 4 176 10 3 6 3 19 0 30 -6 19 -15 20 -176 20 l-169 0 0 -30z""/><path d=""M1360 1050 l0 -41 56 3 c64 3 88 23 68 56 -8 12 -27 18 -68 20 l-56 3 0 -41z""/><path d=""M902 1001 c-8 -4 -12 -19 -10 -32 3 -24 4 -24 140 -27 131 -2 137 -2 143 18 13 42 -9 50 -139 50 -66 0 -127 -4 -134 -9z""/><path d=""M1360 930 l0 -40 53 0 c71 0 98 28 61 64 -10 10 -33 16 -65 16 l-49 0 0 -40z""/><path d=""M844 866 c-3 -8 -4 -23 -2 -33 3 -16 18 -18 159 -21 168 -3 188 3 174 48 -6 19 -14 20 -166 20 -130 0 -161 -3 -165 -14z""/><path d=""M668 728 c-3 -7 -3 -21 0 -31 4 -16 23 -17 256 -15 l251 3 0 25 0 25 -252 3 c-196 2 -252 0 -255 -10z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'RGU':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><title>Revalidación</title><g id=""layer101""  stroke=""none""><path d=""M500 1420 l0 -50 40 0 c29 0 40 4 40 15 0 13 43 15 330 15 l330 0 0 -395 0 -395 -60 0 -60 0 0 -55 0 -55 -270 0 -270 0 0 115 0 115 -40 0 -40 0 0 -150 0 -150 337 0 338 0 72 72 73 72 0 448 0 448 -410 0 -410 0 0 -50z""/><path d=""M315 1320 c-45 -5 -108 -18 -140 -30 -33 -12 -80 -25 -105 -31 -25 -5 -51 -17 -57 -26 -9 -12 -13 -70 -13 -204 0 -183 1 -188 22 -203 12 -9 34 -16 48 -16 18 0 68 -31 150 -92 105 -79 129 -93 161 -93 65 0 110 64 88 122 l-9 23 121 0 c85 0 128 4 148 15 30 15 55 62 49 91 -2 11 9 30 27 45 24 22 30 35 30 69 0 34 -6 47 -30 68 -21 18 -29 32 -27 52 4 31 -40 90 -66 90 -13 0 -17 9 -18 36 0 24 -9 43 -27 62 -27 26 -31 27 -150 29 -67 0 -158 -3 -202 -7z m303 -87 c-3 -15 -18 -19 -87 -23 -65 -4 -86 -9 -95 -22 -8 -13 -8 -23 0 -35 9 -15 31 -19 135 -23 107 -4 124 -7 127 -22 3 -16 -10 -18 -124 -20 -108 -3 -128 -6 -138 -20 -8 -13 -8 -23 0 -35 10 -16 33 -19 165 -23 136 -4 154 -7 157 -22 3 -17 -9 -18 -147 -18 -123 0 -152 -3 -165 -16 -19 -19 -20 -38 -3 -52 7 -7 66 -13 133 -14 111 -3 119 -4 119 -23 0 -19 -8 -20 -175 -25 -170 -5 -175 -6 -178 -27 -2 -12 9 -35 27 -56 17 -19 31 -40 31 -46 0 -26 -38 -6 -149 79 -92 70 -124 90 -149 90 l-33 0 3 152 3 153 35 5 c19 4 46 12 58 19 48 26 150 39 300 40 141 1 153 0 150 -16z""/><path d=""M1360 1291 l0 -39 148 -4 c119 -4 158 -9 207 -27 33 -12 76 -26 95 -29 l35 -7 3 -153 3 -152 -33 0 c-25 0 -57 -20 -149 -90 -111 -85 -149 -105 -149 -79 0 6 14 27 31 46 18 21 29 44 27 56 -3 21 -9 22 -110 25 l-108 3 0 -36 0 -35 50 0 c48 0 49 -1 41 -23 -22 -58 23 -122 88 -122 32 0 56 14 161 93 82 61 132 92 150 92 14 0 36 7 48 16 21 15 22 20 22 203 0 134 -4 192 -12 204 -7 9 -33 21 -58 26 -25 6 -71 19 -103 31 -60 21 -203 39 -319 40 l-68 0 0 -39z""/><path d=""M734 1255 c-16 -41 -4 -44 203 -47 109 -2 206 1 216 5 11 5 17 17 15 30 -3 22 -6 22 -216 25 -183 2 -213 0 -218 -13z""/><path d=""M1360 1170 l0 -41 56 3 c64 3 88 23 68 56 -8 12 -27 18 -68 20 l-56 3 0 -41z""/><path d=""M830 1110 l0 -30 169 0 c104 0 172 4 176 10 3 6 3 19 0 30 -6 19 -15 20 -176 20 l-169 0 0 -30z""/><path d=""M1360 1050 l0 -41 56 3 c64 3 88 23 68 56 -8 12 -27 18 -68 20 l-56 3 0 -41z""/><path d=""M902 1001 c-8 -4 -12 -19 -10 -32 3 -24 4 -24 140 -27 131 -2 137 -2 143 18 13 42 -9 50 -139 50 -66 0 -127 -4 -134 -9z""/><path d=""M1360 930 l0 -40 53 0 c71 0 98 28 61 64 -10 10 -33 16 -65 16 l-49 0 0 -40z""/><path d=""M844 866 c-3 -8 -4 -23 -2 -33 3 -16 18 -18 159 -21 168 -3 188 3 174 48 -6 19 -14 20 -166 20 -130 0 -161 -3 -165 -14z""/><path d=""M668 728 c-3 -7 -3 -21 0 -31 4 -16 23 -17 256 -15 l251 3 0 25 0 25 -252 3 c-196 2 -252 0 -255 -10z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'REV':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><title>Revalidación</title><g id=""layer101""  stroke=""none""><path d=""M500 1420 l0 -50 40 0 c29 0 40 4 40 15 0 13 43 15 330 15 l330 0 0 -395 0 -395 -60 0 -60 0 0 -55 0 -55 -270 0 -270 0 0 115 0 115 -40 0 -40 0 0 -150 0 -150 337 0 338 0 72 72 73 72 0 448 0 448 -410 0 -410 0 0 -50z""/><path d=""M315 1320 c-45 -5 -108 -18 -140 -30 -33 -12 -80 -25 -105 -31 -25 -5 -51 -17 -57 -26 -9 -12 -13 -70 -13 -204 0 -183 1 -188 22 -203 12 -9 34 -16 48 -16 18 0 68 -31 150 -92 105 -79 129 -93 161 -93 65 0 110 64 88 122 l-9 23 121 0 c85 0 128 4 148 15 30 15 55 62 49 91 -2 11 9 30 27 45 24 22 30 35 30 69 0 34 -6 47 -30 68 -21 18 -29 32 -27 52 4 31 -40 90 -66 90 -13 0 -17 9 -18 36 0 24 -9 43 -27 62 -27 26 -31 27 -150 29 -67 0 -158 -3 -202 -7z m303 -87 c-3 -15 -18 -19 -87 -23 -65 -4 -86 -9 -95 -22 -8 -13 -8 -23 0 -35 9 -15 31 -19 135 -23 107 -4 124 -7 127 -22 3 -16 -10 -18 -124 -20 -108 -3 -128 -6 -138 -20 -8 -13 -8 -23 0 -35 10 -16 33 -19 165 -23 136 -4 154 -7 157 -22 3 -17 -9 -18 -147 -18 -123 0 -152 -3 -165 -16 -19 -19 -20 -38 -3 -52 7 -7 66 -13 133 -14 111 -3 119 -4 119 -23 0 -19 -8 -20 -175 -25 -170 -5 -175 -6 -178 -27 -2 -12 9 -35 27 -56 17 -19 31 -40 31 -46 0 -26 -38 -6 -149 79 -92 70 -124 90 -149 90 l-33 0 3 152 3 153 35 5 c19 4 46 12 58 19 48 26 150 39 300 40 141 1 153 0 150 -16z""/><path d=""M1360 1291 l0 -39 148 -4 c119 -4 158 -9 207 -27 33 -12 76 -26 95 -29 l35 -7 3 -153 3 -152 -33 0 c-25 0 -57 -20 -149 -90 -111 -85 -149 -105 -149 -79 0 6 14 27 31 46 18 21 29 44 27 56 -3 21 -9 22 -110 25 l-108 3 0 -36 0 -35 50 0 c48 0 49 -1 41 -23 -22 -58 23 -122 88 -122 32 0 56 14 161 93 82 61 132 92 150 92 14 0 36 7 48 16 21 15 22 20 22 203 0 134 -4 192 -12 204 -7 9 -33 21 -58 26 -25 6 -71 19 -103 31 -60 21 -203 39 -319 40 l-68 0 0 -39z""/><path d=""M734 1255 c-16 -41 -4 -44 203 -47 109 -2 206 1 216 5 11 5 17 17 15 30 -3 22 -6 22 -216 25 -183 2 -213 0 -218 -13z""/><path d=""M1360 1170 l0 -41 56 3 c64 3 88 23 68 56 -8 12 -27 18 -68 20 l-56 3 0 -41z""/><path d=""M830 1110 l0 -30 169 0 c104 0 172 4 176 10 3 6 3 19 0 30 -6 19 -15 20 -176 20 l-169 0 0 -30z""/><path d=""M1360 1050 l0 -41 56 3 c64 3 88 23 68 56 -8 12 -27 18 -68 20 l-56 3 0 -41z""/><path d=""M902 1001 c-8 -4 -12 -19 -10 -32 3 -24 4 -24 140 -27 131 -2 137 -2 143 18 13 42 -9 50 -139 50 -66 0 -127 -4 -134 -9z""/><path d=""M1360 930 l0 -40 53 0 c71 0 98 28 61 64 -10 10 -33 16 -65 16 l-49 0 0 -40z""/><path d=""M844 866 c-3 -8 -4 -23 -2 -33 3 -16 18 -18 159 -21 168 -3 188 3 174 48 -6 19 -14 20 -166 20 -130 0 -161 -3 -165 -14z""/><path d=""M668 728 c-3 -7 -3 -21 0 -31 4 -16 23 -17 256 -15 l251 3 0 25 0 25 -252 3 c-196 2 -252 0 -255 -10z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ship':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 480.002 480.002""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""> <g> <g> <path d=""M472.002,264.001h-48v-152c0-4.418-3.582-8-8-8h-24V88.289c-0.018-13.407-10.881-24.27-24.288-24.288h-31.712v16h31.712 c4.576,0.004,8.284,3.712,8.288,8.288v76.4l-16,16l-18.344-18.344c-3.124-3.123-8.188-3.123-11.312,0l-10.344,10.344V88.001 c0-13.255-10.745-24-24-24s-24,10.745-24,24h16c0-4.418,3.582-8,8-8s8,3.582,8,8v16h-48c-4.418,0-8,3.582-8,8v152h-20.688 l-37.656-37.656c-1.5-1.5-3.534-2.344-5.656-2.344h-48v-88c0-4.418-3.582-8-8-8h-72c-4.418,0-8,3.582-8,8v88h-40 C3.584,224,0.001,227.581,0,231.999c0,0.934,0.163,1.861,0.482,2.738l54.28,149.264h-22.76v16h28.576l3.904,10.736 c1.151,3.161,4.156,5.265,7.52,5.264h400c4.418,0,8-3.582,8-8v-136C480.002,267.583,476.421,264.001,472.002,264.001z  M264.002,120.001h40v72c0.001,4.418,3.583,7.999,8.002,7.998c2.121,0,4.154-0.843,5.654-2.342l18.344-18.344l18.344,18.344 c3.124,3.123,8.188,3.123,11.312,0l24-24c1.5-1.5,2.344-3.534,2.344-5.656v-48h16v144h-144V120.001z M64.002,144.001h56v16h-56 V144.001z M64.002,176.001h56v48h-56V176.001z M464.002,400.001h-386.4l-11.632-32h398.032V400.001z M464.002,352.001H60.146 l-40.72-112h36.576H180.69l37.656,37.656c1.5,1.5,3.534,2.344,5.656,2.344h240V352.001z""/> </g> </g> <g> <g> <path d=""M96.002,272.001c-13.255,0-24,10.745-24,24s10.745,24,24,24s24-10.745,24-24S109.257,272.001,96.002,272.001z  M96.002,304.001c-4.418,0-8-3.582-8-8s3.582-8,8-8s8,3.582,8,8S100.421,304.001,96.002,304.001z""/> </g> </g> <g> <g> <rect x=""368.002"" y=""312.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""400.002"" y=""312.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""384.002"" y=""296.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""416.002"" y=""296.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""280.002"" y=""232.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""312.002"" y=""232.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""0.002"" y=""384.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""368.002"" y=""432.001"" width=""48"" height=""16""/> </g> </g> <g> <g> <rect x=""432.002"" y=""432.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""176.002"" y=""448.001"" width=""40"" height=""16""/> </g> </g> <g> <g> <rect x=""232.002"" y=""448.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""264.002"" y=""448.001"" width=""40"" height=""16""/> </g> </g> <g> <g> <path d=""M180.002,16.001h-20v16h20c6.627,0,12,5.373,12,12s-5.373,12-12,12h-76.248c-26.36,0.031-47.721,21.392-47.752,47.752 v8.248h16v-8.248c0.022-17.527,14.225-31.73,31.752-31.752h76.248c15.464,0,28-12.536,28-28S195.466,16.001,180.002,16.001z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'DOC':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><title>Recepción de documentos</title><g id=""layer101""  stroke=""none""><path d=""M180 1175 l0 -745 468 0 467 0 108 103 107 102 0 643 0 642 -575 0 -575 0 0 -745z m1040 80 l0 -555 -85 0 -85 0 0 -80 0 -80 -380 0 -380 0 0 635 0 635 465 0 465 0 0 -555z""/><path d=""M582 1488 c-32 -32 -14 -61 115 -190 l128 -128 -128 -128 c-96 -96 -127 -134 -127 -153 0 -32 17 -49 50 -49 36 0 330 294 330 330 0 19 -37 62 -153 178 -155 155 -182 173 -215 140z""/><path d=""M1370 1645 c0 -48 2 -55 20 -55 20 0 20 -7 20 -560 l0 -560 -80 0 -80 0 0 -80 0 -80 -385 0 -385 0 0 35 0 35 -55 0 -55 0 0 -90 0 -90 468 0 467 0 108 103 107 102 0 648 0 647 -75 0 -75 0 0 -55z""/><path d=""M1570 1435 c0 -48 2 -55 20 -55 20 0 20 -7 20 -560 l0 -560 -80 0 -80 0 0 -75 0 -75 -385 0 -385 0 0 35 0 35 -55 0 -55 0 0 -90 0 -90 473 0 472 0 103 96 102 96 0 649 0 649 -75 0 -75 0 0 -55z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'files':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M180 1175 l0 -745 468 0 467 0 108 103 107 102 0 643 0 642 -575 0 -575 0 0 -745z m1040 80 l0 -555 -85 0 -85 0 0 -80 0 -80 -380 0 -380 0 0 635 0 635 465 0 465 0 0 -555z""/><path d=""M582 1488 c-32 -32 -14 -61 115 -190 l128 -128 -128 -128 c-96 -96 -127 -134 -127 -153 0 -32 17 -49 50 -49 36 0 330 294 330 330 0 19 -37 62 -153 178 -155 155 -182 173 -215 140z""/><path d=""M1370 1645 c0 -48 2 -55 20 -55 20 0 20 -7 20 -560 l0 -560 -80 0 -80 0 0 -80 0 -80 -385 0 -385 0 0 35 0 35 -55 0 -55 0 0 -90 0 -90 468 0 467 0 108 103 107 102 0 648 0 647 -75 0 -75 0 0 -55z""/><path d=""M1570 1435 c0 -48 2 -55 20 -55 20 0 20 -7 20 -560 l0 -560 -80 0 -80 0 0 -75 0 -75 -385 0 -385 0 0 35 0 35 -55 0 -55 0 0 -90 0 -90 473 0 472 0 103 96 102 96 0 649 0 649 -75 0 -75 0 0 -55z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ATA':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 512 512"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" xml:space=""preserve""> <title>Fecha de entrada</title> <g> <g> <path d=""M378.24,243.712l-96-80c-4.768-3.968-11.424-4.832-17.024-2.208C259.584,164.128,256,169.792,256,176v48H16 c-8.832,0-16,7.168-16,16v32c0,8.832,7.168,16,16,16h240v48c0,6.208,3.584,11.84,9.216,14.496c2.144,0.992,4.48,1.504,6.784,1.504 c3.68,0,7.328-1.248,10.24-3.712l96-80c3.68-3.04,5.76-7.552,5.76-12.288C384,251.264,381.92,246.752,378.24,243.712z""/> </g> </g> <g> <g> <path d=""M480,0H32C14.336,0,0,14.336,0,32v160h64V64h384v384H64V320H0v160c0,17.696,14.336,32,32,32h448c17.696,0,32-14.304,32-32 V32C512,14.336,497.696,0,480,0z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'entry':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 512 512"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" xml:space=""preserve""> <g> <g> <path d=""M378.24,243.712l-96-80c-4.768-3.968-11.424-4.832-17.024-2.208C259.584,164.128,256,169.792,256,176v48H16 c-8.832,0-16,7.168-16,16v32c0,8.832,7.168,16,16,16h240v48c0,6.208,3.584,11.84,9.216,14.496c2.144,0.992,4.48,1.504,6.784,1.504 c3.68,0,7.328-1.248,10.24-3.712l96-80c3.68-3.04,5.76-7.552,5.76-12.288C384,251.264,381.92,246.752,378.24,243.712z""/> </g> </g> <g> <g> <path d=""M480,0H32C14.336,0,0,14.336,0,32v160h64V64h384v384H64V320H0v160c0,17.696,14.336,32,32,32h448c17.696,0,32-14.304,32-32 V32C512,14.336,497.696,0,480,0z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'export':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 512 512""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""> <g> <g> <path d=""M507.296,4.704c-3.36-3.36-8.032-5.056-12.768-4.64L370.08,11.392c-6.176,0.576-11.488,4.672-13.6,10.496 s-0.672,12.384,3.712,16.768l33.952,33.952L224.448,242.272c-6.24,6.24-6.24,16.384,0,22.624l22.624,22.624 c6.272,6.272,16.384,6.272,22.656,0.032l169.696-169.696l33.952,33.952c4.384,4.384,10.912,5.824,16.768,3.744 c2.24-0.832,4.224-2.112,5.856-3.744c2.592-2.592,4.288-6.048,4.608-9.888l11.328-124.448 C512.352,12.736,510.656,8.064,507.296,4.704z""/> </g> </g> <g> <g> <path d=""M448,192v256H64V64h256V0H32C14.304,0,0,14.304,0,32v448c0,17.664,14.304,32,32,32h448c17.664,0,32-14.336,32-32V192H448z ""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'import':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 512 512""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""> <g> <g> <path d=""M287.52,224.48c-3.36-3.36-8-5.088-12.736-4.64l-124.448,11.296c-6.176,0.576-11.52,4.672-13.6,10.496 c-2.112,5.856-0.672,12.384,3.712,16.768l33.952,33.952L4.704,462.048c-6.24,6.24-6.24,16.384,0,22.624l22.624,22.624 c6.24,6.272,16.352,6.272,22.624,0L219.648,337.6l33.952,33.952c4.384,4.384,10.912,5.824,16.768,3.744 c2.24-0.832,4.224-2.112,5.856-3.744c2.592-2.592,4.288-6.048,4.608-9.888l11.328-124.448 C292.608,232.48,290.88,227.84,287.52,224.48z""/> </g> </g> <g> <g> <path d=""M480,0H32C14.336,0,0,14.336,0,32v320h64V64h384v384H160v64h320c17.696,0,32-14.304,32-32V32C512,14.336,497.696,0,480,0z ""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'icon-pdf':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Layer_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 512 512""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""> <path style=""fill:#E2E5E7;"" d=""M128,0c-17.6,0-32,14.4-32,32v448c0,17.6,14.4,32,32,32h320c17.6,0,32-14.4,32-32V128L352,0H128z""/> <path style=""fill:#B0B7BD;"" d=""M384,128h96L352,0v96C352,113.6,366.4,128,384,128z""/> <polygon style=""fill:#CAD1D8;"" points=""480,224 384,128 480,128 ""/> <path style=""fill:#50BEE8;"" d=""M416,416c0,8.8-7.2,16-16,16H48c-8.8,0-16-7.2-16-16V256c0-8.8,7.2-16,16-16h352c8.8,0,16,7.2,16,16 V416z""/> <g> <path style=""fill:#FFFFFF;"" d=""M131.28,326.176l22.272-27.888c6.64-8.688,19.568,2.432,12.288,10.752 c-7.664,9.088-15.728,18.944-23.424,29.024l26.112,32.496c7.024,9.6-7.04,18.816-13.952,9.344l-23.536-30.192l-23.152,30.832 c-6.528,9.328-20.992-1.152-13.68-9.856l25.712-32.624c-8.064-10.096-15.872-19.936-23.664-29.024 c-8.064-9.6,6.912-19.44,12.784-10.48L131.28,326.176z""/> <path style=""fill:#FFFFFF;"" d=""M201.264,327.84v47.328c0,5.648-4.608,8.832-9.2,8.832c-4.096,0-7.68-3.184-7.68-8.832v-72.016 c0-6.656,5.648-8.848,7.68-8.848c3.696,0,5.872,2.192,8.048,4.624l28.16,37.984l29.152-39.408c4.24-5.232,14.592-3.2,14.592,5.648 v72.016c0,5.648-3.584,8.832-7.664,8.832c-4.608,0-8.192-3.184-8.192-8.832V327.84l-21.248,26.864 c-4.592,5.648-10.352,5.648-14.576,0L201.264,327.84z""/> <path style=""fill:#FFFFFF;"" d=""M294.288,303.152c0-4.224,3.584-7.808,8.064-7.808c4.096,0,7.552,3.6,7.552,7.808v64.096h34.8 c12.528,0,12.8,16.752,0,16.752h-42.336c-4.48,0-8.064-3.184-8.064-7.808v-73.04H294.288z""/> </g> <path style=""fill:#CAD1D8;"" d=""M400,432H96v16h304c8.8,0,16-7.2,16-16v-16C416,424.8,408.8,432,400,432z""/> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'icon-xml':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Layer_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 512 512""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""> <path style=""fill:#E2E5E7;"" d=""M128,0c-17.6,0-32,14.4-32,32v448c0,17.6,14.4,32,32,32h320c17.6,0,32-14.4,32-32V128L352,0H128z""/> <path style=""fill:#B0B7BD;"" d=""M384,128h96L352,0v96C352,113.6,366.4,128,384,128z""/> <polygon style=""fill:#CAD1D8;"" points=""480,224 384,128 480,128 ""/> <path style=""fill:#50BEE8;"" d=""M416,416c0,8.8-7.2,16-16,16H48c-8.8,0-16-7.2-16-16V256c0-8.8,7.2-16,16-16h352c8.8,0,16,7.2,16,16 V416z""/> <g> <path style=""fill:#FFFFFF;"" d=""M131.28,326.176l22.272-27.888c6.64-8.688,19.568,2.432,12.288,10.752 c-7.664,9.088-15.728,18.944-23.424,29.024l26.112,32.496c7.024,9.6-7.04,18.816-13.952,9.344l-23.536-30.192l-23.152,30.832 c-6.528,9.328-20.992-1.152-13.68-9.856l25.712-32.624c-8.064-10.096-15.872-19.936-23.664-29.024 c-8.064-9.6,6.912-19.44,12.784-10.48L131.28,326.176z""/> <path style=""fill:#FFFFFF;"" d=""M201.264,327.84v47.328c0,5.648-4.608,8.832-9.2,8.832c-4.096,0-7.68-3.184-7.68-8.832v-72.016 c0-6.656,5.648-8.848,7.68-8.848c3.696,0,5.872,2.192,8.048,4.624l28.16,37.984l29.152-39.408c4.24-5.232,14.592-3.2,14.592,5.648 v72.016c0,5.648-3.584,8.832-7.664,8.832c-4.608,0-8.192-3.184-8.192-8.832V327.84l-21.248,26.864 c-4.592,5.648-10.352,5.648-14.576,0L201.264,327.84z""/> <path style=""fill:#FFFFFF;"" d=""M294.288,303.152c0-4.224,3.584-7.808,8.064-7.808c4.096,0,7.552,3.6,7.552,7.808v64.096h34.8 c12.528,0,12.8,16.752,0,16.752h-42.336c-4.48,0-8.064-3.184-8.064-7.808v-73.04H294.288z""/> </g> <path style=""fill:#CAD1D8;"" d=""M400,432H96v16h304c8.8,0,16-7.2,16-16v-16C416,424.8,408.8,432,400,432z""/> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'SEA':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" width=""612px"" height=""612px"" viewBox=""0 0 612 612""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""> <g> <path d=""M199.944,36.46c0-20.136,16.323-36.46,36.46-36.46c16.756,0,30.724,11.368,34.99,26.766 c0.497-0.026,0.965-0.148,1.469-0.148c15.137,0,27.407,12.271,27.407,27.408c0,3.692-0.753,7.204-2.076,10.418l0.011-0.001 c9.384,0,16.991,7.606,16.991,16.99c0,9.383-7.606,16.991-16.991,16.991c-9.383,0-16.99-7.606-16.99-16.991 c0-0.507,0.105-0.985,0.149-1.481c-2.691,0.883-5.514,1.481-8.5,1.481c-9.152,0-17.205-4.531-22.183-11.425 c-4.386,1.869-9.208,2.912-14.277,2.912C216.268,72.92,199.944,56.596,199.944,36.46z M216.492,217.077h-49.937 c-8.761,0-15.863,7.102-15.863,15.863v106.207l155.305-74.393l155.311,74.393V232.94c0-8.761-7.102-15.863-15.862-15.863h-49.938  M395.508,201.214v-9.721c0-8.761-7.102-15.863-15.862-15.863h-38.529l-6.889-63.13h-56.455l-6.888,63.13h-38.529 c-8.761,0-15.863,7.102-15.863,15.863v9.721H395.508z M306,282.345L75.247,392.877l68.331,199.338 c31.721-14.751,72.487-28.507,114.275-28.507c67.151,0,121.121,31.984,192.013,48.292h11.775l75.112-219.123L306,282.345z""/> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'AIR':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" width=""510px"" height=""510px"" viewBox=""0 0 510 510""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""> <g> <g id=""flights""> <path d=""M510,255c0-20.4-17.85-38.25-38.25-38.25H331.5L204,12.75h-51l63.75,204H76.5l-38.25-51H0L25.5,255L0,344.25h38.25 l38.25-51h140.25l-63.75,204h51l127.5-204h140.25C492.15,293.25,510,275.4,510,255z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ROA':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" width=""612px"" height=""612px"" viewBox=""0 0 612 612""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""> <g> <path d=""M541.322,500.219v-94.372c0-20.277-16.438-36.716-36.715-36.716h-9.598V24.598c0-3.082-1.547-5.958-4.117-7.657 L467.587,1.537c-6.103-4.033-14.239,0.342-14.239,7.657v110.652l-6.945-18.734c-9.34-25.196-33.373-41.918-60.245-41.918H225.702 c-27.03,0-51.169,16.916-60.394,42.323l-6.655,18.329V9.194c0-7.314-8.137-11.69-14.24-7.657L121.107,16.94 c-2.571,1.699-4.118,4.575-4.118,7.657v344.534h-9.597c-20.277,0-36.715,16.438-36.715,36.716v94.372H55.035 c-5.069,0-9.178,4.109-9.178,9.179v50.743c0,5.069,4.109,9.179,9.178,9.179h39.598v24.322c0,10.139,8.219,18.357,18.358,18.357 h48.645c10.139,0,18.358-8.219,18.358-18.357V569.32h252.014v24.322c0,10.139,8.22,18.357,18.357,18.357h48.646 c10.139,0,18.357-8.219,18.357-18.357V569.32h39.598c5.07,0,9.179-4.11,9.179-9.179v-50.742c0-5.07-4.109-9.179-9.179-9.179 L541.322,500.219L541.322,500.219z M170.814,170.975h270.372v90.44H170.814V170.975z M164.527,474.533H133.17 c-9.581,0-17.348-7.768-17.348-17.349v-0.438c0-9.581,7.767-17.348,17.348-17.348h31.356c9.581,0,17.348,7.767,17.348,17.348v0.438 C181.875,466.765,174.108,474.533,164.527,474.533z M368.398,479.648H243.602c-10.139,0-18.358-8.22-18.358-18.357V344.976 c0-10.138,8.219-18.357,18.358-18.357h124.796c10.138,0,18.357,8.22,18.357,18.357v116.314 C386.756,471.428,378.536,479.648,368.398,479.648z M478.829,474.533h-31.356c-9.58,0-17.348-7.768-17.348-17.349v-0.438 c0-9.581,7.768-17.348,17.348-17.348h31.356c9.581,0,17.349,7.767,17.349,17.348v0.438 C496.178,466.765,488.41,474.533,478.829,474.533z M365.607,393.801H246.099c-5.019,0-9.087-4.068-9.087-9.088v-0.184 c0-5.019,4.068-9.086,9.087-9.086h119.508c5.019,0,9.087,4.067,9.087,9.086v0.184C374.694,389.733,370.626,393.801,365.607,393.801 z M365.607,357.085H246.099c-5.019,0-9.087-4.068-9.087-9.087v-0.184c0-5.018,4.068-9.086,9.087-9.086h119.508 c5.019,0,9.087,4.068,9.087,9.086v0.184C374.694,353.017,370.626,357.085,365.607,357.085z M365.607,467.232H246.099 c-5.019,0-9.087-4.068-9.087-9.087v-0.184c0-5.019,4.068-9.087,9.087-9.087h119.508c5.019,0,9.087,4.068,9.087,9.087v0.184 C374.694,463.164,370.626,467.232,365.607,467.232z M365.607,430.516H246.099c-5.019,0-9.087-4.068-9.087-9.086v-0.184 c0-5.019,4.068-9.087,9.087-9.087h119.508c5.019,0,9.087,4.068,9.087,9.087v0.184C374.694,426.448,370.626,430.516,365.607,430.516 z""/> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'RAI':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" width=""410.121px"" height=""410.121px"" viewBox=""0 0 410.121 410.121""  class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal""xml:space=""preserve""> <g> <g> <path d=""M319.228,116.471c-2.277-0.59-6.785-1.715-6.785-1.715l-0.012-50.163c0-10.677-8.771-20.485-24.299-24.461 c-14.918-3.819-26.834-7.109-49.414-8.599c3.793-3.295,6.203-8.142,6.203-13.562C244.921,8.047,236.874,0,226.947,0 c-9.924,0-17.971,8.046-17.971,17.971c0,4.952,2.004,9.433,5.242,12.684h-18.314c3.238-3.251,5.242-7.732,5.242-12.684 C201.146,8.047,193.102,0,183.175,0c-9.926,0-17.971,8.046-17.971,17.971c0,5.419,2.408,10.267,6.203,13.562 c-22.582,1.49-34.496,4.78-49.414,8.599c-15.527,3.976-24.299,13.784-24.299,24.461l-0.014,50.163c0,0-4.508,1.125-6.783,1.715 c-5.418,1.405-9.199,6.293-9.199,11.889v151.906c0,6.783,5.498,12.281,12.281,12.281h222.162c6.783,0,12.281-5.499,12.281-12.281 V128.359C328.425,122.764,324.644,117.875,319.228,116.471z M125.955,68.351c20.594-5.1,41.52-8.215,62.33-9.282v39.196 c-20.803,1.036-41.721,4.061-62.33,9.012V68.351z M205.472,177.957h-0.818c-14.5,0-26.297-11.797-26.297-26.3 c0-14.501,11.797-26.299,26.297-26.299h0.818c14.502,0,26.299,11.798,26.299,26.299 C231.771,166.16,219.974,177.957,205.472,177.957z M284.171,107.277c-20.607-4.952-41.527-7.976-62.33-9.012V59.069 c20.811,1.067,41.735,4.182,62.33,9.282V107.277L284.171,107.277z""/> <path d=""M232.228,350.645h-27.164h-27.166l-7.121,20.02c11.303,0.963,22.605,1.457,33.877,1.457h0.818 c11.271,0,22.576-0.494,33.877-1.457L232.228,350.645z""/> <path d=""M316.267,348.188v-31.004c0-6.113-4.955-11.066-11.066-11.066H104.924c-6.111,0-11.066,4.955-11.066,11.066v31.004 c0,5.041,3.41,9.447,8.289,10.711c3.113,0.807,6.236,1.568,9.359,2.301L98.068,391.66c-2.932,6.645,0.076,14.408,6.721,17.338 c1.727,0.762,3.529,1.123,5.303,1.123c5.051,0,9.867-2.928,12.037-7.846l15.777-35.766c3.795,0.635,7.594,1.219,11.395,1.748 l11.262-31.658c1.49-4.194,5.461-6.994,9.91-6.994h69.178c4.451,0,8.42,2.8,9.912,6.994l11.26,31.658 c3.801-0.529,7.601-1.113,11.396-1.748l15.776,35.766c2.17,4.918,6.986,7.846,12.039,7.846c1.771,0,3.574-0.361,5.302-1.123 c6.645-2.932,9.651-10.693,6.723-17.338l-13.438-30.461c3.123-0.732,6.244-1.494,9.357-2.301 C312.856,357.635,316.267,353.229,316.267,348.188z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'SSE':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M35 1729 l-35 -31 0 -738 0 -738 35 -31 36 -31 585 0 586 0 34 34 34 34 0 135 0 135 -43 63 c-24 34 -48 64 -53 66 -5 2 -8 -51 -6 -129 2 -72 1 -146 -3 -165 -12 -65 2 -63 -556 -64 l-504 0 -23 26 -22 26 0 640 0 640 25 24 24 25 500 0 c275 0 507 -3 516 -6 35 -14 45 -58 45 -202 l0 -138 51 -53 c28 -29 51 -51 52 -49 1 2 0 115 -3 251 l-5 249 -33 29 -32 29 -585 0 -584 0 -36 -31z""/><path d=""M200 1485 c-10 -12 -10 -21 -2 -40 l12 -25 447 2 448 3 0 35 0 35 -446 3 c-392 2 -448 0 -459 -13z""/><path d=""M200 1280 c-8 -14 -8 -26 0 -40 10 -19 21 -20 397 -20 l385 0 -6 31 c-3 17 -6 35 -6 40 0 5 -169 9 -380 9 -369 0 -380 -1 -390 -20z""/><path d=""M1016 1254 c-18 -18 -20 -30 -6 -39 6 -3 10 -36 10 -73 0 -45 7 -80 20 -107 28 -59 243 -350 259 -350 18 0 220 157 221 172 0 6 -3 13 -8 15 -8 3 -162 181 -226 260 -47 59 -87 84 -156 99 -30 6 -60 18 -68 25 -17 18 -27 18 -46 -2z""/><path d=""M206 1114 c-19 -18 -20 -28 -6 -55 10 -18 25 -19 386 -19 339 0 376 2 370 16 -3 9 -6 29 -6 45 l0 29 -364 0 c-318 0 -367 -2 -380 -16z""/><path d=""M200 935 c-13 -15 -5 -55 14 -67 18 -13 826 -10 826 3 0 6 -12 26 -26 45 l-26 34 -388 0 c-327 0 -389 -2 -400 -15z""/><path d=""M1703 823 c-28 -11 -9 -49 73 -154 47 -60 90 -112 95 -115 5 -3 18 -2 29 1 39 12 23 51 -66 165 -81 104 -98 117 -131 103z""/><path d=""M1443 712 c-67 -53 -122 -101 -123 -106 0 -10 188 -252 245 -315 66 -73 132 -89 212 -50 83 40 115 138 73 223 -28 56 -252 340 -270 343 -8 1 -70 -42 -137 -95z""/><path d=""M206 744 c-19 -18 -19 -16 -6 -48 l10 -26 439 0 c386 0 441 2 455 16 20 20 20 38 0 58 -14 14 -68 16 -449 16 -381 0 -435 -2 -449 -16z""/><path d=""M216 514 c-15 -7 -20 -19 -19 -38 1 -16 5 -31 8 -34 11 -12 888 -2 902 10 17 14 17 43 0 56 -18 16 -863 21 -891 6z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'BOO':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M433 1672 l-433 -116 0 -723 c0 -593 2 -723 13 -723 32 0 882 232 889 243 4 7 8 333 8 725 l0 712 -22 -1 c-13 0 -218 -53 -455 -117z m142 -411 c99 -44 124 -178 44 -245 l-30 -25 32 -29 c28 -25 33 -37 36 -88 5 -76 -18 -116 -85 -147 -40 -19 -65 -22 -165 -22 -106 0 -120 2 -133 19 -11 16 -14 70 -14 270 0 138 4 257 9 265 23 35 228 37 306 2z""/><path d=""M360 1116 l0 -96 64 0 c109 0 146 26 146 104 0 55 -34 77 -129 83 l-81 6 0 -97z""/><path d=""M360 865 l0 -85 58 0 c117 0 164 35 147 109 -10 48 -38 61 -127 61 l-78 0 0 -85z""/><path d=""M1020 1065 c0 -399 2 -725 5 -725 3 0 200 -52 438 -115 238 -63 438 -115 445 -115 9 0 12 168 12 723 l0 724 -438 116 c-241 65 -443 117 -450 117 -9 0 -12 -161 -12 -725z m370 81 l5 -139 94 141 94 142 59 0 c45 0 57 -3 52 -12 -9 -14 -181 -255 -195 -274 -6 -7 27 -58 95 -145 57 -74 102 -137 99 -141 -6 -11 -71 -10 -92 2 -10 5 -61 66 -112 135 l-94 126 -3 -130 c-3 -128 -3 -131 -26 -137 -13 -3 -35 -4 -47 -2 l-24 3 -3 275 c-1 151 0 281 3 288 3 9 19 12 47 10 l43 -3 5 -139z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'PPL':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M708 1403 c-14 -9 -36 -34 -48 -55 -21 -37 -23 -38 -76 -38 l-54 0 0 -45 c0 -38 3 -45 20 -45 20 0 20 -7 20 -341 0 -215 4 -347 10 -360 10 -18 26 -19 433 -19 316 0 426 3 435 12 9 9 12 102 12 355 0 336 0 343 20 343 19 0 20 -7 20 -209 0 -181 2 -210 16 -215 9 -3 61 -6 116 -6 l100 0 84 82 84 82 0 138 c0 85 4 138 10 138 6 0 10 20 10 45 l0 45 -40 0 c-34 0 -41 4 -46 25 -3 14 -22 38 -40 55 -30 26 -42 30 -94 30 -52 0 -64 -4 -95 -31 -19 -17 -38 -42 -41 -55 l-6 -24 -194 0 -194 0 -14 35 c-31 73 -145 100 -217 52 l-34 -23 -34 23 c-41 28 -122 31 -163 6z m122 -83 c24 -24 25 -48 4 -78 -31 -44 -104 -20 -104 35 0 57 61 82 100 43z m240 0 c11 -11 20 -29 20 -40 0 -42 -62 -75 -96 -52 -18 12 -35 52 -28 65 29 50 70 61 104 27z m670 0 c24 -24 26 -57 3 -82 -34 -38 -103 -11 -103 40 0 56 61 81 100 42z m50 -327 c0 -10 -21 -39 -47 -65 -45 -45 -50 -48 -100 -48 l-53 0 0 65 0 65 100 0 c88 0 100 -2 100 -17z""/><path d=""M12 1228 c-15 -15 -16 -235 -1 -275 13 -34 55 -80 91 -99 14 -7 51 -17 81 -20 l55 -6 -17 -29 c-14 -26 -15 -31 -1 -56 11 -20 25 -29 49 -31 30 -3 41 5 112 75 56 56 79 86 79 104 -1 16 -26 51 -79 107 -85 89 -113 100 -151 62 -24 -24 -25 -48 -4 -78 15 -22 14 -22 -24 -22 -27 0 -45 7 -60 23 -19 21 -22 35 -22 118 0 112 -12 139 -64 139 -17 0 -37 -5 -44 -12z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ZAR':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M1447 1852 c-32 -35 -21 -69 40 -133 l56 -59 -666 0 -666 0 -15 -22 c-9 -12 -16 -32 -16 -43 0 -11 7 -31 16 -43 l15 -22 667 0 666 0 -57 -58 c-45 -46 -57 -64 -57 -89 0 -41 23 -63 67 -63 32 0 47 11 145 108 108 105 138 144 138 178 0 10 -55 73 -122 141 -112 113 -125 123 -158 123 -23 0 -43 -7 -53 -18z""/><path d=""M100 1188 c-13 -31 -100 -363 -100 -382 0 -13 16 -16 100 -16 l100 0 0 -168 c0 -117 4 -172 12 -180 9 -9 75 -12 235 -12 l223 0 0 -90 0 -90 130 0 130 0 0 90 0 90 63 0 c36 0 68 5 75 12 8 8 12 63 12 180 l0 168 420 0 c375 0 420 2 420 16 0 19 -87 350 -100 382 l-10 22 -850 0 -850 0 -10 -22z""/><path d=""M1190 695 l0 -65 125 0 125 0 0 65 0 65 -125 0 -125 0 0 -65z""/><path d=""M1480 695 l0 -65 125 0 125 0 0 65 0 65 -125 0 -125 0 0 -65z""/><path d=""M1180 583 c0 -10 3 -42 6 -70 l7 -53 123 0 124 0 0 70 0 70 -130 0 c-117 0 -130 -2 -130 -17z""/><path d=""M1480 535 l0 -65 125 0 125 0 0 65 0 65 -125 0 -125 0 0 -65z""/><path d=""M670 115 l0 -66 128 3 127 3 3 63 3 62 -131 0 -130 0 0 -65z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'INR':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M255 1853 c-45 -23 -71 -50 -90 -94 l-18 -39 -74 0 -73 0 0 -65 c0 -58 2 -65 20 -65 19 0 19 -10 22 -476 l3 -476 28 -24 28 -24 564 0 564 0 28 24 28 24 -4 476 -4 476 32 0 31 0 0 -294 c0 -259 2 -295 16 -300 9 -3 81 -6 160 -6 l144 0 115 111 115 112 0 188 c0 161 2 189 15 189 12 0 15 14 15 65 l0 65 -55 0 c-54 0 -55 1 -70 37 -34 82 -96 117 -196 111 -58 -3 -74 -8 -106 -34 -21 -17 -47 -50 -58 -73 l-20 -41 -271 0 -272 0 -6 25 c-9 35 -65 96 -103 111 -74 32 -191 10 -232 -42 l-18 -23 -19 20 c-51 56 -172 78 -239 42z m150 -118 c54 -53 16 -145 -60 -145 -49 0 -85 36 -85 85 0 49 36 85 85 85 25 0 44 -8 60 -25z m331 1 c59 -50 20 -146 -60 -146 -40 0 -86 41 -86 77 0 81 86 121 146 69z m939 -2 c69 -74 -18 -181 -106 -133 -31 16 -49 66 -37 101 22 61 100 79 143 32z m71 -456 c-4 -23 -26 -53 -70 -95 l-64 -63 -76 0 -76 0 0 95 0 95 146 0 147 0 -7 -32z""/><path d=""M1550 640 c-37 -37 -27 -79 36 -148 l55 -62 -796 -2 -797 -3 -19 -24 c-26 -32 -24 -77 6 -106 l24 -25 792 0 791 0 -56 -61 c-47 -51 -56 -66 -56 -98 0 -48 24 -71 75 -71 37 0 47 8 177 138 124 123 138 141 138 172 0 31 -14 49 -138 172 -128 128 -141 138 -175 138 -24 0 -44 -7 -57 -20z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'DES':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M886 1553 c-2 -10 -17 -107 -32 -215 l-27 -198 -176 92 c-97 50 -181 95 -188 100 -29 23 -69 -3 -262 -171 -144 -124 -201 -180 -201 -195 0 -17 20 -32 89 -69 48 -26 93 -47 99 -47 6 0 61 27 121 60 61 33 117 60 125 60 8 0 266 -129 575 -286 308 -157 591 -296 628 -310 49 -17 90 -24 146 -24 74 0 80 2 108 29 61 61 28 158 -78 235 -27 19 -126 76 -221 127 -95 50 -179 101 -187 113 -7 11 -70 149 -140 306 -70 157 -134 292 -141 301 -20 22 -188 109 -212 109 -11 0 -23 -8 -26 -17z""/><path d=""M626 743 c-34 -41 -56 -76 -54 -87 2 -13 39 -39 108 -78 l105 -58 150 6 c83 4 173 10 200 13 l50 6 -85 46 c-47 26 -160 85 -252 133 l-166 86 -56 -67z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'CAT':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M1008 1457 l-258 -101 0 -373 0 -373 53 21 c28 11 150 59 270 106 l217 86 0 369 c0 288 -3 368 -12 368 -7 -1 -129 -47 -270 -103z""/><path d=""M1380 1192 l0 -369 238 -93 c130 -51 252 -99 270 -107 l32 -13 0 373 0 373 -258 102 c-142 56 -264 102 -270 102 -9 0 -12 -95 -12 -368z""/><path d=""M0 1150 l0 -50 340 0 340 0 0 50 0 50 -340 0 -340 0 0 -50z""/><path d=""M94 1026 c-3 -8 -4 -29 -2 -48 l3 -33 293 -3 292 -2 0 50 0 50 -290 0 c-240 0 -292 -2 -296 -14z""/><path d=""M290 835 l0 -45 195 0 195 0 0 45 0 45 -195 0 -195 0 0 -45z""/><path d=""M1320 748 c-192 -71 -510 -202 -503 -208 14 -14 503 -180 529 -180 26 0 514 166 527 180 5 4 -85 45 -200 90 -329 130 -325 129 -353 118z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'TBD':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M336 1898 c-14 -19 -16 -66 -16 -312 l0 -289 -64 61 c-69 67 -101 77 -144 46 -17 -11 -22 -25 -22 -56 0 -40 6 -47 132 -175 179 -180 165 -180 345 0 127 127 133 135 133 173 0 50 -23 74 -71 74 -28 0 -44 -10 -96 -62 l-63 -62 0 292 c0 279 -1 293 -20 312 -29 29 -94 27 -114 -2z""/><path d=""M1180 1906 c0 -20 139 -696 150 -732 12 -36 64 -89 103 -103 36 -14 108 -14 144 0 36 13 88 64 102 98 11 25 151 712 151 738 0 10 -66 13 -325 13 -269 0 -325 -2 -325 -14z m388 -591 c74 -62 33 -175 -63 -175 -73 0 -123 76 -91 138 34 65 101 81 154 37z""/><path d=""M879 783 c-189 -229 -350 -425 -358 -436 -12 -17 -10 -22 17 -50 16 -18 39 -51 51 -75 l21 -42 143 117 c78 65 293 242 476 393 l334 275 -63 7 c-121 14 -211 79 -248 181 -9 26 -20 47 -23 47 -4 0 -161 -188 -350 -417z""/><path d=""M305 905 c-46 -25 -76 -56 -96 -100 -34 -72 -20 -135 29 -135 38 0 48 10 61 56 24 89 107 118 162 56 18 -20 29 -43 29 -61 0 -41 -43 -89 -86 -97 -61 -11 -64 -20 -64 -179 l0 -143 -34 -21 c-64 -40 -86 -142 -46 -209 60 -97 210 -97 270 0 40 66 15 173 -51 212 l-29 17 0 109 0 110 38 19 c135 69 150 248 29 345 -33 27 -48 31 -110 34 -52 2 -80 -2 -102 -13z m135 -705 c29 -29 26 -74 -6 -99 -33 -26 -45 -26 -78 0 -48 38 -22 119 39 119 14 0 34 -9 45 -20z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ABU':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M262 1747 c-67 -68 -122 -131 -122 -141 0 -34 30 -73 138 -178 98 -97 113 -108 145 -108 44 0 67 22 67 63 0 25 -12 43 -57 89 l-57 58 666 0 667 0 15 22 c20 28 20 58 0 86 l-15 22 -666 0 -666 0 56 59 c43 45 57 67 57 89 0 38 -28 62 -71 62 -31 0 -48 -13 -157 -123z""/><path d=""M100 1188 c-13 -32 -100 -363 -100 -382 0 -14 45 -16 420 -16 l420 0 0 -168 c0 -117 4 -172 12 -180 7 -7 39 -12 75 -12 l63 0 0 -90 0 -90 130 0 130 0 0 90 0 90 223 0 c160 0 226 3 235 12 8 8 12 63 12 180 l0 168 100 0 c84 0 100 3 100 16 0 19 -87 351 -100 382 l-10 22 -850 0 -850 0 -10 -22z""/><path d=""M190 695 l0 -65 125 0 125 0 0 65 0 65 -125 0 -125 0 0 -65z""/><path d=""M480 695 l0 -65 125 0 125 0 0 65 0 65 -125 0 -125 0 0 -65z""/><path d=""M190 535 l0 -65 125 0 125 0 0 65 0 65 -125 0 -125 0 0 -65z""/><path d=""M480 530 l0 -70 124 0 123 0 7 53 c11 95 22 87 -124 87 l-130 0 0 -70z""/><path d=""M992 118 l3 -63 128 -3 127 -3 0 66 0 65 -130 0 -131 0 3 -62z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'LLD':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M215 1854 c-48 -26 -67 -46 -87 -91 -19 -43 -19 -43 -73 -43 l-55 0 0 -65 c0 -51 3 -65 15 -65 13 0 15 -28 15 -189 l0 -188 115 -112 115 -111 144 0 c79 0 151 3 160 6 14 5 16 41 16 300 l0 294 31 0 32 0 -4 -476 -4 -476 28 -24 28 -24 564 0 564 0 28 24 28 24 3 476 c3 466 3 476 22 476 18 0 20 7 20 65 l0 65 -74 0 c-73 0 -74 0 -86 30 -33 79 -96 120 -185 120 -60 0 -116 -22 -149 -59 l-19 -20 -18 23 c-41 52 -158 74 -232 42 -38 -15 -94 -76 -103 -111 l-6 -25 -272 0 -271 0 -20 41 c-11 23 -37 56 -58 73 -34 27 -47 31 -110 33 -52 3 -80 -1 -102 -13z m142 -109 c32 -22 43 -65 28 -102 -36 -86 -165 -62 -165 30 0 70 80 112 137 72z m934 2 c45 -34 50 -93 13 -131 -55 -54 -148 -17 -148 59 0 29 7 43 28 61 30 26 81 31 107 11z m344 -12 c17 -16 25 -35 25 -60 0 -25 -8 -44 -25 -60 -16 -17 -35 -25 -60 -25 -25 0 -44 8 -60 25 -17 16 -25 35 -25 60 0 25 8 44 25 60 16 17 35 25 60 25 25 0 44 -8 60 -25z m-1175 -520 l0 -95 -76 0 -76 0 -64 63 c-44 42 -66 72 -70 95 l-6 32 146 0 146 0 0 -95z""/><path d=""M138 522 c-124 -123 -138 -141 -138 -172 0 -31 14 -49 138 -172 132 -132 140 -138 178 -138 51 0 74 23 74 73 0 30 -9 46 -56 96 l-56 61 791 0 792 0 24 25 c30 29 32 74 6 106 l-19 24 -797 3 -796 2 55 62 c44 49 56 68 56 95 0 45 -30 73 -77 73 -34 0 -47 -10 -175 -138z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ATER':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M1657 1754 c-57 -20 -161 -74 -347 -179 l-165 -94 -395 60 c-217 32 -403 59 -413 59 -10 0 -72 -30 -137 -66 -100 -55 -120 -70 -120 -89 0 -18 45 -62 190 -186 145 -125 187 -166 177 -175 -6 -6 -110 -66 -230 -133 l-217 -123 0 -40 c0 -35 73 -568 85 -625 8 -35 38 -29 152 34 71 38 105 63 108 77 2 12 9 86 15 166 6 79 14 148 18 152 4 4 296 176 650 382 353 206 670 395 705 419 72 51 147 129 172 177 13 24 16 49 13 93 -4 52 -9 63 -34 83 -25 21 -40 24 -109 23 -44 0 -97 -7 -118 -15z""/><path d=""M995 899 c-153 -88 -281 -162 -284 -165 -3 -2 10 -52 29 -109 30 -94 36 -105 58 -105 26 0 251 120 274 147 21 25 218 370 218 382 0 6 -3 11 -7 11 -5 -1 -134 -73 -288 -161z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'CAD':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M247 1902 c-15 -17 -17 -60 -17 -415 l0 -395 136 -4 c135 -3 136 -3 160 -31 23 -27 24 -35 24 -163 l0 -134 275 0 276 0 25 25 25 25 -3 541 c-3 522 -4 541 -22 555 -16 12 -93 14 -441 14 -396 0 -423 -1 -438 -18z""/><path d=""M1342 1716 c-37 -39 -62 -73 -62 -86 0 -24 111 -146 142 -156 26 -8 68 22 68 48 0 12 -8 30 -17 41 -18 20 -17 20 22 14 61 -10 75 -34 75 -131 0 -89 8 -106 51 -106 44 0 51 19 47 134 -2 87 -7 109 -24 132 -34 47 -69 66 -130 73 -50 5 -55 8 -41 20 20 16 22 52 5 69 -7 7 -26 12 -43 12 -26 0 -43 -12 -93 -64z""/><path d=""M1220 955 c0 -276 21 -255 -255 -255 l-205 0 0 -190 0 -190 134 0 c128 0 136 -1 163 -24 28 -24 28 -25 31 -160 l4 -136 277 0 c257 0 279 1 294 18 15 17 17 70 17 558 0 450 -2 543 -14 560 -13 18 -30 19 -230 22 l-216 3 0 -206z""/><path d=""M237 1053 c-22 -22 1 -56 117 -172 102 -102 127 -122 145 -116 20 6 21 13 21 134 0 166 5 161 -157 161 -66 0 -123 -3 -126 -7z""/><path d=""M242 658 c-8 -8 -12 -48 -12 -114 0 -88 3 -105 23 -134 34 -50 70 -72 127 -78 36 -3 48 -8 41 -15 -15 -15 -14 -60 1 -75 29 -29 63 -13 132 58 55 57 66 74 61 92 -9 33 -129 148 -153 148 -47 0 -69 -62 -35 -96 17 -17 16 -17 -22 -11 -61 10 -75 34 -75 131 0 50 -5 87 -12 94 -15 15 -61 15 -76 0z""/><path d=""M767 283 c-19 -18 3 -48 117 -162 99 -99 128 -122 147 -119 24 3 24 5 27 127 4 166 8 161 -158 161 -70 0 -130 -3 -133 -7z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ENT':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M1415 1910 c-100 -31 -198 -126 -232 -225 -25 -72 -22 -184 7 -260 32 -81 113 -163 195 -195 82 -32 197 -32 278 -1 70 28 152 105 187 176 22 44 25 64 25 160 0 101 -2 114 -28 163 -35 67 -92 124 -159 159 -45 23 -67 27 -148 30 -52 1 -108 -2 -125 -7z m292 -477 l-27 -28 -100 100 -100 99 -45 -44 -45 -44 -30 29 -30 29 72 73 73 73 130 -130 130 -130 -28 -27z""/><path d=""M40 985 c0 -371 2 -675 5 -675 4 0 114 74 245 165 l240 164 0 -164 c0 -110 4 -165 11 -165 6 0 121 76 257 170 l247 170 83 0 82 0 0 -325 0 -325 195 0 195 0 0 573 0 573 -92 0 c-120 0 -185 24 -268 101 -82 75 -122 157 -128 261 -2 43 0 94 6 115 l10 37 -544 0 -544 0 0 -675z m550 10 l0 -125 -190 0 -190 0 0 125 0 125 190 0 190 0 0 -125z m610 0 l0 -125 -190 0 -190 0 0 125 0 125 190 0 190 0 0 -125z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'RBO':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M2 1178 l3 -741 455 -219 c250 -120 464 -218 475 -217 11 0 236 98 500 218 l480 218 3 741 2 742 -175 0 -175 0 0 -595 0 -595 -610 0 -610 0 0 595 0 595 -175 0 -175 0 2 -742z""/><path d=""M440 1780 l0 -100 115 0 115 0 0 100 0 100 -115 0 -115 0 0 -100z""/><path d=""M710 1780 l0 -100 115 0 115 0 0 100 0 100 -115 0 -115 0 0 -100z""/><path d=""M980 1780 l0 -100 110 0 110 0 0 100 0 100 -110 0 -110 0 0 -100z""/><path d=""M1250 1780 l0 -100 110 0 110 0 0 100 0 100 -110 0 -110 0 0 -100z""/><path d=""M440 1525 l0 -95 115 0 115 0 0 95 0 95 -115 0 -115 0 0 -95z""/><path d=""M710 1525 l0 -95 115 0 115 0 0 95 0 95 -115 0 -115 0 0 -95z""/><path d=""M980 1525 l0 -95 110 0 110 0 0 95 0 95 -110 0 -110 0 0 -95z""/><path d=""M440 1270 l0 -100 115 0 115 0 0 100 0 100 -115 0 -115 0 0 -100z""/><path d=""M710 1270 l0 -100 115 0 115 0 0 100 0 100 -115 0 -115 0 0 -100z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'CAM':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M252 1617 c-35 -20 -55 -56 -56 -103 -1 -52 25 -91 77 -117 30 -15 43 -16 76 -7 58 15 91 59 91 122 0 44 -4 54 -34 84 -29 29 -40 34 -82 34 -27 -1 -59 -6 -72 -13z""/><path d=""M794 1596 c-30 -30 -34 -40 -34 -84 0 -61 32 -106 87 -121 127 -35 214 116 121 210 -26 25 -37 29 -85 29 -49 0 -59 -4 -89 -34z""/><path d=""M16 1514 c-14 -13 -16 -55 -16 -304 0 -338 -6 -320 112 -320 l68 0 0 -235 0 -236 25 -24 24 -25 264 0 c238 0 267 2 285 17 32 27 420 679 427 717 4 18 5 112 3 209 -3 202 -5 207 -85 214 l-49 5 -12 -52 c-20 -86 -67 -133 -155 -154 -75 -18 -189 60 -202 139 -11 68 -7 65 -104 65 l-91 0 -6 -27 c-21 -95 -68 -153 -143 -173 -101 -27 -205 47 -226 163 -7 37 -8 37 -55 37 -31 0 -54 -6 -64 -16z m436 -621 l3 -168 60 0 60 0 3 118 3 117 67 0 c79 0 92 9 92 62 l0 38 121 0 c66 0 118 -3 116 -7 -2 -5 -71 -122 -153 -260 l-149 -253 -162 0 -163 0 0 260 0 260 50 0 50 0 2 -167z""/><path d=""M1342 1508 c-17 -17 -17 -1189 0 -1206 16 -16 70 -16 86 0 9 9 12 147 12 560 l0 548 228 0 c164 0 231 3 240 12 16 16 16 70 0 86 -17 17 -549 17 -566 0z""/><path d=""M1522 1332 c-9 -7 -12 -55 -10 -208 l3 -199 200 0 200 0 3 199 c2 153 -1 201 -10 208 -17 10 -369 10 -386 0z""/><path d=""M1515 817 c-3 -7 -4 -100 -3 -207 l3 -195 180 -3 c99 -1 190 0 203 3 l22 5 -2 203 -3 202 -198 3 c-155 2 -199 0 -202 -11z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'CRU':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_AgenciaAduanal"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M243 1445 c-25 -18 -34 -59 -21 -97 12 -35 35 -48 82 -48 49 0 76 28 76 80 0 52 -27 80 -78 80 -21 0 -47 -7 -59 -15z""/><path d=""M777 1448 c-26 -21 -38 -63 -26 -98 13 -37 36 -50 83 -50 49 0 76 28 76 80 0 53 -27 80 -78 80 -21 0 -46 -6 -55 -12z""/><path d=""M1270 1440 c-49 -49 -10 -140 59 -140 46 0 68 15 80 53 12 36 7 61 -17 90 -22 25 -96 23 -122 -3z""/><path d=""M1580 1440 c-13 -13 -20 -33 -20 -60 0 -52 27 -80 76 -80 49 0 70 14 83 53 14 40 2 80 -28 96 -33 18 -89 13 -111 -9z""/><path d=""M7 1393 c-4 -3 -7 -50 -7 -103 0 -84 3 -99 20 -115 18 -16 20 -31 20 -159 l1 -141 110 -127 110 -128 154 0 c84 0 160 3 169 6 14 5 16 39 16 275 l0 269 648 0 c356 0 653 4 660 8 9 6 12 36 10 113 l-3 104 -73 3 c-62 2 -73 0 -78 -15 -26 -90 -87 -134 -154 -114 -46 13 -77 49 -86 98 -6 31 -9 34 -37 31 -26 -3 -32 -9 -40 -38 -11 -46 -42 -78 -88 -92 -67 -19 -131 24 -145 98 l-6 34 -128 0 -128 0 -6 -34 c-9 -48 -41 -84 -86 -97 -67 -20 -128 24 -154 114 -5 15 -21 17 -146 17 l-140 0 0 -24 c0 -39 -42 -90 -87 -106 -38 -13 -46 -12 -78 3 -38 18 -75 69 -75 105 0 21 -4 22 -83 22 -46 0 -87 -3 -90 -7z m491 -585 l3 -88 -89 0 -89 0 -65 83 c-36 45 -64 85 -61 90 2 4 71 6 151 5 l147 -3 3 -87z""/><path d=""M970 1090 l-285 -5 0 -320 0 -320 618 -3 617 -2 0 324 0 324 -22 6 c-25 6 -474 5 -928 -4z m-90 -109 c6 -12 10 -100 10 -216 0 -116 -4 -204 -10 -216 -12 -21 -40 -25 -58 -7 -17 17 -17 429 0 446 18 18 46 14 58 -7z m179 5 c8 -9 11 -78 9 -226 -3 -190 -5 -214 -20 -224 -13 -8 -23 -8 -35 0 -16 10 -18 34 -21 224 -2 219 1 240 38 240 10 0 23 -6 29 -14z m179 2 c17 -17 17 -429 0 -446 -18 -18 -46 -14 -58 7 -6 12 -10 100 -10 216 0 116 4 204 10 216 12 21 40 25 58 7z m180 0 c17 -17 17 -429 0 -446 -20 -20 -48 -14 -58 14 -6 14 -10 108 -10 209 0 101 4 195 10 209 10 28 38 34 58 14z m186 -4 c9 -24 7 -427 -3 -442 -4 -8 -19 -12 -32 -10 l-24 3 -3 220 c-1 121 0 225 2 232 7 18 53 16 60 -3z m181 -219 l0 -230 -30 0 -30 0 -3 220 c-1 121 0 226 3 233 3 8 16 12 32 10 l28 -3 0 -230z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            default:" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""SinDefinir""></span>'" & vbCrLf)
                    jsAuxiliar_.Append("                        }" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.EstatusTrackingLogistica

                    jsAuxiliar_.Append("                        $('.iEntidadDatos-EstatusTracking_FreightForwarder').parent().addClass(""text-center"")" & vbCrLf)
                    jsAuxiliar_.Append("                        switch(data) {" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'give-money':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 354.796 354.796""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""><g><g id=""Layer_5_58_""><g><g><path d=""M265.442,109.092c-10.602-4.25-13.665-6.82-13.665-11.461c0-3.714,2.813-8.053,10.744-8.053c7.015,0,12.395,2.766,12.443,2.79c0.566,0.302,1.201,0.463,1.83,0.463c1.535,0,2.893-0.929,3.456-2.367l1.927-4.926c0.671-1.795-0.347-3.359-1.645-3.92c-4.319-1.88-12.76-3.335-12.846-3.35c-0.136-0.024-0.609-0.125-0.609-0.678l-0.027-7.146c0-2.152-1.797-3.904-4.003-3.904h-3.457c-2.204,0-4,1.751-4,3.905l0.009,7.513c0,0.576-0.624,0.826-0.852,0.879c-10.655,2.538-17.314,10.343-17.314,20.188c0,12.273,10.145,17.819,21.099,21.982c8.757,3.438,12.329,6.924,12.329,12.037c0,5.564-5.059,9.45-12.307,9.45c-6.189,0-14.565-3.923-14.648-3.963c-0.536-0.254-1.104-0.382-1.688-0.382c-1.594,0-2.982,0.964-3.537,2.457l-1.84,4.982c-0.654,1.86,0.353,3.37,1.642,4.042c5.144,2.679,15.098,4.249,15.541,4.318c0.119,0.017,0.725,0.23,0.725,0.784v7.48c0,2.152,1.797,3.904,4.004,3.904h3.572c2.208,0,4.005-1.751,4.005-3.904v-7.872c0-0.736,0.543-0.801,0.655-0.828c11.351-2.55,18.343-10.855,18.343-21.283C285.325,121.518,279.377,114.597,265.442,109.092z""/><path d=""M260.979,22.509c-51.816,0-93.818,42.005-93.818,93.818c0,51.814,42.002,93.82,93.818,93.82c51.814,0,93.817-42.006,93.817-93.82C354.796,64.514,312.793,22.509,260.979,22.509z M260.979,188.404c-39.808,0-72.076-32.271-72.076-72.076s32.268-72.075,72.076-72.075c39.806,0,72.073,32.27,72.073,72.075S300.785,188.404,260.979,188.404z""/></g><g><path d=""M335.733,255.61c-19.95,11.011-47.389,21.192-74.753,25.484c-24.346,3.818-70.148-5.39-70.148-16.265c0-4.121,40.17,10.154,64.469,3.671c18.633-4.971,15.988-22.401,5.853-24.7c-10.076-2.287-69.108-23.913-94.323-24.659c-11.878-0.351-41.203,4.131-55.393,6.442c-4.861,0.791-7.909,0.704-8.213,5.356c-1.412,21.62-4.195,65.832-5.712,88.926c-0.032,0.488,0.646,7.05,6.061,2.432c5.927-5.054,14.24-10.656,21.929-8.912c12.063,2.737,116.424,21.856,130.819,18.51c20.593-4.787,78.888-39.334,90.065-50.072C363.711,265.176,350.244,247.601,335.733,255.61z""/><path d=""M74.426,224.74l-54.672-2.694c-4.221-0.208-8.532,2.973-9.581,7.066l-9.941,90.255c-1.048,4.094,1.55,7.578,5.773,7.741l60.59-0.006c4.224,0.163,7.942-3.151,8.266-7.365l6.654-86.958C81.837,228.566,78.647,224.948,74.426,224.74z M42.24,315.145c-8.349,0-15.116-6.768-15.116-15.116c0-8.349,6.768-15.116,15.116-15.116s15.116,6.768,15.116,15.116C57.356,308.378,50.589,315.145,42.24,315.145z""/></g></g></g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'FAC':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 354.796 354.796""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""><g><g id=""Layer_5_58_""><g><g><path d=""M265.442,109.092c-10.602-4.25-13.665-6.82-13.665-11.461c0-3.714,2.813-8.053,10.744-8.053c7.015,0,12.395,2.766,12.443,2.79c0.566,0.302,1.201,0.463,1.83,0.463c1.535,0,2.893-0.929,3.456-2.367l1.927-4.926c0.671-1.795-0.347-3.359-1.645-3.92c-4.319-1.88-12.76-3.335-12.846-3.35c-0.136-0.024-0.609-0.125-0.609-0.678l-0.027-7.146c0-2.152-1.797-3.904-4.003-3.904h-3.457c-2.204,0-4,1.751-4,3.905l0.009,7.513c0,0.576-0.624,0.826-0.852,0.879c-10.655,2.538-17.314,10.343-17.314,20.188c0,12.273,10.145,17.819,21.099,21.982c8.757,3.438,12.329,6.924,12.329,12.037c0,5.564-5.059,9.45-12.307,9.45c-6.189,0-14.565-3.923-14.648-3.963c-0.536-0.254-1.104-0.382-1.688-0.382c-1.594,0-2.982,0.964-3.537,2.457l-1.84,4.982c-0.654,1.86,0.353,3.37,1.642,4.042c5.144,2.679,15.098,4.249,15.541,4.318c0.119,0.017,0.725,0.23,0.725,0.784v7.48c0,2.152,1.797,3.904,4.004,3.904h3.572c2.208,0,4.005-1.751,4.005-3.904v-7.872c0-0.736,0.543-0.801,0.655-0.828c11.351-2.55,18.343-10.855,18.343-21.283C285.325,121.518,279.377,114.597,265.442,109.092z""/><path d=""M260.979,22.509c-51.816,0-93.818,42.005-93.818,93.818c0,51.814,42.002,93.82,93.818,93.82c51.814,0,93.817-42.006,93.817-93.82C354.796,64.514,312.793,22.509,260.979,22.509z M260.979,188.404c-39.808,0-72.076-32.271-72.076-72.076s32.268-72.075,72.076-72.075c39.806,0,72.073,32.27,72.073,72.075S300.785,188.404,260.979,188.404z""/></g><g><path d=""M335.733,255.61c-19.95,11.011-47.389,21.192-74.753,25.484c-24.346,3.818-70.148-5.39-70.148-16.265c0-4.121,40.17,10.154,64.469,3.671c18.633-4.971,15.988-22.401,5.853-24.7c-10.076-2.287-69.108-23.913-94.323-24.659c-11.878-0.351-41.203,4.131-55.393,6.442c-4.861,0.791-7.909,0.704-8.213,5.356c-1.412,21.62-4.195,65.832-5.712,88.926c-0.032,0.488,0.646,7.05,6.061,2.432c5.927-5.054,14.24-10.656,21.929-8.912c12.063,2.737,116.424,21.856,130.819,18.51c20.593-4.787,78.888-39.334,90.065-50.072C363.711,265.176,350.244,247.601,335.733,255.61z""/><path d=""M74.426,224.74l-54.672-2.694c-4.221-0.208-8.532,2.973-9.581,7.066l-9.941,90.255c-1.048,4.094,1.55,7.578,5.773,7.741l60.59-0.006c4.224,0.163,7.942-3.151,8.266-7.365l6.654-86.958C81.837,228.566,78.647,224.948,74.426,224.74z M42.24,315.145c-8.349,0-15.116-6.768-15.116-15.116c0-8.349,6.768-15.116,15.116-15.116s15.116,6.768,15.116,15.116C57.356,308.378,50.589,315.145,42.24,315.145z""/></g></g></g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'check-box':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 459 459""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""> <g> <g id=""check-box-outline""> <path d=""M124.95,181.05l-35.7,35.7L204,331.5l255-255l-35.7-35.7L204,260.1L124.95,181.05z M408,408H51V51h255V0H51 C22.95,0,0,22.95,0,51v357c0,28.05,22.95,51,51,51h357c28.05,0,51-22.95,51-51V204h-51V408z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'LFAC':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 459 459""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""> <g> <g id=""check-box-outline""> <path d=""M124.95,181.05l-35.7,35.7L204,331.5l255-255l-35.7-35.7L204,260.1L124.95,181.05z M408,408H51V51h255V0H51 C22.95,0,0,22.95,0,51v357c0,28.05,22.95,51,51,51h357c28.05,0,51-22.95,51-51V204h-51V408z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'DSP':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M170 1675 l0 -35 -85 0 -85 0 0 -670 0 -670 43 0 c36 0 44 -4 59 -30 29 -51 106 -80 213 -80 79 0 98 4 152 29 47 22 64 36 69 55 6 23 11 26 55 26 l49 0 0 670 0 670 -90 0 -90 0 0 35 0 35 -145 0 -145 0 0 -35z m206 -194 c125 -57 119 -227 -9 -276 -57 -22 -110 -8 -156 39 -58 60 -57 141 2 204 49 51 100 62 163 33z m0 -400 c89 -40 119 -149 62 -223 -60 -79 -159 -85 -227 -14 -58 60 -57 141 2 204 49 51 100 62 163 33z m4 -393 c82 -43 110 -119 71 -196 -50 -98 -165 -115 -242 -36 -26 27 -33 43 -37 88 -6 69 20 115 81 145 53 27 72 26 127 -1z""/><path d=""M1379 1407 c-38 -25 -56 -76 -45 -123 4 -16 46 -68 99 -121 l91 -93 -305 0 c-182 0 -319 -4 -342 -11 -77 -21 -105 -130 -48 -187 17 -16 44 -32 62 -36 17 -3 167 -6 333 -6 l301 0 -88 -88 c-90 -91 -111 -129 -100 -183 12 -63 92 -104 152 -80 15 7 118 103 229 215 l202 202 0 54 0 54 -197 199 c-235 237 -267 256 -344 204z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'validated-box':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M170 1675 l0 -35 -85 0 -85 0 0 -670 0 -670 43 0 c36 0 44 -4 59 -30 29 -51 106 -80 213 -80 79 0 98 4 152 29 47 22 64 36 69 55 6 23 11 26 55 26 l49 0 0 670 0 670 -90 0 -90 0 0 35 0 35 -145 0 -145 0 0 -35z m206 -194 c125 -57 119 -227 -9 -276 -57 -22 -110 -8 -156 39 -58 60 -57 141 2 204 49 51 100 62 163 33z m0 -400 c89 -40 119 -149 62 -223 -60 -79 -159 -85 -227 -14 -58 60 -57 141 2 204 49 51 100 62 163 33z m4 -393 c82 -43 110 -119 71 -196 -50 -98 -165 -115 -242 -36 -26 27 -33 43 -37 88 -6 69 20 115 81 145 53 27 72 26 127 -1z""/><path d=""M1379 1407 c-38 -25 -56 -76 -45 -123 4 -16 46 -68 99 -121 l91 -93 -305 0 c-182 0 -319 -4 -342 -11 -77 -21 -105 -130 -48 -187 17 -16 44 -32 62 -36 17 -3 167 -6 333 -6 l301 0 -88 -88 c-90 -91 -111 -129 -100 -183 12 -63 92 -104 152 -80 15 7 118 103 229 215 l202 202 0 54 0 54 -197 199 c-235 237 -267 256 -344 204z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'SAR':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 94.477 94.477"" style=""fill: #000;"" xml:space=""preserve""> <title>Selección automatizada</title> <g> <path d=""M60.936,7.325C60.936,3.279,54.803,0,47.238,0c-7.564,0-13.697,3.279-13.697,7.325h-6.053v82.553h11.15v4.599h17.2v-4.599 h11.15V7.325H60.936z M47.238,81.52c-5.41,0-9.795-4.385-9.795-9.795c0-5.408,4.385-9.794,9.795-9.794s9.795,4.386,9.795,9.794 C57.033,77.135,52.648,81.52,47.238,81.52z M47.238,56.676c-5.41,0-9.795-4.387-9.795-9.796c0-5.41,4.385-9.795,9.795-9.795 s9.795,4.385,9.795,9.795C57.033,52.289,52.648,56.676,47.238,56.676z M47.238,32.545c-5.41,0-9.795-4.385-9.795-9.794 c0-5.41,4.385-9.795,9.795-9.795s9.795,4.385,9.795,9.795C57.033,28.16,52.648,32.545,47.238,32.545z""/> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'traffic-light':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 94.477 94.477"" style=""fill: #000;"" xml:space=""preserve""> <g> <path d=""M60.936,7.325C60.936,3.279,54.803,0,47.238,0c-7.564,0-13.697,3.279-13.697,7.325h-6.053v82.553h11.15v4.599h17.2v-4.599 h11.15V7.325H60.936z M47.238,81.52c-5.41,0-9.795-4.385-9.795-9.795c0-5.408,4.385-9.794,9.795-9.794s9.795,4.386,9.795,9.794 C57.033,77.135,52.648,81.52,47.238,81.52z M47.238,56.676c-5.41,0-9.795-4.387-9.795-9.796c0-5.41,4.385-9.795,9.795-9.795 s9.795,4.385,9.795,9.795C57.033,52.289,52.648,56.676,47.238,56.676z M47.238,32.545c-5.41,0-9.795-4.385-9.795-9.794 c0-5.41,4.385-9.795,9.795-9.795s9.795,4.385,9.795,9.795C57.033,28.16,52.648,32.545,47.238,32.545z""/> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'RAD':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M1595 1843 c-39 -43 -89 -97 -111 -121 -38 -41 -40 -42 -72 -31 -56 19 -188 7 -247 -23 -193 -100 -241 -362 -95 -521 55 -60 114 -93 194 -108 152 -28 311 63 367 210 30 81 24 196 -16 275 l-29 57 55 57 c141 147 149 158 149 206 0 54 -24 76 -84 76 -36 0 -45 -6 -111 -77z m-161 -300 c46 -32 96 -118 96 -166 0 -150 -154 -257 -288 -201 -171 71 -176 305 -7 386 58 27 144 19 199 -19z""/><path d=""M189 1745 c-35 -19 -46 -50 -64 -179 -33 -237 -31 -239 180 -302 83 -24 166 -49 186 -54 31 -9 37 -15 42 -46 6 -33 2 -40 -49 -89 -64 -63 -108 -151 -123 -245 -13 -76 -14 -153 -3 -148 4 2 41 20 82 41 94 47 169 61 290 54 102 -5 179 -27 248 -71 23 -14 44 -26 47 -26 11 0 4 129 -10 185 -23 88 -54 143 -112 204 -43 44 -53 62 -53 89 0 30 4 35 40 49 36 14 39 18 32 42 -4 14 -7 71 -7 126 0 92 3 106 32 167 30 65 100 146 168 195 l30 22 -465 0 c-383 0 -470 -2 -491 -14z""/><path d=""M645 723 c-65 -6 -142 -27 -186 -49 -57 -28 -109 -86 -109 -120 0 -15 -11 -34 -26 -47 -14 -12 -40 -34 -57 -48 l-30 -26 27 -109 c15 -60 34 -119 42 -132 19 -29 344 -192 384 -192 43 0 365 163 386 195 22 34 67 213 60 241 -3 12 -26 35 -51 51 -25 15 -45 34 -45 42 0 36 -42 98 -87 128 -67 46 -209 76 -308 66z m104 -358 l53 -56 -7 -52 c-4 -29 -9 -55 -12 -59 -2 -5 -44 -8 -93 -8 -85 0 -90 1 -94 23 -12 60 -13 83 -4 102 12 23 89 105 99 105 3 0 30 -25 58 -55z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'inspector':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M1595 1843 c-39 -43 -89 -97 -111 -121 -38 -41 -40 -42 -72 -31 -56 19 -188 7 -247 -23 -193 -100 -241 -362 -95 -521 55 -60 114 -93 194 -108 152 -28 311 63 367 210 30 81 24 196 -16 275 l-29 57 55 57 c141 147 149 158 149 206 0 54 -24 76 -84 76 -36 0 -45 -6 -111 -77z m-161 -300 c46 -32 96 -118 96 -166 0 -150 -154 -257 -288 -201 -171 71 -176 305 -7 386 58 27 144 19 199 -19z""/><path d=""M189 1745 c-35 -19 -46 -50 -64 -179 -33 -237 -31 -239 180 -302 83 -24 166 -49 186 -54 31 -9 37 -15 42 -46 6 -33 2 -40 -49 -89 -64 -63 -108 -151 -123 -245 -13 -76 -14 -153 -3 -148 4 2 41 20 82 41 94 47 169 61 290 54 102 -5 179 -27 248 -71 23 -14 44 -26 47 -26 11 0 4 129 -10 185 -23 88 -54 143 -112 204 -43 44 -53 62 -53 89 0 30 4 35 40 49 36 14 39 18 32 42 -4 14 -7 71 -7 126 0 92 3 106 32 167 30 65 100 146 168 195 l30 22 -465 0 c-383 0 -470 -2 -491 -14z""/><path d=""M645 723 c-65 -6 -142 -27 -186 -49 -57 -28 -109 -86 -109 -120 0 -15 -11 -34 -26 -47 -14 -12 -40 -34 -57 -48 l-30 -26 27 -109 c15 -60 34 -119 42 -132 19 -29 344 -192 384 -192 43 0 365 163 386 195 22 34 67 213 60 241 -3 12 -26 35 -51 51 -25 15 -45 34 -45 42 0 36 -42 98 -87 128 -67 46 -209 76 -308 66z m104 -358 l53 -56 -7 -52 c-4 -29 -9 -55 -12 -59 -2 -5 -44 -8 -93 -8 -85 0 -90 1 -94 23 -12 60 -13 83 -4 102 12 23 89 105 99 105 3 0 30 -25 58 -55z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'payment':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 427.479 427.479""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""> <g> <path d=""M287.006,299.735c-6.383,0-11.576-5.193-11.576-11.576s5.193-11.575,11.576-11.575h19.075v-15h-11.58v-18.475h-15v19.561 c-11.01,3.247-19.071,13.441-19.071,25.489c0,14.654,11.922,26.576,26.576,26.576c6.383,0,11.575,5.192,11.575,11.575 s-5.192,11.576-11.575,11.576H267.93v15h11.571v18.479h15v-19.562c11.014-3.245,19.08-13.441,19.08-25.492 C313.581,311.657,301.66,299.735,287.006,299.735z""/> <rect x=""121.397"" y=""337.886"" width=""94.495"" height=""15""/> <rect x=""121.397"" y=""313.484"" width=""94.495"" height=""15""/> <rect x=""150.089"" y=""64.497"" width=""127.3"" height=""15""/> <rect x=""121.397"" y=""137.993"" width=""184.684"" height=""15""/> <rect x=""121.397"" y=""164.991"" width=""184.684"" height=""15""/> <rect x=""121.397"" y=""191.99"" width=""184.684"" height=""15""/> <path d=""M347.982,0v11.999h-16.499V0h-15v11.999h-16.499V0h-15v11.999h-16.498V0h-15v11.999h-16.499V0h-15v11.999H205.49V0h-15 v11.999h-16.499V0h-15v11.999h-16.498V0h-15v11.999h-16.499V0h-15v11.999H79.496V0h-15v427.479h15v-11.999h16.499v11.999h15 v-11.999h16.499v11.999h15v-11.999h16.498v11.999h15v-11.999h16.499v11.999h15v-11.999h16.499v11.999h15v-11.999h16.499v11.999h15 v-11.999h16.498v11.999h15v-11.999h16.499v11.999h15v-11.999h16.499v11.999h15V0H347.982z M79.496,26.999h268.486v373.48H79.496 V26.999z""/> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'PAG':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M210 960 l0 -960 606 0 606 0 134 128 134 127 0 833 0 832 -740 0 -740 0 0 -960z m1340 105 l0 -725 -105 0 -104 0 -3 -102 -3 -103 -492 -3 -493 -2 0 830 0 830 600 0 600 0 0 -725z""/><path d=""M880 1366 c0 -33 -2 -35 -55 -51 -67 -19 -129 -76 -144 -132 -20 -72 -19 -73 59 -73 l70 0 14 34 c8 19 24 39 35 46 21 10 21 9 21 -72 l0 -83 -59 -17 c-96 -28 -141 -89 -141 -188 1 -82 69 -166 153 -190 47 -13 47 -13 47 -56 l0 -44 70 0 70 0 0 44 0 43 65 32 c71 35 107 81 120 154 l7 37 -74 0 -74 0 -13 -35 c-20 -57 -32 -42 -29 36 l3 71 58 18 c78 24 105 44 128 93 56 124 -19 261 -158 292 -29 6 -33 10 -33 41 l0 34 -70 0 -70 0 0 -34z m180 -186 c27 -27 26 -75 -2 -94 -12 -9 -26 -16 -30 -16 -5 0 -8 29 -8 65 0 69 7 78 40 45z m-182 -386 l-3 -36 -27 22 c-17 12 -28 31 -28 45 0 14 11 33 28 45 l27 22 3 -31 c2 -17 2 -47 0 -67z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'payment2':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M210 960 l0 -960 606 0 606 0 134 128 134 127 0 833 0 832 -740 0 -740 0 0 -960z m1340 105 l0 -725 -105 0 -104 0 -3 -102 -3 -103 -492 -3 -493 -2 0 830 0 830 600 0 600 0 0 -725z""/><path d=""M880 1366 c0 -33 -2 -35 -55 -51 -67 -19 -129 -76 -144 -132 -20 -72 -19 -73 59 -73 l70 0 14 34 c8 19 24 39 35 46 21 10 21 9 21 -72 l0 -83 -59 -17 c-96 -28 -141 -89 -141 -188 1 -82 69 -166 153 -190 47 -13 47 -13 47 -56 l0 -44 70 0 70 0 0 44 0 43 65 32 c71 35 107 81 120 154 l7 37 -74 0 -74 0 -13 -35 c-20 -57 -32 -42 -29 36 l3 71 58 18 c78 24 105 44 128 93 56 124 -19 261 -158 292 -29 6 -33 10 -33 41 l0 34 -70 0 -70 0 0 -34z m180 -186 c27 -27 26 -75 -2 -94 -12 -9 -26 -16 -30 -16 -5 0 -8 29 -8 65 0 69 7 78 40 45z m-182 -386 l-3 -36 -27 22 c-17 12 -28 31 -28 45 0 14 11 33 28 45 l27 22 3 -31 c2 -17 2 -47 0 -67z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'PRE':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M177 1852 c-10 -10 -17 -35 -17 -55 0 -30 10 -48 58 -104 32 -38 66 -74 76 -82 9 -8 26 -27 37 -43 l20 -28 -28 -58 c-36 -74 -39 -190 -6 -264 32 -69 94 -131 164 -163 56 -25 70 -27 148 -23 102 5 151 27 217 94 113 117 113 321 0 438 -65 66 -116 89 -213 94 -45 2 -95 0 -112 -6 -29 -10 -32 -8 -136 104 -104 111 -108 114 -149 114 -29 0 -48 -6 -59 -18z m523 -327 c35 -18 87 -75 99 -108 14 -36 14 -108 0 -144 -17 -47 -79 -102 -132 -118 -147 -44 -288 95 -247 244 15 55 83 124 136 139 44 12 107 6 144 -13z""/><path d=""M978 1468 c29 -87 27 -171 -8 -260 -25 -67 -100 -154 -164 -192 -50 -29 -143 -56 -196 -56 -56 0 -151 28 -202 60 l-48 29 0 -188 c0 -139 -3 -191 -12 -198 -7 -5 -79 -33 -160 -62 -80 -29 -156 -60 -167 -68 -37 -25 -36 -28 129 -423 23 -57 30 -65 56 -68 16 -1 103 22 194 53 l164 55 396 0 396 0 164 -55 c91 -31 178 -54 195 -53 28 3 32 10 118 218 58 141 87 225 85 244 -3 29 -8 32 -168 90 -91 34 -171 65 -177 69 -10 6 -13 103 -13 426 0 392 -1 419 -18 434 -16 15 -52 17 -304 17 l-284 0 24 -72z m388 -1020 c-22 -79 -45 -149 -49 -155 -12 -19 -702 -19 -714 0 -4 6 -27 76 -49 155 l-41 142 447 0 447 0 -41 -142z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'previo':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M177 1852 c-10 -10 -17 -35 -17 -55 0 -30 10 -48 58 -104 32 -38 66 -74 76 -82 9 -8 26 -27 37 -43 l20 -28 -28 -58 c-36 -74 -39 -190 -6 -264 32 -69 94 -131 164 -163 56 -25 70 -27 148 -23 102 5 151 27 217 94 113 117 113 321 0 438 -65 66 -116 89 -213 94 -45 2 -95 0 -112 -6 -29 -10 -32 -8 -136 104 -104 111 -108 114 -149 114 -29 0 -48 -6 -59 -18z m523 -327 c35 -18 87 -75 99 -108 14 -36 14 -108 0 -144 -17 -47 -79 -102 -132 -118 -147 -44 -288 95 -247 244 15 55 83 124 136 139 44 12 107 6 144 -13z""/><path d=""M978 1468 c29 -87 27 -171 -8 -260 -25 -67 -100 -154 -164 -192 -50 -29 -143 -56 -196 -56 -56 0 -151 28 -202 60 l-48 29 0 -188 c0 -139 -3 -191 -12 -198 -7 -5 -79 -33 -160 -62 -80 -29 -156 -60 -167 -68 -37 -25 -36 -28 129 -423 23 -57 30 -65 56 -68 16 -1 103 22 194 53 l164 55 396 0 396 0 164 -55 c91 -31 178 -54 195 -53 28 3 32 10 118 218 58 141 87 225 85 244 -3 29 -8 32 -168 90 -91 34 -171 65 -177 69 -10 6 -13 103 -13 426 0 392 -1 419 -18 434 -16 15 -52 17 -304 17 l-284 0 24 -72z m388 -1020 c-22 -79 -45 -149 -49 -155 -12 -19 -702 -19 -714 0 -4 6 -27 76 -49 155 l-41 142 447 0 447 0 -41 -142z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'previo2':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 512 512""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xmlns:xlink=""http://www.w3.org/1999/xlink"" enable-background=""new 0 0 512 512""> <g> <g> <polygon points=""256,11 33.7,76.6 256,152.1 478.3,76.6   ""/> <path d=""M13.1,350.3c0,8.7,5.5,16.4,13.7,19.2l208.9,71V188L13.1,112.3V350.3z""/> <path d=""m465.7,431.1c-4.9,4.7-10.2,8.9-16.1,12.5-5.9,3.6-12.1,6.3-18.5,8.5l20.2,39.2c8.7,13.7 23.1,9.6 27.8,6.6 9.5-5.9 12.5-18.3 6.6-27.8l-20-39z""/> <circle cx=""400.5"" cy=""363.7"" r=""56.7""/> <path d=""m276.2,188v252.5c0,0 44.7-15.8 46.6-17.6-12.6-16.4-20-37-20-59.2 0-53.9 43.8-97.7 97.7-97.7 51.4,0 93.6,39.9 97.4,90.3 0.6-1.9 1-3.9 1-6v-238l-222.7,75.7z""/> </g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'checklist':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M500 1420 l0 -50 40 0 c29 0 40 4 40 15 0 13 43 15 330 15 l330 0 0 -395 0 -395 -60 0 -60 0 0 -55 0 -55 -270 0 -270 0 0 115 0 115 -40 0 -40 0 0 -150 0 -150 337 0 338 0 72 72 73 72 0 448 0 448 -410 0 -410 0 0 -50z""/><path d=""M315 1320 c-45 -5 -108 -18 -140 -30 -33 -12 -80 -25 -105 -31 -25 -5 -51 -17 -57 -26 -9 -12 -13 -70 -13 -204 0 -183 1 -188 22 -203 12 -9 34 -16 48 -16 18 0 68 -31 150 -92 105 -79 129 -93 161 -93 65 0 110 64 88 122 l-9 23 121 0 c85 0 128 4 148 15 30 15 55 62 49 91 -2 11 9 30 27 45 24 22 30 35 30 69 0 34 -6 47 -30 68 -21 18 -29 32 -27 52 4 31 -40 90 -66 90 -13 0 -17 9 -18 36 0 24 -9 43 -27 62 -27 26 -31 27 -150 29 -67 0 -158 -3 -202 -7z m303 -87 c-3 -15 -18 -19 -87 -23 -65 -4 -86 -9 -95 -22 -8 -13 -8 -23 0 -35 9 -15 31 -19 135 -23 107 -4 124 -7 127 -22 3 -16 -10 -18 -124 -20 -108 -3 -128 -6 -138 -20 -8 -13 -8 -23 0 -35 10 -16 33 -19 165 -23 136 -4 154 -7 157 -22 3 -17 -9 -18 -147 -18 -123 0 -152 -3 -165 -16 -19 -19 -20 -38 -3 -52 7 -7 66 -13 133 -14 111 -3 119 -4 119 -23 0 -19 -8 -20 -175 -25 -170 -5 -175 -6 -178 -27 -2 -12 9 -35 27 -56 17 -19 31 -40 31 -46 0 -26 -38 -6 -149 79 -92 70 -124 90 -149 90 l-33 0 3 152 3 153 35 5 c19 4 46 12 58 19 48 26 150 39 300 40 141 1 153 0 150 -16z""/><path d=""M1360 1291 l0 -39 148 -4 c119 -4 158 -9 207 -27 33 -12 76 -26 95 -29 l35 -7 3 -153 3 -152 -33 0 c-25 0 -57 -20 -149 -90 -111 -85 -149 -105 -149 -79 0 6 14 27 31 46 18 21 29 44 27 56 -3 21 -9 22 -110 25 l-108 3 0 -36 0 -35 50 0 c48 0 49 -1 41 -23 -22 -58 23 -122 88 -122 32 0 56 14 161 93 82 61 132 92 150 92 14 0 36 7 48 16 21 15 22 20 22 203 0 134 -4 192 -12 204 -7 9 -33 21 -58 26 -25 6 -71 19 -103 31 -60 21 -203 39 -319 40 l-68 0 0 -39z""/><path d=""M734 1255 c-16 -41 -4 -44 203 -47 109 -2 206 1 216 5 11 5 17 17 15 30 -3 22 -6 22 -216 25 -183 2 -213 0 -218 -13z""/><path d=""M1360 1170 l0 -41 56 3 c64 3 88 23 68 56 -8 12 -27 18 -68 20 l-56 3 0 -41z""/><path d=""M830 1110 l0 -30 169 0 c104 0 172 4 176 10 3 6 3 19 0 30 -6 19 -15 20 -176 20 l-169 0 0 -30z""/><path d=""M1360 1050 l0 -41 56 3 c64 3 88 23 68 56 -8 12 -27 18 -68 20 l-56 3 0 -41z""/><path d=""M902 1001 c-8 -4 -12 -19 -10 -32 3 -24 4 -24 140 -27 131 -2 137 -2 143 18 13 42 -9 50 -139 50 -66 0 -127 -4 -134 -9z""/><path d=""M1360 930 l0 -40 53 0 c71 0 98 28 61 64 -10 10 -33 16 -65 16 l-49 0 0 -40z""/><path d=""M844 866 c-3 -8 -4 -23 -2 -33 3 -16 18 -18 159 -21 168 -3 188 3 174 48 -6 19 -14 20 -166 20 -130 0 -161 -3 -165 -14z""/><path d=""M668 728 c-3 -7 -3 -21 0 -31 4 -16 23 -17 256 -15 l251 3 0 25 0 25 -252 3 c-196 2 -252 0 -255 -10z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'RGU':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M500 1420 l0 -50 40 0 c29 0 40 4 40 15 0 13 43 15 330 15 l330 0 0 -395 0 -395 -60 0 -60 0 0 -55 0 -55 -270 0 -270 0 0 115 0 115 -40 0 -40 0 0 -150 0 -150 337 0 338 0 72 72 73 72 0 448 0 448 -410 0 -410 0 0 -50z""/><path d=""M315 1320 c-45 -5 -108 -18 -140 -30 -33 -12 -80 -25 -105 -31 -25 -5 -51 -17 -57 -26 -9 -12 -13 -70 -13 -204 0 -183 1 -188 22 -203 12 -9 34 -16 48 -16 18 0 68 -31 150 -92 105 -79 129 -93 161 -93 65 0 110 64 88 122 l-9 23 121 0 c85 0 128 4 148 15 30 15 55 62 49 91 -2 11 9 30 27 45 24 22 30 35 30 69 0 34 -6 47 -30 68 -21 18 -29 32 -27 52 4 31 -40 90 -66 90 -13 0 -17 9 -18 36 0 24 -9 43 -27 62 -27 26 -31 27 -150 29 -67 0 -158 -3 -202 -7z m303 -87 c-3 -15 -18 -19 -87 -23 -65 -4 -86 -9 -95 -22 -8 -13 -8 -23 0 -35 9 -15 31 -19 135 -23 107 -4 124 -7 127 -22 3 -16 -10 -18 -124 -20 -108 -3 -128 -6 -138 -20 -8 -13 -8 -23 0 -35 10 -16 33 -19 165 -23 136 -4 154 -7 157 -22 3 -17 -9 -18 -147 -18 -123 0 -152 -3 -165 -16 -19 -19 -20 -38 -3 -52 7 -7 66 -13 133 -14 111 -3 119 -4 119 -23 0 -19 -8 -20 -175 -25 -170 -5 -175 -6 -178 -27 -2 -12 9 -35 27 -56 17 -19 31 -40 31 -46 0 -26 -38 -6 -149 79 -92 70 -124 90 -149 90 l-33 0 3 152 3 153 35 5 c19 4 46 12 58 19 48 26 150 39 300 40 141 1 153 0 150 -16z""/><path d=""M1360 1291 l0 -39 148 -4 c119 -4 158 -9 207 -27 33 -12 76 -26 95 -29 l35 -7 3 -153 3 -152 -33 0 c-25 0 -57 -20 -149 -90 -111 -85 -149 -105 -149 -79 0 6 14 27 31 46 18 21 29 44 27 56 -3 21 -9 22 -110 25 l-108 3 0 -36 0 -35 50 0 c48 0 49 -1 41 -23 -22 -58 23 -122 88 -122 32 0 56 14 161 93 82 61 132 92 150 92 14 0 36 7 48 16 21 15 22 20 22 203 0 134 -4 192 -12 204 -7 9 -33 21 -58 26 -25 6 -71 19 -103 31 -60 21 -203 39 -319 40 l-68 0 0 -39z""/><path d=""M734 1255 c-16 -41 -4 -44 203 -47 109 -2 206 1 216 5 11 5 17 17 15 30 -3 22 -6 22 -216 25 -183 2 -213 0 -218 -13z""/><path d=""M1360 1170 l0 -41 56 3 c64 3 88 23 68 56 -8 12 -27 18 -68 20 l-56 3 0 -41z""/><path d=""M830 1110 l0 -30 169 0 c104 0 172 4 176 10 3 6 3 19 0 30 -6 19 -15 20 -176 20 l-169 0 0 -30z""/><path d=""M1360 1050 l0 -41 56 3 c64 3 88 23 68 56 -8 12 -27 18 -68 20 l-56 3 0 -41z""/><path d=""M902 1001 c-8 -4 -12 -19 -10 -32 3 -24 4 -24 140 -27 131 -2 137 -2 143 18 13 42 -9 50 -139 50 -66 0 -127 -4 -134 -9z""/><path d=""M1360 930 l0 -40 53 0 c71 0 98 28 61 64 -10 10 -33 16 -65 16 l-49 0 0 -40z""/><path d=""M844 866 c-3 -8 -4 -23 -2 -33 3 -16 18 -18 159 -21 168 -3 188 3 174 48 -6 19 -14 20 -166 20 -130 0 -161 -3 -165 -14z""/><path d=""M668 728 c-3 -7 -3 -21 0 -31 4 -16 23 -17 256 -15 l251 3 0 25 0 25 -252 3 c-196 2 -252 0 -255 -10z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'REV':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M500 1420 l0 -50 40 0 c29 0 40 4 40 15 0 13 43 15 330 15 l330 0 0 -395 0 -395 -60 0 -60 0 0 -55 0 -55 -270 0 -270 0 0 115 0 115 -40 0 -40 0 0 -150 0 -150 337 0 338 0 72 72 73 72 0 448 0 448 -410 0 -410 0 0 -50z""/><path d=""M315 1320 c-45 -5 -108 -18 -140 -30 -33 -12 -80 -25 -105 -31 -25 -5 -51 -17 -57 -26 -9 -12 -13 -70 -13 -204 0 -183 1 -188 22 -203 12 -9 34 -16 48 -16 18 0 68 -31 150 -92 105 -79 129 -93 161 -93 65 0 110 64 88 122 l-9 23 121 0 c85 0 128 4 148 15 30 15 55 62 49 91 -2 11 9 30 27 45 24 22 30 35 30 69 0 34 -6 47 -30 68 -21 18 -29 32 -27 52 4 31 -40 90 -66 90 -13 0 -17 9 -18 36 0 24 -9 43 -27 62 -27 26 -31 27 -150 29 -67 0 -158 -3 -202 -7z m303 -87 c-3 -15 -18 -19 -87 -23 -65 -4 -86 -9 -95 -22 -8 -13 -8 -23 0 -35 9 -15 31 -19 135 -23 107 -4 124 -7 127 -22 3 -16 -10 -18 -124 -20 -108 -3 -128 -6 -138 -20 -8 -13 -8 -23 0 -35 10 -16 33 -19 165 -23 136 -4 154 -7 157 -22 3 -17 -9 -18 -147 -18 -123 0 -152 -3 -165 -16 -19 -19 -20 -38 -3 -52 7 -7 66 -13 133 -14 111 -3 119 -4 119 -23 0 -19 -8 -20 -175 -25 -170 -5 -175 -6 -178 -27 -2 -12 9 -35 27 -56 17 -19 31 -40 31 -46 0 -26 -38 -6 -149 79 -92 70 -124 90 -149 90 l-33 0 3 152 3 153 35 5 c19 4 46 12 58 19 48 26 150 39 300 40 141 1 153 0 150 -16z""/><path d=""M1360 1291 l0 -39 148 -4 c119 -4 158 -9 207 -27 33 -12 76 -26 95 -29 l35 -7 3 -153 3 -152 -33 0 c-25 0 -57 -20 -149 -90 -111 -85 -149 -105 -149 -79 0 6 14 27 31 46 18 21 29 44 27 56 -3 21 -9 22 -110 25 l-108 3 0 -36 0 -35 50 0 c48 0 49 -1 41 -23 -22 -58 23 -122 88 -122 32 0 56 14 161 93 82 61 132 92 150 92 14 0 36 7 48 16 21 15 22 20 22 203 0 134 -4 192 -12 204 -7 9 -33 21 -58 26 -25 6 -71 19 -103 31 -60 21 -203 39 -319 40 l-68 0 0 -39z""/><path d=""M734 1255 c-16 -41 -4 -44 203 -47 109 -2 206 1 216 5 11 5 17 17 15 30 -3 22 -6 22 -216 25 -183 2 -213 0 -218 -13z""/><path d=""M1360 1170 l0 -41 56 3 c64 3 88 23 68 56 -8 12 -27 18 -68 20 l-56 3 0 -41z""/><path d=""M830 1110 l0 -30 169 0 c104 0 172 4 176 10 3 6 3 19 0 30 -6 19 -15 20 -176 20 l-169 0 0 -30z""/><path d=""M1360 1050 l0 -41 56 3 c64 3 88 23 68 56 -8 12 -27 18 -68 20 l-56 3 0 -41z""/><path d=""M902 1001 c-8 -4 -12 -19 -10 -32 3 -24 4 -24 140 -27 131 -2 137 -2 143 18 13 42 -9 50 -139 50 -66 0 -127 -4 -134 -9z""/><path d=""M1360 930 l0 -40 53 0 c71 0 98 28 61 64 -10 10 -33 16 -65 16 l-49 0 0 -40z""/><path d=""M844 866 c-3 -8 -4 -23 -2 -33 3 -16 18 -18 159 -21 168 -3 188 3 174 48 -6 19 -14 20 -166 20 -130 0 -161 -3 -165 -14z""/><path d=""M668 728 c-3 -7 -3 -21 0 -31 4 -16 23 -17 256 -15 l251 3 0 25 0 25 -252 3 c-196 2 -252 0 -255 -10z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ship':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 480.002 480.002""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""> <g> <g> <path d=""M472.002,264.001h-48v-152c0-4.418-3.582-8-8-8h-24V88.289c-0.018-13.407-10.881-24.27-24.288-24.288h-31.712v16h31.712 c4.576,0.004,8.284,3.712,8.288,8.288v76.4l-16,16l-18.344-18.344c-3.124-3.123-8.188-3.123-11.312,0l-10.344,10.344V88.001 c0-13.255-10.745-24-24-24s-24,10.745-24,24h16c0-4.418,3.582-8,8-8s8,3.582,8,8v16h-48c-4.418,0-8,3.582-8,8v152h-20.688 l-37.656-37.656c-1.5-1.5-3.534-2.344-5.656-2.344h-48v-88c0-4.418-3.582-8-8-8h-72c-4.418,0-8,3.582-8,8v88h-40 C3.584,224,0.001,227.581,0,231.999c0,0.934,0.163,1.861,0.482,2.738l54.28,149.264h-22.76v16h28.576l3.904,10.736 c1.151,3.161,4.156,5.265,7.52,5.264h400c4.418,0,8-3.582,8-8v-136C480.002,267.583,476.421,264.001,472.002,264.001z  M264.002,120.001h40v72c0.001,4.418,3.583,7.999,8.002,7.998c2.121,0,4.154-0.843,5.654-2.342l18.344-18.344l18.344,18.344 c3.124,3.123,8.188,3.123,11.312,0l24-24c1.5-1.5,2.344-3.534,2.344-5.656v-48h16v144h-144V120.001z M64.002,144.001h56v16h-56 V144.001z M64.002,176.001h56v48h-56V176.001z M464.002,400.001h-386.4l-11.632-32h398.032V400.001z M464.002,352.001H60.146 l-40.72-112h36.576H180.69l37.656,37.656c1.5,1.5,3.534,2.344,5.656,2.344h240V352.001z""/> </g> </g> <g> <g> <path d=""M96.002,272.001c-13.255,0-24,10.745-24,24s10.745,24,24,24s24-10.745,24-24S109.257,272.001,96.002,272.001z  M96.002,304.001c-4.418,0-8-3.582-8-8s3.582-8,8-8s8,3.582,8,8S100.421,304.001,96.002,304.001z""/> </g> </g> <g> <g> <rect x=""368.002"" y=""312.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""400.002"" y=""312.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""384.002"" y=""296.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""416.002"" y=""296.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""280.002"" y=""232.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""312.002"" y=""232.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""0.002"" y=""384.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""368.002"" y=""432.001"" width=""48"" height=""16""/> </g> </g> <g> <g> <rect x=""432.002"" y=""432.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""176.002"" y=""448.001"" width=""40"" height=""16""/> </g> </g> <g> <g> <rect x=""232.002"" y=""448.001"" width=""16"" height=""16""/> </g> </g> <g> <g> <rect x=""264.002"" y=""448.001"" width=""40"" height=""16""/> </g> </g> <g> <g> <path d=""M180.002,16.001h-20v16h20c6.627,0,12,5.373,12,12s-5.373,12-12,12h-76.248c-26.36,0.031-47.721,21.392-47.752,47.752 v8.248h16v-8.248c0.022-17.527,14.225-31.73,31.752-31.752h76.248c15.464,0,28-12.536,28-28S195.466,16.001,180.002,16.001z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'DOC':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M180 1175 l0 -745 468 0 467 0 108 103 107 102 0 643 0 642 -575 0 -575 0 0 -745z m1040 80 l0 -555 -85 0 -85 0 0 -80 0 -80 -380 0 -380 0 0 635 0 635 465 0 465 0 0 -555z""/><path d=""M582 1488 c-32 -32 -14 -61 115 -190 l128 -128 -128 -128 c-96 -96 -127 -134 -127 -153 0 -32 17 -49 50 -49 36 0 330 294 330 330 0 19 -37 62 -153 178 -155 155 -182 173 -215 140z""/><path d=""M1370 1645 c0 -48 2 -55 20 -55 20 0 20 -7 20 -560 l0 -560 -80 0 -80 0 0 -80 0 -80 -385 0 -385 0 0 35 0 35 -55 0 -55 0 0 -90 0 -90 468 0 467 0 108 103 107 102 0 648 0 647 -75 0 -75 0 0 -55z""/><path d=""M1570 1435 c0 -48 2 -55 20 -55 20 0 20 -7 20 -560 l0 -560 -80 0 -80 0 0 -75 0 -75 -385 0 -385 0 0 35 0 35 -55 0 -55 0 0 -90 0 -90 473 0 472 0 103 96 102 96 0 649 0 649 -75 0 -75 0 0 -55z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'files':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M180 1175 l0 -745 468 0 467 0 108 103 107 102 0 643 0 642 -575 0 -575 0 0 -745z m1040 80 l0 -555 -85 0 -85 0 0 -80 0 -80 -380 0 -380 0 0 635 0 635 465 0 465 0 0 -555z""/><path d=""M582 1488 c-32 -32 -14 -61 115 -190 l128 -128 -128 -128 c-96 -96 -127 -134 -127 -153 0 -32 17 -49 50 -49 36 0 330 294 330 330 0 19 -37 62 -153 178 -155 155 -182 173 -215 140z""/><path d=""M1370 1645 c0 -48 2 -55 20 -55 20 0 20 -7 20 -560 l0 -560 -80 0 -80 0 0 -80 0 -80 -385 0 -385 0 0 35 0 35 -55 0 -55 0 0 -90 0 -90 468 0 467 0 108 103 107 102 0 648 0 647 -75 0 -75 0 0 -55z""/><path d=""M1570 1435 c0 -48 2 -55 20 -55 20 0 20 -7 20 -560 l0 -560 -80 0 -80 0 0 -75 0 -75 -385 0 -385 0 0 35 0 35 -55 0 -55 0 0 -90 0 -90 473 0 472 0 103 96 102 96 0 649 0 649 -75 0 -75 0 0 -55z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ATA':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 512 512"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" xml:space=""preserve""> <g> <g> <path d=""M378.24,243.712l-96-80c-4.768-3.968-11.424-4.832-17.024-2.208C259.584,164.128,256,169.792,256,176v48H16 c-8.832,0-16,7.168-16,16v32c0,8.832,7.168,16,16,16h240v48c0,6.208,3.584,11.84,9.216,14.496c2.144,0.992,4.48,1.504,6.784,1.504 c3.68,0,7.328-1.248,10.24-3.712l96-80c3.68-3.04,5.76-7.552,5.76-12.288C384,251.264,381.92,246.752,378.24,243.712z""/> </g> </g> <g> <g> <path d=""M480,0H32C14.336,0,0,14.336,0,32v160h64V64h384v384H64V320H0v160c0,17.696,14.336,32,32,32h448c17.696,0,32-14.304,32-32 V32C512,14.336,497.696,0,480,0z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'entry':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 512 512"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" xml:space=""preserve""> <g> <g> <path d=""M378.24,243.712l-96-80c-4.768-3.968-11.424-4.832-17.024-2.208C259.584,164.128,256,169.792,256,176v48H16 c-8.832,0-16,7.168-16,16v32c0,8.832,7.168,16,16,16h240v48c0,6.208,3.584,11.84,9.216,14.496c2.144,0.992,4.48,1.504,6.784,1.504 c3.68,0,7.328-1.248,10.24-3.712l96-80c3.68-3.04,5.76-7.552,5.76-12.288C384,251.264,381.92,246.752,378.24,243.712z""/> </g> </g> <g> <g> <path d=""M480,0H32C14.336,0,0,14.336,0,32v160h64V64h384v384H64V320H0v160c0,17.696,14.336,32,32,32h448c17.696,0,32-14.304,32-32 V32C512,14.336,497.696,0,480,0z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'export':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 512 512""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""> <g> <g> <path d=""M507.296,4.704c-3.36-3.36-8.032-5.056-12.768-4.64L370.08,11.392c-6.176,0.576-11.488,4.672-13.6,10.496 s-0.672,12.384,3.712,16.768l33.952,33.952L224.448,242.272c-6.24,6.24-6.24,16.384,0,22.624l22.624,22.624 c6.272,6.272,16.384,6.272,22.656,0.032l169.696-169.696l33.952,33.952c4.384,4.384,10.912,5.824,16.768,3.744 c2.24-0.832,4.224-2.112,5.856-3.744c2.592-2.592,4.288-6.048,4.608-9.888l11.328-124.448 C512.352,12.736,510.656,8.064,507.296,4.704z""/> </g> </g> <g> <g> <path d=""M448,192v256H64V64h256V0H32C14.304,0,0,14.304,0,32v448c0,17.664,14.304,32,32,32h448c17.664,0,32-14.336,32-32V192H448z ""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'import':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 512 512""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""> <g> <g> <path d=""M287.52,224.48c-3.36-3.36-8-5.088-12.736-4.64l-124.448,11.296c-6.176,0.576-11.52,4.672-13.6,10.496 c-2.112,5.856-0.672,12.384,3.712,16.768l33.952,33.952L4.704,462.048c-6.24,6.24-6.24,16.384,0,22.624l22.624,22.624 c6.24,6.272,16.352,6.272,22.624,0L219.648,337.6l33.952,33.952c4.384,4.384,10.912,5.824,16.768,3.744 c2.24-0.832,4.224-2.112,5.856-3.744c2.592-2.592,4.288-6.048,4.608-9.888l11.328-124.448 C292.608,232.48,290.88,227.84,287.52,224.48z""/> </g> </g> <g> <g> <path d=""M480,0H32C14.336,0,0,14.336,0,32v320h64V64h384v384H160v64h320c17.696,0,32-14.304,32-32V32C512,14.336,497.696,0,480,0z ""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'icon-pdf':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Layer_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 512 512""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""> <path style=""fill:#E2E5E7;"" d=""M128,0c-17.6,0-32,14.4-32,32v448c0,17.6,14.4,32,32,32h320c17.6,0,32-14.4,32-32V128L352,0H128z""/> <path style=""fill:#B0B7BD;"" d=""M384,128h96L352,0v96C352,113.6,366.4,128,384,128z""/> <polygon style=""fill:#CAD1D8;"" points=""480,224 384,128 480,128 ""/> <path style=""fill:#50BEE8;"" d=""M416,416c0,8.8-7.2,16-16,16H48c-8.8,0-16-7.2-16-16V256c0-8.8,7.2-16,16-16h352c8.8,0,16,7.2,16,16 V416z""/> <g> <path style=""fill:#FFFFFF;"" d=""M131.28,326.176l22.272-27.888c6.64-8.688,19.568,2.432,12.288,10.752 c-7.664,9.088-15.728,18.944-23.424,29.024l26.112,32.496c7.024,9.6-7.04,18.816-13.952,9.344l-23.536-30.192l-23.152,30.832 c-6.528,9.328-20.992-1.152-13.68-9.856l25.712-32.624c-8.064-10.096-15.872-19.936-23.664-29.024 c-8.064-9.6,6.912-19.44,12.784-10.48L131.28,326.176z""/> <path style=""fill:#FFFFFF;"" d=""M201.264,327.84v47.328c0,5.648-4.608,8.832-9.2,8.832c-4.096,0-7.68-3.184-7.68-8.832v-72.016 c0-6.656,5.648-8.848,7.68-8.848c3.696,0,5.872,2.192,8.048,4.624l28.16,37.984l29.152-39.408c4.24-5.232,14.592-3.2,14.592,5.648 v72.016c0,5.648-3.584,8.832-7.664,8.832c-4.608,0-8.192-3.184-8.192-8.832V327.84l-21.248,26.864 c-4.592,5.648-10.352,5.648-14.576,0L201.264,327.84z""/> <path style=""fill:#FFFFFF;"" d=""M294.288,303.152c0-4.224,3.584-7.808,8.064-7.808c4.096,0,7.552,3.6,7.552,7.808v64.096h34.8 c12.528,0,12.8,16.752,0,16.752h-42.336c-4.48,0-8.064-3.184-8.064-7.808v-73.04H294.288z""/> </g> <path style=""fill:#CAD1D8;"" d=""M400,432H96v16h304c8.8,0,16-7.2,16-16v-16C416,424.8,408.8,432,400,432z""/> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'icon-xml':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Layer_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 512 512""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""> <path style=""fill:#E2E5E7;"" d=""M128,0c-17.6,0-32,14.4-32,32v448c0,17.6,14.4,32,32,32h320c17.6,0,32-14.4,32-32V128L352,0H128z""/> <path style=""fill:#B0B7BD;"" d=""M384,128h96L352,0v96C352,113.6,366.4,128,384,128z""/> <polygon style=""fill:#CAD1D8;"" points=""480,224 384,128 480,128 ""/> <path style=""fill:#50BEE8;"" d=""M416,416c0,8.8-7.2,16-16,16H48c-8.8,0-16-7.2-16-16V256c0-8.8,7.2-16,16-16h352c8.8,0,16,7.2,16,16 V416z""/> <g> <path style=""fill:#FFFFFF;"" d=""M131.28,326.176l22.272-27.888c6.64-8.688,19.568,2.432,12.288,10.752 c-7.664,9.088-15.728,18.944-23.424,29.024l26.112,32.496c7.024,9.6-7.04,18.816-13.952,9.344l-23.536-30.192l-23.152,30.832 c-6.528,9.328-20.992-1.152-13.68-9.856l25.712-32.624c-8.064-10.096-15.872-19.936-23.664-29.024 c-8.064-9.6,6.912-19.44,12.784-10.48L131.28,326.176z""/> <path style=""fill:#FFFFFF;"" d=""M201.264,327.84v47.328c0,5.648-4.608,8.832-9.2,8.832c-4.096,0-7.68-3.184-7.68-8.832v-72.016 c0-6.656,5.648-8.848,7.68-8.848c3.696,0,5.872,2.192,8.048,4.624l28.16,37.984l29.152-39.408c4.24-5.232,14.592-3.2,14.592,5.648 v72.016c0,5.648-3.584,8.832-7.664,8.832c-4.608,0-8.192-3.184-8.192-8.832V327.84l-21.248,26.864 c-4.592,5.648-10.352,5.648-14.576,0L201.264,327.84z""/> <path style=""fill:#FFFFFF;"" d=""M294.288,303.152c0-4.224,3.584-7.808,8.064-7.808c4.096,0,7.552,3.6,7.552,7.808v64.096h34.8 c12.528,0,12.8,16.752,0,16.752h-42.336c-4.48,0-8.064-3.184-8.064-7.808v-73.04H294.288z""/> </g> <path style=""fill:#CAD1D8;"" d=""M400,432H96v16h304c8.8,0,16-7.2,16-16v-16C416,424.8,408.8,432,400,432z""/> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'SEA':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" width=""612px"" height=""612px"" viewBox=""0 0 612 612""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""> <g> <path d=""M199.944,36.46c0-20.136,16.323-36.46,36.46-36.46c16.756,0,30.724,11.368,34.99,26.766 c0.497-0.026,0.965-0.148,1.469-0.148c15.137,0,27.407,12.271,27.407,27.408c0,3.692-0.753,7.204-2.076,10.418l0.011-0.001 c9.384,0,16.991,7.606,16.991,16.99c0,9.383-7.606,16.991-16.991,16.991c-9.383,0-16.99-7.606-16.99-16.991 c0-0.507,0.105-0.985,0.149-1.481c-2.691,0.883-5.514,1.481-8.5,1.481c-9.152,0-17.205-4.531-22.183-11.425 c-4.386,1.869-9.208,2.912-14.277,2.912C216.268,72.92,199.944,56.596,199.944,36.46z M216.492,217.077h-49.937 c-8.761,0-15.863,7.102-15.863,15.863v106.207l155.305-74.393l155.311,74.393V232.94c0-8.761-7.102-15.863-15.862-15.863h-49.938  M395.508,201.214v-9.721c0-8.761-7.102-15.863-15.862-15.863h-38.529l-6.889-63.13h-56.455l-6.888,63.13h-38.529 c-8.761,0-15.863,7.102-15.863,15.863v9.721H395.508z M306,282.345L75.247,392.877l68.331,199.338 c31.721-14.751,72.487-28.507,114.275-28.507c67.151,0,121.121,31.984,192.013,48.292h11.775l75.112-219.123L306,282.345z""/> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'AIR':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" width=""510px"" height=""510px"" viewBox=""0 0 510 510""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""> <g> <g id=""flights""> <path d=""M510,255c0-20.4-17.85-38.25-38.25-38.25H331.5L204,12.75h-51l63.75,204H76.5l-38.25-51H0L25.5,255L0,344.25h38.25 l38.25-51h140.25l-63.75,204h51l127.5-204h140.25C492.15,293.25,510,275.4,510,255z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ROA':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" width=""612px"" height=""612px"" viewBox=""0 0 612 612""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""> <g> <path d=""M541.322,500.219v-94.372c0-20.277-16.438-36.716-36.715-36.716h-9.598V24.598c0-3.082-1.547-5.958-4.117-7.657 L467.587,1.537c-6.103-4.033-14.239,0.342-14.239,7.657v110.652l-6.945-18.734c-9.34-25.196-33.373-41.918-60.245-41.918H225.702 c-27.03,0-51.169,16.916-60.394,42.323l-6.655,18.329V9.194c0-7.314-8.137-11.69-14.24-7.657L121.107,16.94 c-2.571,1.699-4.118,4.575-4.118,7.657v344.534h-9.597c-20.277,0-36.715,16.438-36.715,36.716v94.372H55.035 c-5.069,0-9.178,4.109-9.178,9.179v50.743c0,5.069,4.109,9.179,9.178,9.179h39.598v24.322c0,10.139,8.219,18.357,18.358,18.357 h48.645c10.139,0,18.358-8.219,18.358-18.357V569.32h252.014v24.322c0,10.139,8.22,18.357,18.357,18.357h48.646 c10.139,0,18.357-8.219,18.357-18.357V569.32h39.598c5.07,0,9.179-4.11,9.179-9.179v-50.742c0-5.07-4.109-9.179-9.179-9.179 L541.322,500.219L541.322,500.219z M170.814,170.975h270.372v90.44H170.814V170.975z M164.527,474.533H133.17 c-9.581,0-17.348-7.768-17.348-17.349v-0.438c0-9.581,7.767-17.348,17.348-17.348h31.356c9.581,0,17.348,7.767,17.348,17.348v0.438 C181.875,466.765,174.108,474.533,164.527,474.533z M368.398,479.648H243.602c-10.139,0-18.358-8.22-18.358-18.357V344.976 c0-10.138,8.219-18.357,18.358-18.357h124.796c10.138,0,18.357,8.22,18.357,18.357v116.314 C386.756,471.428,378.536,479.648,368.398,479.648z M478.829,474.533h-31.356c-9.58,0-17.348-7.768-17.348-17.349v-0.438 c0-9.581,7.768-17.348,17.348-17.348h31.356c9.581,0,17.349,7.767,17.349,17.348v0.438 C496.178,466.765,488.41,474.533,478.829,474.533z M365.607,393.801H246.099c-5.019,0-9.087-4.068-9.087-9.088v-0.184 c0-5.019,4.068-9.086,9.087-9.086h119.508c5.019,0,9.087,4.067,9.087,9.086v0.184C374.694,389.733,370.626,393.801,365.607,393.801 z M365.607,357.085H246.099c-5.019,0-9.087-4.068-9.087-9.087v-0.184c0-5.018,4.068-9.086,9.087-9.086h119.508 c5.019,0,9.087,4.068,9.087,9.086v0.184C374.694,353.017,370.626,357.085,365.607,357.085z M365.607,467.232H246.099 c-5.019,0-9.087-4.068-9.087-9.087v-0.184c0-5.019,4.068-9.087,9.087-9.087h119.508c5.019,0,9.087,4.068,9.087,9.087v0.184 C374.694,463.164,370.626,467.232,365.607,467.232z M365.607,430.516H246.099c-5.019,0-9.087-4.068-9.087-9.086v-0.184 c0-5.019,4.068-9.087,9.087-9.087h119.508c5.019,0,9.087,4.068,9.087,9.087v0.184C374.694,426.448,370.626,430.516,365.607,430.516 z""/> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'RAI':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.1"" id=""Capa_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" width=""410.121px"" height=""410.121px"" viewBox=""0 0 410.121 410.121""  class = ""iEntidadDatos-EstatusTracking_FreightForwarder""xml:space=""preserve""> <g> <g> <path d=""M319.228,116.471c-2.277-0.59-6.785-1.715-6.785-1.715l-0.012-50.163c0-10.677-8.771-20.485-24.299-24.461 c-14.918-3.819-26.834-7.109-49.414-8.599c3.793-3.295,6.203-8.142,6.203-13.562C244.921,8.047,236.874,0,226.947,0 c-9.924,0-17.971,8.046-17.971,17.971c0,4.952,2.004,9.433,5.242,12.684h-18.314c3.238-3.251,5.242-7.732,5.242-12.684 C201.146,8.047,193.102,0,183.175,0c-9.926,0-17.971,8.046-17.971,17.971c0,5.419,2.408,10.267,6.203,13.562 c-22.582,1.49-34.496,4.78-49.414,8.599c-15.527,3.976-24.299,13.784-24.299,24.461l-0.014,50.163c0,0-4.508,1.125-6.783,1.715 c-5.418,1.405-9.199,6.293-9.199,11.889v151.906c0,6.783,5.498,12.281,12.281,12.281h222.162c6.783,0,12.281-5.499,12.281-12.281 V128.359C328.425,122.764,324.644,117.875,319.228,116.471z M125.955,68.351c20.594-5.1,41.52-8.215,62.33-9.282v39.196 c-20.803,1.036-41.721,4.061-62.33,9.012V68.351z M205.472,177.957h-0.818c-14.5,0-26.297-11.797-26.297-26.3 c0-14.501,11.797-26.299,26.297-26.299h0.818c14.502,0,26.299,11.798,26.299,26.299 C231.771,166.16,219.974,177.957,205.472,177.957z M284.171,107.277c-20.607-4.952-41.527-7.976-62.33-9.012V59.069 c20.811,1.067,41.735,4.182,62.33,9.282V107.277L284.171,107.277z""/> <path d=""M232.228,350.645h-27.164h-27.166l-7.121,20.02c11.303,0.963,22.605,1.457,33.877,1.457h0.818 c11.271,0,22.576-0.494,33.877-1.457L232.228,350.645z""/> <path d=""M316.267,348.188v-31.004c0-6.113-4.955-11.066-11.066-11.066H104.924c-6.111,0-11.066,4.955-11.066,11.066v31.004 c0,5.041,3.41,9.447,8.289,10.711c3.113,0.807,6.236,1.568,9.359,2.301L98.068,391.66c-2.932,6.645,0.076,14.408,6.721,17.338 c1.727,0.762,3.529,1.123,5.303,1.123c5.051,0,9.867-2.928,12.037-7.846l15.777-35.766c3.795,0.635,7.594,1.219,11.395,1.748 l11.262-31.658c1.49-4.194,5.461-6.994,9.91-6.994h69.178c4.451,0,8.42,2.8,9.912,6.994l11.26,31.658 c3.801-0.529,7.601-1.113,11.396-1.748l15.776,35.766c2.17,4.918,6.986,7.846,12.039,7.846c1.771,0,3.574-0.361,5.302-1.123 c6.645-2.932,9.651-10.693,6.723-17.338l-13.438-30.461c3.123-0.732,6.244-1.494,9.357-2.301 C312.856,357.635,316.267,353.229,316.267,348.188z""/> </g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> <g> </g> </svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'SSE':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M35 1729 l-35 -31 0 -738 0 -738 35 -31 36 -31 585 0 586 0 34 34 34 34 0 135 0 135 -43 63 c-24 34 -48 64 -53 66 -5 2 -8 -51 -6 -129 2 -72 1 -146 -3 -165 -12 -65 2 -63 -556 -64 l-504 0 -23 26 -22 26 0 640 0 640 25 24 24 25 500 0 c275 0 507 -3 516 -6 35 -14 45 -58 45 -202 l0 -138 51 -53 c28 -29 51 -51 52 -49 1 2 0 115 -3 251 l-5 249 -33 29 -32 29 -585 0 -584 0 -36 -31z""/><path d=""M200 1485 c-10 -12 -10 -21 -2 -40 l12 -25 447 2 448 3 0 35 0 35 -446 3 c-392 2 -448 0 -459 -13z""/><path d=""M200 1280 c-8 -14 -8 -26 0 -40 10 -19 21 -20 397 -20 l385 0 -6 31 c-3 17 -6 35 -6 40 0 5 -169 9 -380 9 -369 0 -380 -1 -390 -20z""/><path d=""M1016 1254 c-18 -18 -20 -30 -6 -39 6 -3 10 -36 10 -73 0 -45 7 -80 20 -107 28 -59 243 -350 259 -350 18 0 220 157 221 172 0 6 -3 13 -8 15 -8 3 -162 181 -226 260 -47 59 -87 84 -156 99 -30 6 -60 18 -68 25 -17 18 -27 18 -46 -2z""/><path d=""M206 1114 c-19 -18 -20 -28 -6 -55 10 -18 25 -19 386 -19 339 0 376 2 370 16 -3 9 -6 29 -6 45 l0 29 -364 0 c-318 0 -367 -2 -380 -16z""/><path d=""M200 935 c-13 -15 -5 -55 14 -67 18 -13 826 -10 826 3 0 6 -12 26 -26 45 l-26 34 -388 0 c-327 0 -389 -2 -400 -15z""/><path d=""M1703 823 c-28 -11 -9 -49 73 -154 47 -60 90 -112 95 -115 5 -3 18 -2 29 1 39 12 23 51 -66 165 -81 104 -98 117 -131 103z""/><path d=""M1443 712 c-67 -53 -122 -101 -123 -106 0 -10 188 -252 245 -315 66 -73 132 -89 212 -50 83 40 115 138 73 223 -28 56 -252 340 -270 343 -8 1 -70 -42 -137 -95z""/><path d=""M206 744 c-19 -18 -19 -16 -6 -48 l10 -26 439 0 c386 0 441 2 455 16 20 20 20 38 0 58 -14 14 -68 16 -449 16 -381 0 -435 -2 -449 -16z""/><path d=""M216 514 c-15 -7 -20 -19 -19 -38 1 -16 5 -31 8 -34 11 -12 888 -2 902 10 17 14 17 43 0 56 -18 16 -863 21 -891 6z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'BOO':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M433 1672 l-433 -116 0 -723 c0 -593 2 -723 13 -723 32 0 882 232 889 243 4 7 8 333 8 725 l0 712 -22 -1 c-13 0 -218 -53 -455 -117z m142 -411 c99 -44 124 -178 44 -245 l-30 -25 32 -29 c28 -25 33 -37 36 -88 5 -76 -18 -116 -85 -147 -40 -19 -65 -22 -165 -22 -106 0 -120 2 -133 19 -11 16 -14 70 -14 270 0 138 4 257 9 265 23 35 228 37 306 2z""/><path d=""M360 1116 l0 -96 64 0 c109 0 146 26 146 104 0 55 -34 77 -129 83 l-81 6 0 -97z""/><path d=""M360 865 l0 -85 58 0 c117 0 164 35 147 109 -10 48 -38 61 -127 61 l-78 0 0 -85z""/><path d=""M1020 1065 c0 -399 2 -725 5 -725 3 0 200 -52 438 -115 238 -63 438 -115 445 -115 9 0 12 168 12 723 l0 724 -438 116 c-241 65 -443 117 -450 117 -9 0 -12 -161 -12 -725z m370 81 l5 -139 94 141 94 142 59 0 c45 0 57 -3 52 -12 -9 -14 -181 -255 -195 -274 -6 -7 27 -58 95 -145 57 -74 102 -137 99 -141 -6 -11 -71 -10 -92 2 -10 5 -61 66 -112 135 l-94 126 -3 -130 c-3 -128 -3 -131 -26 -137 -13 -3 -35 -4 -47 -2 l-24 3 -3 275 c-1 151 0 281 3 288 3 9 19 12 47 10 l43 -3 5 -139z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'PPL':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M708 1403 c-14 -9 -36 -34 -48 -55 -21 -37 -23 -38 -76 -38 l-54 0 0 -45 c0 -38 3 -45 20 -45 20 0 20 -7 20 -341 0 -215 4 -347 10 -360 10 -18 26 -19 433 -19 316 0 426 3 435 12 9 9 12 102 12 355 0 336 0 343 20 343 19 0 20 -7 20 -209 0 -181 2 -210 16 -215 9 -3 61 -6 116 -6 l100 0 84 82 84 82 0 138 c0 85 4 138 10 138 6 0 10 20 10 45 l0 45 -40 0 c-34 0 -41 4 -46 25 -3 14 -22 38 -40 55 -30 26 -42 30 -94 30 -52 0 -64 -4 -95 -31 -19 -17 -38 -42 -41 -55 l-6 -24 -194 0 -194 0 -14 35 c-31 73 -145 100 -217 52 l-34 -23 -34 23 c-41 28 -122 31 -163 6z m122 -83 c24 -24 25 -48 4 -78 -31 -44 -104 -20 -104 35 0 57 61 82 100 43z m240 0 c11 -11 20 -29 20 -40 0 -42 -62 -75 -96 -52 -18 12 -35 52 -28 65 29 50 70 61 104 27z m670 0 c24 -24 26 -57 3 -82 -34 -38 -103 -11 -103 40 0 56 61 81 100 42z m50 -327 c0 -10 -21 -39 -47 -65 -45 -45 -50 -48 -100 -48 l-53 0 0 65 0 65 100 0 c88 0 100 -2 100 -17z""/><path d=""M12 1228 c-15 -15 -16 -235 -1 -275 13 -34 55 -80 91 -99 14 -7 51 -17 81 -20 l55 -6 -17 -29 c-14 -26 -15 -31 -1 -56 11 -20 25 -29 49 -31 30 -3 41 5 112 75 56 56 79 86 79 104 -1 16 -26 51 -79 107 -85 89 -113 100 -151 62 -24 -24 -25 -48 -4 -78 15 -22 14 -22 -24 -22 -27 0 -45 7 -60 23 -19 21 -22 35 -22 118 0 112 -12 139 -64 139 -17 0 -37 -5 -44 -12z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ZAR':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M1447 1852 c-32 -35 -21 -69 40 -133 l56 -59 -666 0 -666 0 -15 -22 c-9 -12 -16 -32 -16 -43 0 -11 7 -31 16 -43 l15 -22 667 0 666 0 -57 -58 c-45 -46 -57 -64 -57 -89 0 -41 23 -63 67 -63 32 0 47 11 145 108 108 105 138 144 138 178 0 10 -55 73 -122 141 -112 113 -125 123 -158 123 -23 0 -43 -7 -53 -18z""/><path d=""M100 1188 c-13 -31 -100 -363 -100 -382 0 -13 16 -16 100 -16 l100 0 0 -168 c0 -117 4 -172 12 -180 9 -9 75 -12 235 -12 l223 0 0 -90 0 -90 130 0 130 0 0 90 0 90 63 0 c36 0 68 5 75 12 8 8 12 63 12 180 l0 168 420 0 c375 0 420 2 420 16 0 19 -87 350 -100 382 l-10 22 -850 0 -850 0 -10 -22z""/><path d=""M1190 695 l0 -65 125 0 125 0 0 65 0 65 -125 0 -125 0 0 -65z""/><path d=""M1480 695 l0 -65 125 0 125 0 0 65 0 65 -125 0 -125 0 0 -65z""/><path d=""M1180 583 c0 -10 3 -42 6 -70 l7 -53 123 0 124 0 0 70 0 70 -130 0 c-117 0 -130 -2 -130 -17z""/><path d=""M1480 535 l0 -65 125 0 125 0 0 65 0 65 -125 0 -125 0 0 -65z""/><path d=""M670 115 l0 -66 128 3 127 3 3 63 3 62 -131 0 -130 0 0 -65z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'INR':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M255 1853 c-45 -23 -71 -50 -90 -94 l-18 -39 -74 0 -73 0 0 -65 c0 -58 2 -65 20 -65 19 0 19 -10 22 -476 l3 -476 28 -24 28 -24 564 0 564 0 28 24 28 24 -4 476 -4 476 32 0 31 0 0 -294 c0 -259 2 -295 16 -300 9 -3 81 -6 160 -6 l144 0 115 111 115 112 0 188 c0 161 2 189 15 189 12 0 15 14 15 65 l0 65 -55 0 c-54 0 -55 1 -70 37 -34 82 -96 117 -196 111 -58 -3 -74 -8 -106 -34 -21 -17 -47 -50 -58 -73 l-20 -41 -271 0 -272 0 -6 25 c-9 35 -65 96 -103 111 -74 32 -191 10 -232 -42 l-18 -23 -19 20 c-51 56 -172 78 -239 42z m150 -118 c54 -53 16 -145 -60 -145 -49 0 -85 36 -85 85 0 49 36 85 85 85 25 0 44 -8 60 -25z m331 1 c59 -50 20 -146 -60 -146 -40 0 -86 41 -86 77 0 81 86 121 146 69z m939 -2 c69 -74 -18 -181 -106 -133 -31 16 -49 66 -37 101 22 61 100 79 143 32z m71 -456 c-4 -23 -26 -53 -70 -95 l-64 -63 -76 0 -76 0 0 95 0 95 146 0 147 0 -7 -32z""/><path d=""M1550 640 c-37 -37 -27 -79 36 -148 l55 -62 -796 -2 -797 -3 -19 -24 c-26 -32 -24 -77 6 -106 l24 -25 792 0 791 0 -56 -61 c-47 -51 -56 -66 -56 -98 0 -48 24 -71 75 -71 37 0 47 8 177 138 124 123 138 141 138 172 0 31 -14 49 -138 172 -128 128 -141 138 -175 138 -24 0 -44 -7 -57 -20z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'DES':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M886 1553 c-2 -10 -17 -107 -32 -215 l-27 -198 -176 92 c-97 50 -181 95 -188 100 -29 23 -69 -3 -262 -171 -144 -124 -201 -180 -201 -195 0 -17 20 -32 89 -69 48 -26 93 -47 99 -47 6 0 61 27 121 60 61 33 117 60 125 60 8 0 266 -129 575 -286 308 -157 591 -296 628 -310 49 -17 90 -24 146 -24 74 0 80 2 108 29 61 61 28 158 -78 235 -27 19 -126 76 -221 127 -95 50 -179 101 -187 113 -7 11 -70 149 -140 306 -70 157 -134 292 -141 301 -20 22 -188 109 -212 109 -11 0 -23 -8 -26 -17z""/><path d=""M626 743 c-34 -41 -56 -76 -54 -87 2 -13 39 -39 108 -78 l105 -58 150 6 c83 4 173 10 200 13 l50 6 -85 46 c-47 26 -160 85 -252 133 l-166 86 -56 -67z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'CAT':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M1008 1457 l-258 -101 0 -373 0 -373 53 21 c28 11 150 59 270 106 l217 86 0 369 c0 288 -3 368 -12 368 -7 -1 -129 -47 -270 -103z""/><path d=""M1380 1192 l0 -369 238 -93 c130 -51 252 -99 270 -107 l32 -13 0 373 0 373 -258 102 c-142 56 -264 102 -270 102 -9 0 -12 -95 -12 -368z""/><path d=""M0 1150 l0 -50 340 0 340 0 0 50 0 50 -340 0 -340 0 0 -50z""/><path d=""M94 1026 c-3 -8 -4 -29 -2 -48 l3 -33 293 -3 292 -2 0 50 0 50 -290 0 c-240 0 -292 -2 -296 -14z""/><path d=""M290 835 l0 -45 195 0 195 0 0 45 0 45 -195 0 -195 0 0 -45z""/><path d=""M1320 748 c-192 -71 -510 -202 -503 -208 14 -14 503 -180 529 -180 26 0 514 166 527 180 5 4 -85 45 -200 90 -329 130 -325 129 -353 118z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'TBD':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M336 1898 c-14 -19 -16 -66 -16 -312 l0 -289 -64 61 c-69 67 -101 77 -144 46 -17 -11 -22 -25 -22 -56 0 -40 6 -47 132 -175 179 -180 165 -180 345 0 127 127 133 135 133 173 0 50 -23 74 -71 74 -28 0 -44 -10 -96 -62 l-63 -62 0 292 c0 279 -1 293 -20 312 -29 29 -94 27 -114 -2z""/><path d=""M1180 1906 c0 -20 139 -696 150 -732 12 -36 64 -89 103 -103 36 -14 108 -14 144 0 36 13 88 64 102 98 11 25 151 712 151 738 0 10 -66 13 -325 13 -269 0 -325 -2 -325 -14z m388 -591 c74 -62 33 -175 -63 -175 -73 0 -123 76 -91 138 34 65 101 81 154 37z""/><path d=""M879 783 c-189 -229 -350 -425 -358 -436 -12 -17 -10 -22 17 -50 16 -18 39 -51 51 -75 l21 -42 143 117 c78 65 293 242 476 393 l334 275 -63 7 c-121 14 -211 79 -248 181 -9 26 -20 47 -23 47 -4 0 -161 -188 -350 -417z""/><path d=""M305 905 c-46 -25 -76 -56 -96 -100 -34 -72 -20 -135 29 -135 38 0 48 10 61 56 24 89 107 118 162 56 18 -20 29 -43 29 -61 0 -41 -43 -89 -86 -97 -61 -11 -64 -20 -64 -179 l0 -143 -34 -21 c-64 -40 -86 -142 -46 -209 60 -97 210 -97 270 0 40 66 15 173 -51 212 l-29 17 0 109 0 110 38 19 c135 69 150 248 29 345 -33 27 -48 31 -110 34 -52 2 -80 -2 -102 -13z m135 -705 c29 -29 26 -74 -6 -99 -33 -26 -45 -26 -78 0 -48 38 -22 119 39 119 14 0 34 -9 45 -20z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ABU':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M262 1747 c-67 -68 -122 -131 -122 -141 0 -34 30 -73 138 -178 98 -97 113 -108 145 -108 44 0 67 22 67 63 0 25 -12 43 -57 89 l-57 58 666 0 667 0 15 22 c20 28 20 58 0 86 l-15 22 -666 0 -666 0 56 59 c43 45 57 67 57 89 0 38 -28 62 -71 62 -31 0 -48 -13 -157 -123z""/><path d=""M100 1188 c-13 -32 -100 -363 -100 -382 0 -14 45 -16 420 -16 l420 0 0 -168 c0 -117 4 -172 12 -180 7 -7 39 -12 75 -12 l63 0 0 -90 0 -90 130 0 130 0 0 90 0 90 223 0 c160 0 226 3 235 12 8 8 12 63 12 180 l0 168 100 0 c84 0 100 3 100 16 0 19 -87 351 -100 382 l-10 22 -850 0 -850 0 -10 -22z""/><path d=""M190 695 l0 -65 125 0 125 0 0 65 0 65 -125 0 -125 0 0 -65z""/><path d=""M480 695 l0 -65 125 0 125 0 0 65 0 65 -125 0 -125 0 0 -65z""/><path d=""M190 535 l0 -65 125 0 125 0 0 65 0 65 -125 0 -125 0 0 -65z""/><path d=""M480 530 l0 -70 124 0 123 0 7 53 c11 95 22 87 -124 87 l-130 0 0 -70z""/><path d=""M992 118 l3 -63 128 -3 127 -3 0 66 0 65 -130 0 -131 0 3 -62z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'LLD':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M215 1854 c-48 -26 -67 -46 -87 -91 -19 -43 -19 -43 -73 -43 l-55 0 0 -65 c0 -51 3 -65 15 -65 13 0 15 -28 15 -189 l0 -188 115 -112 115 -111 144 0 c79 0 151 3 160 6 14 5 16 41 16 300 l0 294 31 0 32 0 -4 -476 -4 -476 28 -24 28 -24 564 0 564 0 28 24 28 24 3 476 c3 466 3 476 22 476 18 0 20 7 20 65 l0 65 -74 0 c-73 0 -74 0 -86 30 -33 79 -96 120 -185 120 -60 0 -116 -22 -149 -59 l-19 -20 -18 23 c-41 52 -158 74 -232 42 -38 -15 -94 -76 -103 -111 l-6 -25 -272 0 -271 0 -20 41 c-11 23 -37 56 -58 73 -34 27 -47 31 -110 33 -52 3 -80 -1 -102 -13z m142 -109 c32 -22 43 -65 28 -102 -36 -86 -165 -62 -165 30 0 70 80 112 137 72z m934 2 c45 -34 50 -93 13 -131 -55 -54 -148 -17 -148 59 0 29 7 43 28 61 30 26 81 31 107 11z m344 -12 c17 -16 25 -35 25 -60 0 -25 -8 -44 -25 -60 -16 -17 -35 -25 -60 -25 -25 0 -44 8 -60 25 -17 16 -25 35 -25 60 0 25 8 44 25 60 16 17 35 25 60 25 25 0 44 -8 60 -25z m-1175 -520 l0 -95 -76 0 -76 0 -64 63 c-44 42 -66 72 -70 95 l-6 32 146 0 146 0 0 -95z""/><path d=""M138 522 c-124 -123 -138 -141 -138 -172 0 -31 14 -49 138 -172 132 -132 140 -138 178 -138 51 0 74 23 74 73 0 30 -9 46 -56 96 l-56 61 791 0 792 0 24 25 c30 29 32 74 6 106 l-19 24 -797 3 -796 2 55 62 c44 49 56 68 56 95 0 45 -30 73 -77 73 -34 0 -47 -10 -175 -138z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ATER':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M1657 1754 c-57 -20 -161 -74 -347 -179 l-165 -94 -395 60 c-217 32 -403 59 -413 59 -10 0 -72 -30 -137 -66 -100 -55 -120 -70 -120 -89 0 -18 45 -62 190 -186 145 -125 187 -166 177 -175 -6 -6 -110 -66 -230 -133 l-217 -123 0 -40 c0 -35 73 -568 85 -625 8 -35 38 -29 152 34 71 38 105 63 108 77 2 12 9 86 15 166 6 79 14 148 18 152 4 4 296 176 650 382 353 206 670 395 705 419 72 51 147 129 172 177 13 24 16 49 13 93 -4 52 -9 63 -34 83 -25 21 -40 24 -109 23 -44 0 -97 -7 -118 -15z""/><path d=""M995 899 c-153 -88 -281 -162 -284 -165 -3 -2 10 -52 29 -109 30 -94 36 -105 58 -105 26 0 251 120 274 147 21 25 218 370 218 382 0 6 -3 11 -7 11 -5 -1 -134 -73 -288 -161z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'CAD':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M247 1902 c-15 -17 -17 -60 -17 -415 l0 -395 136 -4 c135 -3 136 -3 160 -31 23 -27 24 -35 24 -163 l0 -134 275 0 276 0 25 25 25 25 -3 541 c-3 522 -4 541 -22 555 -16 12 -93 14 -441 14 -396 0 -423 -1 -438 -18z""/><path d=""M1342 1716 c-37 -39 -62 -73 -62 -86 0 -24 111 -146 142 -156 26 -8 68 22 68 48 0 12 -8 30 -17 41 -18 20 -17 20 22 14 61 -10 75 -34 75 -131 0 -89 8 -106 51 -106 44 0 51 19 47 134 -2 87 -7 109 -24 132 -34 47 -69 66 -130 73 -50 5 -55 8 -41 20 20 16 22 52 5 69 -7 7 -26 12 -43 12 -26 0 -43 -12 -93 -64z""/><path d=""M1220 955 c0 -276 21 -255 -255 -255 l-205 0 0 -190 0 -190 134 0 c128 0 136 -1 163 -24 28 -24 28 -25 31 -160 l4 -136 277 0 c257 0 279 1 294 18 15 17 17 70 17 558 0 450 -2 543 -14 560 -13 18 -30 19 -230 22 l-216 3 0 -206z""/><path d=""M237 1053 c-22 -22 1 -56 117 -172 102 -102 127 -122 145 -116 20 6 21 13 21 134 0 166 5 161 -157 161 -66 0 -123 -3 -126 -7z""/><path d=""M242 658 c-8 -8 -12 -48 -12 -114 0 -88 3 -105 23 -134 34 -50 70 -72 127 -78 36 -3 48 -8 41 -15 -15 -15 -14 -60 1 -75 29 -29 63 -13 132 58 55 57 66 74 61 92 -9 33 -129 148 -153 148 -47 0 -69 -62 -35 -96 17 -17 16 -17 -22 -11 -61 10 -75 34 -75 131 0 50 -5 87 -12 94 -15 15 -61 15 -76 0z""/><path d=""M767 283 c-19 -18 3 -48 117 -162 99 -99 128 -122 147 -119 24 3 24 5 27 127 4 166 8 161 -158 161 -70 0 -130 -3 -133 -7z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            case 'ENT':" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<svg version=""1.0"" xmlns=""http://www.w3.org/2000/svg"" class = ""iEntidadDatos-EstatusTracking_FreightForwarder"" viewBox=""0 0 1920 1920"" preserveAspectRatio=""xMidYMid meet""><g id=""layer101""  stroke=""none""><path d=""M1415 1910 c-100 -31 -198 -126 -232 -225 -25 -72 -22 -184 7 -260 32 -81 113 -163 195 -195 82 -32 197 -32 278 -1 70 28 152 105 187 176 22 44 25 64 25 160 0 101 -2 114 -28 163 -35 67 -92 124 -159 159 -45 23 -67 27 -148 30 -52 1 -108 -2 -125 -7z m292 -477 l-27 -28 -100 100 -100 99 -45 -44 -45 -44 -30 29 -30 29 72 73 73 73 130 -130 130 -130 -28 -27z""/><path d=""M40 985 c0 -371 2 -675 5 -675 4 0 114 74 245 165 l240 164 0 -164 c0 -110 4 -165 11 -165 6 0 121 76 257 170 l247 170 83 0 82 0 0 -325 0 -325 195 0 195 0 0 573 0 573 -92 0 c-120 0 -185 24 -268 101 -82 75 -122 157 -128 261 -2 43 0 94 6 115 l10 37 -544 0 -544 0 0 -675z m550 10 l0 -125 -190 0 -190 0 0 125 0 125 190 0 190 0 0 -125z m610 0 l0 -125 -190 0 -190 0 0 125 0 125 190 0 190 0 0 -125z""/></g></svg>'" & vbCrLf)
                    jsAuxiliar_.Append("                            break;" & vbCrLf)
                    jsAuxiliar_.Append("                            default:" & vbCrLf)
                    jsAuxiliar_.Append("                                    return '<span class=""SinDefinir"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                        }" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.ReferenciaTracking

                    jsAuxiliar_.Append("                         return '<a href=""/FrontEnd/Modulos/TrackingOperaciones/Ges003-001-Consultas.Operaciones.aspx?referencia=' + data + '"">' + data + '</a>'; " & vbCrLf)

                Case IEntidadDatos.EstilossCCS.AtlasReferenciaAgenciaAduanal

                    jsAuxiliar_.Append("                        data = data.split("","");" & vbCrLf)
                    jsAuxiliar_.Append("                        var result = """"; " & vbCrLf)
                    jsAuxiliar_.Append("                        $.each(data, function(i,v){" & vbCrLf)
                    jsAuxiliar_.Append("                            result = result + ' ' + '<a href=""/FrontEnd/Modulos/TrackingOperaciones/Ges003-001-Consultas.Operaciones.aspx?referencia=' + v + '"">' + v + '</a>'; " & vbCrLf)
                    jsAuxiliar_.Append("                        });" & vbCrLf)
                    jsAuxiliar_.Append("                        return result" & vbCrLf)

                Case IEntidadDatos.EstilossCCS.SinDefinir

                    jsAuxiliar_.Append("                       if (data != 0){" & vbCrLf)
                    jsAuxiliar_.Append("                        return '<span class=""SinDefinir"">' + data + '</span>'" & vbCrLf)
                    jsAuxiliar_.Append("                       }else{" & vbCrLf)

                    If campo_.TipoDatoHTML = IEntidadDatos.TiposDatosHTML.Fecha Then

                        jsAuxiliar_.Append("                        return '<span class=""SinDefinir""></span>'" & vbCrLf)

                    ElseIf campo_.TipoDatoHTML = IEntidadDatos.TiposDatosHTML.Entero Or
                        campo_.TipoDatoHTML = IEntidadDatos.TiposDatosHTML.Real Then

                        jsAuxiliar_.Append("                        return '<span class=""SinDefinir"">0</span>'" & vbCrLf)

                    Else

                        jsAuxiliar_.Append("                        return '<span class=""SinDefinir"">-</span>'" & vbCrLf)

                    End If

                    jsAuxiliar_.Append("                       }" & vbCrLf)

            End Select

            Return jsAuxiliar_

        End Function

        Private Sub SessionPrepare(ByVal user_ As String, _
                                   ByVal password_ As String)

            _sesion = New SesionWcf

            _sesion.MininimoCaracteresUsuario = 7

            _sesion.MinimoNumerosUsuario = 0

            _sesion.MinimoCaracteresContrasena = 7

            _sesion.MinimoMayusculasContrasena = 2

            _sesion.MinimoMinusculasContrasena = 2

            _sesion.MinimoNumerosContrasena = 2

            _sesion.GrupoEmpresarial = 1

            _sesion.DivisionEmpresarial = 1

            _sesion.Aplicacion = 4

            _sesion.IdentificadorUsuario = Trim(user_)

            _sesion.ContraseniaUsuario = Trim(password_)

        End Sub

        Public Function LogIn(ByVal MobileUserID As String, _
                                ByVal WebServiceUserID As String, _
                                ByVal WebServicePasswordID As String, _
                                ByVal IdRequiredApplication As Integer, _
                                ByVal CorporateNumber As Integer, _
                                Optional ByVal FullAuthentication As Boolean = False) As TagWatcher

            Dim results_ As New TagWatcher

            _sessionStatus = False

            SessionPrepare(WebServiceUserID, _
                           WebServicePasswordID)

            'If _sesion.StatusArgumentos Then
            If 1 Then

                _sessionStatus = True

                _sesion.GrupoEmpresarial = CorporateNumber

                _sesion.DivisionEmpresarial = 1

                _sesion.Aplicacion = IdRequiredApplication

                _sesion.Idioma = ISesion.Idiomas.Espaniol

                If Not FullAuthentication Then

                    _iOperations = _sistema.EnsamblaModulo("Usuarios")

                    Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

                    temporalWorkSpace_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

                    With _iOperations

                        .EspacioTrabajo = temporalWorkSpace_

                        .TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                        .ClausulasLibres = " and t_usuario = '" & MobileUserID & "' and i_Cve_Estado = 1"

                        .CantidadVisibleRegistros = 1000

                        .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                        .TipoInstrumentacion = monitoreo.IBitacoras.TiposInstrumentacion.GestorIEnlaceDatos

                        .ClaveDivisionMiEmpresa = 0

                        .IDAplicacion = monitoreo.IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet

                        .IDRecursoSolicitante = 0

                        .IDUsuario = 0

                    End With

                    If _iOperations.GenerarVista() = IOperacionesCatalogo.EstadoOperacion.COk Then

                        If _sistema.TieneResultados(_iOperations) Then

                            _iOperations = _sistema.EnsamblaModulo("UsuariosAppMovil")

                            _iOperations.ClausulasLibres = " and t_Usuario = '" & MobileUserID & "' and i_Cve_Estado = 1"

                            _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                            _iOperations.TipoInstrumentacion = monitoreo.IBitacoras.TiposInstrumentacion.GestorIEnlaceDatos

                            _iOperations.ClaveDivisionMiEmpresa = 0

                            _iOperations.IDAplicacion = monitoreo.IBitacoras.ClaveTiposAplicacion.AplicacionWebExtranet

                            _iOperations.IDRecursoSolicitante = 0

                            _iOperations.IDUsuario = 0

                            If _iOperations.GenerarVista() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                If _sistema.TieneResultados(_iOperations) Then

                                    If _iOperations.Vista.Tables(0).Columns.Contains("Clave") Then

                                        For Each row_ As DataRow In _iOperations.Vista.Tables(0).Rows

                                            Dim accessRule_ As New ReglaAccesoKrom

                                            With accessRule_

                                                .IDAccesoUsuarioClavesCliente = row_("Clave")

                                                .IDDivisionMiEmpresa = row_("i_Cve_DivisionMiEmpresa")

                                                If Not DBNull.Value.Equals(row_("RFC")) Then

                                                    .RFC = row_("RFC")

                                                Else

                                                    .RFC = Nothing

                                                End If

                                                .ClaveClienteExterno =
                                                    _sistema.ValidarVacios(row_("ID Cliente Externo"),
                                                                               Organismo.VerificarTipoDatoDBNULL.Numero)
                                                'Aduana Sección,  Patente, Tipo Operación, ID Cliente Externo, ID Cliente Empresa

                                                .IDClienteEmpresa =
                                                    _sistema.ValidarVacios(row_("ID Cliente Empresa"),
                                                                               Organismo.VerificarTipoDatoDBNULL.Numero)

                                                .IDDivisionEmpresarialCliente =
                                                    _sistema.ValidarVacios(row_("ID División Cliente"),
                                                                               Organismo.VerificarTipoDatoDBNULL.Numero)

                                                .Aplicacion =
                                                    _sistema.ValidarVacios(row_("i_Cve_Aplicacion"),
                                                                               Organismo.VerificarTipoDatoDBNULL.Numero)

                                                .Accion =
                                                    _sistema.ValidarVacios(row_("i_Cve_Accion"),
                                                                               Organismo.VerificarTipoDatoDBNULL.Numero)

                                                .Aduana =
                                                    _sistema.ValidarVacios(row_("Aduana Sección"),
                                                                               Organismo.VerificarTipoDatoDBNULL.Numero)

                                                .Patente =
                                                    _sistema.ValidarVacios(row_("Patente"),
                                                                               Organismo.VerificarTipoDatoDBNULL.Numero)

                                                .TipoOperacion =
                                                    _sistema.ValidarVacios(row_("Tipo Operación"),
                                                                               Organismo.VerificarTipoDatoDBNULL.Numero)

                                                'Reglas nuevas para administración

                                                'r21.i_Cve_ClienteEmpresaFacturacion,r21.t_RFCFacturacion, r21.i_ExigirCongruenciaImportadorFacturacion
                                                .RFC_ClienteComprobanteFiscal =
                                                    _sistema.ValidarVacios(row_("t_RFCFacturacion"),
                                                                               Organismo.VerificarTipoDatoDBNULL.Cadena)


                                                .ClaveClienteComprobanteFiscal =
                                                    _sistema.ValidarVacios(row_("i_Cve_ClienteEmpresaFacturacion"),
                                                                               Organismo.VerificarTipoDatoDBNULL.Numero)


                                                .ExigirCongruenciaImportadorFacturacion =
                                                    _sistema.ValidarVacios(row_("i_ExigirCongruenciaImportadorFacturacion"),
                                                                               Organismo.VerificarTipoDatoDBNULL.Numero)

                                            End With

                                            _reglasAccesoKrom.Add(accessRule_)

                                        Next




                                        Select Case IdRequiredApplication


                                            Case 12 'Para KrombaseWeb si necesitamos conocer las reglas y los datos para los dashboards

                                                CovertRuleToScript(results_)

                                            Case 4, 13 'En synapsis necesitamos definir si utilizaremos los mismos recursos aún o si es mejor idea migrarlos

                                                _clausulasSQLReglasAcceso = " "

                                                results_.SetOK()

                                        End Select

                                    Else

                                        results_.SetError(ErrorTypes.WS007,
                                                          "4 This mobile user doesn't have client rules to access profile,  although was authenticated")

                                    End If

                                Else

                                    results_.SetError(ErrorTypes.WS007,
                                                      "3 This mobile user doesn't have client rules to access profile,  although was authenticated")

                                End If

                            Else

                                results_.SetError(ErrorTypes.WS007,
                                                  "2 This mobile user doesn't have client rules to access profile,  although was authenticated")

                            End If

                        Else

                            results_.SetError(ErrorTypes.WS007,
                                              "1 This mobile user doesn't have client rules to access profile,  although was authenticated")

                        End If

                        'Else
                        '    results_.SetError(ErrorTypes.WS007, " This mobile user doesn't have client rules to access profile,  although was authenticated")
                        'End If
                    Else

                        results_.SetError(ErrorTypes.WS001)

                    End If

                Else

                    _sessionStatus = _sesion.StatusArgumentos

                    If _sessionStatus Then

                        If _sesion.StatusArgumentos = True And _
                            _sesion.EspacioTrabajo IsNot Nothing Then

                            _iOperations = New OperacionesCatalogo

                            _iOperations = _sistema.ConsultaModulo(_sesion.EspacioTrabajo, "Usuarios", " and t_usuario = '" & MobileUserID & "'").Clone

                            If _sistema.TieneResultados(_iOperations) Then

                                results_.SetOK()

                                _sesionWebIniciada = True

                            Else
                                results_.SetError(ErrorTypes.WS002)

                            End If

                        Else

                            results_.SetError(ErrorTypes.WS003)

                        End If


                    Else

                        results_.SetError(ErrorTypes.WS004)

                    End If

                End If
            Else

                results_.SetError(ErrorTypes.WS005)

            End If

            Return results_

        End Function

        Public Function CovertRuleToScript(ByRef result_ As TagWatcher,
                                           Optional ByVal tipoSeccion_ As TipoSeccion = TipoSeccion.Operativo) As String

            result_.SetOK()

            Dim clausulasSQLReglasAcceso_ As String = Nothing

            Dim script_ As String = Nothing

            Dim scriptDashBoard_ As String = Nothing

            Dim accessRules_ As String = Nothing

            Dim accessRulesDashBoard_ As String = Nothing

            Dim acessed_ As String = Nothing

            Dim acessedDashBoard_ As String = Nothing

            Dim denied_ As String = Nothing

            Dim deniedDashBoard_ As String = Nothing

            Dim counter_ As Int32 = 1

            Dim token_ As String = "and"

            Dim tokenOr_ As String = "or"

            'JC - Agregar
            Dim clavesClienteEmpresa_ As String = ""

            ' Validación para cuando hay un unico permiso
            If _reglasAccesoKrom(0).IDDivisionMiEmpresa > 0 Then

                For Each rule_ As ReglaAccesoKrom In _reglasAccesoKrom

                    ' Validación para cuando hay mas de un registro y alguno de ellos tiene uno de super usuario
                    If rule_.IDDivisionMiEmpresa > 0 Then

                        If counter_ = 1 Then
                            token_ = Nothing
                            tokenOr_ = Nothing
                        Else
                            token_ = "and"
                            tokenOr_ = "or"
                        End If

                        Select Case rule_.Accion

                            Case AccessTypes.Access

                                accessRules_ = " ( "

                                accessRulesDashBoard_ = " ( "

                            Case AccessTypes.Deny

                                Continue For

                        End Select

                        'Obligatorio, división de Krom Aduanal
                        If rule_.IDDivisionMiEmpresa > 0 Then

                            'Al ser multiempresa, ya no es necesario cortar por oficina
                            accessRules_ = accessRules_ & " i_Cve_DivisionMiEmpresa =" & rule_.IDDivisionMiEmpresa
                            'accessRules_ = accessRules_ & " i_Cve_Estado = 1 "

                            accessRulesDashBoard_ = accessRulesDashBoard_ & " i_Cve_DivisionMiEmpresa =" & rule_.IDDivisionMiEmpresa

                        End If

                        'Opcional,  RFC cliente SysExpert o sistema tráfico
                        If Not rule_.RFC Is Nothing And rule_.RFC <> "0" Then

                            accessRules_ = accessRules_ & " and t_RfcExterno ='" & rule_.RFC & "'"

                            accessRulesDashBoard_ = accessRulesDashBoard_ & " and t_RfcExterno ='" & rule_.RFC & "'"

                        End If

                        'r21.i_Cve_ClienteEmpresaFacturacion,r21.t_RFCFacturacion, r21.i_ExigirCongruenciaImportadorFacturacion

                        'Opcional,  Revisamos si es exigible la congrencia entre importadores, exportadores y su factura fiscal ( comprobante fiscal agente aduanal)

                        'MsgBox("llegamos!")

                        'i_Cve_ClienteComprobanteFiscal
                        't_RFCComprobanteFiscal

                        If _enlaceDatos.TipoGestionOperativa = IEnlaceDatos.TiposGestionOperativa.AccesoCuentaGastos Or
                             _enlaceDatos.TipoGestionOperativa = IEnlaceDatos.TiposGestionOperativa.AccesoOperativoYCuentaGastos Then

                            If rule_.RFC_ClienteComprobanteFiscal <> "0" Then

                                'If rule_.ExigirCongruenciaImportadorFacturacion > 0 Then

                                'Opcional,  RFC cliente Comprobante Fiscal
                                If Not rule_.RFC_ClienteComprobanteFiscal Is Nothing Then

                                    accessRules_ = accessRules_ & " and t_RFCComprobanteFiscal ='" & rule_.RFC_ClienteComprobanteFiscal & "'"

                                ElseIf Not rule_.RFC Is Nothing Then

                                    accessRules_ = accessRules_ & " and t_RFCComprobanteFiscal ='" & rule_.RFC & "'"

                                End If

                                'Opcional,  ID Cliente Comprobante Fiscal
                                If rule_.ClaveClienteComprobanteFiscal > 0 Then

                                    accessRules_ = accessRules_ & " and i_Cve_ClienteComprobanteFiscal =" & rule_.ClaveClienteComprobanteFiscal
                                    'accessRules_ = accessRules_ & " and i_Cve_ClienteEmpresa =" & rule_.IDClienteEmpresa

                                ElseIf rule_.IDClienteEmpresa > 0 Then

                                    accessRules_ = accessRules_ & " and i_Cve_ClienteComprobanteFiscal =" & rule_.IDClienteEmpresa.ToString

                                End If

                                'Else

                                '    'IMPLEMENTAR, el usuario no cuenta con reglas de acceso administrativo

                                '    result_.SetError(ErrorTypes.WS008, "Not enough rules to data access")
                                '    _enlaceDatos.MensajeTagWatcher.SetError(ErrorTypes.WS008)

                                '    Return " and i_Cve_Estado = 65"

                                'End If

                            End If

                        Else

                            If rule_.RFC_ClienteComprobanteFiscal <> "0" Then

                                If rule_.ExigirCongruenciaImportadorFacturacion > 0 Then

                                    'Opcional,  RFC cliente Comprobante Fiscal
                                    If Not rule_.RFC_ClienteComprobanteFiscal Is Nothing And rule_.RFC_ClienteComprobanteFiscal <> "" Then

                                        accessRules_ = accessRules_ & " and t_RFCComprobanteFiscal ='" & rule_.RFC_ClienteComprobanteFiscal & "'"

                                    ElseIf Not rule_.RFC Is Nothing Then

                                        accessRules_ = accessRules_ & " and t_RFCComprobanteFiscal ='" & rule_.RFC & "'"

                                    End If

                                    'Opcional,  ID Cliente Comprobante Fiscal
                                    If rule_.ClaveClienteComprobanteFiscal > 0 Then

                                        accessRules_ = accessRules_ & " and i_Cve_ClienteComprobanteFiscal =" & rule_.ClaveClienteComprobanteFiscal
                                        'accessRules_ = accessRules_ & " and i_Cve_ClienteEmpresa =" & rule_.IDClienteEmpresa

                                    ElseIf rule_.IDClienteEmpresa > 0 Then

                                        accessRules_ = accessRules_ & " and i_Cve_ClienteComprobanteFiscal =" & rule_.IDClienteEmpresa.ToString

                                    End If

                                Else

                                    'IMPLEMENTAR, el usuario no cuenta con reglas de acceso administrativo

                                    'result_.SetError(ErrorTypes.WS008, "Not enough rules to data access")
                                    '_enlaceDatos.MensajeTagWatcher.SetError(ErrorTypes.WS008)

                                    'Return " and i_Cve_Estado = 65"

                                End If

                            End If

                        End If

                        'Opcional,  ID Cliente Empresa
                        If rule_.IDClienteEmpresa > 0 Then

                            accessRules_ = accessRules_ & " and i_Cve_VinClienteOperacion =" & rule_.IDClienteEmpresa
                            'accessRules_ = accessRules_ & " and i_Cve_ClienteEmpresa =" & rule_.IDClienteEmpresa

                            accessRulesDashBoard_ = accessRulesDashBoard_ & " and i_Cve_VinClienteOperacion =" & rule_.IDClienteEmpresa

                            'JC - Agregar
                            'If Not clavesClienteEmpresa_.Contains(rule_.IDClienteEmpresa) Then

                            '    clavesClienteEmpresa_ = clavesClienteEmpresa_ & rule_.IDClienteEmpresa & ", "

                            'End If

                        End If

                        'Opcional,  División empresarial cliente
                        If rule_.IDDivisionEmpresarialCliente > 0 Then

                            accessRules_ = accessRules_ & " and i_Cve_DivisionEmpresarialCliente =" & rule_.IDDivisionEmpresarialCliente

                            accessRulesDashBoard_ = accessRulesDashBoard_ & " and i_Cve_DivisionEmpresarialCliente =" & rule_.IDDivisionEmpresarialCliente

                        End If

                        If tipoSeccion_ = TipoSeccion.Operativo Then

                            If rule_.ClaveClienteExterno > 0 Then

                                accessRules_ = accessRules_ & " AND i_Cve_ClienteOperativoExterno =" & rule_.ClaveClienteExterno

                                accessRulesDashBoard_ = accessRulesDashBoard_ & " AND i_Cve_ClienteOperativoExterno =" & rule_.ClaveClienteExterno

                                If Not clavesClienteEmpresa_.Contains(rule_.ClaveClienteExterno) Then

                                    clavesClienteEmpresa_ = clavesClienteEmpresa_ & rule_.ClaveClienteExterno & ", "

                                End If

                            End If

                            'Opcional,  AduanaSeccion
                            If rule_.Aduana > 0 Then

                                accessRules_ = accessRules_ & " and i_AduanaSeccion = " & rule_.Aduana

                            End If

                            'Opcional,  Patente Aduanal
                            If rule_.Patente > 0 Then

                                accessRules_ = accessRules_ & " and i_Patente = " & rule_.Patente

                            End If

                        End If

                        'Opcional,  Tipo Operación
                        If rule_.TipoOperacion > 0 Then

                            accessRules_ = accessRules_ & " and i_TipoOperacion = " & rule_.TipoOperacion

                        End If

                        accessRules_ = accessRules_ & " ) "

                        accessRulesDashBoard_ = accessRulesDashBoard_ & " ) "

                        script_ = script_ & " " & tokenOr_ & " " & accessRules_

                        scriptDashBoard_ = scriptDashBoard_ & " " & tokenOr_ & " " & accessRulesDashBoard_

                        counter_ += 1

                    End If

                Next

                acessed_ = " ( " & script_ & ") "

                acessedDashBoard_ = " ( " & scriptDashBoard_ & ") "

                script_ = Nothing

                scriptDashBoard_ = Nothing

                accessRules_ = Nothing

                accessRulesDashBoard_ = Nothing

                counter_ = 1

                For Each rule_ As ReglaAccesoKrom In _reglasAccesoKrom

                    If rule_.IDDivisionMiEmpresa > 0 Then

                        If counter_ = 1 Then
                            token_ = Nothing
                            tokenOr_ = Nothing
                        Else
                            token_ = "and"
                            tokenOr_ = "or"
                        End If

                        Select Case rule_.Accion

                            Case AccessTypes.Access

                                Continue For

                            Case AccessTypes.Deny

                                accessRules_ = " ( "

                                accessRulesDashBoard_ = " ( "

                        End Select

                        If rule_.IDDivisionMiEmpresa > 0 Then

                            accessRules_ = accessRules_ & " i_Cve_DivisionMiEmpresa =" & rule_.IDDivisionMiEmpresa

                            accessRulesDashBoard_ = accessRulesDashBoard_ & " i_Cve_DivisionMiEmpresa =" & rule_.IDDivisionMiEmpresa

                        End If

                        If Not rule_.RFC Is Nothing Then

                            accessRules_ = accessRules_ & " and t_RfcExterno ='" & rule_.RFC & "'"

                            accessRulesDashBoard_ = accessRulesDashBoard_ & " and t_RfcExterno ='" & rule_.RFC & "'"

                        End If

                        If rule_.IDClienteEmpresa > 0 Then

                            'accessRules_ = accessRules_ & " and i_Cve_ClienteEmpresa =" & rule_.IDClienteEmpresa
                            accessRules_ = accessRules_ & " and i_Cve_VinClienteOperacion =" & rule_.IDClienteEmpresa

                            accessRulesDashBoard_ = accessRulesDashBoard_ & " and i_Cve_VinClienteOperacion =" & rule_.IDClienteEmpresa

                        End If

                        If rule_.IDDivisionEmpresarialCliente > 0 Then

                            accessRules_ = accessRules_ & " and i_Cve_DivisionEmpresarialCliente =" & rule_.IDDivisionEmpresarialCliente

                            accessRulesDashBoard_ = accessRulesDashBoard_ & " and i_Cve_DivisionEmpresarialCliente =" & rule_.IDDivisionEmpresarialCliente

                        End If

                        If tipoSeccion_ = TipoSeccion.Operativo Then

                            If rule_.ClaveClienteExterno > 0 Then

                                accessRules_ = accessRules_ & " AND i_Cve_ClienteOperativoExterno =" & rule_.ClaveClienteExterno

                                accessRulesDashBoard_ = accessRulesDashBoard_ & " AND i_Cve_ClienteOperativoExterno =" & rule_.ClaveClienteExterno

                            End If

                            If rule_.Aduana > 0 Then

                                accessRules_ = accessRules_ & " and i_AduanaSeccion = " & rule_.Aduana

                            End If

                            If rule_.Patente > 0 Then

                                accessRules_ = accessRules_ & " and i_Patente = " & rule_.Patente

                            End If

                        End If

                        If rule_.TipoOperacion > 0 Then

                            accessRules_ = accessRules_ & " and i_TipoOperacion = " & rule_.TipoOperacion

                        End If

                        accessRules_ = accessRules_ & " ) "

                        accessRulesDashBoard_ = accessRulesDashBoard_ & " ) "

                        script_ = script_ & " " & tokenOr_ & " " & accessRules_

                        scriptDashBoard_ = scriptDashBoard_ & " " & tokenOr_ & " " & accessRulesDashBoard_

                        counter_ += 1

                    End If

                Next

                denied_ = Nothing

                deniedDashBoard_ = Nothing

                If Not script_ Is Nothing And Trim(script_) <> "" Then

                    denied_ = " not ( " & script_ & ") "

                    deniedDashBoard_ = " not ( " & scriptDashBoard_ & ") "

                End If

                If Not acessed_ Is Nothing And Trim(acessed_) <> "" Then

                    _clausulasSQLReglasAcceso = " and " & acessed_

                    clausulasSQLReglasAcceso_ = " and " & acessedDashBoard_

                End If

                If Not denied_ Is Nothing And Trim(denied_) <> "" Then

                    _clausulasSQLReglasAcceso = _clausulasSQLReglasAcceso & " and " & denied_

                    clausulasSQLReglasAcceso_ = clausulasSQLReglasAcceso_ & " and " & deniedDashBoard_

                End If

                If _clausulasSQLReglasAcceso Is Nothing Then

                    result_.SetError(ErrorTypes.WS008, "Not enough rules to data access")

                End If

            Else

                _clausulasSQLReglasAcceso = ""

            End If

            'JC - Agregar
            If Not _componentesOperacionesUsuario.ContainsKey(FiltrosDashBoard.RazonesSocialesCliente) Then

                Dim campos_() As String = {"RFC cliente", "Nombre Corto Cliente"}

                '_componentesOperacionesUsuario.Add(FiltrosDashBoard.RazonesSocialesCliente, ObtenerClientes(_clausulasSQLReglasAcceso, campos_))
                _componentesOperacionesUsuario.Add(FiltrosDashBoard.RazonesSocialesCliente, ObtenerClientes(clausulasSQLReglasAcceso_, campos_))

            End If

            If clavesClienteEmpresa_.Length > 0 Then

                If Not _componentesOperacionesUsuario.ContainsKey(FiltrosDashBoard.ClavesCliente) Then

                    Dim campos_() As String = {"Clave cliente", "Nombre Corto Cliente"}

                    _componentesOperacionesUsuario.Add(FiltrosDashBoard.ClavesCliente, ObtenerClientes(" AND i_Cve_ClienteOperativoExterno IN (" & clavesClienteEmpresa_.Substring(0, clavesClienteEmpresa_.Length - 2) & ")", campos_))

                End If

            End If

            _enlaceDatos.ClausulasLibres = _clausulasSQLReglasAcceso

            Return _clausulasSQLReglasAcceso

        End Function

        'JC - Agregar
        Private Function ObtenerClientes(ByVal clausulasLibres_ As String, ByVal campos_() As String)

            Dim dataTable_ As New DataTable

            _iOperations = _sistema.EnsamblaModulo("KardexPermisos")

            _iOperations.OperadorCatalogoConsulta = "Vt025MisClientesExtranet"

            _iOperations.VistaEncabezados = "Ve025IUMisClientesExtranet"

            _iOperations.ClausulasLibres = clausulasLibres_

            _iOperations.CantidadVisibleRegistros = 10000

            _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

            If _iOperations.GenerarVista() = IOperacionesCatalogo.EstadoOperacion.COk Then

                If _sistema.TieneResultados(_iOperations) Then

                    'Obtiene las empresas del cliente (Razones Sociales)
                    Dim dataViewEmpresasCliente_ As New DataView(_iOperations.Vista.Tables(0))

                    dataTable_ = dataViewEmpresasCliente_.ToTable(True, campos_)

                Else

                    dataTable_ = Nothing

                End If

            Else

                dataTable_ = Nothing

            End If

            Return dataTable_

        End Function

        Private Function ObtenerNombreDivisionMiEmpresa(ByVal idDivisionMiEmpresa_ As Int32)

            Dim nombreMiEmpresa_ As String = ""

            Select Case idDivisionMiEmpresa_

                Case 1
                    nombreMiEmpresa_ = "Veracruz"

                Case 2
                    nombreMiEmpresa_ = "Atlas"

                Case 3
                    nombreMiEmpresa_ = "México"

                Case 6
                    nombreMiEmpresa_ = "Altamira"

                Case 7
                    nombreMiEmpresa_ = "Toluca"

                Case 8
                    nombreMiEmpresa_ = "Manzanillo"

                Case 9
                    nombreMiEmpresa_ = "Lazaro Cardenas"

                Case 113
                    nombreMiEmpresa_ = "Nuevo Laredo"

                Case Else
                    nombreMiEmpresa_ = ""

            End Select

            Return nombreMiEmpresa_

        End Function

#End Region

    End Class

End Namespace


