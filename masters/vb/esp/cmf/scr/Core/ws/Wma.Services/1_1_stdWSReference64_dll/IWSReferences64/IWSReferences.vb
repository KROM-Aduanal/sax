Imports System.ServiceModel
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.WebServices.WSReferences

Namespace Wma.WebServices
    ''<XmlSerializerFormat>
    ''<ServiceContract(namespace:="http://localhost/wcf3/")>
    <ServiceContract()>
    Public Interface IWSReferences

        <DataContract()>
        Enum ManagementCompanies
            <EnumMember>
            <Description("Undefined")> UND = 0
            <EnumMember>
            <Description("Grupo Reyes Kuri S. C.")> RKU = 1
            <EnumMember>
            <Description("Krom Logística")> ATV = 2
            <EnumMember>
            <Description("Despachos Aereos Integrados S. C.")> DAI = 3
            <EnumMember>
            <Description("Servicios Aduanales del Pacifíco S. C.")> SAP = 8
            <EnumMember>
            <Description("ervicios Aduanales del Pacifíco S. C.")> LZR = 9
            <EnumMember>
            <Description("Comercio Exterior del Golfo S.C")> CEG = 6
            <EnumMember>
            <Description("Comercio Exterior del Golfo S.C")> TOL = 7
            <EnumMember>
            <Description("Solium Forwarding Inc.")> LAR = 113
        End Enum

        <DataContract()>
        Enum SystemOwners
            <EnumMember>
            <Description("Undefined")> UND = 0
            <EnumMember>
            <Description("eZego")> EZEGO = 1
            <EnumMember>
            <Description("eSolium")> ESOLM = 2
            <EnumMember>
            <Description("SIR Reco")> SIRRE = 3
            <EnumMember>
            <Description("SysExpert")> SYSEX = 4
            <EnumMember>
            <Description("Slam")> SLAM = 5
        End Enum



#Region "Properties"

        <DataMember>
        Property XMLSchemaResult As XMLSchemaObject

        '<WebGet(RequestFormat:=WebMessageFormat.Json, _
        '        ResponseFormat:=WebMessageFormat.Json, _
        '        UriTemplate:="Test/{YourName}")>

        '<WebInvoke(Method:="GET",
        '    UriTemplate:="/TestName/{YourName}",
        '    RequestFormat:=WebMessageFormat.Json,
        '    ResponseFormat:=WebMessageFormat.Json)>

        <OperationContract()>
        <WebInvoke(Method:="GET",
                   UriTemplate:="/TestService?YourName={YourName}&LastName={LastName}",
                   RequestFormat:=WebMessageFormat.Json,
                   ResponseFormat:=WebMessageFormat.Json,
                   BodyStyle:=WebMessageBodyStyle.Wrapped)>
        Function TestService(Optional ByVal YourName As String = "rafa", Optional ByVal LastName As String = "Gon") As String

        Function SetReferenceStatusKromBase(ByVal MobileUserID As String, _
                                        ByVal WebServiceUserID As String, _
                                        ByVal WebServicePasswordID As String, _
                                        ByVal ReferenceID As String, _
                                        ByVal SystemOwner As SystemOwners, _
                                        ByVal SetThisStatus As ReferenceStatusTypes, _
                                        ByVal KromCompanyID As Integer) As TagWatcher


        Function GetReferenceStatusListKromBase(ByVal MobileUserID As String, _
                                    ByVal WebServiceUserID As String, _
                                    ByVal WebServicePasswordID As String, _
                                    ByVal ReferenceID As String, _
                                    ByVal KromCompanyID As Integer) As TagWatcher


        Function SetNewReferenceInKromBase(ByVal MobileUserID As String, _
                                        ByVal WebServiceUserID As String, _
                                        ByVal WebServicePasswordID As String, _
                                        ByVal ReferenceID As String, _
                                        ByVal SystemOwner As SystemOwners, _
                                        ByVal RFCOperationsClient As String, _
                                        ByVal ClientCompanyName_ As String, _
                                        ByVal ModalityType As WSReferences.ModalityTypes, _
                                        ByVal KromCompanyID As Integer, _
                                        ByVal CustomSection As CustomSections, _
                                        ByVal OperationType As OperationTypes, _
                                        Optional ByVal IDOperationsClient As Integer = 0, _
                                        Optional ByVal IDAdministrativeClient As Integer = 0) As TagWatcher



        '<XmlSerializerFormat>
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
        Function GetBasicInformationSchemaInKromBase(ByVal MobileUserID As String, _
                                        ByVal WebServiceUserID As String, _
                                        ByVal WebServicePasswordID As String, _
                                        Optional ByVal RFCOperationsClient As String = Nothing, _
 _
                                        Optional ByVal CustomerName_ As String = Nothing, _
                                        Optional ByVal SupplierName_ As String = Nothing, _
 _
                                        Optional ByVal VesselName_ As String = Nothing, _
                                        Optional ByVal ModalityType As String = Nothing, _
                                        Optional ByVal CustomSection As String = Nothing, _
                                        Optional ByVal OperationType As String = Nothing, _
 _
                                        Optional ByVal DisplayData As String = Nothing) As XMLSchemaObject

        'Optional ByVal ModalityType As WSReferences.ModalityTypes = WSReferences.ModalityTypes.UND, _
        '                                        Optional ByVal CustomSection As CustomSections = CustomSections.UND, _
        '                                        Optional ByVal OperationType As OperationTypes = OperationTypes.UND, _
        ' _
        '                                        Optional ByVal DisplayData As WSReferences.TypesDisplay = WSReferences.TypesDisplay.ShowAll) As XMLSchemaObject


        '<XmlSerializerFormat>
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
 _
                                        Optional ByVal ModalityType_ As String = Nothing, _
                                        Optional ByVal CustomSection_ As String = Nothing, _
                                        Optional ByVal OperationType_ As String = Nothing, _
 _
                                        Optional ByVal DisplayData_ As String = Nothing) As XMLSchemaObject

        '                                       Optional ByVal ModalityType As WSReferences.ModalityTypes = WSReferences.ModalityTypes.UND, _
        '                                       Optional ByVal CustomSection As CustomSections = CustomSections.UND, _
        '                                       Optional ByVal OperationType As OperationTypes = OperationTypes.UND, _
        '_
        '                                       Optional ByVal DisplayData As WSReferences.TypesDisplay = WSReferences.TypesDisplay.ShowAll) As XMLSchemaObject


