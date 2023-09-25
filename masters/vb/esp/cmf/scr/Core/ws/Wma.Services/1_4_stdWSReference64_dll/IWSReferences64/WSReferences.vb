Imports gsol
Imports gsol.BaseDatos
Imports gsol.BaseDatos.Operaciones
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.Text
Imports System.Xml.Serialization
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher

Imports System.Reflection
Imports gsol.basededatos
'Imports WSReferences
Imports Wma.WebServices.WSReferences

'Imports Wma.WebServices

Namespace Wma.WebServices

    <DataContract()>
    <XmlSerializerFormat>
    Public Class WSReferences
        Implements IWSReferences

#Region "Enums"

        <DataContract>
        Public Enum AccessTypes
            <EnumMember> Access = 1
            <EnumMember> Deny = 2
        End Enum

        <DataContract>
        Public Enum ApplicationTypes
            <EnumMember> Undefined = 0
            <EnumMember> Desktop = 4
            <EnumMember> Mobile = 11
            <EnumMember> Web = 12
        End Enum

        <DataContract>
        Public Enum ContainerSizes
            <EnumMember> Undefined = 0
            <EnumMember> C20Inches = 20
            <EnumMember> C40Inches = 40
        End Enum

        <DataContract>
        Public Enum ContainerTypes
            <EnumMember> Undefined
            <EnumMember> OpenTop
            <EnumMember> Refrigerated
        End Enum

        <DataContract>
        Public Enum CustomSections
            <Description("Undefined")>
            <EnumMember> UND = 0
            <Description("Manzanillo ( Marítimo )")>
            <EnumMember> SAP = 160
            <Description("México ( Terrestre )")>
            <EnumMember> PAN = 200
            <Description("Nuevo Laredo ( Terrestre )")>
            <EnumMember> SFI = 240
            <Description("Tuxpan Veracruz( Marítimo )")>
            <EnumMember> TUX = 420
            <Description("Veracruz ( Marítimo )")>
            <EnumMember> TUX1 = 421
            <Description("Tuxpan ( Terrestre )")>
            <EnumMember> RKU = 430
            <Description("México ( Áereo )")>
            <EnumMember> DAI = 470
            <Description("Lázaro Cárdenas ( Marítimo )")>
            <EnumMember> ALC = 510
            <Description("Toluca ( Áereo )")>
            <EnumMember> TOL = 650
            <Description("Toluca ( Áereo ) Seccion 1")>
            <EnumMember> TOL1 = 651
            <Description("Colombia N.L. ( Terrestres )")>
            <EnumMember> SFC = 800
            <Description("Altamira ( Marítimo )")>
            <EnumMember> CEG = 810
        End Enum

        <DataContract>
        Public Enum GuidTypes
            <Description("Undefined")>
            <EnumMember> UND
            <Description("Master")>
            <EnumMember> Master
            <Description("House")>
            <EnumMember> House
        End Enum

        <DataContract>
        Public Enum ModalityTypes
            <Description("Undefined")>
            <EnumMember> UND
            <Description("Sea")>
            <EnumMember> SEA
            <Description("Air")>
            <EnumMember> AIR
            <Description("Road")>
            <EnumMember> ROA
            <Description("Rail")>
            <EnumMember> RAI
            <Description("Logística")>
            <EnumMember> MDL
        End Enum

        <DataContract>
        Public Enum OperationTypes
            <Description("Undefined")>
            <EnumMember> UND
            <Description("Importación")>
            <EnumMember> IMP
            <Description("Exportación")>
            <EnumMember> EXP
        End Enum

        <DataContract>
        Public Enum PatentNumbers
            <Description("Undefined")>
            <EnumMember> UND = 0
            <Description("Rolando Reyes Kuri ")>
            <EnumMember> RRK = 3210
            <Description("Luis de la Cruz Reyes")>
            <EnumMember> LDC = 3921
            <Description("Sergio Álvarez Ramírez")>
            <EnumMember> SAR = 3931
            <Description("Jesús Gomez Reyes")>
            <EnumMember> JGR = 3945
            <Description("Marco Antonio Barquín de la Calle")>
            <EnumMember> MBC = 3962
        End Enum

        <DataContract>
        Public Enum ReferenceStatusTypes
            <Description("Sin definir")>
            <EnumMember> UND = 0
            <Description("Anticipo")>
            <EnumMember> ANT = 5
            <Description("Alta de la referencia")>
            <EnumMember> ALR = 27
            <Description("Emisión de cuenta de gastos")>
            <EnumMember> FAC = 38
            <Description("PENDIENTE DE FACTURAR NUEVAMENTE")>
            <EnumMember> PFAC = 41
            <Description("Despacho aduanal")>
            <EnumMember> DSP = 49
            <Description("Arribo de buque")>
            <EnumMember> ABU = 50
            <Description("Arribo de la mercancía")>
            <EnumMember> AME = 51
            <Description("Arribo de tren")>
            <EnumMember> ATR = 52
            <Description("Arribo dentro de México")>
            <EnumMember> AMX = 53
            <Description("Aterrizaje")>
            <EnumMember> ATER = 54
            <Description("Booking")>
            <EnumMember> BOO = 55
            <Description("Canje de documentos")>
            <EnumMember> CAD = 56
            <Description("Carga de mercancías")>
            <EnumMember> CAM = 57
            <Description("Carga en tránsito")>
            <EnumMember> CAT = 58
            <Description("Cruce de unidad")>
            <EnumMember> CRU = 59
            <Description("Descarga/entrega en planta")>
            <EnumMember> ENP = 60
            <Description("Despegue")>
            <EnumMember> DES = 61
            <Description("Entregado")>
            <EnumMember> ENT = 62
            <Description("Ingreso a EUA")>
            <EnumMember> IEU = 63
            <Description("Inicio de ruta")>
            <EnumMember> INR = 64
            <Description("Llegada a destino")>
            <EnumMember> LLD = 65
            <Description("Pago de cuenta de gastos")>
            <EnumMember> LFAC = 66
            <Description("Pago de pedimento")>
            <EnumMember> PAG = 67
            <Description("Previo")>
            <EnumMember> PRE = 68
            <Description("Recepción de documentos")>
            <EnumMember> DOC = 69
            <Description("Recepción en bodega")>
            <EnumMember> RBO = 70
            <Description("Recolección de guía")>
            <EnumMember> RGU = 71
            <Description("Recolección/Posición en planta")>
            <EnumMember> PPL = 72
            <Description("Reconocimiento aduanero")>
            <EnumMember> RAD = 73
            <Description("Revalidación")>
            <EnumMember> REV = 74
            <Description("Selección automátizada rojo")>
            <EnumMember> SAR = 75
            <Description("Transbordo")>
            <EnumMember> TBD = 76
            <Description("Zarpe de buque")>
            <EnumMember> ZAR = 77
            <Description("Fecha de Entrada")>
            <EnumMember> ATA = 94
            <Description("Solicitud de Servicio")>
            <EnumMember> SSE = 95
        End Enum

        <DataContract>
        Public Enum SchemaTypes
            <EnumMember> OnlyHeaders
            <EnumMember> ReferenceType
            <EnumMember> Advanced1
        End Enum

        <DataContract>
        Public Enum TypesDisplay
            <EnumMember> ShowAll
            <EnumMember> NotDelivered
            <EnumMember> Delivered
        End Enum

#End Region

#Region "Attributes"

        Private _xmlSchemaObject As XMLSchemaObject

        Private _xmlSchemaObject2 As XMLSchemaObjectPrueba

        Private _iOperations As IOperacionesCatalogo

        Private _system As Organismo

        Public Shared _sesion As ISesion
        'Public Shared _sesion As ISesionWcf

        Private _sessionStatus As Boolean

        Private _dataAccessRulesList As List(Of DataAccessRules)

        Private _scriptOfRules As String

#End Region

#Region "Builders"

        Sub New()

            MyBase.New()

            _system = New Organismo

            _xmlSchemaObject = New XMLSchemaObject

            _xmlSchemaObject2 = New XMLSchemaObjectPrueba

            _sesion = New SesionWcf
            '_sesion = New SesionWcf

            _iOperations = New OperacionesCatalogo

            _dataAccessRulesList = New List(Of DataAccessRules)

            _scriptOfRules = Nothing

        End Sub

#End Region

#Region "Properties"

        Public Property XMLSchemaResult As XMLSchemaObject Implements IWSReferences.XMLSchemaResult
            Get
                Return _xmlSchemaObject
            End Get
            Set(value As XMLSchemaObject)
                _xmlSchemaObject = value
            End Set
        End Property

        Public Property XMLSchemaResult2 As XMLSchemaObjectPrueba Implements IWSReferences.XMLSchemaResult2
            Get
                Return _xmlSchemaObject2
            End Get
            Set(value As XMLSchemaObjectPrueba)
                _xmlSchemaObject2 = value
            End Set
        End Property

#End Region

