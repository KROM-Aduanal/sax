Imports System.IO
Imports System.Web
Imports gsol.krom
Imports Wma.Exceptions
Imports MongoDB.Driver.GridFS
Imports MongoDB.Bson
Imports Rec.Globals.Utils
Imports System.Reflection
Imports System.Web.Script.Serialization
Imports MongoDB.Driver
Imports MongoDB.Bson.Serialization
Imports MongoDB.Bson.Serialization.Attributes
Imports System.IO.Compression
Imports System.Text
Imports System.Runtime.Serialization
Imports System.ComponentModel
'pag 54 pdf patrones
Public Interface IControladorDocumento

#Region "Propiedades"
    Property State As TagWatcher

    Property DisableMD5 As Boolean

    Property ChunkSizeBytes As Integer

    Property BulkLimit As Integer

#End Region

#Region "Métodos"

    Function GetDocumentMetadata(ByVal documentId_ As Object) As TagWatcher
    Function GetDocument(ByVal documentId_ As Object) As TagWatcher
    Sub UploadDocument(ByVal document_ As Stream, ByVal func_ As Action(Of Object))
    Sub DownloadDocument(ByVal documentId_ As Object, ByVal Optional root_ As String = Nothing)
    Sub DownloadDocuments(ByVal documents_ As List(Of Object), ByVal Optional root_ As String = Nothing)
    Function FiledDocument(ByVal documentId_ As Object) As TagWatcher
    Function DeleteDocument(ByVal documentId_ As Object) As TagWatcher

#End Region

End Interface


Public Class ControladorDocumento
    Implements IControladorDocumento, IDisposable


#Region "Atributos"

    Private _database As IMongoDatabase

    Private _gridfs As GridFSBucket

    Private _disposedValue As Boolean

#End Region

#Region "Propiedades"

    Public Property State As TagWatcher Implements IControladorDocumento.State

    Public Property DisableMD5 As Boolean = False Implements IControladorDocumento.DisableMD5

    Public Property ChunkSizeBytes As Integer = 16000001 Implements IControladorDocumento.ChunkSizeBytes

    Public Property BulkLimit As Integer = 20 Implements IControladorDocumento.BulkLimit

#End Region

#Region "Eventos"

    Public Event OnUploadDocumentSuccess As EventHandler

    Public Event OnUploadDocumentError As EventHandler

    Public Event OnDownloadDocumentSuccess As EventHandler

    Public Event OnDownloadDocumentError As EventHandler

#End Region