#End Region


#Region "Methods"

#End Region


    End Interface


    '<XmlSerializerFormat>
    <DataContract()>
    Public Class Reference

#Region "Attributes"

        Private _id As String

        Private _modalityType As ModalityTypes

        Private _operationType As WSReferences.OperationTypes

        Private _managementCompanyId As IWSReferences.ManagementCompanies

        Private _statusID As ReferenceStatusTypes

        Private _statusTag As String

        'Private _dateLastStatus As DateTime
        Private _dateLastStatus As String

        Private _customSection As WSReferences.CustomSections

        Private _delivered As Int32

        'Private _historicDate As DateTime
        Private _historicDate As String

        Private _customsDeclarationDocumentID As String

        Private _customsDeclararionKey As String

        Private _year As Int32

        Private _originCountry As String

        Private _billOfLadings As List(Of BillOfLading)

        Private _commercialInvoices As List(Of CommercialInvoice)

        Private _containers As List(Of Container)

        Private _statusList As List(Of StatusItem)

        Private _packagesAndPallets As String

        Private _estimatedTimeArrive As String

        Private _refereceDate As String

        Private _vesselName As String

        Private _supplierName As String

        Private _customerName As String

        Private _booking As String

#End Region

#Region "Builders"

        Sub New()

            _packagesAndPallets = Nothing

            _estimatedTimeArrive = Nothing

            _refereceDate = Nothing

            _vesselName = Nothing

            _supplierName = Nothing

            _customerName = Nothing

            _id = Nothing

            _modalityType = ModalityTypes.UND

            _operationType = WSReferences.OperationTypes.UND

            _managementCompanyId = IWSReferences.ManagementCompanies.UND

            _statusID = ReferenceStatusTypes.UND

            _statusTag = Nothing

            '_dateLastStatus = Now
            _dateLastStatus = Nothing

            _customSection = WSReferences.CustomSections.UND

            _delivered = 0

            _historicDate = Nothing

            _customsDeclarationDocumentID = Nothing

            _customsDeclararionKey = Nothing

            _year = 0

            _originCountry = Nothing

            _booking = Nothing

        End Sub

#End Region

