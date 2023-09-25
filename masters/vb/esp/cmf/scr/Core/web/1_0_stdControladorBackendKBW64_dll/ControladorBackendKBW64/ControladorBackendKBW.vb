Imports System.Web
Imports gsol.krom.ControladorAccesoKBW64
Imports System.Web.Script.Serialization

Public Class ControladorBackendKBW
    Inherits System.Web.UI.Page

#Region "enums"

    Public Enum Datos
        SinDefinir = 0
        PaginaReciente = 1
        PerfilUsuario = 2
        SessionID = 3
    End Enum

    Public Enum Cookies

        MiSesion = 1
        MiCache = 2

    End Enum


#End Region

#Region "Atributos"

    Private _statements As Sax.SaxStatements = Sax.SaxStatements.GetInstance()

    Private _accessControl As New ControladorAccesoKBW

#End Region

#Region "Propiedades"

    Public Property Statements As SaxStatements
        Get
            Return _statements
        End Get
        Set(value As SaxStatements)
            _statements = value
        End Set
    End Property

    Public Property PageToRedirectWithoutSession As String = "/default.aspx" 'http://web.kromaduanal.com

    Public Property PageToRedirectWithSession As String = "/CapaPresentacion/Ges003-001-Consultas.Principal.aspx"

#End Region

#Region "Constructor"

    Public Overridable Sub Inicializa()
    End Sub

    Protected Overridable Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Request.RawUrl = "/default.aspx" Then

            Inicializa()

        Else

            If verificaCredenciales() Then

                Inicializa()

                'Preferencias(Cookies.MiSesion, Datos.PaginaReciente, False, Request.RawUrl)

            Else

                Response.Redirect(PageToRedirectWithoutSession)

            End If

        End If

    End Sub

    Protected Overridable Sub Page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Function GetVars(ByVal var_ As String,
                        Optional ByVal defaultValue_ As Object = Nothing) As Object

        Dim pagevar_ = var_

        If Session(pagevar_) Is Nothing Then

            Session(pagevar_) = defaultValue_

        End If

        Return Session(pagevar_)

    End Function

    Public Sub SetVars(ByVal var_ As String,
                        Optional ByVal value_ As Object = Nothing)

        Dim pagevar_ = var_

        Session(pagevar_) = value_

    End Sub

    Public Function GetDefaultPage() As String

        'Dim cookie_ As HttpCookie = Preferencias(Cookies.MiSesion, Datos.PaginaReciente, crearSiNoExiste_:=False)

        'If cookie_ IsNot Nothing Then

        '    Dim lastPage_ As String = cookie_.Value

        '    If Not "PaginaReciente=" & PageToRedirectWithoutSession = lastPage_ Then

        '        Return lastPage_.Replace("PaginaReciente=", String.Empty)

        '    Else

        '        Return PageToRedirectWithSession

        '    End If

        'Else

        '    Return PageToRedirectWithSession

        'End If

        Return PageToRedirectWithSession

    End Function

#End Region

