Imports System.Web.Services
Imports System.Web
Imports gsol.BaseDatos.Operaciones
Imports System.Net
Imports System.Web.Script.Serialization
Imports gsol.correoelectronico
Imports Wma.Exceptions
Imports gsol.seguridad
Imports System.Text

Public Class RecuperarContrasena
    Inherits System.Web.UI.Page

#Region "Metodos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <WebMethod>
    Public Shared Function EnviarContrasena(ByVal data_ As Object)

        Dim espacioTrabajo_ As New EspacioTrabajo

        Dim organismo_ As New Organismo

        Dim httpContext_ As HttpContext = HttpContext.Current

        Dim codigo_ As Int32 = 400

        Dim mensaje_ As String = "Algo salió mal"

        Dim usuario_ As String = data_("user")

        Dim respuestaRecaptcha_ As String = data_("g-recaptcha-response")

        If respuestaRecaptcha_ <> "" Then

            If Not espacioTrabajo_ Is Nothing Then

                Dim ioperacionesnew_ As IOperacionesCatalogo

                ioperacionesnew_ = organismo_.EnsamblaModulo("Usuarios")

                With ioperacionesnew_

                    .EspacioTrabajo = espacioTrabajo_

                    .ClausulasLibres = " AND t_Usuario = '" & usuario_ & "'"

                End With

                ioperacionesnew_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                ioperacionesnew_.GenerarVista()

                If organismo_.TieneResultados(ioperacionesnew_) Then

                    Dim webClient_ = New WebClient

                    Dim response_ As String = respuestaRecaptcha_

                    Dim secretKey_ As String = "6Lc2rocUAAAAACVoBi28g-_3Gd0uMcKsHRQ8XZv0"

                    Dim result_ As String = webClient_.DownloadString(String.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey_, response_))

                    Dim googleResult_ As Dictionary(Of String, String) = New JavaScriptSerializer().Deserialize(Of Dictionary(Of String, String))(result_)

                    Dim recaptchaStatus_ As Boolean = googleResult_("success")

                    If recaptchaStatus_ Then

                        Dim t_Usuario_ As String = ioperacionesnew_.Vista(0, "t_Usuario", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        Dim t_Nombre_ As String = ioperacionesnew_.Vista(0, "t_Nombre", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        Dim i_Cve_Usuario_ As String = ioperacionesnew_.Vista(0, "i_Cve_Usuario", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        'codigo_ = 200

                        mensaje_ = organismo_.GegeraContrasena(Organismo.NivelContrasena.media)

                        'pendienteeeeeeee

                        If EnviarCorreo(i_Cve_Usuario_, t_Usuario_, t_Nombre_, organismo_.GegeraContrasena(Organismo.NivelContrasena.media)) Then
                            'If 1 Then

                            codigo_ = 200

                            mensaje_ = "Se te ha enviado un correo con tu nueva contraseña"

                        Else

                            codigo_ = 400

                            mensaje_ = "Algo salió mal, intenta nuevamente"

                        End If

                    Else

                        codigo_ = 400

                        mensaje_ = "Algo salió mal, intenta nuevamente"

                    End If

                Else

                    codigo_ = 400

                    mensaje_ = "No encontramos el usuario"

                End If

            End If

        Else

            codigo_ = 400

            mensaje_ = "¿Eres un robot? Debes activar el reCAPTCHA"

        End If

        Dim respuesta_ As String = New JavaScriptSerializer().Serialize(New With {.code = codigo_, .message = mensaje_})

        Return respuesta_

    End Function

    Private Shared Function EnviarCorreo(ByVal i_Cve_Usuario_ As Integer, ByVal usuario_ As String, ByVal nombreUsuario_ As String, ByVal nuevaContrasena_ As String)

        Dim espacioTrabajoExtranet_ As IEspacioTrabajo

        Dim modalidad_ As IEspacioTrabajo.ModalidadesEspacio

        Dim credenciales_ As gsol.ICredenciales

        espacioTrabajoExtranet_ = New EspacioTrabajo

        credenciales_ = New Credenciales

        modalidad_ = IEspacioTrabajo.ModalidadesEspacio.ProduccionExtranet

        With credenciales_

            .Aplicacion = 4

            .CredencialUsuario = usuario_

            .DivisionEmpresaria = 1

        End With

        With espacioTrabajoExtranet_

            .ModalidadEspacio = modalidad_

            .MisCredenciales = credenciales_

        End With

        Dim operacionesCatalogo_ = New OperacionesCatalogo

        Dim operacionesCorreo_ = New OperacionesCorreoElectronico64

        Dim correo_ = New MensajeCorreoElectronico

        Dim destinatario_ = New List(Of String) From {usuario_}

        Dim copiaOculta_ = New List(Of String) From {"juan.castellanos@kromaduanal.com"}

        With operacionesCatalogo_

            .EspacioTrabajo = espacioTrabajoExtranet_

        End With

        Dim cuerpoCorreo_ As New StringBuilder()

        cuerpoCorreo_.Append("<!DOCTYPE html>")

        cuerpoCorreo_.Append("<html lang=""en"">")

        cuerpoCorreo_.Append("<head>")

        cuerpoCorreo_.Append("    <meta charset=""UTF-8"">")

        cuerpoCorreo_.Append("    <title>Solicitud pago</title>")

        cuerpoCorreo_.Append("</head>")

        cuerpoCorreo_.Append("<body bgcolor=""#eaeff2"">")

        cuerpoCorreo_.Append("    <div style=""font-family: Arial,sans-serif;font-size: 14px;text-align: center;margin: 0;padding: 0;background-color: #eaeff2;box-sizing: border-box;"">")

        cuerpoCorreo_.Append("        <table width=""500"" align=""center"" cellpadding=""0"" style=""border-collapse:collapse;"">")

        cuerpoCorreo_.Append("            <tbody style=""border: 1px solid #eceff1;"">")

        cuerpoCorreo_.Append("                <tr bgcolor=""#ffffff"" align=""center"">")

        cuerpoCorreo_.Append("                    <td align=""center"" style=""border-top:6px solid #217d7e;""></td>")

        cuerpoCorreo_.Append("                </tr>")

        cuerpoCorreo_.Append("                <tr bgcolor=""#ffffff"" align=""center"">")

        cuerpoCorreo_.Append("                    <td align=""center"" style=""padding:25px 0px 15px 0px;color:#217d7e;font-size:20px;""><strong>RECUPERACIÓN DE CONTRASEÑA</strong></td>")

        cuerpoCorreo_.Append("                </tr>")

        cuerpoCorreo_.Append("                <tr bgcolor=""#ffffff"" align=""center"">")

        cuerpoCorreo_.Append("                    <td style=""font-size:16px;color:#272727;""><div style=""margin-bottom: 16px;"">Hola <strong>" & nombreUsuario_ & "</strong> lamentamos que perdieras tu contraseña, pero no te preocupes te proporcionamos una nueva.</div></td>")

        cuerpoCorreo_.Append("                </tr>")

        cuerpoCorreo_.Append("                <tr bgcolor=""#ffffff"" align=""center"">")

        cuerpoCorreo_.Append("                    <td style=""font-size:16px;color:#272727;""><div style=""margin-bottom: 16px;"">Tu nueva contraseña es <strong>" & nuevaContrasena_ & "</strong></div></td>")

        cuerpoCorreo_.Append("                </tr>")

        cuerpoCorreo_.Append("                <tr bgcolor=""#ffffff"" align=""center"">")

        cuerpoCorreo_.Append("                    <td>")

        cuerpoCorreo_.Append("                        <div style=""border-bottom:1px dashed #cccccc;width: 90%;margin-bottom: 10px;""></div>")

        cuerpoCorreo_.Append("                    </td>")

        cuerpoCorreo_.Append("                </tr>")

        cuerpoCorreo_.Append("                <tr bgcolor=""#ffffff"" align=""center"">")

        cuerpoCorreo_.Append("                    <td style=""font-size:12px;color:#272727;padding: 0px 55px;""><div style=""margin-bottom:  10px;"">Si tu nueva contraseña no es de tu agrado, dentro de tu perfil puedes cambiarla en el momento que deseés.</div></td>")

        cuerpoCorreo_.Append("                </tr>")

        cuerpoCorreo_.Append("                <tr bgcolor=""#ffffff"" align=""center"">")

        cuerpoCorreo_.Append("                    <td style=""font-size:10px;color:#272727;padding: 0px 40px;""><div style=""margin-bottom: 10px;"">El equipo de desarrollo de KROM te desea un excelente día y siempre esta trabajando para brindarte una mejor experiencia.</div></td>")

        cuerpoCorreo_.Append("                </tr>")

        cuerpoCorreo_.Append("                <tr bgcolor=""#212D38"" align=""center"">")

        cuerpoCorreo_.Append("                    <td style=""font-size: 16px;color: #d8d8d8;padding: 25px 0;"">")

        cuerpoCorreo_.Append("                        <strong>Cómplices</strong> de tu liderazgo con innovación y entrega")

        cuerpoCorreo_.Append("                    </td>")

        cuerpoCorreo_.Append("                </tr>")

        cuerpoCorreo_.Append("            </tbody>")

        cuerpoCorreo_.Append("        </table>")

        cuerpoCorreo_.Append("    </div>")

        cuerpoCorreo_.Append("</body>")

        cuerpoCorreo_.Append("</html>")

        With correo_

            .Too = destinatario_

            .BCC = copiaOculta_

            .Subject = "Recuperación de contraseña"

            .Body = cuerpoCorreo_.ToString()

        End With

        operacionesCorreo_.EnviarCorreo(operacionesCatalogo_,
                                        correo_,
                                        IOperacionesCorreoElectronico.Prioridades.Alta,
                                        IOperacionesCorreoElectronico.CantidadIntentosEnvio.Normal,
                                        IOperacionesCorreoElectronico.Modalidades.Individual)

        If operacionesCorreo_.Estatus.ErrorDescription <> TagWatcher.TypeStatus.Errors Then

            If CifrarContrasena(i_Cve_Usuario_, nuevaContrasena_) Then

                Return True

            Else

                Return False

            End If

        Else

            Return False

        End If

    End Function

    Private Shared Function CifrarContrasena(ByVal i_Cve_Usuario_ As Integer, ByVal nuevaContrasena As String)

        Dim organismo_ As New Organismo

        Dim cifrado_ As ICifrado = New Cifrado256

        Dim tokens_ As New List(Of String) From {"Usuarios", "KardexPermisos"}

        Dim cambioContrasena As Boolean = False

        For Each token_ As String In tokens_

            Dim ioperacionesUpdate_ As IOperacionesCatalogo

            ioperacionesUpdate_ = organismo_.EnsamblaModulo(token_)

            ioperacionesUpdate_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

            ioperacionesUpdate_.PreparaCatalogo()

            Dim _lista As New List(Of String)

            _lista.Add("t_Contrasena")

            ioperacionesUpdate_.CampoPorNombre("t_Contrasena") = cifrado_.CifraCadena(nuevaContrasena, ICifrado.Metodos.SHA1)

            organismo_.OptimizaOperacion(ioperacionesUpdate_, _lista, IOperacionesCatalogo.TiposOperacionSQL.Modificar)

            If ioperacionesUpdate_.Modificar(i_Cve_Usuario_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                cambioContrasena = True

            Else

                cambioContrasena = False

            End If

        Next

        Return cambioContrasena

    End Function

#End Region

End Class
