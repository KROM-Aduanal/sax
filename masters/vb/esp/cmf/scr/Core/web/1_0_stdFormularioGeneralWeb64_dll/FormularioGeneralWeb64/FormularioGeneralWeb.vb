Imports Gsol.krom.IEnlaceDatos.TiposGestionOperativa
Imports Gsol.krom.IEnlaceDatos.FormatosRespuesta
Imports Gsol.krom.ControladorAccesoKBW64
Imports Gsol.BaseDatos.Operaciones
Imports Gsol.basededatos
Imports Wma.Exceptions
Imports Gsol.krom
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Net.Http
Imports System.Web.UI

Public Class FormularioGeneralWeb
    Inherits System.Web.UI.Page

#Region "Enums"
    Enum StatusMessage
        Success = 1
        Fail = 2
        Info = 3
    End Enum

    Enum ResponseFormat
        DataTable = 1
        Collection = 2
        JSON = 3
        XML = 4
    End Enum

#End Region

#Region "Atributos"

    'Private _mensajePeticiones As Dictionary(Of String, Object)

    Private _accessControl As ControladorAccesoKBW

#End Region

#Region "Propiedades"

    Private ReadOnly Property IsPopup As Boolean
        Get
            Return If(Request.QueryString("is_popup") = "true", True, False)
        End Get
    End Property

    'Public Property MensajePeticiones() As Dictionary(Of String, Object)

    '    Get

    '        Return _mensajePeticiones

    '    End Get

    '    Set(ByVal value_ As Dictionary(Of String, Object))

    '        _mensajePeticiones = value_

    '    End Set

    'End Property


    'Public Property Test As String

#End Region

#Region "Constructores"

    Sub New()

        'RestaurarMensajePeticiones()

        _accessControl = New ControladorAccesoKBW()

    End Sub

#End Region

