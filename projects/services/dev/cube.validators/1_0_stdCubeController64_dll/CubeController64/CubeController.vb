Imports System.Linq.Expressions
Imports Cube.Interpreters
Imports Cube.ValidatorReport
Imports gsol.basededatos
Imports gsol.krom
Imports Microsoft.SqlServer.Server
Imports Microsoft.VisualBasic.ApplicationServices
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
Imports MongoDB.Driver.Linq
Imports Sax
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Utils
Imports Wma.Exceptions

Public Class CubeController
    Implements ICubeController, ICloneable, IDisposable

#Region "Attrubutes"

    Private _scope As List(Of ICubeController.ContainedCubes)

    Private _rooms As List(Of Room)

    Private _roomsResource As List(Of RoomResource)

    Private _status As TagWatcher

    Private _reports As ValidatorReport

    Private _interpreter As IMathematicalInterpreter



#End Region

#Region "Properties"

    Public Property rooms As List(Of Room) Implements ICubeController.rooms

        Get

            Return _rooms

        End Get

        Set(value As List(Of Room))

            _rooms = value

        End Set

    End Property

    Public Property status As TagWatcher Implements ICubeController.status

        Get

            Return _status

        End Get

        Set(value As TagWatcher)

            _status = value

        End Set

    End Property

    Public Property reports As ValidatorReport Implements ICubeController.reports

        Get

            Return _reports

        End Get

        Set(value As ValidatorReport)

            _reports = value

        End Set

    End Property

    Public Property scope As List(Of ICubeController.ContainedCubes) Implements ICubeController.scope

        Get
            scope = _scope

        End Get
        Set(value As List(Of ICubeController.ContainedCubes))

            _scope = value

        End Set

    End Property

#End Region

