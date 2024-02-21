Imports Cube.Interpreters
Imports Cube.ValidatorReport
Imports gsol.basededatos
Imports gsol.krom
Imports Microsoft.SqlServer.Server
Imports Microsoft.VisualBasic.ApplicationServices
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
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

    Private _status As TagWatcher

    Private _reports As ValidatorReport



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

        _status = New TagWatcher()

        _status.ObjectReturned = parametros_


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

        _status = New TagWatcher()

        _status.ObjectReturned = operands_


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


    Public Function SetFormula(Of T)(roomName_ As String, roomRules_ As String, cubeDestin_ As String, contenttype_ As String) As TagWatcher Implements ICubeController.SetFormula

        Dim params_ As New List(Of String) From {cubeDestin_ & "." & roomName_}

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

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos

            Dim rolId_ = GetRolCubeId(cubeDestin_)

            OnRol(sax_.SaxSettings(1).servers.nosql.mongodb.rol, rolId_)

            Dim iconexionesNoSQL_ As IConexionesNoSQL = New ConexionesNoSQL

            Dim operationsDB_ = _enlaceDatos.GetMongoCollection(Of Room)("", rolId_)

            Dim room_ = New Room With
                                {._id = ObjectId.GenerateNewId,
                                  .roomname = cubeDestin_ & "." & roomName_,
                                  .rules = roomRules_,
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
                                  .status = Nothing,
                                  .messages = New List(Of String),
                                  .contenttype = contenttype_.ToLower
                                  }

            Dim updateDefinition_ = Builders(Of Room).
                                    Update.
                                   Set(Function(e) e.roomname, room_.roomname).
                                   Set(Function(e) e.rules, room_.rules).
                                   Set(Function(e) e.required, room_.required).
                                   Set(Function(e) e.fieldsrequired, room_.fieldsrequired).
                                   Set(Function(e) e.type, room_.type).
                                   Set(Function(e) e.addresses, room_.addresses).
                                   Set(Function(e) e.status, room_.status).
                                   Set(Function(e) e.messages, room_.messages).
                                   Set(Function(e) e.contenttype, room_.contenttype)

            operationsDB_.UpdateOne(Function(e) e.roomname.Equals(cubeDestin_ & "." & roomName_),
                                    updateDefinition_,
                                    New UpdateOptions With {.IsUpsert = True})

        End Using

        SwicthedProjectSax(13)

        _status = New TagWatcher()

        _status.SetOK()

        Return _status

    End Function

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
                                                 Match(Function(ch) ch.roomname.Contains(token_)).
                                                 Limit(cuenta_).
                                                 ToList)

                End If


                cuenta_ = cuenta_ - _rooms.Count

                rolId_ += 1

            End While

        End Using


        SwicthedProjectSax(13)

        _status = New TagWatcher()

        _status.ObjectReturned = _rooms

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