#Region "Metodos"

    Public Overridable Sub AceptaConfirmacionDialogo(ByVal argument_ As String)
    End Sub

    Public Overridable Sub RechazaConfirmacionDialogo(ByVal argument_ As String)
    End Sub

    Protected Overridable Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        If IsPopup = True Then

            Me.MasterPageFile = "~/FrontEnd/Modulos/Modal.Master"

        End If

    End Sub

    Protected Overridable Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Valida el inicio de sesión y se guardo la cookie
        If verificaCredenciales() = False Then

            Response.Redirect("/")

        End If

        Dim dataClient_ = System.Web.HttpContext.Current.Session("__dialog")

        If dataClient_ IsNot Nothing Then

            System.Web.HttpContext.Current.Session("__dialog") = Nothing

            If Convert.ToBoolean(dataClient_.Item("accept")) = True Then

                AceptaConfirmacionDialogo(dataClient_.Item("arg"))

            Else

                RechazaConfirmacionDialogo(dataClient_.Item("arg"))

            End If

        End If

        'EventsHandler(Me)

        'PreparaRequest(Request, Response, Me)

    End Sub

    'Public Sub EventsHandler(ByVal self As Object)

    '    Dim serverEvent_ = System.Web.HttpContext.Current.Request.Form("__SERVEREVENT")

    '    If Not String.IsNullOrEmpty(serverEvent_) Then

    '        Try

    '            CallByName(self, serverEvent_, vbMethod)

    '        Catch ex As Exception
    '        End Try

    '    End If

    'End Sub

    Public Function ConsultaLibre(ByVal sqlQueryString_ As String) As DataTable

        Dim organismo_ = New Organismo()

        organismo_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        organismo_.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(sqlQueryString_)

        If Not organismo_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente Is Nothing Then

            If Not organismo_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

                If organismo_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count > 0 Then

                    Return organismo_.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0)

                End If

            End If

        End If

        Return Nothing

    End Function

    Public Function ConsultarEnlace(ByVal entidad_ As IEntidadDatos,
                                    ByVal dimension_ As IEnlaceDatos.TiposDimension,
                                    Optional ByVal granularidad_ As IEnlaceDatos.TiposDimension = IEnlaceDatos.TiposDimension.SinDefinir,
                                    Optional ByVal presentacion_ As IEnlaceDatos.ModalidadPresentacionEncabezados = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo,
                                    Optional ByVal clausulasLibres_ As String = Nothing,
                                    Optional ByVal formato_ As ResponseFormat = ResponseFormat.DataTable) As Object

        'Dim respuesta_ As New Dictionary(Of String, Object)

        'respuesta_.Add("code", _mensajePeticiones("codigoError"))

        'respuesta_.Add("message", _mensajePeticiones("mensajeError"))

        Dim resultados_ As New DataTable


        If verificaCredenciales() = True Then

            Using enlaceDatos_ As IEnlaceDatos = GenerarEnlaceDatos()

                With enlaceDatos_

                    .Granularidad = granularidad_

                    If Not clausulasLibres_ Is Nothing Then

                        .ClausulasLibres = clausulasLibres_

                    End If

                    .EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")

                    .ModalidadPresentacion = presentacion_

                    .TipoRespuestaRequerida = Automatico

                    .TipoConexion = IConexiones.TipoConexion.DirectMongoDB

                    .TipoGestionOperativa = AccesoOperativo

                    .OrigenDatos = IConexiones.Controladores.MongoDB

                    .ObjetoDatos = IConexiones.TiposRepositorio.DataSetObject

                End With

                Using entidadDatos_ As IEntidadDatos = entidad_

                    entidadDatos_.Dimension = dimension_

                    Dim Operacion_ = enlaceDatos_.Consultar(entidadDatos_)

                    'Dim eventMonitorTagWatcher_ As TagWatcherEventMonitor

                    'eventMonitorTagWatcher_ = TagWatcherEventMonitor.ObtainInstance()

                    If Operacion_.Status = TagWatcher.TypeStatus.Ok Then

                        resultados_ = enlaceDatos_.IOperaciones.Vista.Tables(0)

                        'respuesta_("code") = _mensajePeticiones("codigoExito")

                        'respuesta_("message") = Nothing
                    Else



                    End If

                End Using

            End Using

            'respuesta_("response") = ObtenerListaResultados(tablaResultados_)

        Else

            'respuesta_("code") = _mensajePeticiones("codigoProhibido")

            'respuesta_("message") = _mensajePeticiones("mensajeProhibido")

            'respuesta_("response") = Nothing

        End If

        'RestaurarMensajePeticiones()

        If formato_ = ResponseFormat.DataTable Then

            Return resultados_

        ElseIf formato_ = ResponseFormat.JSON Then

            Return New JavaScriptSerializer().Serialize(ObtenerListaResultados(resultados_))

        ElseIf formato_ = ResponseFormat.Collection Then

            Return ObtenerListaResultados(resultados_)

        ElseIf formato_ = ResponseFormat.XML Then

            Return Nothing

        Else

            Return Nothing

        End If

    End Function

    Public Function EliminarEnlace(ByVal llaves_ As List(Of String),
                                   ByVal entidad_ As IEntidadDatos,
                                   ByVal dimension_ As IEnlaceDatos.TiposDimension,
                                   Optional ByVal granularidad_ As IEnlaceDatos.TiposDimension = IEnlaceDatos.TiposDimension.SinDefinir) As Boolean

        'Dim respuesta_ As New Dictionary(Of String, Object)

        'respuesta_.Add("code", _mensajePeticiones("codigoError"))

        'respuesta_.Add("message", _mensajePeticiones("mensajeError"))

        If verificaCredenciales() = True Then

            Using enlaceDatos_ As IEnlaceDatos = GenerarEnlaceDatos()

                With enlaceDatos_

                    .EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")

                    .Granularidad = granularidad_

                    .TipoConexion = IConexiones.TipoConexion.SqlCommand

                    '.ReflejarEn = IEnlaceDatos.DestinosParaReplicacion.NoSQLMongoDB

                End With

                Using entidadDatos_ As IEntidadDatos = entidad_

                    With entidadDatos_

                        .Dimension = dimension_

                        .DeteleOnKeyValues = llaves_

                    End With

                    Dim Operacion_ = enlaceDatos_.EliminarDatos(entidadDatos_)

                    'Dim eventMonitorTagWatcher_ As TagWatcherEventMonitor

                    'eventMonitorTagWatcher_ = TagWatcherEventMonitor.ObtainInstance()

                    If Operacion_.Status = TagWatcher.TypeStatus.Ok Then

                        'respuesta_("code") = _mensajePeticiones("codigoExito")

                        'respuesta_("message") = _mensajePeticiones("mensajeBorrado")

                        Return True

                    End If

                End Using

            End Using

            'respuesta_("response") = Nothing

        Else

            'respuesta_("code") = _mensajePeticiones("codigoProhibido")

            'respuesta_("message") = _mensajePeticiones("mensajeProhibido")

            'respuesta_("response") = Nothing

        End If

        'RestaurarMensajePeticiones()

        'Return New JavaScriptSerializer().Serialize(respuesta_)

        Return False

    End Function

    Public Function RealizaUPSERT(ByVal entidad_ As IEntidadDatos,
                                  Optional ByVal granularidad_ As IEnlaceDatos.TiposDimension = IEnlaceDatos.TiposDimension.SinDefinir) As Boolean

        'Dim respuesta_ As New Dictionary(Of String, Object)

        If verificaCredenciales() = True Then

            Return RealizaGuardadoSingular(entidad_, granularidad_)

            'Else

            'respuesta_("code") = _mensajePeticiones("codigoProhibido")

            'respuesta_("message") = _mensajePeticiones("mensajeProhibido")

            'respuesta_("response") = Nothing

        End If

        'RestaurarMensajePeticiones()

        'Return New JavaScriptSerializer().Serialize(respuesta_)

        Return False

    End Function

    Public Function RealizaUPSERT(ByVal entidades_ As List(Of IEntidadDatos),
                                  Optional ByVal granularidad_ As IEnlaceDatos.TiposDimension = IEnlaceDatos.TiposDimension.SinDefinir) As Boolean

        Dim respuesta_ As New Dictionary(Of String, Object)

        If verificaCredenciales() = True Then

            Return RealizaGuardadoPlural(entidades_, granularidad_)

            'Else

            'respuesta_("code") = _mensajePeticiones("codigoProhibido")

            'respuesta_("message") = _mensajePeticiones("mensajeProhibido")

            'respuesta_("response") = Nothing

        End If

        Return False

        'RestaurarMensajePeticiones()

        'Return New JavaScriptSerializer().Serialize(respuesta_)

    End Function

    Public Function RealizaUPSERT(ByVal coleccion_ As List(Of Object),
                                  Optional ByVal granularidad_ As IEnlaceDatos.TiposDimension = IEnlaceDatos.TiposDimension.SinDefinir) As Boolean

        'Dim respuesta_ As New Dictionary(Of String, Object)

        If verificaCredenciales() = True Then

            For Each elemento_ As Object In coleccion_

                If elemento_.[GetType]() = GetType(List(Of IEntidadDatos)) Then

                    Return RealizaGuardadoPlural(elemento_, granularidad_)

                Else

                    Return RealizaGuardadoSingular(elemento_, granularidad_)

                End If

            Next

            'Else

            'respuesta_("code") = _mensajePeticiones("codigoProhibido")

            'respuesta_("message") = _mensajePeticiones("mensajeProhibido")

            'respuesta_("response") = Nothing

        End If

        Return False
        'RestaurarMensajePeticiones()

        'Return New JavaScriptSerializer().Serialize(respuesta_)

    End Function

    Private Function RealizaGuardadoSingular(ByRef entidad_ As IEntidadDatos,
                                        ByRef granularidad_ As IEnlaceDatos.TiposDimension) As Boolean

        'respuesta_("response") = Nothing

        Using enlaceDatos_ As IEnlaceDatos = GenerarEnlaceDatos()

            With enlaceDatos_

                .EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")

                .Granularidad = granularidad_

                .TipoConexion = IConexiones.TipoConexion.SqlCommand

                .ReflejarEn = IEnlaceDatos.DestinosParaReplicacion.NoSQLMongoDB

            End With

            Using iEntidadDatos_ As IEntidadDatos = entidad_

                Dim Operacion_ As TagWatcher

                Dim item_ = New Dictionary(Of String, String)

                If iEntidadDatos_.UpdateOnKeyValues.Count Then

                    Operacion_ = enlaceDatos_.ModificarDatos(iEntidadDatos_)

                Else

                    Operacion_ = enlaceDatos_.AgregarDatos(iEntidadDatos_)

                    item_.Add("updateOnKeyValue", Operacion_.ObjectReturned)

                End If

                If Operacion_.Status = TagWatcher.TypeStatus.Ok Then

                    'RETORNAR ITEM AGREGADO A MEDIAS NO HAY ID
                    'For Each campoVirtual_ As CampoVirtual In iEntidadDatos_.Atributos

                    '    item_.Add(campoVirtual_.Atributo, campoVirtual_.Valor)

                    'Next

                    'respuesta_("response") = item_

                    'respuesta_("code") = _mensajePeticiones("codigoExito")

                    'respuesta_("message") = _mensajePeticiones("mensajeInsercción")
                    Return True

                End If

            End Using

        End Using

        Return False

    End Function

    Private Function RealizaGuardadoPlural(ByRef entidades_ As List(Of IEntidadDatos),
                                        ByRef granularidad_ As IEnlaceDatos.TiposDimension) As Boolean

        Using enlaceDatos_ As IEnlaceDatos = GenerarEnlaceDatos()

            With enlaceDatos_

                .EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")

                .Granularidad = granularidad_

                .TipoConexion = IConexiones.TipoConexion.SqlCommand

                .ReflejarEn = IEnlaceDatos.DestinosParaReplicacion.NoSQLMongoDB

            End With

            'Dim iEntidadDatosEdicion_ = New List(Of IEntidadDatos)

            Dim iEntidadDatosAgregar_ As List(Of IEntidadDatos) = New List(Of IEntidadDatos)

            For Each iEntidad_ As IEntidadDatos In entidades_
                'UPDATE ELEMENTS ONE FOR ONE
                If iEntidad_.UpdateOnKeyValues.Count Then

                    'iEntidadDatosEdicion_.Add(iEntidad_)
                    RealizaGuardadoSingular(iEntidad_, granularidad_)

                Else
                    'PREPARE DATA FOR ADD NEW ITEMS
                    iEntidadDatosAgregar_.Add(iEntidad_)

                End If

            Next

            If iEntidadDatosAgregar_.Count > 0 Then

                If iEntidadDatosAgregar_.Count = 1 Then

                    Dim Operacion_ = enlaceDatos_.AgregarDatos(iEntidadDatosAgregar_(0))

                    If Operacion_.Status = TagWatcher.TypeStatus.Ok Then

                        Return True

                    End If

                Else
                    Dim Operacion_ = enlaceDatos_.AgregarDatos(bulkDatos_:=iEntidadDatosAgregar_)

                    If Operacion_.Status = TagWatcher.TypeStatus.Ok Then

                        'respuesta_("code") = _mensajePeticiones("codigoExito")

                        'respuesta_("message") = _mensajePeticiones("mensajeInsercción")
                        Return True

                    End If

                End If

            End If

            'Dim Operacion2_ = enlaceDatos_.ModificarDatos(iEntidadDatosEdicion_)

            'If Operacion2_.Status = TagWatcher.TypeStatus.Ok Then

            '    respuesta_("code") = _mensajePeticiones("codigoExito")

            '    respuesta_("message") = _mensajePeticiones("mensajeInsercción")

            'End If

        End Using

        Return False

    End Function

    Public Sub EntityDataBinding(ByRef entidad_ As IEntidadDatos, ByVal datos_ As Object, ByVal keyfield_ As String)

        With entidad_

            For Each dato_ As KeyValuePair(Of String, Object) In datos_

                If dato_.Key = keyfield_ Then

                    .UpdateOnKeyValues.Add(dato_.Value)

                Else

                    .Attribute(dato_.Key) = dato_.Value

                End If

            Next

        End With

    End Sub

    Private Function GenerarEnlaceDatos() As IEnlaceDatos

        Dim enlaceDatos_ = New EnlaceDatos

        With enlaceDatos_

            .Granularidad = IEnlaceDatos.TiposDimension.SinDefinir

            .ModalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo

            .TipoRespuestaRequerida = IEnlaceDatos.FormatosRespuesta.IOperaciones

            .ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

            .TipoConexion = IConexiones.TipoConexion.Automatico

        End With

        Return enlaceDatos_

    End Function

    Private Function ObtenerListaResultados(ByRef tablaResultados_ As DataTable) As List(Of Dictionary(Of Object, Object))

        Dim listaResultados_ As New List(Of Dictionary(Of Object, Object))

        For Each fila_ As DataRow In tablaResultados_.Rows

            Dim campo_ As New Dictionary(Of Object, Object)

            For Each columna_ As DataColumn In tablaResultados_.Columns

                Dim tipoDato_ = fila_(columna_).GetType()

                Select Case tipoDato_

                    Case System.Type.GetType("System.DateTime")
                        System.Type.GetType("System.Date")

                        campo_.Add(columna_.ColumnName, Date.Parse(fila_(columna_)).ToString("yyyy-MM-dd"))

                    Case Else

                        campo_.Add(columna_.ColumnName, fila_(columna_))

                End Select

            Next

            listaResultados_.Add(campo_)

        Next

        Return listaResultados_

    End Function

    'Private Sub RestaurarMensajePeticiones()

    '    _mensajePeticiones = New Dictionary(Of String, Object)

    '    _mensajePeticiones("codigoExito") = 200

    '    _mensajePeticiones("codigoError") = 400

    '    _mensajePeticiones("codigoProhibido") = 403

    '    _mensajePeticiones("mensajeInsercción") = "Registro insertado exitosamente"

    '    _mensajePeticiones("mensajeActualizacion") = "Registro actualizado exitosamente"

    '    _mensajePeticiones("mensajeBorrado") = "Registro eliminado exitosamente"

    '    _mensajePeticiones("mensajeError") = "Algo salio mal"

    '    _mensajePeticiones("mensajeProhibido") = "La sesión de usuario ha expirado o no tienes autorización"

    'End Sub

    Private Function verificaCredenciales() As Boolean

        Dim conexion_ = False

        If Not _accessControl.SesionAbierta() Then

            If Not _accessControl.IntentarConexion() Then

                conexion_ = False

            Else

                conexion_ = True

            End If

        Else

            conexion_ = True

        End If

        If Session("ControladorWeb") Is Nothing Then

            conexion_ = False

        End If

        Return conexion_

    End Function

    Public Sub DisplayMessage(ByVal message_ As String,
                              Optional ByVal status_ As StatusMessage = StatusMessage.Success)

        Dim arguments_ = New List(Of String)

        arguments_.Add("'" & message_ & "'")

        arguments_.Add("'" & status_ & "'")

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "DisplayMessage(" & String.Join(", ", arguments_) & ");", True)

    End Sub

    Public Sub DisplayAlert(ByVal title_ As String,
                            ByVal message_ As String,
                            ByVal argument_ As String,
                            Optional accept_ As String = "Entendido",
                            Optional reject_ As String = Nothing)

        Dim arguments_ = New List(Of String)

        arguments_.Add("'" & title_ & "'")

        arguments_.Add("'" & message_ & "'")

        arguments_.Add("'" & accept_ & "'")

        arguments_.Add("'" & reject_ & "'")

        arguments_.Add("'" & argument_ & "'")

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "DisplayAlert(" & String.Join(", ", arguments_) & ");", True)

    End Sub

    <System.Web.Services.WebMethod>
    Public Shared Function ProcessDialogConfirmation(ByVal arguments_ As Dictionary(Of String, Object)) As String

        System.Web.HttpContext.Current.Session("__dialog") = arguments_

        Return "{""code"":""200""}"

    End Function

    'Public Shared Sub PreparaRequest(ByRef request As HttpRequest, ByRef response As HttpResponse, ByRef obj As Object)

    '    If Not String.IsNullOrEmpty(request.QueryString("control")) Then

    '        Dim responseJson_ As String = Nothing

    '        Dim controlsList_ As New List(Of String) From {"SelectControl", "FindboxControl", "FindbarControl", "FormControl", "CatalogControl"}

    '        Dim control_ = request.QueryString("control")

    '        Dim method_ As String = request.QueryString("method")

    '        If controlsList_.IndexOf(request.QueryString("control")) >= 0 Then

    '            If request.Form.AllKeys.Length Then

    '                Dim dataPost_ = New Dictionary(Of String, Object)()

    '                Dim requestPost_ = request.Form

    '                For Each key_ In requestPost_.Keys

    '                    If key_.Contains("[]") Then
    '                        Dim a As Object = requestPost_(key_)

    '                        Dim arr_() As String = Split(requestPost_(key_), ",")

    '                        dataPost_.Add(Replace(key_, "[]", ""), arr_)

    '                    Else

    '                        dataPost_.Add(key_, requestPost_(key_))

    '                    End If

    '                Next
    '                Try

    '                    responseJson_ = CallByName(obj, method_, vbMethod, dataPost_)

    '                Catch ex As Exception

    '                    responseJson_ = "{""code"":""400"",""response"":"""",""message"":""No se encontro un punto de respuesta""}"

    '                End Try

    '            Else
    '                Try

    '                    responseJson_ = CallByName(obj, method_, vbMethod)

    '                Catch ex As Exception

    '                    responseJson_ = "{""code"":""400"",""response"":"""",""message"":""No se encontro un punto de respuesta""}"

    '                End Try

    '            End If

    '        End If

    '        'Send the Response in JSON format to Client.

    '        response.ContentType = "text/json"

    '        response.Write(responseJson_)

    '        response.End()

    '    End If

    'End Sub

