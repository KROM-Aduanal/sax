Imports gsol.krom
Imports MongoDB.Driver
Imports Wma.Exceptions
Imports Rec.Globals.Controllers.IControladorInstitucionBancaria.Modalidades
Imports MongoDB.Bson
Imports System.Net.Mime.MediaTypeNames
Imports System.Text.RegularExpressions
Imports System.Collections.Specialized.BitVector32
Imports System.Windows
Imports MongoDB.Driver.Linq.Processors
Imports MongoDB.Bson.Serialization
Imports System.Xml
Imports Newtonsoft.Json

Public Class ControladorInstitucionBancaria
    Implements IControladorInstitucionBancaria, ICloneable, IDisposable


#Region "Atributos"
    Private _conservarBancos As Boolean
#End Region

#Region "Propiedades"
    Property InstitucionesBancarias As List(Of InstitucionBancaria) Implements IControladorInstitucionBancaria.InstitucionesBancarias

    ReadOnly Property Estado As TagWatcher Implements IControladorInstitucionBancaria.Estado

    Property ModalidadTrabajo As IControladorInstitucionBancaria.Modalidades Implements IControladorInstitucionBancaria.ModalidadTrabajo

#End Region

#Region "Constructores"
    Sub New()
        _Estado = New TagWatcher
        ModalidadTrabajo = Interno
        _conservarBancos = True
        _InstitucionesBancarias = New List(Of InstitucionBancaria)
    End Sub
#End Region

