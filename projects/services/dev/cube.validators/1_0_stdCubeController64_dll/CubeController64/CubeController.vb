Imports System.Collections.Specialized.BitVector32
Imports System.Linq.Expressions
Imports System.Web.UI.WebControls
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
    Private _rooms As List(Of room)

    Private _roomsResource As List(Of roomresource)

    Private _status As TagWatcher

    Private _reports As ValidatorReport

    Private _interpreter As IMathematicalInterpreter

    Private _fieldmiss As List(Of String)



#End Region
#Region "Properties"

    Public Property rooms As List(Of room) Implements ICubeController.rooms

        Get
            Return _rooms

        End Get

        Set(value As List(Of room))

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

    Public Property fieldmiss As List(Of String) Implements ICubeController.fieldmiss

        Get
            fieldmiss = _fieldmiss

        End Get
        Set(value As List(Of String))

            _fieldmiss = value

        End Set

    End Property

#End Region

#Region "Methods"

    Sub New()

        _interpreter = New MathematicalInterpreterNCalc
        GetFieldsNamesResource()
        _interpreter.addOperands(GetOperands().ObjectReturned)


    End Sub

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

    Public Function ValidateFields(Of T)(campos_ As List(Of String), documentoElectronico_ As DocumentoElectronico, ruta_ As Integer) As ValidatorReport Implements ICubeController.ValidateFields
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

            _enlaceDatos.GetMongoCollection(Of room)("", rolId_).Aggregate.
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

    Function FindSignatureRule(listacontextos_ As List(Of roomcontext), rules_ As String, firma_ As String) As String

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

                _enlaceDatos.GetMongoCollection(Of room)("", rolId_).Aggregate.
                              Match(Function(s) s.contenttype.Equals("operando")).ToList.
                              ForEach(Sub(room_)

                                          ' If(room_)

                                          operands_.AddRange(New List(Of String) From {room_.roomname.Substring(room_.roomname.IndexOf(".") + 1), FindSignatureRule(room_.addresses, room_.rules, firma_).Replace("[13]", Chr(13))})

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

        Dim rooms_ As New List(Of room)

        Dim sax_ = SwicthedProjectSax(16)


        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos



            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)



            _enlaceDatos.GetMongoCollection(Of room)("", rolId_).
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

        Dim updateRaiz_ As Boolean = False

        If contenttype_.ToUpper = "FÓRMULA" OrElse contenttype_ = "fórmula" Then

            contenttype_ = "formula"

        End If

        If roomRules_.IndexOf(".csv") > -1 Then

            contenttype_ = "csv"

            roomRules_ = GetCsv(roomName_, roomRules_).ObjectReturned

        Else



            params_.AddRange(_interpreter.GetParams(roomRules_))

        End If

        Dim outputDatabaseName_ As String = ""

        Dim outputCollectionName_ As String = ""

        Dim sax_ = SwicthedProjectSax(16)

        Dim valorPresentacion_ As String = roomName_

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos

            Dim rolId_ = GetRolCubeId(cubeDestin_)

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)

            Dim operationsDB_ = enlaceDatos_.GetMongoCollection(Of room)("", rolId_)

            Dim roomHistoryList_ As New List(Of roomhistory)

            Dim roomawait_ As New List(Of roomhistory)

            Dim newObjectId_ = ObjectId.GenerateNewId

            Dim roomCurrent_ As room = Nothing

            Dim roomfoud_ As Boolean = False

            operationsDB_.Aggregate.
                          Match(Function(e) e.roomname.Equals(cubeDestin_.ToUpper & "." & roomName_.ToUpper.Replace(" ", "").Replace(Chr(160), "").Replace(Chr(13), "").Replace(Chr(10), "")) Or e._id = idRoom_).
                          ToList.ForEach(Sub(rooms_)

                                             If (rooms_._id <> idRoom_) Then

                                                 roomfoud_ = True

                                             Else

                                                 Dim roomHistory_ As New roomhistory With {._id = ObjectId.GenerateNewId,
                                           .rules = roomRules_,
                                           .roomname = cubeDestin_.ToUpper & "." & roomName_.ToUpper.Replace(" ", "").Replace(Chr(160), "").Replace(Chr(13), "").Replace(Chr(10), ""),
                                           .description = descriptionRules_,
                                           .addresses = New List(Of roomcontext) From {New roomcontext With
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
                                            },
                                           .messages = rooms_.messages,
                                           .status = status_,
                                           .contenttype = contenttype_.ToLower,
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

                                                     roomawait_.Add(roomHistory_)



                                                 Else

                                                     roomawait_.AddRange(rooms_.awaitingupdates)

                                                     roomawait_.Insert(0, roomHistory_)

                                                 End If

                                                 newObjectId_ = rooms_._id

                                                 roomawait_(0).status = enviado_

                                                 rooms_.awaitingupdates = roomawait_

                                                 roomCurrent_ = rooms_

                                             End If





                                         End Sub)

            If roomfoud_ Then

                _status = New TagWatcher

                SwicthedProjectSax(13)



                _status.ObjectReturned = roomCurrent_

                _status.SetOK()


            Else

                If roomCurrent_ Is Nothing Then

                    roomCurrent_ = New room With
                                   {._id = newObjectId_,
                                     .roomname = cubeDestin_ & "." & roomName_.ToUpper.Replace(" ", "").Replace(Chr(160), "").Replace(Chr(13), "").Replace(Chr(10), ""),
                                     .rules = roomRules_,
                                     .description = descriptionRules_,
                                     .required = True,
                                     .fieldsrequired = New List(Of String),
                                     .type = "warning",
                                     .addresses = New List(Of roomcontext) From
                                                                       {
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
                                                                        },
                                     .status = status_,
                                     .messages = New List(Of String),
                                     .contenttype = contenttype_.ToLower,
                                     .awaitingupdates = roomawait_,
                                     .historical = roomHistoryList_
                                     }
                    updateRaiz_ = True
                Else



                    If roomCurrent_.awaitingupdates IsNot Nothing Then

                        If (roomCurrent_.awaitingupdates.Count > 0) Then

                            Dim roomTemp_ = roomCurrent_.awaitingupdates(0)

                            If enviado_ = "on" Then

                                roomCurrent_.roomname = roomTemp_.roomname
                                roomCurrent_.rules = roomTemp_.rules
                                roomCurrent_.addresses = roomTemp_.addresses
                                roomCurrent_.description = roomTemp_.description
                                roomCurrent_.messages = roomTemp_.messages
                                roomCurrent_.contenttype = roomTemp_.contenttype
                                roomCurrent_.awaitingupdates(0).status = "on"
                                roomCurrent_.historical = roomHistoryList_
                                roomCurrent_.status = "on"

                                updateRaiz_ = True

                            End If

                        End If

                    End If




                End If


                If roomHistoryList_.Count = 0 Then

                    roomHistoryList_.Add(New roomhistory With {._id = ObjectId.GenerateNewId,
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

                    If roomCurrent_.historical Is Nothing Then

                        roomCurrent_.historical = roomHistoryList_

                    Else

                        If roomCurrent_.historical.Count = 0 Then

                            roomCurrent_.historical = roomHistoryList_

                        Else


                        End If

                    End If

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


                If roomCurrent_.awaitingupdates.Count > 3 Then

                    roomCurrent_.awaitingupdates.RemoveRange(3, roomCurrent_.awaitingupdates.Count - 3)

                End If


                Dim updateDefinition_ = Builders(Of room).
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
                                       Set(Function(e) e.awaitingupdates, roomCurrent_.awaitingupdates).
                                       Set(Function(e) e.historical, roomCurrent_.historical)

                operationsDB_.UpdateOne(Function(e) e._id = roomCurrent_._id,
                                        updateDefinition_,
                                        New UpdateOptions With {.IsUpsert = True})

                OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, 7)



                Dim operationsDBResource_ = enlaceDatos_.GetMongoCollection(Of roomresource)("", 7)



                Dim roomResource_ As New roomresource With {
                                                            ._id = ObjectId.GenerateNewId,
                                                            .roomname = roomCurrent_.roomname,
                                                            .description = roomCurrent_.description,
                                                            .status = roomCurrent_.status,
                                                            .contenttype = roomCurrent_.contenttype,
                                                            .createat = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                                            .rolid = rolId_,
                                                            .branchname = GetCubeSource(rolId_),
                                                            .idroom = roomCurrent_._id,
                                                            .username = userName_,
                                                            .valorpresentacion = valorPresentacion_
                                                             }

                Dim updateDefinitionResource_ = Builders(Of roomresource).
                                        Update.
                                       Set(Function(e) e.roomname, roomResource_.roomname.ToUpper).
                                       Set(Function(e) e.description, roomResource_.description).
                                       Set(Function(e) e.status, roomResource_.status).
                                       Set(Function(e) e.contenttype, roomResource_.contenttype).
                                       Set(Function(e) e.createat, roomResource_.createat).
                                       Set(Function(e) e.rolid, roomResource_.rolid).
                                       Set(Function(e) e.branchname, roomResource_.branchname).
                                       Set(Function(e) e.idroom, roomResource_.idroom).
                                       Set(Function(e) e.username, roomResource_.username).
                                       Set(Function(e) e.valorpresentacion, roomResource_.valorpresentacion.ToUpper)

                operationsDBResource_.UpdateOne(Function(e) e.idroom = roomResource_.idroom,
                                        updateDefinitionResource_,
                                        New UpdateOptions With {.IsUpsert = True})


                roomCurrent_.historical(0).createat = DateTime.Now

                If updateRaiz_ Then

                    Dim operationsvalidfields_ = enlaceDatos_.GetMongoCollection(Of validfields)("", 8)

                    operationsvalidfields_.
                    UpdateMany(Function(e) e.sectionfield.Equals(roomResource_.valorpresentacion.ToUpper),
                                 Builders(Of validfields).Update.Set(Function(ch) ch.status, "off"))

                End If

                _status = New TagWatcher() With {.ObjectReturned = roomCurrent_}

                SwicthedProjectSax(13)

                _status.SetOK()


            End If
        End Using





        Return _status

    End Function



    Public Sub FillRoomResource() Implements ICubeController.FillRoomResource

        Dim resourceRooms_ As New List(Of roomresource)

        Dim sax_ = SwicthedProjectSax(16)

        Dim cuenta_ = 15

        Dim rolId_ = 1

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            While rolId_ <= 6


                OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)



                _enlaceDatos.GetMongoCollection(Of room)("", rolId_).
                                  Aggregate.
                                  ToList.
                                  ForEach(Sub(room_)

                                              Dim valorPresentacion_ = room_.roomname.Substring(room_.roomname.IndexOf(".") + 1)

                                              resourceRooms_.Add(New roomresource With {
                                                                  ._id = ObjectId.GenerateNewId,
                                                                  .roomname = room_.roomname,
                                                                  .description = room_.description,
                                                                  .status = room_.status,
                                                                  .contenttype = room_.contenttype,
                                                                  .createat = DateTime.Now,
                                                                  .rolid = rolId_,
                                                                  .branchname = GetCubeSource(rolId_),
                                                                  .idroom = room_._id,
                                                                  .username = "originalfourier@gmail.com",
                                                                  .valorpresentacion = valorPresentacion_
                                                                   })


                                          End Sub)

                rolId_ += 1

            End While

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, 7)

            For Each resourceRoom_ In resourceRooms_

                _enlaceDatos.GetMongoCollection(Of roomresource)("", 7).InsertOne(resourceRoom_)

            Next



        End Using


        SwicthedProjectSax(13)

    End Sub

    Public Sub UpdateRoomResource() Implements ICubeController.UpdateRoomResource

        Dim resourceRooms_ As New List(Of roomresource)

        Dim sax_ = SwicthedProjectSax(16)

        Dim cuenta_ = 15

        Dim rolId_ = 1

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            While rolId_ <= 6


                OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)



                _enlaceDatos.GetMongoCollection(Of room)("", rolId_).
                                  Aggregate.
                                  ToList.
                                  ForEach(Sub(room_)

                                              Dim valorPresentacion_ = room_.roomname.Substring(room_.roomname.IndexOf(".") + 1)

                                              resourceRooms_.Add(New roomresource With {
                                                                  ._id = ObjectId.GenerateNewId,
                                                                  .roomname = room_.roomname,
                                                                  .description = room_.description,
                                                                  .status = room_.status,
                                                                  .contenttype = room_.contenttype,
                                                                  .createat = DateTime.Now,
                                                                  .rolid = rolId_,
                                                                  .branchname = GetCubeSource(rolId_),
                                                                  .idroom = room_._id,
                                                                  .username = "originalfourier@gmail.com",
                                                                  .valorpresentacion = valorPresentacion_
                                                                   })


                                          End Sub)

                rolId_ += 1

            End While


            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, 7)

            Dim operationsDBResource_ = _enlaceDatos.GetMongoCollection(Of roomresource)("", 7)

            For Each resourceRoom_ In resourceRooms_


                Dim updateDefinitionResource_ = Builders(Of roomresource).
                                        Update.
                                       Set(Function(e) e.roomname, resourceRoom_.roomname.ToUpper).
                                       Set(Function(e) e.valorpresentacion, resourceRoom_.valorpresentacion.ToUpper)

                operationsDBResource_.UpdateOne(Function(e) e.idroom = resourceRoom_.idroom,
                                        updateDefinitionResource_,
                                        New UpdateOptions With {.IsUpsert = True})

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
            conexion.GetDatabase(database_.name).GetCollection(Of room)(roots_(0).linkedresources(0).name).Aggregate.
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

        _roomsResource = New List(Of roomresource)

        Dim sax_ = SwicthedProjectSax(16)

        Dim rolId_ = 7

        _reports = New ValidatorReport

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)


            _roomsResource.AddRange(_enlaceDatos.GetMongoCollection(Of roomresource)("", rolId_).
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



                OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, _roomsResource(0).rolid)

                _rooms = New List(Of room)

                _rooms.AddRange(_enlaceDatos.GetMongoCollection(Of room)("", _roomsResource(0).rolid).
                                                     Aggregate.
                                                     Match(Function(ch) ch._id = _roomsResource(0).idroom).
                                                     ToList)

                Dim found_ = True

                Dim mensaje_ As String = ""

                For Each param_ In _rooms(0).addresses(1).ref.Skip(1)

                    Dim newParam_ = ""

                    For Each key_ In params_.Keys

                        If key_.Substring(0, key_.LastIndexOf(".")) = param_ Then

                            found_ = True

                            Exit For

                        Else

                            If key_ = param_ Then

                                If Not params_.ContainsKey(key_ & ".0") Then

                                    newParam_ = key_

                                    found_ = True

                                    Exit For

                                End If


                            End If

                            found_ = False

                            mensaje_ &= param_ & Chr(13)

                        End If

                    Next

                    If newParam_ <> "" Then

                        params_(newParam_ & ".0") = params_(newParam_)

                        params_.Remove(newParam_)

                    End If

                Next

                If found_ Then

                    If _interpreter Is Nothing Then

                        _interpreter = New MathematicalInterpreterNCalc

                        GetFieldsNamesResource()

                    End If



                    _status = New TagWatcher() With {.ObjectReturned = _interpreter.RunExpression(Of T)(_rooms(0).rules, params_)}

                    _status.SetOK()


                    If TypeOf _status.ObjectReturned Is Dictionary(Of String, String) Then

                        _reports.result = New List(Of String)

                        For Each key_ In status.ObjectReturned.Keys

                            _reports.result.Add("[" & key_ & "," & status.ObjectReturned(key_))

                        Next


                    Else

                        _reports.result = New List(Of String)

                        If TypeOf _status.ObjectReturned Is List(Of String) Then



                            _reports.result = _status.ObjectReturned


                        Else

                            _reports.result.Add(_status.ObjectReturned)

                        End If

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

                    _reports.ShowMessageError()

                End If


            End If


        End Using


        SwicthedProjectSax(13)


        Return _reports

    End Function

    Public Function GetRoomNames(Optional token_ As String = "") As TagWatcher Implements ICubeController.GetRoomNames

        _rooms = New List(Of room)

        Dim sax_ = SwicthedProjectSax(16)

        Dim cuenta_ = 15

        Dim rolId_ = 1

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            While rolId_ <= 6 And cuenta_ > 0


                OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)
                If token_ = "" Then

                    _rooms.AddRange(_enlaceDatos.GetMongoCollection(Of room)("", rolId_).
                                      Aggregate.
                                      Limit(cuenta_).
                                      ToList)
                Else

                    _rooms.AddRange(_enlaceDatos.GetMongoCollection(Of room)("", rolId_).
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

    Function GetRoomNamesResource(Optional token_ As String = "", Optional typeSearch_ As Int16 = 1) As TagWatcher Implements ICubeController.GetRoomNamesResource
        _roomsResource = New List(Of roomresource)

        Dim sax_ = SwicthedProjectSax(16)

        Dim cuenta_ = 15

        Dim rolId_ = 7

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)

            If token_ = "" Then
                _roomsResource.AddRange(_enlaceDatos.GetMongoCollection(Of roomresource)("", rolId_).
                                      Aggregate.
                                      Limit(cuenta_).
                                      ToList)
            Else

                If typeSearch_ = 1 Then

                    _roomsResource.AddRange(_enlaceDatos.GetMongoCollection(Of roomresource)("", rolId_).
                                     Aggregate.
                                     Match(Function(ch) ch.valorpresentacion.ToUpper.Contains(token_.ToUpper) Or ch.branchname.Equals(token_)).
                                     Limit(cuenta_).
                                     ToList)

                Else

                    _roomsResource.AddRange(_enlaceDatos.GetMongoCollection(Of roomresource)("", rolId_).
                                 Aggregate.
                                 Match(token_.Replace("^", "").Replace(" ", "")).
                                 Limit(cuenta_).
                                 ToList)

                End If



                '_roomsResource.AddRange(_enlaceDatos.GetMongoCollection(Of roomresource)("", rolId_).
                '                 Aggregate.
                '                 Match(Function(ch) ch.valorpresentacion.ToUpper.Contains(token_.ToUpper) Or ch.branchname.Equals(token_)).
                '                 Limit(cuenta_).
                '                 ToList)

            End If




        End Using


        SwicthedProjectSax(13)

        _status = New TagWatcher() With {.ObjectReturned = _roomsResource}

        Return _status

    End Function

    Function SetFieldsNamesResource(Section_ As String, Campo_ As String) As TagWatcher

        Dim validfields_ = New List(Of validfields)

        Dim sax_ = SwicthedProjectSax(16)

        Dim cuenta_ = 15

        Dim rolId_ = 7

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)

            Dim operationsvalidfields_ = _enlaceDatos.GetMongoCollection(Of validfields)("", 8)

            Dim sections_ = Section_.Split(",")

            For Each sectionAux_ In sections_


                Dim numsection_ = sectionAux_.Replace(" ", "").Replace(",", "").Replace(Chr(34), "")

                Dim indexSection_ = numsection_.IndexOf("S")

                If indexSection_ <> -1 Then


                    numsection_ = numsection_.Substring(indexSection_ + 1)

                    validfields_.Clear()

                    validfields_.AddRange(operationsvalidfields_.
                                                 Aggregate.
                                                 Match(Function(ch) ch.valorpresentacion.Equals("S" & numsection_ & "." & Campo_)).
                                                 ToList)




                    If validfields_.Count = 0 Then

                        validfields_.Add(New validfields With {
                                                            ._id = ObjectId.GenerateNewId,
                                                            .section = "ANS" & numsection_,
                                                            .sectionexcel = "S" & numsection_,
                                                            .sectionfield = Campo_,
                                                            .valorpresentacion = "S" & numsection_ & "." & Campo_,
                                                            .details = Nothing,
                                                            .status = "on",
                                                            .contentype = "PEDIMENTO",
                                                            .archivado = False,
                                                            .estado = 1
                                                             })

                        Dim updateDefinition_ = Builders(Of validfields).
                                       Update.
                                      Set(Function(e) e.section, validfields_(0).section).
                                      Set(Function(e) e.sectionexcel, validfields_(0).sectionexcel).
                                      Set(Function(e) e.sectionfield, validfields_(0).sectionfield).
                                      Set(Function(e) e.valorpresentacion, validfields_(0).valorpresentacion).
                                      Set(Function(e) e.status, validfields_(0).status).
                                      Set(Function(e) e.contentype, validfields_(0).contentype).
                                      Set(Function(e) e.archivado, validfields_(0).archivado).
                                      Set(Function(e) e.estado, validfields_(0).estado)

                        operationsvalidfields_.UpdateOne(Function(e) e._id = validfields_(0)._id,
                                        updateDefinition_,
                                        New UpdateOptions With {.IsUpsert = True})

                    End If

                End If

            Next

        End Using


        SwicthedProjectSax(13)

        _status = New TagWatcher() With {.ObjectReturned = validfields_}

        Return _status

    End Function

    Function GetValidFieldsOn(sentence_ As String) As TagWatcher Implements ICubeController.GetValidFieldsOn

        Dim validfields_ = New List(Of String)

        Dim sax_ = SwicthedProjectSax(16)

        Dim filtro_ As FilterDefinition(Of validfields) = Builders(Of validfields).Filter.Eq(Of String)("status", "on")

        For Each word_ In sentence_.Split(" ")

            filtro_ = filtro_ And Builders(Of validfields).Filter.Regex("sectionfield", New BsonRegularExpression(word_, "i"))

        Next

        _fieldmiss = New List(Of String)

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, 7)

            _enlaceDatos.GetMongoCollection(Of validfields)("", 8).
                         Aggregate.Match(filtro_).ToList.ForEach(Sub(campos_)

                                                                     If validfields_.IndexOf(campos_.sectionfield) = -1 Then

                                                                         validfields_.Add(campos_.sectionfield)

                                                                     End If

                                                                 End Sub)

        End Using


        SwicthedProjectSax(13)

        _status = New TagWatcher() With {.ObjectReturned = validfields_}

        Return _status

    End Function

    Function SetFieldsNamesResourceOff(campo_ As String) As TagWatcher

        Dim validfields_ = New List(Of String)

        Dim sax_ = SwicthedProjectSax(16)

        Dim cuenta_ = 15

        Dim rolId_ = 7

        _fieldmiss = New List(Of String)

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)

            Dim operationsvalidfields_ = _enlaceDatos.GetMongoCollection(Of validfields)("", 8)




            _enlaceDatos.GetMongoCollection(Of roomresource)("", rolId_).
                         Aggregate.ToList.ForEach(Sub(campos_)
                                                      operationsvalidfields_.
                                                      UpdateMany(Function(e) e.sectionfield.Equals(campos_.valorpresentacion),
                                                                   Builders(Of validfields).Update.Set(Function(ch) ch.status, "off"))
                                                  End Sub)

        End Using




        SwicthedProjectSax(13)



        _interpreter.SetValidFields(validfields_)

        _status = New TagWatcher() With {.ObjectReturned = validfields_}

        Return _status

    End Function

    Function GetFieldsNamesResource() As TagWatcher Implements ICubeController.GetFieldsNamesResource

        Dim validFields_ As New List(Of String)

        Dim sax_ = SwicthedProjectSax(16)

        Dim cuenta_ = 15

        Dim rolId_ = 7

        _fieldmiss = New List(Of String)

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)

            Dim operationsvalidfields_ = _enlaceDatos.GetMongoCollection(Of validfields)("", 8)

            operationsvalidfields_.
                          Aggregate.
                          ToList.ForEach(Sub(campo_)

                                             validFields_.Add(campo_.valorpresentacion)

                                             If campo_.status = "on" Then

                                                 If _fieldmiss.IndexOf(campo_.sectionfield) = -1 Then

                                                     _fieldmiss.Add(campo_.sectionfield)

                                                 End If


                                             End If


                                         End Sub)




        End Using




        SwicthedProjectSax(13)



        _interpreter.SetValidFields(validFields_)

        _status = New TagWatcher() With {.ObjectReturned = validFields_}

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

            Dim currentLine_ As String() = lines_(fila_).Split(New String() {","}, 2, StringSplitOptions.None)

            For column_ As Integer = 0 To headers_.Length - 1

                dictionaryLines_.Add(headers_(column_),
                                         currentLine_(column_))

            Next

            result_.Add(dictionaryLines_)

        Next

        For Each elemento_ In result_

            SetFieldsNamesResource(elemento_("ID Seccion"), elemento_("Nombre Sistema"))
        Next
        ' SetFieldsNamesResourceOff()
        Return ""

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


    'Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
    '    ' Activar el sistema de búsqueda de palabras clave
    '    Dim palabrasSugeridas As List(Of String) = BuscarPalabrasClave(TextBox1.Text)

    '    ' Mostrar las palabras sugeridas en un diálogo
    '    Dim dialogo As New Dialog
    '    Dim listaPalabras As New ListBox()
    '    listaPalabras.Items.AddRange(palabrasSugeridas.ToArray())
    '    dialogo.Controls.Add(listaPalabras)
    '    dialogo.ShowDialog()

    '    ' Si el usuario selecciona una palabra clave, insertarla en el código
    '    If dialogo.DialogResult = DialogResult.OK Then
    '        Dim palabraSeleccionada As String = listaPalabras.SelectedItem
    '        TextBox1.Text &= palabraSeleccionada
    '    End If
    'End Sub

    'Private Function BuscarPalabrasClave(texto As String) As List(Of String)
    '    ' Implementar la lógica de búsqueda de palabras clave
    '    ' ...
    'End Function

#End Region

End Class