#Region "Methods"

        ' ******************
        'MAIN FUNCTIONS
        ' ******************
        Public Function PruebaXmlSerializerFormat(ByVal MobileUserID As String,
                                                               ByVal WebServiceUserID As String,
                                                               ByVal WebServicePasswordID As String,
                                                               Optional ByVal RFCOperationsClient As String = Nothing,
                                                               Optional ByVal CustomerName_ As String = Nothing,
                                                               Optional ByVal SupplierName_ As String = Nothing,
                                                               Optional ByVal VesselName_ As String = Nothing,
                                                               Optional ByVal ReferenceNumber_ As String = Nothing,
                                                               Optional ByVal CustomsDocument As String = Nothing,
                                                               Optional ByVal ModalityType As WSReferences.ModalityTypes = 0,
                                                               Optional ByVal CustomSection As WSReferences.CustomSections = 0,
                                                               Optional ByVal OperationType As WSReferences.OperationTypes = 0,
                                                               Optional ByVal DisplayData As WSReferences.TypesDisplay = 0) As XMLSchemaObjectPrueba _
                                                                Implements IWSReferences.PruebaXmlSerializerFormat

            Dim result_ As TagWatcher = New TagWatcher

            Dim xmlSchemaObject_ As New XMLSchemaObjectPrueba

            result_ = LogIn(MobileUserID, WebServiceUserID, WebServicePasswordID, 4, 1, False)

            If (result_.Errors <> TagWatcher.ErrorTypes.WS000) Then

                xmlSchemaObject_.TagWatcher = result_

            Else

                Dim ContainterID As String = Nothing

                Dim CommercialInvoice As String = Nothing

                xmlSchemaObject_ = ReturnXMLSchema2(WSReferences.SchemaTypes.Advanced1, MobileUserID, RFCOperationsClient, CustomerName_, SupplierName_, VesselName_, ReferenceNumber_, CustomsDocument, ContainterID, CommercialInvoice, ModalityType, CustomSection, OperationType, DisplayData)

            End If

            Return xmlSchemaObject_

        End Function

        Public Function GetAdvancedInformationSchemaInKromBase(ByVal MobileUserID As String,
                                                               ByVal WebServiceUserID As String,
                                                               ByVal WebServicePasswordID As String,
                                                               Optional ByVal RFCOperationsClient As String = Nothing,
                                                               Optional ByVal CustomerName_ As String = Nothing,
                                                               Optional ByVal SupplierName_ As String = Nothing,
                                                               Optional ByVal VesselName_ As String = Nothing,
                                                               Optional ByVal ReferenceNumber_ As String = Nothing,
                                                               Optional ByVal CustomsDocument As String = Nothing,
                                                               Optional ByVal ModalityType As WSReferences.ModalityTypes = 0,
                                                               Optional ByVal CustomSection As WSReferences.CustomSections = 0,
                                                               Optional ByVal OperationType As WSReferences.OperationTypes = 0,
                                                               Optional ByVal DisplayData As WSReferences.TypesDisplay = 0) As XMLSchemaObject _
                                                                Implements IWSReferences.GetAdvancedInformationSchemaInKromBase

            Dim result_ As TagWatcher = New TagWatcher

            Dim xmlSchemaObject_ As XMLSchemaObject = New XMLSchemaObject

            result_ = LogIn(MobileUserID, WebServiceUserID, WebServicePasswordID, 4, 1, False)

            If (result_.Errors <> TagWatcher.ErrorTypes.WS000) Then

                xmlSchemaObject_.TagWatcher = result_

            Else

                Dim ContainterID As String = Nothing

                Dim CommercialInvoice As String = Nothing

                xmlSchemaObject_ = ReturnXMLSchema(WSReferences.SchemaTypes.Advanced1, MobileUserID, RFCOperationsClient, CustomerName_, SupplierName_, VesselName_, ReferenceNumber_, CustomsDocument, ContainterID, CommercialInvoice, ModalityType, CustomSection, OperationType, DisplayData)

            End If

            Return xmlSchemaObject_

        End Function

        Public Function GetBasicInformationSchemaInKromBase(ByVal MobileUserID As String, _
                                                            ByVal WebServiceUserID As String, _
                                                            ByVal WebServicePasswordID As String, _
 _
                                                                      Optional ByVal RFCOperationsClient As String = Nothing, _
 _
                                                                      Optional ByVal CustomerName_ As String = Nothing, _
                                                                      Optional ByVal SupplierName_ As String = Nothing, _
 _
                                                                      Optional ByVal VesselName_ As String = Nothing, _
                                                                      Optional ByVal ModalityType As WSReferences.ModalityTypes = WSReferences.ModalityTypes.UND, _
                                                                      Optional ByVal CustomSection As WSReferences.CustomSections = WSReferences.CustomSections.UND, _
                                                                      Optional ByVal OperationType As WSReferences.OperationTypes = WSReferences.OperationTypes.UND, _
 _
                                                                      Optional ByVal DisplayData As WSReferences.TypesDisplay = WSReferences.TypesDisplay.ShowAll) As XMLSchemaObject Implements IWSReferences.GetBasicInformationSchemaInKromBase

            Dim result_ As New TagWatcher

            Dim xmlSchemaObject_ As New XMLSchemaObject

            result_ = LogIn(MobileUserID, WebServiceUserID, WebServicePasswordID, 11, 1, False)

            If result_.Errors = ErrorTypes.WS000 Then

                xmlSchemaObject_ = ReturnXMLSchema(SchemaTypes.OnlyHeaders,
                                                   MobileUserID, _
                                                   RFCOperationsClient, _
                                                   CustomerName_, _
                                                   SupplierName_, _
                                                   VesselName_, _
                                                   , _
                                                   , _
                                                   , _
                                                   , _
                                                   ModalityType, _
                                                   CustomSection, _
                                                   OperationType, _
                                                   DisplayData)

            Else

                xmlSchemaObject_.TagWatcher = result_

            End If

            Return xmlSchemaObject_

        End Function

        Private Function GetReferenceInformationInKromBase(ByVal ThroughUserPermissions As String, _
                                                          ByVal WebServiceUserID As String, _
                                                          ByVal WebServicePasswordID As String, _
                                                          ByVal ReferenceID As String) As XMLSchemaObject

            _xmlSchemaObject = New XMLSchemaObject

            Dim listaReferencias_ As New List(Of Reference)

            Dim referencia1_ As New Reference

            referencia1_.CustomsDeclararionKey = "A1"

            referencia1_.CustomsDeclarationDocumentID = "00000001"

            referencia1_.CustomSection = 430

            referencia1_.DateLastStatus = Now

            referencia1_.Delivered = 0

            referencia1_.ID = "RKU16-00000"

            referencia1_.ManagementCompanyId = 1

            referencia1_.ModalityType = WSReferences.ModalityTypes.SEA

            referencia1_.OperationType = WSReferences.OperationTypes.IMP

            referencia1_.OperationYear = 2016

            referencia1_.OriginCountry = "EUA"

            referencia1_.StatusID = ReferenceStatusTypes.DOC

            referencia1_.StatusTag = ReferenceStatusTypes.DOC.ToString

            listaReferencias_.Add(referencia1_)

            Dim TagWatcher_ As New TagWatcher

            TagWatcher_.Errors = ErrorTypes.WS000

            TagWatcher_.Status = TypeStatus.Ok

            TagWatcher_.ResultsCount = 0

            _xmlSchemaObject.References.Add(referencia1_)

            _xmlSchemaObject.TagWatcher = TagWatcher_

            _xmlSchemaObject.SetXMLSchemaType = SchemaTypes.OnlyHeaders

            Dim schemaType_ As SchemaTypes = SchemaTypes.OnlyHeaders

            Select Case schemaType_

                Case SchemaTypes.OnlyHeaders

                    _xmlSchemaObject.SetXMLSchemaType = SchemaTypes.OnlyHeaders

                Case SchemaTypes.ReferenceType

                    _xmlSchemaObject.SetXMLSchemaType = SchemaTypes.ReferenceType

                Case SchemaTypes.Advanced1

                    _xmlSchemaObject.SetXMLSchemaType = SchemaTypes.Advanced1

            End Select

            Return _xmlSchemaObject

        End Function

        Public Function GetReferenceStatusListKromBase(ByVal ThroughUserPermissions As String, _
                                                       ByVal WebServiceUserID As String, _
                                                       ByVal WebServicePasswordID As String, _
                                                       ByVal ReferenceID As String, _
                                                       ByVal KromCompanyID As Integer) As TagWatcher _
                                                   Implements IWSReferences.GetReferenceStatusListKromBase

        End Function

        Public Function SetNewReferenceInKromBase(ByVal ThroughUserPermissions As String, _
                                                  ByVal WebServiceUserID As String, _
                                                  ByVal WebServicePasswordID As String, _
                                                  ByVal ReferenceID As String, _
                                                  ByVal SystemOwner As IWSReferences.SystemOwners, _
                                                  ByVal RFCOperationsClient As String, _
                                                  ByVal ClientCompanyName_ As String, _
                                                  ByVal ModalityType As WSReferences.ModalityTypes, _
                                                  ByVal KromCompanyID As Integer, _
                                                  ByVal CustomSection As WSReferences.CustomSections, _
                                                  ByVal OperationType As WSReferences.OperationTypes, _
                                                        Optional ByVal IDOperationsClient As Integer = 0, _
                                                        Optional ByVal IDAdministrativeClient As Integer = 0) As TagWatcher _
                                              Implements IWSReferences.SetNewReferenceInKromBase

        End Function

        Public Function SetReferenceStatusKromBase(ByVal ThroughUserPermissions As String, _
                                                   ByVal WebServiceUserID As String, _
                                                   ByVal WebServicePasswordID As String, _
                                                   ByVal ReferenceID As String, _
                                                   ByVal SystemOwner As IWSReferences.SystemOwners, _
                                                   ByVal SetThisStatus As ReferenceStatusTypes, _
                                                   ByVal KromCompanyID As Integer) As TagWatcher _
                                               Implements IWSReferences.SetReferenceStatusKromBase

        End Function


        ' ******************
        'SUPPORT FUNCTIONS
        ' ******************
        Public Function TestService(ByVal YourName As String) As String _
            Implements IWSReferences.TestService

            Return "hello " & YourName

        End Function

        Private Function CovertRuleToScript(ByRef result_ As TagWatcher) As String
            Dim script_ As String = Nothing

            Dim accessRules_ As String = Nothing

            Dim acessed_ As String = Nothing

            Dim denied_ As String = Nothing

            Dim counter_ As Int32 = 1

            Dim token_ As String = "and"

            Dim tokenOr_ As String = "or"

            For Each rule_ As DataAccessRules In _dataAccessRulesList

                If counter_ = 1 Then
                    token_ = Nothing
                    tokenOr_ = Nothing
                Else
                    token_ = "and"
                    tokenOr_ = "or"
                End If

                Select Case rule_.Accion

                    Case AccessTypes.Access

                        accessRules_ = " ( "

                    Case AccessTypes.Deny

                        Continue For

                End Select

                If rule_.IDDivisionMiEmpresa > 0 Then

                    accessRules_ = accessRules_ & " Reference.i_Cve_DivisionMiEmpresa =" & rule_.IDDivisionMiEmpresa

                End If

                If Not rule_.RFC Is Nothing Then

                    accessRules_ = accessRules_ & " and t_RfcExterno ='" & rule_.RFC & "'"

                End If
                If rule_.IDClienteEmpresa > 0 Then

                    accessRules_ = accessRules_ & " and Reference.i_Cve_VinClienteOperacion =" & rule_.IDClienteEmpresa
                    'accessRules_ = accessRules_ & " and i_Cve_ClienteEmpresa =" & rule_.IDClienteEmpresa

                End If

                If rule_.IDDivisionEmpresarialCliente > 0 Then

                    accessRules_ = accessRules_ & " and i_Cve_DivisionEmpresarialCliente =" & rule_.IDDivisionEmpresarialCliente

                End If

                If rule_.Aduana > 0 Then

                    accessRules_ = accessRules_ & " and i_AduanaSeccion = " & rule_.Aduana

                End If

                If rule_.Patente > 0 Then

                    accessRules_ = accessRules_ & " and i_Patente = " & rule_.Patente

                End If
                If rule_.TipoOperacion > 0 Then

                    accessRules_ = accessRules_ & " and i_TipoOperacion = " & rule_.TipoOperacion

                End If

                accessRules_ = accessRules_ & " ) "

                script_ = script_ & " " & tokenOr_ & " " & accessRules_

                counter_ += 1

            Next

            acessed_ = " ( " & script_ & ") "

            script_ = Nothing

            accessRules_ = Nothing

            counter_ = 1

            For Each rule_ As DataAccessRules In _dataAccessRulesList

                If counter_ = 1 Then
                    token_ = Nothing
                    tokenOr_ = Nothing
                Else
                    token_ = "and"
                    tokenOr_ = "or"
                End If

                Select Case rule_.Accion

                    Case AccessTypes.Access

                        Continue For

                    Case AccessTypes.Deny

                        accessRules_ = " ( "

                End Select

                If rule_.IDDivisionMiEmpresa > 0 Then

                    accessRules_ = accessRules_ & " Reference.i_Cve_DivisionMiEmpresa =" & rule_.IDDivisionMiEmpresa

                End If

                If Not rule_.RFC Is Nothing Then

                    accessRules_ = accessRules_ & " and t_RfcExterno ='" & rule_.RFC & "'"

                End If
                If rule_.IDClienteEmpresa > 0 Then

                    'accessRules_ = accessRules_ & " and i_Cve_ClienteEmpresa =" & rule_.IDClienteEmpresa
                    accessRules_ = accessRules_ & " and Reference.i_Cve_VinClienteOperacion =" & rule_.IDClienteEmpresa

                End If

                If rule_.IDDivisionEmpresarialCliente > 0 Then

                    accessRules_ = accessRules_ & " and i_Cve_DivisionEmpresarialCliente =" & rule_.IDDivisionEmpresarialCliente

                End If

                If rule_.Aduana > 0 Then

                    accessRules_ = accessRules_ & " and i_AduanaSeccion = " & rule_.Aduana

                End If

                If rule_.Patente > 0 Then

                    accessRules_ = accessRules_ & " and i_Patente = " & rule_.Patente

                End If
                If rule_.TipoOperacion > 0 Then

                    accessRules_ = accessRules_ & " and i_TipoOperacion = " & rule_.TipoOperacion

                End If

                accessRules_ = accessRules_ & " ) "

                script_ = script_ & " " & tokenOr_ & " " & accessRules_

                counter_ += 1

            Next

            denied_ = Nothing

            If Not script_ Is Nothing And Trim(script_) <> "" Then

                denied_ = " not ( " & script_ & ") "

            End If

            If Not acessed_ Is Nothing And Trim(acessed_) <> "" Then

                _scriptOfRules = " and " & acessed_

            End If

            If Not denied_ Is Nothing And Trim(denied_) <> "" Then

                _scriptOfRules = _scriptOfRules & " and " & denied_

            End If

            If _scriptOfRules Is Nothing Then

                result_.SetError(ErrorTypes.WS008, "Not enough rules to data access")

            End If

            Return _scriptOfRules

        End Function

        Private Sub SessionPrepare(ByVal user_ As String, _
                                   ByVal password_ As String)

            _sesion = New SesionWcf

            With _sesion

                .MininimoCaracteresUsuario = 7

                .MinimoNumerosUsuario = 0

                .MinimoCaracteresContrasena = 7

                .MinimoMayusculasContrasena = 2

                .MinimoMinusculasContrasena = 2

                .MinimoNumerosContrasena = 2

                .GrupoEmpresarial = 1

                .DivisionEmpresarial = 1

                .Aplicacion = 4

                .IdentificadorUsuario = Trim(user_)

                .ContraseniaUsuario = Trim(password_)

            End With

        End Sub

        Private Function LogIn(ByVal MobileUserID As String,
                               ByVal WebServiceUserID As String,
                               ByVal WebServicePasswordID As String,
                               ByVal IdRequiredApplication As Integer,
                               ByVal CorporateNumber As Integer,
                               Optional ByVal FullAuthentication As Boolean = False) As TagWatcher

            Dim enumerator As IEnumerator = Nothing

            Dim results_ As TagWatcher = New TagWatcher

            _sessionStatus = False

            SessionPrepare(WebServiceUserID, WebServicePasswordID)

            If (WSReferences._sesion.StatusArgumentos) Then
                'If (Not WSReferences._sesion.StatusArgumentos) Then

                results_.SetError(TagWatcher.ErrorTypes.WS005, Nothing)

            Else

                _sessionStatus = True

                WSReferences._sesion.GrupoEmpresarial = CorporateNumber

                WSReferences._sesion.DivisionEmpresarial = 1

                WSReferences._sesion.Aplicacion = IdRequiredApplication

                WSReferences._sesion.Idioma = ISesion.Idiomas.Espaniol
                'WSReferences._sesion.Idioma = ISesionWcf.Idiomas.Espaniol

                If (FullAuthentication) Then

                    _sessionStatus = WSReferences._sesion.StatusArgumentos

                    If (Not _sessionStatus) Then

                        results_.SetError(TagWatcher.ErrorTypes.WS004, Nothing)

                    ElseIf (Not (WSReferences._sesion.StatusArgumentos And WSReferences._sesion.EspacioTrabajo IsNot Nothing)) Then

                        results_.SetError(TagWatcher.ErrorTypes.WS003, Nothing)

                    Else

                        _iOperations = New OperacionesCatalogo()

                        _iOperations = DirectCast(_system.ConsultaModulo(WSReferences._sesion.EspacioTrabajo, "Usuarios", String.Concat(" and t_usuario = '", MobileUserID, "'")).Clone(), IOperacionesCatalogo)

                        If (Not _system.TieneResultados(_iOperations)) Then

                            results_.SetError(TagWatcher.ErrorTypes.WS002, Nothing)

                        Else

                            results_.SetOK(0)

                        End If

                    End If

                Else

                    _iOperations = _system.EnsamblaModulo("Usuarios")

                    Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo()

                    With temporalWorkSpace_

                        .ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

                    End With

                    _iOperations.EspacioTrabajo = temporalWorkSpace_

                    _iOperations.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                    _iOperations.ClausulasLibres = String.Concat(" and t_usuario = '", MobileUserID, "'")

                    _iOperations.CantidadVisibleRegistros = 1000

                    _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                    If (_iOperations.GenerarVista("%", Nothing, "like") <> IOperacionesCatalogo.EstadoOperacion.COk) Then

                        results_.SetError(TagWatcher.ErrorTypes.WS001, Nothing)

                    ElseIf (Not _system.TieneResultados(_iOperations)) Then

                        results_.SetError(TagWatcher.ErrorTypes.WS007, "1 This mobile user doesn't have client rules to access profile,  although was authenticated")

                    Else

                        _iOperations = _system.EnsamblaModulo("AccesoUsuariosClavesCliente")

                        _iOperations.ClausulasLibres = String.Concat(" and t_Usuario = '", MobileUserID, "'")

                        _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                        If (_iOperations.GenerarVista("%", Nothing, "like") <> IOperacionesCatalogo.EstadoOperacion.COk) Then

                            results_.SetError(TagWatcher.ErrorTypes.WS007, "2 This mobile user doesn't have client rules to access profile,  although was authenticated")

                        ElseIf (Not _system.TieneResultados(_iOperations)) Then

                            results_.SetError(TagWatcher.ErrorTypes.WS007, "3 This mobile user doesn't have client rules to access profile,  although was authenticated")

                        ElseIf (Not _iOperations.Vista.Tables(0).Columns.Contains("i_Cve_AccesoUsuarioClavesCliente")) Then

                            results_.SetError(TagWatcher.ErrorTypes.WS007, "4 This mobile user doesn't have client rules to access profile,  although was authenticated")

                        Else

                            Try

                                enumerator = _iOperations.Vista.Tables(0).Rows.GetEnumerator()

                                While enumerator.MoveNext()

                                    Dim row_ As DataRow = DirectCast(enumerator.Current, DataRow)

                                    Dim accessRule_ As DataAccessRules = New DataAccessRules()

                                    With accessRule_

                                        .IDAccesoUsuarioClavesCliente = Conversions.ToInteger(row_("i_Cve_AccesoUsuarioClavesCliente"))

                                        .IDDivisionMiEmpresa = Conversions.ToInteger(row_("i_Cve_DivisionMiEmpresa"))

                                    End With

                                    If (DBNull.Value.Equals(RuntimeHelpers.GetObjectValue(row_("t_RFC")))) Then

                                        accessRule_.RFC = Nothing

                                    Else

                                        accessRule_.RFC = Conversions.ToString(row_("t_RFC"))

                                    End If

                                    accessRule_.IDClienteEmpresa = Conversions.ToInteger(row_("i_Cve_ClienteEmpresa"))

                                    accessRule_.IDDivisionEmpresarialCliente = Conversions.ToInteger(row_("i_Cve_DivisionEmpresarialCliente"))

                                    accessRule_.Aplicacion = DirectCast(Conversions.ToInteger(row_("i_Cve_Aplicacion")), WSReferences.ApplicationTypes)

                                    accessRule_.Accion = DirectCast(Conversions.ToInteger(row_("i_Cve_Accion")), WSReferences.AccessTypes)

                                    accessRule_.Aduana = DirectCast(Conversions.ToInteger(row_("i_Aduana")), WSReferences.CustomSections)

                                    accessRule_.Patente = DirectCast(Conversions.ToInteger(row_("i_Patente")), WSReferences.PatentNumbers)

                                    accessRule_.TipoOperacion = DirectCast(Conversions.ToInteger(row_("i_TipoOperacion")), WSReferences.OperationTypes)

                                    _dataAccessRulesList.Add(accessRule_)

                                End While

                            Finally

                                If (TypeOf enumerator Is IDisposable) Then

                                    TryCast(enumerator, IDisposable).Dispose()

                                End If

                            End Try

                            CovertRuleToScript(results_)

                        End If

                    End If

                End If

            End If

            Return results_

        End Function

        'FRAMEWORK

        'Private Function LogIn(ByVal MobileUserID As String, _
        '                            ByVal WebServiceUserID As String, _
        '                            ByVal WebServicePasswordID As String, _
        '                            ByVal IdRequiredApplication As Integer, _
        '                            ByVal CorporateNumber As Integer, _
        '                            Optional ByVal FullAuthentication As Boolean = False) As TagWatcher

        '    Dim results_ As New TagWatcher

        '    _sessionStatus = False

        '    SessionPrepare(WebServiceUserID, _
        '                   WebServicePasswordID)

        '    If _sesion.StatusArgumentos Then

        '        _sessionStatus = True

        '        _sesion.GrupoEmpresarial = CorporateNumber

        '        _sesion.DivisionEmpresarial = 1

        '        _sesion.Aplicacion = IdRequiredApplication

        '        _sesion.Idioma = ISesion.Idiomas.Espaniol

        '        If Not FullAuthentication Then

        '            _iOperations = _system.EnsamblaModulo("Usuarios")

        '            Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

        '            temporalWorkSpace_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

        '            _iOperations.EspacioTrabajo = temporalWorkSpace_

        '            _iOperations.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

        '            _iOperations.ClausulasLibres = " and t_usuario = '" & MobileUserID & "'"

        '            _iOperations.CantidadVisibleRegistros = 1000

        '            _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

        '            If _iOperations.GenerarVista() = IOperacionesCatalogo.EstadoOperacion.COk Then

        '                If _system.TieneResultados(_iOperations) Then

        '                    _iOperations = _system.EnsamblaModulo("AccesoUsuariosClavesCliente")

        '                    _iOperations.ClausulasLibres = " and t_Usuario = '" & MobileUserID & "'"

        '                    _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

        '                    If _iOperations.GenerarVista() = IOperacionesCatalogo.EstadoOperacion.COk Then

        '                        If _system.TieneResultados(_iOperations) Then

        '                            If _iOperations.Vista.Tables(0).Columns.Contains("i_Cve_AccesoUsuarioClavesCliente") Then

        '                                For Each row_ As DataRow In _iOperations.Vista.Tables(0).Rows

        '                                    Dim accessRule_ As New DataAccessRules

        '                                    accessRule_.IDAccesoUsuarioClavesCliente = row_("i_Cve_AccesoUsuarioClavesCliente")

        '                                    accessRule_.IDDivisionMiEmpresa = row_("i_Cve_DivisionMiEmpresa")

        '                                    If Not DBNull.Value.Equals(row_("t_RFC")) Then
        '                                        accessRule_.RFC = row_("t_RFC")
        '                                    Else : accessRule_.RFC = Nothing
        '                                    End If
        '                                    'accessRule_.RFC = row_("t_RFC")

        '                                    accessRule_.IDClienteEmpresa = row_("i_Cve_ClienteEmpresa")

        '                                    accessRule_.IDDivisionEmpresarialCliente = row_("i_Cve_DivisionEmpresarialCliente")

        '                                    accessRule_.Aplicacion = row_("i_Cve_Aplicacion")

        '                                    accessRule_.Accion = row_("i_Cve_Accion")

        '                                    accessRule_.Aduana = row_("i_Aduana")

        '                                    accessRule_.Patente = row_("i_Patente")

        '                                    accessRule_.TipoOperacion = row_("i_TipoOperacion")

        '                                    _dataAccessRulesList.Add(accessRule_)

        '                                Next

        '                                CovertRuleToScript(results_)
        '                            Else
        '                                results_.SetError(ErrorTypes.WS007, "4 This mobile user doesn't have client rules to access profile,  although was authenticated")
        '                            End If

        '                        Else

        '                            results_.SetError(ErrorTypes.WS007, "3 This mobile user doesn't have client rules to access profile,  although was authenticated")

        '                        End If

        '                    Else

        '                        results_.SetError(ErrorTypes.WS007, "2 This mobile user doesn't have client rules to access profile,  although was authenticated")

        '                    End If
        '                Else
        '                    results_.SetError(ErrorTypes.WS007, "1 This mobile user doesn't have client rules to access profile,  although was authenticated")
        '                End If

        '                'Else
        '                '    results_.SetError(ErrorTypes.WS007, " This mobile user doesn't have client rules to access profile,  although was authenticated")
        '                'End If
        '            Else

        '                results_.SetError(ErrorTypes.WS001)

        '            End If

        '        Else

        '            _sessionStatus = _sesion.StatusArgumentos

        '            If _sessionStatus Then

        '                If _sesion.StatusArgumentos = True And _
        '                    _sesion.EspacioTrabajo IsNot Nothing Then

        '                    _iOperations = New OperacionesCatalogo

        '                    _iOperations = _system.ConsultaModulo(_sesion.EspacioTrabajo, "Usuarios", " and t_usuario = '" & MobileUserID & "'").Clone

        '                    If _system.TieneResultados(_iOperations) Then

        '                        results_.SetOK()

        '                    Else
        '                        results_.SetError(ErrorTypes.WS002)

        '                    End If

        '                Else

        '                    results_.SetError(ErrorTypes.WS003)

        '                End If


        '            Else

        '                results_.SetError(ErrorTypes.WS004)

        '            End If

        '        End If
        '    Else

        '        results_.SetError(ErrorTypes.WS005)

        '    End If

        '    Return results_

        'End Function

        Private Sub AddStatus(ByRef _statusList As List(Of StatusItem), _
                              ByVal _orderNumber As Int32, _
                              ByVal _idStatus As ReferenceStatusTypes, _
                              ByVal _lastDate As DateTime, _
                             Optional ByVal _isDone As Boolean = False)


            Dim _item1 As New StatusItem : _item1.IsDone = _isDone : _item1.OrderNumber = _orderNumber : _item1.StatusType = _idStatus : _item1.LastDateTime = _lastDate

            _statusList.Add(_item1)
        End Sub

        Private Sub CheckInvoce(ByVal reference_ As String, _
                                ByRef _fac As Boolean, _
                                ByRef _lfac As Boolean, _
                                ByRef _facDate As DateTime, _
                                ByRef _lfacDate As DateTime)

            Dim respuesta_ As Boolean = False

            Dim query_ As String = Nothing

            query_ = " Select top(1) f_FechaEmision,i_cve_Estatus,SaldoActual, ultimoMovimiento from Vt014Facturas" & _
                        " where t_referenciasFactura like '%" & Trim(reference_) & "%'" & _
                        " order by f_FechaEmision desc"

            _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            respuesta_ = _system.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(query_)

            If respuesta_ Then

                If Not _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente Is Nothing Then
                    If Not _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then
                        If _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count > 0 Then

                            For Each registro_ As DataRow In _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows

                                Dim saldoActual_ As String = Nothing

                                Dim estatusActual_ As String = Nothing


                                saldoActual_ = registro_("SaldoActual")

                                estatusActual_ = registro_("i_cve_Estatus")


                                Select Case estatusActual_

                                    Case "0"

                                        _fac = False

                                    Case "1"

                                        _fac = True

                                        _facDate = registro_("f_FechaEmision")

                                        Select Case saldoActual_

                                            Case "0"

                                                _lfac = True

                                                _lfacDate = registro_("UltimoMovimiento")

                                            Case Else

                                                _lfac = False

                                                _lfacDate = registro_("UltimoMovimiento")

                                        End Select

                                    Case "2"

                                    Case "3"

                                End Select

                            Next

                        End If
                    End If
                End If
            End If
        End Sub

        Private Function CheckDateStatus(ByRef tStatus As DataTable, ByVal status_ As String) As Date

            Dim resultado_ As Date = Nothing

            If tStatus.Rows.Count > 0 Then

                If tStatus.Columns.Contains("t_Referencia") Then

                    Dim registro_ As DataRow() = tStatus.Select("t_claveAccion = '" & status_ & "'")

                    If registro_.Length > 0 Then

                        resultado_ = registro_(0)("Fecha")

                    End If

                End If

            End If

            Return resultado_

        End Function

        Private Function GetStatusList(ByVal reference_ As Reference) As List(Of StatusItem)

            Dim _query As String

            Dim tablaStatus As DataTable

            Dim FechaStatus As Date

            Dim _statusList As New List(Of StatusItem)

            Dim _operationModal As CustomSections = reference_.CustomSection

            Dim _operationType As OperationTypes = reference_.OperationType

            Dim _dsp As Boolean = False : If reference_.Delivered = "1" Then : _dsp = True : End If

            Dim _alr As Boolean = False : If reference_.Delivered = "1" Then : _alr = True : End If

            Dim _doc As Boolean = False : If reference_.Delivered = "1" Then : _doc = True : End If

            Dim _rev As Boolean = False : If reference_.Delivered = "1" Then : _rev = True : End If

            Dim _rgu As Boolean = False : If reference_.Delivered = "1" Then : _rgu = True : End If

            Dim _pre As Boolean = False : If reference_.Delivered = "1" Then : _pre = True : End If

            Dim _pag As Boolean = False : If reference_.Delivered = "1" Then : _pag = True : End If

            Dim _atr As Boolean = False

            Dim _amx As Boolean = False

            Dim _sar As Boolean = False

            Dim _rad As Boolean = False

            Dim _fac As Boolean = False

            Dim _facDate As New DateTime

            Dim _lfac As Boolean = False

            Dim _lfacDate As New DateTime

            '/*******************************************************************/
            'Desactivo consulta de estatus para verificar estatus de facturacion y liquidacion de facturacion
            'CheckInvoce(reference_.ID, _fac, _lfac, _facDate, _lfacDate)
            '/*******************************************************************/

            'If reference_.Delivered = "1" Then
            '    _fac = True
            'End If

            Dim _cam As Boolean = False : If reference_.Delivered = "1" Then : _cam = True : End If

            Dim _cru As Boolean = False : If reference_.Delivered = "1" Then : _cru = True : End If

            Dim _ieu As Boolean = False

            Dim _MedioTransporteArribo As Integer = 0

            Dim _ata As Boolean = False

            Dim _sse As Boolean = False

            'AddStatus(_statusList, 0, ReferenceStatusTypes.ALR, Now, _alr)

            _query = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " & _
                    "SELECT MP.i_Cve_MaestroOperaciones, MP.t_Referencia, ET.i_Cve_PlantillaTracking, AT.i_Cve_AccionTracking," & _
                        "AT.t_ClaveAccion,MAX(DT.f_Alta) AS Fecha, Vi.i_cve_MedioTransporteArribo " & _
                    "FROM Reg009MaestroOperaciones MP  with (nolock) " & _
                        "INNER JOIN Enc008Tracking ET  with (nolock) ON (MP.i_Cve_MaestroOperaciones=ET.i_Cve_Operacion) " & _
                        "INNER JOIN Det008TrackingDetalles DT  with (nolock) ON (ET.i_Cve_Tracking=DT.i_Cve_EncabezadoTracking) " & _
                        "INNER JOIN Cat008AccionesTracking AT  with (nolock) ON (DT.i_Cve_AccionTracking=AT.i_Cve_AccionTracking) " & _
                        "INNER JOIN Vin003OperacionesAgenciasAduanales Vi  with (nolock) ON (vi.t_NumeroReferencia=MP.t_Referencia) " & _
                        "WHERE Dt.i_cve_estado=1 and ET.i_cve_estado=1 and MP.i_cve_estado=1 and MP.t_Referencia='" & reference_.ID & "' " & _
                    "GROUP BY MP.i_Cve_MaestroOperaciones, MP.t_Referencia, ET.i_Cve_PlantillaTracking, AT.i_Cve_AccionTracking,AT.t_ClaveAccion,Vi.i_cve_MedioTransporteArribo "

            '_system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            'If _system.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_query) Then
            '    If Not _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente Is Nothing And
            '       Not _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing And
            '       _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count > 0 Then

            '        tablaStatus = New DataTable
            '        tablaStatus = _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0)
            '        _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            '        End If
            '    End If

            Dim conexion_ As IConexiones = New Conexiones

            conexion_.ControladorBaseDatos = IConexiones.Controladores.SQLServer2008

            'conexion_.Contrasena = "S0l1umF0rW"
            'conexion_.Usuario = "sa"
            'conexion_.NombreBaseDatos = "solium"
            'conexion_.IpServidor = "10.66.1.19"

            'conexion_.Contrasena = "K1s45gri"
            'conexion_.Usuario = "krombase"
            'conexion_.NombreBaseDatos = "solium"
            'conexion_.IpServidor = "10.66.1.5"

            'NUBE
            conexion_.IpServidor = "10.66.100.5"
            conexion_.NombreBaseDatos = "solium"
            conexion_.Usuario = "sa"
            conexion_.Contrasena = "S0l1umF0rW"

            conexion_.ObjetoDatos = IConexiones.TipoConexion.SqlCommand
            'conexion_.IniciaConexion()
            'conexion_.EjecutaConsulta("")
            'conexion_.TerminaConexion()
            conexion_.DataSetReciente.Tables.Clear()

            If conexion_.EjecutaConsultaIndividual(_query) Then
                If Not conexion_.DataSetReciente Is Nothing And
                   Not conexion_.DataSetReciente.Tables Is Nothing And conexion_.DataSetReciente.Tables.Count > 0 Then

                    '----------------------------------------------------------------------------------------------
                    'INSERTAR REGISTRO DE HISTORICO DE CONSULTA DE LISTA DE TRACKING
                    '----------------------------------------------------------------------------------------------
                    '_query = "INSERT INTO bit021HistoricoAccesoWCF (t_usuario,t_TipoAcceso,f_FechaRegistro) " & _
                    '"VALUES ('" & MobileUserID & "',1,'" & Now.ToString("yyyy-MM-dd hh:mm:ss.f") & "') "
                    'conexion_.EjecutaSentencia(_query)

                    tablaStatus = New DataTable
                    tablaStatus = conexion_.DataSetReciente.Tables(0)
                    If Not DBNull.Value.Equals(conexion_.DataSetReciente.Tables(0).Rows(0)("i_cve_MedioTransporteArribo")) Then
                        If conexion_.DataSetReciente.Tables(0).Rows(0)("i_cve_MedioTransporteArribo") <> 0 Then
                            _MedioTransporteArribo = conexion_.DataSetReciente.Tables(0).Rows(0)("i_cve_MedioTransporteArribo")
                        End If
                    End If
                    conexion_.DataSetReciente.Tables.Clear()
                End If
            End If

            'Verifica si se trata de una referencia de Atlas, ATV
            If reference_.ID.Substring(0, 3) = "ATV" Then
                _operationModal = CustomSections.UND
            End If

            Select Case _operationModal
                'Maritimo
                Case CustomSections.ALC, CustomSections.CEG, CustomSections.RKU, CustomSections.SAP, CustomSections.TUX, CustomSections.TUX1

                    Select Case _operationType

                        Case OperationTypes.IMP

                            'DOC()
                            AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                            'ATA()
                            AddStatus(_statusList, 2, ReferenceStatusTypes.ATA, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ATA.ToString), _ata)

                            'REV()  
                            AddStatus(_statusList, 3, ReferenceStatusTypes.REV, CheckDateStatus(tablaStatus, ReferenceStatusTypes.REV.ToString), _rev)

                            'PRE()
                            AddStatus(_statusList, 4, ReferenceStatusTypes.PRE, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PRE.ToString), _pre)

                            'PAG()
                            AddStatus(_statusList, 5, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                            'SAR()                            
                            AddStatus(_statusList, 6, ReferenceStatusTypes.SAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SAR.ToString), _sar)

                            'RAD()
                            AddStatus(_statusList, 7, ReferenceStatusTypes.RAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RAD.ToString), _rad)

                            'DSP()
                            AddStatus(_statusList, 8, ReferenceStatusTypes.DSP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DSP.ToString), _dsp)

                            'FAC()
                            'AddStatus(_statusList, 8, ReferenceStatusTypes.FAC, _facDate, _fac)
                            AddStatus(_statusList, 9, ReferenceStatusTypes.FAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.FAC.ToString), _fac)

                            'LFAC()
                            'AddStatus(_statusList, 9, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)
                            AddStatus(_statusList, 10, ReferenceStatusTypes.LFAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LFAC.ToString), _lfac)

                        Case OperationTypes.EXP

                            'DOC()
                            AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                            'PAG()
                            AddStatus(_statusList, 2, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                            'SAR()                            
                            AddStatus(_statusList, 3, ReferenceStatusTypes.SAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SAR.ToString), _sar)

                            'RAD()
                            AddStatus(_statusList, 4, ReferenceStatusTypes.RAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RAD.ToString), _rad)

                            'DSP()
                            AddStatus(_statusList, 5, ReferenceStatusTypes.DSP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DSP.ToString), _dsp)

                            'FAC()
                            'AddStatus(_statusList, 6, ReferenceStatusTypes.FAC, _facDate, _fac)
                            AddStatus(_statusList, 6, ReferenceStatusTypes.FAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.FAC.ToString), _fac)

                            'LFAC()
                            'AddStatus(_statusList, 7, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)
                            AddStatus(_statusList, 7, ReferenceStatusTypes.LFAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LFAC.ToString), _lfac)

                    End Select

                    'Aereos
                Case CustomSections.DAI, CustomSections.TOL, CustomSections.TOL1

                    Select Case _operationType

                        Case OperationTypes.IMP

                            'DOC()
                            AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                            'ATA()
                            AddStatus(_statusList, 2, ReferenceStatusTypes.ATA, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ATA.ToString), _ata)

                            'REV() Se comenta por no estar en Catalogo
                            'AddStatus(_statusList, 2, ReferenceStatusTypes.REV, CheckDateStatus(tablaStatus, ReferenceStatusTypes.REV.ToString), _rev)

                            'RGU()
                            AddStatus(_statusList, 4, ReferenceStatusTypes.RGU, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RGU.ToString), _rgu)

                            'PRE() Se agregar por estar en catalogo
                            AddStatus(_statusList, 3, ReferenceStatusTypes.PRE, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PRE.ToString), _pre)

                            'PAG()
                            AddStatus(_statusList, 5, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                            'SAR()                            
                            AddStatus(_statusList, 6, ReferenceStatusTypes.SAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SAR.ToString), _sar)

                            'RAD()
                            AddStatus(_statusList, 7, ReferenceStatusTypes.RAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RAD.ToString), _rad)

                            'DSP()
                            AddStatus(_statusList, 8, ReferenceStatusTypes.DSP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DSP.ToString), _dsp)

                            'FAC()
                            'AddStatus(_statusList, 8, ReferenceStatusTypes.FAC, _facDate, _fac)
                            AddStatus(_statusList, 9, ReferenceStatusTypes.FAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.FAC.ToString), _fac)

                            'LFAC()
                            'AddStatus(_statusList, 9, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)
                            AddStatus(_statusList, 10, ReferenceStatusTypes.LFAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LFAC.ToString), _lfac)

                        Case OperationTypes.EXP

                            'DOC()
                            AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                            'PAG()
                            AddStatus(_statusList, 2, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                            'SAR()                            
                            AddStatus(_statusList, 3, ReferenceStatusTypes.SAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SAR.ToString), _sar)

                            'RAD()
                            AddStatus(_statusList, 4, ReferenceStatusTypes.RAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RAD.ToString), _rad)

                            'DSP()
                            AddStatus(_statusList, 5, ReferenceStatusTypes.DSP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DSP.ToString), _dsp)

                            'FAC()
                            'AddStatus(_statusList, 6, ReferenceStatusTypes.FAC, _facDate, _fac)
                            AddStatus(_statusList, 6, ReferenceStatusTypes.FAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.FAC.ToString), _fac)

                            'LFAC()
                            'AddStatus(_statusList, 7, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)
                            AddStatus(_statusList, 7, ReferenceStatusTypes.LFAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LFAC.ToString), _lfac)

                    End Select

                    'Terrestres
                Case CustomSections.PAN, CustomSections.SFI, CustomSections.SFC

                    Select Case _operationType

                        Case OperationTypes.IMP
                            'Si el medio de arribo es 6 (Ferroviario) de lo contrario lo considera como carretero
                            If _MedioTransporteArribo = 6 Then

                                'ATR() Se agrega por estar en catalogo
                                'AddStatus(_statusList, 1, ReferenceStatusTypes.ATR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ATR.ToString), _atr)

                                'DOC()
                                AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                                'ATA()
                                AddStatus(_statusList, 2, ReferenceStatusTypes.ATA, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ATA.ToString), _ata)

                                'PAG()
                                AddStatus(_statusList, 3, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                                'AMX()
                                'AddStatus(_statusList, 5, ReferenceStatusTypes.AMX, CheckDateStatus(tablaStatus, ReferenceStatusTypes.AMX.ToString), _amx)

                                'ATR() Se agrega por estar en catalogo
                                AddStatus(_statusList, 4, ReferenceStatusTypes.ATR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ATR.ToString), _atr)

                                'SAR()                            
                                AddStatus(_statusList, 5, ReferenceStatusTypes.SAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SAR.ToString), _sar)

                                'RAD()
                                AddStatus(_statusList, 6, ReferenceStatusTypes.RAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RAD.ToString), _rad)

                                'DSP()
                                AddStatus(_statusList, 7, ReferenceStatusTypes.DSP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DSP.ToString), _dsp)

                                'FAC()
                                'AddStatus(_statusList, 12, ReferenceStatusTypes.FAC, _facDate, _fac)
                                AddStatus(_statusList, 8, ReferenceStatusTypes.FAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.FAC.ToString), _fac)

                                'LFAC()
                                'AddStatus(_statusList, 13, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)
                                AddStatus(_statusList, 9, ReferenceStatusTypes.LFAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LFAC.ToString), _lfac)

                            Else

                                'RBO()
                                AddStatus(_statusList, 1, ReferenceStatusTypes.RBO, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RBO.ToString), _doc)

                                'PRE()
                                AddStatus(_statusList, 2, ReferenceStatusTypes.PRE, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PRE.ToString), _pre)

                                'DOC()
                                AddStatus(_statusList, 3, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                                'ATA()
                                AddStatus(_statusList, 4, ReferenceStatusTypes.ATA, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ATA.ToString), _ata)

                                'PAG()
                                AddStatus(_statusList, 5, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                                'CAM()
                                AddStatus(_statusList, 6, ReferenceStatusTypes.CAM, CheckDateStatus(tablaStatus, ReferenceStatusTypes.CAM.ToString), _cam)

                                'CRU()
                                AddStatus(_statusList, 7, ReferenceStatusTypes.CRU, CheckDateStatus(tablaStatus, ReferenceStatusTypes.CRU.ToString), _cru)

                                'SAR()                            
                                AddStatus(_statusList, 8, ReferenceStatusTypes.SAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SAR.ToString), _sar)

                                'RAD()
                                AddStatus(_statusList, 9, ReferenceStatusTypes.RAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RAD.ToString), _rad)

                                'DSP()
                                AddStatus(_statusList, 10, ReferenceStatusTypes.DSP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DSP.ToString), _dsp)

                                'FAC()
                                'AddStatus(_statusList, 12, ReferenceStatusTypes.FAC, _facDate, _fac)
                                AddStatus(_statusList, 11, ReferenceStatusTypes.FAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.FAC.ToString), _fac)

                                'LFAC()
                                'AddStatus(_statusList, 13, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)
                                AddStatus(_statusList, 12, ReferenceStatusTypes.LFAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LFAC.ToString), _lfac)

                            End If

                        Case OperationTypes.EXP

                            'Si el medio de arribo es 6 (Ferroviario) de lo contrario lo considera como carretero
                            If _MedioTransporteArribo = 6 Then

                                'DOC()
                                AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                                'ATR()
                                'AddStatus(_statusList, 2, ReferenceStatusTypes.ATR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ATR.ToString), _atr)

                                'CRU()
                                AddStatus(_statusList, 2, ReferenceStatusTypes.CRU, CheckDateStatus(tablaStatus, ReferenceStatusTypes.CRU.ToString), _cru)

                                'PAG()
                                AddStatus(_statusList, 3, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                                'SAR()                            
                                AddStatus(_statusList, 4, ReferenceStatusTypes.SAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SAR.ToString), _sar)

                                'RAD()
                                AddStatus(_statusList, 5, ReferenceStatusTypes.RAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RAD.ToString), _rad)

                                'DSP()
                                AddStatus(_statusList, 6, ReferenceStatusTypes.DSP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DSP.ToString), _dsp)

                                'IEU()
                                'AddStatus(_statusList, 7, ReferenceStatusTypes.IEU, CheckDateStatus(tablaStatus, ReferenceStatusTypes.IEU.ToString), _ieu)

                                'FAC()
                                'AddStatus(_statusList, 10, ReferenceStatusTypes.FAC, _facDate, _fac)
                                AddStatus(_statusList, 7, ReferenceStatusTypes.FAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.FAC.ToString), _fac)

                                'LFAC()
                                'AddStatus(_statusList, 11, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)
                                AddStatus(_statusList, 8, ReferenceStatusTypes.LFAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LFAC.ToString), _lfac)

                            Else

                                'DOC()
                                AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                                'AME
                                'AddStatus(_statusList, 2, ReferenceStatusTypes.AME, CheckDateStatus(tablaStatus, ReferenceStatusTypes.AME.ToString), _cam)

                                'PAG()
                                AddStatus(_statusList, 2, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                                'CRU()
                                AddStatus(_statusList, 3, ReferenceStatusTypes.CRU, CheckDateStatus(tablaStatus, ReferenceStatusTypes.CRU.ToString), _cru)

                                'SAR()                            
                                AddStatus(_statusList, 4, ReferenceStatusTypes.SAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SAR.ToString), _sar)

                                'RAD()
                                AddStatus(_statusList, 5, ReferenceStatusTypes.RAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RAD.ToString), _rad)

                                'DSP()
                                AddStatus(_statusList, 6, ReferenceStatusTypes.DSP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DSP.ToString), _dsp)

                                'IEU()
                                'AddStatus(_statusList, 8, ReferenceStatusTypes.IEU, CheckDateStatus(tablaStatus, ReferenceStatusTypes.IEU.ToString), _ieu)

                                'FAC()
                                'AddStatus(_statusList, 10, ReferenceStatusTypes.FAC, _facDate, _fac)
                                AddStatus(_statusList, 7, ReferenceStatusTypes.FAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.FAC.ToString), _fac)

                                'LFAC()
                                'AddStatus(_statusList, 11, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)
                                AddStatus(_statusList, 8, ReferenceStatusTypes.LFAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LFAC.ToString), _lfac)

                            End If

                    End Select

                    'Logistica
                Case CustomSections.UND

                    Select Case _operationType

                        Case OperationTypes.IMP, OperationTypes.EXP, OperationTypes.UND

                            'SSE()
                            AddStatus(_statusList, 1, ReferenceStatusTypes.SSE, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SSE.ToString), False)

                            'BOO()
                            AddStatus(_statusList, 2, ReferenceStatusTypes.BOO, CheckDateStatus(tablaStatus, ReferenceStatusTypes.BOO.ToString), False)

                            'ATA()
                            AddStatus(_statusList, 3, ReferenceStatusTypes.ATA, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ATA.ToString), False)

                            'PPL()
                            AddStatus(_statusList, 4, ReferenceStatusTypes.PPL, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PPL.ToString), False)

                            'ZAR()
                            AddStatus(_statusList, 5, ReferenceStatusTypes.ZAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ZAR.ToString), False)

                            'INR()
                            AddStatus(_statusList, 6, ReferenceStatusTypes.INR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.INR.ToString), False)

                            'DES()
                            AddStatus(_statusList, 7, ReferenceStatusTypes.DES, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DES.ToString), False)

                            'CAT()
                            AddStatus(_statusList, 8, ReferenceStatusTypes.CAT, CheckDateStatus(tablaStatus, ReferenceStatusTypes.CAT.ToString), False)

                            'TBD()
                            AddStatus(_statusList, 9, ReferenceStatusTypes.TBD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.TBD.ToString), False)

                            'ABU()
                            AddStatus(_statusList, 10, ReferenceStatusTypes.ABU, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ABU.ToString), False)

                            'LLD()
                            AddStatus(_statusList, 11, ReferenceStatusTypes.LLD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LLD.ToString), False)

                            'ATER()
                            AddStatus(_statusList, 12, ReferenceStatusTypes.ATER, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ATER.ToString), False)

                            'CAD()
                            AddStatus(_statusList, 13, ReferenceStatusTypes.CAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.CAD.ToString), False)

                            'ENP()
                            'AddStatus(_statusList, 12, ReferenceStatusTypes.ENP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ENP.ToString), False)

                            'ENT()
                            AddStatus(_statusList, 14, ReferenceStatusTypes.ENT, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ENT.ToString), False)

                    End Select

            End Select

            Return _statusList

        End Function

        'WS PRODUCCIÓN
        Private Function ReturnXMLSchema(ByVal schemaType_ As WSReferences.SchemaTypes,
                                         ByVal MobileUserID As String,
                                         Optional ByVal RFCOperationsClient As String = Nothing,
                                         Optional ByVal CustomerName_ As String = Nothing,
                                         Optional ByVal SupplierName_ As String = Nothing,
                                         Optional ByVal VesselName_ As String = Nothing,
                                         Optional ByVal ReferenceNumber As String = Nothing,
                                         Optional ByVal CustomsDocument As String = Nothing,
                                         Optional ByVal ContainterID As String = Nothing,
                                         Optional ByVal CommercialInvoice As String = Nothing,
                                         Optional ByVal ModalityType As WSReferences.ModalityTypes = 0,
                                         Optional ByVal CustomSection As WSReferences.CustomSections = 0,
                                         Optional ByVal OperationType As WSReferences.OperationTypes = 0,
                                         Optional ByVal DisplayData As WSReferences.TypesDisplay = 0) As XMLSchemaObject

            Dim enumerator As IEnumerator = Nothing

            Dim enumerator1 As IEnumerator = Nothing

            Dim now As DateTime

            _xmlSchemaObject = New XMLSchemaObject

            Dim TagWatcher_ As New TagWatcher

            With TagWatcher_

                .Errors = TagWatcher.ErrorTypes.WS000

                .Status = TagWatcher.TypeStatus.Ok

                .ResultsCount = 0

            End With

            Dim clausulasAdicionales_ As String = Nothing

            If (ReferenceNumber IsNot Nothing) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and t_NumeroReferencia = '", ReferenceNumber, "'")

            End If

            If (CustomsDocument IsNot Nothing) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_NumeroPedimentoCompleto like '%", CustomsDocument, "%'")

            End If

            Select Case DisplayData

                Case WSReferences.TypesDisplay.NotDelivered

                    clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_Despachada = 0")
                    Exit Select

                Case WSReferences.TypesDisplay.Delivered

                    clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_Despachada = 1")
                    Exit Select

            End Select

            If (CustomerName_ IsNot Nothing) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and t_NombreCliente = '", CustomerName_, "'")

            End If

            If (VesselName_ IsNot Nothing) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and t_DescripcionNaveBuque = '", VesselName_, "'")

            End If

            If (ModalityType <> WSReferences.ModalityTypes.UND) Then

                Select Case ModalityType
                    Case WSReferences.ModalityTypes.SEA
                        clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_AduanaSeccion not in (430,510,810,420,160) ")
                        Exit Select

                    Case WSReferences.ModalityTypes.AIR
                        clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_AduanaSeccion in (470,650)")
                        Exit Select

                    Case WSReferences.ModalityTypes.ROA
                        clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_AduanaSeccion in (200,240,800) ", Conversions.ToString(CInt(CustomSection)))
                        Exit Select

                End Select

            End If

            If (CustomSection <> WSReferences.CustomSections.UND) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_AduanaSeccion = ", Conversions.ToString(CInt(CustomSection)))

            End If

            If (CustomsDocument IsNot Nothing) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_NumeroPedimento like '%", CustomsDocument, "%'")

            End If

            If (OperationType <> WSReferences.OperationTypes.UND) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_TipoOperacion = ", Conversions.ToString(CInt(OperationType)))

            End If

            If (RFCOperationsClient IsNot Nothing) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and contains (t_RFCExterno, '", RFCOperationsClient, ")' ")

            End If

            Select Case schemaType_

                Case WSReferences.SchemaTypes.OnlyHeaders

                    _iOperations = _system.EnsamblaModulo("OperacionesAgenciasAduanales")

                    Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

                    With temporalWorkSpace_

                        .ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

                    End With

                    _iOperations.EspacioTrabajo = temporalWorkSpace_

                    _iOperations.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                    _iOperations.CantidadVisibleRegistros = 1000

                    _iOperations.ClausulasLibres = _scriptOfRules

                    _iOperations.ClausulasLibres = String.Concat(_iOperations.ClausulasLibres, clausulasAdicionales_)

                    _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                    _iOperations.GenerarVista("%", Nothing, "like")

                    If (Not _system.TieneResultados(_iOperations)) Then

                        TagWatcher_.Errors = TagWatcher.ErrorTypes.WS006

                        TagWatcher_.Status = TagWatcher.TypeStatus.Errors

                        TagWatcher_.ResultsCount = 0

                    Else

                        Try

                            enumerator = _iOperations.Vista.Tables(0).Rows.GetEnumerator()

                            While enumerator.MoveNext()

                                Dim row_ As DataRow = DirectCast(enumerator.Current, DataRow)

                                Dim referencia_ As Reference = New Reference

                                With referencia_

                                    .CustomsDeclararionKey = Conversions.ToString(row_("t_Cve_Pedimento"))

                                    .CustomsDeclarationDocumentID = Conversions.ToString(row_("i_NumeroPedimentoCompleto"))

                                    .CustomSection = Conversions.ToString(row_("i_AduanaSeccion"))

                                    .DateLastStatus = DateAndTime.Now

                                    .Delivered = Conversions.ToString(row_("i_Despachada"))

                                    .ID = Conversions.ToString(row_("t_NumeroReferencia"))

                                    .ManagementCompanyId = Conversions.ToString(row_("i_Cve_DivisionMiEmpresa"))

                                End With

                                Dim item As Object = row_("i_AduanaSeccion")

                                If (Conversions.ToBoolean(If(Conversions.ToBoolean(Operators.CompareObjectEqual(item, "430", False)) OrElse Conversions.ToBoolean(Operators.CompareObjectEqual(item, "510", False)) OrElse Conversions.ToBoolean(Operators.CompareObjectEqual(item, "810", False)) OrElse Conversions.ToBoolean(Operators.CompareObjectEqual(item, "160", False)) OrElse Conversions.ToBoolean(Operators.CompareObjectEqual(item, "420", False)), True, False))) Then

                                    referencia_.ModalityType = Conversions.ToString(1)

                                ElseIf (Conversions.ToBoolean(If(Conversions.ToBoolean(Operators.CompareObjectEqual(item, "470", False)) OrElse Conversions.ToBoolean(Operators.CompareObjectEqual(item, "650", False)), True, False))) Then

                                    referencia_.ModalityType = Conversions.ToString(2)

                                ElseIf (Conversions.ToBoolean(If(Conversions.ToBoolean(Operators.CompareObjectEqual(item, "200", False)) OrElse Conversions.ToBoolean(Operators.CompareObjectEqual(item, "240", False)) OrElse Conversions.ToBoolean(Operators.CompareObjectEqual(item, "800", False)), True, False))) Then

                                    referencia_.ModalityType = Conversions.ToString(3)

                                End If

                                Dim obj As Object = row_("i_TipoOperacion")

                                If (Operators.ConditionalCompareObjectEqual(obj, "1", False)) Then

                                    referencia_.OperationType = Conversions.ToString(1)

                                ElseIf (Not Operators.ConditionalCompareObjectEqual(obj, "2", False)) Then

                                    referencia_.OperationType = Conversions.ToString(0)

                                Else

                                    referencia_.OperationType = Conversions.ToString(2)

                                End If

                                referencia_.OperationYear = Conversions.ToString(row_("i_Anio"))

                                If (Not Operators.ConditionalCompareObjectEqual(row_("i_Despachada"), "1", False)) Then

                                    referencia_.StatusID = Conversions.ToString(69)

                                    referencia_.StatusTag = WSReferences.ReferenceStatusTypes.DOC.ToString()

                                Else

                                    referencia_.StatusID = Conversions.ToString(49)

                                    referencia_.StatusTag = WSReferences.ReferenceStatusTypes.DSP.ToString()

                                End If

                                If (DBNull.Value.Equals(RuntimeHelpers.GetObjectValue(row_("t_DescripcionNaveBuque")))) Then

                                    referencia_.VesselName = Nothing

                                Else

                                    referencia_.VesselName = Conversions.ToString(row_("t_DescripcionNaveBuque"))

                                End If

                                _xmlSchemaObject.References.Add(referencia_)

                            End While

                        Finally

                            If (TypeOf enumerator Is IDisposable) Then

                                TryCast(enumerator, IDisposable).Dispose()

                            End If

                        End Try

                    End If

                    _xmlSchemaObject.TagWatcher = TagWatcher_

                    Exit Select

                Case WSReferences.SchemaTypes.Advanced1

                    Dim str() As String = {"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;  DECLARE @Tipo int  DECLARE @UsuarioExterno int  DECLARE @NRegistros int  DECLARE @NRegNoConcluidos int   DECLARE @NRegConcluidos int     SELECT top (1) @tipo=i_cve_tipoacceso, @UsuarioExterno=i_cve_cliente_externo FROM Reg021AccesoUsuariosClavesCliente  where t_Usuario='", MobileUserID, "'    select @NRegistros = case @UsuarioExterno WHEN 1 THEN 2000 WHEN 2 THEN 500 ELSE 1000 END;     Select @NRegNoConcluidos=count(Reference.t_NumeroReferencia)  from Vin003OperacionesAgenciasAduanales as Reference with (nolock)  INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones  where (Reference.i_cve_Estatus=1) and (not Reference.f_FechaUltimoEstatus is null) and Reference.f_FechaAltaOperacion>=convert(datetime,'2016-08-01',120)   and (OpMaster.i_TipoReferencia=1)  and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)>=convert(date,DATEADD(day, -2, getdate()),120))                    or (Reference.f_FechaLiquidacionFactura is null)))     or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)>=convert(date,DATEADD(day, -2, getdate()),120)) or             (Reference.f_FechaDespacho is null))))  and Reference.i_Cve_Estado =1 ", _scriptOfRules, " ", clausulasAdicionales_, "   Select @NRegConcluidos = Case WHEN @NRegNoConcluidos>=@NRegistros THEN 0 Else @NRegistros-@NRegNoConcluidos END;    Select Reference.t_CantidadBultos as PackagesAndPallets, Reference.f_FechaEstimadaArribo as EstimatedTimeArrive,CONVERT(VARCHAR(12),Reference.f_FechaAltaOperacion,106) as RefereceDate, CASE WHEN i_AduanaSeccion in (430,160,810,510,420) THEN Reference.t_DescripcionNaveBuque ELSE 'N/A' END AS VesselName, Reference.t_NombreCliente as CustomerName,RTRIM(LTRIM(Reference.t_NumeroReferencia)) as ID,  CASE WHEN not (i_cve_modalidad is null or i_cve_modalidad='') then i_cve_modalidad  WHEN i_AduanaSeccion in (430,160,810,510,420) THEN '1'  WHEN i_AduanaSeccion in (470,650) THEN '2'   WHEN i_AduanaSeccion in (200,800,240) THEN '3'   END AS ModalityType,    Reference.i_TipoOperacion as OperationType,   CASE WHEN i_AduanaSeccion in (430) THEN '1'  WHEN i_AduanaSeccion in (420) THEN '1'  WHEN i_AduanaSeccion in (160) THEN '8'  WHEN i_AduanaSeccion in (810) THEN '6'  WHEN i_AduanaSeccion in (510) THEN '9'  WHEN i_AduanaSeccion in (800) THEN '113'  WHEN i_AduanaSeccion in (240) THEN '113'  WHEN i_AduanaSeccion in (470) THEN '3'  WHEN i_AduanaSeccion in (650) THEN '7'  WHEN i_AduanaSeccion in (200) THEN '3'  END AS ManagementCompanyId,  Reference.i_Cve_TrackingReciente as StatusID, Reference.t_Cve_TrackingReciente as StatusTag, case when not Reference.f_FechaUltimoEstatus is null then convert(datetime,Reference.f_FechaUltimoEstatus,126)  else convert(datetime,GETDATE(), 126) end as DateLastStatus,  Reference.i_AduanaSeccion as  CustomSection, Reference.i_Despachada as Delivered, case @tipo   when 1 then CAST(f_FechaDespacho AS DATETIME)   when 2 then CAST(f_FechaLiquidacionFactura AS DATETIME)  else null end as HistoricDate,  Reference.i_NumeroPedimentoCompleto as CustomsDeclarationDocumentID, Reference.t_Cve_Pedimento as CustomsDeclararionKey, Reference.i_Anio as OperationYear, Reference.t_booking as Booking,  ( Select BillOfLading.t_NumeroGuia as IDGUID, BillOfLading.i_TipoGuia as TypeBL from Rep003RegGuiasInternacionales as BillOfLading with (nolock)  where BillOfLading.i_cve_estado=1 and  Reference.i_Cve_VinOperacionesAgenciasAduanales = BillOfLading.i_Cve_VinOperacionesAgenciasAduanales FOR XML AUTO, TYPE ) as BillOfLadings,  ( Select CommercialInvoice.t_NumeroFactura as InvoiceNumber, CommercialInvoice.f_FechaFactura as InvoiceDate, t_Proveedor as SupplierName, '0' as TaxID,  ( Select InvoceItem.t_NumeroItem as PartNumber, InvoceItem.t_DescripcionMercancia  as Description,t_OrdenCompra as PurchaseOrder 		 from Rep003DetFacturasComerciales as InvoceItem with (nolock)          where InvoceItem.i_cve_estado=1 and CommercialInvoice.i_Cve_VinOperacionesAgenciasAduanales = InvoceItem.i_Cve_VinOperacionesAgenciasAduanales 		       and CommercialInvoice.i_Cve_FacturaComercial =  InvoceItem.i_Cve_FacturaComercial FOR XML AUTO, TYPE ) as InvoiceItems     from Rep003EncFacturasComerciales as CommercialInvoice with (nolock)          where CommercialInvoice.i_cve_estado=1 and Reference.i_Cve_VinOperacionesAgenciasAduanales = CommercialInvoice.i_Cve_VinOperacionesAgenciasAduanales FOR XML AUTO, TYPE ) as CommercialInvoices,  ( Select Container.t_NumeroContenedor as MarksOrNumbers,Container.i_Cve_idContenedorTipo as ContainerType,'' as ContainterSize from Rep003RegContenedores as Container with (nolock)  where Container.i_cve_estado=1 and Reference.i_Cve_VinOperacionesAgenciasAduanales = Container.i_Cve_VinOperacionesAgenciasAduanales FOR XML AUTO, TYPE ) as Containers  from Vin003OperacionesAgenciasAduanales as Reference with (nolock)  INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones    where Reference.t_NumeroReferencia in (  Select top(@NRegistros)  RTRIM(LTRIM(Reference.t_NumeroReferencia))   from Vin003OperacionesAgenciasAduanales as Reference with (nolock)  INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones  where (Reference.i_cve_Estatus=1) and (not Reference.f_FechaUltimoEstatus is null) and Reference.f_FechaAltaOperacion>=convert(datetime,'2016-08-01',120)   and (OpMaster.i_TipoReferencia=1) and (t_Cve_Pedimento<>'R1' or t_Cve_Pedimento is null)  and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)>=convert(date,DATEADD(day, -2, getdate()),120))                    or (Reference.f_FechaLiquidacionFactura is null)))     or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)>=convert(date,DATEADD(day, -2, getdate()),120)) or             (Reference.f_FechaDespacho is null))))  and Reference.i_Cve_Estado =1 ", _scriptOfRules, " ", clausulasAdicionales_, " ORDER by Reference.f_FechaUltimoEstatus desc )   or Reference.t_NumeroReferencia in (    Select top(@NRegConcluidos)  RTRIM(LTRIM(Reference.t_NumeroReferencia))   from Vin003OperacionesAgenciasAduanales as Reference with (nolock)  INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones  where (Reference.i_cve_Estatus=1) and (not Reference.f_FechaUltimoEstatus is null) and Reference.f_FechaAltaOperacion>=convert(datetime,'2016-10-15',120)   and (OpMaster.i_TipoReferencia=1) and (t_Cve_Pedimento<>'R1' or t_Cve_Pedimento is null)  and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)<convert(date,DATEADD(day, -2, getdate()),120))                    or (not Reference.f_FechaLiquidacionFactura is null)))     or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)<convert(date,DATEADD(day, -2, getdate()),120)) and             (not Reference.f_FechaDespacho is null))))  and Reference.i_Cve_Estado =1 ", _scriptOfRules, " ", clausulasAdicionales_, " ORDER by Reference.f_FechaUltimoEstatus desc )     ORDER by DateLastStatus desc  FOR XML AUTO, ROOT ('References');"}

                    Dim _query As String = String.Concat(str)

                    Dim conexion_ As IConexiones = New Conexiones()

                    With conexion_

                        .ControladorBaseDatos = IConexiones.Controladores.SQLServer2008

                        ''PRODUCCIÓN
                        '.NombreBaseDatos = "solium"
                        '.Usuario = "krombase"
                        '.Contrasena = "K1s45gri"
                        '.IpServidor = "10.66.1.5"


                        ''DESARROLLO
                        '.NombreBaseDatos = "solium"
                        '.Contrasena = "S0l1umF0rW"
                        '.Usuario = "sa"
                        '.IpServidor = "10.66.2.102"


                        'NUBE
                        .IpServidor = "10.66.100.5"
                        .NombreBaseDatos = "solium"
                        .Usuario = "sa"
                        .Contrasena = "S0l1umF0rW"

                        .ObjetoDatos = IConexiones.TipoConexion.SqlCommand

                    End With

                    conexion_.DataSetReciente.Tables.Clear()

                    If (conexion_.EjecutaConsultaIndividual(_query)) Then

                        If (conexion_.DataSetReciente IsNot Nothing) Then

                            If (conexion_.DataSetReciente.Tables IsNot Nothing) Then

                                If (conexion_.DataSetReciente.Tables.Count > 0) Then

                                    Dim objectXML_ As StringBuilder = New StringBuilder()

                                    objectXML_.Append("<?xml version=""1.0""?><GetAdvancedInformationSchemaInKromBaseResult xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  <SetXMLSchemaType>Advanced1</SetXMLSchemaType><TagWatcher Errors=""WS000"" ResultsCount=""0""> <Status>Ok</Status> <ErrorDescription></ErrorDescription> </TagWatcher> ")

                                    Try

                                        enumerator1 = conexion_.DataSetReciente.Tables(0).Rows.GetEnumerator()

                                        While enumerator1.MoveNext()

                                            Dim registro_ As DataRow = DirectCast(enumerator1.Current, DataRow)

                                            objectXML_.Append(RuntimeHelpers.GetObjectValue(registro_(0)))

                                        End While

                                    Finally

                                        If (TypeOf enumerator1 Is IDisposable) Then

                                            TryCast(enumerator1, IDisposable).Dispose()

                                        End If

                                    End Try

                                    objectXML_.Append("</GetAdvancedInformationSchemaInKromBaseResult>")

                                    Dim serializer As XmlSerializer = New XmlSerializer(GetType(XMLSchemaObject), New XmlRootAttribute("GetAdvancedInformationSchemaInKromBaseResult"))

                                    Dim xmlSchemaObjectDeserializated_ As XMLSchemaObject = Nothing

                                    Dim string_reader As StringReader = New StringReader(objectXML_.ToString())

                                    xmlSchemaObjectDeserializated_ = DirectCast(serializer.Deserialize(string_reader), XMLSchemaObject)

                                    _xmlSchemaObject.References = xmlSchemaObjectDeserializated_.References

                                    If (ReferenceNumber Is Nothing) Then

                                        str = New String() {"INSERT INTO bit021HistoricoAccesoWCF (t_usuario,t_TipoAcceso,f_FechaRegistro) VALUES ('", MobileUserID, "',1,'", Nothing, Nothing}

                                        now = DateAndTime.Now

                                        str(3) = now.ToString("yyyy-dd-MM hh:mm:ss.f")

                                        str(4) = "') "

                                        conexion_.EjecutaSentenciaIndividual(String.Concat(str))

                                    ElseIf (_xmlSchemaObject.References IsNot Nothing) Then

                                        If (_xmlSchemaObject.References.Count >= 1) Then

                                            _xmlSchemaObject.References(0).StatusCollection = GetStatusList(_xmlSchemaObject.References(0))

                                            str = New String() {"INSERT INTO bit021HistoricoAccesoWCF (t_usuario,t_TipoAcceso,t_Referencia,f_FechaRegistro) VALUES ('", MobileUserID, "',2,'", ReferenceNumber, "','", Nothing, Nothing}

                                            now = DateAndTime.Now

                                            str(5) = now.ToString("yyyy-dd-MM hh:mm:ss.f")

                                            str(6) = "') "

                                            conexion_.EjecutaSentenciaIndividual(String.Concat(str))

                                        End If

                                    End If

                                    _xmlSchemaObject.TagWatcher = xmlSchemaObjectDeserializated_.TagWatcher

                                End If

                            End If

                        End If

                    End If

                    Exit Select

            End Select

            _xmlSchemaObject.SetXMLSchemaType = schemaType_

            Return _xmlSchemaObject

        End Function

        Private Function ReturnXMLSchema2(ByVal schemaType_ As WSReferences.SchemaTypes,
                                         ByVal MobileUserID As String,
                                         Optional ByVal RFCOperationsClient As String = Nothing,
                                         Optional ByVal CustomerName_ As String = Nothing,
                                         Optional ByVal SupplierName_ As String = Nothing,
                                         Optional ByVal VesselName_ As String = Nothing,
                                         Optional ByVal ReferenceNumber As String = Nothing,
                                         Optional ByVal CustomsDocument As String = Nothing,
                                         Optional ByVal ContainterID As String = Nothing,
                                         Optional ByVal CommercialInvoice As String = Nothing,
                                         Optional ByVal ModalityType As WSReferences.ModalityTypes = 0,
                                         Optional ByVal CustomSection As WSReferences.CustomSections = 0,
                                         Optional ByVal OperationType As WSReferences.OperationTypes = 0,
                                         Optional ByVal DisplayData As WSReferences.TypesDisplay = 0) As XMLSchemaObjectPrueba

            Dim enumerator As IEnumerator = Nothing

            Dim enumerator1 As IEnumerator = Nothing

            Dim now As DateTime

            _xmlSchemaObject2 = New XMLSchemaObjectPrueba

            Dim TagWatcher_ As New TagWatcher

            With TagWatcher_

                .Errors = TagWatcher.ErrorTypes.WS000

                .Status = TagWatcher.TypeStatus.Ok

                .ResultsCount = 0

            End With

            Dim clausulasAdicionales_ As String = Nothing

            If (ReferenceNumber IsNot Nothing) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and t_NumeroReferencia = '", ReferenceNumber, "'")

            End If

            If (CustomsDocument IsNot Nothing) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_NumeroPedimentoCompleto like '%", CustomsDocument, "%'")

            End If

            Select Case DisplayData

                Case WSReferences.TypesDisplay.NotDelivered

                    clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_Despachada = 0")
                    Exit Select

                Case WSReferences.TypesDisplay.Delivered

                    clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_Despachada = 1")
                    Exit Select

            End Select

            If (CustomerName_ IsNot Nothing) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and t_NombreCliente = '", CustomerName_, "'")

            End If

            If (VesselName_ IsNot Nothing) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and t_DescripcionNaveBuque = '", VesselName_, "'")

            End If

            If (ModalityType <> WSReferences.ModalityTypes.UND) Then

                Select Case ModalityType
                    Case WSReferences.ModalityTypes.SEA
                        clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_AduanaSeccion not in (430,510,810,420,160) ")
                        Exit Select

                    Case WSReferences.ModalityTypes.AIR
                        clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_AduanaSeccion in (470,650)")
                        Exit Select

                    Case WSReferences.ModalityTypes.ROA
                        clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_AduanaSeccion in (200,240,800) ", Conversions.ToString(CInt(CustomSection)))
                        Exit Select

                End Select

            End If

            If (CustomSection <> WSReferences.CustomSections.UND) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_AduanaSeccion = ", Conversions.ToString(CInt(CustomSection)))

            End If

            If (CustomsDocument IsNot Nothing) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_NumeroPedimento like '%", CustomsDocument, "%'")

            End If

            If (OperationType <> WSReferences.OperationTypes.UND) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and i_TipoOperacion = ", Conversions.ToString(CInt(OperationType)))

            End If

            If (RFCOperationsClient IsNot Nothing) Then

                clausulasAdicionales_ = String.Concat(clausulasAdicionales_, " and contains (t_RFCExterno, '", RFCOperationsClient, ")' ")

            End If

            Select Case schemaType_

                Case WSReferences.SchemaTypes.OnlyHeaders

                Case WSReferences.SchemaTypes.Advanced1

                    Dim str() As String = {"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;  DECLARE @Tipo int  DECLARE @UsuarioExterno int  DECLARE @NRegistros int  DECLARE @NRegNoConcluidos int   DECLARE @NRegConcluidos int     SELECT top (1) @tipo=i_cve_tipoacceso, @UsuarioExterno=i_cve_cliente_externo FROM Reg021AccesoUsuariosClavesCliente  where t_Usuario='", MobileUserID, "'    select @NRegistros = case @UsuarioExterno WHEN 1 THEN 2000 WHEN 2 THEN 500 ELSE 5 END;     Select @NRegNoConcluidos=count(Reference.t_NumeroReferencia)  from Vin003OperacionesAgenciasAduanales as Reference with (nolock)  INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones  where (Reference.i_cve_Estatus=1) and (not Reference.f_FechaUltimoEstatus is null) and Reference.f_FechaAltaOperacion>=convert(datetime,'2016-08-01',120)   and (OpMaster.i_TipoReferencia=1)  and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)>=convert(date,DATEADD(day, -2, getdate()),120))                    or (Reference.f_FechaLiquidacionFactura is null)))     or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)>=convert(date,DATEADD(day, -2, getdate()),120)) or             (Reference.f_FechaDespacho is null))))  and Reference.i_Cve_Estado =1 ", _scriptOfRules, " ", clausulasAdicionales_, "   Select @NRegConcluidos = Case WHEN @NRegNoConcluidos>=@NRegistros THEN 0 Else @NRegistros-@NRegNoConcluidos END;    Select  CASE WHEN i_AduanaSeccion in (430,160,810,510,420) THEN Reference.t_DescripcionNaveBuque  ELSE 'N/A'   END AS VesselName, Reference.t_CantidadBultos as PackagesAndPallets,Reference.f_FechaEstimadaArribo as EstimatedTimeArrive,CONVERT(VARCHAR(12),Reference.f_FechaAltaOperacion,106) as RefereceDate,Reference.t_NombreCliente as CustomerName,RTRIM(LTRIM(Reference.t_NumeroReferencia)) as ID,  CASE WHEN not (i_cve_modalidad is null or i_cve_modalidad='') then i_cve_modalidad  WHEN i_AduanaSeccion in (430,160,810,510,420) THEN '1'  WHEN i_AduanaSeccion in (470,650) THEN '2'   WHEN i_AduanaSeccion in (200,800,240) THEN '3'   END AS ModalityType,    Reference.i_TipoOperacion as OperationType,   CASE WHEN i_AduanaSeccion in (430) THEN '1'  WHEN i_AduanaSeccion in (420) THEN '1' WHEN i_AduanaSeccion in (421) THEN '1'  WHEN i_AduanaSeccion in (160) THEN '8'  WHEN i_AduanaSeccion in (810) THEN '6'  WHEN i_AduanaSeccion in (510) THEN '9'  WHEN i_AduanaSeccion in (800) THEN '113'  WHEN i_AduanaSeccion in (240) THEN '113'  WHEN i_AduanaSeccion in (470) THEN '3'  WHEN i_AduanaSeccion in (650) THEN '7'  WHEN i_AduanaSeccion in (200) THEN '3'  END AS ManagementCompanyId,  Reference.i_Cve_TrackingReciente as StatusID, Reference.t_Cve_TrackingReciente as StatusTag, case when not Reference.f_FechaUltimoEstatus is null then convert(datetime,Reference.f_FechaUltimoEstatus,126)  else convert(datetime,GETDATE(), 126) end as DateLastStatus,  Reference.i_AduanaSeccion as  CustomSection, Reference.i_Despachada as Delivered, case @tipo   when 1 then CAST(f_FechaDespacho AS DATETIME)   when 2 then CAST(f_FechaLiquidacionFactura AS DATETIME)  else null end as HistoricDate,  Reference.i_NumeroPedimentoCompleto as CustomsDeclarationDocumentID, Reference.t_Cve_Pedimento as CustomsDeclararionKey, Reference.i_Anio as OperationYear, Reference.t_booking as Booking,  ( Select BillOfLading.t_NumeroGuia as IDGUID, BillOfLading.i_TipoGuia as TypeBL from Rep003RegGuiasInternacionales as BillOfLading with (nolock)  where BillOfLading.i_cve_estado=1 and  Reference.i_Cve_VinOperacionesAgenciasAduanales = BillOfLading.i_Cve_VinOperacionesAgenciasAduanales FOR XML AUTO, TYPE ) as BillOfLadings,  ( Select CommercialInvoice.t_NumeroFactura as InvoiceNumber, CommercialInvoice.f_FechaFactura as InvoiceDate, t_Proveedor as SupplierName, '0' as TaxID,  ( Select InvoceItem.t_NumeroItem as PartNumber, InvoceItem.t_DescripcionMercancia  as Description,t_OrdenCompra as PurchaseOrder 		 from Rep003DetFacturasComerciales as InvoceItem with (nolock)          where InvoceItem.i_cve_estado=1 and CommercialInvoice.i_Cve_VinOperacionesAgenciasAduanales = InvoceItem.i_Cve_VinOperacionesAgenciasAduanales 		       and CommercialInvoice.i_Cve_FacturaComercial =  InvoceItem.i_Cve_FacturaComercial FOR XML AUTO, TYPE ) as InvoiceItems     from Rep003EncFacturasComerciales as CommercialInvoice with (nolock)          where CommercialInvoice.i_cve_estado=1 and Reference.i_Cve_VinOperacionesAgenciasAduanales = CommercialInvoice.i_Cve_VinOperacionesAgenciasAduanales FOR XML AUTO, TYPE ) as CommercialInvoices,  ( Select Container.t_NumeroContenedor as MarksOrNumbers,Container.i_Cve_idContenedorTipo as ContainerType,'' as ContainterSize from Rep003RegContenedores as Container with (nolock)  where Container.i_cve_estado=1 and Reference.i_Cve_VinOperacionesAgenciasAduanales = Container.i_Cve_VinOperacionesAgenciasAduanales FOR XML AUTO, TYPE ) as Containers  from Vin003OperacionesAgenciasAduanales as Reference with (nolock)  INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones    where Reference.t_NumeroReferencia in (  Select top(@NRegistros)  RTRIM(LTRIM(Reference.t_NumeroReferencia))   from Vin003OperacionesAgenciasAduanales as Reference with (nolock)  INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones  where (Reference.i_cve_Estatus=1) and (not Reference.f_FechaUltimoEstatus is null) and Reference.f_FechaAltaOperacion>=convert(datetime,'2016-08-01',120)   and (OpMaster.i_TipoReferencia=1) and (t_Cve_Pedimento<>'R1' or t_Cve_Pedimento is null)  and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)>=convert(date,DATEADD(day, -2, getdate()),120))                    or (Reference.f_FechaLiquidacionFactura is null)))     or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)>=convert(date,DATEADD(day, -2, getdate()),120)) or             (Reference.f_FechaDespacho is null))))  and Reference.i_Cve_Estado =1 ", _scriptOfRules, " ", clausulasAdicionales_, " ORDER by Reference.f_FechaUltimoEstatus desc )   or Reference.t_NumeroReferencia in (    Select top(@NRegConcluidos)  RTRIM(LTRIM(Reference.t_NumeroReferencia))   from Vin003OperacionesAgenciasAduanales as Reference with (nolock)  INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones  where (Reference.i_cve_Estatus=1) and (not Reference.f_FechaUltimoEstatus is null) and Reference.f_FechaAltaOperacion>=convert(datetime,'2016-10-15',120)   and (OpMaster.i_TipoReferencia=1) and (t_Cve_Pedimento<>'R1' or t_Cve_Pedimento is null)  and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)<convert(date,DATEADD(day, -2, getdate()),120))                    or (not Reference.f_FechaLiquidacionFactura is null)))     or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)<convert(date,DATEADD(day, -2, getdate()),120)) and             (not Reference.f_FechaDespacho is null))))  and Reference.i_Cve_Estado =1 ", _scriptOfRules, " ", clausulasAdicionales_, " ORDER by Reference.f_FechaUltimoEstatus desc )     ORDER by DateLastStatus desc  FOR XML AUTO, ROOT ('References');"}

                    Dim _query As String = String.Concat(str)

                    Dim conexion_ As IConexiones = New Conexiones()

                    With conexion_

                        .ControladorBaseDatos = IConexiones.Controladores.SQLServer2008

                        'PRODUCCIÓN
                        '.NombreBaseDatos = "solium"
                        '.Contrasena = "K1s45gri"
                        '.Usuario = "krombase"
                        '.IpServidor = "10.66.1.5"


                        'DESARROLLO
                        .NombreBaseDatos = "solium"
                        .Contrasena = "S0l1umF0rW"
                        .Usuario = "sa"
                        .IpServidor = "10.66.2.102"

                        .ObjetoDatos = IConexiones.TipoConexion.SqlCommand

                    End With

                    conexion_.DataSetReciente.Tables.Clear()

                    If (conexion_.EjecutaConsultaIndividual(_query)) Then

                        If (conexion_.DataSetReciente IsNot Nothing) Then

                            If (conexion_.DataSetReciente.Tables IsNot Nothing) Then

                                If (conexion_.DataSetReciente.Tables.Count > 0) Then

                                    Dim objectXML_ As StringBuilder = New StringBuilder()

                                    objectXML_.Append("<?xml version=""1.0""?><GetAdvancedInformationSchemaInKromBaseResult xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  <SetXMLSchemaType>Advanced1</SetXMLSchemaType><TagWatcher Errors=""WS000"" ResultsCount=""0""> <Status>Ok</Status> <ErrorDescription></ErrorDescription> </TagWatcher> ")

                                    Try

                                        enumerator1 = conexion_.DataSetReciente.Tables(0).Rows.GetEnumerator()

                                        While enumerator1.MoveNext()

                                            Dim registro_ As DataRow = DirectCast(enumerator1.Current, DataRow)

                                            objectXML_.Append(RuntimeHelpers.GetObjectValue(registro_(0)))

                                        End While

                                    Finally

                                        If (TypeOf enumerator1 Is IDisposable) Then

                                            TryCast(enumerator1, IDisposable).Dispose()

                                        End If

                                    End Try

                                    objectXML_.Append("</GetAdvancedInformationSchemaInKromBaseResult>")

                                    Dim serializer As XmlSerializer = New XmlSerializer(GetType(XMLSchemaObjectPrueba), New XmlRootAttribute("GetAdvancedInformationSchemaInKromBaseResult"))

                                    Dim xmlSchemaObjectDeserializated_ As XMLSchemaObjectPrueba = Nothing

                                    Dim string_reader As StringReader = New StringReader(objectXML_.ToString())

                                    xmlSchemaObjectDeserializated_ = DirectCast(serializer.Deserialize(string_reader), XMLSchemaObjectPrueba)

                                    _xmlSchemaObject2.References = xmlSchemaObjectDeserializated_.References

                                    If (ReferenceNumber Is Nothing) Then

                                        str = New String() {"INSERT INTO bit021HistoricoAccesoWCF (t_usuario,t_TipoAcceso,f_FechaRegistro) VALUES ('", MobileUserID, "',1,'", Nothing, Nothing}

                                        now = DateAndTime.Now

                                        str(3) = now.ToString("yyyy-dd-MM hh:mm:ss.f")

                                        str(4) = "') "

                                        conexion_.EjecutaSentenciaIndividual(String.Concat(str))

                                    ElseIf (_xmlSchemaObject2.References IsNot Nothing) Then

                                        If (_xmlSchemaObject2.References.Count >= 1) Then

                                            _xmlSchemaObject2.References(0).StatusCollection = GetStatusList(_xmlSchemaObject2.References(0))

                                            str = New String() {"INSERT INTO bit021HistoricoAccesoWCF (t_usuario,t_TipoAcceso,t_Referencia,f_FechaRegistro) VALUES ('", MobileUserID, "',2,'", ReferenceNumber, "','", Nothing, Nothing}

                                            now = DateAndTime.Now

                                            str(5) = now.ToString("yyyy-dd-MM hh:mm:ss.f")

                                            str(6) = "') "

                                            conexion_.EjecutaSentenciaIndividual(String.Concat(str))

                                        End If

                                    End If

                                    '_xmlSchemaObject2.TagWatcher = xmlSchemaObjectDeserializated_.TagWatcher

                                End If

                            End If

                        End If

                    End If

                    Exit Select

            End Select

            _xmlSchemaObject2.SetXMLSchemaType = schemaType_

            Return _xmlSchemaObject2

        End Function

        'FRAMEWORK

        '   Private Function ReturnXMLSchema(ByVal schemaType_ As SchemaTypes, _
        '                                    ByVal MobileUserID As String, _
        '                                    Optional ByVal RFCOperationsClient As String = Nothing, _
        '_
        '                                    Optional ByVal CustomerName_ As String = Nothing, _
        '                                    Optional ByVal SupplierName_ As String = Nothing, _
        '_
        '                                    Optional ByVal VesselName_ As String = Nothing, _
        '_
        '                                    Optional ByVal ReferenceNumber As String = Nothing, _
        '                                    Optional ByVal CustomsDocument As String = Nothing, _
        '                                    Optional ByVal ContainterID As String = Nothing, _
        '                                    Optional ByVal CommercialInvoice As String = Nothing, _
        '_
        '                                    Optional ByVal ModalityType As WSReferences.ModalityTypes = WSReferences.ModalityTypes.UND, _
        '                                    Optional ByVal CustomSection As WSReferences.CustomSections = WSReferences.CustomSections.UND, _
        '                                    Optional ByVal OperationType As WSReferences.OperationTypes = WSReferences.OperationTypes.UND, _
        '                                    Optional ByVal DisplayData As WSReferences.TypesDisplay = WSReferences.TypesDisplay.ShowAll
        '                                    ) As XMLSchemaObject

        '       _xmlSchemaObject = New XMLSchemaObject

        '       Dim TagWatcher_ As New TagWatcher

        '       TagWatcher_.Errors = ErrorTypes.WS000

        '       TagWatcher_.Status = TypeStatus.Ok

        '       TagWatcher_.ResultsCount = 0

        '       Dim clausulasAdicionales_ As String = Nothing

        '       'If Not LastUpdate Is Nothing Then
        '       '    Dim FechaLastUpdate As DateTime = Nothing

        '       '    If Date.TryParse(LastUpdate, FechaLastUpdate) Then
        '       '        clausulasAdicionales_ = clausulasAdicionales_ & " and convert(datetime,Reference.f_FechaAltaOperacion,120)>=convert(datetime,'" + FechaLastUpdate.ToString("yyyy-MM-dd HH:mm:ss") + "',120)"
        '       '    End If

        '       'End If


        '       If Not ReferenceNumber Is Nothing Then
        '           clausulasAdicionales_ = clausulasAdicionales_ & " and t_NumeroReferencia = '" & ReferenceNumber & "'"
        '       End If

        '       If Not CustomsDocument Is Nothing Then
        '           clausulasAdicionales_ = clausulasAdicionales_ & " and i_NumeroPedimentoCompleto like '%" & CustomsDocument & "%'"
        '       End If

        '       Select Case DisplayData

        '           Case TypesDisplay.Delivered

        '               clausulasAdicionales_ = clausulasAdicionales_ & " and i_Despachada = 1"

        '           Case TypesDisplay.NotDelivered

        '               clausulasAdicionales_ = clausulasAdicionales_ & " and i_Despachada = 0"

        '       End Select

        '       If Not CustomerName_ Is Nothing Then
        '           clausulasAdicionales_ = clausulasAdicionales_ & " and t_NombreCliente = '" & CustomerName_ & "'"
        '       End If

        '       If Not VesselName_ Is Nothing Then
        '           clausulasAdicionales_ = clausulasAdicionales_ & " and t_DescripcionNaveBuque = '" & VesselName_ & "'"
        '       End If

        '       If Not ModalityType = ModalityTypes.UND Then
        '           Select Case ModalityType

        '               Case ModalityTypes.AIR

        '                   clausulasAdicionales_ = clausulasAdicionales_ & " and i_AduanaSeccion in (470,650)"

        '               Case ModalityTypes.SEA

        '                   clausulasAdicionales_ = clausulasAdicionales_ & " and i_AduanaSeccion not in (430,510,810,420,160) "  '(470,650,200)

        '               Case ModalityTypes.ROA

        '                   clausulasAdicionales_ = clausulasAdicionales_ & " and i_AduanaSeccion in (200,240,800) " & CustomSection   '(200)

        '           End Select

        '       End If

        '       If Not CustomSection = CustomSections.UND Then
        '           clausulasAdicionales_ = clausulasAdicionales_ & " and i_AduanaSeccion = " & CustomSection
        '       End If

        '       If Not CustomsDocument Is Nothing Then
        '           clausulasAdicionales_ = clausulasAdicionales_ & " and i_NumeroPedimento like '%" & CustomsDocument & "%'"
        '       End If

        '       If Not OperationType = OperationTypes.UND Then
        '           clausulasAdicionales_ = clausulasAdicionales_ & " and i_TipoOperacion = " & OperationType
        '       End If

        '       If Not RFCOperationsClient Is Nothing Then
        '           'clausulasAdicionales_ = clausulasAdicionales_ & " and t_RFCExterno = '" & RFCOperationsClient & "'"
        '           clausulasAdicionales_ = clausulasAdicionales_ & " and contains (t_RFCExterno, '" & RFCOperationsClient & ")' "
        '       End If

        '       Select Case schemaType_

        '           Case SchemaTypes.OnlyHeaders

        '               _iOperations = _system.EnsamblaModulo("OperacionesAgenciasAduanales")

        '               Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

        '               temporalWorkSpace_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

        '               _iOperations.EspacioTrabajo = temporalWorkSpace_

        '               _iOperations.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

        '               _iOperations.CantidadVisibleRegistros = 1000

        '               _iOperations.ClausulasLibres = _scriptOfRules

        '               _iOperations.ClausulasLibres = _iOperations.ClausulasLibres & _
        '                   clausulasAdicionales_

        '               _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

        '               _iOperations.GenerarVista()

        '               If _system.TieneResultados(_iOperations) Then

        '                   For Each row_ As DataRow In _iOperations.Vista.Tables(0).Rows

        '                       Dim referencia_ As New Reference

        '                       referencia_.CustomsDeclararionKey = row_("t_Cve_Pedimento")

        '                       referencia_.CustomsDeclarationDocumentID = row_("i_NumeroPedimentoCompleto")

        '                       referencia_.CustomSection = row_("i_AduanaSeccion")

        '                       referencia_.DateLastStatus = Now

        '                       referencia_.Delivered = row_("i_Despachada")

        '                       referencia_.ID = row_("t_NumeroReferencia")

        '                       referencia_.ManagementCompanyId = row_("i_Cve_DivisionMiEmpresa")

        '                       Select Case row_("i_AduanaSeccion")

        '                           Case "430", "510", "810", "160", "420"
        '                               referencia_.ModalityType = WSReferences.ModalityTypes.SEA

        '                           Case "470", "650"
        '                               referencia_.ModalityType = WSReferences.ModalityTypes.AIR
        '                           Case "200", "240", "800"
        '                               referencia_.ModalityType = WSReferences.ModalityTypes.ROA

        '                       End Select

        '                       Select Case row_("i_TipoOperacion")

        '                           Case "1" : referencia_.OperationType = WSReferences.OperationTypes.IMP
        '                           Case "2" : referencia_.OperationType = WSReferences.OperationTypes.EXP
        '                           Case Else
        '                               referencia_.OperationType = WSReferences.OperationTypes.UND
        '                       End Select

        '                       referencia_.OperationYear = row_("i_Anio")

        '                       If row_("i_Despachada") = "1" Then

        '                           referencia_.StatusID = ReferenceStatusTypes.DSP
        '                           referencia_.StatusTag = ReferenceStatusTypes.DSP.ToString

        '                       Else

        '                           referencia_.StatusID = ReferenceStatusTypes.DOC
        '                           referencia_.StatusTag = ReferenceStatusTypes.DOC.ToString

        '                       End If

        '                       If Not DBNull.Value.Equals(row_("t_DescripcionNaveBuque")) Then

        '                           referencia_.VesselName = row_("t_DescripcionNaveBuque")

        '                       Else

        '                           referencia_.VesselName = Nothing

        '                       End If


        '                       _xmlSchemaObject.References.Add(referencia_)

        '                   Next

        '               Else

        '                   TagWatcher_.Errors = ErrorTypes.WS006

        '                   TagWatcher_.Status = TypeStatus.Errors

        '                   TagWatcher_.ResultsCount = 0

        '               End If

        '               _xmlSchemaObject.TagWatcher = TagWatcher_

        '           Case SchemaTypes.Advanced1

        '               Dim respuesta_ As Boolean = False

        '               Dim _query As String = _
        '           "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " & _
        '           " DECLARE @Tipo int " & _
        '           " DECLARE @UsuarioExterno int " & _
        '           " DECLARE @NRegistros int " & _
        '           " DECLARE @NRegNoConcluidos int  " & _
        '           " DECLARE @NRegConcluidos int  " & _
        '           " " & _
        '           " " & _
        '           " SELECT top (1) @tipo=i_cve_tipoacceso, @UsuarioExterno=i_cve_cliente_externo FROM Reg021AccesoUsuariosClavesCliente " & _
        '           " where t_Usuario='" & MobileUserID & "' " & _
        '           " " & _
        '           " " & _
        '           " select @NRegistros = case @UsuarioExterno WHEN 1 THEN 2000 WHEN 2 THEN 500 ELSE 5 END;  " & _
        '           " " & _
        '           " " & _
        '           " Select @NRegNoConcluidos=count(Reference.t_NumeroReferencia) " & _
        '           " from Vin003OperacionesAgenciasAduanales as Reference with (nolock) " & _
        '           " INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones " & _
        '           " where (Reference.i_cve_Estatus=1) and (not Reference.f_FechaUltimoEstatus is null) and Reference.f_FechaAltaOperacion>=convert(datetime,'2016-08-01',120)  " & _
        '           " and (OpMaster.i_TipoReferencia=1) " & _
        '           " and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)>=convert(date,DATEADD(day, -2, getdate()),120)) " & _
        '           "                   or (Reference.f_FechaLiquidacionFactura is null)))  " & _
        '           "   or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)>=convert(date,DATEADD(day, -2, getdate()),120)) or  " & _
        '           "           (Reference.f_FechaDespacho is null)))) " & _
        '           " and Reference.i_Cve_Estado =1 " & _scriptOfRules & " " & clausulasAdicionales_ & _
        '           " " & _
        '           " " & _
        '           " Select @NRegConcluidos = Case WHEN @NRegNoConcluidos>=@NRegistros THEN 0 Else @NRegistros-@NRegNoConcluidos END; " & _
        '           " " & _
        '           " " & _
        '           " Select " & _
        '           " CASE WHEN i_AduanaSeccion in (430,160,810,510,420) THEN Reference.t_DescripcionNaveBuque " & _
        '                 " ELSE 'N/A'  " & _
        '           " END AS VesselName, " & _
        '           "Reference.t_CantidadBultos as PackagesAndPallets," & _
        '           "Reference.f_FechaEstimadaArribo as EstimatedTimeArrive," & _
        '           "CONVERT(VARCHAR(12),Reference.f_FechaAltaOperacion,106) as RefereceDate," & _
        '           "Reference.t_NombreCliente as CustomerName," & _
        '           "RTRIM(LTRIM(Reference.t_NumeroReferencia)) as ID," & _
        '           " " & _
        '            " CASE WHEN not (i_cve_modalidad is null or i_cve_modalidad='') then i_cve_modalidad " & _
        '                 " WHEN i_AduanaSeccion in (430,160,810,510,420) THEN '1' " & _
        '                 " WHEN i_AduanaSeccion in (470,650) THEN '2'  " & _
        '                 " WHEN i_AduanaSeccion in (200,800,240) THEN '3'  " & _
        '           " END AS ModalityType, " & _
        '           "  " & _
        '           " Reference.i_TipoOperacion as OperationType, " & _
        '           " " & _
        '           " CASE WHEN i_AduanaSeccion in (430) THEN '1' " & _
        '                " WHEN i_AduanaSeccion in (420) THEN '1' " & _
        '                " WHEN i_AduanaSeccion in (160) THEN '8' " & _
        '                " WHEN i_AduanaSeccion in (810) THEN '6' " & _
        '                " WHEN i_AduanaSeccion in (510) THEN '9' " & _
        '                " WHEN i_AduanaSeccion in (800) THEN '113' " & _
        '                " WHEN i_AduanaSeccion in (240) THEN '113' " & _
        '                " WHEN i_AduanaSeccion in (470) THEN '3' " & _
        '                " WHEN i_AduanaSeccion in (650) THEN '7' " & _
        '                " WHEN i_AduanaSeccion in (200) THEN '3' " & _
        '          " END AS ManagementCompanyId," & _
        '          " " & _
        '          " Reference.i_Cve_TrackingReciente as StatusID," & _
        '          " Reference.t_Cve_TrackingReciente as StatusTag," & _
        '          " case when not Reference.f_FechaUltimoEstatus is null then convert(datetime,Reference.f_FechaUltimoEstatus,126) " & _
        '          " else convert(datetime,GETDATE(), 126) end as DateLastStatus, " & _
        '          " Reference.i_AduanaSeccion as  CustomSection," & _
        '          " Reference.i_Despachada as Delivered," & _
        '          " case @tipo  " & _
        '          " when 1 then CAST(f_FechaDespacho AS DATETIME)  " & _
        '          " when 2 then CAST(f_FechaLiquidacionFactura AS DATETIME) " & _
        '          " else null end as HistoricDate, " & _
        '          " Reference.i_NumeroPedimentoCompleto as CustomsDeclarationDocumentID," & _
        '          " Reference.t_Cve_Pedimento as CustomsDeclararionKey," & _
        '          " Reference.i_Anio as OperationYear," & _
        '          " Reference.t_booking as Booking," & _
        '           " " & _
        '           " ( Select BillOfLading.t_NumeroGuia as IDGUID, BillOfLading.i_TipoGuia as TypeBL" & _
        '           " from Rep003RegGuiasInternacionales as BillOfLading with (nolock) " & _
        '           " where BillOfLading.i_cve_estado=1 and  Reference.i_Cve_VinOperacionesAgenciasAduanales = BillOfLading.i_Cve_VinOperacionesAgenciasAduanales" & _
        '           " FOR XML AUTO, TYPE" & _
        '           " ) as BillOfLadings," & _
        '           " " & _
        '           " ( Select CommercialInvoice.t_NumeroFactura as InvoiceNumber, CommercialInvoice.f_FechaFactura as InvoiceDate, t_Proveedor as SupplierName, '0' as TaxID," & _
        '           " " & _
        '           " ( Select InvoceItem.t_NumeroItem as PartNumber, InvoceItem.t_DescripcionMercancia  as Description,t_OrdenCompra as PurchaseOrder" & _
        '           " 		 from Rep003DetFacturasComerciales as InvoceItem with (nolock) " & _
        '           "         where InvoceItem.i_cve_estado=1 and CommercialInvoice.i_Cve_VinOperacionesAgenciasAduanales = InvoceItem.i_Cve_VinOperacionesAgenciasAduanales" & _
        '           " 		       and CommercialInvoice.i_Cve_FacturaComercial =  InvoceItem.i_Cve_FacturaComercial" & _
        '                   " FOR XML AUTO, TYPE" & _
        '           " ) as InvoiceItems" & _
        '           " " & _
        '           "    from Rep003EncFacturasComerciales as CommercialInvoice with (nolock) " & _
        '           "         where CommercialInvoice.i_cve_estado=1 and Reference.i_Cve_VinOperacionesAgenciasAduanales = CommercialInvoice.i_Cve_VinOperacionesAgenciasAduanales" & _
        '            " FOR XML AUTO, TYPE" & _
        '           " ) as CommercialInvoices," & _
        '           " " & _
        '           " ( Select Container.t_NumeroContenedor as MarksOrNumbers,Container.i_Cve_idContenedorTipo as ContainerType,'' as ContainterSize" & _
        '           " from Rep003RegContenedores as Container with (nolock) " & _
        '               " where Container.i_cve_estado=1 and Reference.i_Cve_VinOperacionesAgenciasAduanales = Container.i_Cve_VinOperacionesAgenciasAduanales" & _
        '            " FOR XML AUTO, TYPE" & _
        '           " ) as Containers" & _
        '           " " & _
        '           " from Vin003OperacionesAgenciasAduanales as Reference with (nolock) " & _
        '           " INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones " & _
        '           " " & _
        '           " " & _
        '           " where Reference.t_NumeroReferencia in ( " & _
        '           " Select top(@NRegistros) " & _
        '           " RTRIM(LTRIM(Reference.t_NumeroReferencia)) " & _
        '           " " & _
        '           " from Vin003OperacionesAgenciasAduanales as Reference with (nolock) " & _
        '           " INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones " & _
        '           " where (Reference.i_cve_Estatus=1) and (not Reference.f_FechaUltimoEstatus is null) and Reference.f_FechaAltaOperacion>=convert(datetime,'2016-08-01',120)  " & _
        '           " and (OpMaster.i_TipoReferencia=1) and (t_Cve_Pedimento<>'R1' or t_Cve_Pedimento is null) " & _
        '           " and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)>=convert(date,DATEADD(day, -2, getdate()),120)) " & _
        '           "                   or (Reference.f_FechaLiquidacionFactura is null)))  " & _
        '           "   or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)>=convert(date,DATEADD(day, -2, getdate()),120)) or  " & _
        '           "           (Reference.f_FechaDespacho is null)))) " & _
        '           " and Reference.i_Cve_Estado =1 " & _scriptOfRules & " " & clausulasAdicionales_ & _
        '           " ORDER by Reference.f_FechaUltimoEstatus desc )  " & _
        '           " or Reference.t_NumeroReferencia in ( " & _
        '           " " & _
        '           " " & _
        '           " Select top(@NRegConcluidos) " & _
        '           " RTRIM(LTRIM(Reference.t_NumeroReferencia)) " & _
        '           " " & _
        '           " from Vin003OperacionesAgenciasAduanales as Reference with (nolock) " & _
        '           " INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones " & _
        '           " where (Reference.i_cve_Estatus=1) and (not Reference.f_FechaUltimoEstatus is null) and Reference.f_FechaAltaOperacion>=convert(datetime,'2016-10-15',120)  " & _
        '           " and (OpMaster.i_TipoReferencia=1) and (t_Cve_Pedimento<>'R1' or t_Cve_Pedimento is null) " & _
        '           " and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)<convert(date,DATEADD(day, -2, getdate()),120)) " & _
        '           "                   or (not Reference.f_FechaLiquidacionFactura is null)))  " & _
        '           "   or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)<convert(date,DATEADD(day, -2, getdate()),120)) and  " & _
        '           "           (not Reference.f_FechaDespacho is null)))) " & _
        '           " and Reference.i_Cve_Estado =1 " & _scriptOfRules & " " & clausulasAdicionales_ & _
        '           " ORDER by Reference.f_FechaUltimoEstatus desc )  " & _
        '           " " & _
        '           " " & _
        '           " ORDER by DateLastStatus desc " & _
        '           " FOR XML AUTO, ROOT ('References');"


        '               ''''''Estas sentencias son para enviar solo las no concluidas cosiderando un tiempo de 2 dias posteriores''''
        '               '" and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)>=convert(date,DATEADD(day, -2, getdate()),120)) " & _
        '               '"                   or (Reference.f_FechaLiquidacionFactura is null)))  " & _
        '               '"   or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)>=convert(date,DATEADD(day, -2, getdate()),120)) or  " & _
        '               '"           (Reference.f_FechaDespacho is null)))) " & _


        '               '     " when 1 then CAST(DATEADD(day, 2, f_FechaDespacho) AS DATETIME)  " & _
        '               '" when 2 then CAST(DATEADD(day, 2, f_FechaLiquidacionFactura) AS DATETIME) " & _

        '               '" case when not f_FechaRegistroHistorico is null then (CAST(Reference.f_FechaRegistroHistorico AS DATETIME) " & _
        '               '"          + CAST(Reference.h_HoraRegistroHistorico AS DATETIME)) else CAST(Reference.f_FechaDespacho AS DATETIME) end as HistoricDate, " & _

        '               'Reference.t_DescripcionNaveBuque as VesselName," & _
        '               'convert(datetime,Reference.f_FechaAltaOperacion,126)
        '               '" where Reference.f_FechaAltaOperacion>=convert(datetime,'2016-08-01',120) and " & _

        '               '" Reference.f_FechaUltimoEstatus as DateLastStatus," & _
        '               '" where convert(datetime,Reference.f_FechaAltaOperacion,102)>=convert(datetime,'" + LastUpdate.ToString("yyyy-MM-dd HH:mm:ss") + "',102) and" & _

        '               '_system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()
        '               'respuesta_ = _system.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_query)


        '               Dim conexion_ As IConexiones = New Conexiones
        '               conexion_.ControladorBaseDatos = IConexiones.Controladores.SQLServer2008

        '               'conexion_.IpServidor = "10.66.1.5"
        '               'conexion_.NombreBaseDatos = "solium"
        '               'conexion_.Contrasena = "K1s45gri"
        '               'conexion_.Usuario = "krombase"

        '               conexion_.IpServidor = "10.66.2.102"
        '               conexion_.NombreBaseDatos = "solium"
        '               conexion_.Usuario = "sa"
        '               conexion_.Contrasena = "S0l1umF0rW"

        '               conexion_.ObjetoDatos = IConexiones.TipoConexion.SqlCommand
        '               conexion_.DataSetReciente.Tables.Clear()
        '               respuesta_ = conexion_.EjecutaConsultaIndividual(_query)


        '               If respuesta_ Then
        '                   'If Not _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente Is Nothing Then
        '                   '    If Not _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then
        '                   '        If _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count > 0 Then
        '                   If Not conexion_.DataSetReciente Is Nothing Then
        '                       If Not conexion_.DataSetReciente.Tables Is Nothing Then
        '                           If conexion_.DataSetReciente.Tables.Count > 0 Then
        '                               'Dim object_ As String = _
        '                               '    "<?xml version=" & Chr(34) & "1.0" & Chr(34) & "?>" & _
        '                               '    "<GetAdvancedInformationSchemaInKromBaseResult xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & " xmlns:xsd=" & Chr(34) & "http://www.w3.org/2001/XMLSchema" & Chr(34) & ">" & _
        '                               '    "  <SetXMLSchemaType>Advanced1</SetXMLSchemaType>" & _
        '                               '      "<TagWatcher Errors=" & Chr(34) & "WS000" & Chr(34) & " ResultsCount=" & Chr(34) & "0" & Chr(34) & "> " & _
        '                               '          "<Status>Ok</Status> " & _
        '                               '          "<ErrorDescription></ErrorDescription> " & _
        '                               '      "</TagWatcher> "


        '                               'For Each registro_ As DataRow In _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows


        '                               Dim objectXML_ As StringBuilder     'RIGG2
        '                               objectXML_ = New StringBuilder()  'RIGG2

        '                               objectXML_.Append("<?xml version=" & Chr(34) & "1.0" & Chr(34) & "?>" & _
        '                                   "<GetAdvancedInformationSchemaInKromBaseResult xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & " xmlns:xsd=" & Chr(34) & "http://www.w3.org/2001/XMLSchema" & Chr(34) & ">" & _
        '                                   "  <SetXMLSchemaType>Advanced1</SetXMLSchemaType>" & _
        '                                     "<TagWatcher Errors=" & Chr(34) & "WS000" & Chr(34) & " ResultsCount=" & Chr(34) & "0" & Chr(34) & "> " & _
        '                                         "<Status>Ok</Status> " & _
        '                                         "<ErrorDescription></ErrorDescription> " & _
        '                                     "</TagWatcher> ")

        '                               For Each registro_ As DataRow In conexion_.DataSetReciente.Tables(0).Rows
        '                                   'object_ = object_ & registro_(0) 'RIGG2
        '                                   objectXML_.Append(registro_(0))
        '                               Next

        '                               'object_ = object_ & _  'RIGG2
        '                               '    "</GetAdvancedInformationSchemaInKromBaseResult>"  'RIGG2

        '                               objectXML_.Append("</GetAdvancedInformationSchemaInKromBaseResult>")

        '                               Dim serializer As New XmlSerializer(GetType(XMLSchemaObject), New XmlRootAttribute("GetAdvancedInformationSchemaInKromBaseResult"))

        '                               Dim xmlSchemaObjectDeserializated_ As XMLSchemaObject = Nothing

        '                               Dim string_reader As StringReader

        '                               'string_reader = New StringReader(object_) 'RIGG2
        '                               string_reader = New StringReader(objectXML_.ToString)

        '                               xmlSchemaObjectDeserializated_ = DirectCast(serializer.Deserialize(string_reader), XMLSchemaObject)

        '                               _xmlSchemaObject.References = xmlSchemaObjectDeserializated_.References

        '                               If Not ReferenceNumber Is Nothing Then

        '                                   If Not _xmlSchemaObject.References Is Nothing Then
        '                                       If _xmlSchemaObject.References.Count >= 1 Then

        '                                           _xmlSchemaObject.References(0).StatusCollection = GetStatusList(_xmlSchemaObject.References(0))

        '                                           '----------------------------------------------------------------------------------------------
        '                                           'INSERTAR REGISTRO DE HISTORICO DE CONSULTA DE LISTA DE TRACKING
        '                                           '----------------------------------------------------------------------------------------------
        '                                           _query = "INSERT INTO bit021HistoricoAccesoWCF (t_usuario,t_TipoAcceso,t_Referencia,f_FechaRegistro) " & _
        '                                                   "VALUES ('" & MobileUserID & "',2,'" & ReferenceNumber & "','" & Now.ToString("yyyy-dd-MM hh:mm:ss.f") & "') "
        '                                           '"VALUES ('" & MobileUserID & " - " & _iOperations.EspacioTrabajo.MisCredenciales.ClaveUsuario & "',2,'" & Now.ToString("yyyy-MM-dd hh:mm:ss.f") & "') "
        '                                           conexion_.EjecutaSentenciaIndividual(_query)

        '                                       End If
        '                                   End If

        '                               Else
        '                                   '----------------------------------------------------------------------------------------------
        '                                   'INSERTAR REGISTRO DE HISTORICO DE CONSULTA DE LISTA DE TRACKING
        '                                   '----------------------------------------------------------------------------------------------
        '                                   _query = "INSERT INTO bit021HistoricoAccesoWCF (t_usuario,t_TipoAcceso,f_FechaRegistro) " & _
        '                                              "VALUES ('" & MobileUserID & "',1,'" & Now.ToString("yyyy-dd-MM hh:mm:ss.f") & "') "
        '                                   '''"VALUES ('" & MobileUserID & " - " & _iOperations.EspacioTrabajo.MisCredenciales.ClaveUsuario & "',1,'" & Now.ToString("yyyy-MM-dd hh:mm:ss.f") & "') "
        '                                   conexion_.EjecutaSentenciaIndividual(_query)

        '                               End If

        '                               _xmlSchemaObject.TagWatcher = xmlSchemaObjectDeserializated_.TagWatcher

        '                           End If

        '                       End If

        '                   End If
        '               End If
        '           Case SchemaTypes.ReferenceType


        '       End Select

        '       _xmlSchemaObject.SetXMLSchemaType = schemaType_

        '       Return _xmlSchemaObject

        '   End Function

