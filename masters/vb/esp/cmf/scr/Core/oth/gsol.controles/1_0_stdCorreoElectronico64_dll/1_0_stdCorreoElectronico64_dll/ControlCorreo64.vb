Imports System.IO
Imports System.Net
Imports Newtonsoft.Json
Imports gsol.correoelectronico
Imports Limilabs.Mail
Imports System.Text

Namespace gsol.Controladores
    Public Class ControlCorreo64


#Region "Atributos"

        Private _path As String = "C:\logs\CorreoElectronico\LocalStorage\"

        Private _archivo As String

        Private _ruta As String

        Private _json As String

        Public _existeElemento As Boolean

        Private _numeroProcesos As Integer = 3

        Private _correo As MensajeCorreoElectronico



#End Region

#Region "Constructores"

        Sub New(ByVal divisionMiEmpresa_ As Integer, ByVal buzonCorreo_ As Integer, ByVal uid_ As Integer, ByVal email_ As IMail)

            Correo = New MensajeCorreoElectronico

            CargaInformacionMensajeCorreoElectronicoRecibido(Correo, buzonCorreo_, divisionMiEmpresa_, email_, uid_)

            _existeElemento = False

            _archivo = "ServicioCorreo_" & divisionMiEmpresa_ & "_" & buzonCorreo_ & ".json"

            _ruta = _path & _archivo

        End Sub

#End Region

#Region "Metodos"

        Public Sub EliminarItem(ByVal elemento_ As MensajeCorreoElectronico)

            'buscamos el objeto correo en la coleccion, se elimina si se encuentra.

            'Fetch the JSON string from URL.
            ServicePointManager.Expect100Continue = True
            ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)

            Try

                If File.Exists(_ruta) Then

                    Dim cliente_ As WebClient = New WebClient()

                    cliente_.Encoding = Encoding.UTF8

                    _json = cliente_.DownloadString(_ruta)

                    '_json = (New WebClient).DownloadString(_ruta)

                    'Deserialize the JSON string.
                    Dim customers As List(Of MensajeCorreoElectronico) = JsonConvert.DeserializeObject(Of List(Of MensajeCorreoElectronico))(_json)

                    Dim customersNuevo As List(Of MensajeCorreoElectronico) = New List(Of MensajeCorreoElectronico)

                    For Each valor As MensajeCorreoElectronico In customers

                        'si existe el valor, aumentamos su numero de procesamiento
                        If Not (valor.ClaveDivisionMiEmpresa = elemento_.ClaveDivisionMiEmpresa And valor.ClaveBandeja = elemento_.ClaveBandeja And valor.CorreoRecepcionUID = elemento_.CorreoRecepcionUID) Then

                            customersNuevo.Add(valor)

                        End If

                    Next

                    'Serialize the JSON string.
                    _json = JsonConvert.SerializeObject(customersNuevo, Formatting.Indented)

                    GuardarLog(_json)


                End If


            Catch ex As Exception

                'Console.WriteLine("104 >>> CC >>> " & ex.Message.ToString)

            End Try

        End Sub

        Public Sub AgregaItem(ByVal consulta_ As MensajeCorreoElectronico)

            'Agrega un correo a la coleccion, si ya existe actualiza su numero de preocesos

            Dim customersNuevo As List(Of MensajeCorreoElectronico) = New List(Of MensajeCorreoElectronico)

            Dim banderaExiste As Boolean = False

            Try

                If File.Exists(_ruta) Then

                    Dim cliente_ As WebClient = New WebClient()

                    cliente_.Encoding = Encoding.UTF8

                    _json = cliente_.DownloadString(_ruta)

                    '_json = (New WebClient).DownloadString(_ruta)

                    Dim customers As List(Of MensajeCorreoElectronico) = JsonConvert.DeserializeObject(Of List(Of MensajeCorreoElectronico))(_json)

                    For Each valor As MensajeCorreoElectronico In customers

                        'si existe el valor, aumentamos su numero de procesamiento
                        If valor.ClaveDivisionMiEmpresa = consulta_.ClaveDivisionMiEmpresa And valor.ClaveBandeja = consulta_.ClaveBandeja And valor.CorreoRecepcionUID = consulta_.CorreoRecepcionUID Then

                            banderaExiste = True

                            If valor.CantidadIntentos < _numeroProcesos Then

                                valor.CantidadIntentos += 1

                            End If

                        End If

                        customersNuevo.Add(valor)

                    Next


                End If

                If Not banderaExiste Then

                    customersNuevo.Add(consulta_)

                End If

                'Serialize the JSON string.
                _json = JsonConvert.SerializeObject(customersNuevo, Formatting.Indented)

                GuardarLog(_json)

            Catch ex As Exception

                Console.WriteLine("103 >>> CC >>> " & ex.Message.ToString)

            End Try

        End Sub

        Private Sub GuardarLog(ByVal json_ As String)

            If Not Directory.Exists(_path) Then

                My.Computer.FileSystem.CreateDirectory(_path)

            End If

            'Borramos el archivo y lo escribimos nuevamente
            If File.Exists(_ruta) Then
                ' Create a file to write to.
                File.Delete(_ruta)

            End If

            Using sw As StreamWriter = File.AppendText(_ruta)

                sw.WriteLine(json_)

            End Using

        End Sub

