Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports Wma.Exceptions
Imports System.ServiceModel
Imports System.Web
Imports Wma.Exceptions.TagWatcher

Imports System
Imports System.Runtime.Serialization

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
'Imports WSReferences
Imports Wma.WebServices.WSReferences

Namespace Wma.WebServices

    <ServiceContract>
    <XmlSerializerFormat>
    Public Interface IWSReferences

#Region "Enums"

        <DataContract>
        Enum ManagementCompanies
            <Description("Undefined")>
            <EnumMember> UND = 0
            <Description("Grupo Reyes Kuri S. C.")>
            <EnumMember> RKU = 1
            <Description("Krom Logística")>
            <EnumMember> ATV = 2
            <Description("Despachos Aereos Integrados S. C.")>
            <EnumMember> DAI = 3
            <Description("Comercio Exterior del Golfo S.C")>
            <EnumMember> CEG = 6
            <Description("Comercio Exterior del Golfo S.C")>
            <EnumMember> TOL = 7
            <Description("Servicios Aduanales del Pacifíco S. C.")>
            <EnumMember> SAP = 8
            <Description("Servicios Aduanales del Pacifíco S. C.")>
            <EnumMember> LZR = 9
            <Description("Solium Forwarding Inc.")>
            <EnumMember> LAR = 113
        End Enum

        <DataContract>
        Enum SystemOwners
            <Description("Undefined")>
            <EnumMember> UND
            <Description("eZego")>
            <EnumMember> EZEGO
            <Description("eSolium")>
            <EnumMember> ESOLM
            <Description("SIR Reco")>
            <EnumMember> SIRRE
            <Description("SysExpert")>
            <EnumMember> SYSEX
            <Description("Slam")>
            <EnumMember> SLAM
        End Enum

#End Region

#Region "Properties"

        <DataMember>
        Property XMLSchemaResult As XMLSchemaObject

        <DataMember>
        Property XMLSchemaResult2 As XMLSchemaObjectPrueba

#End Region

#Region "Methods"


        '<WebGet(RequestFormat:=WebMessageFormat.Json, _
        '        ResponseFormat:=WebMessageFormat.Json, _
        '        UriTemplate:="Test/{YourName}")>

        '<WebInvoke(Method:="GET",
        '    UriTemplate:="/TestName/{YourName}",
        '    RequestFormat:=WebMessageFormat.Json,
        '    ResponseFormat:=WebMessageFormat.Json)>

        <OperationContract()>
        <XmlSerializerFormat>
        Function TestService(ByVal YourName As String) As String

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


        <OperationContract()>
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
                                        Optional ByVal ModalityType As WSReferences.ModalityTypes = WSReferences.ModalityTypes.UND, _
                                        Optional ByVal CustomSection As CustomSections = CustomSections.UND, _
                                        Optional ByVal OperationType As OperationTypes = OperationTypes.UND, _
 _
                                        Optional ByVal DisplayData As WSReferences.TypesDisplay = WSReferences.TypesDisplay.ShowAll) As XMLSchemaObject

        <OperationContract>
        <ServiceKnownType(GetType(TagWatcher))>
        <ServiceKnownType(GetType(Reference))>
        <ServiceKnownType(GetType(StatusItem))>
        <ServiceKnownType(GetType(WSReferences.CustomSections))>
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
                                        Optional ByVal ModalityType As WSReferences.ModalityTypes = WSReferences.ModalityTypes.UND, _
                                        Optional ByVal CustomSection As CustomSections = CustomSections.UND, _
                                        Optional ByVal OperationType As OperationTypes = OperationTypes.UND, _
 _
                                        Optional ByVal DisplayData As WSReferences.TypesDisplay = WSReferences.TypesDisplay.ShowAll) As XMLSchemaObject



        <OperationContract>
        <ServiceKnownType(GetType(TagWatcher))>
        <ServiceKnownType(GetType(Reference))>
        <ServiceKnownType(GetType(StatusItem))>
        <ServiceKnownType(GetType(WSReferences.CustomSections))>
        <ServiceKnownType(GetType(Object))>
        <ServiceKnownType(GetType(DataTable))>
        <XmlSerializerFormat>
        Function PruebaXmlSerializerFormat(ByVal MobileUserID As String,
                                        ByVal WebServiceUserID As String,
                                        ByVal WebServicePasswordID As String,
                                        Optional ByVal RFCOperationsClient As String = Nothing,
                                        Optional ByVal CustomerName_ As String = Nothing,
                                        Optional ByVal SupplierName_ As String = Nothing,
                                        Optional ByVal VesselName_ As String = Nothing,
                                        Optional ByVal ReferenceNumber_ As String = Nothing,
                                        Optional ByVal CustomsDocument As String = Nothing,
                                        Optional ByVal ModalityType As WSReferences.ModalityTypes = WSReferences.ModalityTypes.UND,
                                        Optional ByVal CustomSection As CustomSections = CustomSections.UND,
                                        Optional ByVal OperationType As OperationTypes = OperationTypes.UND,
                                        Optional ByVal DisplayData As WSReferences.TypesDisplay = WSReferences.TypesDisplay.ShowAll) As XMLSchemaObjectPrueba