#Region "Metodos"

    Public Overridable Function GetDocumentMetadata(ByVal documentId_ As Object) As TagWatcher _
        Implements IControladorDocumento.GetDocumentMetadata

        PrepararGridFs()

        Try

            Using stream_ = _gridfs.OpenDownloadStreamAsync(documentId_, Nothing, Nothing)

                Dim fileMetadata_ = stream_.Result.FileInfo.Metadata

                stream_.Result.Close()

                Dim documentoDigital_ = BsonSerializer.Deserialize(Of PropiedadesDocumento)(fileMetadata_)

                Return New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok, .ObjectReturned = documentoDigital_}

            End Using

        Catch ex As Exception

            Return New TagWatcher With {.Status = TagWatcher.TypeStatus.Errors, .ErrorDescription = ex.Message}

        End Try

    End Function

    Public Overridable Function GetDocument(ByVal documentId_ As Object) As TagWatcher _
        Implements IControladorDocumento.GetDocument

        PrepararGridFs()

        Try

            Dim file_ As Byte() = _gridfs.DownloadAsBytesAsync(documentId_, Nothing, Nothing).Result

            Dim document_ As Byte() = file_.Skip(4).ToArray

            Return New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok, .ObjectReturned = document_} 'New MemoryStream(document_)

        Catch ex As Exception

            Return New TagWatcher With {.Status = TagWatcher.TypeStatus.Errors, .ErrorDescription = ex.Message}

        End Try

    End Function

    Public Overridable Sub DownloadDocuments(ByVal documentsId_ As List(Of Object),
                                               Optional root_ As String = Nothing) Implements IControladorDocumento.DownloadDocuments

        If documentsId_.Count > BulkLimit Then

            RaiseEvent OnDownloadDocumentError(New TagWatcher With {.Status = TagWatcher.TypeStatus.Errors, .ErrorDescription = "Solo puedes descargar un máximo de " & BulkLimit & "documentos a la vez."}, EventArgs.Empty)

        Else

            PrepararGridFs()

            Dim location_ = IIf(root_ IsNot Nothing, root_, Environment.GetFolderPath(Environment.SpecialFolder.Desktop))

            Using memoryStream_ = New MemoryStream()

                Using zipArchive_ = New ZipArchive(memoryStream_, ZipArchiveMode.Create, True)

                    For Each documentId_ In documentsId_

                        Dim file_ As Byte() = _gridfs.DownloadAsBytesAsync(documentId_, Nothing, Nothing).Result

                        Dim mimetype_ = MimetypeFromByteArray(file_)

                        Dim document_ As Byte() = file_.Skip(4).ToArray

                        Dim zipEntry_ = zipArchive_.CreateEntry(documentId_.ToString & "." & mimetype_)

                        Using entryStream = zipEntry_.Open()

                            entryStream.Write(document_, 0, document_.Length)

                        End Using

                    Next

                End Using

                Dim zipBytes_ = memoryStream_.ToArray()

                Try

                    Dim zipname_ = ObjectId.GenerateNewId()

                    File.WriteAllBytes(location_ & "\\" & zipname_.ToString & ".zip", zipBytes_)

                    RaiseEvent OnDownloadDocumentSuccess(New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}, EventArgs.Empty)

                Catch ex As Exception

                    RaiseEvent OnDownloadDocumentError(New TagWatcher With {.Status = TagWatcher.TypeStatus.Errors, .ErrorDescription = ex.Message}, EventArgs.Empty)

                End Try

            End Using

        End If

    End Sub

    Public Overridable Sub DownloadDocument(ByVal documentId_ As Object,
                                              Optional root_ As String = Nothing) Implements IControladorDocumento.DownloadDocument

        PrepararGridFs()

        Dim location_ = IIf(root_ IsNot Nothing, root_, Environment.GetFolderPath(Environment.SpecialFolder.Desktop))

        Try

            Dim file_ As Byte() = _gridfs.DownloadAsBytesAsync(documentId_, Nothing, Nothing).Result

            Dim mimetype_ = MimetypeFromByteArray(file_)

            Dim document_ As Byte() = file_.Skip(4).ToArray

            File.WriteAllBytes(location_ & "\\" & documentId_.ToString & "." & mimetype_, document_)

            RaiseEvent OnDownloadDocumentSuccess(New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}, EventArgs.Empty)

        Catch ex As Exception

            RaiseEvent OnDownloadDocumentError(New TagWatcher With {.Status = TagWatcher.TypeStatus.Errors, .ErrorDescription = ex.Message}, EventArgs.Empty)

        End Try

    End Sub

    Public Overridable Sub UploadDocument(document_ As Stream,
                                           func_ As Action(Of Object)) _
                                           Implements IControladorDocumento.UploadDocument

        PrepararGridFs()

        Dim documentodigital_ As New PropiedadesDocumento

        If Not func_ Is Nothing Then

            func_(documentodigital_)

        End If

        Dim buff_ As Byte() = CombineByteArray(MimetypeToByteArray(documentodigital_.formatoarchivo.ToString), StreamToByteArray(document_))

        document_.Close()

        Dim opciones_ = New GridFSUploadOptions With {
                                .ChunkSizeBytes = ChunkSizeBytes,
                                .BatchSize = Convert.ToInt32(buff_.Length),
                                .Metadata = documentodigital_.ToBsonDocument,
                                .DisableMD5 = DisableMD5
                            }

        Try

            Dim objectId_ = _gridfs.UploadFromBytesAsync(documentodigital_.nombrearchivo, buff_, opciones_).Result.ToString

            RaiseEvent OnUploadDocumentSuccess(New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok, .ObjectReturned = New Dictionary(Of String, Object) From {{"fileId", objectId_}, {"fileName", documentodigital_.nombrearchivo}}}, EventArgs.Empty)

        Catch ex As Exception

            RaiseEvent OnUploadDocumentError(New TagWatcher With {.Status = TagWatcher.TypeStatus.Errors, .ErrorDescription = ex.Message}, EventArgs.Empty)

        End Try

        'Dim mimetype = DirectCast([Enum].Parse(GetType(DocumentoDigital.FormatosArchivo), Chr(file_.Last)), DocumentoDigital.FormatosArchivo).ToString
        'metadata_.Add("contentType", MimeMapping.GetMimeMapping(document_.Name))
    End Sub

    Public Overridable Function FiledDocument(ByVal documentId_ As Object) As TagWatcher _
        Implements IControladorDocumento.FiledDocument

        PrepararGridFs()

        Dim operationsDB_ = _database.GetCollection(Of BsonDocument)("fs.files")

        Dim filter_ = Builders(Of BsonDocument).Filter.Eq(Of ObjectId)("_id", documentId_)

        Dim setStructureOfSubs_ = Builders(Of BsonDocument).Update.Set(Of Integer)("metadata.archivado", 0)

        Dim result_ = operationsDB_.UpdateOneAsync(filter_, setStructureOfSubs_).Result

        If result_.MatchedCount <> 0 Then

            Return New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}

        ElseIf result_.UpsertedId IsNot Nothing Then

            Return New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}

        Else

            Return New TagWatcher With {.Status = TagWatcher.TypeStatus.Errors, .ErrorDescription = "No se ha podido archivar el documento"}

        End If

    End Function

    Public Overridable Function DeleteDocument(ByVal documentId_ As Object) As TagWatcher _
        Implements IControladorDocumento.DeleteDocument

        PrepararGridFs()

        Dim operationsDB_ = _database.GetCollection(Of BsonDocument)("fs.files")

        Dim filter_ = Builders(Of BsonDocument).Filter.Eq(Of ObjectId)("_id", documentId_)

        Dim setStructureOfSubs_ = Builders(Of BsonDocument).Update.Set(Of Integer)("metadata.estado", 0)

        Dim result_ = operationsDB_.UpdateOneAsync(filter_, setStructureOfSubs_).Result

        If result_.MatchedCount <> 0 Then

            Return New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}

        ElseIf result_.UpsertedId IsNot Nothing Then

            Return New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}

        Else

            Return New TagWatcher With {.Status = TagWatcher.TypeStatus.Errors, .ErrorDescription = "No se ha podido borrar el documento"}

        End If

    End Function

    Private Sub PrepararGridFs()

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            _database = iEnlace_.GetMongoClient().GetDatabase("SynDocs")

            _gridfs = New GridFSBucket(_database)

        End Using

    End Sub

    Private Function MimetypeFromByteArray(ByRef file_ As Byte())

        Dim mimetype_ = Chr(file_(0)) & Chr(file_(1)) & Chr(file_(2)) & Chr(file_(3))

        Return Trim(mimetype_)

    End Function

    Private Function MimetypeToByteArray(ByVal string_ As String)

        string_ = IIf(string_.Length = 3, string_ & Chr(32), string_)

        Return Encoding.ASCII.GetBytes(string_)

    End Function

    Private Function StreamToByteArray(ByVal stream_ As Stream)

        Using ms_ As New MemoryStream

            stream_.CopyTo(ms_)

            Return ms_.ToArray()

        End Using

    End Function

    Public Shared Function CombineByteArray(ByVal first_ As Byte(), ByVal second_ As Byte()) As Byte()

        Dim bytes_ As Byte() = New Byte(first_.Length + second_.Length - 1) {}

        Buffer.BlockCopy(first_, 0, bytes_, 0, first_.Length)

        Buffer.BlockCopy(second_, 0, bytes_, first_.Length, second_.Length)

        Return bytes_

    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not _disposedValue Then

            If disposing Then

                _gridfs = Nothing

                _database = Nothing

                State = Nothing

            End If

            _disposedValue = True

        End If

    End Sub

    ' ' TODO: reemplazar el finalizador solo si "Dispose(disposing As Boolean)" tiene código para liberar los recursos no administrados
    ' Protected Overrides Sub Finalize()
    '     ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose

        Dispose(disposing:=True)

        GC.SuppressFinalize(Me)

    End Sub