#End Region

#Region "Funciones"

        Public Function BuscaItem(ByVal consulta_ As MensajeCorreoElectronico) As Boolean

            'Consulta en la coleccion, si ya se ha procesado el correo, nos devuelve true si no ha sido procesado o el numero es menor al establecido, actualiza variable existeElemento_

            Dim procesarCorreo_ As Boolean = True

            Try

                If File.Exists(_ruta) Then

                    Dim cliente_ As WebClient = New WebClient()

                    cliente_.Encoding = Encoding.UTF8

                    _json = cliente_.DownloadString(_ruta)

                    '_json = (New WebClient).DownloadString(_ruta)

                    Dim customers As List(Of MensajeCorreoElectronico) = JsonConvert.DeserializeObject(Of List(Of MensajeCorreoElectronico))(_json)

                    If customers.Count > 0 Then

                        For Each valor As MensajeCorreoElectronico In customers

                            If valor.ClaveDivisionMiEmpresa = consulta_.ClaveDivisionMiEmpresa And valor.ClaveBandeja = consulta_.ClaveBandeja And valor.CorreoRecepcionUID = consulta_.CorreoRecepcionUID Then

                                _existeElemento = True

                                If Not (valor.CantidadIntentos < _numeroProcesos) Then

                                    procesarCorreo_ = False

                                End If

                            End If

                        Next

                    End If

                End If

            Catch ex As Exception

                Console.WriteLine("102 >>> CC >>> " & ex.Message.ToString)

            End Try

            Return procesarCorreo_

        End Function

        Private Sub CargaInformacionMensajeCorreoElectronicoRecibido(ByRef correoRecibido_ As MensajeCorreoElectronico,
                                                                     ByVal cuentaCorreo_ As Integer,
                                                                     ByVal divisionMiEmpresa_ As Integer,
                                                                     ByVal email_ As IMail,
                                                                     ByVal uid_ As Integer,
                                                                     Optional claveCorreoElectronicoEntradaRelacionado_ As Integer = 0)
            Try

                With correoRecibido_

                    .FechaEnvioRecibido = IIf(IsNothing(email_.Date), DateTime.Now.ToString("dd/MM/yyyy H:mm:ss"), email_.Date)

                    .ClaveBandeja = cuentaCorreo_

                    .ClaveDivisionMiEmpresa = divisionMiEmpresa_

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

                    .EstatusMensajeSalida = email_.Attachments.Count

                    If Not claveCorreoElectronicoEntradaRelacionado_ = 0 Then

                        .ClaveCorreoElectronicoRelacionado = claveCorreoElectronicoEntradaRelacionado_

                        .EsAttachment = True

                    Else

                        .EsAttachment = False

                    End If

                    .CorreoRecepcionUID = uid_

                    .CantidadIntentos = 1

                End With

            Catch ex_ As Exception

                Console.WriteLine("101 >>> CC >>> " & ex_.Message.ToString)

            End Try

        End Sub

        Private Function EliminarCaracteresEnTexto(ByVal texto_ As String) As String

            If Not IsNothing(texto_) Then

                texto_ = (texto_.Replace("'", "")).Replace("""", "")

            Else

                texto_ = ""

            End If

            Return texto_

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

#End Region

#Region "Propiedades"

        Public Property Correo As MensajeCorreoElectronico

            Get

                Return _correo

            End Get

            Set(value As MensajeCorreoElectronico)

                _correo = value

            End Set

        End Property

#End Region


    End Class

End Namespace
