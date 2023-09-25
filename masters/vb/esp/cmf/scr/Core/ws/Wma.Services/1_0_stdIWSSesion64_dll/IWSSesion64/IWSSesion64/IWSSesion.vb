
Imports System.Net.Security
Imports System.ComponentModel
Imports System.Reflection
Imports System.Xml.Serialization
Imports Wma.Exceptions
Imports Wma.WebServices

'Namespace Wma.WebServices

<ServiceContract()>
<XmlSerializerFormat>
Public Interface IWSSesion


#Region "Enums"

    Enum SessionTypes
        WebServiceCredential
        MovilUserCredential
        StandartCredential
    End Enum

#End Region

    <OperationContract()>
    Function TesterService(ByVal YourName As String) As String



    <ServiceKnownType(GetType(UserProfile))>
    <ServiceKnownType(GetType(TagWatcher))>
    <OperationContract()>
    <XmlSerializerFormat>
    Function GetProfile(ByVal MobileUserID As String, _
                            ByVal WebServiceUserID As String, _
                            ByVal WebServicePasswordID As String, _
                            Optional ByVal IdRequiredApplication As Integer = 4, _
                            Optional ByVal CorporateNumber As Integer = 1, _
                            Optional ByVal CompanyId As Integer = 0, _
                            Optional ByVal FullAuthentication As Boolean = False) As UserProfile


    <ServiceKnownType(GetType(TagWatcher))>
    <OperationContract()>
    <XmlSerializerFormat>
    Function SetBirthDate(ByVal MobileUserID As String, _
                            ByVal WebServiceUserID As String, _
                            ByVal WebServicePasswordID As String, _
                           ByVal BirthDate As String) As TagWatcher


    <ServiceKnownType(GetType(TagWatcher))>
    <OperationContract()>
    <XmlSerializerFormat>
    Function SetPhone(ByVal MobileUserID As String, _
                           ByVal WebServiceUserID As String, _
                           ByVal WebServicePasswordID As String, _
                           ByVal Phone As String) As TagWatcher

    <ServiceKnownType(GetType(TagWatcher))>
    <OperationContract()>
    <XmlSerializerFormat>
    Function SetEMail(ByVal MobileUserID As String, _
                          ByVal WebServiceUserID As String, _
                          ByVal WebServicePasswordID As String, _
                          ByVal EMail As String) As TagWatcher


    <ServiceKnownType(GetType(TagWatcher))>
    <OperationContract()>
    <XmlSerializerFormat>
    Function SetURLUserPicture(ByVal MobileUserID As String, _
                            ByVal WebServiceUserID As String, _
                            ByVal WebServicePasswordID As String, _
                            ByVal URLPicture As String) As TagWatcher



End Interface


'End Namespace