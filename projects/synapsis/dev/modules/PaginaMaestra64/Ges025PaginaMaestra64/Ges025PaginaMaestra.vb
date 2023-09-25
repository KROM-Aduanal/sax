Imports Gsol.Web.Modulos.Configuracion
Imports Gsol.Krom.ControladorAccesoKBW64
Imports Gsol.Krom.MenuDinamico
Imports Gsol.Web.Components
Imports System.Web
Imports Gsol.Krom
Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports Gsol.Web.Gsol

Public Class Ges025PaginaMaestra
    Inherits System.Web.UI.MasterPage

#Region "Atributos"

    Private _componente As KromComponentes

    Private _nombreUsuario As String

    Private _imagenUsuario As String

    Private _menuDinamico As String

    Const _paginaLogIn As String = "/FrontEnd/Modulos/Generales/LogIn/LogIn.aspx?sta=0"

    Private _accessControl As ControladorAccesoKBW

#End Region

#Region "Propiedades"

    ReadOnly Property Componente As KromComponentes

        Get

            Return _componente

        End Get

    End Property

    ReadOnly Property NombreUsuario As String

        Get

            Return _nombreUsuario

        End Get

    End Property

    ReadOnly Property ImagenUsuario As String

        Get

            Return _imagenUsuario

        End Get

    End Property

    ReadOnly Property MenuDinamico As String

        Get

            Return _menuDinamico

        End Get

    End Property

#End Region

#Region "Constructores"

    Sub New()

        _componente = New KromComponentes()

        _accessControl = New ControladorAccesoKBW()

        _nombreUsuario = ""

        _imagenUsuario = ""

        _menuDinamico = ""

    End Sub

#End Region

#Region "Metodos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not _accessControl.SesionAbierta() Then

            If Not _accessControl.IntentarConexion() Then

                Response.Redirect(_paginaLogIn)

            End If

        End If

        If Session("usuario") Is Nothing Then

            Response.Redirect(_paginaLogIn)

        Else

            If Not Session("ControladorWeb") Is Nothing Then

                Dim request_ = HttpContext.Current.Request

                Dim rutaImagenUsuario_ As String = Server.MapPath("/FrontEnd/Recursos/Imgs/" & Session("DatosUsuario").item("Imagen"))

                Dim urlImagen_ = request_.Url.GetLeftPart(UriPartial.Authority) & request_.ApplicationPath & "FrontEnd/Recursos/Imgs/" & Session("DatosUsuario").item("Imagen")

                urlImagen_ = urlImagen_.Replace(":8083", "")

                Dim rutaImagenAvatar_ As String = "/FrontEnd/Recursos/Imgs/avatarkrom.png"

                'Obtiene el nombre del usuario
                _nombreUsuario = Session("DatosUsuario").item("Nombre")

                'Valida la imagen de perfil del usuario
                If Session("DatosUsuario").item("Imagen") <> "" Then

                    If System.IO.File.Exists(rutaImagenUsuario_) Then

                        _imagenUsuario = urlImagen_

                    Else

                        _imagenUsuario = rutaImagenAvatar_

                    End If

                Else

                    _imagenUsuario = rutaImagenAvatar_

                End If

                'Response.AppendHeader("Refresh", ((Session.Timeout * 60) + 5).ToString() + "; Url=http://web.kromaduanal.com/default.aspx")

                'Se envía el menu dinamico
                Dim menuDinamico_ As New MenuDinamicoWeb

                _menuDinamico = menuDinamico_.CrearJSONMenu(Session("EspacioTrabajoExtranet"))

            Else

                Response.Redirect(_paginaLogIn)

            End If

        End If

    End Sub

    'Obtiene el nombre del usuario con base a la clase PerfilUsuario (Ges003-001-Edicion.PerfilUsuario.aspx)
    Private Sub ObtenerNombreUsuario()

        Dim perfilUsuario_ As New Ges003_001_PerfilUsuario

        Dim usuario_ As Dictionary(Of String, String) = perfilUsuario_.InformacionUsuario()

        _nombreUsuario = If(usuario_.Count > 0, usuario_.Item("nombre_usuario") & " " & usuario_.Item("apellidoPaterno_usuario"), "")

    End Sub

#End Region

End Class