#End Region

    End Interface

    <DataContract>
    <XmlSerializerFormat>
    Public Class XMLSchemaObjectPrueba

#Region "Attributes"

        Private _references As List(Of Reference)

        Private _TagWatcher As TagWatcher

        Private _schemaType As SchemaTypes

#End Region

#Region "Builders"

        Sub New()

            _references = New List(Of Reference)

            _TagWatcher = New TagWatcher

            _schemaType = New SchemaTypes

        End Sub

#End Region

#Region "Properties"

        <DataMember>
        Public Property References As List(Of Reference)
            Get
                Return _references
            End Get
            Set(ByVal value As List(Of Reference))
                _references = value
            End Set
        End Property

        <DataMember>
        Property TagWatcher As TagWatcher
            Get
                Return _TagWatcher
            End Get
            Set(value As TagWatcher)
                _TagWatcher = value
            End Set
        End Property

        <DataMember>
        Property SetXMLSchemaType As SchemaTypes
            Get
                Return _schemaType
            End Get
            Set(value As SchemaTypes)
                _schemaType = value
            End Set
        End Property

#End Region

#Region "Methods"

#End Region

    End Class

    <DataContract>
    <XmlSerializerFormat>
    Public Class Reference

#Region "Attributes"

        Private _statusList As List(Of StatusItem)

        Private _id As String

        Private _modalityType As WSReferences.ModalityTypes

        Private _operationType As WSReferences.OperationTypes

        Private _managementCompanyId As IWSReferences.ManagementCompanies

        Private _statusID As WSReferences.ReferenceStatusTypes

        Private _statusTag As String

        Private _dateLastStatus As DateTime

        Private _customSection As WSReferences.CustomSections

        Private _delivered As Integer

        Private _historicDate As DateTime

        Private _customsDeclarationDocumentID As String

        Private _customsDeclararionKey As String

        Private _year As Integer

        Private _originCountry As String

        Private _billOfLadings As List(Of BillOfLading)

        Private _commercialInvoices As List(Of CommercialInvoice)

        Private _containers As List(Of Container)

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

            _id = Nothing

            _modalityType = Nothing

            _operationType = Nothing

            _managementCompanyId = Nothing

            _statusID = Nothing

            _statusTag = Nothing

            _dateLastStatus = Nothing

            _customSection = Nothing

            _delivered = Nothing

            _historicDate = Nothing

            _customsDeclarationDocumentID = Nothing

            _customsDeclararionKey = Nothing

            _year = Nothing

            _originCountry = Nothing

            _packagesAndPallets = Nothing

            _estimatedTimeArrive = Nothing

            _refereceDate = Nothing

            _vesselName = Nothing

            _supplierName = Nothing

            _customerName = Nothing

            _booking = Nothing

        End Sub

#End Region

