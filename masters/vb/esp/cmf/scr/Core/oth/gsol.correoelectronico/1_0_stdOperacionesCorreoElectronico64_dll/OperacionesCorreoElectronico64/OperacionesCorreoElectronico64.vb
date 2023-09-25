Imports Wma.Exceptions
Imports gsol.BaseDatos.Operaciones
Imports gsol.correoelectronico
Imports gsol.seguridad
Imports System.Security.Cryptography
Imports Limilabs.Client.SMTP
Imports Limilabs.Mail
Imports Limilabs.Mail.Headers
Imports Limilabs.Mail.Fluent
Imports Limilabs.Mail.MIME
Imports gsol.documento
Imports System.IO
Imports Limilabs.Client.IMAP
Imports gsol.Componentes.SistemaBase
Imports System.Text
Imports gsol.Controladores

Namespace gsol.correoelectronico

    Public Class OperacionesCorreoElectronico64
        Implements IOperacionesCorreoElectronico

#Region "Atributos"

        Private _estatus As TagWatcher

        Private _operaciones As IOperacionesCatalogo

        Private _sistema As Organismo

        Private _mensajeCorreoElectronico As MensajeCorreoElectronico

        Private _coleccionCorreos As Dictionary(Of String, Integer)

        Private _coleccionDominiosNoPermitidos As Dictionary(Of String, Integer)

        Private _bitacoraLecturaBuzonEntrada As BitacoraLecturaBuzonEntrada

        Public _archivosReprocesado As Dictionary(Of String, String)

        Public _correosAProcesar As Integer = 0

        Public _correosProcesados As Integer = 0

        Public _correosEfectivamenteProcesados As Integer = 0

        Private _correoControl As ControlCorreo64

#End Region

#Region "Propiedades"

        Public Property MensajeCorreoElectronico As MensajeCorreoElectronico Implements IOperacionesCorreoElectronico.MensajeCorreoElectronico

            Get

                Return _mensajeCorreoElectronico

            End Get

            Set(value As MensajeCorreoElectronico)

                _mensajeCorreoElectronico = value

            End Set

        End Property

        Public Property Operaciones As IOperacionesCatalogo Implements IOperacionesCorreoElectronico.Operaciones

            Get

                Return _operaciones

            End Get

            Set(value As IOperacionesCatalogo)

                _operaciones = value

            End Set

        End Property

        Public ReadOnly Property Estatus As TagWatcher Implements IOperacionesCorreoElectronico.Estatus

            Get

                Return _estatus

            End Get

        End Property

        Public Property BitacoraLecturaBuzonEntrada As BitacoraLecturaBuzonEntrada

            Get

                Return _bitacoraLecturaBuzonEntrada

            End Get

            Set(value As BitacoraLecturaBuzonEntrada)

                _bitacoraLecturaBuzonEntrada = value

            End Set

        End Property



#End Region

#Region "Constructores"

        Sub New()

            _operaciones = New OperacionesCatalogo

            _estatus = New TagWatcher

            _estatus.SetOK()

            _sistema = New Organismo

            _mensajeCorreoElectronico = New MensajeCorreoElectronico

            _bitacoraLecturaBuzonEntrada = New BitacoraLecturaBuzonEntrada

        End Sub

        Sub New(ByVal ioperaciones_ As OperacionesCatalogo)

            Me.New()

            _operaciones = ioperaciones_

        End Sub

#End Region

