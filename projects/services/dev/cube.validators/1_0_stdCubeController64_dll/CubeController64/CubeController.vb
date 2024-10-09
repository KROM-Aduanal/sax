Imports System.IO
Imports System.Linq.Expressions
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.ServiceModel
Imports System.Web.Security
Imports Cube.Interpreters
Imports Cube.ValidatorReport
Imports gsol.krom
Imports Microsoft.VisualBasic.ApplicationServices
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports MongoDB.Driver.Core.WireProtocol
Imports MongoDB.Driver.Linq
Imports Sax
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher

Public Class CubeController
    Implements ICubeController, ICloneable, IDisposable

#Region "Attrubutes"

    Private _scope As List(Of ICubeController.CubeSlices)

    Private _rooms As List(Of room)

    Private _roomsResource As List(Of roomresource)

    Private _status As TagWatcher

    Private _reports As ValidatorReport

    Private _interpreter As IMathematicalInterpreter

    Private _fieldmiss As List(Of String)

    Private _rolids As Dictionary(Of ICubeController.CubeSlices, ICubeController.RootCube)

    Private _limit

#End Region

#Region "Properties"

    Public Property interpreter As IMathematicalInterpreter Implements ICubeController.interpreter

        Get
            Return _interpreter

        End Get

        Set(value_ As IMathematicalInterpreter)

            _interpreter = value_

        End Set

    End Property

    Public Property rooms As List(Of room) Implements ICubeController.rooms

        Get
            Return _rooms

        End Get

        Set(value_ As List(Of room))

            _rooms = value_

        End Set

    End Property

    Public ReadOnly Property status As TagWatcher Implements ICubeController.status

        Get

            Return _status

        End Get

    End Property

    Public ReadOnly Property reports As ValidatorReport Implements ICubeController.reports

        Get

            Return _reports

        End Get

    End Property

    Public Property scope As List(Of ICubeController.CubeSlices) Implements ICubeController.scope

        Get
            scope = _scope

        End Get
        Set(value_ As List(Of ICubeController.CubeSlices))

            _scope = value_

        End Set

    End Property

    Public ReadOnly Property fieldmiss As List(Of String) Implements ICubeController.fieldmiss

        Get
            fieldmiss = _fieldmiss

        End Get

    End Property

    Public Property limit As Int32 Implements ICubeController.limit

        Get
            Return _limit

        End Get

        Set(value_ As Int32)

            _limit = value_

        End Set

    End Property

#End Region

#Region "Methods"

    Sub New()

        _limit = 15

        _rolids = New Dictionary(Of ICubeController.CubeSlices, ICubeController.RootCube) _
                                From {{ICubeController.CubeSlices.A22, ICubeController.RootCube.RoomsA22},
                                      {ICubeController.CubeSlices.VOCE, ICubeController.RootCube.RoomsVOCE},
                                      {ICubeController.CubeSlices.UAA, ICubeController.RootCube.RoomsUAA},
                                      {ICubeController.CubeSlices.UCAA, ICubeController.RootCube.RoomsUCAA},
                                      {ICubeController.CubeSlices.UCC, ICubeController.RootCube.RoomsUCC},
                                      {ICubeController.CubeSlices.CDI, ICubeController.RootCube.RoomsCDI},
                                      {ICubeController.CubeSlices.PREV, ICubeController.RootCube.RoomsPREV}}

        _interpreter = New MathematicalInterpreterNCalc

        GetFieldsNamesResource()

    End Sub

    Public Sub New(cloneReadOnly_ As CubeController)

        _status = cloneReadOnly_.status

        _fieldmiss = cloneReadOnly_.fieldmiss

        _reports = CType(cloneReadOnly_.reports.Clone(), ValidatorReport)

    End Sub


#Region "IDisposable Support"

    Private disposedValue As Boolean ' Para detectar llamadas redundantes




    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not Me.disposedValue Then

            If disposing Then

                ' TODO: eliminar estado administrado (objetos administrados).

            End If

            'PONES LAS PROPIEDADES DE TU CLASE EN VACÏO

            With Me

                ._fieldmiss = Nothing

                ._interpreter.Dispose()

                ._reports = Nothing

                ._rooms = Nothing

                ._scope = Nothing

                ._status = Nothing

                ._roomsResource = Nothing

            End With

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

        ' ESTO SERÏA LO QUE PONDRÏAS EN TU DISPOSE PERSONALIZADO

        Dispose(True)

        GC.SuppressFinalize(Me)

    End Sub

