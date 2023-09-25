Imports gsol.monitoreo
Imports gsol.seguridad
Imports gsol.BaseDatos.Operaciones

Imports System
Imports System.Threading.Tasks
Imports System.Text.RegularExpressions

Imports MongoDB.Bson
Imports MongoDB.Driver
Imports MongoDB.Driver.Linq
Imports MongoDB.Driver.GridFS
Imports MongoDB.Driver.Builders
Imports MongoDB.Bson.Serialization
Imports MongoDB.Driver.MongoDBRefSerializer
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Bson.Serialization.BsonSerializationContext
Imports gsol.krom

Namespace gsol.basededatos

    Public Class ConexionesNoSQL : Inherits Conexiones
        Implements IConexiones, IConexionesNoSQL, IDisposable

#Region "Attributes"

        Private _i_TipoInstrumentacion As IBitacoras.TiposInstrumentacion = IBitacoras.TiposInstrumentacion.GestorIEnlaceDatos

        Private _i_TiempoEspera As Int32 = 0

        'Ahora incluiremos el manifiesto de carga de Sax para obtener recursos de ahí mediante el singleton
        Private _statements As Sax.SaxStatements = Sax.SaxStatements.GetInstance()
#End Region

#Region "Builders"

        Sub New()

            'Configuracion.ObtenerInstancia()

            'IpServidor = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.BMDDireccionIPMongoDB)

            'PuertoConexion = 27017

            'NombreBaseDatos = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.BMDNombreMongoDB)

            'Usuario = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.BMDUsuarioMongoDB)

            'Contrasena = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.BMDPasswordMongoDB)

            Dim _statements As Sax.SaxStatements = Sax.SaxStatements.GetInstance()

            With _statements

                IpServidor = .GetEndPoint("project", .GetRol("project", "bigdataodfs", "nosql", "mongodb").endpointId).ip

                NombreBaseDatos = .GetRol("project", "bigdataodfs", "nosql", "mongodb").name

                PuertoConexion = .GetEndPoint("project", .GetRol("project", "bigdataodfs", "nosql", "mongodb").endpointId).port

                Usuario = .GetCredentials("project", .GetRol("project", "bigdataodfs", "nosql", "mongodb").credentialId).user

                Contrasena = .GetCredentials("project", .GetRol("project", "bigdataodfs", "nosql", "mongodb").credentialId).password

            End With

            _estadoconexion = False

            'Mongo settings

            ControladorBaseDatos = IConexiones.Controladores.MongoDB

            If ObjetoDatos = IConexiones.TipoConexion.Automatico Then

                ObjetoDatos = IConexiones.TipoConexion.DirectMongoDB

            ElseIf ObjetoDatos = IConexiones.TipoConexion.MySQLCommand _
                Or ObjetoDatos = IConexiones.TipoConexion.OdbcCommand _
                Or ObjetoDatos = IConexiones.TipoConexion.SqlCommand Then

                ObjetoDatos = IConexiones.TipoConexion.DirectMongoDB

            End If

            If RepositorioDatos = IConexiones.TiposRepositorio.Automatico Then

                RepositorioDatos = IConexiones.TiposRepositorio.BSONDocumentObject

            End If


            _cadenaconexion = CadenaConexion()

        End Sub

#End Region

#Region "Properties"

#End Region

