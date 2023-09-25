Imports System.IO

Namespace gsol.monitoreo

    Public Class BitacoraExcepciones
        Implements IBitacoras


#Region "Atributos"

        Private _rutabitacora As String

        Private _nombrearchivo As String

        'Número único
        Private _numerounicoguid As String

#End Region

#Region "Constructores"
        Sub New()
            _rutabitacora = Nothing

            _nombrearchivo = "log"

            _numerounicoguid = Nothing

        End Sub

#End Region

#Region "Propiedades"

        Public Property DirectorioBitacoras As String _
            Implements IBitacoras.DirectorioBitacoras
            Get
                Return _rutabitacora
            End Get
            Set(ByVal value As String)
                _rutabitacora = value
            End Set
        End Property


        Public Property NombreArchivo As String _
            Implements IBitacoras.NombreArchivo
            Get
                Return _nombrearchivo
            End Get
            Set(ByVal value As String)
                _nombrearchivo = value
            End Set
        End Property


#End Region

#Region "Metodos"

        Public Sub DocumentaError(ByVal modulo_ As String, _
                                  ByVal numeroerror_ As String, _
                                  ByVal excepcion_ As System.Exception) _
                                 Implements IBitacoras.DocumentaError

            Dim escritor_ As StreamWriter
            Try
                'Número único
                _numerounicoguid = System.Guid.NewGuid.ToString()

                Dim FilePath As String = _rutabitacora & _
                                        "\Error-" & _
                                        _nombrearchivo & _
                                        " [" & _numerounicoguid & "]" & _
                                        Now.ToString.Replace(":", "").Replace(".", "").Replace("/", "-") & ".log"

                'Crea el directorio si no existe 
                If Not Directory.Exists(_rutabitacora) Then
                    Directory.CreateDirectory(_rutabitacora)
                End If

                escritor_ = File.AppendText(FilePath)
                escritor_.WriteLine("{" & modulo_ & "} " & _
                numeroerror_ & "-" & _
                excepcion_.StackTrace & " Mensaje:" & _
                excepcion_.Message & ".........." & _
                excepcion_.Source & ", " & _
                excepcion_.TargetSite.ToString & "[" & Now.ToString & "]")

                escritor_.Flush()
                escritor_.Close()

            Catch excepcion__ As Exception

                DocumentaError(modulo_, numeroerror_, excepcion__)

            End Try
        End Sub


        Public Sub DocumentaObservacion(ByVal modulo_ As String, _
                                        ByVal mensaje_ As String) _
                                        Implements IBitacoras.DocumentaObservacion
            Try
                Dim escritor_ As StreamWriter

                'Número único
                _numerounicoguid = System.Guid.NewGuid.ToString()


                Dim FilePath As String = _rutabitacora & _
                                        "\Observación-" & _
                                        _nombrearchivo & _
                                        " [" & _numerounicoguid & "]" & _
                                        Now.ToString.Replace(":", "").Replace(".", "").Replace("/", "-") & ".log"

                escritor_ = File.AppendText(FilePath)
                escritor_.WriteLine("{" & modulo_ & "} " & _
                mensaje_)

                escritor_.Flush()
                escritor_.Close()

            Catch ex As Exception

                DocumentaObservacion(modulo_, mensaje_)

            End Try
        End Sub


        Private Sub EnviaCorreo(ByVal remitente_ As String, _
                         ByVal destinatario_ As String, _
                         ByVal asunto_ As String, _
                         ByVal mensaje_ As String)
            Dim SmtpObj As New System.Net.Mail.SmtpClient

            Dim MailNachricht As New System.Net.Mail.MailMessage()
            SmtpObj.Credentials = New System.Net.NetworkCredential("correo@rkzego.com", _
                                                                   "contraseña")
            SmtpObj.Port = 9025

            SmtpObj.Host = "server"
            With MailNachricht
                .From = New System.Net.Mail.MailAddress(remitente_)
                .BodyEncoding = System.Text.Encoding.UTF8
                .To.Add(destinatario_)
                .Subject = asunto_
                .IsBodyHtml = False
                .Body = mensaje_
            End With

            MailNachricht.Attachments.Clear()
            Try

                SmtpObj.Send(MailNachricht)

            Catch ex As Exception

            End Try
        End Sub

#End Region



    End Class

End Namespace

