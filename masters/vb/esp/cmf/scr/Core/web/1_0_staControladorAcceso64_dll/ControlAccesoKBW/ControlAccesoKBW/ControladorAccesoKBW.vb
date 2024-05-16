Imports gsol.seguridad
Imports gsol.krom.controladores
Imports Wma.WebServices
Imports System.Web
Imports System.Security.Cryptography
Imports System.Web.Script.Serialization


Public Class ControladorAccesoKBW

#Region "Atributos"

    Private _contrasenaServicioWeb As String

    Private _datosUsuario As Dictionary(Of String, String)

    Private _cantidadRegistros As Dictionary(Of String, Integer)

    Private _wsSesion As ISesion

    Private _userProfile As UserProfile

    Private _controladorWeb As ControladorWeb

    Private _espacioTrabajoExtranet As IEspacioTrabajo

    Private _credenciales As ICredenciales

#End Region

#Region "Propiedades"

#End Region

#Region "Constructores"

#End Region

#Region "Metodos"

    Private Function CodificarJson(ByVal Data As Dictionary(Of String, String)) As String

        Dim JSON = New JavaScriptSerializer()

        Return JSON.Serialize(Data)

    End Function

    Private Function DecodificarJson(ByVal PlainText As String) As Dictionary(Of String, Object)

        Dim JSON = New JavaScriptSerializer()

        Return JSON.DeserializeObject(PlainText)

    End Function

    Private Function EncriptarCadena(ByVal plainText As String) As String

        Dim organismo_ = New Organismo

        Dim metodo_ As SymmetricAlgorithm = New RijndaelManaged

        Dim cifrado_ As ICifrado = New Cifrado256

        Dim cadenaCifrada_ As String = cifrado_.CifraCadena(plainText.ToString, metodo_, organismo_.ManifiestoGlobal(Configuracion.DatosGlobalesSistema.LlaveCifrado))

        Return cadenaCifrada_

    End Function

    Private Function DesencriptarCadena(ByVal cipherText As String) As String

        Dim organismo_ = New Organismo

        Dim metodo_ As SymmetricAlgorithm = New RijndaelManaged

        Dim cifrado_ As ICifrado = New Cifrado256

        Dim cadenaDescifrada_ As String = cifrado_.DescifraCadena(cipherText, metodo_, organismo_.ManifiestoGlobal(Configuracion.DatosGlobalesSistema.LlaveCifrado))

        Return cadenaDescifrada_

    End Function

    Private Function Autorizar(ByVal datosUsuario_ As Dictionary(Of String, Object)) As Boolean

        _contrasenaServicioWeb = "AB24rsdAQ54"

        _datosUsuario = New Dictionary(Of String, String)

        _cantidadRegistros = New Dictionary(Of String, Integer)

        _wsSesion = New SesionWcf

        _userProfile = New UserProfile

        _espacioTrabajoExtranet = New EspacioTrabajo

        _credenciales = New Credenciales

        Dim _statements As Sax.SaxStatements = Sax.SaxStatements.GetInstance()

        Dim appid_ As Integer = _statements.GetAppOnLine().saxidapp

        '12 '_statements.GetAppOnLine().saxidapp

        _userProfile = _wsSesion.GetProfileWorkSpace(_espacioTrabajoExtranet,
                                                     datosUsuario_("usuario"),
                                                     datosUsuario_("usuario"),
                                                     datosUsuario_("contrasena"),
                                                     appid_,
                                                     1,
                                                     1,
                                                     True)

        If _userProfile.Result.Status = Wma.Exceptions.TagWatcher.TypeStatus.Ok Then


            If _espacioTrabajoExtranet.SectorEntorno Is Nothing Or
               _espacioTrabajoExtranet.SectorEntorno.Count = 0 Then

                _userProfile.Result.SetError(Wma.Exceptions.TagWatcher.ErrorTypes.KBW_040_0002)

                System.Web.HttpContext.Current.Session("fallaLogin_") = _userProfile.Result.ErrorDescription

                Return False

            Else

                System.Web.HttpContext.Current.Session("usuario") = datosUsuario_("usuario")

                System.Web.HttpContext.Current.Session("contrasena") = datosUsuario_("contrasena")

                'Cargar variables de sesión y elementos de trabajo
                With _datosUsuario

                    .Add("MobilUserID", datosUsuario_("usuario"))

                    .Add("WebServiceUserID", datosUsuario_("usuario"))

                    .Add("WebServicePasswordID", datosUsuario_("contrasena"))

                    .Add("Nombre", _userProfile.Name & " " & _userProfile.PLastName)

                    .Add("Compañia", _userProfile.CompanyName)

                    .Add("Telefono", _userProfile.Phone)

                    .Add("Cumpleaños", _userProfile.BirthDate)

                    .Add("Correo", _userProfile.EMail)

                    .Add("Imagen", _userProfile.URLUserPicture)

                End With

                _cantidadRegistros.Add("LimiteRegistrosColeccionMuestral", 300)

                _cantidadRegistros.Add("LimiteResultados", 10000)

                _cantidadRegistros.Add("LimiteMuestralMinimo", 20)

                _credenciales = _espacioTrabajoExtranet.MisCredenciales

                _controladorWeb = New ControladorWeb(_espacioTrabajoExtranet)


                If Not _controladorWeb.ClausulasSQLReglasAcceso Is Nothing Then

                    System.Web.HttpContext.Current.Session("DatosUsuario") = _datosUsuario

                    System.Web.HttpContext.Current.Session("CantidadRegistros") = _cantidadRegistros

                    System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet") = _espacioTrabajoExtranet

                    System.Web.HttpContext.Current.Session("ControladorWeb") = _controladorWeb

                    Return True

                Else

                    System.Web.HttpContext.Current.Session("fallaLogin_") = _userProfile.Result.ErrorDescription

                    Return False

                End If

            End If

        End If

        Return False

    End Function

    Public Function SesionAbierta() As Boolean

        If Not System.Web.HttpContext.Current.Session("ControladorWeb") Is Nothing And
            Not System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet") Is Nothing Then

            Return True

        Else

            Return False

        End If

    End Function

    Public Function IntentarConexion() As Boolean

        If Not System.Web.HttpContext.Current.Request.Cookies("cookieSesion") Is Nothing Then

            Dim TokenUsuarioCadena_ = System.Web.HttpContext.Current.Request.Cookies("cookieSesion").Values("tokenDeUsuario")

            If Not TokenUsuarioCadena_ Is Nothing Then

                Dim TokenUsuarioObjecto_ = DecodificarJson(DesencriptarCadena(TokenUsuarioCadena_))

                If Not TokenUsuarioObjecto_ Is Nothing Then

                    Dim controlAcceso_ As New ControladorAccesoKBW()

                    Return controlAcceso_.Autorizar(TokenUsuarioObjecto_)

                End If

            End If

        End If

        Return False

    End Function

    Public Sub GuardarConexion(ByVal credenciales_ As Dictionary(Of String, String))

        Dim tokenUsuarioCadena_ = CodificarJson(credenciales_)

        Dim cookieSesion_ As New HttpCookie("cookieSesion")

        With cookieSesion_

            .Values("tokenDeUsuario") = EncriptarCadena(tokenUsuarioCadena_)

            .Expires = DateTime.MaxValue

        End With

        System.Web.HttpContext.Current.Response.AppendCookie(cookieSesion_)


    End Sub

    Public Sub Desconectar()

        Dim httpcontext_ = New HttpContextWrapper(HttpContext.Current)

        Dim _response = httpcontext_.Response

        Dim cookie_ = New HttpCookie("cookieSesion")

        With cookie_

            .Expires = DateTime.Now.AddDays(-1)

        End With

        _response.Cookies.Set(cookie_)

        HttpContext.Current.Session.RemoveAll()

        'HttpContext.Current.Request.Cookies.Clear()

        HttpContext.Current.Session.Abandon()

    End Sub

#End Region

End Class
