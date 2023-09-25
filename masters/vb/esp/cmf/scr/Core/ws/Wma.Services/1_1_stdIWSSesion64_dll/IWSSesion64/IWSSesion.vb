Imports System.ServiceModel
Imports System.Net.Security
'Imports System.ComponentModel
'Imports System.Reflection
'Imports System.Xml.Serialization
Imports Wma.Exceptions

Namespace Wma.WebServices

    <ServiceContract()>
    Public Interface IWSSesion

#Region "Enums"

        Enum SessionTypes
            WebServiceCredential
            MovilUserCredential
            StandartCredential
        End Enum

#End Region

        <OperationContract()>
        <WebInvoke(Method:="GET",
                   UriTemplate:="/TesterService?YourName={YourName}",
                           RequestFormat:=WebMessageFormat.Json,
                           ResponseFormat:=WebMessageFormat.Json,
                           BodyStyle:=WebMessageBodyStyle.Bare)>
        Function TesterService(ByVal YourName As String) As String

        <ServiceKnownType(GetType(UserProfile))>
        <ServiceKnownType(GetType(TagWatcher))>
        <OperationContract()>
        <WebInvoke(Method:="GET",
                   UriTemplate:="/GetProfile?MobileUserID={MobileUserID}" & _
                                "&WebServiceUserID={WebServiceUserID}" & _
                                "&WebServicePasswordID={WebServicePasswordID}" & _
                                "&IdRequiredApplication={IdRequiredApplication}" & _
                                "&CorporateNumber={CorporateNumber}" & _
                                "&CompanyId={CompanyId}" & _
                                "&FullAuthentication_={FullAuthentication_}",
                           RequestFormat:=WebMessageFormat.Json,
                           ResponseFormat:=WebMessageFormat.Json,
                           BodyStyle:=WebMessageBodyStyle.Bare)>
        Function GetProfile(ByVal MobileUserID As String, _
                            ByVal WebServiceUserID As String, _
                            ByVal WebServicePasswordID As String, _
                            Optional ByVal IdRequiredApplication As String = Nothing, _
                            Optional ByVal CorporateNumber As String = Nothing, _
                            Optional ByVal CompanyId As String = Nothing, _
                            Optional ByVal FullAuthentication_ As String = Nothing) As UserProfile

        'Optional ByVal IdRequiredApplication As String = "4", _
        'Optional ByVal CorporateNumber As Integer = 1, _
        'Optional ByVal CompanyId As Integer = 0, _
        'Optional ByVal FullAuthentication As Boolean = False) As UserProfile

        '<ServiceKnownType(GetType(TagWatcher))>
        '<OperationContract()>
        Function SetBirthDate(ByVal MobileUserID As String, _
                            ByVal WebServiceUserID As String, _
                            ByVal WebServicePasswordID As String, _
                           ByVal BirthDate As String) As TagWatcher


        '<ServiceKnownType(GetType(TagWatcher))>
        '<OperationContract()>
        Function SetPhone(ByVal MobileUserID As String, _
                           ByVal WebServiceUserID As String, _
                           ByVal WebServicePasswordID As String, _
                           ByVal Phone As String) As TagWatcher

        '<ServiceKnownType(GetType(TagWatcher))>
        '<OperationContract()>
        Function SetEMail(ByVal MobileUserID As String, _
                          ByVal WebServiceUserID As String, _
                          ByVal WebServicePasswordID As String, _
                          ByVal EMail As String) As TagWatcher


        '<ServiceKnownType(GetType(TagWatcher))>
        '<OperationContract()>
        Function SetURLUserPicture(ByVal MobileUserID As String, _
                            ByVal WebServiceUserID As String, _
                            ByVal WebServicePasswordID As String, _
                            ByVal URLPicture As String) As TagWatcher


    End Interface


End Namespace