#End Region

    End Class

    <DataContract>
    <XmlSerializerFormat>
    Public Class XMLSchemaObject

#Region "Attributes"

        Private _references As List(Of Reference)

        Private _TagWatcher As TagWatcher

        Private _schemaType As SchemaTypes

#End Region

#Region "Builders"

        Sub New()

            MyBase.New()

            _references = New List(Of Reference)

            _TagWatcher = New TagWatcher

        End Sub

#End Region

#Region "Properties"

        <DataMember>
        Property SetXMLSchemaType As SchemaTypes
            Get
                Return _schemaType
            End Get
            Set(value As SchemaTypes)
                _schemaType = value
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
        Public Property References As List(Of Reference)
            Get
                Return _references
            End Get
            Set(ByVal value As List(Of Reference))
                _references = value
            End Set
        End Property

#End Region

#Region "Methods"

#End Region

    End Class

    <DataContract()>
    <XmlSerializerFormat>
    Public Class DataAccessRules

#Region "Enums"


#End Region

#Region "Attributes"

        Private _i_Cve_AccesoUsuarioClavesCliente As Int32
        Private _i_Cve_DivisionMiEmpresa As Int32
        Private _t_RFC As String
        Private _i_Cve_ClienteEmpresa As Int32
        Private _i_Cve_DivisionEmpresarialCliente As Int32
        Private _i_Cve_Aplicacion As ApplicationTypes
        Private _i_Cve_Accion As AccessTypes
        Private _i_Aduana As CustomSections
        Private _i_Patente As PatentNumbers
        Private _i_TipoOperacion As OperationTypes
        Private _scriptOfRule As String

