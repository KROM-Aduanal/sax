Imports Wma.Exceptions

Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports System.Threading.Tasks
'Imports gsol.krom
Imports gsol.krom
Imports gsol.BaseDatos.Operaciones


'Imports gsol.monitoreo
'Imports gsol.BaseDatos.Operaciones

Namespace gsol.basededatos

    Public Interface IConexionesNoSQL : Inherits IDisposable


#Region "Methods"
        'Function GetMongoCollection(Of T)(Optional ByVal rolname_ As IEnlaceDatos.RolNames = IEnlaceDatos.RolNames.bigdata) As IMongoCollection(Of T)
        'Function GetMongoCollection(Of T)(ByVal resourceName_ As String) As IMongoCollection(Of T)

        'Function GetMongoCollection(Of T)(ByRef imongoClient_ As IMongoClient,
        '                                  ByVal resourceName_ As String) As IMongoCollection(Of T)
        Function GetMongoCollection(Of T)(ByRef imongoClient_ As IMongoClient,
                                                 Optional ByVal resourceName_ As String = Nothing,
                                                 Optional ByVal rootId_ As Int32? = Nothing) As IMongoCollection(Of T)

        Function GetMongoCollection(Of T)(Optional ByVal resourceName_ As String = Nothing,
                                                 Optional ByVal rootId_ As Int32? = Nothing) As IMongoCollection(Of T)

        'Function GetMongoCollection(Of T)(ByVal collectiontype_ As Sax.SaxStatements.CollectionTypes,
        '                                         Optional ByVal rolname_ As IEnlaceDatos.RolNames = IEnlaceDatos.RolNames.bigdata) As IMongoCollection(Of T)
        'Function GetMongoCollection(Of T)(ByRef imongoClient_ As IMongoClient,
        '                                         ByVal collectionTypes_ As Sax.SaxStatements.CollectionTypes,
        '                                         Optional ByVal rolname_ As IEnlaceDatos.RolNames = IEnlaceDatos.RolNames.bigdata) As IMongoCollection(Of T)
        'Function GetMongoClient(ByVal rolname_ As IEnlaceDatos.RolNames,
        '                          Optional ByVal settingsType_ As Sax.SaxStatements.SettingTypes = Sax.SaxStatements.SettingTypes.Projects) As IMongoClient

        Function GetMongoClient(Optional ByVal settingsType_ As Sax.SaxStatements.SettingTypes = Sax.SaxStatements.SettingTypes.Projects) As IMongoClient

        Function EjecutaConsultaAsync(ByVal collectionName_ As String,
                                                  ByVal queryDocument_ As QueryDocument,
                                                  Optional ByVal projectionDefinition_ As ProjectionDefinition(Of BsonDocument) = Nothing,
                                                  Optional ByVal collectionLimit_ As Int32 = 200) As Task(Of List(Of BsonDocument))

        Function EjecutaConsulta(ByVal collectionName_ As String,
                                 ByVal queryDocument_ As QueryDocument,
                                 Optional ByVal projectionDefinition_ As ProjectionDefinition(Of BsonDocument) = Nothing,
                                 Optional ByVal collectionLimit_ As Int32 = 200) As List(Of BsonDocument) 'MongoCursor(Of BsonDocument)


        Function Find(ByVal collectionName_ As String,
                                 ByVal queryDocument_ As QueryDocument,
                                 Optional ByVal projectionDefinition_ As ProjectionDefinition(Of BsonDocument) = Nothing,
                                 Optional ByVal collectionLimit_ As Int32 = 200) As List(Of BsonDocument) 'MongoCursor(Of BsonDocument)


        Function Aggregate(ByVal collectionName_ As String,
                                 ByVal queryDocument_ As QueryDocument,
                                 Optional ByVal projectionDefinition_ As ProjectionDefinition(Of BsonDocument) = Nothing,
                                 Optional ByVal collectionLimit_ As Int32 = 200) As List(Of BsonDocument) 'MongoCursor(Of BsonDocument)

        'Function EjecutaSentencia(ByVal collectionName_ As String,
        '                          ByVal bsonDocument_ As BsonDocument) As Boolean

        Function EjecutaSentencia(ByVal collectionName_ As String,
                                  ByVal bsonDocument_ As BsonDocument,
                                  ByVal tipoOperacion_ As IOperacionesCatalogo.TiposOperacionSQL) As Boolean
        Function EjecutaSentencia(ByVal collectionName_ As String,
                                  ByVal bsonDocument_ As List(Of BsonDocument),
                                  ByVal tipoOperacion_ As IOperacionesCatalogo.TiposOperacionSQL) As Boolean

        Function EjecutaSentencia(ByVal collectionName_ As String,
                                  ByVal bsonDocument_ As BsonDocument,
                                  ByVal tipoOperacion_ As IOperacionesCatalogo.TiposOperacionSQL,
                                  ByVal apuntador_ As Dictionary(Of String, Object)) As Task(Of Boolean)


#End Region

    End Interface

End Namespace
