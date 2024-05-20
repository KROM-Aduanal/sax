Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Syn.Documento
Imports Wma.Exceptions

Public Interface ICubeController : Inherits IDisposable

#Region "Enum"


    Enum ContainedCubes
        SinDefinir = 0
        A22 = 1
        VOCE = 2
        UCAA = 3
        UAA = 4
        UCC = 5
        CDI = 6
        TODOS = 7
    End Enum

    Enum CubeErrorTypes

        Undefined = 0

        RegimenClaveOperacion = 1

        IncotermIncrementables = 2
    End Enum

#End Region


#Region "Properties"

    Property scope As List(Of ContainedCubes)

    Property rooms As List(Of Room)

    Property status As TagWatcher

    Property reports As ValidatorReport

    Property fieldmiss As List(Of String)


#End Region

#Region "Methods"


    Function ValidateFields(Of T)(values_ As Dictionary(Of String, T)) As ValidatorReport

    Function ValidateFields(Of T)(documentoElectronico_ As DocumentoElectronico) As ValidatorReport

    Function SetCsv(roomName_ As String, csvFilePath_ As String) As TagWatcher

    Function SetJson(roomName_ As String, jsonFilePath_ As String) As TagWatcher

    Function GetCsv(roomName_ As String, csvFilePath_ As String) As TagWatcher

    Function GetJson(roomName_ As String, jsonFilePath_ As String) As TagWatcher

    Function GetFormula(roomName_ As String) As TagWatcher

    Function GetOperands(Optional firma_ As String = "") As TagWatcher

    Function SetFormula(Of T)(idRoom_ As ObjectId, roomName_ As String, roomRules_ As String, cubeDestin_ As String, contenType_ As String, descriptionRules_ As String, status_ As String, Optional idUser_ As ObjectId = Nothing, Optional userName_ As String = "", Optional enviado_ As String = "unsent", Optional reason_ As String = "") As TagWatcher

    ' Function SetFormula(Of T)(idRoom_ As ObjectId, NewroomName_ As String, roomRules_ As String, cubeDestin_ As String, contenttype_ As String, descriptionRules_ As String, status_ As String, Optional idUser_ As ObjectId = Nothing, Optional userName_ As String = "") As TagWatcher

    Function RunRoom(Of T)(roomName_ As String, params_ As String) As ValidatorReport

    Function GetReports() As ValidatorReport

    Function GetReports(roomName_ As String) As ValidatorReport

    Function GetStatus(_idPedimento As ObjectId) As ValidatorReport

    Function GetRoomNames(Optional token_ As String = "") As TagWatcher

    Function GetRoomNamesResource(Optional token_ As String = "", Optional typeSearch_ As Int16 = 1) As TagWatcher

    Function GetFieldsNamesResource() As TagWatcher

    Function GetRoom(idRoom_ As ObjectId, rolId_ As Int32) As TagWatcher

    Function RunRoom(Of T)(roomname_ As String, params_ As Dictionary(Of String, T)) As ValidatorReport

    Function ActualizaClase(Of T)(Origen As String) As T

    Function CamposExcelMongo(excelFilePath_ As String) As String

    Sub FillRoomResource()

    Sub UpdateRoomResource()

#End Region

End Interface