#Region "Métodos"

        'Métodos encargados del envío del correo electrónico
        Public Sub EnviarCorreo(ByVal ioperacionescatalogo_ As IOperacionesCatalogo,
                                ByVal mensajeCorreoElectronico_ As MensajeCorreoElectronico,
                                Optional prioridad_ As IOperacionesCorreoElectronico.Prioridades = IOperacionesCorreoElectronico.Prioridades.Normal,
                                Optional cantidadIntentosEnvio_ As IOperacionesCorreoElectronico.CantidadIntentosEnvio = IOperacionesCorreoElectronico.CantidadIntentosEnvio.Normal,
                                Optional modalidad_ As IOperacionesCorreoElectronico.Modalidades = IOperacionesCorreoElectronico.Modalidades.Individual) Implements IOperacionesCorreoElectronico.EnviarCorreo

            _estatus.SetOK()

            _operaciones = ioperacionescatalogo_

            If Not mensajeCorreoElectronico_.Too Is Nothing And mensajeCorreoElectronico_.Too.Count > 0 Then

                Select Case modalidad_

                    Case IOperacionesCorreoElectronico.Modalidades.Individual

                        ProcesarEnvioCorreo(mensajeCorreoElectronico_,
                                            mensajeCorreoElectronico_.Too,
                                            prioridad_,
                                            cantidadIntentosEnvio_,
                                            modalidad_)

                    Case IOperacionesCorreoElectronico.Modalidades.Masivo

                        For Each destinatario_ As String In mensajeCorreoElectronico_.Too

                            Dim listaDistribucion = New List(Of String)

                            listaDistribucion.Add(destinatario_)

                            ProcesarEnvioCorreo(mensajeCorreoElectronico_,
                                                listaDistribucion,
                                                prioridad_,
                                                cantidadIntentosEnvio_,
                                                modalidad_)

                        Next

                End Select

            Else

                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1012)

            End If

        End Sub

        Public Sub EnviarCorreo(ByVal mensajeCorreoElectronico_ As MensajeCorreoElectronico,
                                     Optional prioridad_ As IOperacionesCorreoElectronico.Prioridades = IOperacionesCorreoElectronico.Prioridades.Normal,
                                     Optional cantidadIntentosEnvio_ As IOperacionesCorreoElectronico.CantidadIntentosEnvio = IOperacionesCorreoElectronico.CantidadIntentosEnvio.Normal,
                                     Optional modalidad_ As IOperacionesCorreoElectronico.Modalidades = IOperacionesCorreoElectronico.Modalidades.Individual) Implements IOperacionesCorreoElectronico.EnviarCorreo

            _estatus.SetOK()

            EnviarCorreo(_operaciones.Clone,
                         mensajeCorreoElectronico_,
                         prioridad_,
                         cantidadIntentosEnvio_,
                         modalidad_)

        End Sub

        Private Sub ProcesarEnvioCorreo(ByVal mensajeCorreoElectronico_ As MensajeCorreoElectronico,
                                        ByVal too_ As List(Of String),
                                        Optional prioridad_ As IOperacionesCorreoElectronico.Prioridades = IOperacionesCorreoElectronico.Prioridades.Normal,
                                        Optional cantidadIntentosEnvio_ As IOperacionesCorreoElectronico.CantidadIntentosEnvio = IOperacionesCorreoElectronico.CantidadIntentosEnvio.Normal,
                                        Optional modalidad_ As IOperacionesCorreoElectronico.Modalidades = IOperacionesCorreoElectronico.Modalidades.Individual)

            Try

                Dim cuentaCorreo_ = New CuentaCorreo

                Dim buzonEnvio_ = New OperacionesCatalogo

                buzonEnvio_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                buzonEnvio_ = _sistema.ConsultaModulo(_operaciones.EspacioTrabajo,
                                                      "BuzonEnvio",
                                                      " and i_Cve_DivisionMiEmpresa = " & _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria)

                If _sistema.TieneResultados(buzonEnvio_) Then

                    For Each fila_ As DataRow In buzonEnvio_.Vista.Tables(0).Rows

                        With cuentaCorreo_

                            .ClaveCuentaCorreo = _sistema.ValidarVacios(fila_.Item("Clave BuzonEnvio").ToString, Organismo.VerificarTipoDatoDBNULL.Cadena)

                            .Correo = _sistema.ValidarVacios(fila_.Item("Correo").ToString, Organismo.VerificarTipoDatoDBNULL.Cadena)

                            .Contrasenia = _sistema.ValidarVacios(fila_.Item("Contrasenia").ToString, Organismo.VerificarTipoDatoDBNULL.Cadena)

                            .NombreServidor = _sistema.ValidarVacios(fila_.Item("Nombre Servidor").ToString, Organismo.VerificarTipoDatoDBNULL.Cadena)

                        End With

                    Next

                    Dim proEmail_ As New MailBuilder()

                    proEmail_.Html = mensajeCorreoElectronico_.Body

                    proEmail_.Subject = mensajeCorreoElectronico_.Subject

                    proEmail_.From.Add(New MailBox(cuentaCorreo_.Correo))

                    Dim documentoAdjunto_ As MimeData

                    If mensajeCorreoElectronico_.Adjuntos.Count > 0 Then

                        For Each adjunto_ As documento.Documento In mensajeCorreoElectronico_.Adjuntos

                            If Not adjunto_.RutaDocumentoOrigen Is Nothing And adjunto_.Clave > 0 Then

                                If File.Exists(adjunto_.RutaDocumentoOrigen) Then

                                    documentoAdjunto_ = proEmail_.AddAttachment(adjunto_.RutaDocumentoOrigen)

                                    If adjunto_.NombreDocumentoOriginal Is Nothing Then

                                        documentoAdjunto_.FileName = adjunto_.NombreDocumento

                                    Else

                                        documentoAdjunto_.FileName = adjunto_.NombreDocumentoOriginal

                                    End If

                                Else

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1010)

                                    Exit Sub

                                End If

                            Else

                                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1009)

                                Exit Sub

                            End If

                        Next

                    End If

                    Select Case prioridad_

                        Case IOperacionesCorreoElectronico.Prioridades.Alta

                            proEmail_.PriorityHigh()

                        Case IOperacionesCorreoElectronico.Prioridades.Normal

                        Case IOperacionesCorreoElectronico.Prioridades.Baja

                            proEmail_.PriorityLow()

                    End Select

                    If Not too_ Is Nothing Then

                        For Each destinatario_ As String In too_

                            proEmail_.To.Add(New MailBox(destinatario_))

                        Next

                    End If

                    If Not mensajeCorreoElectronico_.CC Is Nothing Then

                        For Each destinatarioCopia_ As String In mensajeCorreoElectronico_.CC

                            proEmail_.Cc.Add(New MailBox(destinatarioCopia_))

                        Next

                    End If

                    If Not mensajeCorreoElectronico_.BCC Is Nothing Then

                        For Each destinatarioCopiaOculta_ As String In mensajeCorreoElectronico_.BCC

                            proEmail_.Bcc.Add(New MailBox(destinatarioCopiaOculta_))

                        Next

                    End If

                    Dim email_ As IMail = proEmail_.Create()

                    Using smtp_ As New Smtp

                        mensajeCorreoElectronico_.CantidadIntentos = 0

                        smtp_.Connect(cuentaCorreo_.NombreServidor)

                        smtp_.UseBestLogin(cuentaCorreo_.Correo, DescifrarContraseña(cuentaCorreo_.Contrasenia))

                        While mensajeCorreoElectronico_.CantidadIntentos < cantidadIntentosEnvio_

                            Dim result_ As ISendMessageResult = smtp_.SendMessage(email_)

                            If result_.Status = 0 Then

                                mensajeCorreoElectronico_.EstatusMensajeSalida = IOperacionesCorreoElectronico.EstatusMensajeCorreoElectronicoSalida.Enviado

                                Dim CorreoElectronicoEnviado_ = New OperacionesCatalogo

                                CorreoElectronicoEnviado_.EspacioTrabajo = _operaciones.EspacioTrabajo

                                CorreoElectronicoEnviado_ = _sistema.EnsamblaModulo("CorreoElectronicoEnviado")

                                With CorreoElectronicoEnviado_

                                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                                    mensajeCorreoElectronico_.SenderFrom = cuentaCorreo_.Correo

                                    .CampoPorNombre("t_Subject") = mensajeCorreoElectronico_.Subject

                                    .CampoPorNombre("t_Sender") = cuentaCorreo_.Correo

                                    .CampoPorNombre("t_To") = RetornarListaToString(too_,
                                                                                    ";",
                                                                                    False)

                                    .CampoPorNombre("t_CC") = RetornarListaToString(mensajeCorreoElectronico_.CC,
                                                                                    ";",
                                                                                    False)

                                    .CampoPorNombre("t_BCC") = RetornarListaToString(mensajeCorreoElectronico_.BCC,
                                                                                    ";",
                                                                                    False)

                                    .CampoPorNombre("i_Cve_BuzonEnvio") = cuentaCorreo_.ClaveCuentaCorreo

                                    .CampoPorNombre("f_Envio") = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")

                                    .CampoPorNombre("i_Cve_Usuario") = _operaciones.EspacioTrabajo.MisCredenciales.ClaveUsuario

                                    .CampoPorNombre("i_Cve_Estado") = 1

                                    .CampoPorNombre("i_Cve_Estatus") = 1

                                    .CampoPorNombre("i_Cve_DivisionMiEmpresa") = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

                                    If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                        Dim ExtCorreoElectronicoEnviado_ = New OperacionesCatalogo

                                        ExtCorreoElectronicoEnviado_.EspacioTrabajo = _operaciones.EspacioTrabajo

                                        ExtCorreoElectronicoEnviado_ = _sistema.EnsamblaModulo("ExtCorreoElectronicoEnviado")

                                        With ExtCorreoElectronicoEnviado_

                                            .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                                            .CampoPorNombre("i_Cve_CorreoElectronicoEnviado") = CorreoElectronicoEnviado_.ValorIndice

                                            .CampoPorNombre("t_Body") = email_.GetBodyAsText

                                            .CampoPorNombre("i_Cve_Estado") = 1

                                            .CampoPorNombre("i_Cve_Estatus") = 1

                                            .CampoPorNombre("i_Cve_DivisionMiEmpresa") = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

                                            If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                                If mensajeCorreoElectronico_.Adjuntos.Count > 0 Then

                                                    Dim contador_ As Integer = 0

                                                    For Each adjunto_ As documento.Documento In mensajeCorreoElectronico_.Adjuntos

                                                        Dim documentosBandejaSalida_ = New OperacionesCatalogo

                                                        documentosBandejaSalida_.EspacioTrabajo = _operaciones.EspacioTrabajo

                                                        documentosBandejaSalida_ = _sistema.EnsamblaModulo("DocumentosBandejaSalida")

                                                        With documentosBandejaSalida_

                                                            .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                                                            .CampoPorNombre("i_Cve_CorreoElectronicoEnviado") = CorreoElectronicoEnviado_.ValorIndice

                                                            .CampoPorNombre("i_Cve_Documento") = adjunto_.Clave

                                                            .CampoPorNombre("i_Cve_Usuario") = _operaciones.EspacioTrabajo.MisCredenciales.ClaveUsuario

                                                            .CampoPorNombre("i_Cve_Estado") = 1

                                                            .CampoPorNombre("i_Cve_Estatus") = 1

                                                            .CampoPorNombre("i_Cve_DivisionMiEmpresa") = _operaciones.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

                                                            If Not .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                                                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1014)

                                                                Exit Sub

                                                            End If

                                                        End With

                                                        contador_ = contador_ + 1

                                                        If contador_ = mensajeCorreoElectronico_.Adjuntos.Count Then

                                                            Exit Sub

                                                        End If

                                                    Next

                                                Else

                                                    Exit Sub

                                                End If

                                            Else

                                                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1013)

                                                Exit Sub

                                            End If

                                        End With

                                    Else

                                        _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1007)

                                        Exit Sub

                                    End If

                                End With

                            Else

                                mensajeCorreoElectronico_.CantidadIntentos = mensajeCorreoElectronico_.CantidadIntentos + 1

                            End If

                            smtp_.Close()

                        End While

                        If mensajeCorreoElectronico_.CantidadIntentos = cantidadIntentosEnvio_ Then

                            mensajeCorreoElectronico_.EstatusMensajeSalida = IOperacionesCorreoElectronico.EstatusMensajeCorreoElectronicoSalida.NoEnviado

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1008)

                            Exit Sub

                        End If

                    End Using

                Else

                    mensajeCorreoElectronico_.EstatusMensajeSalida = IOperacionesCorreoElectronico.EstatusMensajeCorreoElectronicoSalida.NoEnviado

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1006)

                End If

            Catch ex As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1015)

            End Try

        End Sub

        'Métodos encargados de la lectura del correo electrónico

        Private Sub BuscarListadoUsuariosCuentaCorreo(ByVal cuentaCorreo_ As CuentaCorreo)

            Dim OperacionesUsuariosCarteraCliente_ = New OperacionesCatalogo

            OperacionesUsuariosCarteraCliente_.EspacioTrabajo = _operaciones.EspacioTrabajo

            OperacionesUsuariosCarteraCliente_.CantidadVisibleRegistros = 0

            OperacionesUsuariosCarteraCliente_.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

            OperacionesUsuariosCarteraCliente_ = _sistema.ConsultaModulo(OperacionesUsuariosCarteraCliente_.EspacioTrabajo,
                                                                "EncUsuariosCuentaClienteGeneral",
                                                                " and i_Cve_DivisionMiEmpresa = " & cuentaCorreo_.ClaveDivisionMiEmpresa & " and i_RevisaCorreo = 1")

            If _sistema.TieneResultados(OperacionesUsuariosCarteraCliente_) Then

                For Each fila_ As DataRow In OperacionesUsuariosCarteraCliente_.Vista.Tables(0).Rows

                    _coleccionCorreos.Add(_sistema.ValidarVacios(fila_.Item("Usuario").ToString, Organismo.VerificarTipoDatoDBNULL.Cadena),
                                          _sistema.ValidarVacios(fila_.Item("Clave").ToString, Organismo.VerificarTipoDatoDBNULL.Cadena))

                Next

            Else

            End If

        End Sub

        Private Sub BuscarDominiosNoPermitidos()

            _coleccionDominiosNoPermitidos.Add("kromaduanal.com", 1)

            _coleccionDominiosNoPermitidos.Add("kromlogistica.com", 2)

            _coleccionDominiosNoPermitidos.Add("grupobravant.onmicrosoft.com", 3)

        End Sub

        Public Sub LeerBuzonCorreo(ByVal ioperacionescatalogo_ As IOperacionesCatalogo,
                                   ByVal cuentaCorreo_ As CuentaCorreo) Implements IOperacionesCorreoElectronico.LeerBuzonCorreo

            Dim mensaje_ As String

            Try

                _estatus.SetOK()

                _operaciones = ioperacionescatalogo_

                _coleccionCorreos = New Dictionary(Of String, Integer)

                _coleccionDominiosNoPermitidos = New Dictionary(Of String, Integer)

                Try

                    BuscarListadoUsuariosCuentaCorreo(cuentaCorreo_:=cuentaCorreo_)

                Catch ex As Exception

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1020)

                    mensaje_ = _estatus.ErrorDescription & " " & cuentaCorreo_.Correo.ToString

                    EnviarCorreoEquipoSoporte(mensaje_:=EliminarCaracteresEnTexto(texto_:=mensaje_))

                    Return

                End Try

                BuscarDominiosNoPermitidos()

                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                    Dim imap_ = New Imap()

                    'Abrimos la conexión en una carpeta especifica
                    imap_.ConnectSSL(cuentaCorreo_.NombreServidor)

                    Try

                        imap_.Login(cuentaCorreo_.Correo, DescifrarContraseña(cuentaCorreo_.Contrasenia))

                    Catch ex As Exception

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1019)

                        mensaje_ = _estatus.ErrorDescription & " " & cuentaCorreo_.Correo.ToString

                        EnviarCorreoEquipoSoporte(mensaje_:=EliminarCaracteresEnTexto(texto_:=mensaje_))

                        Return

                    End Try

                    imap_.SelectInbox()

                    'Verficamos que tenga correos sin leer el buzón
                    Dim uids_ As List(Of Long) = imap_.Search(Flag.Unseen)


                    Try

                        If uids_.Count > 0 Then

                            _correosAProcesar = uids_.Count

                            'Por cada uno de los correos necesitamos hacer el procedimiento de guardado
                            For Each uid_ As Long In uids_

                                Dim email_ As IMail = New MailBuilder().CreateFromEml(imap_.GetMessageByUID(uid_))

                                imap_.MarkMessageUnseenByUID(uid_)

                                _correosProcesados += 1

                                _correoControl = New ControlCorreo64(cuentaCorreo_.ClaveDivisionMiEmpresa, cuentaCorreo_.ClaveCuentaCorreo, uid_, email_)

                                If _correoControl.BuscaItem(_correoControl.Correo) Then

                                    '_correoControl.AgregaItem(_correoControl.Correo)

                                    'Este metodo inserta y realiza el demas procedimiento del correo electronico
                                    ProcesarLecturaCorreo(email_:=email_,
                                                          cuentaCorreo_:=cuentaCorreo_,
                                                          uid_:=uid_)

                                    If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                        If _correoControl._existeElemento Then

                                            _correoControl.EliminarItem(_correoControl.Correo)

                                        End If

                                        imap_.MarkMessageSeenByUID(uid_)

                                        If Not cuentaCorreo_.PermiteMarcarCorreosComoLeidos Then

                                            imap_.MarkMessageUnseenByUID(uid_)

                                        End If

                                    End If

                                    _estatus.SetOK()

                                End If

                            Next

                        End If

                        imap_.Close(True)

                    Catch ex_ As Exception

                        imap_.Close(True)

                        Try
                            _correoControl.AgregaItem(_correoControl.Correo)

                        Catch ex As Exception

                        End Try

                        mensaje_ = "101 " & ex_.Message.ToString & " " & cuentaCorreo_.Correo.ToString

                        EnviarCorreoEquipoSoporte(mensaje_:=EliminarCaracteresEnTexto(texto_:=mensaje_))

                    End Try



                End If

            Catch ex_ As Exception

                mensaje_ = "102 " & ex_.Message.ToString & " " & cuentaCorreo_.Correo.ToString

                EnviarCorreoEquipoSoporte(mensaje_:=EliminarCaracteresEnTexto(texto_:=mensaje_))

            End Try

        End Sub

        Public Sub ReprocesaLecturaCorreo(ByVal ioperacionescatalogo_ As IOperacionesCatalogo,
                                 ByVal cuentaCorreo_ As CuentaCorreo,
                                 ByVal UIID_ As Long,
                                 ByVal CorreoElectonicoEntrada_ As Integer)

            Dim mensaje_ As String

            Try

                _estatus.SetOK()

                _operaciones = ioperacionescatalogo_

                _coleccionCorreos = New Dictionary(Of String, Integer)

                _coleccionDominiosNoPermitidos = New Dictionary(Of String, Integer)

                Try

                    BuscarListadoUsuariosCuentaCorreo(cuentaCorreo_:=cuentaCorreo_)

                Catch ex As Exception

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1020)

                    mensaje_ = _estatus.ErrorDescription & " " & cuentaCorreo_.Correo.ToString

                    EnviarCorreoEquipoSoporte(mensaje_:=EliminarCaracteresEnTexto(texto_:=mensaje_))

                    Return

                End Try

                BuscarDominiosNoPermitidos()

                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                    Dim imap_ = New Imap()

                    imap_.ConnectSSL(cuentaCorreo_.NombreServidor)

                    Try

                        imap_.Login(cuentaCorreo_.Correo, DescifrarContraseña(cuentaCorreo_.Contrasenia))

                    Catch ex As Exception

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1019)

                        mensaje_ = _estatus.ErrorDescription & " " & cuentaCorreo_.Correo.ToString

                        EnviarCorreoEquipoSoporte(mensaje_:=EliminarCaracteresEnTexto(texto_:=mensaje_))

                        Return

                    End Try

                    imap_.SelectInbox()

                    Try

                        Dim email_ As IMail = New MailBuilder().CreateFromEml(imap_.GetMessageByUID(UIID_))

                        ProcesarLecturaCorreoAttachments(email_:=email_,
                                                      cuentaCorreo_:=cuentaCorreo_,
                                                      uid_:=UIID_,
                                                      CorreoElectonicoEntrada_:=CorreoElectonicoEntrada_)
                    Catch ex As Exception

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1021)

                    End Try

                    imap_.Close(True)

                End If

            Catch ex_ As Exception

                mensaje_ = "103 " & ex_.Message.ToString & " " & cuentaCorreo_.Correo.ToString

                EnviarCorreoEquipoSoporte(mensaje_:=EliminarCaracteresEnTexto(texto_:=mensaje_))

            End Try

        End Sub

        Private Sub EnviarCorreoEquipoSoporte(Optional mensaje_ As String = Nothing)

            Dim operacionesCorreoElectronico_ = New OperacionesCorreoElectronico64(_operaciones.Clone)

            Dim mensajeCorreo_ = New MensajeCorreoElectronico

            mensajeCorreo_.Subject = "Error en servicio de correo electrónico KROM " & Date.Now

            Dim cuerpoCorreo_ = New StringBuilder

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

            cuerpoCorreo_.Append("                    <td align=""center"" style=""padding:25px 0px 15px 0px;color:#217d7e;font-size:20px;""><strong>Servicio de correo electrónico</strong></td>")

            cuerpoCorreo_.Append("                </tr>")

            cuerpoCorreo_.Append("                <tr bgcolor=""#ffffff"" align=""center"">")

            cuerpoCorreo_.Append("                    <td style=""font-size:16px;color:#272727;""><div style=""margin-bottom: 6px;"">El servicio de correo electrónico encargado de registrar los correos electrónicos generó un error, favor de revisar los logs.<strong>Log: " & mensaje_ & " </strong></div></td>")

            cuerpoCorreo_.Append("                </tr>")

            cuerpoCorreo_.Append("                <tr bgcolor=""#ffffff"" align=""center"">")

            cuerpoCorreo_.Append("                <tr bgcolor=""#ffffff"" align=""center"">")

            cuerpoCorreo_.Append("                    <td>")

            cuerpoCorreo_.Append("                        <div style=""border-bottom:1px dashed #cccccc;width: 90%;margin-bottom: 10px;""></div>")

            cuerpoCorreo_.Append("                    </td>")

            cuerpoCorreo_.Append("                </tr>")

            cuerpoCorreo_.Append("                <tr bgcolor=""#ffffff"" align=""center"">")

            cuerpoCorreo_.Append("                </tr>")

            cuerpoCorreo_.Append("                <tr bgcolor=""#ffffff"" align=""center"">")

            cuerpoCorreo_.Append("                    <td style=""font-size:12px;color:#272727;padding: 0px 55px;""><div style=""margin-bottom:  10px;"">El servicio de correo electrónico es fundamental para las operaciones de Krom Aduanal, es necesario dar prioridad a este tema.</div></td>")

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

            mensajeCorreo_.Body = cuerpoCorreo_.ToString

            Dim to_ As List(Of String) = New List(Of String)

            'to_.Add("monitoreoserviciosti@kromaduanal.com")

            to_.Add("christian.romero@kromaduanal.com")

            mensajeCorreo_.Too = to_

            operacionesCorreoElectronico_.EnviarCorreo(mensajeCorreo_,
                                                       IOperacionesCorreoElectronico.Prioridades.Normal,
                                                       IOperacionesCorreoElectronico.CantidadIntentosEnvio.Normal,
                                                       IOperacionesCorreoElectronico.Modalidades.Masivo)

            '_estatus.SetError(TagWatcher.ErrorTypes.C6_028_1004)

        End Sub

        Public Sub LeerBuzonCorreo(ByVal cuentaCorreo_ As CuentaCorreo) Implements IOperacionesCorreoElectronico.LeerBuzonCorreo

            _estatus.SetOK()

            LeerBuzonCorreo(ioperacionescatalogo_:=_operaciones.Clone,
                            cuentaCorreo_:=cuentaCorreo_)

        End Sub

        Private Sub ProcesarLecturaCorreo_(ByVal email_ As IMail,
                                          ByVal cuentaCorreo_ As CuentaCorreo,
                                          Optional claveCorreoElectronicoEntradaRelacionado_ As Integer = 0,
                                          Optional uid_ As Long = 0)

            'Creamos un objeto correo electronico para guardar el correo
            'En caso de que el correo electronico sea adjunto de otro entonces el campo opcional es 1
            Dim correoRecibido_ = New MensajeCorreoElectronico

            Try

                If Not VerificarCorreoExistente(correoRecepcionUID_:=cuentaCorreo_.Correo & "-" & correoRecibido_.FechaEnvioRecibido.ToString("yyyy") & correoRecibido_.FechaEnvioRecibido.ToString("mm") & correoRecibido_.FechaEnvioRecibido.ToString("dd") & "-" & uid_, claveCorreoElectronicoEntradaRelacionado_:=claveCorreoElectronicoEntradaRelacionado_) Then

                    'Verificamos que tenga Attacthments o que sea un correo tipo attachment
                    If ((Not email_.From(0).DomainPart = "kromaduanal.com" And
                       Not email_.From(0).DomainPart = "kromlogistica.com" And
                       Not email_.From(0).DomainPart = "grupobravant.onmicrosoft.com")) Then

                        If email_.Attachments.Count >= 1 Or claveCorreoElectronicoEntradaRelacionado_ >= 1 Then

                            Dim EsAttachment_ As Boolean

                            With correoRecibido_

                                .FechaEnvioRecibido = email_.Date

                                .ClaveBandeja = cuentaCorreo_.ClaveCuentaCorreo

                                .ClaveDivisionMiEmpresa = cuentaCorreo_.ClaveDivisionMiEmpresa

                                .Estado = 1

                                .Estatus = 1

                                .BCC = RetornarListaCorreos(email_.Bcc)

                                .CC = RetornarListaCorreos(email_.Cc)

                                .SenderFrom = email_.From(0).Address

                                .Subject = (email_.Subject.ToString.Replace("'", "")).Replace("""", "")

                                .Too = RetornarListaCorreos(email_.To)

                                .Body = (email_.GetBodyAsText().ToString.Replace("'", "")).Replace("""", "")

                                If Not claveCorreoElectronicoEntradaRelacionado_ = 0 Then

                                    .ClaveCorreoElectronicoRelacionado = claveCorreoElectronicoEntradaRelacionado_

                                    EsAttachment_ = 1

                                Else

                                    EsAttachment_ = 0

                                End If

                                .EsAttachment = EsAttachment_

                            End With

                            Dim CorreoElectronicoEntrada_ = New OperacionesCatalogo

                            CorreoElectronicoEntrada_.EspacioTrabajo = _operaciones.EspacioTrabajo

                            CorreoElectronicoEntrada_ = _sistema.EnsamblaModulo("CorreoElectronicoEntrada")

                            With CorreoElectronicoEntrada_

                                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                                .CampoPorNombre("t_Subject") = correoRecibido_.Subject

                                .CampoPorNombre("t_From") = correoRecibido_.SenderFrom

                                .CampoPorNombre("t_To") = RetornarListaToString(correoRecibido_.Too, ",", False)

                                .CampoPorNombre("t_CC") = RetornarListaToString(correoRecibido_.CC, ",", False)

                                .CampoPorNombre("t_BCC") = RetornarListaToString(correoRecibido_.BCC, ",", False)

                                .CampoPorNombre("i_Cve_BuzonEntrada") = correoRecibido_.ClaveBandeja

                                .CampoPorNombre("f_Recepcion") = correoRecibido_.FechaEnvioRecibido.ToString("dd/MM/yyyy H:mm:ss")

                                .CampoPorNombre("i_AñoRecepcion") = correoRecibido_.FechaEnvioRecibido.ToString("yyyy")

                                If uid_ > 0 Then

                                    .CampoPorNombre("i_UID") = uid_

                                    .CampoPorNombre("t_CorreoRecepcionUID") = cuentaCorreo_.Correo & "-" & correoRecibido_.FechaEnvioRecibido.ToString("yyyy") & correoRecibido_.FechaEnvioRecibido.ToString("mm") & correoRecibido_.FechaEnvioRecibido.ToString("dd") & "-" & uid_


                                End If

                                .CampoPorNombre("i_Attachment") = IIf(correoRecibido_.EsAttachment, 1, 0)

                                .CampoPorNombre("i_Cve_CorreoElectronicoEntradaRelacionado") = IIf(correoRecibido_.ClaveCorreoElectronicoRelacionado > 0, correoRecibido_.ClaveCorreoElectronicoRelacionado, Nothing)

                                .CampoPorNombre("i_Cve_Estado") = correoRecibido_.Estado

                                .CampoPorNombre("i_Cve_Estatus") = correoRecibido_.Estatus

                                .CampoPorNombre("i_Cve_DivisionMiEmpresa") = correoRecibido_.ClaveDivisionMiEmpresa

                                If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                    correoRecibido_.Clave = .ValorIndice

                                    Dim ExtCorreoElectronicoEntrada_ = New OperacionesCatalogo

                                    ExtCorreoElectronicoEntrada_.EspacioTrabajo = _operaciones.EspacioTrabajo

                                    ExtCorreoElectronicoEntrada_ = _sistema.EnsamblaModulo("ExtCorreoElectronicoEntrada")

                                    With ExtCorreoElectronicoEntrada_

                                        .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                                        .CampoPorNombre("i_Cve_CorreoElectronicoEntrada") = correoRecibido_.Clave

                                        .CampoPorNombre("t_Body") = correoRecibido_.Body

                                        .CampoPorNombre("i_Cve_Estado") = 1

                                        .CampoPorNombre("i_Cve_Estatus") = 1

                                        .CampoPorNombre("i_Cve_DivisionMiEmpresa") = correoRecibido_.ClaveDivisionMiEmpresa

                                        If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                            'En caso de que no sea un correo adjunto en otro correo entonces verificamos la 
                                            'inserción de la bandeja de entrada
                                            If Not EsAttachment_ Then

                                                'Este metodo inserta la bandeja de entrada con respecto a las reglas de la cartera
                                                VincularBandejaEntradaUsuarios(correoRecibido_:=correoRecibido_)

                                            End If

                                            'Este metodo procede a insertar cada uno de los adjuntos del correo

                                            ProcesarAttachments(correoElectronico_:=correoRecibido_,
                                                                cuentaCorreo_:=cuentaCorreo_,
                                                                email_:=email_)

                                            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                                BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_)

                                            End If

                                        Else

                                            _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1013)

                                            BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_)

                                            Exit Sub

                                        End If

                                    End With

                                Else

                                    _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1000)

                                    BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_)

                                    Exit Sub

                                End If

                            End With

                        End If

                    End If

                End If

            Catch ex_ As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1004)

                BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_,
                                                 observaciones_:=ex_.Message)

            End Try

        End Sub

        Private Sub ProcesarLecturaCorreo(ByVal email_ As IMail,
                                          ByVal cuentaCorreo_ As CuentaCorreo,
                                          Optional claveCorreoElectronicoEntradaRelacionado_ As Integer = 0,
                                          Optional uid_ As Long = 0)

            'Creamos un objeto correo electronico para guardar el correo
            'En caso de que el correo electronico sea adjunto de otro entonces el campo opcional es 1
            Dim correoRecibido_ = New MensajeCorreoElectronico




            Try

                CargaInformacionMensajeCorreoElectronicoRecibido(correoRecibido_:=correoRecibido_,
                                                                 cuentaCorreo_:=cuentaCorreo_,
                                                                 email_:=email_,
                                                                 uid_:=uid_,
                                                                 claveCorreoElectronicoEntradaRelacionado_:=claveCorreoElectronicoEntradaRelacionado_)

                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                    If Not _coleccionDominiosNoPermitidos.ContainsKey(correoRecibido_.SenderFromDomain) Then

                        'Verificamos que tenga Attacthments o que sea un correo tipo attachment
                        If email_.Attachments.Count >= 1 Or claveCorreoElectronicoEntradaRelacionado_ >= 1 Then

                            If VerificarAttachmentsValidos(email_:=email_) Then

                                If Not VerificaSiCorrreoExiste(uid_, cuentaCorreo_) Then

                                    InsertarCorreoElectronicoBaseDatos(correoRecibido_:=correoRecibido_,
                                                                       cuentaCorreo_:=cuentaCorreo_,
                                                                       uid_:=uid_)

                                    _correosEfectivamenteProcesados += 1

                                    If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                        'En caso de que no sea un correo adjunto en otro correo entonces verificamos la 
                                        'inserción de la bandeja de entrada
                                        If Not correoRecibido_.EsAttachment Then

                                            'Este metodo inserta la bandeja de entrada con respecto a las reglas de la cartera
                                            VincularBandejaEntradaUsuarios(correoRecibido_:=correoRecibido_)

                                        End If

                                        'Este metodo procede a insertar cada uno de los adjuntos del correo

                                        ProcesarAttachments(correoElectronico_:=correoRecibido_,
                                                            cuentaCorreo_:=cuentaCorreo_,
                                                            email_:=email_)

                                        If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                            BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_)

                                        End If

                                    End If

                                End If

                            End If

                        End If

                    End If

                End If

            Catch ex_ As Exception

                Try
                    _correoControl.AgregaItem(_correoControl.Correo)

                Catch ex As Exception

                End Try

                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1004)

                BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_,
                                                 observaciones_:=ex_.Message)

            End Try

        End Sub

        Private Sub ProcesarLecturaCorreoAttachments(ByVal email_ As IMail,
                                          ByVal cuentaCorreo_ As CuentaCorreo,
                                          ByVal uid_ As Long,
                                          ByVal CorreoElectonicoEntrada_ As Integer)

            'Creamos un objeto correo electronico para guardar el correo
            Dim correoRecibido_ = New MensajeCorreoElectronico

            Try

                CargaInformacionMensajeCorreoElectronicoRecibido(correoRecibido_:=correoRecibido_,
                                                                 cuentaCorreo_:=cuentaCorreo_,
                                                                 email_:=email_,
                                                                 uid_:=uid_)


                correoRecibido_.Clave = CorreoElectonicoEntrada_

                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                    If Not _coleccionDominiosNoPermitidos.ContainsKey(correoRecibido_.SenderFromDomain) Then

                        'Verificamos que tenga Attacthments o que sea un correo tipo attachment
                        If email_.Attachments.Count >= 1 Then

                            If VerificarAttachmentsValidos(email_:=email_) Then

                                'Este metodo procede a insertar cada uno de los adjuntos del correo

                                ProcesarAttachmentsCorreo(correoElectronico_:=correoRecibido_,
                                                        cuentaCorreo_:=cuentaCorreo_,
                                                        email_:=email_)

                                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                                    BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_)

                                End If

                            End If

                        End If

                    End If

                End If

            Catch ex_ As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1004)

                BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_,
                                                 observaciones_:=ex_.Message)

            End Try

        End Sub

        Private Sub CargaInformacionMensajeCorreoElectronicoRecibido(ByRef correoRecibido_ As MensajeCorreoElectronico,
                                                                     ByVal cuentaCorreo_ As CuentaCorreo,
                                                                     ByVal email_ As IMail,
                                                                     ByVal uid_ As Integer,
                                                                     Optional claveCorreoElectronicoEntradaRelacionado_ As Integer = 0)
            Try

                With correoRecibido_

                    .FechaEnvioRecibido = IIf(IsNothing(email_.Date), DateTime.Now.ToString("dd/MM/yyyy H:mm:ss:mmm"), email_.Date)

                    .ClaveBandeja = cuentaCorreo_.ClaveCuentaCorreo

                    .ClaveDivisionMiEmpresa = cuentaCorreo_.ClaveDivisionMiEmpresa

                    .Estado = 1

                    .Estatus = 1

                    .BCC = RetornarListaCorreos(email_.Bcc)

                    .CC = RetornarListaCorreos(email_.Cc)

                    .SenderFrom = EliminarCaracteresEnTexto(texto_:=email_.From(0).Address)

                    .SenderFromDomain = EliminarCaracteresEnTexto(texto_:=email_.From(0).DomainPart)

                    'Subject viene vacio
                    .Subject = EliminarCaracteresEnTexto(texto_:=email_.Subject.ToString)

                    .Too = RetornarListaCorreos(email_.To)

                    .Body = EliminarCaracteresEnTexto(texto_:=email_.GetBodyAsText().ToString)

                    If Not claveCorreoElectronicoEntradaRelacionado_ = 0 Then

                        .ClaveCorreoElectronicoRelacionado = claveCorreoElectronicoEntradaRelacionado_

                        .EsAttachment = True

                    Else

                        .EsAttachment = False

                    End If

                    .CorreoRecepcionUID = cuentaCorreo_.Correo & "-" & correoRecibido_.FechaEnvioRecibido.ToString("yyyy") & correoRecibido_.FechaEnvioRecibido.ToString("mm") & correoRecibido_.FechaEnvioRecibido.ToString("dd") & "-" & uid_

                End With

            Catch ex_ As Exception

                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1004)

                BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_,
                                                 observaciones_:=ex_.Message)

            End Try

        End Sub

        Private Sub InsertarCorreoElectronicoBaseDatos(ByRef correoRecibido_ As MensajeCorreoElectronico,
                                                       ByVal cuentaCorreo_ As CuentaCorreo,
                                                       Optional uid_ As Long = 0)

            Dim CorreoElectronicoEntrada_ = New OperacionesCatalogo

            CorreoElectronicoEntrada_.EspacioTrabajo = _operaciones.EspacioTrabajo

            CorreoElectronicoEntrada_ = _sistema.EnsamblaModulo("CorreoElectronicoEntrada")

            With CorreoElectronicoEntrada_

                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                .CampoPorNombre("t_Subject") = correoRecibido_.Subject

                .CampoPorNombre("t_From") = correoRecibido_.SenderFrom

                .CampoPorNombre("t_To") = RetornarListaToString(correoRecibido_.Too, ",", False)

                .CampoPorNombre("t_CC") = RetornarListaToString(correoRecibido_.CC, ",", False)

                .CampoPorNombre("t_BCC") = RetornarListaToString(correoRecibido_.BCC, ",", False)

                .CampoPorNombre("i_Cve_BuzonEntrada") = correoRecibido_.ClaveBandeja

                .CampoPorNombre("f_Recepcion") = correoRecibido_.FechaEnvioRecibido.ToString("dd/MM/yyyy HH:mm:ss.fff")

                .CampoPorNombre("i_AñoRecepcion") = correoRecibido_.FechaEnvioRecibido.ToString("yyyy")

                .CampoPorNombre("i_UID") = uid_

                .CampoPorNombre("t_CorreoRecepcionUID") = correoRecibido_.CorreoRecepcionUID

                .CampoPorNombre("i_Attachment") = IIf(correoRecibido_.EsAttachment, 1, 0)

                .CampoPorNombre("i_Cve_CorreoElectronicoEntradaRelacionado") = IIf(correoRecibido_.ClaveCorreoElectronicoRelacionado > 0, correoRecibido_.ClaveCorreoElectronicoRelacionado, Nothing)

                .CampoPorNombre("i_Cve_Estado") = correoRecibido_.Estado

                .CampoPorNombre("i_Cve_Estatus") = correoRecibido_.Estatus

                .CampoPorNombre("i_Cve_DivisionMiEmpresa") = correoRecibido_.ClaveDivisionMiEmpresa

                If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                    correoRecibido_.Clave = .ValorIndice

                    Dim ExtCorreoElectronicoEntrada_ = New OperacionesCatalogo

                    ExtCorreoElectronicoEntrada_.EspacioTrabajo = _operaciones.EspacioTrabajo

                    ExtCorreoElectronicoEntrada_ = _sistema.EnsamblaModulo("ExtCorreoElectronicoEntrada")

                    With ExtCorreoElectronicoEntrada_

                        .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                        .CampoPorNombre("i_Cve_CorreoElectronicoEntrada") = correoRecibido_.Clave

                        .CampoPorNombre("t_Body") = correoRecibido_.Body

                        .CampoPorNombre("i_Cve_Estado") = 1

                        .CampoPorNombre("i_Cve_Estatus") = 1

                        .CampoPorNombre("i_Cve_DivisionMiEmpresa") = correoRecibido_.ClaveDivisionMiEmpresa

                        If Not .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                            _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1013)

                            BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_)

                        End If

                    End With

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1000)

                    BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_)

                End If

            End With

        End Sub

        Private Sub VincularBandejaEntradaUsuarios(ByVal correoRecibido_ As MensajeCorreoElectronico)

            Dim correoInsertadoTo_ As Boolean = False

            Dim correoInsertadoCC_ As Boolean = False

            Dim destinatarios_ As List(Of String) = New List(Of String)

            For Each fila_ As String In correoRecibido_.Too

                fila_ = fila_.ToLower

                If _coleccionCorreos.ContainsKey(fila_) Then

                    If Not destinatarios_.Contains(fila_) Then

                        Dim OperacionesBandejaEntrada_ = New OperacionesCatalogo

                        OperacionesBandejaEntrada_.EspacioTrabajo = _operaciones.EspacioTrabajo

                        OperacionesBandejaEntrada_ = _sistema.EnsamblaModulo("BandejaEntrada")

                        With OperacionesBandejaEntrada_

                            .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                            .CampoPorNombre("i_Cve_UsuariosCuentaCliente") = _coleccionCorreos.Item(fila_)

                            .CampoPorNombre("i_Cve_CorreoElectronicoEntrada") = correoRecibido_.Clave

                            .CampoPorNombre("i_Cve_Estado") = correoRecibido_.Estado

                            .CampoPorNombre("i_Cve_Estatus") = correoRecibido_.Estatus

                            .CampoPorNombre("i_Cve_DivisionMiEmpresa") = correoRecibido_.ClaveDivisionMiEmpresa

                            If Not .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1001)

                                BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_)

                            Else

                                correoInsertadoTo_ = True

                            End If

                        End With

                        destinatarios_.Add(fila_)

                    End If

                End If

            Next

            For Each fila_ As String In correoRecibido_.CC

                fila_ = fila_.ToLower

                If _coleccionCorreos.ContainsKey(fila_) Then

                    If Not destinatarios_.Contains(fila_) Then

                        Dim OperacionesBandejaEntrada_ = New OperacionesCatalogo

                        OperacionesBandejaEntrada_.EspacioTrabajo = _operaciones.EspacioTrabajo

                        OperacionesBandejaEntrada_ = _sistema.EnsamblaModulo("BandejaEntrada")

                        With OperacionesBandejaEntrada_

                            .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                            .CampoPorNombre("i_Cve_UsuariosCuentaCliente") = _coleccionCorreos.Item(fila_)

                            .CampoPorNombre("i_Cve_CorreoElectronicoEntrada") = correoRecibido_.Clave

                            .CampoPorNombre("i_Cve_Estado") = correoRecibido_.Estado

                            .CampoPorNombre("i_Cve_Estatus") = correoRecibido_.Estatus

                            .CampoPorNombre("i_Cve_DivisionMiEmpresa") = correoRecibido_.ClaveDivisionMiEmpresa

                            If Not .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1001)

                                BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_)


                            Else

                                correoInsertadoCC_ = True

                            End If

                        End With

                        destinatarios_.Add(fila_)

                    End If

                End If

            Next

            If Not correoInsertadoTo_ And Not correoInsertadoCC_ Then

                Dim OperacionesBandejaEntrada_ = New OperacionesCatalogo

                OperacionesBandejaEntrada_.EspacioTrabajo = _operaciones.EspacioTrabajo

                OperacionesBandejaEntrada_ = _sistema.EnsamblaModulo("BandejaEntrada")

                With OperacionesBandejaEntrada_

                    .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                    .CampoPorNombre("i_Cve_CorreoElectronicoEntrada") = correoRecibido_.Clave

                    .CampoPorNombre("i_Cve_Estado") = correoRecibido_.Estado

                    .CampoPorNombre("i_Cve_Estatus") = correoRecibido_.Estatus

                    .CampoPorNombre("i_Cve_DivisionMiEmpresa") = correoRecibido_.ClaveDivisionMiEmpresa

                    If Not .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                        _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1001)

                        BitacoraCorreoElectronicoEntrada(correoElectronico_:=correoRecibido_)

                    End If

                End With

            End If

        End Sub

        Private Sub ProcesarAttachments(ByVal correoElectronico_ As MensajeCorreoElectronico,
                                        ByVal cuentaCorreo_ As CuentaCorreo,
                                        ByVal email_ As IMail)

            'Por cada uno de los adjunto se procede al analisis para ver que hacer con el
            For Each attachment_ As MimeData In email_.Attachments

                'No vamos a guardar los attachment que tengan un contentid ya que eso indicaria que son las imagenes de que vienen 
                'en su firma
                'If attachment_.ContentId Is Nothing Then
                If Not attachment_.ContentType.MimeType.Name = "image" Then

                    'Si el nombre del documento es vacio quiere decir que es un correo adjunto dentro de otro
                    If Not attachment_.ContentType.MimeType.Name = "message" Then

                        Dim operacionesDocumento_ = New OperacionesDocumento64(_operaciones.Clone)

                        operacionesDocumento_.ProcesarDocumentoCorreoElectronico(correoElectronico_:=correoElectronico_,
                                                                                 attachment_:=attachment_)

                        If operacionesDocumento_.Estatus.Status = TagWatcher.TypeStatus.Errors Then

                            BitacoraCorreoElectronicoEntradaDocumentos(correoElectronico_:=correoElectronico_,
                                                                       tagWatcher_:=operacionesDocumento_.Estatus,
                                                                       observaciones_:=attachment_.FileName)

                        End If

                    Else

                        'Este metodo se encarga de insertar un correo electronico y en este caso le mandamos el correo electronico padre
                        ProcesarLecturaCorreo(email_:=DirectCast(attachment_, MimeRfc822).Message,
                                              cuentaCorreo_:=cuentaCorreo_,
                                              claveCorreoElectronicoEntradaRelacionado_:=correoElectronico_.Clave)

                    End If

                End If

            Next

        End Sub

        Private Sub ProcesarAttachmentsCorreo(ByVal correoElectronico_ As MensajeCorreoElectronico,
                                        ByVal cuentaCorreo_ As CuentaCorreo,
                                        ByVal email_ As IMail)

            _archivosReprocesado = New Dictionary(Of String, String)

            Dim contador_ As Integer = 0

            For Each attachment_ As MimeData In email_.Attachments

                If Not attachment_.ContentType.MimeType.Name = "image" Then

                    'Si el nombre del documento es vacio quiere decir que es un correo adjunto dentro de otro
                    If Not attachment_.ContentType.MimeType.Name = "message" Then

                        Dim operacionesDocumento_ = New OperacionesDocumento64(_operaciones.Clone)

                        operacionesDocumento_.ProcesarDocumentoCorreoElectronico(correoElectronico_:=correoElectronico_,
                                                                                 attachment_:=attachment_)

                        If operacionesDocumento_.Estatus.Status = TagWatcher.TypeStatus.Errors Then

                            BitacoraCorreoElectronicoEntradaDocumentos(correoElectronico_:=correoElectronico_,
                                                                       tagWatcher_:=operacionesDocumento_.Estatus,
                                                                       observaciones_:=attachment_.FileName)

                            _archivosReprocesado.Add(attachment_.FileName, operacionesDocumento_.Estatus.ErrorDescription.ToString)

                        End If

                        If Not IsNothing(operacionesDocumento_._archivosReprocesado) Then

                            For Each msj_ In operacionesDocumento_._archivosReprocesado

                                _archivosReprocesado.Add(msj_.Key, msj_.Value)

                            Next

                        End If

                        contador_ += operacionesDocumento_._contador

                    End If

                End If

            Next

            _archivosReprocesado.Add("Total Archivos", contador_)

        End Sub

        Private Sub BitacoraCorreoElectronicoEntrada(ByVal correoElectronico_ As MensajeCorreoElectronico,
                                                     Optional observaciones_ As String = Nothing)

            Dim BitacoraCorreoElectronicoEntrada_ = New OperacionesCatalogo

            BitacoraCorreoElectronicoEntrada_.EspacioTrabajo = _operaciones.EspacioTrabajo

            BitacoraCorreoElectronicoEntrada_ = _sistema.EnsamblaModulo("BitacoraCorreoElectronicoEntrada")

            With BitacoraCorreoElectronicoEntrada_

                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                If correoElectronico_.Clave > 0 Then

                    .CampoPorNombre("i_Cve_CorreoElectronicoEntrada") = correoElectronico_.Clave

                End If

                .CampoPorNombre("i_Cve_BuzonEntrada") = correoElectronico_.ClaveBandeja

                .CampoPorNombre("t_Subject") = correoElectronico_.Subject

                .CampoPorNombre("t_From") = correoElectronico_.SenderFrom

                .CampoPorNombre("t_EstatusTagWatcher") = _estatus.Status.ToString

                If _estatus.Status = TagWatcher.TypeStatus.Errors Then

                    .CampoPorNombre("t_CodigoTagWatcher") = _estatus.Errors.ToString

                    .CampoPorNombre("t_Observaciones") = "TagWatcher: " & _estatus.ErrorDescription & "Observaciones: " & EliminarCaracteresEnTexto(texto_:=observaciones_)

                End If

                .CampoPorNombre("f_Recepcion") = correoElectronico_.FechaEnvioRecibido.ToString("dd/MM/yyyy H:mm:ss")

                .CampoPorNombre("f_Registro") = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")

                .CampoPorNombre("i_Cve_Estado") = 1

                .CampoPorNombre("i_Cve_Estatus") = 1

                .CampoPorNombre("i_Cve_DivisionMiEmpresa") = correoElectronico_.ClaveDivisionMiEmpresa

                If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                    'Implementar algo

                End If

            End With

        End Sub

        Public Sub BitacoraCorreoElectronicoEntradaDocumentos(ByVal correoElectronico_ As MensajeCorreoElectronico,
                                                               ByVal tagWatcher_ As TagWatcher,
                                                               Optional observaciones_ As String = Nothing)

            Dim BitacoraCorreoElectronicoEntrada_ = New OperacionesCatalogo

            BitacoraCorreoElectronicoEntrada_.EspacioTrabajo = _operaciones.EspacioTrabajo

            BitacoraCorreoElectronicoEntrada_ = _sistema.EnsamblaModulo("BitacoraCorreoElectronicoEntrada")

            With BitacoraCorreoElectronicoEntrada_

                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                If correoElectronico_.Clave > 0 Then

                    .CampoPorNombre("i_Cve_CorreoElectronicoEntrada") = correoElectronico_.Clave

                End If

                .CampoPorNombre("i_Cve_BuzonEntrada") = correoElectronico_.ClaveBandeja

                .CampoPorNombre("t_Subject") = correoElectronico_.Subject

                .CampoPorNombre("t_From") = correoElectronico_.SenderFrom

                .CampoPorNombre("t_EstatusTagWatcher") = tagWatcher_.Status.ToString

                If tagWatcher_.Status = TagWatcher.TypeStatus.Errors Then

                    .CampoPorNombre("t_CodigoTagWatcher") = tagWatcher_.Errors.ToString

                    .CampoPorNombre("t_Observaciones") = " TagWatcher: " & tagWatcher_.ErrorDescription & " Observaciones: " & EliminarCaracteresEnTexto(texto_:=observaciones_)

                End If

                .CampoPorNombre("f_Recepcion") = correoElectronico_.FechaEnvioRecibido.ToString("dd/MM/yyyy H:mm:ss")

                .CampoPorNombre("f_Registro") = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss")

                .CampoPorNombre("i_Cve_Estado") = 1

                .CampoPorNombre("i_Cve_Estatus") = 1

                .CampoPorNombre("i_Cve_DivisionMiEmpresa") = correoElectronico_.ClaveDivisionMiEmpresa

                If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                    'Implementar algo

                End If

            End With

        End Sub

        Public Sub CambiarEstatusCorreoBandejaEntradaCatalogo(ByVal gsCatalogo_ As GsCatalogo,
                                                              Optional nombreCampoEntornoCorreoBandejaEntrada_ As String = "Clave BandejaEntrada",
                                                              Optional nombreCampoEntornoUID_ As String = "UID",
                                                              Optional nombreCampoEntornoClaveBuzonEntrada As String = "Clave BuzonEntrada",
                                                              Optional estatusCorreoBandejaEntrada_ As IOperacionesCorreoElectronico.EstatusMensajeBandejaEntrada = IOperacionesCorreoElectronico.EstatusMensajeBandejaEntrada.Pendiente,
                                                              Optional operacionesCatalogo_ As IOperacionesCatalogo = Nothing)

            _estatus.SetOK()

            Dim generoError_ As Boolean = False

            Dim sbCadena_ = New System.Text.StringBuilder()

            Dim rtbMensaje_ As New Windows.Forms.RichTextBox

            sbCadena_.Append("{\rtf1\ansi")

            sbCadena_.Append("{\colortbl ; \red255\green60\blue51; \red0\green0\blue255;}")

            Dim respuesta_ As String = Nothing

            For Each fila_ As Windows.Forms.DataGridViewRow In gsCatalogo_.DgvCatalogo.Rows

                If fila_.Selected Then

                    _estatus.SetOK()

                    CambiarEstatusCorreoBandejaEntrada(claveBandejaEntrada_:=fila_.Cells(nombreCampoEntornoCorreoBandejaEntrada_).Value,
                                                       UID_:=fila_.Cells(nombreCampoEntornoUID_).Value,
                                                       claveBuzonCorreoEntrada_:=fila_.Cells(nombreCampoEntornoClaveBuzonEntrada).Value,
                                                       estatusCorreoBandejaEntrada_:=estatusCorreoBandejaEntrada_,
                                                       operacionesCatalogo_:=operacionesCatalogo_)

                    If _estatus.Status = TagWatcher.TypeStatus.Errors Then

                        sbCadena_.Append("\par ID: " & fila_.Cells(nombreCampoEntornoCorreoBandejaEntrada_).Value &
                                         "\par Estatus: " & IIf(_estatus.Status = TagWatcher.TypeStatus.Errors, " \b\cf1 " & _estatus.ErrorDescription & " \cf0\b0", "\b Estatus cambiado. \b0") &
                                         "\par ******************************************************")

                        generoError_ = True

                    End If

                End If

            Next

            sbCadena_.Append("}")

            rtbMensaje_.Rtf = sbCadena_.ToString()

            If generoError_ Then

                _sistema.GsDialogo(Nothing, rtbMensaje_, Componentes.SistemaBase.GsDialogo.TipoDialogo.AvisoGrande)

            End If

        End Sub

        Public Sub CambiarEstatusCorreoBandejaEntrada(ByVal claveBandejaEntrada_ As Integer,
                                                      ByVal UID_ As Integer,
                                                      ByVal claveBuzonCorreoEntrada_ As Integer,
                                                      Optional estatusCorreoBandejaEntrada_ As IOperacionesCorreoElectronico.EstatusMensajeBandejaEntrada = IOperacionesCorreoElectronico.EstatusMensajeBandejaEntrada.Pendiente,
                                                      Optional operacionesCatalogo_ As IOperacionesCatalogo = Nothing,
                                                      Optional actualizarBaseDatos_ As Boolean = True)

            _estatus.SetOK()

            If Not operacionesCatalogo_ Is Nothing Then

                _operaciones = operacionesCatalogo_

            End If

            Dim buzonEntrada_ = ObtenerInformacionBuzonEntradaPorClave(claveCuentaCorreo_:=claveBuzonCorreoEntrada_)

            If Not IsNothing(buzonEntrada_) Then

                Dim imap_ = New Imap()

                imap_.ConnectSSL(buzonEntrada_.NombreServidor)

                imap_.Login(buzonEntrada_.Correo, DescifrarContraseña(buzonEntrada_.Contrasenia))

                imap_.SelectInbox()

                Select Case estatusCorreoBandejaEntrada_

                    Case IOperacionesCorreoElectronico.EstatusMensajeBandejaEntrada.Eliminado

                        imap_.MarkMessageSeenByUID(UID_)

                    Case IOperacionesCorreoElectronico.EstatusMensajeBandejaEntrada.Leido

                        imap_.MarkMessageSeenByUID(UID_)

                    Case IOperacionesCorreoElectronico.EstatusMensajeBandejaEntrada.Pendiente

                        imap_.MarkMessageUnseenByUID(UID_)

                    Case Else


                End Select

                imap_.Close(True)

                If actualizarBaseDatos_ Then

                    CambiarEstatusCorreoBandejaEntradaBaseDatos(claveBandejaEntrada_:=claveBandejaEntrada_,
                                                                estatusCorreoBandejaEntrada_:=estatusCorreoBandejaEntrada_)

                End If

            Else


            End If

        End Sub

        Private Sub CambiarEstatusCorreoBandejaEntradaBaseDatos(ByVal claveBandejaEntrada_ As Integer,
                                                                Optional estatusCorreoBandejaEntrada_ As IOperacionesCorreoElectronico.EstatusMensajeBandejaEntrada = IOperacionesCorreoElectronico.EstatusMensajeBandejaEntrada.Pendiente)

            Dim OCExtensionDocumento_ = New OperacionesCatalogo

            OCExtensionDocumento_ = _sistema.EnsamblaModulo("BandejaEntrada")

            With OCExtensionDocumento_

                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                .CampoPorNombre("i_Cve_Estatus") = estatusCorreoBandejaEntrada_

                .EditaCampoPorNombre("i_Cve_CorreoElectronicoEntrada").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_Cve_UsuariosCuentaCliente").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_Cve_Estado").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_Cve_DivisionMiEmpresa").PuedeModificar = ICaracteristica.TiposRigorDatos.No

            End With

            If Not OCExtensionDocumento_.Modificar(claveBandejaEntrada_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1016)

            End If

        End Sub

        Public Sub InsertarBitacoraLecturaBuzonEntrada(ByVal cuentaCorreo_ As CuentaCorreo)

            _estatus.SetOK()

            Dim ioBitacoraLectura_ = New OperacionesCatalogo

            ioBitacoraLectura_.EspacioTrabajo = _operaciones.EspacioTrabajo

            ioBitacoraLectura_ = _sistema.EnsamblaModulo("BitacoraLecturaBuzonEntrada")

            With _bitacoraLecturaBuzonEntrada

                .ClaveBuzonEntrada = cuentaCorreo_.ClaveCuentaCorreo

                .FechaInicioActualizacion = DateTime.Now

                .Estado = cuentaCorreo_.Estado

                .Estatus = cuentaCorreo_.Estatus

                .ClaveDivisionMiEmpresa = cuentaCorreo_.ClaveDivisionMiEmpresa

            End With

            With ioBitacoraLectura_

                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                .CampoPorNombre("i_Cve_BuzonEntrada") = _bitacoraLecturaBuzonEntrada.ClaveBuzonEntrada

                .CampoPorNombre("f_FechaInicioActualizacion") = _bitacoraLecturaBuzonEntrada.FechaInicioActualizacion.ToString("dd/MM/yyyy H:mm:ss")

                .CampoPorNombre("i_Cve_Estado") = _bitacoraLecturaBuzonEntrada.Estado

                .CampoPorNombre("i_Cve_Estatus") = _bitacoraLecturaBuzonEntrada.Estatus

                .CampoPorNombre("i_Cve_DivisionMiEmpresa") = _bitacoraLecturaBuzonEntrada.ClaveDivisionMiEmpresa

                If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                    _bitacoraLecturaBuzonEntrada.ClaveBitacoraLecturaBuzonEntrada = ioBitacoraLectura_.ValorIndice

                Else

                    _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1017)

                End If

            End With

        End Sub

        Public Sub ActualizarBitacoraLecturaBuzonEntrada(ByVal claveBitacora_ As Integer)

            _estatus.SetOK()

            Dim ioBitacoraLectura_ = New OperacionesCatalogo

            ioBitacoraLectura_ = _sistema.EnsamblaModulo("BitacoraLecturaBuzonEntrada")

            With _bitacoraLecturaBuzonEntrada

                .ClaveBitacoraLecturaBuzonEntrada = claveBitacora_

                .FechaFinActualizacion = DateTime.Now

            End With

            With ioBitacoraLectura_

                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                .CampoPorNombre("f_FechaFinActualizacion") = _bitacoraLecturaBuzonEntrada.FechaFinActualizacion.ToString("dd/MM/yyyy H:mm:ss")

            End With

            If Not ioBitacoraLectura_.Modificar(claveBitacora_) = IOperacionesCatalogo.EstadoOperacion.COk Then

                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1018)

            End If

        End Sub