#End Region

    Private Function FindSignatureRule(contextList_ As List(Of roomcontext), rules_ As String, firm_ As String) As String

        Dim ruleList_ = contextList_.Find(Function(ch) ch.firmacontext = firm_ And ch.firmacontext <> "")

        If ruleList_ Is Nothing Then

            Return rules_

        Else

            Return ruleList_.rules

        End If

    End Function

    Private Function GetFieldsNamesResource() As TagWatcher

        Dim validFields_ As New List(Of String)

        _fieldmiss = New List(Of String)

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(ICubeController.EnlaceSax.Cube)

            Dim operationsvalidfields_ = _enlaceDatos.GetMongoCollectionByRootId(Of validfields)(ICubeController.RootCube.ValidFields)

            operationsvalidfields_.
                          Aggregate.
                          ToList.ForEach(Sub(field_)

                                             validFields_.Add(field_.valorpresentacion)

                                             If field_.status = "on" Then

                                                 If _fieldmiss.IndexOf(field_.sectionfield) = -1 Then

                                                     _fieldmiss.Add(field_.sectionfield)

                                                 End If

                                             End If

                                         End Sub)

        End Using

        _interpreter.SetValidFields(validFields_)

        _status = New TagWatcher() With {.ObjectReturned = validFields_}

        _status.SetOK()

        _interpreter.addOperands(GetOperands().ObjectReturned)

        Return _status

    End Function

    Private Function GetOperands(Optional firm_ As String = Nothing) As TagWatcher

        Dim operands_ = New List(Of String)

        For Each rolId_ In _rolids.Keys

            Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(ICubeController.EnlaceSax.Cube)

                _enlaceDatos.GetMongoCollectionByRootId(Of room)(_rolids(rolId_)).Aggregate.
                              Match(Function(s) s.contenttype.Equals(ICubeController.ContentTypes.Operando.ToString.ToLower)).ToList.
                              ForEach(Sub(room_)

                                          operands_.AddRange(New List(Of String) From {room_.roomname.Substring(room_.roomname.IndexOf(".") + 1), FindSignatureRule(room_.addresses, room_.rules, If(firm_, "")).Replace("[13]", Chr(13))})

                                      End Sub)

            End Using

        Next

        _status = New TagWatcher() With {.ObjectReturned = operands_}

        If operands_.Count = 0 Then

            _status.SetOKBut(operands_, "No se encontraron operandos")

        Else

            _status.SetOK()

        End If

        Return _status

    End Function

    Private Sub InsertRule(roomCurrent_ As room,
                           roomResource_ As roomresource,
                           operationsDB_ As IMongoCollection(Of room),
                           enlaceDatos_ As IEnlaceDatos)

        roomCurrent_.historical = New List(Of roomhistory) From
                                  {
                                        New roomhistory With
                                        {
                                              ._id = ObjectId.GenerateNewId,
                                              .rules = roomCurrent_.rules,
                                              .description = roomCurrent_.description,
                                              .roomname = roomCurrent_.roomname,
                                              .addresses = roomCurrent_.addresses,
                                              .messages = roomCurrent_.messages,
                                              .status = roomCurrent_.status,
                                              .contenttype = roomCurrent_.contenttype,
                                              .username = roomResource_.username,
                                              .createat = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                              .reason = "ALTA"
                                        }
                                   }

        Dim updateDefinition_ = LoadUpdateDefinition(Of room)(roomCurrent_)

        Dim operartionResult_ = operationsDB_.UpdateOne(Function(e) e._id = roomCurrent_._id,
                                updateDefinition_,
                                New UpdateOptions With {.IsUpsert = True})

        If operartionResult_.UpsertedId Is Nothing Then

            _status.SetError("Error al insertar en la base de datos")

        Else

            Dim operationsDBResource_ = enlaceDatos_.GetMongoCollectionByRootId(Of roomresource)(ICubeController.RootCube.RoomNames)

            Dim updateDefinitionResource_ = LoadUpdateDefinition(Of roomresource)(roomResource_)

            operartionResult_ = operationsDBResource_.UpdateOne(Function(e) e.idroom = roomResource_.idroom,
                                            updateDefinitionResource_,
                                            New UpdateOptions With {.IsUpsert = True})

            If operartionResult_.UpsertedId Is Nothing Then

                _status.SetError("Error al insertar a la base de datos")

            Else

                roomCurrent_.historical(0).createat = DateTime.Now

                Dim operationsvalidfields_ = enlaceDatos_.GetMongoCollectionByRootId(Of validfields)(ICubeController.RootCube.ValidFields)

                operationsvalidfields_.
                UpdateMany(Function(e) e.sectionfield.Equals(roomResource_.valorpresentacion.ToUpper),
                             Builders(Of validfields).Update.Set(Function(ch) ch.status, "off"))

            End If

        End If

    End Sub

    Private Function LoadUpdateDefinition(Of T)(room_ As T, Optional ByVal awaitingaproval_ As Boolean = False) As UpdateDefinition(Of T)

        If GetType(T) Is GetType(room) Then

            Dim roomCurrent_ As room

            Dim formatter_ As New BinaryFormatter()

            Dim stream_ As New MemoryStream()

            formatter_.Serialize(stream_, room_)

            stream_.Position = 0

            roomCurrent_ = formatter_.Deserialize(stream_)

            With roomCurrent_

                Return Builders(Of T).
                                    Update.
                                    Set(Of String)("roomname",
                                                    .roomname.ToUpper).
                                    Set(Of String)("rules",
                                                    .rules).
                                    Set(Of String)("description",
                                                    .description).
                                    Set(Of Boolean)("required",
                                                    .required).
                                    Set(Of List(Of String))("fieldsrequired",
                                                    .fieldsrequired).
                                    Set(Of String)("usetype",
                                                    .usetype).
                                    Set(Of List(Of roomcontext))("addresses",
                                                    .addresses).
                                    Set(Of String)("status",
                                                    .status).
                                    Set(Of List(Of String))("messages",
                                                    .messages).
                                    Set(Of String)("contenttype",
                                                    .contenttype).
                                    Set(Of List(Of roomhistory))("awaitingupdates",
                                                    .awaitingupdates).
                                    Set(Of List(Of roomhistory))("historical",
                                                    .historical)

            End With



        Else

            Dim roomResource_ As roomresource

            Dim formatter_ As New BinaryFormatter()

            Dim stream_ As New MemoryStream()

            formatter_.Serialize(stream_, room_)

            stream_.Position = 0

            roomResource_ = formatter_.Deserialize(stream_)

            With roomResource_

                Return Builders(Of T).
                                            Update.
                                            Set(Of String)("roomname",
                                                            .roomname.ToUpper).
                                            Set(Of String)("description",
                                                            .description).
                                            Set(Of String)("status",
                                                            .status).
                                            Set(Of String)("contenttype",
                                                            .contenttype).
                                            Set(Of Date)("createat",
                                                            .createat).
                                            Set(Of Integer)("rolid",
                                                            .rolid).
                                            Set(Of String)("branchname",
                                                            .branchname).
                                            Set(Of ObjectId)("idroom",
                                                            .idroom).
                                            Set(Of String)("username",
                                                            .username).
                                            Set(Of String)("valorpresentacion",
                                                            .valorpresentacion.ToUpper).
                                            Set(Of Boolean)("awaitingapproval",
                                                            awaitingaproval_).
                                            Set(Of String)("usetype",
                                                            .usetype)

            End With

        End If

    End Function

    Private Sub RunRules(Of T)(rules_ As String,
                               params_ As Dictionary(Of String, T),
                               Optional preferIndex_ As Int32? = Nothing)

        If _interpreter Is Nothing Then

            _interpreter = New MathematicalInterpreterNCalc

            GetFieldsNamesResource()

        End If

        If preferIndex_ Is Nothing Then

            _status = New TagWatcher() With {.ObjectReturned = _interpreter.RunExpression(Of T)(rules_, params_)}

            _reports.resultTagWatcher = _status

        Else

            _status = New TagWatcher() With {.ObjectReturned = _interpreter.RunExpression(Of T)(rules_, params_, preferIndex_)}

            _reports.resultTagWatcher = _status

        End If

        _status.SetOK()

        If TypeOf _status.ObjectReturned Is Dictionary(Of String, List(Of String)) Then

            _reports.result = New List(Of String)

            For Each key_ In _status.ObjectReturned.Keys


                Dim stringJoin_ = ""

                For Each elementList In status.ObjectReturned(key_)

                    stringJoin_ &= elementList & ","

                Next

                _reports.result.Add("[" & key_ & "|" & stringJoin_ & "]")

            Next

        Else

            If TypeOf _status.ObjectReturned Is Dictionary(Of String, String) Then

                _reports.result = New List(Of String)

                For Each key_ In _status.ObjectReturned.Keys

                    _reports.result.Add("[" & key_ & "|" & _status.ObjectReturned(key_) & "]")

                Next

            Else

                _reports.result = New List(Of String)

                If TypeOf _status.ObjectReturned Is List(Of String) Then

                    _reports.result = _status.ObjectReturned

                Else

                    _reports.result.Add(_status.ObjectReturned)

                End If

            End If

        End If

    End Sub

    Private Function SetRoomContext(params_ As List(Of String)) As List(Of roomcontext)

        Return New List(Of roomcontext) From {
                                                    New roomcontext With
                                                    {
                                                         ._idcontext = ObjectId.GenerateNewId,
                                                         .context = "home",
                                                         .firmacontext = Nothing,
                                                         .rules = Nothing,
                                                         .ref = Nothing,
                                                         .loc = New List(Of Int32) From {12, 1005},
                                                         .cached = Nothing,
                                                         .result = Nothing,
                                                         .status = Nothing,
                                                         .timelife = Nothing
                                                    },
                                                    New roomcontext With
                                                    {
                                                         ._idcontext = ObjectId.GenerateNewId,
                                                         .context = "resourcerequired",
                                                         .firmacontext = Nothing,
                                                         .rules = Nothing,
                                                         .ref = params_,
                                                         .loc = Nothing,
                                                         .cached = Nothing,
                                                         .result = Nothing,
                                                         .status = Nothing,
                                                         .timelife = Nothing
                                                    }
        }

    End Function

    Private Sub SetValuesRooms(ByRef rooms_ As room,
                               ByRef roomCurrent_ As room,
                               ByRef roomHistoryList_ As List(Of roomhistory),
                               ByRef roomAwait_ As List(Of roomhistory),
                               ByRef valorPresentacion_ As String,
                               ByRef newObjectId_ As ObjectId,
                               roomRules_ As String,
                               cubeSliceType_ As String,
                               roomName_ As String,
                               descriptionRules_ As String,
                               params_ As List(Of String),
                               messages_ As List(Of String),
                               status_ As String,
                               contenttype_ As String,
                               reason_ As String,
                               idUser_ As ObjectId,
                               userName_ As String,
                               enviado_ As String,
                               useType_ As String)

        Dim roomHistory_ = New roomhistory With {._id = ObjectId.GenerateNewId,
                                                  .rules = roomRules_,
                                                  .roomname = cubeSliceType_ &
                                                              "." &
                                                              roomName_.ToUpper.Replace(" ", "").
                                                                                Replace(Chr(160), "").
                                                                                Replace(Chr(13), "").
                                                                                Replace(Chr(10), ""),
                                                  .description = descriptionRules_,
                                                  .addresses = SetRoomContext(params_),
                                                  .messages = messages_,
                                                  .status = status_,
                                                  .contenttype = contenttype_.ToLower,
                                                  .reason = reason_,
                                                  .createat = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                                  .usetype = useType_
        }

        If idUser_ <> Nothing Then

            roomHistory_._iduser = idUser_

        End If

        If userName_ <> "" Then

            roomHistory_.username = userName_

        End If

        If rooms_.historical Is Nothing Then

            roomHistoryList_.Add(roomHistory_)

        Else

            If rooms_.historical.Count = 0 Then

                roomHistoryList_.Add(roomHistory_)

            Else

                roomHistoryList_.AddRange(rooms_.historical)

                valorPresentacion_ = rooms_.historical(0).roomname.Substring(rooms_.historical(0).roomname.IndexOf(".") + 1)

                If enviado_ = "on" Then

                    roomHistoryList_.Insert(0, roomHistory_)

                    valorPresentacion_ = roomHistory_.roomname.Substring(roomHistory_.roomname.IndexOf(".") + 1)


                End If

                If valorPresentacion_ = Nothing Then

                    valorPresentacion_ = rooms_.roomname.Substring(rooms_.roomname.IndexOf(".") + 1)

                End If

            End If

        End If

        If rooms_.awaitingupdates Is Nothing Then

            roomAwait_.Add(roomHistory_)

        Else

            roomAwait_.AddRange(rooms_.awaitingupdates)

            roomAwait_.Insert(0, roomHistory_)

        End If

        newObjectId_ = rooms_._id

        roomAwait_(0).status = enviado_

        rooms_.awaitingupdates = roomAwait_

        rooms_.usetype = useType_

        roomCurrent_ = rooms_

    End Sub

    Private Sub UpdateRules(ByRef roomCurrent_ As room,
                            roomHistoryList_ As List(Of roomhistory),
                           operationsDB_ As IMongoCollection(Of room),
                           enlaceDatos_ As IEnlaceDatos,
                            cubeSliceType_ As ICubeController.CubeSlices,
                            valorPresentacion_ As String,
                            enviado_ As String)

        Dim updateRoot_ = False

        Dim awaitingapproval_ = False

        Dim idroom_ As ObjectId

        With roomCurrent_

            If .awaitingupdates IsNot Nothing Then

                If (.awaitingupdates.Count > 0) Then

                    Dim roomTemp_ = .awaitingupdates(0)

                    If enviado_ = "on" Then

                        .roomname = roomTemp_.roomname

                        .rules = roomTemp_.rules

                        .addresses = roomTemp_.addresses

                        .description = roomTemp_.description

                        .messages = roomTemp_.messages

                        .contenttype = roomTemp_.contenttype

                        .awaitingupdates(0).status = "on"

                        .historical = roomHistoryList_

                        .status = "on"

                        .usetype = roomTemp_.usetype

                        updateRoot_ = True

                    End If

                    If enviado_ = "sent" Then

                        awaitingapproval_ = True

                    End If

                End If

            End If

            If .historical Is Nothing Then

                .historical = roomHistoryList_

            Else

                If .historical.Count = 0 Then

                    .historical = roomHistoryList_

                End If

            End If

            If .historical.Count > 3 Then

                .historical.RemoveRange(3, .historical.Count - 3)

            End If

            If .awaitingupdates.Count > 3 Then

                .awaitingupdates.RemoveRange(3, .awaitingupdates.Count - 3)

            End If


            idroom_ = ._id

        End With

        Dim updateDefinition_ = LoadUpdateDefinition(Of room)(roomCurrent_)

        Dim operationResult_ = operationsDB_.UpdateOne(Function(e) e._id = idroom_,
                                    updateDefinition_,
New UpdateOptions With {.IsUpsert = True})

        If operationResult_.ModifiedCount = 0 Then

            _status.SetError("Sin Cambios en ROOM")

        Else

            Dim roomResource_ As New roomresource With {
                                                                ._id = ObjectId.GenerateNewId,
                                                                .roomname = roomCurrent_.roomname,
                                                                .description = roomCurrent_.description,
                                                                .status = .status,
                                                                .contenttype = roomCurrent_.contenttype,
                                                                .createat = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                                                .rolid = cubeSliceType_,
                                                                .branchname = cubeSliceType_.ToString,
                                                                .idroom = roomCurrent_._id,
                                                                .username = roomHistoryList_(0).username,
                                                                .valorpresentacion = valorPresentacion_,
                                                                .usetype = roomCurrent_.usetype
                                                                 }

            Dim operationsDBResource_ = enlaceDatos_.GetMongoCollectionByRootId(Of roomresource)(ICubeController.RootCube.RoomNames)

            Dim updateDefinitionResource_ = LoadUpdateDefinition(Of roomresource)(roomResource_, awaitingapproval_)
            operationResult_ = operationsDBResource_.UpdateOne(Function(e) e.idroom = roomResource_.idroom,
updateDefinitionResource_,
                                            New UpdateOptions With {.IsUpsert = True})

            If operationResult_.ModifiedCount = 0 Then

                _status.SetError("Sin Cambios en RoomResource")
            Else

                roomCurrent_.historical(0).createat = DateTime.Now

                If updateRoot_ Then

                    Dim operationsvalidfields_ = enlaceDatos_.GetMongoCollectionByRootId(Of validfields)(ICubeController.RootCube.ValidFields)
                    operationsvalidfields_.
                        UpdateMany(Function(e) e.sectionfield.Equals(roomResource_.valorpresentacion.ToUpper),
                                     Builders(Of validfields).Update.Set(Function(ch) ch.status, "off"))

                End If

            End If

        End If

    End Sub
    Public Function Clone() As Object Implements ICloneable.Clone, ICubeController.Clone
        Dim cubeClone_ As ICubeController = New CubeController(Me)
        With cubeClone_

            .interpreter = CType(Me.interpreter.Clone(), CubeController)

            .limit = Me.limit

            .rooms = Me.rooms

            .scope = Me.scope

        End With

        Return cubeClone_

    End Function

    Public Function GetFormula(roomname_ As String) As TagWatcher Implements ICubeController.GetFormula

        Dim operation_ As String = ""

        Dim useType_ = ICubeController.UseType.MOTOR

        Dim params_ As New List(Of String)

        Dim splitDot_ = roomname_.IndexOf(".")

        Dim splitFinalDot_ = roomname_.LastIndexOf(".")

        If splitDot_ <> splitFinalDot_ Then

            Dim algo_ = roomname_.Substring(splitFinalDot_ + 1, roomname_.Length - splitFinalDot_ - 2)

            Select Case roomname_.Substring(splitFinalDot_ + 1, roomname_.Length - splitFinalDot_ - 2)

                Case "MC"

                    useType_ = ICubeController.UseType.MOTOR

                Case "VAL"

                    useType_ = ICubeController.UseType.VALIDATION

                Case "AS"

                    useType_ = ICubeController.UseType.ASSISTANCE


            End Select

            roomname_ = roomname_.Substring(1, splitFinalDot_ - 1)

        Else

            roomname_ = roomname_.Substring(1, roomname_.Length - 2)

        End If

        Dim cubename_ = roomname_.Substring(0, splitDot_ - 1)

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(ICubeController.EnlaceSax.Cube)

            _enlaceDatos.GetMongoCollectionByRootId(Of room)([Enum].Parse(GetType(ICubeController.CubeSlices), cubename_, True)).Aggregate.
                              Project(Function(ch) New With {
                                                               ch._id,
                                                               ch.roomname,
                                                               ch.rules,
                                                               ch.usetype,
                                                               Key .parametros = ch.addresses(1).ref}).
                              Match(Function(s) s.roomname.Equals(roomname_) And
                                                s.usetype.Equals(useType_.ToString)).ToList.
                              ForEach(Sub(room_)

                                          operation_ = room_.rules

                                          params_ = room_.parametros

                                      End Sub)

        End Using

        params_.Insert(0, operation_)

        _status = New TagWatcher() With {.ObjectReturned = params_}

        If operation_ <> "" Then

            _status.SetOK()

        Else

            _status.SetOKBut(operation_, "No se encontró la regla")

        End If

        Return _status

    End Function

    Public Function GetReports() As ValidatorReport Implements ICubeController.GetReports

        _reports = New ValidatorReport

        _reports.SetHeaderReport("Reporte Gral. del Cubo:",
                                                 DateTime.Now,
                                                AdviceTypesReport.Information,
                                                 AdviceTypesReport.Information,
                                                "Reporte de las Recámaras:",
                                                "",
                                                "", "", TriggerSourceTypes.Cube)

        Dim reportText_ As String = ""

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(ICubeController.EnlaceSax.Cube)

            For Each rolId_ In _rolids.Keys

                Dim filter_ = Builders(Of room).
                              Filter.
                              And(
                                   Builders(Of room).
                                   Filter.
                                   Regex("roomname",
                                         New BsonRegularExpression(rolId_.ToString(),
                                                                   "i")),
                                   Builders(Of room).
                                   Filter.
                                   Eq(Of String)("status",
                                                 "on"))


                Dim cuentaOn_ = _enlaceDatos.GetMongoCollectionByRootId(Of room)(_rolids(rolId_)).
                                  CountDocuments(filter_)

                filter_ = Builders(Of room).
                              Filter.
                              And(
                                   Builders(Of room).
                                   Filter.
                                   Regex("roomname",
                                         New BsonRegularExpression(rolId_.ToString(),
                                                                   "i")),
                                   Builders(Of room).
                                   Filter.
                                   Eq(Of String)("status",
                                                 "off"))

                Dim cuentaOff_ = _enlaceDatos.GetMongoCollectionByRootId(Of room)(_rolids(rolId_)).
                                  CountDocuments(filter_)

                filter_ = Builders(Of room).
                              Filter.
                              And(
                                   Builders(Of room).
                                   Filter.
                                   Regex("roomname",
                                         New BsonRegularExpression(rolId_.ToString(),
                                                                   "i")),
                                   Builders(Of room).
                                   Filter.
                                   Eq(Of String)("status",
                                                 "off"))


                If cuentaOff_ + cuentaOn_ > 0 Then

                    reportText_ &= "Slice:" &
                                   rolId_.ToString &
                                   Chr(13) &
                                   "ON:" &
                                   cuentaOn_ &
                                   Chr(13) &
                                  "OFF:" &
                                  cuentaOff_ &
                                  Chr(13)

                    If rolId_.ToString = "A22" Then

                        Dim filterValidField_ = Builders(Of validfields).
                                  Filter.
                                  Eq(Of String)("status",
                                                 "on")

                        Dim aggregatePipeline_ = _enlaceDatos.GetMongoCollectionByRootId(Of validfields)(ICubeController.RootCube.ValidFields).
                                                 Aggregate().
                                                 Match(Function(ch) ch.status.Equals("on")).
                                                 Group(Function(ch) ch.sectionfield,
                                                      Function(g) New With {
                                                      Key ._id = g.Key,
                                                       Key .count = g.Count()
                                })

                        Dim resultado_ = aggregatePipeline_.ToList()

                        If resultado_.Count > 0 Then

                            reportText_ &= Chr(13) &
                                           "CamposPedimentoSinFormula:" &
                                           resultado_.Count &
                                           Chr(13)

                        End If

                    End If

                    _reports.AddDetailReport(AdviceTypesReport.Information,
                                         reportText_,
                                         "",
                                         ICubeController.CubeErrorTypes.Undefined)

                End If

                reportText_ = ""

            Next

        End Using

        _reports.ShowMessageError(3)

        Return _reports

    End Function

    Public Function GetReports(roomname_ As String) As ValidatorReport Implements ICubeController.GetReports

        Throw New NotImplementedException()

    End Function

    Public Function GetRoom(idRoom_ As ObjectId, rolId_ As Int32) As TagWatcher Implements ICubeController.GetRoom

        Dim rooms_ As New List(Of room)

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(ICubeController.EnlaceSax.Cube)

            _enlaceDatos.GetMongoCollectionByRootId(Of room)(_rolids(rolId_)).
                                  Aggregate.Match(Function(ch) ch._id = idRoom_).
                                  ToList.
                                  ForEach(Sub(room_)

                                              rooms_.Add(room_)

                                          End Sub)

        End Using

        _status = New TagWatcher With {.ObjectReturned = rooms_}

        If rooms_.Count = 0 Then

            _status.SetOKBut(rooms_, "Recámara no encontrada")

        Else

            _status.SetOK()

        End If

        Return _status

    End Function

    Public Function GetRoomNamesResource(Optional token_ As String = Nothing,
                                  Optional typeSearch_ As ICubeController.TypeSearch = ICubeController.
                                                                                       TypeSearch.
                                                                                       ValorPresentacionBranchName) _
                                                                                       As TagWatcher _
                                  Implements ICubeController.GetRoomNamesResource

        _roomsResource = New List(Of roomresource)

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(ICubeController.EnlaceSax.Cube)

            If token_ Is Nothing Then

                _roomsResource.AddRange(_enlaceDatos.GetMongoCollectionByRootId(Of roomresource)(ICubeController.RootCube.RoomNames).
                                      Aggregate.
                                      Limit(_limit).
                                      ToList)
            Else

                If typeSearch_ = 1 Then

                    _roomsResource.AddRange(_enlaceDatos.GetMongoCollectionByRootId(Of roomresource)(ICubeController.RootCube.RoomNames).
                                     Aggregate.
                                     Match(Function(ch) ch.valorpresentacion.ToUpper.Contains(token_.ToUpper) Or ch.branchname.Equals(token_)).
                                     Limit(_limit).
                                     ToList)

                Else

                    _roomsResource.AddRange(_enlaceDatos.GetMongoCollectionByRootId(Of roomresource)(ICubeController.RootCube.RoomNames).
                                 Aggregate.
                                 Match(token_.Replace("^", "").Replace(" ", "")).
                                 Limit(_limit).
                                 ToList)

                End If

            End If

        End Using

        _status = New TagWatcher() With {.ObjectReturned = _roomsResource}

        _status.SetOK()

        Return _status

    End Function

    Public Function GetRoomNamesResource(awatingaproval_ As Boolean,
                                  Optional token_ As String = Nothing,
                                  Optional typeSearch_ As ICubeController.TypeSearch = ICubeController.
                                                                                       TypeSearch.
                                                                                       ValorPresentacionBranchName) _
                                                                                       As TagWatcher _
                                  Implements ICubeController.GetRoomNamesResource

        _roomsResource = New List(Of roomresource)

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(ICubeController.EnlaceSax.Cube)

            If token_ Is Nothing Then

                _roomsResource.AddRange(_enlaceDatos.GetMongoCollectionByRootId(Of roomresource)(ICubeController.RootCube.RoomNames).
                                      Aggregate.
                                      Match(Function(ch) ch.awaitingapproval = awatingaproval_).
                                      Limit(_limit).
                                      ToList)
            Else

                If typeSearch_ = 1 Then

                    _roomsResource.AddRange(_enlaceDatos.GetMongoCollectionByRootId(Of roomresource)(ICubeController.RootCube.RoomNames).
                                     Aggregate.
                                     Match(Function(ch) (ch.valorpresentacion.ToUpper.Contains(token_.ToUpper) Or ch.branchname.Equals(token_)) And ch.awaitingapproval = awatingaproval_).
                                     Limit(_limit).
                                     ToList)

                Else

                    _roomsResource.AddRange(_enlaceDatos.GetMongoCollectionByRootId(Of roomresource)(ICubeController.RootCube.RoomNames).
                                 Aggregate.
                                 Match(Function(ch) ch.awaitingapproval = awatingaproval_).
                                 Match(token_.Replace("^", "").Replace(" ", "")).
                                 Limit(_limit).
                                 ToList)

                End If

            End If

        End Using

        _status = New TagWatcher() With {.ObjectReturned = _roomsResource}

        _status.SetOK()

        Return _status

    End Function

    Public Function GetSectionsResource(fieldName_ As String) As TagWatcher Implements ICubeController.GetSectionsResource

        Dim sections_ As String = ""

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(ICubeController.EnlaceSax.Cube)

            Dim operationsvalidfields_ = _enlaceDatos.GetMongoCollectionByRootId(Of validfields)(ICubeController.RootCube.ValidFields)

            operationsvalidfields_.
                          Aggregate.
                          Match(Function(ch) ch.sectionfield.Equals(fieldName_)).
                          ToList.ForEach(Sub(campo_)

                                             If sections_.IndexOf(campo_.sectionexcel & ",") = -1 Then

                                                 sections_ &= campo_.sectionexcel & ","

                                             End If

                                         End Sub)

        End Using

        If sections_ <> "" Then

            sections_ = sections_.Substring(0, sections_.Length - 1)

        End If

        _status = New TagWatcher() With {.ObjectReturned = sections_}

        If sections_ = "" Then

            _status.SetOKBut(sections_, "El Campo no se encontró en ninguna sección")

        Else

            _status.SetOK()

        End If

        Return _status

    End Function

    Public Function GetValidFieldsOn(sentence_ As String) As TagWatcher Implements ICubeController.GetValidFieldsOn

        'ESTA FUNCIÓN SIRVE PARA OBTENER AQUELLO CAMPOS QUE AÚN NO HAN SIDO UTILIZADOS

        Dim validfields_ = New List(Of String)

        Dim filtro_ As FilterDefinition(Of validfields) = Builders(Of validfields).Filter.Eq(Of String)("status", "on")

        For Each word_ In sentence_.Split(" ")

            filtro_ = filtro_ And Builders(Of validfields).Filter.Regex("sectionfield", New BsonRegularExpression(word_, "i"))

        Next

        _fieldmiss = New List(Of String)

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(ICubeController.EnlaceSax.Cube)

            _enlaceDatos.GetMongoCollectionByRootId(Of validfields)(ICubeController.RootCube.ValidFields).
                         Aggregate.Match(filtro_).ToList.ForEach(Sub(campos_)

                                                                     If validfields_.IndexOf(campos_.sectionfield) = -1 Then

                                                                         validfields_.Add(campos_.sectionfield)

                                                                     End If

                                                                 End Sub)

        End Using

        _status = New TagWatcher() With {.ObjectReturned = validfields_}

        _status.SetOK()

        Return _status

    End Function

    Public Function CamposExcelMongo(excelFilePath_ As String) As String Implements ICubeController.CamposExcelMongo


        Dim csvData_ As String = System.IO.File.ReadAllText(excelFilePath_)

        Dim lines_ As String() = csvData_.Split(New String() {System.Environment.NewLine},
                                                    StringSplitOptions.RemoveEmptyEntries)

        Dim headers_ As String() = lines_(0).Split(","c)

        Dim result_ As New List(Of Dictionary(Of String, String))

        For fila_ As Integer = 1 To lines_.Length - 1

            Dim dictionaryLines_ As New Dictionary(Of String, String)

            Dim currentLine_ As String() = lines_(fila_).Split(New String() {","}, 3, StringSplitOptions.None)

            For column_ As Integer = 0 To 1

                dictionaryLines_.Add(headers_(column_),
                                         currentLine_(column_))

            Next

            result_.Add(dictionaryLines_)

        Next

        Dim or_ As String = "{$or:["

        For Each elemento_ In result_

            or_ &= "{$and:[{roomname:'" & elemento_("ROOMNAME") & "'},{usetype:'MOTOR'}]},"

        Next

        or_ = or_.Substring(0, or_.Length - 1) & "]}"

        Dim fs As Object
        Dim a As Object
        fs = CreateObject("Scripting.FileSystemObject")
        a = fs.CreateTextFile("C:\ZERG\archivo.txt", True)
        a.WriteLine(or_)
        a.Close()

        ' SetFieldsNamesResourceOff()
        Return ""

    End Function

    Public Function RunAssistance(Of T)(roomname_ As String, params_ As Dictionary(Of String, T)) As TagWatcher Implements ICubeController.RunAssistance

        Dim operacion_ As String = ""

        Dim parametros_ As New List(Of String)

        _roomsResource = New List(Of roomresource)

        _reports = New ValidatorReport

        Dim tagWatcher_ As New TagWatcher

        If roomname_.Contains(".") Then

            If _interpreter Is Nothing Then

                _interpreter = New MathematicalInterpreterNCalc

                GetFieldsNamesResource()

            End If




            Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(ICubeController.EnlaceSax.Cube)

                _roomsResource.AddRange(_enlaceDatos.GetMongoCollectionByRootId(Of roomresource)(ICubeController.
                                                                                                 RootCube.
                                                                                                 RoomNames).
                                                     Aggregate.
                                                     Match(Function(ch) ch.roomname.Equals(roomname_) And
                                                                        ch.usetype.Equals(ICubeController.UseType.ASSISTANCE.ToString)).
                                                     ToList)

                If _roomsResource.Count = 0 Then

                    tagWatcher_.SetError("Recámara no encontrada")

                Else

                    _rooms = New List(Of room)

                    _rooms.AddRange(_enlaceDatos.GetMongoCollectionByRootId(Of room)(_rolids(_roomsResource(0).rolid)).
                                                         Aggregate.
                                                         Match(Function(ch) ch._id = _roomsResource(0).idroom).
                                                         ToList)

                    Dim found_ = True

                    Dim mensaje_ As String = ""

                    For Each param_ In _rooms(0).addresses(1).ref.Skip(1)

                        Dim newParam_ = ""

                        For Each key_ In params_.Keys

                            If key_ = param_ Then

                                If Not params_.ContainsKey(key_ & ".0") Then

                                    newParam_ = key_

                                    found_ = True

                                    Exit For

                                End If

                            Else

                                If key_.Substring(0, key_.LastIndexOf(".")) = param_ Then

                                    found_ = True

                                    Exit For

                                Else

                                    found_ = False

                                End If

                            End If

                        Next

                        If Not found_ Then

                            mensaje_ &= param_ & Chr(13)

                        End If

                        If newParam_ <> "" Then

                            params_(newParam_ & ".0") = params_(newParam_)

                            params_.Remove(newParam_)

                        End If

                    Next

                    _status = New TagWatcher() With {.ObjectReturned = _interpreter.RunExpression(Of T)(_rooms(0).rules, params_)}

                    tagWatcher_ = _status

                    tagWatcher_.SetOK()


                End If

            End Using

        Else

            tagWatcher_.SetError("Recámara no encontrada")

        End If

        Return tagWatcher_

    End Function

    Public Function RunRoom(Of T)(roomname_ As String,
                                  params_ As Dictionary(Of String, T),
                                  Optional useType_ As ICubeController.UseType = ICubeController.UseType.Undefined,
                                  Optional ByRef requieredfields_ As List(Of String) = Nothing,
                                  Optional preferIndex_ As Int32? = Nothing) As ValidatorReport Implements ICubeController.RunRoom

        Dim operacion_ As String = ""

        Dim parametros_ As New List(Of String)

        _roomsResource = New List(Of roomresource)

        _reports = New ValidatorReport

        Dim funcFilter_ = Builders(Of roomresource).Filter.Eq(Of String)("roomname", roomname_)

        If useType_ <> ICubeController.UseType.Undefined Then

            funcFilter_ = Builders(Of roomresource).Filter.And(funcFilter_,
                                                               Builders(Of roomresource).
                                                               Filter.
                                                               Eq(Of String)("usetype",
                                                                             useType_.ToString))
        End If


        If roomname_.Contains(".") Then

            Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(ICubeController.
                                                                 EnlaceSax.
                                                                 Cube)

                _roomsResource.AddRange(_enlaceDatos.
                                        GetMongoCollectionByRootId(Of roomresource)(ICubeController.
                                                                                    RootCube.
                                                                                    RoomNames).
                                        Aggregate.
                                        Match(funcFilter_).
                                        ToList)

                If _roomsResource.Count = 0 Then


                    _reports.SetHeaderReport("Recámara no encontrada",
                                             DateTime.Now,
                                            AdviceTypesReport.Alert,
                                             AdviceTypesReport.Alert,
                                            "La recámara '" & roomname_ & "' No fue Encontrada",
                                            roomname_,
                                            "", "", TriggerSourceTypes.Cube)

                    _reports.ShowMessageError(0)

                Else

                    _rooms = New List(Of room)

                    _rooms.AddRange(_enlaceDatos.GetMongoCollectionByRootId(Of room)(_rolids(_roomsResource(0).rolid)).
                                                         Aggregate.
                                                         Match(Function(ch) ch._id = _roomsResource(0).idroom).
                                                         ToList)

                    Dim found_ = True

                    Dim mensaje_ As String = ""

                    requieredfields_ = New List(Of String)

                    For Each param_ In _rooms(0).addresses(1).ref.Skip(1)

                        Dim newParam_ = ""

                        requieredfields_.Add(param_)

                        For Each key_ In params_.Keys



                            If key_ = param_ Then

                                If Not params_.ContainsKey(key_ & ".0") Then

                                    newParam_ = key_

                                    found_ = True

                                    Exit For

                                End If

                            Else

                                If key_.Substring(0, key_.LastIndexOf(".")) = param_ Then

                                    found_ = True

                                    Exit For

                                Else

                                    found_ = False

                                End If

                            End If

                        Next

                        If Not found_ Then

                            mensaje_ &= param_ & Chr(13)

                        End If

                        If newParam_ <> "" Then

                            params_(newParam_ & ".0") = params_(newParam_)

                            params_.Remove(newParam_)

                        End If

                    Next

                    If found_ Then

                        If preferIndex_ = -1 Then

                            RunRules(Of T)(_rooms(0).rules, params_)

                        Else

                            RunRules(Of T)(_rooms(0).rules, params_, preferIndex_)


                        End If


                        If _rooms(0).messages.Count = 0 Then

                            _reports.messages = New List(Of String) From {"", "", ""}

                        Else

                            _reports.messages = _rooms(0).messages

                        End If

                    Else

                        _reports = New ValidatorReport

                        _reports.SetHeaderReport("Parámetros no encontrados",
                                                 DateTime.Now,
                                                AdviceTypesReport.Alert,
                                                 AdviceTypesReport.Alert,
                                                "No se encontraron los siguientes parámetros:'" & mensaje_,
                                                mensaje_,
                                                "", "", TriggerSourceTypes.Cube)

                        _reports.ShowMessageError(0)

                    End If

                End If

            End Using

        Else

            RunRules(Of T)(roomname_, params_)

        End If

        Return _reports

    End Function

    Public Function RunTransaction(idRoom_ As ObjectId,
                               roomName_ As String,
                               roomRules_ As String,
                               cubeSliceType_ As ICubeController.CubeSlices,
                               contenttype_ As ICubeController.ContentTypes,
                               descriptionRules_ As String,
                               status_ As String,
                               useType_ As ICubeController.UseType,
                               messages_ As List(Of String),
                               Optional idUser_ As ObjectId = Nothing,
                               Optional userName_ As String = Nothing,
                               Optional enviado_ As String = "unsent",
                               Optional reason_ As String = Nothing) As TagWatcher _
                               Implements ICubeController.RunTransaction

        Dim params_ As New List(Of String) From {cubeSliceType_.ToString & "." & roomName_.
                                                 ToUpper.Replace(" ", "").
                                                 Replace(Chr(160), "").
                                                 Replace(Chr(13), "").
                                                 Replace(Chr(10), "")}

        Dim updateRoot_ As Boolean = False

        If roomRules_.IndexOf(".csv") > -1 Then

            contenttype_ = "csv"

        Else

            params_.AddRange(_interpreter.GetParams(roomRules_))

        End If

        If messages_ Is Nothing Then

            messages_ = New List(Of String)

        End If

        Dim outputDatabaseName_ As String = ""

        Dim outputCollectionName_ As String = ""

        Dim valorPresentacion_ As String = roomName_

        Dim awaitingapproval_ As Boolean = False

        userName_ = If(userName_, "")

        reason_ = If(reason_, "")

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos(ICubeController.EnlaceSax.Cube)

            Dim filter_ = Builders(Of room).
                              Filter.
                              Or(
                                    Builders(Of room).
                                    Filter.
                                    And(
                                         Builders(Of room).
                                         Filter.
                                         Eq(Of String)("roomname",
                                                       cubeSliceType_.ToString &
                                                       "." &
                                                       roomName_.
                                                       ToUpper.
                                                       Replace(" ", "").
                                                       Replace(Chr(160), "").
                                                       Replace(Chr(13), "").
                                                       Replace(Chr(10), "")),
                                         Builders(Of room).
                                         Filter.
                                         Eq(Of String)("usetype",
                                                         useType_.ToString)),
                                  Builders(Of room).
                                  Filter.
                                  Eq(Of ObjectId)("_id",
                                                 idRoom_))
            Dim operationsDB_ = enlaceDatos_.
                                GetMongoCollectionByRootId(Of room)(_rolids(cubeSliceType_))

            Dim roomHistoryList_ As New List(Of roomhistory)

            Dim roomawait_ As New List(Of roomhistory)

            Dim newObjectId_ = ObjectId.GenerateNewId

            Dim roomCurrent_ As room = Nothing

            Dim roomfound_ As Boolean = False

            operationsDB_.Aggregate.
                          Match(filter_).
                          ToList.ForEach(Sub(rooms_)

                                             If (rooms_._id <> idRoom_) Then

                                                 roomfound_ = True

                                                 roomCurrent_ = rooms_

                                             Else

                                                 SetValuesRooms(rooms_,
                                                                roomCurrent_,
                                                                roomHistoryList_,
                                                                roomawait_,
                                                                valorPresentacion_,
                                                                newObjectId_,
                                                                roomRules_,
                                                                cubeSliceType_.ToString,
                                                                roomName_,
                                                                descriptionRules_,
                                                                params_,
                                                                messages_,
                                                                status_,
                                                                contenttype_.ToString,
                                                                reason_,
                                                                idUser_,
                                                                userName_,
                                                                enviado_,
                                                                useType_.ToString)

                                             End If


                                         End Sub)

            If roomfound_ Then

                _status = New TagWatcher()

                _status.ObjectReturned = roomCurrent_

                _status.SetError("Ya existe una recámara con ese nombre en ese Gajo")


            Else

                _status = New TagWatcher()

                If roomCurrent_ Is Nothing Then

                    roomCurrent_ = New room With
                                   {
                                       ._id = newObjectId_,
                                       .roomname = cubeSliceType_.ToString & "." & roomName_.ToUpper.Replace(" ", "").Replace(Chr(160), "").Replace(Chr(13), "").Replace(Chr(10), ""),
                                       .rules = roomRules_,
                                       .description = descriptionRules_,
                                       .required = True,
                                       .fieldsrequired = New List(Of String),
                                       .usetype = useType_.ToString,
                                       .addresses = SetRoomContext(params_),
                                      .status = status_,
                                      .messages = messages_,
                                      .contenttype = contenttype_.ToString.ToLower,
                                      .awaitingupdates = roomawait_,
                                      .historical = roomHistoryList_
                    }

                    Dim roomResource_ As New roomresource With {
                                                            ._id = ObjectId.GenerateNewId,
                                                            .roomname = roomCurrent_.roomname,
                                                            .description = roomCurrent_.description,
                                                            .status = roomCurrent_.status,
                                                            .contenttype = roomCurrent_.contenttype,
                                                            .createat = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                                            .rolid = cubeSliceType_,
                                                            .branchname = cubeSliceType_.ToString,
                                                            .idroom = roomCurrent_._id,
                                                            .username = userName_,
                                                            .valorpresentacion = valorPresentacion_,
                                                            .usetype = roomCurrent_.usetype
                    }

                    InsertRule(roomCurrent_,
                               roomResource_,
                               operationsDB_,
                               enlaceDatos_)

                Else

                    UpdateRules(roomCurrent_,
                                roomHistoryList_,
                                operationsDB_,
                                enlaceDatos_,
                                cubeSliceType_,
                                valorPresentacion_,
                                enviado_)

                End If

                If _status.Status <> TypeStatus.Errors Then

                    _status.ObjectReturned = roomCurrent_

                    _status.SetOK()

                End If

            End If

        End Using

        Return _status

    End Function

    Public Function SetValidationPanel(validationPanel_ As validationpanel) As TagWatcher

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos(ICubeController.EnlaceSax.Cube)

            Dim operationsvalidfields_ = _enlaceDatos.GetMongoCollectionByRootId(Of validationpanel)(ICubeController.RootCube.ValidationPanel)

            operationsvalidfields_.InsertOne(validationPanel_)

        End Using

        _status = New TagWatcher() With {.ObjectReturned = validationPanel_}

        _status.SetOK()

        Return _status

    End Function

#End Region

End Class