#End Region

#Region "Builders"

        Sub New()

            _i_Cve_AccesoUsuarioClavesCliente = 0
            _i_Cve_DivisionMiEmpresa = 0
            _t_RFC = Nothing
            _i_Cve_ClienteEmpresa = 0
            _i_Cve_DivisionEmpresarialCliente = 0
            _i_Cve_Aplicacion = ApplicationTypes.Mobile
            _i_Cve_Accion = AccessTypes.Deny
            _i_Aduana = CustomSections.UND
            _i_Patente = PatentNumbers.UND
            _i_TipoOperacion = OperationTypes.UND

        End Sub

#End Region

#Region "Properties"

        <DataMember, XmlAttribute>
        Property IDAccesoUsuarioClavesCliente As Int32
            Get
                Return _i_Cve_AccesoUsuarioClavesCliente
            End Get
            Set(value As Int32)
                _i_Cve_AccesoUsuarioClavesCliente = value
            End Set
        End Property

        <DataMember, XmlAttribute>
        Property Accion As AccessTypes
            Get
                Return _i_Cve_Accion
            End Get
            Set(value As AccessTypes)
                _i_Cve_Accion = value
            End Set
        End Property

        <DataMember, XmlAttribute>
        Property IDDivisionMiEmpresa As Int32
            Get
                Return _i_Cve_DivisionMiEmpresa
            End Get
            Set(value As Int32)
                _i_Cve_DivisionMiEmpresa = value
            End Set
        End Property

        <DataMember>
        Property RFC As String
            Get
                Return _t_RFC
            End Get
            Set(value As String)
                _t_RFC = value
            End Set
        End Property

        <DataMember>
        Property IDClienteEmpresa As Int32
            Get
                Return _i_Cve_ClienteEmpresa
            End Get
            Set(value As Int32)
                _i_Cve_ClienteEmpresa = value
            End Set
        End Property

        <DataMember>
        Property IDDivisionEmpresarialCliente As Int32
            Get
                Return _i_Cve_DivisionEmpresarialCliente
            End Get
            Set(value As Int32)
                _i_Cve_DivisionEmpresarialCliente = value
            End Set
        End Property

        <DataMember>
        Property Aplicacion As ApplicationTypes
            Get
                Return _i_Cve_Aplicacion
            End Get
            Set(value As ApplicationTypes)
                _i_Cve_Aplicacion = value
            End Set
        End Property


        <DataMember>
        Property Aduana As CustomSections
            Get
                Return _i_Aduana
            End Get
            Set(value As CustomSections)
                _i_Aduana = value
            End Set
        End Property

        <DataMember>
        Property Patente As PatentNumbers
            Get
                Return _i_Patente
            End Get
            Set(value As PatentNumbers)
                _i_Patente = value
            End Set
        End Property

        <DataMember>
        Property TipoOperacion As OperationTypes
            Get
                Return _i_TipoOperacion
            End Get
            Set(value As OperationTypes)
                _i_TipoOperacion = value
            End Set
        End Property