#Region "Métodos"

    Public Function NuevoBanco(banco_ As InstitucionBancaria,
                               Optional session_ As IClientSessionHandle = Nothing) As TagWatcher _
                               Implements IControladorInstitucionBancaria.NuevoBanco

        With _Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

                Dim operationsDB_ = iEnlace_.
                                   GetMongoCollection(Of InstitucionBancaria)(banco_.
                                   GetType.Name)
                If session_ Is Nothing Then
                    session_ = operationsDB_.Database.Client.StartSession
                End If

                Dim result_ = operationsDB_.InsertOneAsync(session_, banco_)

                '                '            Dim collection = database.GetCollection(Of TipoCargo)("TipoCargo")

                '                '            Dim result_ = operationsDB_.Aggregate.
                '                'Project(Function(ch) New With {Key .IDS = ch._id, Key .uso = ch.uso, Key ._idempresa = ch._idempresa,
                '                '    Key .cuenta = "NO"
                '                '}).
                '                '            Group(Function(ser) New With {
                '                '    Key ._id = ser.IDS, Key .uso = ser.uso, Key ._idempresa = ser._idempresa
                '                '}, Function(g) New With {
                '                '    .countSI = g.Sum(Function(x) x.cuenta)
                '                '}).
                '                'Match(Function(g) g.countSI >= 0).
                '                'Project(Function(e) New With {
                '                '    Key ._id = banco_._id, Key .uso = banco_.uso, Key ._idempresa = banco_._idempresa}).Merge(Of InstitucionBancaria)(iEnlace_.
                '                '                               GetMongoCollection(Of InstitucionBancaria)((New InstitucionBancaria).
                '                '                               GetType.Name), New MergeStageOptions(Of InstitucionBancaria)).ToList

                '                ' Importar el espacio de nombres MongoDB.Driver


                '                ' Crear una instancia del cliente, el servidor y la base de datos

                '                ' Obtener la colección Reg007InstitucionesBancarias
                '                '   Dim coll As MongoCollection(Of BsonDocument) = db.GetCollection(Of BsonDocument)("Reg007InstitucionesBancarias")

                '                ' Construir la etapa de proyección



                '                '                Dim projectDefinition = New BsonDocument("$project",
                '                '            New BsonDocument(
                '                '                New List(Of BsonElement) From {
                '                '                    New BsonElement("_id", banco_._id),
                '                '                    New BsonElement("_idempresa", banco_._idempresa),
                '                '                    New BsonElement("_idinstitucionbancaria", banco_._idinstitucionbancaria),
                '                '                    New BsonElement("razonsocialespaniol", banco_.razonsocialespaniol),
                '                '                    New BsonElement("_iddomicilio", banco_._iddomicilio),
                '                '                    New BsonElement("domiciliofiscal", banco_.domiciliofiscal),
                '                '                    New BsonElement("cuenta", "NO")
                '                '}
                '                ')
                '                ')

                '                Dim projectDefinition = New BsonDocument("$project",
                '            New BsonDocument(
                '                New List(Of BsonElement) From {
                '                    New BsonElement("_id", banco_._id),
                '                    New BsonElement("cuenta",
                '                                    BsonDocument.Parse("{$cond:{if:{$eq:['$uso.clave','" &
                '                                    banco_.uso.Find(Function(e) e.info = "SAT").clave &
                '                                    "']},then:'SI',else:'NO'}}"))
                '                     }))

                '                Dim projectDefinition2 =
                '            New BsonDocument(
                '                New List(Of BsonElement) From {
                '                    New BsonElement("_id", banco_._id),
                '                    New BsonElement("cuenta", BsonDocument.Parse("{$cond:{if:{$eq:['$uso.clave','" & banco_.uso.Find(Function(e) e.info = "SAT").clave & "']},then:'SI',else:'NO'}}"))
                '}
                ')


                '                Dim projectStage As ProjectionDefinition(Of BsonDocument, BsonDocument) = New JsonProjectionDefinition(Of BsonDocument, BsonDocument)(projectDefinition.ToJson.ToString)

                '                Dim groupDefinition = New BsonDocument("$group",
                '                                          BsonDocument.Parse("{" & "_id:null}").
                '                                          Add("countSI", New BsonDocument("$sum",
                '                                          New BsonDocument("$cond",
                '                                          New BsonArray({
                '                                          BsonDocument.Parse("{" & "$eq:['$cuenta','SI']}"),
                '                                          1,
                '                                          0})))).
                '                                          Add("countNO", New BsonDocument("$sum",
                '                                          New BsonDocument("$cond",
                '                                          New BsonArray({
                '                                          BsonDocument.Parse("{" & "$eq:['$cuenta','NO']}"),
                '                                          1,
                '                                          0})))))

                '                Dim groupDefinition2 =
                '                                          BsonDocument.Parse("{" & "_id:null}").
                '                                          Add("countSI", New BsonDocument("$sum",
                '                                          New BsonDocument("$cond",
                '                                          New BsonArray({
                '                                          BsonDocument.Parse("{" & "$eq:['$cuenta','SI']}"),
                '                                          1,
                '                                          0})))).
                '                                          Add("countNO", New BsonDocument("$sum",
                '                                          New BsonDocument("$cond",
                '                                          New BsonArray({
                '                                          BsonDocument.Parse("{" & "$eq:['$cuenta','NO']}"),
                '                                          1,
                '                                          0}))))

                '                Dim groupStage As ProjectionDefinition(Of InstitucionBancaria, InstitucionBancaria) = New JsonProjectionDefinition(Of InstitucionBancaria, InstitucionBancaria)(groupDefinition.ToJson.ToString)

                '                Dim matchDefinition = New BsonDocument("$match",
                '                                      New BsonDocument("countSI", 0))

                '                Dim matchDefinition2 = New BsonDocument("countSI", 0)

                '                Dim MatchStage As ProjectionDefinition(Of InstitucionBancaria, InstitucionBancaria) = New JsonProjectionDefinition(Of InstitucionBancaria, InstitucionBancaria)(matchDefinition.ToJson.ToString)

                '                Dim project2definition = New BsonDocument("$project",
                '                                         New BsonDocument("_id",
                '                                                          banco_._id).
                '                                         Add("_idempresa",
                '                                             banco_._idempresa).
                '                                         Add("_idinstitucionbancaria",
                '                                             BsonDocument.Parse("{" & "$literal:" & banco_._idinstitucionbancaria & "}")).
                '                                         Add("uso",
                '                                             New BsonArray(ObtieneListaBsonDocument(banco_.uso))).
                '                                         Add("otrosaliasinstitucion",
                '                                             New BsonArray(ObtieneListaBsonDocument(banco_.otrosaliasinstitucion))).
                '                                         Add("razonsocialespaniol",
                '                                             banco_.razonsocialespaniol).
                '                                         Add("_iddomicilio",
                '                                             banco_._iddomicilio).
                '                                         Add("domiciliofiscal",
                '                                             banco_.domiciliofiscal).
                '                                         Add("metadatos",
                '                                             New BsonArray(ObtieneListaBsonDocument(banco_.metadatos))).
                '                                         Add("tipobanco",
                '                                             BsonDocument.Parse("{" & "$literal:" & banco_.tipobanco & "}")).
                '                                         Add("estado",
                '                                             BsonDocument.Parse("{" & "$literal:" & banco_.estado & "}")).
                '                                         Add("archivado",
                '                                             BsonDocument.Parse("{" & "$literal:false}")))


                '                Dim project2definition2 =
                '                                         New BsonDocument("_id", banco_._id).
                '                                         Add("_idempresa", banco_._idempresa).
                '                                         Add("_idinstitucionbancaria", BsonDocument.Parse("{" & "$literal:" & banco_._idinstitucionbancaria & "}")).
                '                                         Add("uso", New BsonArray(ObtieneListaBsonDocument(banco_.uso))).'{BsonDocument.Parse("{" & "clave:'" & banco_.uso(0).clave & "', info:'" & banco_.uso(0).info & "'}")})).
                '                                         Add("otrosaliasinstitucion", New BsonArray(ObtieneListaBsonDocument(banco_.otrosaliasinstitucion))).'{BsonDocument.Parse("{" & "tipoalias:'" & banco_.otrosaliasinstitucion(0).tipoalias & "', valor:'" & banco_.otrosaliasinstitucion(0).valor & "'}")})).
                '                                         Add("razonsocialespaniol", banco_.razonsocialespaniol).
                '                                         Add("_iddomicilio", banco_._iddomicilio).
                '                                         Add("domiciliofiscal", banco_.domiciliofiscal).
                '                                         Add("metadatos", New BsonArray(ObtieneListaBsonDocument(banco_.metadatos))).'{BsonDocument.Parse("{" & "tipoalias:'" & banco_.metadatos(0).tipoalias & "', valor:'" & banco_.metadatos(0).valor & "'}")})).
                '                                         Add("tipobanco", BsonDocument.Parse("{" & "$literal:" & banco_.tipobanco & "}")).
                '                                         Add("estado", BsonDocument.Parse("{" & "$literal:" & banco_.estado & "}")).
                '                                         Add("archivado", BsonDocument.Parse("{" & "$literal:false}"))

                '                Dim project2Stage As ProjectionDefinition(Of InstitucionBancaria, InstitucionBancaria) = New JsonProjectionDefinition(Of InstitucionBancaria, InstitucionBancaria)(project2definition.ToJson.ToString)

                '                Dim mergeDefinition = New BsonDocument("$merge",
                '                                      New BsonDocument("into", "Reg007InstitucionesBancarias").
                '                                      Add("on", "_id").
                '                                      Add("whenMatched", "keepExisting").
                '                                      Add("whenNotMatched", "insert"))

                '                Dim mergeDefinition2 = New BsonDocument("into", "Reg007InstitucionesBancarias").
                '        Add("on", "_id").
                '        Add("whenMatched", "keepExisting").
                '        Add("whenNotMatched", "insert")
                '                'Dim mergeOptions As New MergeIntoOptions()
                '                'mergeOptions.Into = "nueva_coleccion"
                '                'mergeOptions.WhenMatched = WhenMatched.Merge
                '                'mergeOptions.WhenNotMatched = WhenNotMatched.Insert

                '                Dim mergeStage As ProjectionDefinition(Of InstitucionBancaria, InstitucionBancaria) = New JsonProjectionDefinition(Of InstitucionBancaria, InstitucionBancaria)(mergeDefinition.ToJson.ToString)

                '                'pipeline.AppendStage(projectDefinition)
                '                'pipeline.Add(groupDefinition)
                '                'pipeline.Add(matchDefinition)
                '                'pipeline.Add(project2definition)
                '                'pipeline.Add(mergeDefinition)}

                '                Dim pipeline As PipelineDefinition(Of BsonDocument, BsonDocument) = New BsonDocument() {
                '                                                                                    projectDefinition,
                '                                                                                    groupDefinition,
                '                                                                                    matchDefinition,
                '                                                                                    project2definition,
                '                                                                                    mergeDefinition}



                '                Dim pipeline2 As New List(Of PipelineStageDefinition(Of BsonDocument, BsonDocument))

                '                pipeline2.Add(projectDefinition)
                '                pipeline2.Add(groupDefinition)
                '                pipeline2.Add(matchDefinition)
                '                pipeline2.Add(project2definition)

                '                Dim aggregationOptions As New AggregateOptions()

                '                'If aggregationOptions.BatchSize Is Nothing Then
                '                '    MsgBox("FUE NOTHING")
                '                'Else
                '                '    MsgBox(aggregationOptions.BatchSize)
                '                'End If

                '                aggregationOptions.BatchSize = 1
                '                ' MsgBox(aggregationOptions.BatchSize)
                '                'aggregationOptions.
                '                'Dim result_ = operationsDB_.Aggregate(pipeline, aggregationOptions).ToList

                '                ' Obtener una instancia de IMongoCollection(Of TOutput)

                '                '            Dim outputCollection As IMongoCollection(Of BsonDocument) = operationsDB_

                '                '            ' Crear una lista de etapas de agregación
                '                '            Dim pipeline As New List(Of BsonDocument)()

                '                '            ' Agregar etapas a la lista de agregación
                '                '            pipeline.Add(New BsonDocument("$match", New BsonDocument("field1", "value1")))
                '                '            pipeline.Add(New BsonDocument("$group", New BsonDocument("_id", "$field2").Add("count", New BsonDocument("$sum", 1))))

                '                '            ' Crear opciones para la etapa de combinación
                '                '            Dim mergeOptions As New MergeStageOptions(Of BsonDocument) With {
                '                '                .WhenMatched = MergeStageWhenMatched.KeepExisting,
                '                '                .WhenNotMatched = MergeStageWhenNotMatched.Insert
                '                '            }

                '                '            ' Ejecutar la operación Merge
                '                '            Dim cursor As IAsyncCursor(Of MyDocument) = inputCollection.Aggregate().AppendStage(Of BsonDocument)(pipeline).Merge(outputCollection, mergeOptions)

                '                '            ' Iterar sobre los documentos combinados
                '                '            While Await cursor.MoveNextAsync()
                '                'For Each document As MyDocument In cursor.Current
                '                '                    ' Hacer algo con el documento combinado
                '                '                Next
                '                '            End While

                '                Dim matchDefinitionpipe2 = New BsonDocument("$match",
                '                                      New BsonDocument("_id", banco_._id))

                '                Dim pipeline_ As PipelineDefinition(Of BsonDocument, BsonDocument) = New BsonDocument() {project2definition, matchDefinitionpipe2} ', mergeDefinition}

                '                Dim mergeOptions As New MergeStageOptions(Of BsonDocument) With {
                '                                .WhenMatched = MergeStageWhenMatched.KeepExisting,
                '                                .WhenNotMatched = MergeStageWhenNotMatched.Insert
                '                            }

                '                'Dim algo_ = operationsDB_.
                '                'Dim result_ = operationsDB_.Aggregate().AppendStage(Of BsonDocument)(pipeline2(0)).
                '                '                                        AppendStage(Of BsonDocument)(pipeline2(1)).
                '                '                                        AppendStage(Of BsonDocument)(pipeline2(2)).
                '                '                                        AppendStage(Of BsonDocument)(pipeline2(3)).
                '                '                                        Merge(algo_, mergeOptions).ToList '.Merge(Of InstitucionBancaria)(BsonSerializer.Deserialize(Of InstitucionBancaria)(mergeDefinition2)).ToList

                '                'Dim result_ = operationsDB_.Aggregate(aggregationOptions).
                '                '                            Project(projectDefinition2).
                '                '                            Group(groupDefinition2).
                '                '                            Match(matchDefinition2).
                '                '                            Project(project2definition2).
                '                '                            Merge(operationsDB_, mergeOptions).ToList '.Merge(Of InstitucionBancaria)(BsonSerializer.Deserialize(Of InstitucionBancaria)(mergeDefinition2)).ToList

                '                'Dim result_ = operationsDB_.AggregateAsync(pipeline, aggregationOptions).Result.ToList

                '                Dim result_ = operationsDB_.Aggregate(pipeline, aggregationOptions).ToList '.Merge(algo_, mergeOptions).ToList '.Merge(Of InstitucionBancaria)(BsonSerializer.Deserialize(Of InstitucionBancaria)(mergeDefinition2)).ToList

                '                If result_.Count = 0 Then

                '                    .SetOKBut(Me, "Sin Registros")
                '                    .ObjectReturned = Nothing
                '                Else
                '                    .SetOK()
                '                    .ObjectReturned = banco_
                '                End If
                '                '.SetError(Me, "Ya existe un banco con esa clave SAT")
                '                '.ObjectReturned = Nothing

            End Using
        End With

        Return _Estado

    End Function

    Public Function ActualizaBanco(banco_ As InstitucionBancaria,
                                   Optional session_ As IClientSessionHandle = Nothing) As TagWatcher _
                                   Implements IControladorInstitucionBancaria.ActualizaBanco

        With _Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of InstitucionBancaria)((New InstitucionBancaria).GetType.Name)

                Dim definicion_ = Builders(Of InstitucionBancaria).Update.Set(Function(e) e._idinstitucionbancaria,
                                                                              banco_._idinstitucionbancaria).
                                                                          Set(Function(e) e.otrosaliasinstitucion,
                                                                              banco_.otrosaliasinstitucion).
                                                                          Set(Function(e) e.razonsocialespaniol,
                                                                              banco_.razonsocialespaniol).
                                                                          Set(Function(e) e.uso,
                                                                              banco_.uso).
                                                                          Set(Function(e) e.domiciliofiscal,
                                                                              banco_.domiciliofiscal).
                                                                          Set(Function(e) e.tipobanco,
                                                                              banco_.tipobanco).
                                                                          Set(Function(e) e.metadatos,
                                                                              banco_.metadatos).
                                                                          Set(Function(e) e.archivado,
                                                                              banco_.archivado).
                                                                          Set(Function(e) e.estado,
                                                                              banco_.estado)

                Dim result_ = operationsDB_.UpdateOne(Function(e) e._id = banco_._id, definicion_)

                If result_.ModifiedCount Then

                    .SetOK()

                    .ObjectReturned = banco_

                Else

                    .SetError()

                    .ObjectReturned = Nothing

                End If

            End Using

        End With

        Return _Estado

    End Function

    Public Function ActualizaBanco(idBanco As ObjectId,
                                   ByVal tokens_ As Dictionary(Of IControladorInstitucionBancaria.CamposBusquedaSimple, Object),
                                   Optional session_ As IClientSessionHandle = Nothing) As TagWatcher _
        Implements IControladorInstitucionBancaria.ActualizaBanco

        Dim banderaUso_ As Boolean = False

        Dim banderaComercial_ As Boolean = False

        Dim aliasbancos_ As New AliasBancos

        Dim aliasuso_ As New UsoIdentificador

        Dim aliasComercial_ As String = ""

        Dim result_ As UpdateResult

        With _Estado

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

                Dim operationsDB_ = iEnlace_.
                                    GetMongoCollection(Of InstitucionBancaria)((New InstitucionBancaria).
                                    GetType.Name)

                Dim definicion_ As UpdateDefinition(Of InstitucionBancaria) = Nothing

                Dim primero_ As UpdateDefinition(Of InstitucionBancaria) = Nothing

                For Each token_ As KeyValuePair(Of IControladorInstitucionBancaria.CamposBusquedaSimple,
                                                Object) In tokens_

                    Select Case token_.Key

                        Case IControladorInstitucionBancaria.CamposBusquedaSimple.IDS

                            definicion_ = Nothing

                        Case IControladorInstitucionBancaria.CamposBusquedaSimple.IDBANCARIO

                            If definicion_ Is Nothing Then

                                definicion_ = Builders(Of InstitucionBancaria).
                                             Update.
                                             Set(Function(e) e._idinstitucionbancaria,
                                                             token_.Value)

                            Else

                                definicion_ = definicion_.
                                              Set(Function(e) e._idinstitucionbancaria,
                                                              token_.Value)

                            End If

                        Case IControladorInstitucionBancaria.CamposBusquedaSimple.CLAVEUSONUEVO

                            If definicion_ Is Nothing Then

                                definicion_ = Builders(Of InstitucionBancaria).
                                              Update.
                                              Set(Function(e) e.uso,
                                                              token_.Value)

                            Else

                                definicion_ = definicion_.
                                              Set(Function(e) e.uso,
                                                              token_.Value)

                            End If


                        Case IControladorInstitucionBancaria.CamposBusquedaSimple.CLAVEUSOBUSCAACTUALIZA

                            banderaUso_ = True

                            aliasuso_ = token_.Value.Info

                            If definicion_ Is Nothing Then

                                definicion_ = Builders(Of InstitucionBancaria).
                                              Update.
                                              Set(Of String)("uso.$.clave",
                                                             token_.Value.clave)

                            Else

                                definicion_ = definicion_.
                                              Set(Of String)("uso.$.clave",
                                                             token_.Value.clave)

                            End If

                        Case IControladorInstitucionBancaria.CamposBusquedaSimple.NOMBRECOMERCIALNUEVO

                            If definicion_ Is Nothing Then

                                definicion_ = Builders(Of InstitucionBancaria).Update.AddToSet(Of AliasBancos)(Function(e) e.otrosaliasinstitucion, token_.Value)

                            Else
                                definicion_ = definicion_.AddToSet(Of AliasBancos)(Function(e) e.otrosaliasinstitucion, token_.Value)

                            End If

                        Case IControladorInstitucionBancaria.CamposBusquedaSimple.NOMBRECOMERCIALBUSCAACTUALIZA

                            banderaComercial_ = True

                            aliasComercial_ = token_.Value.TipoAlias

                            If definicion_ Is Nothing Then

                                definicion_ = Builders(Of InstitucionBancaria).Update.Set(Of String)("otrosaliasinstitucion.$.valor", token_.Value.valor)

                            Else

                                definicion_ = definicion_.Set(Of String)("otrosaliasinstitucion.$.valor", token_.Value.valor)

                            End If

                        Case IControladorInstitucionBancaria.CamposBusquedaSimple.RAZONSOCIAL

                            If definicion_ Is Nothing Then

                                definicion_ = Builders(Of InstitucionBancaria).Update.Set(Function(e) e.razonsocialespaniol, token_.Value)

                            Else

                                definicion_ = definicion_.Set(Function(e) e.razonsocialespaniol, token_.Value)

                            End If

                        Case IControladorInstitucionBancaria.CamposBusquedaSimple.DOMICILIOFISCAL

                            If definicion_ Is Nothing Then

                                definicion_ = Builders(Of InstitucionBancaria).Update.Set(Function(e) e.domiciliofiscal, token_.Value)

                            Else

                                definicion_ = definicion_.Set(Function(e) e.domiciliofiscal, token_.Value)

                            End If

                        Case IControladorInstitucionBancaria.CamposBusquedaSimple.METADATOS

                            If definicion_ Is Nothing Then

                                definicion_ = Builders(Of InstitucionBancaria).Update.Set(Function(e) e.metadatos, token_.Value)

                            Else

                                definicion_ = definicion_.Set(Function(e) e.razonsocialespaniol, token_.Value)

                            End If

                        Case IControladorInstitucionBancaria.CamposBusquedaSimple.ARCHIVADO

                            If definicion_ Is Nothing Then

                                definicion_ = Builders(Of InstitucionBancaria).Update.Set(Function(e) e.archivado, token_.Value)

                            Else

                                definicion_ = definicion_.Set(Function(e) e.archivado, token_.Value)

                            End If

                        Case IControladorInstitucionBancaria.CamposBusquedaSimple.ESTADO

                            If definicion_ Is Nothing Then

                                definicion_ = Builders(Of InstitucionBancaria).Update.Set(Function(e) e.estado, token_.Value)

                            Else

                                definicion_ = definicion_.Set(Function(e) e.estado, token_.Value)

                            End If

                    End Select

                Next
                Dim filter_ = Builders(Of InstitucionBancaria).Filter.Eq(Function(e) e._id, idBanco)

                If banderaComercial_ Then

                    filter_ = filter_ And
                                 Builders(Of InstitucionBancaria).Filter.
                                 ElemMatch(Function(e) e.otrosaliasinstitucion,
                                           Function(alias2_) alias2_.tipoalias.
                                           Equals(aliasComercial_))
                End If

                If banderaUso_ Then

                    filter_ = filter_ And
                                 Builders(Of InstitucionBancaria).Filter.
                                 ElemMatch(Function(e) e.uso,
                                           Function(alias2_) alias2_.info.
                                           Equals(aliasuso_))
                End If

                result_ = operationsDB_.UpdateOne(filter_, definicion_)


                If result_.ModifiedCount Then

                    .SetOK()

                    .ObjectReturned = True

                Else

                    .SetError()

                    .ObjectReturned = Nothing

                End If

            End Using

        End With

        Return _Estado

    End Function


    Private Function BuscarBancos(ByVal tokens_ As Dictionary(Of IControladorInstitucionBancaria.CamposBusquedaSimple,
                                  Object),
                                  modalidad_ As IControladorInstitucionBancaria.Modalidades) As TagWatcher _
                                  Implements IControladorInstitucionBancaria.BuscarBancos

        Dim definicion_ As FilterDefinition(Of InstitucionBancaria) = Nothing

        _Estado.ObjectReturned = Nothing

        If modalidad_ = Interno Then

        Else

            InstitucionesBancarias = New List(Of InstitucionBancaria)

            For Each token_ As KeyValuePair(Of IControladorInstitucionBancaria.CamposBusquedaSimple,
                                            Object) In tokens_

                Select Case token_.Key

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.IDS

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).
                                          Filter.
                                          Eq(Function(e) e._id,
                                                         token_.Value)

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).
                                                          Filter.
                                                          Eq(Function(e) e._id,
                                                                         token_.Value)

                        End If

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.IDBANCARIO

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).
                                          Filter.
                                          Eq(Function(e) e._idinstitucionbancaria,
                                                         token_.Value)

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).
                                                          Filter.
                                                          Eq(Function(e) e._idinstitucionbancaria,
                                                                         token_.Value)

                        End If


                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.CLAVEUSOBUSCAACTUALIZA

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).
                                          Filter.
                                          ElemMatch(Function(e) e.uso,
                                                    Function(alias_) alias_.clave.
                                                                     Equals(token_.Value.
                                                                            ToString.ToUpper))

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).
                                                          Filter.ElemMatch(Function(e) e.uso,
                                                                           Function(alias_) alias_.clave.
                                                                                            Equals(token_.Value.
                                                                                                   ToString.ToUpper))

                        End If

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.NOMBRECOMERCIALBUSCAACTUALIZA

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).
                                          Filter.ElemMatch(Function(e) e.otrosaliasinstitucion,
                                                           Function(alias_) alias_.valor.
                                                                            Contains(token_.Value.
                                                                                   ToString.ToUpper))

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).
                                                          Filter.ElemMatch(Function(e) e.otrosaliasinstitucion,
                                                                           Function(alias_) alias_.valor.
                                                                                            Contains(token_.Value.
                                                                                                   ToString.ToUpper))

                        End If

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.RAZONSOCIAL

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).
                                          Filter.
                                          Regex(Function(e) e.razonsocialespaniol,
                                                            New BsonRegularExpression(token_.Value,
                                                                                      "i"))

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).
                                                          Filter.
                                                          Regex(Function(e) e.razonsocialespaniol,
                                                                            New BsonRegularExpression(token_.Value,
                                                                                                      "i"))

                        End If

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.METADATOS

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).
                                          Filter.ElemMatch(Function(e) e.otrosaliasinstitucion,
                                                           Function(alias_) alias_.valor.
                                                                            Equals(token_.Value.
                                                                                   ToString.ToUpper))

                            definicion_ = Builders(Of InstitucionBancaria).
                                          Filter.
                                          ElemMatch(Function(e) e.metadatos,
                                                           Function(alias_) alias_.valor.
                                                                            Equals(token_.Value.
                                                                                   ToString.ToUpper))

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).
                                                          Filter.
                                                          ElemMatch(Function(e) e.metadatos,
                                                                    Function(alias_) alias_.valor.
                                                                            Equals(token_.Value.
                                                                                   ToString.ToUpper))

                        End If

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.ARCHIVADO

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).
                                          Filter.
                                          Eq(Function(e) e.archivado,
                                                         token_.Value)

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).
                                                          Filter.
                                                          Eq(Function(e) e.archivado,
                                                                         token_.Value)

                        End If

                    Case IControladorInstitucionBancaria.CamposBusquedaSimple.ESTADO

                        If definicion_ Is Nothing Then

                            definicion_ = Builders(Of InstitucionBancaria).
                                          Filter.
                                          Eq(Function(e) e.estado,
                                                         token_.Value)

                        Else

                            definicion_ = definicion_ And Builders(Of InstitucionBancaria).
                                                          Filter.Eq(Function(e) e.estado,
                                                                                token_.Value)

                        End If

                End Select

            Next

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

                Dim operationsDB_ = iEnlace_.
                                    GetMongoCollection(Of InstitucionBancaria)((New InstitucionBancaria).
                                    GetType.Name)

                operationsDB_.Aggregate.
                              Match(definicion_).
                              ToList.ForEach(Sub(resultado_)

                                                 InstitucionesBancarias.Add(resultado_)

                                             End Sub)

            End Using

            _Estado.ObjectReturned = InstitucionesBancarias


        End If

        If _Estado.ObjectReturned Is Nothing Then

            _Estado.SetError(Me, "Error Conexión")

        Else

            If _Estado.ObjectReturned.Count = 0 Then

                _Estado.SetError(Me, "No se encontraron Instituciones Bancarias")

            Else

                _Estado.SetOK()

            End If

        End If

        Return _Estado

    End Function

    Private Function ObtieneListaBsonDocument(Of T)(lista_ As List(Of T)) As List(Of BsonDocument)
        Dim listaResult_ = New List(Of BsonDocument)
        For Each elemento_ In lista_
            'MsgBox(elemento_.ToJson.ToString)
            Dim bson_ = New BsonDocument(elemento_.ToBsonDocument)
            listaResult_.Add(bson_)
        Next
        Return listaResult_
    End Function

#End Region

#Region "Clon"

    Public Function Clone() As Object Implements ICloneable.Clone

        Dim institucionBancariaClonada_ As IControladorInstitucionBancaria = New ControladorInstitucionBancaria

        With institucionBancariaClonada_

            .InstitucionesBancarias = Me.InstitucionesBancarias

            .Estado.ObjectReturned = Me.Estado.ObjectReturned

            .ModalidadTrabajo = Me.ModalidadTrabajo

        End With

        Return institucionBancariaClonada_

    End Function

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' Para detectar llamadas redundantes

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not Me.disposedValue Then

            If disposing Then
                ' TODO: eliminar estado administrado (objetos administrados).
            End If

            'Propiedades no administradas

            With Me

                .InstitucionesBancarias = Nothing

                .Estado.Clear()

                .ModalidadTrabajo = Nothing

                ._conservarBancos = Nothing


            End With

            ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
            ' TODO: Establecer campos grandes como Null.
        End If

        Me.disposedValue = True

    End Sub


    ' Visual Basic agregó este código para implementar correctamente el modelo descartable.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en Dispose(disposing As Boolean).
        Dispose(True)

        GC.SuppressFinalize(Me)

    End Sub

#End Region

End Class
