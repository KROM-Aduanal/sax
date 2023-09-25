Imports System.IO
Imports System.Reflection
Imports System.Web.Script.Serialization
Imports MongoDB.Bson
Imports Rec.Globals
Imports Rec.Globals.Controllers
Imports Wma.Exceptions

Public Class FileUploadHandler
    Implements IHttpHandler

#Region "Propiedades"

    Private CDocumentos As New ControladorDocumento

    Private Code As Integer = 200

    Private Response As Object = Nothing

    Private Message As String = Nothing

    'Creating a Synchronous HTTP Handler
    Public ReadOnly Property IsReusable As Boolean Implements IHttpHandler.IsReusable

        Get

            Return False

        End Get

    End Property

#End Region

#Region "Constructor"

    Public Sub New()
        MyBase.New()

        AddHandler CDocumentos.OnUploadDocumentSuccess, AddressOf UploadDocumentSuccess

        AddHandler CDocumentos.OnUploadDocumentError, AddressOf UploadDocumentError

        AddHandler CDocumentos.OnDownloadDocumentSuccess, AddressOf DownloadDocumentSuccess

        AddHandler CDocumentos.OnDownloadDocumentError, AddressOf DownloadDocumentError

    End Sub

#End Region

#Region "Eventos"

    Private Sub UploadDocumentSuccess(sender As TagWatcher, e As EventArgs)

        Code = 200

        Response = sender.ObjectReturned

    End Sub

    Private Sub UploadDocumentError(sender As TagWatcher, e As EventArgs)

        Code = 400

        Message = sender.ErrorDescription

    End Sub

    Private Sub DownloadDocumentSuccess(sender As TagWatcher, e As EventArgs)

        Code = 200

        Response = sender.ObjectReturned

    End Sub

    Private Sub DownloadDocumentError(sender As TagWatcher, e As EventArgs)

        Code = 400

        Message = sender.ErrorDescription

    End Sub

#End Region

#Region "Metodos"
    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest

        'If context.Request.HttpMethod = "POST" Then

        If context.Request.Path = "/FileUploadHandler.upload" Then

            If context.Request.Files.Count > 0 Then

                Dim files As HttpFileCollection = context.Request.Files

                For i As Integer = 0 To files.Count - 1

                    Dim document As HttpPostedFile = files(i)

                    Dim data = New JavaScriptSerializer().Deserialize(Of Dictionary(Of String, Object))(context.Request.Form("data"))

                    Dim propiedadesDocumento = New JavaScriptSerializer().Deserialize(Of PropiedadesDocumento)(New JavaScriptSerializer().Serialize(data.Item("data")))

                    CDocumentos.UploadDocument(document.InputStream, Sub(ByVal documentodigital_ As PropiedadesDocumento)

                                                                         For Each property_ As PropertyInfo In documentodigital_.GetType.GetProperties

                                                                             If property_.CanWrite Then

                                                                                 property_.SetValue(documentodigital_, property_.GetValue(propiedadesDocumento, Nothing), Nothing)

                                                                             End If

                                                                         Next

                                                                     End Sub)

                    CDocumentos.Dispose()

                Next

                context.Response.ContentType = "text/json"

                context.Response.Write("{""code"":""" & Code & """,""response"":" & New JavaScriptSerializer().Serialize(Response) & ",""message"":""" & Message & """}")

                context.Response.End()

            End If

        ElseIf context.Request.Path = "/FileUploadHandler.download" Then

            Dim data = New JavaScriptSerializer().Deserialize(Of Dictionary(Of String, Object))(context.Request.Form("data"))

            Dim objId = New ObjectId(data.Item("fileId").ToString)

            Dim document_ As Byte() = CDocumentos.GetDocument(objId).ObjectReturned

            CDocumentos.Dispose()

            context.Response.ContentType = "text/json"

            context.Response.Write("{""code"":""" & Code & """,""response"":""" & Convert.ToBase64String(document_, 0, document_.Length) & """,""message"":""" & Message & """}")

            context.Response.End()

        ElseIf context.Request.Path = "/FileUploadHandler.delete" Then

            Dim data = New JavaScriptSerializer().Deserialize(Of Dictionary(Of String, Object))(context.Request.Form("data"))

            Dim objId = New ObjectId(data.Item("fileId").ToString)

            Dim response_ As TagWatcher = CDocumentos.DeleteDocument(objId)

            CDocumentos.Dispose()

            If response_.Status = TagWatcher.TypeStatus.Ok Then

                Code = 200

                Message = "Elemento borrado correctamente"

            Else

                Code = 400

                Message = "Lo sentimos, ocurrio un error inesperado al intentar esta acción"

            End If

            context.Response.ContentType = "text/json"

            context.Response.Write("{""code"":""" & Code & """,""response"":""" & Response & """,""message"":""" & Message & """}")

            context.Response.End()

        End If

        'End If

    End Sub

#End Region

End Class