#End Region

End Class

' Esta clase prepara todos los datos de la entidad que resive del post
'Public Class EntityDataBinding

'    Private _datos As Object

'    Private _entidades As List(Of IEntidadDatos)

'    Private _gruposEntidades As List(Of Object)

'    ReadOnly Property Data() As Object
'        Get

'            If _gruposEntidades.Count = 1 Then

'                Return _gruposEntidades(0)

'            Else

'                Return _gruposEntidades

'            End If

'        End Get
'    End Property

'    Sub New(ByVal datos_ As Object)

'        _datos = datos_

'        _entidades = New List(Of IEntidadDatos)

'        _gruposEntidades = New List(Of Object)

'    End Sub

'    Private Function ObtenerCamposEntidad(ByRef campos_ As Type) As List(Of String)

'        Dim fields_ = New List(Of String)

'        Dim identifiers_ = System.Enum.GetValues(campos_)

'        For Each identifier_ As Integer In identifiers_

'            fields_.Add(System.Enum.GetName(campos_, identifier_))

'        Next

'        fields_.Add("updateOnKeyValue")

'        Return fields_

'    End Function

'    Private Function PrepararEntidad(ByRef entidad_ As IEntidadDatos,
'                                     ByRef dimension_ As IEnlaceDatos.TiposDimension,
'                                     ByRef veCampos_ As List(Of String),
'                                     ByRef datosEntidad_ As Object) As IEntidadDatos