#End Region

#Region "Methods"

#End Region

    End Class

    <DataContract()>
    <XmlSerializerFormat>
    Public Class BillOfLading

#Region "Attributes"

        Private _id As String

        Private _typeBL As Int32

#End Region

#Region "Enums"

#End Region

#Region "Builders"

        Sub New()

            _id = Nothing

            _typeBL = 0

        End Sub

#End Region

#Region "Properties"

        <DataMember, XmlAttribute>
        Property IDGUID As String
            Get
                Return _id
            End Get
            Set(value As String)
                _id = value
            End Set

        End Property

        <DataMember, XmlAttribute>
        Property TypeBL As Int32
            Get
                Return _typeBL
            End Get
            Set(value As Int32)
                _typeBL = value
            End Set
        End Property

#End Region

#Region "Methods"


#End Region


    End Class

    <DataContract()>
    <XmlSerializerFormat>
    Public Class CommercialInvoice

#Region "Attributes"

        Private _invoiceNumber As String

        Private _invoiceDate As Date

        Private _supplierName As String

        Private _taxID As String

        Private _invoiceItems As List(Of InvoceItem)

#End Region

#Region "Enums"

#End Region

#Region "Builders"

        Sub New()

            _invoiceNumber = Nothing
            '_invoiceNumber = New StringBuilder()
            '_invoiceNumber.Clear()

            _invoiceDate = Nothing

            _supplierName = Nothing
            '_supplierName = New StringBuilder()
            '_supplierName.Clear()

            _taxID = Nothing
            '_taxID = New StringBuilder()
            '_taxID.Clear()

            _invoiceItems = New List(Of InvoceItem)

        End Sub

