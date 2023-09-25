Imports System.ServiceModel
Imports System.ComponentModel
Imports System.Xml.Serialization

Imports WSExtranetReferences
Imports Wma.Exceptions
Imports System.ServiceModel.Web

' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
<ServiceContract()>
Public Interface IWSExtranetReferences

#Region "Methods"

    <OperationContract()>
    Function TestWebService(Optional ByVal nombre_ As String = "") As String

    '<OperationContract()>
    <OperationContract()>
    <WebInvoke(Method:="GET",
           UriTemplate:="/GetBasic/{MobileUserID}" & _
           "/{WebServiceUserID}/{WebServicePasswordID}/{RFCOperationsClient}/{CustomerName_}/{SupplierName_}/{VesselName_}/{ModalityType}/{CustomSection}/{OperationType}/{DisplayData}",
           RequestFormat:=WebMessageFormat.Json,
           ResponseFormat:=WebMessageFormat.Json,
           BodyStyle:=WebMessageBodyStyle.Bare)>
<ServiceKnownType(GetType(Reference))>
<ServiceKnownType(GetType(TagWatcher))>
<ServiceKnownType(GetType(CustomSections))>
    Function GetBasicInformationSchemaInKromBase(ByVal mobileUserID_ As String _
                                                , ByVal webServiceUserID_ As String _
                                                , ByVal webServicePasswordID_ As String _
                                                , Optional ByVal rfcCOperationsClient_ As String = Nothing _
                                                , Optional ByVal customerName_ As String = Nothing _
                                                , Optional ByVal supplierName_ As String = Nothing _
                                                , Optional ByVal vesselName_ As String = Nothing _
                                                , Optional ByVal modalityType_ As WSExtranetReferences.ModalityTypes = WSExtranetReferences.ModalityTypes.UND _
                                                , Optional ByVal customSection_ As WSExtranetReferences.CustomSections = WSExtranetReferences.CustomSections.UND _
                                                , Optional ByVal operationType_ As WSExtranetReferences.OperationTypes = WSExtranetReferences.OperationTypes.UND _
                                                , Optional ByVal displayData_ As WSExtranetReferences.TypesDisplay = WSExtranetReferences.TypesDisplay.ShowAll) As XMLSchemaObject





    <OperationContract()>
              <WebInvoke(Method:="GET",
                         UriTemplate:="/GetAdvanced?MobileUserID={MobileUserID}" & _
                                      "&WebServiceUserID={WebServiceUserID}" & _
                                      "&WebServicePasswordID={WebServicePasswordID}" & _
                                      "&RFCOperationsClient={RFCOperationsClient}" & _
                                      "&CustomerName_={CustomerName_}" & _
                                      "&SupplierName_={SupplierName_}" & _
                                      "&VesselName_={VesselName_}" & _
                                      "&ReferenceNumber_={ReferenceNumber_}" & _
                                      "&CustomsDocument={CustomsDocument}" & _
                                      "&ModalityType_={ModalityType_}" & _
                                      "&CustomSection_={CustomSection_}" & _
                                      "&OperationType_={OperationType_}" & _
                                      "&DisplayData_={DisplayData_}",
                         RequestFormat:=WebMessageFormat.Json,
                         ResponseFormat:=WebMessageFormat.Json,
                         BodyStyle:=WebMessageBodyStyle.Bare)>
              <ServiceKnownType(GetType(Reference))>
              <ServiceKnownType(GetType(StatusItem))>
              <ServiceKnownType(GetType(TagWatcher))>
              <ServiceKnownType(GetType(CustomSections))>
                        <ServiceKnownType(GetType(XMLSchemaObject))>
    Function GetAdvancedInformationSchemaInKromBase(ByVal MobileUserID As String, _
                                        ByVal WebServiceUserID As String, _
                                        ByVal WebServicePasswordID As String, _
                                        Optional ByVal RFCOperationsClient As String = Nothing, _
 _
                                        Optional ByVal CustomerName_ As String = Nothing, _
                                        Optional ByVal SupplierName_ As String = Nothing, _
 _
                                        Optional ByVal VesselName_ As String = Nothing, _
 _
                                        Optional ByVal ReferenceNumber_ As String = Nothing, _
                                        Optional ByVal CustomsDocument As String = Nothing, _
                                       Optional ByVal modalityType_ As ModalityTypes = WSExtranetReferences.ModalityTypes.UND, _
                                        Optional ByVal customSection_ As WSExtranetReferences.CustomSections = WSExtranetReferences.CustomSections.UND, _
                                        Optional ByVal operationType_ As WSExtranetReferences.OperationTypes = WSExtranetReferences.OperationTypes.UND, _
                                        Optional ByVal displayData_ As WSExtranetReferences.TypesDisplay = WSExtranetReferences.TypesDisplay.ShowAll) As XMLSchemaObject

 '                                       Optional ByVal ModalityType_ As String = Nothing, _
    '                                       Optional ByVal CustomSection_ As String = Nothing, _
    '                                       Optional ByVal OperationType_ As String = Nothing, _
    '_
    '                                       Optional ByVal DisplayData_ As String = Nothing) As XMLSchemaObject

#End Region

End Interface

' Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
' Puede agregar archivos XSD al proyecto. Después de compilar el proyecto, puede usar directamente los tipos de datos definidos aquí, con el espacio de nombres "WSExtranetReferences64.ContractType".

<DataContract()>
Public Class CompositeType

    '<DataMember()>
    'Public Property StringValue() As String

End Class