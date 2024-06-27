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
Imports Newtonsoft.Json.Linq
Imports Syn.CustomBrokers.Controllers

Public Class ControllerChatGPT
    Implements IControllerChatGPT

    Private ReadOnly _apiKey As String

    Private _prompt As String

    Private ReadOnly _client As RestClient

    Public Property Status As TagWatcher Implements IControllerChatGPT.Status

    Sub New(Optional documentoCargado_ As IControllerChatGPT.DocumentoCargado = IControllerChatGPT.DocumentoCargado.BL)

        _apiKey = "sk-apikey"

        '_prompt = "Evaluando tus conocimientos sobre comercio exterior mexicano, ley aduanera vigente y Anexo 22 mexicano, basándote en los datos proporcionados, determina si la mercancía es peligrosa. 
        '           En caso de ser mercancía a peligrosa, Proporcionando las siguientes respuestas específicas: Numero de referencia: poner el número de referencia 'NumeroReferencia', Peligrosa: si o no, 
        '           Clave IMO: poner que clave IMO es, Confiabilidad: 0-100% y no pongas acentos en las palabras Limita las respuestas a lo solicitado y proporcióname una respuesta solida evita explicaciones adicionales"
        Select Case documentoCargado_
            Case IControllerChatGPT.DocumentoCargado.BL
                _prompt = "Actúa como un experto en comercio exterior mexicano. Analiza el conocimiento de embarque (BL) proporcionado y responde en formato JSON puro sin incluir caracteres de nueva línea (\n) ni bloques de código como ```json, solo analiza las imágenes enviadas junto a este prompt. Responde solo con el contenido en la siguiente estructura específica:" &
                           vbCrLf & "{" &
                           vbCrLf & "    _operacion: {" &
                           vbCrLf & "        _materialPeligroso:""debe decir true si es si o false si es no""" &
                           vbCrLf & "    }," &
                           vbCrLf & "    _importacion: {" &
                           vbCrLf & "        _guia :{" &
                           vbCrLf & "            _listaGuias : [{" &
                           vbCrLf & "                _guia : ""numero de guia, pon especial atención aquí de no cambiar ni saltar caracteres, repito pon especial atención aquí por favor""," &
                           vbCrLf & "                _transportista: ""nombre del transportista""," &
                           vbCrLf & "                _tipoCarga : ""el tipo de la carga""," &
                           vbCrLf & "                _pais: ""pais de origen""," &
                           vbCrLf & "                perBruto : ""peso bruto, el peso total, solo numero sin separacion de miles y respetar el punto decimal""," &
                           vbCrLf & "                _unidadMedida : ""unidade de medida""," &
                           vbCrLf & "                _salidaOrigen: ""Fecha Salida de origen o de abordaje y similares, en formato dd/mm/yyyy, si no la encuentras omite este nodo""," &
                           vbCrLf & "                _descripcionMercancia: ""Descripcion de la mercancia""," &
                           vbCrLf & "                _consignatario : ""poner la razon social o nombre del consignatario, solo el consignatario o consignee, no el nombre del buque""" &
                           vbCrLf & "            }]" &
                           vbCrLf & "        }" &
                           vbCrLf & "    }" &
                           vbCrLf & "    _confiabilidad: ""0-100%""," &
                           vbCrLf & "}"

            Case IControllerChatGPT.DocumentoCargado.FacturaImportacion
                _prompt = "Actúa como un experto en comercio exterior mexicano. Analiza la factura comercial de importación proporcionada y responde en formato JSON puro sin incluir caracteres de nueva línea (\n) ni bloques de código como ```json, solo analiza las imágenes enviadas junto a este prompt. Responde solo con el contenido en la siguiente estructura específica:" &
                           vbCrLf & "{" &
                           vbCrLf & "    generales: {" &
                           vbCrLf & "        numeroFactura:""numero o folio de la factura""," &
                           vbCrLf & "        fechaFactura:""fecha de emision de la factura, en formato dd/mm/yyyy, si no la encuentras omite este nodo""," &
                           vbCrLf & "        razonSocial:""razon social o nombre del cliente""," &
                           vbCrLf & "        taxId:""Tax ID del cliente""," &
                           vbCrLf & "        domicilio:""domicilio del cliente""," &
                           vbCrLf & "        valorFactura:""valor total de la factura""," &
                           vbCrLf & "        pesoTotal:""peso total de la factura""" &
                           vbCrLf & "    }," &
                           vbCrLf & "    proveedor: {" &
                           vbCrLf & "        razonSocial:""razon social o nombre del proveedor""," &
                           vbCrLf & "        taxId:""Tax ID del proveedor o emisor de la factura""," &
                           vbCrLf & "        domicilio:""domicilio del proveedor""" &
                           vbCrLf & "    }," &
                           vbCrLf & "    partidas:  [{ aqui se tiene una lista de las partidas de la factura con los siguientes datos" &
                           vbCrLf & "        numeroPartida : ""numero de la partida""," &
                           vbCrLf & "        numeroParte: ""numero de parte""," &
                           vbCrLf & "        valorPartida : ""valor de la partida""," &
                           vbCrLf & "        moneda: ""tipo de moneda de la partida""," &
                           vbCrLf & "        pesoNeto : ""peso neto de la partida""," &
                           vbCrLf & "        precioUnitario : ""precio unitario en la partida""," &
                           vbCrLf & "        cantidad: ""cantidad en la partida""," &
                           vbCrLf & "        unidadMedida: ""unidade de medida""," &
                           vbCrLf & "        descripcion : ""descripcion de la partida""" &
                           vbCrLf & "    }]" &
                           vbCrLf & "    _confiabilidad: ""0-100%""," &
                           vbCrLf & "}"

            Case IControllerChatGPT.DocumentoCargado.FacturaExportacion
                _prompt = "Actúa como un experto en comercio exterior mexicano. Analiza el conocimiento de embarque (BL) proporcionado y responde en formato JSON puro sin incluir caracteres de nueva línea (\n) ni bloques de código como ```json, solo analiza las imágenes enviadas junto a este prompt. Responde solo con el contenido en la siguiente estructura específica:" &
                           vbCrLf & "{" &
                           vbCrLf & "    _operacion: {" &
                           vbCrLf & "        _materialPeligroso:""debe decir true si es si o false si es no""" &
                           vbCrLf & "    }," &
                           vbCrLf & "    _importacion: {" &
                           vbCrLf & "        _guia :{" &
                           vbCrLf & "            _listaGuias : [{" &
                           vbCrLf & "                _guia : ""numero de guia, pon especial atención aquí de no cambiar ni saltar caracteres, repito pon especial atención aquí por favor""," &
                           vbCrLf & "                _transportista: ""nombre del transportista""," &
                           vbCrLf & "                _tipoCarga : ""el tipo de la carga""," &
                           vbCrLf & "                _pais: ""pais de origen""," &
                           vbCrLf & "                perBruto : ""peso bruto, el peso total, solo numero sin separacion de miles y respetar el punto decimal""," &
                           vbCrLf & "                _unidadMedida : ""unidade de medida""," &
                           vbCrLf & "                _salidaOrigen: ""Fecha Salida de origen o de abordaje y similares, en formato dd/mm/yyyy, si no la encuentras omite este nodo""," &
                           vbCrLf & "                _descripcionMercancia: ""Descripcion de la mercancia""," &
                           vbCrLf & "                _consignatario : ""poner la razon social o nombre del consignatario, solo el consignatario o consignee, no el nombre del buque""" &
                           vbCrLf & "            }]" &
                           vbCrLf & "        }" &
                           vbCrLf & "    }" &
                           vbCrLf & "    _confiabilidad: ""0-100%""," &
                           vbCrLf & "}"

        End Select

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

        Dim imageMessages As New List(Of Object)()

        request_.AddHeader("Authorization", "Bearer " & _apiKey)

        request_.AddHeader("Content-Type", "application/json")

        For Each doc_ In document_
            imageMessages.Add(New With {
            .type = "image_url",
            .image_url = New With {
                .url = "data:image/jpeg;base64," & Convert.ToBase64String(doc_),
                .detail = "high"
            }
        })
        Next

        Dim body = New With {
            .model = "gpt-4o",
            .messages = New Object() {
                New With {
                    .role = "system",
                    .content = _prompt
                },
                 New With {
                    .role = "user",
                    .content = imageMessages
                }
            },
            .max_tokens = 600,
            .temperature = 0.2
        }

        request_.AddJsonBody(JsonConvert.SerializeObject(body))

        Dim response_ = _client.Execute(request_)

        If response_.IsSuccessful Then

            Dim jsonResponse As JObject = JObject.Parse(response_.Content)

            Dim rawContent As String = jsonResponse("choices")(0)("message")("content").ToString()

            rawContent = rawContent.Replace("```json", "").Replace("```", "").Trim()

            _Status.ObjectReturned = JsonConvert.DeserializeObject(Of Referencia)(rawContent)

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