#Region "Methods"

    Function ActualizaClase(Of T)(Origen As String) As T Implements ICubeController.ActualizaClase

        Dim objetoDeserializado As T = Newtonsoft.Json.JsonConvert.DeserializeObject(Of T)(Origen)

        Return objetoDeserializado

    End Function
    Public Sub Dispose() Implements IDisposable.Dispose
        Throw New NotImplementedException()
    End Sub

    Public Function ValidateFields(Of T)(values_ As Dictionary(Of String, T)) As ValidatorReport Implements ICubeController.ValidateFields
        Throw New NotImplementedException()
    End Function

    Public Function ValidateFields(Of T)(documentoElectronico_ As DocumentoElectronico) As ValidatorReport Implements ICubeController.ValidateFields
        Throw New NotImplementedException()
    End Function

    Public Function SetCsv(roomname_ As String, csvFilePath_ As String) As TagWatcher Implements ICubeController.SetCsv
        Throw New NotImplementedException()
    End Function

    Public Function SetJson(roomname_ As String, csvFilePath_ As String) As TagWatcher Implements ICubeController.SetJson
        Throw New NotImplementedException()
    End Function

    Public Function GetCsv(roomname_ As String, csvFilePath_ As String) As TagWatcher Implements ICubeController.GetCsv
        Throw New NotImplementedException()
    End Function

    Public Function GetJson(roomname_ As String, csvFilePath_ As String) As TagWatcher Implements ICubeController.GetJson
        Throw New NotImplementedException()
    End Function

    Public Function GetFormula(roomname_ As String) As TagWatcher Implements ICubeController.GetFormula



        Dim operacion_ As String = ""

        Dim parametros_ As New List(Of String)

        Dim posicionPunto_ = roomname_.IndexOf(".")

        Dim cubename_ = roomname_.Substring(1, posicionPunto_ - 1)

        Dim rolId_ = GetRolCubeId(cubename_)

        Dim sax_ = SwicthedProjectSax(16)


        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)

            _enlaceDatos.GetMongoCollection(Of Room)("", rolId_).Aggregate.
                              Project(Function(ch) New With {
                                                               ch._id,
                                                               ch.roomname,
                                                               ch.rules,
                                                               Key .parametros = ch.addresses(1).ref}).
                              Match(Function(s) s.roomname.Equals(roomname_.Substring(1, roomname_.Length - 2))).ToList.
                              ForEach(Sub(room_)

                                          operacion_ = room_.rules

                                          parametros_ = room_.parametros

                                      End Sub)

        End Using


        SwicthedProjectSax(13)

        parametros_.Insert(0, operacion_)

        _status = New TagWatcher() With {.ObjectReturned = parametros_}

        Return _status

    End Function

    Function FindSignatureRule(listacontextos_ As List(Of Framing), rules_ As String, firma_ As String) As String

        Dim listarules_ = listacontextos_.Find(Function(ch) ch.firmacontext = firma_ And ch.firmacontext <> "")

        If listarules_ Is Nothing Then

            Return rules_

        Else

            Return listarules_.rules

        End If

    End Function

    Public Function GetOperands(Optional firma_ As String = "") As TagWatcher Implements ICubeController.GetOperands

        Dim operands_ = New List(Of String)

        Dim rolIds_ As New List(Of Int32) From {1, 2, 3, 4, 5, 6}


        Dim sax_ = SwicthedProjectSax(16)


        For Each rolId_ In rolIds_

            Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

                OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)

                _enlaceDatos.GetMongoCollection(Of Room)("", rolId_).Aggregate.
                              Match(Function(s) s.contenttype.Equals("operando")).ToList.
                              ForEach(Sub(room_)

                                          operands_.AddRange(FindSignatureRule(room_.addresses, room_.rules, firma_).Split(New String() {"="}, 2, StringSplitOptions.None).ToList())

                                      End Sub)

            End Using

        Next

        SwicthedProjectSax(13)

        _status = New TagWatcher() With {.ObjectReturned = operands_}

        Return _status

    End Function

    Private Function SwicthedProjectSax(appId_ As Int32) As Sax.SaxStatements

        Dim sax_ As Sax.SaxStatements = Sax.SaxStatements.GetInstance(appId_)

        Dim saxSetting1_ = sax_.SaxSettings(1)

        sax_.SaxSettings(1) = sax_.SaxSettings(2)

        sax_.SaxSettings(2) = saxSetting1_

        Return sax_

    End Function

    Private Sub OnRol(ByRef roles_ As List(Of Sax.rol), rolId_ As Int32)

        For Each rol_ In roles_

            If rol_._id = rolId_ Then

                rol_.status = "on"
                rol_.rolname = "bigdataops"

            Else

                rol_.status = "off"
                rol_.rolname = "bigdatamathematic"

            End If

        Next

    End Sub

    Private Function GetRolCubeId(cubeName_) As Int32

        Dim rolId_ As Int32

        Select Case cubeName_

            Case "A22"

                rolId_ = 1

            Case "VOCE"

                rolId_ = 2

            Case "UAA"

                rolId_ = 3

            Case "UCAA"

                rolId_ = 4

            Case "UCC"

                rolId_ = 5

            Case "CDI"

                rolId_ = 6

            Case Else

                rolId_ = 1


        End Select

        Return rolId_

    End Function

    Public Function GetRoom(idRoom_ As ObjectId, rolId_ As Int32) As TagWatcher Implements ICubeController.GetRoom

        Dim rooms_ As New List(Of Room)

        Dim sax_ = SwicthedProjectSax(16)


        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos



            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)



            _enlaceDatos.GetMongoCollection(Of Room)("", rolId_).
                                  Aggregate.Match(Function(ch) ch._id = idRoom_).
                                  ToList.
                                  ForEach(Sub(room_)

                                              rooms_.Add(room_)


                                          End Sub)


        End Using

        SwicthedProjectSax(13)

        _status = New TagWatcher With {.ObjectReturned = rooms_}

        Return _status


    End Function

    Private Function GetCubeSource(rolId_ As Int32) As String

        Dim cubeName_ As String

        Select Case rolId_

            Case 1

                cubeName_ = "A22"

            Case 2

                cubeName_ = "VOCE"

            Case 3

                cubeName_ = "UAA"

            Case 4

                cubeName_ = "UCAA"

            Case 5

                cubeName_ = "UCC"

            Case 6

                cubeName_ = "CDI"

            Case 7

                cubeName_ = "Resource"

            Case Else

                cubeName_ = "A22"


        End Select

        Return cubeName_

    End Function


    Public Function SetFormula(Of T)(idRoom_ As ObjectId, roomName_ As String, roomRules_ As String, cubeDestin_ As String, contenttype_ As String, descriptionRules_ As String, status_ As String, Optional idUser_ As ObjectId = Nothing, Optional userName_ As String = "", Optional enviado_ As String = "unsent", Optional reason_ As String = "") As TagWatcher Implements ICubeController.SetFormula

        Dim params_ As New List(Of String) From {cubeDestin_ & "." & roomName_.ToUpper.Replace(" ", "").Replace(Chr(160), "").Replace(Chr(13), "").Replace(Chr(10), "")}

        'Dim contenttype_ = "formulas"



        If contenttype_.ToUpper = "FÓRMULA" OrElse contenttype_ = "fórmula" Then

            contenttype_ = "formula"

        End If

        If roomRules_.IndexOf(".csv") > -1 Then

            contenttype_ = "csv"

            roomRules_ = GetCsv(roomName_, roomRules_).ObjectReturned

        Else

            Dim mateController_ As IMathematicalInterpreter = New MathematicalInterpreterNCalc

            params_.AddRange(mateController_.GetParams(roomRules_))

        End If

        Dim outputDatabaseName_ As String = ""

        Dim outputCollectionName_ As String = ""

        Dim sax_ = SwicthedProjectSax(16)

        Dim valorPresentacion_ As String = roomName_

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            Dim rolId_ = GetRolCubeId(cubeDestin_)

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)

            Dim operationsDB_ = _enlaceDatos.GetMongoCollection(Of Room)("", rolId_)

            Dim roomHistoryList_ As New List(Of RoomHistory)

            Dim roomawait_ As New List(Of RoomHistory)

            Dim newObjectId_ = ObjectId.GenerateNewId

            Dim roomCurrent_ As Room = Nothing

            operationsDB_.Aggregate.
                          Match(Function(e) e.roomname.ToUpper.Contains(cubeDestin_.ToUpper & "." & roomName_.ToUpper.Replace(" ", "").Replace(Chr(160), "").Replace(Chr(13), "").Replace(Chr(10), "")) Or e._id = idRoom_).
                          ToList.ForEach(Sub(rooms_)

                                             Dim roomHistory_ As New RoomHistory With {._id = ObjectId.GenerateNewId,
                                                                                       .rules = roomRules_,
                                                                                       .roomname = roomName_,
                                                                                       .description = descriptionRules_,
                                                                                       .addresses = New List(Of Framing) From {New Framing With
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
                                                                                                                               New Framing With
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
                                                                                        },
                                                                                       .messages = rooms_.messages,
                                                                                       .status = rooms_.status,
                                                                                       .contenttype = rooms_.contenttype,
                                                                                       .reason = reason_,
                                                                                       .createat = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
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

                                                 roomHistoryList_.AddRange(rooms_.historical)

                                                 valorPresentacion_ = rooms_.historical(0).roomname

                                                 If enviado_ = "on" Then

                                                     roomHistoryList_.Insert(0, roomHistory_)

                                                     valorPresentacion_ = roomHistory_.roomname


                                                 End If


                                             End If

                                             If rooms_.awaitingupdate Is Nothing Then

                                                 roomawait_.Add(roomHistory_)
                                             Else

                                                 roomawait_.AddRange(rooms_.awaitingupdate)

                                                 roomawait_.Insert(0, roomHistory_)

                                             End If

                                             newObjectId_ = rooms_._id

                                             roomawait_(0).status = enviado_

                                             rooms_.awaitingupdate = roomawait_

                                             roomCurrent_ = rooms_



                                         End Sub)

            If roomCurrent_ Is Nothing Then

                roomCurrent_ = New Room With
                               {._id = newObjectId_,
                                 .roomname = cubeDestin_ & "." & roomName_.ToUpper.Replace(" ", "").Replace(Chr(160), "").Replace(Chr(13), "").Replace(Chr(10), ""),
                                 .rules = roomRules_,
                                 .description = descriptionRules_,
                                 .required = True,
                                 .fieldsrequired = New List(Of String),
                                 .type = "warning",
                                 .addresses = New List(Of Framing) From
                                                                   {
                                                                      New Framing With
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
                                                                      New Framing With
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
                                                                    },
                                 .status = status_,
                                 .messages = New List(Of String),
                                 .contenttype = contenttype_.ToLower,
                                 .awaitingupdate = roomawait_,
                                 .historical = roomHistoryList_
                                 }

            Else

                If roomCurrent_.awaitingupdate IsNot Nothing Then

                    If (roomCurrent_.awaitingupdate.Count > 0) Then

                        Dim roomTemp_ = roomCurrent_.awaitingupdate(0)

                        If enviado_ = "on" Then

                            roomCurrent_.roomname = roomTemp_.roomname
                            roomCurrent_.rules = roomTemp_.rules
                            roomCurrent_.addresses = roomTemp_.addresses
                            roomCurrent_.description = roomTemp_.description
                            roomCurrent_.messages = roomTemp_.messages
                            roomCurrent_.contenttype = roomTemp_.contenttype
                            roomCurrent_.awaitingupdate(0).status = "on"
                            roomCurrent_.historical = roomHistoryList_

                        End If

                    End If

                End If


            End If


            If roomHistoryList_.Count = 0 Then

                roomHistoryList_.Add(New RoomHistory With {._id = ObjectId.GenerateNewId,
                                          .rules = roomRules_,
                                          .description = descriptionRules_,
                                          .roomname = roomName_,
                                          .addresses = roomCurrent_.addresses,
                                          .messages = roomCurrent_.messages,
                                          .status = roomCurrent_.status,
                                          .contenttype = roomCurrent_.contenttype,
                                          .createat = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                          .reason = "ALTA"
                                          })
                If idUser_ <> Nothing Then

                    roomHistoryList_(0)._iduser = idUser_


                End If

                If userName_ <> "" Then

                    roomHistoryList_(0).username = userName_


                End If

                roomCurrent_.historical = roomHistoryList_

            Else



            End If


            If roomCurrent_.historical.Count > 3 Then

                roomCurrent_.historical.RemoveRange(3, roomCurrent_.historical.Count - 3)

            End If

            'If roomawait_.Count = 0 Then

            '    roomawait_.Add(New RoomHistory With {._id = ObjectId.GenerateNewId,
            '                              .rules = roomCurrent_.rules,
            '                              .description = roomCurrent_.description,
            '                              .roomname = roomCurrent_.roomname,
            '                              .addresses = roomCurrent_.addresses,
            '                              .messages = roomCurrent_.messages,
            '                              .status = roomCurrent_.status,
            '                              .contenttype = roomCurrent_.contenttype,
            '                              .createat = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            '                              .reason = "ALTA"
            '                              })
            '    If idUser_ <> Nothing Then

            '        roomawait_(0)._iduser = idUser_


            '    End If

            '    If userName_ <> "" Then

            '        roomawait_(0).username = userName_


            '    End If

            '    roomCurrent_.awaitingupdate = roomawait_

            'End If


            If roomCurrent_.awaitingupdate.Count > 3 Then

                roomCurrent_.awaitingupdate.RemoveRange(3, roomCurrent_.awaitingupdate.Count - 3)

            End If


            Dim updateDefinition_ = Builders(Of Room).
                                    Update.
                                   Set(Function(e) e.roomname, roomCurrent_.roomname.ToUpper).
                                   Set(Function(e) e.rules, roomCurrent_.rules).
                                   Set(Function(e) e.description, roomCurrent_.description).
                                   Set(Function(e) e.required, roomCurrent_.required).
                                   Set(Function(e) e.fieldsrequired, roomCurrent_.fieldsrequired).
                                   Set(Function(e) e.type, roomCurrent_.type).
                                   Set(Function(e) e.addresses, roomCurrent_.addresses).
                                   Set(Function(e) e.status, roomCurrent_.status).
                                   Set(Function(e) e.messages, roomCurrent_.messages).
                                   Set(Function(e) e.contenttype, roomCurrent_.contenttype).
                                   Set(Function(e) e.awaitingupdate, roomCurrent_.awaitingupdate).
                                   Set(Function(e) e.historical, roomCurrent_.historical)

            operationsDB_.UpdateOne(Function(e) e._id = roomCurrent_._id,
                                    updateDefinition_,
                                    New UpdateOptions With {.IsUpsert = True})

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, 7)



            Dim operationsDBResource_ = _enlaceDatos.GetMongoCollection(Of RoomResource)("", 7)



            Dim roomResource_ As New RoomResource With {
                                                        ._id = ObjectId.GenerateNewId,
                                                        .roomname = roomCurrent_.roomname,
                                                        .description = roomCurrent_.description,
                                                        .rules = roomCurrent_.rules,
                                                        .status = roomCurrent_.status,
                                                        .contenttype = roomCurrent_.contenttype,
                                                        .createat = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                                        .rolId_ = rolId_,
                                                        .cubeSource_ = GetCubeSource(rolId_),
                                                        ._idroom = roomCurrent_._id,
                                                        .username = userName_,
                                                        .valorpresentacion = valorPresentacion_
                                                         }

            Dim updateDefinitionResource_ = Builders(Of RoomResource).
                                    Update.
                                   Set(Function(e) e.roomname, roomResource_.roomname.ToUpper).
                                   Set(Function(e) e.description, roomResource_.description).
                                   Set(Function(e) e.rules, roomResource_.rules).
                                   Set(Function(e) e.status, roomResource_.status).
                                   Set(Function(e) e.contenttype, roomResource_.contenttype).
                                   Set(Function(e) e.createat, roomResource_.createat).
                                   Set(Function(e) e.rolId_, roomResource_.rolId_).
                                   Set(Function(e) e.cubeSource_, roomResource_.cubeSource_).
                                   Set(Function(e) e._idroom, roomResource_._idroom).
                                   Set(Function(e) e.username, roomResource_.username).
                                   Set(Function(e) e.valorpresentacion, roomResource_.valorpresentacion.ToUpper)

            operationsDBResource_.UpdateOne(Function(e) e._idroom = roomResource_._idroom,
                                    updateDefinitionResource_,
                                    New UpdateOptions With {.IsUpsert = True})


            roomCurrent_.historical(0).createat = DateTime.Now

            _status = New TagWatcher() With {.ObjectReturned = roomCurrent_}

        End Using

        SwicthedProjectSax(13)

        _status.SetOK()

        Return _status

    End Function

    'Public Function SetFormula(Of T)(idRoom_ As ObjectId, NewroomName_ As String, roomRules_ As String, cubeDestin_ As String, contenttype_ As String, descriptionRules_ As String, status_ As String, Optional idUser_ As ObjectId = Nothing, Optional userName_ As String = "") As TagWatcher Implements ICubeController.SetFormula

    '    Dim params_ As New List(Of String) From {cubeDestin_ & "." & NewroomName_.ToUpper.Replace(" ", "").Replace(Chr(160), "").Replace(Chr(13), "").Replace(Chr(10), "")}

    '    'Dim contenttype_ = "formulas"



    '    If contenttype_.ToUpper = "FÓRMULA" OrElse contenttype_ = "fórmula" Then

    '        contenttype_ = "formula"

    '    End If

    '    If roomRules_.IndexOf(".csv") > -1 Then

    '        contenttype_ = "csv"

    '        roomRules_ = GetCsv(NewroomName_, roomRules_).ObjectReturned

    '    Else

    '        Dim mateController_ As IMathematicalInterpreter = New MathematicalInterpreterNCalc

    '        params_.AddRange(mateController_.GetParams(roomRules_))

    '    End If

    '    Dim outputDatabaseName_ As String = ""

    '    Dim outputCollectionName_ As String = ""

    '    Dim sax_ = SwicthedProjectSax(16)

    '    Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

    '        Dim rolId_ = GetRolCubeId(cubeDestin_)

    '        OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)

    '        Dim operationsDB_ = _enlaceDatos.GetMongoCollection(Of Room)("", rolId_)

    '        Dim roomHistoryList_ As New List(Of RoomHistory)

    '        Dim newObjectId_ = ObjectId.GenerateNewId

    '        operationsDB_.Aggregate.
    '                      Match(Function(e) e._id = idRoom_).
    '                      ToList.ForEach(Sub(rooms_)

    '                                         Dim roomHistory_ As New RoomHistory With {._id = ObjectId.GenerateNewId,
    '                                                                                   .rules = roomRules_,
    '                                                                                   .roomname = rooms_.roomname,
    '                                                                                   .description = descriptionRules_,
    '                                                                                   .addresses = rooms_.addresses,
    '                                                                                   .messages = rooms_.messages,
    '                                                                                   .status = rooms_.status,
    '                                                                                   .contenttype = rooms_.contenttype,
    '                                                                                   .createat = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
    '                                                                                   }

    '                                         If idUser_ <> Nothing Then

    '                                             roomHistory_._iduser = idUser_


    '                                         End If

    '                                         If userName_ <> "" Then

    '                                             roomHistory_.username = userName_


    '                                         End If

    '                                         If rooms_.historical Is Nothing Then

    '                                             roomHistoryList_.Add(roomHistory_)
    '                                         Else

    '                                             roomHistoryList_.AddRange(rooms_.historical)

    '                                             roomHistoryList_.Insert(0, roomHistory_)

    '                                         End If

    '                                         newObjectId_ = rooms_._id

    '                                     End Sub)

    '        Dim room_ = New Room With
    '                            {._id = newObjectId_,
    '                              .roomname = cubeDestin_ & "." & NewroomName_.ToUpper.Replace(" ", "").Replace(Chr(160), "").Replace(Chr(13), "").Replace(Chr(10), ""),
    '                              .rules = roomRules_,
    '                              .description = descriptionRules_,
    '                              .required = True,
    '                              .fieldsrequired = New List(Of String),
    '                              .type = "warning",
    '                              .addresses = New List(Of Framing) From
    '                                                                {
    '                                                                   New Framing With
    '                                                                               {
    '                                                                                 ._idcontext = ObjectId.GenerateNewId,
    '                                                                                 .context = "home",
    '                                                                                 .firmacontext = Nothing,
    '                                                                                 .rules = Nothing,
    '                                                                                 .ref = Nothing,
    '                                                                                 .loc = New List(Of Int32) From {12, 1005},
    '                                                                                 .cached = Nothing,
    '                                                                                 .result = Nothing,
    '                                                                                 .status = Nothing,
    '                                                                                 .timelife = Nothing
    '                                                                                 },
    '                                                                   New Framing With
    '                                                                               {
    '                                                                                 ._idcontext = ObjectId.GenerateNewId,
    '                                                                                 .context = "resourcerequired",
    '                                                                                 .firmacontext = Nothing,
    '                                                                                 .rules = Nothing,
    '                                                                                 .ref = params_,
    '                                                                                 .loc = Nothing,
    '                                                                                  .cached = Nothing,
    '                                                                                  .result = Nothing,
    '                                                                                  .status = Nothing,
    '                                                                                  .timelife = Nothing
    '                                                                                }
    '                                                                 },
    '                              .status = status_,
    '                              .messages = New List(Of String),
    '                              .contenttype = contenttype_.ToLower,
    '                              .historical = roomHistoryList_
    '                              }

    '        If roomHistoryList_.Count = 0 Then

    '            roomHistoryList_.Add(New RoomHistory With {._id = ObjectId.GenerateNewId,
    '                                      .rules = roomRules_,
    '                                      .description = descriptionRules_,
    '                                      .roomname = room_.roomname,
    '                                      .addresses = room_.addresses,
    '                                      .messages = room_.messages,
    '                                      .status = room_.status,
    '                                      .contenttype = room_.contenttype,
    '                                      .createat = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
    '                                      })
    '            room_.historical = roomHistoryList_

    '        End If


    '        If room_.historical.Count > 3 Then

    '            room_.historical.RemoveRange(3, room_.historical.Count - 3)

    '        End If


    '        Dim updateDefinition_ = Builders(Of Room).
    '                                Update.
    '                               Set(Function(e) e.roomname, room_.roomname.ToUpper).
    '                               Set(Function(e) e.rules, room_.rules).
    '                               Set(Function(e) e.description, room_.description).
    '                               Set(Function(e) e.required, room_.required).
    '                               Set(Function(e) e.fieldsrequired, room_.fieldsrequired).
    '                               Set(Function(e) e.type, room_.type).
    '                               Set(Function(e) e.addresses, room_.addresses).
    '                               Set(Function(e) e.status, room_.status).
    '                               Set(Function(e) e.messages, room_.messages).
    '                               Set(Function(e) e.contenttype, room_.contenttype).
    '                               Set(Function(e) e.historical, room_.historical)


    '        operationsDB_.UpdateOne(Function(e) e._id = room_._id,
    '                                updateDefinition_,
    '                                New UpdateOptions With {.IsUpsert = True})

    '        OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, 7)



    '        Dim operationsDBResource_ = _enlaceDatos.GetMongoCollection(Of RoomResource)("", 7)

    '        Dim roomResource_ As New RoomResource With {
    '                                                    ._id = ObjectId.GenerateNewId,
    '                                                    .roomname = room_.roomname,
    '                                                    .description = room_.description,
    '                                                    .rules = room_.rules,
    '                                                    .status = room_.status,
    '                                                    .contenttype = room_.contenttype,
    '                                                    .createat = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
    '                                                    .rolId_ = rolId_,
    '                                                    .cubeSource_ = GetCubeSource(rolId_),
    '                                                    ._idroom = room_._id,
    '                                                    .username = userName_,
    '                                                    .valorpresentacion = NewroomName_
    '                                                     }

    '        Dim updateDefinitionResource_ = Builders(Of RoomResource).
    '                                Update.
    '                               Set(Function(e) e.roomname, roomResource_.roomname.ToUpper).
    '                               Set(Function(e) e.description, roomResource_.description).
    '                               Set(Function(e) e.rules, roomResource_.rules).
    '                               Set(Function(e) e.status, roomResource_.status).
    '                               Set(Function(e) e.contenttype, roomResource_.contenttype).
    '                               Set(Function(e) e.createat, roomResource_.createat).
    '                               Set(Function(e) e.rolId_, roomResource_.rolId_).
    '                               Set(Function(e) e.cubeSource_, roomResource_.cubeSource_).
    '                               Set(Function(e) e._idroom, roomResource_._idroom).
    '                               Set(Function(e) e.username, roomResource_.username).
    '                               Set(Function(e) e.valorpresentacion, roomResource_.valorpresentacion.ToUpper)

    '        operationsDBResource_.UpdateOne(Function(e) e._idroom = roomResource_._idroom,
    '                                updateDefinitionResource_,
    '                                New UpdateOptions With {.IsUpsert = True})


    '        room_.historical(0).createat = DateTime.Now

    '        _status = New TagWatcher() With {.ObjectReturned = room_}

    '    End Using

    '    SwicthedProjectSax(13)

    '    _status.SetOK()

    '    Return _status

    'End Function


    Public Sub FillRoomResource() Implements ICubeController.FillRoomResource

        Dim resourceRooms_ As New List(Of RoomResource)

        Dim sax_ = SwicthedProjectSax(16)

        Dim cuenta_ = 15

        Dim rolId_ = 1

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            While rolId_ <= 6


                OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)



                _enlaceDatos.GetMongoCollection(Of Room)("", rolId_).
                                  Aggregate.
                                  ToList.
                                  ForEach(Sub(room_)

                                              Dim valorPresentacion_ = room_.roomname.Substring(room_.roomname.IndexOf(".") + 1)

                                              resourceRooms_.Add(New RoomResource With {
                                                                  ._id = ObjectId.GenerateNewId,
                                                                  .roomname = room_.roomname,
                                                                  .description = room_.description,
                                                                  .rules = room_.rules,
                                                                  .status = room_.status,
                                                                  .contenttype = room_.contenttype,
                                                                  .createat = DateTime.Now,
                                                                  .rolId_ = rolId_,
                                                                  .cubeSource_ = GetCubeSource(rolId_),
                                                                  ._idroom = room_._id,
                                                                  .username = "originalfourier@gmail.com",
                                                                  .valorpresentacion = valorPresentacion_
                                                                   })


                                          End Sub)

                rolId_ += 1

            End While

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, 7)

            For Each resourceRoom_ In resourceRooms_

                _enlaceDatos.GetMongoCollection(Of RoomResource)("", 7).InsertOne(resourceRoom_)

            Next



        End Using


        SwicthedProjectSax(13)

    End Sub

    Public Function RunRoom(Of T)(roomname_ As String, params As String) As ValidatorReport Implements ICubeController.RunRoom

        Dim operacion_ As String = ""

        Dim parametros_ As New List(Of String)

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos


            Dim noSQL_ = Sax.SaxStatements.GetInstance(13).SaxSettings(1)

            Dim endPoint_ = noSQL_.endpoints.FindAll(Function(ch) ch.info.Contains("bigdatamathematics"))(0)

            Dim roots_ = noSQL_.roots.FindAll(Function(ch) endPoint_.info.Contains(ch.rolnamelinked))

            Dim database_ = noSQL_.servers.nosql.mongodb.rol.FindAll(Function(ch) ch.rolname = roots_(0).rolnamelinked)(0)

            Dim conexion As IMongoClient = New MongoClient("mongodb://" & endPoint_.ip & ":" & endPoint_.port)
            conexion.GetDatabase(database_.name).GetCollection(Of Room)(roots_(0).linkedresources(0).name).Aggregate.
                                                                       Project(Function(ch) New With {
                                                                                             ch._id,
                                                                                             ch.roomname,
                                                                                              ch.rules,
                                                                                             Key .parametros = ch.addresses(1).ref}).
                                                                       Match(Function(s) s.roomname.Equals(roomname_.Substring(1, roomname_.Length - 2))).ToList.
                                                                       ForEach(Sub(room_)

                                                                                   operacion_ = room_.rules

                                                                                   parametros_ = room_.parametros

                                                                               End Sub)
        End Using

        _status = New TagWatcher()

        parametros_.Insert(0, operacion_)

        _status.ObjectReturned = parametros_

        Return New ValidatorReport

    End Function

    Public Function RunRoom(Of T)(roomname_ As String, params_ As Dictionary(Of String, T)) As ValidatorReport Implements ICubeController.RunRoom

        Dim operacion_ As String = ""

        Dim parametros_ As New List(Of String)

        _roomsResource = New List(Of RoomResource)

        Dim sax_ = SwicthedProjectSax(16)

        Dim rolId_ = 7

        _reports = New ValidatorReport

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)


            _roomsResource.AddRange(_enlaceDatos.GetMongoCollection(Of RoomResource)("", rolId_).
                                                 Aggregate.
                                                 Match(Function(ch) ch.roomname.Equals(roomname_)).
                                                 ToList)

            If _roomsResource.Count = 0 Then


                _reports.SetHeaderReport("Recámara no encontrada",
                                         DateTime.Now,
                                        AdviceTypesReport.Alert,
                                         AdviceTypesReport.Alert,
                                        "La recámara '" & roomname_ & "' No fue Encontrada",
                                        roomname_,
                                        "", "", TriggerSourceTypes.Cube)

                _reports.ShowMessageError()

            Else



                OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, _roomsResource(0).rolId_)

                _rooms = New List(Of Room)

                _rooms.AddRange(_enlaceDatos.GetMongoCollection(Of Room)("", _roomsResource(0).rolId_).
                                                     Aggregate.
                                                     Match(Function(ch) ch._id = _roomsResource(0)._idroom).
                                                     ToList)

                Dim found_ = True

                Dim mensaje_ As String = ""

                For Each param_ In _rooms(0).addresses(1).ref.Skip(1)



                    For Each key_ In params_.Keys

                        If key_.Substring(0, key_.LastIndexOf(".")) = param_ Then

                            found_ = True

                            Exit For

                        Else

                            found_ = False

                            mensaje_ &= param_ & Chr(13)

                        End If

                    Next

                Next

                If found_ Then


                    _interpreter = New MathematicalInterpreterNCalc

                    _status = New TagWatcher()

                    _status.ObjectReturned = _interpreter.RunExpression(Of T)(_rooms(0).rules, params_)

                    _status.SetOK()

                    _reports.result = _status.ObjectReturned

                Else

                    _reports = New ValidatorReport

                    _reports.SetHeaderReport("Parámetros no encontrados",
                                             DateTime.Now,
                                            AdviceTypesReport.Alert,
                                             AdviceTypesReport.Alert,
                                            "No se encontraron los siguientes parámetros:'" & mensaje_,
                                            mensaje_,
                                            "", "", TriggerSourceTypes.Cube)

                    _reports.ShowMessageError()

                End If


            End If


        End Using


        SwicthedProjectSax(13)


        Return _reports

    End Function

    Public Function GetRoomNames(Optional token_ As String = "") As TagWatcher Implements ICubeController.GetRoomNames

        _rooms = New List(Of Room)

        Dim sax_ = SwicthedProjectSax(16)

        Dim cuenta_ = 15

        Dim rolId_ = 1

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            While rolId_ <= 6 And cuenta_ > 0


                OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)

                If token_ = "" Then

                    _rooms.AddRange(_enlaceDatos.GetMongoCollection(Of Room)("", rolId_).
                                      Aggregate.
                                      Limit(cuenta_).
                                      ToList)
                Else

                    _rooms.AddRange(_enlaceDatos.GetMongoCollection(Of Room)("", rolId_).
                                                 Aggregate.
                                                 Match(Function(ch) ch.roomname.ToUpper.Contains(token_.ToUpper)).
                                                 Limit(cuenta_).
                                                 ToList)

                End If


                cuenta_ -= _rooms.Count

                rolId_ += 1

            End While

        End Using


        SwicthedProjectSax(13)

        _status = New TagWatcher() With {.ObjectReturned = _rooms}

        Return _status

    End Function

    Function GetRoomNamesResource(Optional token_ As String = "") As TagWatcher Implements ICubeController.GetRoomNamesResource

        _roomsResource = New List(Of RoomResource)

        Dim sax_ = SwicthedProjectSax(16)

        Dim cuenta_ = 15

        Dim rolId_ = 7

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)

            If token_ = "" Then

                _roomsResource.AddRange(_enlaceDatos.GetMongoCollection(Of RoomResource)("", rolId_).
                                      Aggregate.
                                      Limit(cuenta_).
                                      ToList)
            Else

                _roomsResource.AddRange(_enlaceDatos.GetMongoCollection(Of RoomResource)("", rolId_).
                                                 Aggregate.
                                                 Match(Function(ch) ch.valorpresentacion.ToUpper.Contains(token_.ToUpper) Or ch.cubeSource_.Equals(token_)).
                                                 Limit(cuenta_).
                                                 ToList)

            End If




        End Using


        SwicthedProjectSax(13)

        _status = New TagWatcher() With {.ObjectReturned = _roomsResource}

        Return _status

    End Function


    Public Function GetReports() As ValidatorReport Implements ICubeController.GetReports
        Throw New NotImplementedException()
    End Function

    Public Function GetReports(roomname_ As String) As ValidatorReport Implements ICubeController.GetReports
        Throw New NotImplementedException()
    End Function

    Public Function GetStatus(_idpedimento As ObjectId) As ValidatorReport Implements ICubeController.GetStatus

        Dim documentoElectronico_ As New DocumentoElectronico

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            _enlaceDatos.GetMongoCollection(Of OperacionGenerica)("", 1).Aggregate.
                          Match(Function(s) s.Id = _idpedimento).ToList.
                          ForEach(Sub(pedimento_)

                                      documentoElectronico_ = pedimento_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                                  End Sub)

        End Using

        ' Dim organismo_ As New Organismo

        Dim expressionOk_ As Boolean

        'Dim algo_ = organismo_.ObtenerCamposSeccionExterior(New List(Of ObjectId) From {_idpedimento},
        '                                        New ConstructorPedimentoNormal(), New Dictionary(Of [Enum], List(Of [Enum])) From
        '                                        {{SeccionesPedimento.ANS1, New List(Of [Enum]) From
        '                                        {CamposPedimento.CA_REGIMEN,
        '                                        CamposPedimento.CA_TIPO_OPERACION,
        '                                        CamposPedimento.CA_CVE_PEDIMENTO,
        '                                        CamposPedimento.CA_FLETES}},
        '                                        {SeccionesPedimento.ANS13, New List(Of [Enum]) From
        '                                        {CamposPedimento.CA_INCOTERM
        '                                        }}})


        ' checando Regimen



        Dim camposRegimen_ = documentoElectronico_.Campo(CamposPedimento.CA_REGIMEN)

        Dim camposTipoOperacion_ As String = documentoElectronico_.Campo(CamposPedimento.CA_TIPO_OPERACION).Valor

        If camposTipoOperacion_ = "0" Then

            camposTipoOperacion_ = "1"

        Else

            camposTipoOperacion_ = "2"


        End If

        Dim cvePedimento_ As String = documentoElectronico_.Campo(CamposPedimento.CA_CVE_PEDIMENTO).ValorPresentacion.Substring(0, 2)

        Dim camposFletes_ = documentoElectronico_.Campo(CamposPedimento.CA_FLETES)

        Dim camposIncoterm_ As String = documentoElectronico_.Campo(CamposPedimento.CA_INCOTERM).Valor

        If camposIncoterm_ = Nothing Then

            camposIncoterm_ = ""

        Else


            If camposIncoterm_.Length >= 3 Then

                camposIncoterm_ = camposIncoterm_.Substring(0, 3)

            End If

        End If


        '  Dim tagwatcher_ = GetFormula("'A22.REGIMEN" & camposRegimen_.Valor.ToString.ToUpper & "'")

        Dim interpreter_ As New MathematicalInterpreterNCalc


        If _reports Is Nothing Then

            _reports = New ValidatorReport

        End If

        If cvePedimento_ = "CT" Then

            If "OK" = interpreter_.RunExpression(Of Object)("ROOM('A22.PEDIMENTOCT','" &
                                                                               cvePedimento_ & "'," &
                                                                               camposTipoOperacion_ & ")", Nothing).ToString Then

                MsgBox("Relación Regimen-Tipo-Operacion-CvePedimento Correcta" &
                   Chr(13) &
                   "tipo de operación:" &
                    IIf(camposTipoOperacion_ = "1", "Importación", "Exportación") &
                                     Chr(13) &
                                     "clave de pedimento:" &
                                     cvePedimento_ &
                                     Chr(13) &
                                     "regimen:" & camposRegimen_.Valor.ToString)

                expressionOk_ = True

            Else


                _reports.SetHeaderReport("No es válida la relación de " &
                                     Chr(13) &
                                     "tipo de operacipon: " &
                                     IIf(camposTipoOperacion_ = "1", "Importación",
                                         IIf(camposTipoOperacion_ = "2", "Exportación", camposTipoOperacion_)) &
                                     Chr(13) &
                                     "clave de pedimento:" &
                                    cvePedimento_ &
                                     Chr(13) &
                                     "regimen:" & camposRegimen_.Valor.ToString,
                                         DateTime.Now,
                                        AdviceTypesReport.Alert,
                                         AdviceTypesReport.Alert,
                                        "Relación Regimen-Tipo-Operacion-CvePedimento Inválida",
                                        "RegimenClaveOperacion",
                                        "", "", TriggerSourceTypes.Cube)

                _reports.ShowMessageError()

                expressionOk_ = False

            End If

        Else

            If "OK" = interpreter_.RunExpression(Of Object)("ROOM('A22.REGIMEN" & camposRegimen_.Valor.ToString.ToUpper & "'," &
                                                                               camposTipoOperacion_ & ",'" &
                                                                               cvePedimento_ & "')", Nothing).ToString Then

                MsgBox("Relación Regimen-Tipo-Operacion-CvePedimento Correcta" &
                   Chr(13) &
                   "tipo de operación:" &
                    IIf(camposTipoOperacion_ = "1", "Importación", "Exportación") &
                                     Chr(13) &
                                     "clave de pedimento:" &
                                    cvePedimento_ &
                                     Chr(13) &
                                     "regimen:" & camposRegimen_.Valor.ToString)


                expressionOk_ = True

            Else


                _reports.SetHeaderReport("No es válida la relación de " &
                                     Chr(13) &
                                     "tipo de operacipon: " &
                                     IIf(camposTipoOperacion_ = "1", "Importación",
                                         IIf(camposTipoOperacion_ = "2", "Exportación", camposTipoOperacion_)) &
                                     Chr(13) &
                                     "clave de pedimento:" &
                                    cvePedimento_ &
                                     Chr(13) &
                                     "regimen:" & camposRegimen_.Valor.ToString,
                                         DateTime.Now,
                                        AdviceTypesReport.Alert,
                                         AdviceTypesReport.Alert,
                                        "Relación Regimen-Tipo-Operacion-CvePedimento Inválida",
                                        "RegimenClaveOperacion",
                                        "", "", TriggerSourceTypes.Cube)

                _reports.ShowMessageError()

                expressionOk_ = False

            End If


        End If

        If expressionOk_ Then


            If camposFletes_.Valor Is Nothing Then

                camposFletes_.Valor = 0

            End If

            If "OK" = interpreter_.RunExpression(Of Object)("ROOM('A22.INCOTERMINCREMENTABLE','" &
                                                                               camposIncoterm_ & "'," &
                                                                               camposFletes_.Valor & ",'" &
                                                                               cvePedimento_ & "')", Nothing).ToString Then

                MsgBox("Relación Incoterms - Incrementables Correcto" &
                   Chr(13) &
                   "Incoterm:" &
                   camposIncoterm_ &
                   Chr(13) &
                   "Incrementable Flete:" &
                   camposFletes_.Valor.ToString)


                expressionOk_ = True

            Else


                _reports.SetHeaderReport("No es válida la relación de " &
                                         Chr(13) &
                                        "Incoterm:" &
                                        camposIncoterm_ &
                   Chr(13) &
                   "Incrementable Flete:" &
                   camposFletes_.Valor.ToString,
                                         DateTime.Now,
                                        AdviceTypesReport.Alert,
                                         AdviceTypesReport.Alert,
                                        "Relación Incoterm-Incrementables Inválida",
                                        "IncotermIncrementables",
                                        "", "", TriggerSourceTypes.Cube)

                _reports.ShowMessageError()

                expressionOk_ = False

            End If

        End If



        Return _reports

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

#End Region

End Class