#End Region

End Class

<Serializable>
Public Class PropiedadesDocumento

#Region "Enums"

    Enum FormatosArchivo
        SinDefinir = 0
        <EnumMember> <Description("application/pdf")> pdf = 1
        <EnumMember> <Description("image/jpeg")> jpg = 2
        <EnumMember> <Description("text/xml")> xml = 3
    End Enum

    Enum TiposAccesibilidad
        SinDefinir = 0
        Publico = 1
        Privado = 2
    End Enum

    Enum TiposVinculacion
        SinDefinir = 0
        AgenciaAduanal = 1
        Sucursal = 2
        Corresponsalia = 3
        Cliente = 4
    End Enum

#End Region

#Region "Propiedades"

    'Property id As ObjectId
    Property nombrearchivo As String
    Property formatoarchivo As FormatosArchivo
    Property accesibilidad As TiposAccesibilidad
    Property tipovinculacion As TiposVinculacion
    Property datosadicionales As InformacionDocumento
    <BsonRepresentation(BsonType.ObjectId)>
    Property _idpropietario As String
    Property nombrepropietario As String
    <BsonRepresentation(BsonType.ObjectId)>
    Property _idinstitucionpropietaria As String
    Property nombreinstitucionpropietaria As String
    Property idpermisoconsulta As Integer
    Property archivado As Boolean = False
    Property estado As Integer = 1

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

End Class

'Public Overridable Function CargarDocumento(document_ As HttpPostedFile) As TagWatcher Implements IControladorDocumentoDigital.CargarDocumento

'    Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos
'    Dim operationsDB_ = iEnlace_.GetMongoClient()
'    Dim database_ = operationsDB_.GetDatabase("SynDocs")

'    Dim fs As New GridFSBucket(database_)

'    Dim fileStream = document_.InputStream
'    Dim fileBytes(0 To fileStream.Length - 1) As Byte
'    fileStream.Read(fileBytes, 0, fileBytes.Length)
'    fileStream.Close()

'    Dim metadata_ = New BsonDocument

'    metadata_.Add("contentType", document_.ContentType)

'    Dim opciones = New GridFSUploadOptions With {
'                        .ChunkSizeBytes = 16000001,
'                        .BatchSize = Convert.ToInt32(fileBytes.Length),
'                        .Metadata = metadata_
'                    }

'    Dim objectId = fs.UploadFromBytesAsync(document_.FileName, fileBytes, opciones).Result.ToString

'    Return New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok, .ObjectReturned = objectId}

'End Function


'https://stackoverflow.com/questions/17217077/create-zip-file-from-byte