#Region "Metodos"

    Public Function Preferencias(ByVal variableGlobal_ As Cookies,
                                 Optional ByVal eliminar_ As Boolean = False) As HttpCookie

        Dim cookie_ As HttpCookie

        If eliminar_ Then

            cookie_ = Request.Cookies(variableGlobal_.ToString)

            cookie_.Values.Set(variableGlobal_.ToString, "")

            cookie_.Values.Clear()

            cookie_.Values.Remove(variableGlobal_.ToString)

            cookie_.Expires = DateTime.Now.AddDays(-1)

            Response.Cookies.Set(cookie_)

            Session.Abandon()

            Return cookie_

        Else

            cookie_ = Request.Cookies(variableGlobal_.ToString)

            Return cookie_

        End If

    End Function

    Public Function Preferencias(ByVal variableGlobal_ As Cookies,
                                          ByVal dato_ As Datos,
                                          ByVal crearSiNoExiste_ As Boolean,
                                          Optional ByVal valorAsignado_ As Object = Nothing) As HttpCookie
        Return PreferenciasUsuario(variableGlobal_, dato_, crearSiNoExiste_, valorAsignado_)


    End Function

    Function PreferenciasUsuario(ByVal variableGlobal_ As Cookies,
                                 ByVal dato_ As Datos,
                                 ByVal crearSiNoExiste_ As Boolean,
                                 Optional ByVal valorAsignado_ As Object = Nothing,
                                 Optional ByVal eliminar_ As Boolean = False) As HttpCookie

        Dim cookie_ As HttpCookie

        If eliminar_ Then

            Response.Cookies.Item(variableGlobal_.ToString).Value = Nothing

            Response.Cookies.Remove(variableGlobal_.ToString)

            Response.Cookies.Clear()

            Response.Cookies.Add(New HttpCookie(variableGlobal_.ToString, ""))

            Response.Cookies(variableGlobal_.ToString).Expires = DateTime.Now.AddDays(-1)

            Session.Abandon()

            Return Nothing

        End If

        cookie_ = Request.Cookies(variableGlobal_.ToString)

        If crearSiNoExiste_ Then

            If cookie_ Is Nothing Then
                'No existe
                cookie_ = New HttpCookie(variableGlobal_.ToString)

                cookie_.Values.Add(dato_.ToString, valorAsignado_)

                cookie_.Expires = DateTime.MaxValue 'Nunca caduca

                System.Web.HttpContext.Current.Response.AppendCookie(cookie_)

            Else
                'Existe Request.RawUrl
                cookie_ = Request.Cookies(variableGlobal_.ToString)

                cookie_.Values.Set(dato_.ToString, valorAsignado_)

                cookie_.Expires = DateTime.MaxValue 'Nunca caduca

                Response.Cookies.Set(cookie_)

            End If

        Else

            If cookie_ IsNot Nothing And valorAsignado_ IsNot Nothing Then

                'Existe Request.RawUrl y solo se edita sin crear

                cookie_.Values.Set(dato_.ToString, valorAsignado_)

                cookie_.Expires = DateTime.MaxValue 'Nunca caduca

                Response.Cookies.Set(cookie_)

            End If

        End If

        Return cookie_

    End Function

    Public Sub guardaCredenciales(ByVal usuario_ As String, contrasena_ As String, Optional ByVal recordarSesion_ As String = Nothing)

        _accessControl.GuardarConexion(New Dictionary(Of String, String) From {{"usuario", usuario_},
                                                                               {"contrasena", contrasena_}})
        'If recordarSesion_ = "on" Then

        '    If Preferencias(Cookies.MiSesion, Datos.PaginaReciente, False) Is Nothing Then

        '        Preferencias(Cookies.MiSesion, Datos.PaginaReciente, True, Request.RawUrl)

        '    End If

        'End If

    End Sub

    Public Function verificaCredenciales() As Boolean

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

        Return conexion_

    End Function

    Public Overridable Sub Limpiar()
    End Sub

    Public Overridable Sub LimpiaSesion()
    End Sub

    'Public Sub LimpiaVariablesSession()

    '    For Each objeto_ As HttpSessionStateBase In Page.Session
    '        objeto_.Abandon()
    '        objeto_.Clear()
    '    Next

    '    For Each objeto_ As HttpSessionStateBase In Page.Application.Contents

    '        objeto_.Abandon()
    '        objeto_.Clear()

    '    Next

    'End Sub



    Public Shared Sub PreparaRequest(ByRef request As HttpRequest, ByRef response As HttpResponse, ByRef obj As Object)

        If Not String.IsNullOrEmpty(request.QueryString("method")) Then

            Dim clausulasLibres_ As New Dictionary(Of String, String)

            clausulasLibres_.Add("clausulasLibres_", Nothing)

            Dim conditions_ = HttpUtility.UrlDecode(request.QueryString("q"))

            If conditions_ IsNot Nothing Then

                clausulasLibres_ = New JavaScriptSerializer().Deserialize(Of Dictionary(Of String, String))(conditions_)

            End If

            Dim responseJson_ As String = Nothing

            Dim methodsList_ As New List(Of String) From {
                "ConsultarControladorWeb",
                "DashBoard",
                "ObtenerCantidadOperaciones",
                "ObtenerOperacionesPorRFC",
                "ObtenerOperacionesPorAduana",
                "ObtenerDatosIndicadoresDashBoard",
                "ConsultarSolicitudesPendientes"
            }

            Dim method_ As String = request.QueryString("method")

            If methodsList_.IndexOf(method_) >= 0 Then

                Try

                    If clausulasLibres_.Count > 1 Then

                        responseJson_ = New JavaScriptSerializer().Serialize(CallByName(obj, method_, vbMethod, clausulasLibres_))

                    Else

                        responseJson_ = New JavaScriptSerializer().Serialize(CallByName(obj, method_, vbMethod, clausulasLibres_("clausulasLibres_")))

                    End If

                Catch ex As Exception

                    responseJson_ = "{""code"":""400"",""response"":"""",""message"":""No se encontro un punto de respuesta""}"

                End Try

            End If

            'Send the Response in JSON format to Client.

            response.ContentType = "text/json"

            response.Write(responseJson_)

            response.End()

        End If

    End Sub

#End Region

#Region "Acciones"

#End Region

End Class