#Region "Properties"

        <DataMember>
        Public Property StatusCollection As List(Of StatusItem)
            Get
                Return _statusList
            End Get
            Set(ByVal value As List(Of StatusItem))
                _statusList = value
            End Set
        End Property

        <DataMember>
        Public Property BillOfLadings As List(Of BillOfLading)
            Get
                Return _billOfLadings
            End Get
            Set(ByVal value As List(Of BillOfLading))
                _billOfLadings = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property Booking As String
            Get
                Return _booking
            End Get
            Set(ByVal value As String)
                _booking = value
            End Set
        End Property

        <DataMember>
        Public Property CommercialInvoices As List(Of CommercialInvoice)
            Get
                Return _commercialInvoices
            End Get
            Set(ByVal value As List(Of CommercialInvoice))
                _commercialInvoices = value
            End Set
        End Property

        <DataMember>
        Public Property Containers As List(Of Container)
            Get
                Return _containers
            End Get
            Set(ByVal value As List(Of Container))
                _containers = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property PackagesAndPallets As String
            Get
                Return _packagesAndPallets
            End Get
            Set(ByVal value As String)
                _packagesAndPallets = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property EstimatedTimeArrive As String
            Get
                Return _estimatedTimeArrive
            End Get
            Set(ByVal value As String)
                _estimatedTimeArrive = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property RefereceDate As String
            Get
                Return _refereceDate
            End Get
            Set(ByVal value As String)
                _refereceDate = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property VesselName As String
            Get
                Return _vesselName
            End Get
            Set(ByVal value As String)
                _vesselName = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property CustomerName As String
            Get
                Return _customerName
            End Get
            Set(ByVal value As String)
                _customerName = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property ID As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property ModalityType As String
            Get
                Return _modalityType
            End Get
            Set(ByVal value As String)
                _modalityType = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property OperationType As String
            Get
                Return _operationType
            End Get
            Set(ByVal value As String)
                _operationType = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property ManagementCompanyId As String
            Get
                Return _managementCompanyId
            End Get
            Set(ByVal value As String)
                _managementCompanyId = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property StatusID As String
            Get
                Return _statusID
            End Get
            Set(ByVal value As String)
                _statusID = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property StatusTag As String
            Get
                Return _statusTag
            End Get
            Set(ByVal value As String)
                _statusTag = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property DateLastStatus As DateTime
            Get
                Return _dateLastStatus
            End Get
            Set(ByVal value As DateTime)
                _dateLastStatus = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property CustomSection As String
            Get
                Return _customSection
            End Get
            Set(ByVal value As String)
                _customSection = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property Delivered As String
            Get
                Return _delivered
            End Get
            Set(ByVal value As String)
                _delivered = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property HistoricDate As DateTime
            Get
                Return _historicDate
            End Get
            Set(ByVal value As DateTime)
                _historicDate = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property CustomsDeclarationDocumentID As String
            Get
                Return _customsDeclarationDocumentID
            End Get
            Set(ByVal value As String)
                _customsDeclarationDocumentID = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property CustomsDeclararionKey As String
            Get
                Return _customsDeclararionKey
            End Get
            Set(ByVal value As String)
                _customsDeclararionKey = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property OperationYear As String
            Get
                Return _year
            End Get
            Set(ByVal value As String)
                _year = value
            End Set
        End Property

        <DataMember>
        <XmlAttribute>
        Public Property OriginCountry As String
            Get
                Return _originCountry
            End Get
            Set(ByVal value As String)
                _originCountry = value
            End Set
        End Property

        Public Property SupplierName As String
            Get
                Return _supplierName
            End Get
            Set(ByVal value As String)
                _supplierName = value
            End Set
        End Property

#End Region

#Region "Methods"

#End Region

    End Class

    <DataContract()>
    <XmlSerializerFormat>
    Public Class StatusItem

#Region "Attributes"

        Private _status As ReferenceStatusTypes

        Private _done As Boolean

        Private _lastDate As DateTime

        Private _orderNumber As Int32

#End Region

#Region "Builders"

        Sub New()

            _status = New ReferenceStatusTypes

            _status = ReferenceStatusTypes.UND

            _done = False

            _lastDate = Now

            _orderNumber = 0

        End Sub

#End Region

#Region "Properties"

        <DataMember, XmlAttribute>
        Public Property OrderNumber As Int32

            Get

                Return _orderNumber

            End Get

            Set(value As Int32)

                _orderNumber = value

            End Set

        End Property


        <DataMember, XmlAttribute>
        Public Property StatusID As Int32

            Get

                Return _status

            End Get

            Set(value As Int32)

                _status = value

            End Set

        End Property

        <DataMember, XmlAttribute>
        Public Property StatusType As ReferenceStatusTypes

            Get

                Return _status

            End Get

            Set(value As ReferenceStatusTypes)

                _status = value

            End Set

        End Property

        <DataMember, XmlAttribute>
        Public Property IsDone As Boolean

            Get

                Return _done

            End Get

            Set(value As Boolean)

                _done = value

            End Set

        End Property

        <DataMember, XmlAttribute>
        Public Property LastDateTime As DateTime
            Get

                Return _lastDate

            End Get

            Set(value As DateTime)

                _lastDate = value

            End Set

        End Property

#End Region

#Region "Methods"

#End Region

    End Class

End Namespace