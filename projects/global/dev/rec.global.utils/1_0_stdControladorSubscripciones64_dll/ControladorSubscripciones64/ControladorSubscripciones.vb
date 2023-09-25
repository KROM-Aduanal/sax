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
'Imports iText.IO.Source
Imports System.Text
Imports System.Runtime.Serialization
Imports System.ComponentModel
Imports Syn.Documento
Imports System.Security.Cryptography

Public Class ControladorSubscripciones
    Implements IDisposable

#Region "Enums"

    Enum Modalidades
        [Default] = 1
        ConRevisionEscritura = 2
    End Enum

#End Region

#Region "Atributos"

    Private _disposedValue As Boolean

    Private _statements As Sax.SaxStatements = Sax.SaxStatements.GetInstance()

#End Region

#Region "Propiedades"

    Public Property Estado As TagWatcher

    Public Property Modalidad As Modalidades

    Public Property Subscritores As List(Of subscriptionsgroup)

#End Region

#Region "Constructores"

    Sub New()

        Estado = New TagWatcher()

        Modalidad = Modalidades.Default

    End Sub

#End Region

#Region "Metodos"

    Public Function LeerSuscriptores(Of T)(id_ As Object) As List(Of subscriptionsgroup)

        Dim collectionName_ As String = Nothing : Dim databaseName_ As String = Nothing

        _statements.GetDatabaseAndCollectionName(databaseName_, collectionName_, GetType(T).Name, Nothing)

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim operationsDB_ = iEnlace_.GetMongoClient().
                                         GetDatabase(databaseName_).
                                         GetCollection(Of subscriptionsgroup)("Faf" & collectionName_)

            Dim filter_ = Builders(Of subscriptionsgroup).Filter.Eq(Of Object)("subscriptions.followers._idfriend", id_)

            Subscritores = operationsDB_.Find(filter_).ToList()

            Return Subscritores

        End Using

    End Function

    Public Function LeerSuscriptores(resource_ As String, id_ As Object) As List(Of subscriptionsgroup)

        Dim collectionName_ As String = Nothing : Dim databaseName_ As String = Nothing

        _statements.GetDatabaseAndCollectionName(databaseName_, collectionName_, resource_, Nothing)

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim operationsDB_ = iEnlace_.GetMongoClient().
                                         GetDatabase(databaseName_).
                                         GetCollection(Of subscriptionsgroup)("Faf" & collectionName_)

            Dim filter_ = Builders(Of subscriptionsgroup).Filter.Eq(Of Object)("subscriptions.followers._idfriend", id_)

            Subscritores = operationsDB_.Find(filter_).ToList()

            Return Subscritores

        End Using

    End Function

    Public Function EliminarSuscriptores(Of T)(ByVal id_ As Object) As TagWatcher

        Dim collectionName_ As String = Nothing : Dim databaseName_ As String = Nothing

        _statements.GetDatabaseAndCollectionName(databaseName_, collectionName_, GetType(T).Name, Nothing)

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim operationsDB_ = iEnlace_.GetMongoClient().
                                         GetDatabase(databaseName_).
                                         GetCollection(Of subscriptionsgroup)("Faf" & collectionName_)

            Dim filter_ = Builders(Of subscriptionsgroup).Filter.Eq(Of Object)("subscriptions.followers._idfriend", id_)

            Dim setStructureOfSubs_ = Builders(Of subscriptionsgroup).Update.PullFilter("subscriptions.followers", Builders(Of follower).Filter.Eq(Of Object)("_idfriend", id_))

            Dim result_ = operationsDB_.UpdateOneAsync(filter_, setStructureOfSubs_).Result

            If result_.ModifiedCount > 0 Then

                Estado.SetOK()

            Else

                Estado.SetOKInfo(Me, "Elemento no encontrado.")

            End If

            Return Estado

        End Using

    End Function

    Public Function EliminarSuscriptores(resource_ As String, ByVal id_ As Object) As TagWatcher

        Dim collectionName_ As String = Nothing : Dim databaseName_ As String = Nothing

        _statements.GetDatabaseAndCollectionName(databaseName_, collectionName_, resource_, Nothing)

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim operationsDB_ = iEnlace_.GetMongoClient().
                                         GetDatabase(databaseName_).
                                         GetCollection(Of subscriptionsgroup)("Faf" & collectionName_)

            Dim filter_ = Builders(Of subscriptionsgroup).Filter.Eq(Of Object)("subscriptions.followers._idfriend", id_)

            Dim setStructureOfSubs_ = Builders(Of subscriptionsgroup).Update.PullFilter("subscriptions.followers", Builders(Of follower).Filter.Eq(Of Object)("_idfriend", id_))

            Dim result_ = operationsDB_.UpdateOneAsync(filter_, setStructureOfSubs_).Result

            If result_.ModifiedCount > 0 Then

                Estado.SetOK()

            Else

                Estado.SetOKInfo(Me, "Elemento no encontrado.")

            End If

            Return Estado

        End Using

    End Function

    Public Function EliminarSuscriptores(temporalName_ As String, temporalId_ As ObjectId, session_ As IClientSessionHandle) As TagWatcher
        Return New TagWatcher(1)
    End Function

    Public Function Bitacora() As TagWatcher

        Estado.SetOK()

        Return Estado

    End Function

    'J. Oropeza: es posible este método se mueva a organismo ya que su funcionalidad es muy genérica y sera muy utilizada desde diversos puntos
    Public Function Disponibillidad(Of T)(ByVal id_ As Object) As Boolean

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)(GetType(T).Name)

            Dim filter_ = Builders(Of OperacionGenerica).Filter.Eq(Of Object)("_id", id_) And
                          Builders(Of OperacionGenerica).Filter.Eq(Of Object)("abierto", True)

            Dim results_ = operationsDB_.Find(filter_).ToList()

            If results_.Count Then

                Return True

            Else

                Return False

            End If

        End Using

    End Function

    Public Function DifusionDatos(ByVal documentoElectronico_ As DocumentoElectronico, Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        'Dim listOfUpdateModels = New List(Of UpdateOneModel(Of OperacionGenerica))

        For Each subscriptionsgroup_ As subscriptionsgroup In Subscritores

            Dim idList_ = New List(Of Object)

            Dim updateDefinition_ = New List(Of UpdateDefinition(Of OperacionGenerica))()

            For Each follower_ As follower In subscriptionsgroup_.subscriptions.followers

                idList_.Add(follower_._myid)

            Next

            With updateDefinition_

                For Each fieldinfo_ As fieldInfo In subscriptionsgroup_.subscriptions.fields

                    .Add(Builders(Of OperacionGenerica).Update.Set(Of Object)(subscriptionsgroup_.subscriptions.namespaces(fieldinfo_.nsp - 1).path & "." & fieldinfo_.attr, documentoElectronico_.Campo(fieldinfo_._id).Valor))

                Next

                .Add(Builders(Of OperacionGenerica).Update.Set(Of Object)("FirmaDigital", GenerarFirmaDigital()))

            End With

            Dim collectionName_ As String = Nothing : Dim databaseName_ As String = Nothing

            Dim rol_ = _statements.GetDatabaseAndCollectionName(databaseName_, collectionName_, subscriptionsgroup_.fromcollectionname.Replace(_statements.GetOfficeOnline()._id.ToString.PadLeft(2, "0"c), ""), Nothing)

            If rol_.officesuffix Then

                collectionName_ = collectionName_ & _statements.GetOfficeOnline()._id.ToString.PadLeft(2, "0"c)

            End If

            'Dim updateOneModel = New UpdateOneModel(Of OperacionGenerica)(
            '    Builders(Of OperacionGenerica).Filter.In(Of Object)("_id", idList_),
            '    Builders(Of OperacionGenerica).Update.Combine(updateDefinition_))

            'listOfUpdateModels.Add(updateOneModel)

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

                Dim operationsDB_ = iEnlace_.GetMongoClient().
                                             GetDatabase(databaseName_).
                                             GetCollection(Of OperacionGenerica)(collectionName_)

                Dim filter_ = Builders(Of OperacionGenerica).Filter.In(Of Object)("_id", idList_)

                Dim combinedUpdate_ = Builders(Of OperacionGenerica).Update.Combine(updateDefinition_)

                Try

                    Dim result_ = operationsDB_.UpdateOneAsync(session_, filter_, combinedUpdate_).Result

                    If result_.ModifiedCount > 0 Then

                        Estado.SetOK()

                    Else

                        Estado.SetOKInfo(Me, "Elemento no encontrado o no sufrio cambios.")

                    End If

                Catch ex As Exception

                    Estado.SetError(Me, ex.Message)

                End Try
                '----------------------bulk
                'operationsDB_.BulkWriteAsync(listOfUpdateModels)

            End Using

        Next

        Return Estado

    End Function


    Public Function FirmaDigital(Of T)(ByVal id_ As Object) As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)(GetType(T).Name)

            Dim filter_ = Builders(Of OperacionGenerica).Filter.Eq(Of Object)("_id", id_)

            Dim setStructureOfSubs_ = Builders(Of OperacionGenerica).Update.Set(Function(x) x.FirmaDigital, GenerarFirmaDigital())

            Dim result_ = operationsDB_.UpdateOneAsync(filter_, setStructureOfSubs_).Result

            If result_.ModifiedCount > 0 Then

                Estado.SetOK()

            Else

                Estado.SetOKInfo(Me, "Elemento no encontrado.")

            End If

            Return Estado

        End Using

    End Function

    Private Function GenerarFirmaDigital() As String

        Dim sha_ As New SHA1CryptoServiceProvider

        Dim bytesToHash_() As Byte

        Dim espacioTrabajo_ = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")

        bytesToHash_ = System.Text.Encoding.ASCII.GetBytes(espacioTrabajo_.MisCredenciales.ClaveUsuario & Date.Now.ToString)

        bytesToHash_ = sha_.ComputeHash(bytesToHash_)

        Dim signature_ As String = ""

        For Each byte_ As Byte In bytesToHash_

            signature_ += byte_.ToString("x2")

        Next

        Return signature_

    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not _disposedValue Then

            If disposing Then

                Estado = Nothing

                Modalidad = Nothing

                Subscritores = Nothing

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
