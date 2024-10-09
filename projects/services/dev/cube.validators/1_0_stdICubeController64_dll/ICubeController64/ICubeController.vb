Imports Cube.Interpreters
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Syn.Documento
Imports Wma.Exceptions

Public Interface ICubeController : Inherits IDisposable

#Region "Enum"

    Enum CubeSlices

        Undefined = 0

        A22 = 1

        VOCE = 2

        UCAA = 3

        UAA = 4

        UCC = 5

        CDI = 6

        PREV = 8

        TODOS = 15

    End Enum

    Enum ContentTypes

        Undefined = 0

        Formula = 1

        Operando = 2

    End Enum

    Enum CubeErrorTypes

        Undefined = 0

        UndefidedRoom = 1

        UndefinedContentType = 2

        ErrorRuleRules = 3

        UndefinedContext = 4

        ErrorConnection = 5

        UndefinedCube = 6

    End Enum

    Enum TypeSearch

        Undefined = 0

        ValorPresentacionBranchName = 1

        Free = 2

    End Enum

    Enum RootCube

        Undefined = 0

        RoomsA22 = 1

        RoomsVOCE = 2

        RoomsUAA = 3

        RoomsUCAA = 4

        RoomsUCC = 5

        RoomsCDI = 6

        RoomNames = 7

        ValidFields = 8

        ValidationPanel = 9

        RoomsPREV = 10

    End Enum

    Enum EnlaceSax

        Undefined = 0

        Synapsis = 13

        Cube = 16

    End Enum

    Enum Operational

        Offline = 0

        Online = 1

    End Enum

    Enum UseType

        Undefined = 0

        MOTOR = 1

        VALIDATION = 2

        ASSISTANCE = 3

    End Enum

#End Region


#Region "Properties"

    Property scope As List(Of CubeSlices)

    Property rooms As List(Of room)

    ReadOnly Property status As TagWatcher

    ReadOnly Property reports As ValidatorReport

    ReadOnly Property fieldmiss As List(Of String)

    Property interpreter As IMathematicalInterpreter

    Property limit As Int32


#End Region

#Region "Methods"

    Function Clone() As Object

    Function GetFormula(roomName_ As String) As TagWatcher

    Function GetReports() As ValidatorReport

    Function GetReports(roomName_ As String) As ValidatorReport

    Function GetRoom(idRoom_ As ObjectId, rolId_ As Int32) As TagWatcher

    Function GetRoomNamesResource(Optional token_ As String = Nothing,
                                  Optional typeSearch_ As TypeSearch = TypeSearch.ValorPresentacionBranchName) As TagWatcher
    Function GetRoomNamesResource(awatingaproval_ As Boolean,
                                  Optional token_ As String = Nothing,
                                  Optional typeSearch_ As TypeSearch = TypeSearch.ValorPresentacionBranchName) As TagWatcher
    Function GetSectionsResource(fieldName_ As String) As TagWatcher

    Function GetValidFieldsOn(sentence_ As String) As TagWatcher

    Function RunAssistance(Of T)(roomname_ As String,
                           params_ As Dictionary(Of String, T)) As TagWatcher

    Function RunRoom(Of T)(roomname_ As String,
                           params_ As Dictionary(Of String, T),
                           Optional useType_ As ICubeController.UseType = ICubeController.UseType.Undefined,
                           Optional ByRef requieredfields_ As List(Of String) = Nothing,
                           Optional preferIndex_ As Int32? = Nothing) As ValidatorReport

    Function RunTransaction(idRoom_ As ObjectId,
                            roomName_ As String,
                            roomRules_ As String,
                            cubeSliceType_ As ICubeController.CubeSlices,
                            contenttype_ As ICubeController.ContentTypes,
                            descriptionRules_ As String,
                            status_ As String,
                            useType_ As UseType,
                            messages_ As List(Of String),
                            Optional idUser_ As ObjectId = Nothing,
                            Optional userName_ As String = Nothing,
                            Optional enviado_ As String = "unsent",
                            Optional reason_ As String = Nothing) As TagWatcher


    'Sub SetValidField(field_ As String)
    Function CamposExcelMongo(excelFilePath_ As String) As String



#End Region

End Interface
