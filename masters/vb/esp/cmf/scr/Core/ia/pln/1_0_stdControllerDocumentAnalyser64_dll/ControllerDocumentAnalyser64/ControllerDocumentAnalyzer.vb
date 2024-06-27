Imports Wma.Exceptions
Imports MongoDB.Bson
Imports System.IO
Imports System.Drawing
Imports Syn.Utils
Imports System.Collections.Generic


Public Class ControllerDocumentAnalyzer
    Implements IControllerDocumentAnalyzer

    Private _organismo As Organismo

    Private _gpt As IControllerChatGPT

    Private _tipoTransformer As Transformer

    Enum Transformer As Int16
        GPT = 1
        Otro = 99
    End Enum

    'Private Property Document As T

#Region "Constructores"

    Sub New()

        _organismo = New Organismo

        Inicializa(Transformer.GPT)

    End Sub

    Sub Inicializa(Optional transformer_ As Transformer = Transformer.GPT)

        _tipoTransformer = transformer_

    End Sub

#End Region

    Public Property Status As TagWatcher Implements IControllerDocumentAnalyzer.Status

    Function DocumentAnalyzer(Of T)(ByVal document_ As List(Of MemoryStream), Optional ByVal documetoCargado As IControllerChatGPT.DocumentoCargado = IControllerChatGPT.DocumentoCargado.BL) As TagWatcher Implements IControllerDocumentAnalyzer.DocumentAnalyzer


        Dim imagenByte_ As List(Of Byte()) = _organismo.ConvertirPDFaByte(document_(0))

        Dim base64String As String = Convert.ToBase64String(imagenByte_(0))

        _tipoTransformer = Transformer.GPT

        Select Case _tipoTransformer

            Case Transformer.GPT

                _gpt = New ControllerChatGPT(documetoCargado)

                Status = _gpt.DocumentAnalyzer(Of Object)(imagenByte_)

        End Select

        Return Status

    End Function

    Public Function GetResponse(operationNumber As ObjectId) As TagWatcher Implements IControllerDocumentAnalyzer.GetResponse
        Throw New NotImplementedException()
    End Function

End Class
