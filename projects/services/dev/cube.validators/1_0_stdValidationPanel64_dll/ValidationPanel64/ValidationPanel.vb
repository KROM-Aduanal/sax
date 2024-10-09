Imports Cube.Validators
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Wma.Exceptions



Public Class validationpanel

    Enum PanelError

        SinDefinir = 0

        Alert = 1

        Warning = 2

        Information = 3

    End Enum

    Public Property _id As ObjectId

    '<BsonRepresentation(BsonType.String)>

    <BsonIgnoreIfNull>
    Public Property fieldenum As CamposPedimento
    <BsonIgnoreIfNull>
    Public Property fieldname As String
    <BsonIgnoreIfNull>
    Public Property taked As Boolean
    <BsonIgnoreIfNull>
    Public Property route As IValidationRoute.ValidationRoutes
    <BsonIgnoreIfNull>
    Public Property description As String
    <BsonIgnoreIfNull>
    Public Property details As CreationDetails

    <BsonIgnoreIfNull>
    Public Property type As Integer
    <BsonIgnoreIfNull>
    Public Property filed As Boolean
    <BsonIgnoreIfNull>
    Public Property status As String
    <BsonIgnoreIfNull>
    Public Property estado As Int32

    Sub SetValidationPanel(field_ As CamposPedimento,
                                        route_ As IValidationRoute.ValidationRoutes,
                                        description_ As String,
                                        iduser_ As ObjectId,
                                        username_ As String,
                                        errortype_ As PanelError,
                                        enviroment_ As Integer,
                                        companyId_ As Integer,
                                        areatype_ As String,
                                        value_ As String,
                                        erroremited_ As String,
                                        rolid_ As Integer)

        _id = ObjectId.GenerateNewId

        fieldenum = field_

        fieldname = field_.ToString

        route = route_

        description = description_

        details = New CreationDetails With {._idcreation = ObjectId.GenerateNewId,
                                            .username = username_,
                                            .errortype = errortype_,
                                            .erroremited = erroremited_,
                                            .rolid = rolid_,
                                            .value = value_,
                                            .enviroment = enviroment_,
                                            .companyid = companyId_,
                                            .areatype = areatype_}


    End Sub


End Class


Public Class CreationDetails

    Public Property _idcreation As ObjectId
    <BsonIgnoreIfNull>
    Public Property _iduser As ObjectId
    <BsonIgnoreIfNull>
    Public Property username As String
    <BsonIgnoreIfNull>
    Public Property enviroment As Integer
    <BsonIgnoreIfNull>
    Public Property companyid As Integer
    <BsonIgnoreIfNull>
    Public Property areatype As String

    <BsonIgnoreIfNull>
    Public Property value As String
    <BsonIgnoreIfNull>
    Public Property erroremited As String
    <BsonIgnoreIfNull>
    Public Property errortype As Integer
    <BsonIgnoreIfNull>
    Public Property rolid As Int32


End Class