'        Dim nuevaEntidad_ As IEntidadDatos = entidad_.Clone

'        With nuevaEntidad_

'            .Dimension = dimension_

'            .UpdateOnKeyValues = New List(Of String)

'        End With

'        For Each campo As KeyValuePair(Of String, Object) In datosEntidad_

'            Dim name = campo.Key

'            Dim value = campo.Value

'            If veCampos_.Contains(name) Then

'                If name = "updateOnKeyValue" Then

'                    If Not value Is Nothing And Double.Parse(value) > 0 Then

'                        nuevaEntidad_.UpdateOnKeyValues.Add(value)

'                    End If

'                Else

'                    nuevaEntidad_.Attribute(name) = value

'                End If

'            End If

'        Next

'        Return nuevaEntidad_

'    End Function

'    Public Sub Add(ByVal entidad_ As IEntidadDatos,
'                   ByVal dimension_ As IEnlaceDatos.TiposDimension,
'                   ByVal campos_ As Type,
'                   Optional ByVal sectionId_ As String = Nothing)

'        _entidades = New List(Of IEntidadDatos)

'        Dim veCampos_ = ObtenerCamposEntidad(campos_)

'        If sectionId_ Is Nothing Then

'            sectionId_ = dimension_.ToString()

'        End If

'        Dim datosEntidad = _datos(sectionId_)

'        If datosEntidad.[GetType]() = GetType(Dictionary(Of String, Object)) Then

'            Dim nuevaEntidad_ As IEntidadDatos = PrepararEntidad(entidad_, dimension_, veCampos_, datosEntidad)

'            _entidades.Add(nuevaEntidad_)

'        Else

'            For Each fila As Dictionary(Of String, Object) In datosEntidad

'                Dim nuevaEntidad_ As IEntidadDatos = PrepararEntidad(entidad_, dimension_, veCampos_, fila)

'                _entidades.Add(nuevaEntidad_)

'            Next

'        End If

'        _gruposEntidades.Add(_entidades)

'    End Sub

'End Class