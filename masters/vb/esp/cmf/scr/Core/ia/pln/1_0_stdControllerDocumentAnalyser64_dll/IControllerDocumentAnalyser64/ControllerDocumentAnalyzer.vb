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
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As TagWatcher)
            Throw New NotImplementedException()
        End Set
    End Property

    Function DocumentAnalyzer(Of T)(ByVal document_ As List(Of MemoryStream)) As TagWatcher Implements IControllerDocumentAnalyzer.DocumentAnalyzer

        Dim imagenByte_ As New List(Of Byte()) '= _organismo.ConvertirPDFaByte(document_(0))

        _tipoTransformer = Transformer.GPT

        Select Case _tipoTransformer

            Case Transformer.GPT

                _gpt = New ControllerChatGPT

                _gpt.DocumentAnalyzer(Of Object)(imagenByte_)

        End Select


    End Function

    Public Function GetResponse(operationNumber As ObjectId) As TagWatcher Implements IControllerDocumentAnalyzer.GetResponse
        Throw New NotImplementedException()
    End Function

End Class
