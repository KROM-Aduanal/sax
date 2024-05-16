Imports gsol.krom
Imports Wma.Exceptions
Imports MongoDB.Bson
Imports System.Net.Mime.MediaTypeNames
Imports System.Drawing
Imports System.IO
Imports System.Net
Imports Newtonsoft.Json
Imports RestSharp
Imports System.Runtime.InteropServices

Public Class ControllerChatGPT
    Implements IControllerChatGPT

    Private ReadOnly _apiKey As String

    Private _prompt As String

    Private ReadOnly _client As RestClient

    Public Property Status As TagWatcher Implements IControllerChatGPT.Status

    Sub New()

        _apiKey = "key"

        '_prompt = "Evaluando tus conocimientos sobre comercio exterior mexicano, ley aduanera vigente y Anexo 22 mexicano, basándote en los datos proporcionados, determina si la mercancía es peligrosa. 
        '           En caso de ser mercancía a peligrosa, Proporcionando las siguientes respuestas específicas: Numero de referencia: poner el número de referencia 'NumeroReferencia', Peligrosa: si o no, 
        '           Clave IMO: poner que clave IMO es, Confiabilidad: 0-100% y no pongas acentos en las palabras Limita las respuestas a lo solicitado y proporcióname una respuesta solida evita explicaciones adicionales"

        _prompt = "De que color el el cielo en LATAM?"

        _Status = New TagWatcher

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        ' Desactivar la validación de certificados SSL (solo para desarrollo)
        ServicePointManager.ServerCertificateValidationCallback =
            Function(sender, certificate, chain, sslPolicyErrors) True

        ' Base URL for the ChatGPT API
        _client = New RestClient("https://api.openai.com/v1/")

    End Sub

    Public Function DocumentAnalyzer(Of T)(document_ As List(Of Byte())) As TagWatcher Implements IControllerChatGPT.DocumentAnalyzer

        Dim request_ = New RestRequest("chat/completions", Method.Post)

        request_.AddHeader("Authorization", "Bearer " & _apiKey)

        request_.AddHeader("Content-Type", "application/json")

        Dim body = New With {
            .model = "gpt-4-turbo",
            .messages = New Object() {
                New With {
                    .role = "user",
                    .content = New Object() {
                        New With {.type = "text", .text = _prompt},
                        New With {.type = "image_url", .image_url = New With {.url = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg"}},
                        New With {.type = "image_url", .image_url = New With {.url = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg"}}
                    }
                }
            },
            .max_tokens = 300
        }

        request_.AddJsonBody(JsonConvert.SerializeObject(body))

        Dim response_ = _client.Execute(request_)

        If response_.IsSuccessful Then

            _Status.ObjectReturned = response_.Content

            _Status.SetOK()

            Return _Status

        Else

            _Status.ErrorDescription = "Error: " & response_.StatusCode.ToString() & " - " & response_.ErrorMessage

            _Status.SetError()

            Return _Status

        End If



    End Function

    Public Function GetResponse(operationNumber_ As ObjectId) As TagWatcher Implements IControllerChatGPT.GetResponse

        Throw New NotImplementedException()

    End Function

End Class


Public Class ChatGptApiClient
    Private ReadOnly _client As RestClient


    Private _apikey As String

    Public Sub New(ByVal apikey_ As String)
        ' Configurar TLS 1.2
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        ' Desactivar la validación de certificados SSL (solo para desarrollo)
        ServicePointManager.ServerCertificateValidationCallback =
            Function(sender, certificate, chain, sslPolicyErrors) True

        _apikey = apikey_

        ' Base URL for the ChatGPT API
        _client = New RestClient("https://api.openai.com/v1/")
    End Sub

    Public Function AskToChatGPT(message As String,
                                 Optional temperature As Double = 0.7) As String

        Dim request = New RestRequest("chat/completions", Method.Post)
        request.AddHeader("Authorization", "Bearer " & _apikey)
        request.AddHeader("Content-Type", "application/json")

        ' Contexto inicial para el modelo
        Dim initialContext As String = "Soy un experto en comercio exterior mexicano."

        ' Aquí construimos el cuerpo de la solicitud según los parámetros requeridos por la API de OpenAI
        'gpt-4.0-turbo
        'gpt-3.5-turbo
        request.AddJsonBody(New With {
            .model = "gpt-3.5-turbo",
            .temperature = temperature,
            .messages = New Object() {
                New With {.role = "system", .content = initialContext},
                New With {.role = "user", .content = message}
            }
        })

        ' Enviar la solicitud y esperar la respuesta
        Dim response = _client.Execute(request)
        If response.IsSuccessful Then
            Return response.Content
        Else
            Return "Error: " & response.StatusCode.ToString() & " - " & response.ErrorMessage
        End If
    End Function



End Class