#End Region

#Region "Properties"
        <DataMember>
        Property InvoiceItems As List(Of InvoceItem)
            Get
                Return _invoiceItems
            End Get
            Set(value As List(Of InvoceItem))
                _invoiceItems = value
            End Set
        End Property

        'Property InvoiceNumber As String
        <DataMember, XmlAttribute>
        Property InvoiceNumber As String
            Get
                Return _invoiceNumber
            End Get
            Set(value As String)
                _invoiceNumber = value
            End Set
        End Property

        <DataMember, XmlAttribute>
        Property InvoiceDate As Date
            Get
                Return _invoiceDate
            End Get
            Set(value As Date)
                _invoiceDate = value
            End Set
        End Property

        'Property SupplierName As String
        <DataMember, XmlAttribute>
        Property SupplierName As String
            Get
                Return _supplierName
            End Get
            Set(value As String)
                _supplierName = value
            End Set
        End Property

        'Property TaxID As String
        <DataMember, XmlAttribute>
        Property TaxID As String
            Get
                Return _taxID
            End Get
            Set(value As String)
                _taxID = value
            End Set
        End Property

#End Region

#Region "Methods"

#End Region

    End Class

    <DataContract()>
    <XmlSerializerFormat>
    Public Class InvoceItem

#Region "Attributes"

        Private _partNumber As String

        Private _description As String

        Private _purchaseOrder As String