#Region "Methods"

        Public Function GetMongoCollection(Of T)(Optional ByVal resourceName_ As String = Nothing,
                                                 Optional ByVal rootId_ As Int32? = Nothing) As IMongoCollection(Of T) Implements IConexionesNoSQL.GetMongoCollection

            Return GetMongoCollection(Of T)(GetMongoClient(), resourceName_:=resourceName_, rootId_:=rootId_)

        End Function

        Public Function GetMongoCollection(Of T)(ByRef imongoClient_ As IMongoClient,
                                                 Optional ByVal resourceName_ As String = Nothing,
                                                 Optional ByVal rootId_ As Int32? = Nothing) As IMongoCollection(Of T) Implements IConexionesNoSQL.GetMongoCollection

            Dim mongoCollection_ As IMongoCollection(Of T)

            Dim databaseName_ As String = Nothing

            Dim collectionName_ As String = Nothing

            Dim rol_ As Sax.rol

            With _statements

                rol_ = .GetDatabaseAndCollectionName(databaseName_, collectionName_, resourceName_, rootId_)


                If rol_.officesuffix Then

                    collectionName_ = collectionName_ & .GetOfficeOnline()._id.ToString.PadLeft(2, "0"c)

                End If

                mongoCollection_ = imongoClient_.GetDatabase(databaseName_).GetCollection(Of T)(collectionName_)  'D0001_OO

            End With

            Return mongoCollection_

        End Function


        Public Function GetMongoClient(Optional ByVal settingsType_ As Sax.SaxStatements.SettingTypes = Sax.SaxStatements.SettingTypes.Projects) As IMongoClient Implements IConexionesNoSQL.GetMongoClient

            Dim stringConnection_ As String = Nothing

            Dim settingstypestr_ As String = Nothing

            Dim dbcontrollerstr_ As String = "mongodb"

            Dim typecontroller_ As String = "nosql"

            Select Case settingsType_
                Case Sax.SaxStatements.SettingTypes.Core : settingstypestr_ = "core"
                Case Sax.SaxStatements.SettingTypes.Projects : settingstypestr_ = "project"
                Case Sax.SaxStatements.SettingTypes.Globals : settingstypestr_ = "globals"
            End Select

            Dim ipservidor_ As String
            Dim stringoptions_ As String
            Dim usuario_ As String
            Dim contrasena_ As String
            Dim puerto_ As String

            With _statements

                ipservidor_ = .GetEndPoint(settingstypestr_, .GetRol(settingstypestr_, "bigdataops", "nosql", "mongodb").endpointId).ip
                stringoptions_ = .GetEndPoint(settingstypestr_, .GetRol(settingstypestr_, "bigdataops", "nosql", "mongodb").endpointId).stringoptions
                puerto_ = .GetEndPoint(settingstypestr_, .GetRol(settingstypestr_, "bigdataops", "nosql", "mongodb").endpointId).port

                usuario_ = .GetCredentials(settingstypestr_, .GetRol(settingstypestr_, "bigdataops", "nosql", "mongodb").credentialId).user
                contrasena_ = .GetCredentials(settingstypestr_, .GetRol(settingstypestr_, "bigdataops", "nosql", "mongodb").credentialId).password

            End With


            stringConnection_ = "mongodb://" & IIf(Not usuario_ Is Nothing, usuario_ & ":" & contrasena_ & "@", Nothing) &
                                Trim(ipservidor_) &
                                IIf(Not puerto_ Is Nothing, ":" & puerto_, Nothing) &
                                IIf(Not stringoptions_ Is Nothing, "/" & stringoptions_, Nothing)

            Dim client_ = New MongoClient(stringConnection_)

            Return client_

        End Function

        Public Async Function AggregateAsync(ByVal collectionName_ As IMongoCollection(Of BsonDocument),
                                         Optional ByVal match_ As BsonDocument = Nothing,
                                         Optional ByVal unwind_ As BsonDocument = Nothing,
                                         Optional ByVal groupBy_ As BsonDocument = Nothing,
                                         Optional ByVal projectionFields_ As BsonDocument = Nothing,
                                         Optional ByVal sort_ As BsonDocument = Nothing) As Task

            Dim utcTime5yearsago As DateTime = DateTime.Now.AddYears(-1).ToUniversalTime()

            Dim internalMatch_ = New BsonDocument From {
        {"$match", New BsonDocument From {
            {"f_FechaModificacion", New BsonDocument From {
                {"$gte", utcTime5yearsago}
            }}
        }}
        }

            '*****************funciona OK********************
            'Dim matchMembershipDateOperation = New BsonDocument From {
            '                                                                {"$match", New BsonDocument From {
            '                                                                    {"f_FechaModificacion", New BsonDocument From {
            '                                                                        {"$gte", inicio_}
            '                                                                    }}
            '                                                                }}
            '                                                             }

            Dim internalUnwind_ = New BsonDocument
            '    From {
            '    {"$unwind", "$Cars"}
            '}
            Dim internalGroupBy_ = New BsonDocument
            'From {
            '    {"$group", New BsonDocument From {
            '        {"_id", New BsonDocument From {
            '            {"Car", "$Cars"}
            '        }},
            '        {"Owners", New BsonDocument From {
            '            {"$addToSet", New BsonDocument From {
            '                {"_id", "$_id"},
            '                {"Lastname", "$Lastname"},
            '                {"Forename", "$Forename"},
            '                {"Age", "$Age"},
            '                {"MembershipDate", "$MembershipDate"}
            '            }}
            '        }}
            '    }}
            '}
            Dim internalProjectionFields_ = New BsonDocument From {
        {"$project", New BsonDocument From {
            {"_id", 0},
            {"MakeOfCar", "$_id.Car"},
            {"Owners", 1}
        }}
    }
            Dim internalSort_ = New BsonDocument From {
        {"$sort", New BsonDocument From {
            {"MakeOfCar", 1}
        }}
    }
            Dim pipeline_ = {match_, unwind_, groupBy_, projectionFields_, sort_}

            Dim objectResult_ = Await collectionName_.Aggregate(Of BsonDocument)(pipeline_).ToListAsync()

            'Console.WriteLine("Members grouped by Car Marque and ordered by Surname Forename")

            For Each stat As BsonDocument In objectResult_

                Console.WriteLine(vbLf & vbCr & "Car Marque : {0}" & vbLf & vbCr)

                'Dim clubMembers As IEnumerable(Of BsonDocument) =
                '    stat.Owners.ToArray().[Select](Function(d) BsonSerializer.Deserialize(Of ClubMember)(d)).OrderBy(Function(c) c.Lastname).ThenBy(Function(c) c.Forename).ThenBy(Function(c) c.Age).[Select](Function(c) c)

                'For Each clubMember As ClubMember In clubMembers
                '    ConsoleHelper.PrintClubMemberToConsole(clubMember)
                'Next
            Next

        End Function

        Public Overloads Function Aggregate(ByVal collectionName_ As String,
                                                  ByVal queryDocument_ As QueryDocument,
                                                  Optional ByVal projectionDefinition_ As ProjectionDefinition(Of BsonDocument) = Nothing,
                                                  Optional ByVal collectionLimit_ As Int32 = 200) As List(Of BsonDocument) Implements IConexionesNoSQL.Aggregate


            'Dim cliente_ = New MongoClient(_cadenaconexion)

            'Dim database_ = cliente_.GetDatabase(NombreBaseDatos)

            'Dim collectionData_ = database_.GetCollection(Of BsonDocument)(collectionName_)

            '-----------------------------------------------------------------------------------

            Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim collectionData_ = GetMongoCollection(Of BsonDocument)(resourceName_:=collectionName_)

            '----------------------------------------------------

            Dim listBSONDocuments_ As List(Of BsonDocument)



            If Not projectionDefinition_ Is Nothing Then

                Dim utcTime5yearsago = DateTime.Now.AddYears(-1).ToUniversalTime()
                '03/20/2021
                Dim inicio_ = New DateTime(2021, 3, 24)

                Dim fin_ = New DateTime(2021, 4, 4)

                '*****************funciona OK********************
                Dim matchMembershipDateOperation = New BsonDocument From {
                                                                            {"$match", New BsonDocument From {
                                                                                {"f_FechaModificacion", New BsonDocument From {
                                                                                    {"$gte", inicio_}
                                                                                }}
                                                                            }}
                                                                         }
                'Dim pipeline = {matchMembershipDateOperation, unwindCarsOperation, groupByCarTypeOperation, projectMakeOfCarOperation, sortCarsOperation}
                Dim pipeline = {matchMembershipDateOperation}

                'es async y await tambien
                Dim carStatsList = collectionData_.Aggregate(Of BsonDocument)(pipeline).ToList()

                Dim command As String = "{f_FechaModificacion : {$gte: new Date('23/03/2021').toLocaleDateString(), $lt : new Date('24/03/2021').toLocaleDateString() }}"

                Dim bsonDoc = BsonSerializer.Deserialize(Of BsonDocument)(command)
                Dim queryDoc = New QueryDocument(bsonDoc)
                'Dim p As MongoCursor(Of Person) = people.Find(queryDoc)

                '*********************************************

                listBSONDocuments_ = collectionData_.Find(queryDoc).Limit(collectionLimit_).ToList

                '**************bloque de pruebas**********************

            Else

                listBSONDocuments_ = collectionData_.Find(queryDocument_).Limit(collectionLimit_).ToList

            End If

            Return listBSONDocuments_

        End Function

        Public Overloads Function EjecutaConsulta(ByVal collectionName_ As String,
                                                  ByVal queryDocument_ As QueryDocument,
                                                  Optional ByVal projectionDefinition_ As ProjectionDefinition(Of BsonDocument) = Nothing,
                                                  Optional ByVal collectionLimit_ As Int32 = 200) As List(Of BsonDocument) Implements IConexionesNoSQL.EjecutaConsulta, IConexionesNoSQL.Find

            'MOP 07/03/2022 obsoleto--------------------------------------------------------
            'Dim cliente_ = New MongoClient(_cadenaconexion)

            'Dim database_ = cliente_.GetDatabase(NombreBaseDatos)

            'Dim collectionData_ = database_.GetCollection(Of BsonDocument)(collectionName_)

            '- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            '-----------------------------------------------------------------------------------

            Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim collectionData_ = GetMongoCollection(Of BsonDocument)(resourceName_:=collectionName_)

            '----------------------------------------------------

            '- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            Dim listBSONDocuments_ As List(Of BsonDocument)

            If Not projectionDefinition_ Is Nothing Then

                listBSONDocuments_ = collectionData_.
                    Find(queryDocument_).
                    Project(projectionDefinition_).
                    Limit(collectionLimit_).ToList

                'Using cursor = Await col.Find(New BsonDocument).Project(projectionStructure_).ToCursorAsync
                '    While (Await cursor.MoveNextAsync())
                '         For Each doc In cursor.Current
                '            dr = dtCliente.NewRow()
                '            dr("ID") = doc("_id")
                '            dr("NOMBRE") = doc("NOMBRE")
                '            dr("TELEFONO") = doc("TELEFONO")
                '            dr("FECHA_NACIMIENTO") = doc("FECHA_NACIMIENTO")
                '            dtCliente.Rows.Add(dr)

                '        Next
                '    End While
                'End Using

            Else

                listBSONDocuments_ = collectionData_.
                    Find(queryDocument_).
                    Limit(collectionLimit_).
                    ToList

            End If

            Return listBSONDocuments_

        End Function

        Public Overloads Async Function EjecutaConsultaAsync(ByVal collectionName_ As String,
                                                  ByVal queryDocument_ As QueryDocument,
                                                  Optional ByVal projectionDefinition_ As ProjectionDefinition(Of BsonDocument) = Nothing,
                                                  Optional ByVal collectionLimit_ As Int32 = 200) As Task(Of List(Of BsonDocument)) Implements IConexionesNoSQL.EjecutaConsultaAsync

            '-----------------------------------------------------------------------------------

            'Dim cliente_ = New MongoClient(_cadenaconexion)

            'Dim database_ = cliente_.GetDatabase(NombreBaseDatos)

            'Dim collectionData_ = database_.GetCollection(Of BsonDocument)(collectionName_)


            '-----------------------------------------------------------------------------------


            Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim collectionData_ = GetMongoCollection(Of BsonDocument)(resourceName_:=collectionName_)


            '----------------------------------------------------

            Dim listBSONDocuments_ As New List(Of BsonDocument)

            If Not projectionDefinition_ Is Nothing Then

                Using cursor_ = Await collectionData_.
                    Find(queryDocument_).
                    Project(projectionDefinition_).
                    Limit(collectionLimit_).
                    ToCursorAsync

                    While (Await cursor_.MoveNextAsync())

                        For Each document_ In cursor_.Current

                            listBSONDocuments_.Add(document_)

                        Next

                    End While

                End Using

            Else

                Using cursor_ = Await collectionData_.
                    Find(queryDocument_).
                    Limit(collectionLimit_).
                    ToCursorAsync

                    While (Await cursor_.MoveNextAsync())

                        For Each document_ In cursor_.Current

                            listBSONDocuments_.Add(document_)

                        Next

                    End While

                End Using

            End If

            Return listBSONDocuments_

        End Function

        Public Overloads Function EjecutaConsultaRESPALDO(ByVal collectionName_ As String,
                                                  ByVal queryDocument_ As QueryDocument,
                                                  Optional ByVal projectionDefinition_ As ProjectionDefinition(Of BsonDocument) = Nothing,
                                                  Optional ByVal collectionLimit_ As Int32 = 200) As List(Of BsonDocument)


            'Dim cliente_ = New MongoClient(_cadenaconexion)

            'Dim database_ = cliente_.GetDatabase(NombreBaseDatos)

            'Dim collectionData_ = database_.GetCollection(Of BsonDocument)(collectionName_)


            '-----------------------------------------------------------------------------------


            Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim collectionData_ = GetMongoCollection(Of BsonDocument)(resourceName_:=collectionName_)


            '----------------------------------------------------



            Dim listBSONDocuments_ As List(Of BsonDocument)


            If Not projectionDefinition_ Is Nothing Then

                '**************bloque de pruebas**********************

                'Dim utcTime5yearsago = DateTime.Now.AddYears(-1).ToUniversalTime()
                ''03/20/2021
                'Dim inicio_ = New DateTime(2021, 3, 24)

                'Dim fin_ = New DateTime(2021, 4, 4)

                '*****************funciona OK Aggregation********************
                'Dim matchMembershipDateOperation = New BsonDocument From {
                '                                                            {"$match", New BsonDocument From {
                '                                                                {"f_FechaModificacion", New BsonDocument From {
                '                                                                    {"$gte", inicio_}
                '                                                                }}
                '                                                            }}
                '                                                         }
                ''Dim pipeline = {matchMembershipDateOperation, unwindCarsOperation, groupByCarTypeOperation, projectMakeOfCarOperation, sortCarsOperation}
                'Dim pipeline = {matchMembershipDateOperation}

                ''es async y await tambien
                'Dim carStatsList = collectionData_.Aggregate(Of BsonDocument)(pipeline).ToList()

                'listBSONDocuments_ = carStatsList

                'Return listBSONDocuments_

                '*****************funciona OK Aggregation********************

                '********************COMMAND*********************************************************

                'Dim command As String = "{f_FechaModificacion : {$gte: new Date('24/03/2021').toLocaleDateString(), $lt : new Date('24/03/2021').toLocaleDateString() }}"
                'Dim command As String = "{f_FechaModificacion : {$gte: new Date('24/03/2021')}, t_Referencia: /.*TOL.*/}"  'OK


                'OK, así se suman cosas al QueryDocument,
                ''por cierto supuestamente Query.EQ genera un queryDocument, la verdad es que en las pruebas Query.EQ hoy genera QueryBuilder y no es lo mismo
                Dim command As String = "{f_FechaModificacion : {$eq: new Date('23/03/2021')}}"
                Dim bsonDoc = BsonSerializer.Deserialize(Of BsonDocument)(command)
                Dim queryDoc = New QueryDocument(bsonDoc)

                queryDocument_.AddRange(bsonDoc)

                listBSONDocuments_ = collectionData_.Find(queryDocument_).Limit(collectionLimit_).ToList

                '********************COMMAND*********************************************************

                '***************filter ***************************
                Dim inicio_ = New DateTime(2021, 3, 24).ToUniversalTime

                'FALLA con string directamente
                Dim filter22s = New FilterDefinitionBuilder(Of BsonDocument)().Gt(CType("f_FechaModificacion", FieldDefinition(Of BsonDocument, Date)), "24/03/2021")
                listBSONDocuments_ = collectionData_.Find(filter22s).Limit(collectionLimit_).ToList

                'OK, funciona perfecto.
                Dim fromDate = DateTime.SpecifyKind(DateTime.Parse("23/03/2021"), DateTimeKind.Utc)
                Dim toDate = DateTime.SpecifyKind(DateTime.Parse("24/03/2021"), DateTimeKind.Utc)
                Dim clause1 = New FilterDefinitionBuilder(Of BsonDocument)().Gte(CType("f_FechaModificacion", FieldDefinition(Of BsonDocument, DateTime)), toDate)
                listBSONDocuments_ = collectionData_.Find(clause1).Limit(collectionLimit_).ToList


                Dim clause22 = New FilterDefinitionBuilder(Of BsonDocument)()




                'FALLA, no acepta la conversión de IMongoQuery a MongoWrapper, pero tiene potencial el Query de IMongoQuery
                'Dim clause2 As IMongoQuery = query.[And](query.GTE("f_FechaModificacion", "23/03/2021"), query.LTE("f_FechaModificacion", "24/03/2021"))
                'Dim clause2 = New FilterDefinitionBuilder(Of BsonDocument)().[And](Query.GTE("f_FechaModificacion", fromDate), Query.LTE("f_FechaModificacion", toDate))
                'Dim carStatsList = collectionData_.Aggregate(Of BsonDocument)(clause2).ToList()
                'listBSONDocuments_ = collectionData_.Find(clause2).Limit(collectionLimit_).ToList

                'clause2 = Query.[And](Query.GTE("f_FechaModificacion", fromDate),
                '           Query.LTE("f_FechaModificacion", toDate),
                '           Query.EQ(CType("f_FechaModificacion", FieldDefinition(Of BsonDocument, DateTime), toDate)),
                '           Query.EQ("k", "msg"))



                'OK, la fecha se pide normal,y todo ok, pero el depurador la muestra en otro formado; No afecta, pero tomar en cuenta.
                Dim filter2 = "{f_FechaModificacion: {$gte: new Date('24/03/2021')}, t_Referencia: /.*TOL.*/}"

                'OK, { $And[ {"f_FechaModificacion":{$gte: New Date('23/03/2021')}}, {"f_FechaModificacion":{$lte: new Date('24/03/2021')}} ]}

                listBSONDocuments_ = collectionData_.Find(filter2).Limit(collectionLimit_).ToList

                Dim clauses = New List(Of FilterDefinition(Of BsonDocument))

                Dim filterBuilder_ As New FilterDefinitionBuilder(Of BsonDocument)


                clauses.Add(clause1)
                'clauses.Add(clause2)


                'OK
                Dim filterDefinition2 = Builders(Of BsonDocument).Filter.Gte(Of DateTime)("f_FechaModificacion", fromDate)
                'Dim filter = Builders(Of BsonDocument).Filter.Lte("instock.qty", 20)
                listBSONDocuments_ = collectionData_.Find(filterDefinition2).Limit(collectionLimit_).ToList

                'FALLA, por que aparentemente hay Diferencias entre GTE y Gte, uno despacha IMongoQuery y otro QueryWrapper, 
                'Dim filterDefinition3 = Builders(Of BsonDocument).Filter.And(Query.GTE("f_FechaModificacion", fromDate), Query.LTE("f_FechaModificacion", toDate))
                'Dim filter = Builders(Of BsonDocument).Filter.Lte("instock.qty", 20)
                'listBSONDocuments_ = collectionData_.Find(filterDefinition3).Limit(collectionLimit_).ToList


                'listBSONDocuments_ = collectionData_.Find(clause2).Limit(collectionLimit_).ToList




                '**************bloque de pruebas**********************



                'listBSONDocuments_ = collectionData_.Find(matchMembershipDateOperation).Project(projectionDefinition_).Limit(collectionLimit_).ToList

            Else

                listBSONDocuments_ = collectionData_.Find(queryDocument_).Limit(collectionLimit_).ToList

            End If

            Return listBSONDocuments_

        End Function

        Public Overloads Function EjecutaSentencia(ByVal collectionName_ As String,
                                                   ByVal bsonDocument_ As BsonDocument,
                                                   ByVal tipoOperacion_ As IOperacionesCatalogo.TiposOperacionSQL) As Boolean Implements IConexionesNoSQL.EjecutaSentencia

            '-----------------------------------------------------------------------------------

            Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim collectionData_ = GetMongoCollection(Of BsonDocument)(resourceName_:=collectionName_)

            '----------------------------------------------------

            Select Case tipoOperacion_

                Case IOperacionesCatalogo.TiposOperacionSQL.InsertOne

                    'Dim cliente_ = New MongoClient(_cadenaconexion)

                    'Dim database_ = cliente_.GetDatabase(NombreBaseDatos)

                    'Dim collection_ = database_.GetCollection(Of BsonDocument)(collectionName_)

                    collectionData_.InsertOne(bsonDocument_)


                Case IOperacionesCatalogo.TiposOperacionSQL.InsertOneAsync

                    'Dim cliente_ = New MongoClient(_cadenaconexion)

                    'Dim database_ = cliente_.GetDatabase(NombreBaseDatos)

                    'Dim collection_ = database_.GetCollection(Of BsonDocument)(collectionName_)

                    collectionData_.InsertOneAsync(bsonDocument_)

                Case Else

                    ' Not implemented

            End Select

            Return True

        End Function

        Public Overloads Function EjecutaSentencia(ByVal collectionName_ As String,
                                                   ByVal bsonDocument_ As List(Of BsonDocument),
                                                   ByVal tipoOperacion_ As IOperacionesCatalogo.TiposOperacionSQL) As Boolean Implements IConexionesNoSQL.EjecutaSentencia



            '-----------------------------------------------------------------------------------

            Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim collectionData_ = GetMongoCollection(Of BsonDocument)(resourceName_:=collectionName_)

            '----------------------------------------------------

            Select Case tipoOperacion_

                Case IOperacionesCatalogo.TiposOperacionSQL.InsertMany

                    ' Not implemented

                Case IOperacionesCatalogo.TiposOperacionSQL.InsertManyAsync

                    'Dim cliente_ = New MongoClient(_cadenaconexion)

                    'Dim database_ = cliente_.GetDatabase(NombreBaseDatos)

                    'Dim collection_ = database_.GetCollection(Of BsonDocument)(collectionName_)

                    collectionData_.InsertManyAsync(bsonDocument_)

                Case Else

                    ' Not implemented

            End Select

            Return True

        End Function

        Public Overloads Async Function EjecutaSentencia(ByVal collectionName_ As String,
                                                         ByVal bsonDocument_ As BsonDocument,
                                                         ByVal tipoOperacion_ As IOperacionesCatalogo.TiposOperacionSQL,
                                                         ByVal apuntador_ As Dictionary(Of String, Object)) As Task(Of Boolean) Implements IConexionesNoSQL.EjecutaSentencia


            '-----------------------------------------------------------------------------------

            Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim collectionData_ = GetMongoCollection(Of BsonDocument)(resourceName_:=collectionName_)

            '----------------------------------------------------

            Select Case tipoOperacion_

                Case IOperacionesCatalogo.TiposOperacionSQL.UpdateOne

                    ' Not implemented

                Case IOperacionesCatalogo.TiposOperacionSQL.UpdateMany

                    ' Not implemented

                Case IOperacionesCatalogo.TiposOperacionSQL.UpdateOneAsync

                    'Dim cliente_ = New MongoClient(_cadenaconexion)

                    'Dim database_ = cliente_.GetDatabase(NombreBaseDatos)

                    'Dim col = database_.GetCollection(Of BsonDocument)(collectionName_)


                    Dim llave_ As String = apuntador_.Item("Llave")

                    Dim valor_ As String = apuntador_.Item("Valor")

                    Dim filtro_ = Builders(Of BsonDocument).Filter.Eq(Of Integer)(llave_, valor_)

                    Dim updoneresult = Await collectionData_.UpdateOneAsync(filtro_, New BsonDocument("$set", bsonDocument_))

                Case IOperacionesCatalogo.TiposOperacionSQL.UpdateManyAsync

                    'Dim cliente_ = New MongoClient(_cadenaconexion)

                    'Dim database_ = cliente_.GetDatabase(NombreBaseDatos)

                    'Dim col_ = database_.GetCollection(Of BsonDocument)(collectionName_)

                    Dim llave_ As String = apuntador_.Item("Llave")

                    Dim valores_ As String = apuntador_.Item("Valor")

                    Dim indices_ = New BsonArray

                    If valores_.Contains(",") Then

                        valores_.Split(",").Select(
                            Function(x)

                                indices_.Add(Convert.ToInt64(x))

                                Return True

                            End Function
                            ).ToList()

                    Else

                        indices_.Add(New BsonInt64(Convert.ToInt32(valores_)))

                    End If

                    Dim filtro_ As New BsonDocument

                    Dim sentencia_ As New BsonDocument

                    sentencia_.Add("$in", indices_)

                    filtro_.Add(llave_, sentencia_)

                    Dim updoneresult = Await collectionData_.UpdateManyAsync(filtro_, New BsonDocument("$set", bsonDocument_))

                    ' JCCS - 2021-10-28 - Este segmento es una forma de hacerlo por la piedras, se deja comentado para que en el futuro recordar que se pued hacer por string
                    'Dim filtro_ = Builders(Of BsonDocument).Filter.Or(
                    '    Builders(Of BsonDocument).Filter.Eq(Of Integer)("i_Cve_MaestroOperaciones", 507179),
                    '    Builders(Of BsonDocument).Filter.Eq(Of Integer)("i_Cve_MaestroOperaciones", 507180),
                    '    Builders(Of BsonDocument).Filter.Eq(Of Integer)("i_Cve_MaestroOperaciones", 507181),
                    '    Builders(Of BsonDocument).Filter.Eq(Of Integer)("i_Cve_MaestroOperaciones", 507182),
                    '    Builders(Of BsonDocument).Filter.Eq(Of Integer)("i_Cve_MaestroOperaciones", 507183)
                    ')

                    'Dim updoneresult = Await col_.UpdateManyAsync("{i_Cve_MaestroOperaciones: {$in: [507179, 507180, 507181, 507182, 507183]}}", New BsonDocument("$set", bsonDocument_))

                Case Else

                    ' Not implemented

            End Select

            Return True

        End Function

        Public Overrides Function EjecutaSentencia(ByVal _sentencia As String) As Boolean Implements IConexiones.EjecutaSentencia

            'Dim cliente_ = New MongoClient(_cadenaconexion)

            'Dim databaseName_ = cliente_.GetDatabase(NombreBaseDatos)

            'Dim collection = databaseName_.GetCollection(Of BsonDocument)("EstudianteC")

            '-----------------------------------------------------------------------------------

            Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim collection = GetMongoCollection(Of BsonDocument)(resourceName_:="EstudianteC")

            '----------------------------------------------------

            Dim keys_ As BsonDocument = New BsonDocument()

            keys_.Add("Nombre", 1, True)

            Dim indexKeys_ As IMongoIndexKeys = New IndexKeysDocument(keys_)

            Dim indexOptions_ As IndexOptionsDocument = New IndexOptionsDocument("unique", 1)

            'collection.CreateIndex(indexKeys_, indexOptions_)
            'collection.CreateIndex(indexKeys_, indexOptions_)


            'Dim claveEstudiante_ As ObjectId = New ObjectId

            Dim claveEstudiante_ As ObjectId = ObjectId.GenerateNewId

            Dim claveReferencia_ As ObjectId = ObjectId.GenerateNewId

            Dim referencia1_ As BsonDocument = New BsonDocument() _
            .Add("_id", claveReferencia_) _
            .Add("i_ID", claveEstudiante_) _
            .Add("Contacto", "Natalia Mendez") _
            .Add("Familiar", "Sí") _
            .Add("Fecha", 1981)

            'BsonValue.Create(claveReferencia_)) _
            Dim referencia2_ As BsonDocument = New BsonDocument() _
            .Add("_id", claveReferencia_) _
            .Add("i_ID", claveEstudiante_) _
            .Add("Contacto", "Bertholdo Ortiz") _
            .Add("Familiar", "Sí") _
            .Add("Fecha", 1996)

            'Dim referencias_ As New BsonArray()
            'referencias_.Add("Contacto", "Natalia Mendez") _
            '.Add("Familiar", "Sí") _
            '.Add("Fecha", 1981)

            'Dim a4 As BsonArray = New BsonArray(New Integer() {65, 66})

            'Dim referenciasx_ As BsonArray = New BsonArray({
            'New BsonDocument({"author", "jim"}),
            'New BsonDocument({"author", "jim"})
            '})

            Dim referencias_ As BsonArray = New BsonArray({referencia1_, referencia2_})

            'Dim claveEstudiante_ As ObjectId = ObjectId.GenerateNewId

            'Estudiante nuevo
            Dim estudiante_ As BsonDocument = New BsonDocument() _
            .Add("_id", BsonValue.Create(claveEstudiante_)) _
            .Add("Nombre", "") _
            .Add("Edad", 39) _
            .Add("Año", 1981) _
            .Add("Refs", "refs")


            'estudiante_.Aggregate(referencia1_.Values)

            '.Add("contactos", referencia1_, True)


            '.Add(referencia2_)



            collection.InsertOne(estudiante_)

            'MsgBox("Se inserto el ID:" & BsonValue.Create(claveEstudiante_).ToString)
            MsgBox("Se inserto el ID:" & claveEstudiante_.ToString)


            Dim query = New QueryDocument()

            'Dim query = New BsonDocument()


            For Each item As BsonDocument In collection.Find(query).ToList
                Dim id As BsonElement = item.GetElement("_id")
                Dim nombre As BsonElement = item.GetElement("Nombre")
                Dim edad As BsonElement = item.GetElement("Edad")
                Dim anio As BsonElement = item.GetElement("Año")
                'Dim title = item.GetElement("title")

                'MsgBox("Author: 0" & author.Value.ToString & "," & title.Value.ToString)

                'TextBox1.Text = TextBox1.Text & vbCrLf &
                '                "Id: " & id.Value.ToString & " nombre:" & nombre.Value.ToString & " edad:" & edad.Value.ToString & " año:" & anio.Value.ToString

                'For Each element As BsonElement In item.Elements

                '    ' MsgBox("element.Name.ToString=" & element.Name.ToString & "  element.Value=" & element.Value.ToString)

                'Next

            Next

            '####################################

        End Function

        Public Function EjecutaSentenciaIndividual(_sentencia As String) As Boolean Implements IConexiones.EjecutaSentenciaIndividual

        End Function

        Public Function EjecutaConsultaDirectaEstandarizada(_sentencia As String) As Wma.Exceptions.TagWatcher Implements IConexiones.EjecutaConsultaDirectaEstandarizada

        End Function

        Public Function EjecutaConsultaDirectaEstandarizadaDataTable(datadapter_ As IDbDataAdapter, command_ As IDbCommand, _sentencia As String) As Wma.Exceptions.TagWatcher Implements IConexiones.EjecutaConsultaDirectaEstandarizadaDataTable

        End Function

        Public Function EjecutaConsultaDirectaEstandarizadaOpenClose(_sentencia As String) As Wma.Exceptions.TagWatcher Implements IConexiones.EjecutaConsultaDirectaEstandarizadaOpenClose

        End Function

        Public Function EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable(_sentencia As String) As Wma.Exceptions.TagWatcher Implements IConexiones.EjecutaConsultaDirectaEstandarizadaOpenCloseDataTable

        End Function


#Region "ConectionData"

#End Region

#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean ' Para detectar llamadas redundantes

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: eliminar estado administrado (objetos administrados).
                End If

                ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
                ' TODO: Establecer campos grandes como Null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: invalidar Finalize() sólo si la instrucción Dispose(ByVal disposing As Boolean) anterior tiene código para liberar recursos no administrados.
        'Protected Overrides Sub Finalize()
        '    ' No cambie este código. Ponga el código de limpieza en la instrucción Dispose(ByVal disposing As Boolean) anterior.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Visual Basic agregó este código para implementar correctamente el modelo descartable.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' No cambie este código. Coloque el código de limpieza en Dispose(disposing As Boolean).
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region

    End Class

End Namespace
