Imports System.Web.Services
Imports gsol.krom.controladores
Imports gsol.krom.menuDinamico

Public Class Home

    Inherits System.Web.UI.MasterPage

    'Public _session As SessionApp.UserProfile

#Region "Atributos"

    Private _ligaRutasComponentes As String

    Private _nombreUsuario As String

    Private _imagenUsuario As String

    Private _menuDinamico As String

#End Region

#Region "Constructores"

    Sub New()

        _ligaRutasComponentes = "http://localhost:1755/CapaPresentacion/"

        _nombreUsuario = ""

        _imagenUsuario = ""

        _menuDinamico = ""

    End Sub

#End Region

#Region "Propiedades"

    ReadOnly Property LigaRutasComponentes As String

        Get

            Return _ligaRutasComponentes

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

#Region "Metodos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("usuario") Is Nothing Then

            Response.Redirect("../default.aspx")

        Else

            If Not Session("ControladorWeb") Is Nothing Then

                Dim request_ = HttpContext.Current.Request

                Dim rutaImagenUsuario_ As String = Server.MapPath("~/CapaPresentacion/Componentes/imgs/" & Session("DatosUsuario").item("Imagen"))

                Dim urlImagen_ = request_.Url.GetLeftPart(UriPartial.Authority) & request_.ApplicationPath & "CapaPresentacion/Componentes/imgs/" & Session("DatosUsuario").item("Imagen")

                urlImagen_ = urlImagen_.Replace(":8083", "")

                'Dim urlImagen_ = request_.Url.Host & request_.ApplicationPath & "CapaPresentacion/Componentes/imgs/" & Session("DatosUsuario").item("Imagen")

                Dim rutaImagenAvatar_ As String = "/CapaPresentacion/Componentes/dist/img/avatarkrom.png"

                'ObtenerNombreUsuario()

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

                'Session("DatosUsuario").item("Imagen") = _imagenUsuario

                Response.AppendHeader("Refresh", ((Session.Timeout * 60) + 5).ToString() + "; Url=http://web.kromaduanal.com/default.aspx")

                'Se envía el menu dinamico
                'Dim algo As Object = Session("EspacioTrabajoExtranet")
                Dim menuDinamico_ As New MenuDinamicoWeb

                _menuDinamico = menuDinamico_.CrearJSONMenu(Session("EspacioTrabajoExtranet"))
                '_menuDinamico = menuDinamico_.ArmarMenuDinamico()

            Else

                Response.Redirect("../default.aspx")

            End If

        End If

    End Sub

    'Obtiene el nombre del usuario con base a la clase PerfilUsuario (Ges003-001-Edicion.PerfilUsuario.aspx)
    Private Sub ObtenerNombreUsuario()

        Dim perfilUsuario_ As New PerfilUsuario

        Dim usuario_ As Dictionary(Of String, String) = perfilUsuario_.InformacionUsuario()

        _nombreUsuario = If(usuario_.Count > 0, usuario_.Item("nombre_usuario") & " " & usuario_.Item("apellidoPaterno_usuario"), "")

    End Sub

#End Region

End Class