#Region "Properties"

        '<XmlAttribute>
        <DataMember>
        Property ID As String

            Get

                Return _id

            End Get

            Set(value As String)

                _id = value

            End Set

        End Property

        <DataMember>
        Property StatusCollection As List(Of StatusItem)

            Get

                Return _statusList

            End Get

            Set(value As List(Of StatusItem))

                _statusList = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Property PackagesAndPallets As String

            Get

                Return _packagesAndPallets

            End Get

            Set(value As String)

                _packagesAndPallets = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Property EstimatedTimeArrive As String

            Get

                Return _estimatedTimeArrive

            End Get

            Set(value As String)

                _estimatedTimeArrive = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Property RefereceDate As String

            Get

                Return _refereceDate

            End Get

            Set(value As String)

                _refereceDate = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Property VesselName As String

            Get

                Return _vesselName

            End Get

            Set(value As String)

                _vesselName = value

            End Set

        End Property

        Property SupplierName As String

            Get

                Return _supplierName

            End Get

            Set(value As String)

                _supplierName = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Property CustomerName As String

            Get

                Return _customerName

            End Get

            Set(value As String)

                _customerName = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Property ModalityType As Integer

            Get

                Return _modalityType

            End Get

            Set(value As Integer)

                _modalityType = value

            End Set

        End Property


        '<XmlAttribute>
        <DataMember>
        Property OperationType As Integer

            Get

                Return _operationType

            End Get

            Set(value As Integer)

                _operationType = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Property ManagementCompanyId As Integer

            Get

                Return _managementCompanyId

            End Get

            Set(value As Integer)

                _managementCompanyId = value

            End Set

        End Property


        '<XmlAttribute>
        <DataMember>
        Property StatusID As Integer

            Get

                Return _statusID

            End Get

            Set(value As Integer)

                _statusID = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Property StatusTag As String

            Get

                Return _statusTag

            End Get

            Set(value As String)

                _statusTag = value

            End Set

        End Property


        '<XmlAttribute>
        <DataMember>
        Property DateLastStatus As String
            Get

                Return _dateLastStatus

            End Get

            Set(value As String)

                _dateLastStatus = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Property CustomSection As Integer

            Get

                Return _customSection

            End Get

            Set(value As Integer)

                _customSection = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Property Delivered As String

            Get

                Return _delivered

            End Get

            Set(value As String)

                _delivered = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Property HistoricDate As String

            Get

                Return _historicDate

            End Get

            Set(value As String)

                _historicDate = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Property CustomsDeclarationDocumentID As String

            Get

                Return _customsDeclarationDocumentID

            End Get

            Set(value As String)

                _customsDeclarationDocumentID = value
            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Property CustomsDeclararionKey As String

            Get

                Return _customsDeclararionKey

            End Get

            Set(value As String)

                _customsDeclararionKey = value

            End Set
        End Property

        <DataMember>
        Property OperationYear As String

            Get

                Return _year

            End Get

            Set(value As String)

                _year = value

            End Set
        End Property

        '<XmlAttribute>
        <DataMember>
        Property OriginCountry As String
            Get

                Return _originCountry

            End Get

            Set(value As String)

                _originCountry = value

            End Set

        End Property

        <DataMember>
        Property Booking As String

            Get

                Return _booking

            End Get

            Set(value As String)

                _booking = value

            End Set

        End Property

        <DataMember>
        Property BillOfLadings As List(Of BillOfLading)
            Get

                Return _billOfLadings

            End Get

            Set(value As List(Of BillOfLading))

                _billOfLadings = value

            End Set

        End Property

        <DataMember>
        Property CommercialInvoices As List(Of CommercialInvoice)

            Get

                Return _commercialInvoices

            End Get

            Set(value As List(Of CommercialInvoice))

                _commercialInvoices = value

            End Set

        End Property

        <DataMember>
        Property Containers As List(Of Container)
            Get

                Return _containers

            End Get

            Set(value As List(Of Container))

                _containers = value

            End Set

        End Property

#End Region

#Region "Methods"

#End Region

    End Class

    '<XmlSerializerFormat>
    <DataContract()>
    Public Class StatusItem

#Region "Attributes"

        Private _status As ReferenceStatusTypes

        Private _done As Boolean

        'Private _lastDate As DateTime
        Private _lastDate As String

        Private _orderNumber As Int32

#End Region

#Region "Builders"

        Sub New()

            _status = New ReferenceStatusTypes

            _status = ReferenceStatusTypes.UND

            _done = False

            '_lastDate = Now
            _lastDate = Nothing

            _orderNumber = 0

        End Sub

#End Region

#Region "Properties"

        '<XmlAttribute>
        <DataMember>
        Public Property OrderNumber As Int32

            Get

                Return _orderNumber

            End Get

            Set(value As Int32)

                _orderNumber = value

            End Set

        End Property


        '<XmlAttribute>
        <DataMember>
        Public Property StatusID As Int32

            Get

                Return _status

            End Get

            Set(value As Int32)

                _status = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Public Property StatusType As ReferenceStatusTypes

            Get

                Return _status

            End Get

            Set(value As ReferenceStatusTypes)

                _status = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Public Property IsDone As Boolean

            Get

                Return _done

            End Get

            Set(value As Boolean)

                _done = value

            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
        Public Property LastDateTime As String
            Get

                Return _lastDate

            End Get

            Set(value As String)

                _lastDate = value

            End Set

        End Property

#End Region

#Region "Methods"

#End Region

    End Class

End Namespace