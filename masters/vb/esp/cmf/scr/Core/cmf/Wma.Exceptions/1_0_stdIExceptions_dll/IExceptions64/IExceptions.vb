Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Runtime.Serialization
Imports System.ServiceModel

Namespace Wma.Exceptions

    <ServiceContract(namespace:="http://ws.kromaduanal.com/WebServices/")> _
    <XmlSerializerFormat>
    Public Interface IExceptions

#Region "Attributes"


#End Region

#Region "Enums"

        <DataContract()>
        Enum ErrorTypes
            <EnumMember> <Description("Empty")> WS000 = 0
            <EnumMember> <Description("Error:WS-001, Mobile user was not found")> WS001 = 1
            <EnumMember> <Description("Error:WS-002, Information not available")> WS002 = 2
            <EnumMember> <Description("Error:WS-003, Data request was rejected due to authentication errors")> WS003 = 3
            <EnumMember> <Description("Error:WS-004, Data request was rejected due to authentication errors")> WS004 = 4
            <EnumMember> <Description("Error:WS-005, Data request was rejected due to authentication errors")> WS005 = 5
            <EnumMember> <Description("Error:WS-006, Query returns 0 rows")> WS006 = 6
            <EnumMember> <Description("Error:WS-007: Mobile user do not have client resources defined to access")> WS007 = 7
            <EnumMember> <Description("Error:WS-008: Not enough rules to data access")> WS008 = 8
            <EnumMember> <Description("Error:WS-009: -")> WS009 = 9
        End Enum

        '<DataContract()>
        'Enum TypeApplication
        '    <EnumMember> <Description("Undefined")> UND = 0
        '    <EnumMember> <Description("KromBase")> KBA = 1
        '    <EnumMember> <Description("MobileApp")> MAP = 2
        '    <EnumMember> <Description("Extranet")> EXT = 3
        'End Enum

        '<DataContract()>
        'Enum ModuleID
        '    <EnumMember> <Description("Undefined")> UND = 0
        'End Enum

        <DataContract()>
        Enum TypeStatus
            <EnumMember> Empty
            <EnumMember> Ok
            <EnumMember> Errors
            <EnumMember> Truncated
        End Enum

#End Region



#Region "Properties"

        <DataMember>
        Property Status As TypeStatus

        <DataMember, XmlAttribute>
        Property Errors As ErrorTypes


        <DataMember, XmlAttribute>
        Property ResultsCount As Integer


        <DataMember>
        Property ErrorDescription As String

#End Region

#Region "Methods"

        Sub SetOK(Optional ByVal resultsCont_ As Integer = 0)

        Sub SetError(Optional ByVal error_ As ErrorTypes = 0, _
                     Optional ByVal description_ As String = Nothing)


        Sub SetResults(ByVal result_ As TypeStatus, _
                       ByVal resultsCont_ As Integer, _
                       ByVal errorType_ As ErrorTypes)


        Function GetEnumDescription(ByVal EnumConstant As [Enum]) As String


#End Region



    End Interface

End Namespace