#End Region

#Region "Builders"

        Sub New()

            _partNumber = Nothing
            '_partNumber = New StringBuilder
            '_partNumber.Clear()

            _description = Nothing
            '_description = New StringBuilder
            '_description.Clear()

            _purchaseOrder = Nothing
            '_purchaseOrder = New StringBuilder
            '_purchaseOrder.Clear()

        End Sub

#End Region

#Region "Properties"

        <DataMember, XmlAttribute>
        Property PartNumber As String
            Get
                Return _partNumber
            End Get
            Set(value As String)
                _partNumber = value
            End Set
        End Property

        <DataMember, XmlAttribute>
        Property Description As String
            Get
                Return _description
            End Get
            Set(value As String)
                _description = value
            End Set
        End Property

        <DataMember, XmlAttribute>
        Property PurchaseOrder As String
            Get
                Return _purchaseOrder
            End Get
            Set(value As String)
                _purchaseOrder = value
            End Set
        End Property

#End Region

#Region "Methods"

#End Region

    End Class

    <DataContract()>
    <XmlSerializerFormat>
    Public Class Container

#Region "Attributes"

        Private _marksOrNumber As String

        Private _containerType As Int32

        Private _containterSize As String

#End Region

#Region "Enums"


#End Region

#Region "Builders"

        Sub New()

            _marksOrNumber = Nothing

            _containerType = 0

            _containterSize = Nothing

        End Sub

#End Region

#Region "Properties"

        <DataMember, XmlAttribute>
        Property MarksOrNumbers As String
            Get
                Return _marksOrNumber
            End Get
            Set(value As String)
                _marksOrNumber = value
            End Set
        End Property

        <DataMember, XmlAttribute>
        Property ContainerType As Int32
            Get
                Return _containerType
            End Get
            Set(value As Int32)
                _containerType = value
            End Set
        End Property

#End Region

#Region "Methods"

#End Region


    End Class

End Namespace