Imports gsol.BaseDatos.Operaciones
Imports System.Net.Security
Imports System.ComponentModel
Imports System.Reflection
Imports System.Xml.Serialization

Namespace Wma.WebServices

    ' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    'Name := "Customer", [Namespace] := "http://www.contoso.com")
    '<ServiceKnownType("GetKnownTypes", GetType(Helper)), ServiceContract()> _

    <ServiceContract()>
    <XmlSerializerFormat>
    Public Interface IWSSesion

        Enum SessionTypes
            WebServiceCredential
            MovilUserCredential
            StandartCredential
        End Enum

        '<OperationContract()>
        '<OperationContract(Name:="TesterDataConnection", ProtectionLevel:=ProtectionLevel.EncryptAndSign)>
        <OperationContract()>
        Function TesterService42(ByVal YourName As String) As String


        <ServiceKnownType(GetType(UserProfile))>
        <ServiceKnownType(GetType(AboutAsking))>
        <OperationContract()>
        <XmlSerializerFormat>
        Function GetProfile(ByVal ThroughUserPermissions As String, _
                            ByVal WebServiceUserID As String, _
                            ByVal WebServicePasswordID As String, _
                            ByVal IdRequiredApplication As Integer, _
                            ByVal CorporateNumber As Integer, _
                            ByVal CompanyId As Integer, _
                            Optional ByVal FullAuthentication As Boolean = False) As UserProfile

    End Interface



    ' Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    ' Puede agregar archivos XSD al proyecto. Después de compilar el proyecto, puede usar directamente los tipos de datos definidos aquí, con el espacio de nombres "IWSSesion64.ContractType".

    ''<AttributeUsageAttribute(AttributeTargets.Class)>

    <DataContract()>
    <XmlSerializerFormat>
    Public Class AboutAsking

#Region "Attributes"

        Private _status As TypeStatus

        Private _error As ErrorTypes

        Private _resultsCont As Integer

        Public Enum ErrorTypes
            <Description("Empty")> WS000 = 0
            <Description("Error:WS-001, Information not available")> WS001 = 1
            <Description("Error:WS-002, Information not available")> WS002 = 2
            <Description("Error:WS-003, Second regnition reject data request")> WS003 = 3
            <Description("Error:WS-004, Second regnition reject data request")> WS004 = 4
            <Description("Error:WS-005, First regnition reject data request")> WS005 = 5
        End Enum

        Public Enum TypeStatus
            Empty
            Ok
            Errors
            Truncated
        End Enum

#End Region

#Region "Builders"
        Sub New()

            _resultsCont = 0

            _status = TypeStatus.Empty

            _error = ErrorTypes.WS000

        End Sub
#End Region


#Region "Properties"

        '<DataMember()>
        <DataMember, XmlText>
        Property Status As TypeStatus
            Get
                Return _status
            End Get
            Set(value As TypeStatus)
                _status = value
            End Set
        End Property

        '<DataMember()>
        <DataMember, XmlAttribute>
        Property Errors As ErrorTypes

            Get
                Return _error
            End Get
            Set(value As ErrorTypes)
                _error = value
            End Set

        End Property

        '<DataMember()>
        <DataMember, XmlAttribute>
        Property ResultsCount As Integer
            Get
                Return _resultsCont
            End Get
            Set(value As Integer)
                _resultsCont = value
            End Set
        End Property

#End Region


#Region "Methods"

        <OperationContract()>
        Public Function ErrorDescription(ByVal errorID_ As ErrorTypes) As String

            Return GetEnumDescription(DirectCast(Convert.ToInt32(errorID_), ErrorTypes))

        End Function

        Public Function GetEnumDescription(ByVal EnumConstant As [Enum]) As String
            Dim fi As FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
            Dim attr() As DescriptionAttribute = _
                          DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), _
                          False), DescriptionAttribute())

            If attr.Length > 0 Then
                Return attr(0).Description
            Else
                Return EnumConstant.ToString()
            End If
        End Function

#End Region







    End Class

    '<DataContract()>
    <DataContract(Name:="UserProfile", [Namespace]:="http://www.10.66.1.150/WebServices/")> _
    <AttributeUsageAttribute(AttributeTargets.Class)>
    Public Class UserProfile

#Region "Attributes"

        Public Enum SessionTypes
            Standard
            Platino
            Golden
        End Enum

        Private _nombre As String

        Private _apellidoPaterno As String

        Private _apellidoMaterno As String

        Private _fechaRegistro As Date

        Private _tipoSesion As SessionTypes

        Private _result As AboutAsking

#End Region

#Region "Builders"


        Sub New()

            _result = Nothing

            _nombre = Nothing

            _apellidoPaterno = Nothing

            _apellidoMaterno = Nothing

            _fechaRegistro = Now

            _tipoSesion = SessionTypes.Standard

            _result = New AboutAsking

        End Sub

#End Region

#Region "Properties"

        'setters, they doesn't work

        'getters
        '<DataMember()>
        <DataMember(Name:="ObjectResult")> _
        Public Property Result As AboutAsking

            Set(value As AboutAsking)

                _result = value

            End Set

            Get
                Return _result

            End Get

        End Property

        <DataMember()>
        Public Property Nombre As String
            Set(value As String)
                _nombre = value
            End Set
            Get
                Return _nombre
            End Get
        End Property

        <DataMember()>
        Public Property ApellidoMaterno As String
            Set(value As String)
                _apellidoMaterno = value
            End Set
            Get
                Return _apellidoMaterno
            End Get
        End Property

        <DataMember()>
        Public Property ApellidoParterno As String
            Set(value As String)
                _apellidoPaterno = value
            End Set
            Get
                Return _apellidoPaterno
            End Get

        End Property

        <DataMember()>
        Public Property FechaRegistro As Date

            Set(value As Date)
                _fechaRegistro = value
            End Set
            Get
                Return _fechaRegistro
            End Get

        End Property

#End Region


#Region "Methods"

        Sub SetOK(Optional ByVal resultsCont_ As Integer = 0)

            _result.Status = AboutAsking.TypeStatus.Ok

            _result.ResultsCount = resultsCont_

            _result.Errors = AboutAsking.ErrorTypes.WS000

        End Sub

        Sub SetError(Optional ByVal error_ As AboutAsking.ErrorTypes = 0)

            _result.Status = AboutAsking.TypeStatus.Errors

            _result.ResultsCount = 0

            _result.Errors = error_

        End Sub

        Sub SetResults(ByVal result_ As AboutAsking.TypeStatus, _
                       ByVal resultsCont_ As Integer, _
                       ByVal errorType_ As AboutAsking.ErrorTypes)

            _result.Status = result_

            _result.ResultsCount = resultsCont_

            _result.Errors = errorType_

        End Sub

#End Region

    End Class

End Namespace