#End Region

#Region "Funciones"

        Public Function CifrarContraseña(ByVal contraseña_ As String) As String

            Dim metodo_ As SymmetricAlgorithm = New RijndaelManaged

            Dim cif_ As ICifrado = New Cifrado256

            Return cif_.CifraCadena(contraseña_,
                                    metodo_,
                                    _sistema.ManifiestoGlobal(Configuracion.DatosGlobalesSistema.LlaveCifrado))

        End Function

        Public Function DescifrarContraseña(ByVal contraseña_ As String) As String

            Dim cifrado_ As ICifrado = New Cifrado256

            Dim metodo_ As SymmetricAlgorithm

            metodo_ = New RijndaelManaged

            Return cifrado_.DescifraCadena(contraseña_,
                                           metodo_,
                                           _sistema.ManifiestoGlobal(Configuracion.DatosGlobalesSistema.LlaveCifrado))

        End Function

        Private Function RetornarListaToString(ByVal lista_ As List(Of String),
                                               ByVal separador_ As String,
                                               Optional comillas_ As Boolean = False)

            Dim cadena_ As String = Nothing

            If Not lista_ Is Nothing Then

                For Each registro_ As String In lista_

                    registro_ = EliminarCaracteresEnTexto(texto_:=registro_)

                    If comillas_ Then

                        If cadena_ Is Nothing Then

                            cadena_ = "'" & registro_.ToString & "'"

                        Else

                            cadena_ = cadena_ & "" & separador_ & "'" & registro_.ToString & "'"

                        End If

                    Else
                        Try

                            If cadena_ Is Nothing Then

                                cadena_ = registro_.ToString

                            Else

                                cadena_ = cadena_ & "" & separador_ & "" & registro_.ToString

                            End If

                        Catch ex As Exception

                            cadena_ = cadena_ & "" & separador_ & "" & registro_.ToString

                        End Try

                    End If

                Next

            End If

            Return cadena_

        End Function

        Private Function RetornarListaCorreos(ByVal lista_ As IList(Of Headers.MailAddress))

            Dim listaCorreos_ As List(Of String) = New List(Of String)

            For counter_ As Integer = 0 To lista_.Count - 1

                For counter2_ As Integer = 0 To lista_(counter_).GetMailboxes.Count - 1

                    listaCorreos_.Add(lista_(counter_).GetMailboxes.Item(counter2_).Address)

                Next counter2_

            Next counter_

            Return listaCorreos_

        End Function

        Private Function ObtenerInformacionBuzonEntradaPorClave(ByVal claveCuentaCorreo_ As Integer) As CuentaCorreo

            Dim buzonEntrada_ As CuentaCorreo = New CuentaCorreo

            Dim iOperacionesBuzonesEntrada = New OperacionesCatalogo

            iOperacionesBuzonesEntrada = _operaciones.Clone

            iOperacionesBuzonesEntrada = _sistema.ConsultaModulo(iOperacionesBuzonesEntrada.EspacioTrabajo,
                                                      "BuzonEntrada",
                                                       " and i_Cve_BuzonEntrada = " & claveCuentaCorreo_)

            If _sistema.TieneResultados(iOperacionesBuzonesEntrada) Then

                For Each fila_ As DataRow In iOperacionesBuzonesEntrada.Vista.Tables(0).Rows

                    buzonEntrada_.ClaveCuentaCorreo = _sistema.ValidarVacios(fila_.Item("Clave").ToString, Organismo.VerificarTipoDatoDBNULL.Cadena)

                    buzonEntrada_.Correo = _sistema.ValidarVacios(fila_.Item("Correo").ToString, Organismo.VerificarTipoDatoDBNULL.Cadena)

                    buzonEntrada_.Contrasenia = _sistema.ValidarVacios(fila_.Item("Contrasenia").ToString, Organismo.VerificarTipoDatoDBNULL.Cadena)

                    buzonEntrada_.NombreServidor = _sistema.ValidarVacios(fila_.Item("Nombre Servidor").ToString, Organismo.VerificarTipoDatoDBNULL.Cadena)

                    buzonEntrada_.ClaveDivisionMiEmpresa = _sistema.ValidarVacios(fila_.Item("Clave DivisionMiEmpresa").ToString, Organismo.VerificarTipoDatoDBNULL.Cadena)

                Next

            Else

                Return Nothing

            End If

            Return buzonEntrada_

        End Function

        Private Function VerificarCorreoExistente(ByVal correoRecepcionUID_ As String,
                                                  Optional claveCorreoElectronicoEntradaRelacionado_ As Integer = 0) As Boolean

            If claveCorreoElectronicoEntradaRelacionado_ = 0 Then

                Dim dataTable_ As DataTable = Nothing

                Dim consulta_ As String = Nothing

                consulta_ = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED " &
                            "select i_Cve_CorreoElectronicoEntrada " &
                             "from Bit028CorreoElectronicoEntrada as b with(nolock) " &
                             "where i_Cve_Estado=1 and t_CorreoRecepcionUID='" & correoRecepcionUID_ & "'"

                '"where contains(t_CorreoRecepcionUID,'" & correoRecepcionUID_ & "')"

                _estatus = _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(consulta_)

                If Not _estatus.Status = TagWatcher.TypeStatus.Errors Then

                    dataTable_ = DirectCast(_estatus.ObjectReturned, DataTable)

                    If dataTable_.Rows.Count > 0 Then

                        Return True

                    End If

                End If

            End If

            Return False

        End Function

        Private Function VerificaSiCorrreoExiste(ByVal uid_ As Long, ByVal cuentaCorreo2_ As CuentaCorreo)

            Dim bandera_ As Boolean = False

            Dim cuentaCorreo_ = New CuentaCorreo

            Dim correoExistente_ = New OperacionesCatalogo

            Dim clausulaLibre_ As String = " and i_Cve_Estado = 1  and  i_Cve_BuzonEntrada = " & cuentaCorreo2_.ClaveCuentaCorreo & " and i_Cve_DivisionMiEmpresa = " & cuentaCorreo2_.ClaveDivisionMiEmpresa & " and i_UID = " & uid_

            correoExistente_ = _operaciones.Clone

            correoExistente_ = _sistema.ConsultaModulo(correoExistente_.EspacioTrabajo,
                                                        "CorreoElectronicoCaracteristicasVinculaciones",
                                                        clausulaLibre_)

            If _sistema.TieneResultados(correoExistente_) Then

                For Each fila_ As DataRow In correoExistente_.Vista.Tables(0).Rows

                    If (fila_.Item("i_Cve_Estatus").ToString = 0) Then

                        Dim claveCorreoElectonicoEntrada_ = fila_.Item("i_Cve_CorreoElectronicoEntrada").ToString

                        _estatus.SetOK()

                        Dim ioperacionesCorreoEntrada_ As IOperacionesCatalogo = _sistema.EnsamblaModulo("CorreoElectronicoCaracteristicasVinculaciones")

                        Dim lista_ As New List(Of String)

                        lista_.Add("i_Cve_Estado")

                        ioperacionesCorreoEntrada_.PreparaCatalogo()

                        With ioperacionesCorreoEntrada_

                            .CampoPorNombre("i_Cve_Estado") = 0

                            If .Modificar(claveCorreoElectonicoEntrada_) = IOperacionesCatalogo.EstadoOperacion.CError Then

                                _estatus.SetError(TagWatcher.ErrorTypes.C6_028_1018)

                            End If

                            bandera_ = False

                        End With

                    Else

                        bandera_ = True

                    End If

                Next

            End If

            Return bandera_

        End Function

        Private Function VerificaCorreoIgual(ByVal cuentaCorreo_ As CuentaCorreo, ByVal correoRecibido_ As MensajeCorreoElectronico)

            Dim bandera_ As Boolean

            Dim clausulaLibre_ As String

            Try
                If Not correoRecibido_.EsAttachment Then

                    Dim correoExistente_ = New OperacionesCatalogo

                    clausulaLibre_ = " and i_Cve_Estado = 1  and  i_Cve_BuzonEntrada = " & cuentaCorreo_.ClaveCuentaCorreo &
                                " and i_Cve_DivisionMiEmpresa = " & cuentaCorreo_.ClaveDivisionMiEmpresa &
                                " and t_subject= '" & correoRecibido_.Subject &
                                "' and t_to = '" & RetornarListaToString(correoRecibido_.Too, ",", False) &
                                "' and t_From= '" & correoRecibido_.SenderFrom &
                                "' and f_Recepcion = '" & correoRecibido_.FechaEnvioRecibido.ToString("dd/MM/yyyy HH:mm:ss.fff") &
                                "' and i_Attachment=0"

                    correoExistente_ = _operaciones.Clone

                    correoExistente_ = _sistema.ConsultaModulo(correoExistente_.EspacioTrabajo,
                                                                "CorreoElectronicoEntrada",
                                                                clausulaLibre_)

                    If _sistema.TieneResultados(correoExistente_) Then

                        bandera_ = True

                    Else

                        bandera_ = False

                    End If

                Else

                    bandera_ = False

                End If

            Catch ex As Exception

                bandera_ = False

            End Try

            Return bandera_

        End Function

        Private Function VerificarAttachmentsValidos(ByVal email_ As IMail)

            Dim extension_ As String

            Dim extensionesPermitidos_ = New List(Of String) From {".zip", ".rar", ".csv", ".doc", ".docx", ".pdf", ".xls", ".xlsx", ".xml", ".xlsb"}

            For Each attachment_ As MimeData In email_.Attachments

                extension_ = Path.GetExtension(attachment_.FileName)

                If Not (extension_ Is Nothing) Then

                    If extensionesPermitidos_.Contains(extension_.ToLower) Then

                        Return True

                    End If

                    If attachment_.ContentType.MimeType.Name = "message" Then

                        Return True

                    End If

                End If

            Next

            Return False

        End Function

        Private Function EliminarCaracteresEnTexto(ByVal texto_ As String) As String

            If Not IsNothing(texto_) Then

                texto_ = (texto_.Replace("'", "")).Replace("""", "")

            Else

                texto_ = ""

            End If

            Return texto_

        End Function

#End Region

    End Class

End Namespace
