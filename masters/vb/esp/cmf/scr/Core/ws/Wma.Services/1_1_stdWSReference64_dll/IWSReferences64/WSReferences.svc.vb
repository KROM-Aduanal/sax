Imports System.ComponentModel
Imports System.Xml.Serialization
Imports Gsol.BaseDatos.Operaciones
Imports Gsol
Imports Gsol.basededatos
Imports System.IO
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.WebServices.WSReferences

Namespace Wma.WebServices

    '<XmlSerializerFormat>
    <DataContract()>
    Public Class WSReferences
        Implements IWSReferences

#Region "Enums"

        <DataContract()>
        Enum TypesDisplay
            <EnumMember> ShowAll = 0
            <EnumMember> NotDelivered = 1
            <EnumMember> Delivered = 2
        End Enum


        <DataContract()>
        Enum ContainerTypes
            <EnumMember> Undefined = 0
            <EnumMember> OpenTop = 1
            <EnumMember> Refrigerated = 2
        End Enum

        <DataContract()>
        Enum ContainerSizes
            <EnumMember> Undefined = 0
            <EnumMember> C20Inches = 20
            <EnumMember> C40Inches = 40
        End Enum

        <DataContract()>
        Enum GuidTypes
            <EnumMember> <Description("Undefined")> UND = 0
            <EnumMember> <Description("Master")> Master = 1
            <EnumMember> <Description("House")> House = 2
        End Enum

        <DataContract()>
        Enum PatentNumbers
            <EnumMember> <Description("Undefined")> UND = 0
            <EnumMember> <Description("Rolando Reyes Kuri ")> RRK = 3210
            <EnumMember> <Description("Luis de la Cruz Reyes")> LDC = 3921
            <EnumMember> <Description("Jesús Gomez Reyes")> JGR = 3945
            <EnumMember> <Description("Sergio Álvarez Ramírez")> SAR = 3931
            <EnumMember> <Description("Marco Antonio Barquín de la Calle")> MBC = 3962
        End Enum

        <DataContract()>
        Enum AccessTypes
            <EnumMember> Access = 1
            <EnumMember> Deny = 2
        End Enum


        <DataContract()>
        Enum ApplicationTypes
            <EnumMember> Undefined = 0
            <EnumMember> Desktop = 4
            <EnumMember> Mobile = 11
            <EnumMember> Web = 12
        End Enum

        <DataContract()>
        Enum SchemaTypes
            <EnumMember> OnlyHeaders
            <EnumMember> ReferenceType
            <EnumMember> Advanced1
        End Enum

        <DataContract()>
        Enum ModalityTypes
            <EnumMember> <Description("Undefined")> UND = 0
            <EnumMember> <Description("Sea")> SEA = 1
            <EnumMember> <Description("Air")> AIR = 2
            <EnumMember> <Description("Road")> ROA = 3
            <EnumMember> <Description("Rail")> RAI = 4
            <EnumMember> <Description("Logística")> MDL = 5
        End Enum


        <DataContract()>
        Enum CustomSections
            <EnumMember> <Description("Undefined")> UND = 0
            <EnumMember> <Description("México ( Áereo )")> DAI = 470
            <EnumMember> <Description("México ( Terrestre )")> PAN = 200
            <EnumMember> <Description("Veracruz ( Marítimo )")> RKU = 430
            <EnumMember> <Description("Lázaro Cárdenas ( Marítimo )")> ALC = 510
            <EnumMember> <Description("Toluca ( Áereo )")> TOL = 650
            <EnumMember> <Description("Altamira ( Marítimo )")> CEG = 810
            <EnumMember> <Description("Nuevo Laredo ( Terrestre )")> SFI = 240
            <EnumMember> <Description("Colombia N.L. ( Terrestres )")> SFC = 800
            <EnumMember> <Description("Tuxpan Veracruz( Marítimo )")> TUX = 420
            <EnumMember> <Description("Manzanillo ( Marítimo )")> SAP = 160
        End Enum


        <DataContract()>
        Enum OperationTypes
            <EnumMember> <Description("Undefined")> UND = 0
            <EnumMember> <Description("Importación")> IMP = 1
            <EnumMember> <Description("Exportación")> EXP = 2
        End Enum




        <DataContract()>
        Enum ReferenceStatusTypes
            <EnumMember> <Description("Sin definir")> UND = 0
            <EnumMember> <Description("Arribo de buque")> ABU = 50
            <EnumMember> <Description("Arribo de la mercancía")> AME = 51
            <EnumMember> <Description("Arribo de tren")> ATR = 52
            <EnumMember> <Description("Arribo dentro de México")> AMX = 53
            <EnumMember> <Description("Aterrizaje")> ATER = 54
            <EnumMember> <Description("Booking")> BOO = 55
            <EnumMember> <Description("Canje de documentos")> CAD = 56
            <EnumMember> <Description("Carga de mercancías")> CAM = 57
            <EnumMember> <Description("Carga en tránsito")> CAT = 58
            <EnumMember> <Description("Cruce de unidad")> CRU = 59
            <EnumMember> <Description("Descarga/entrega en planta")> ENP = 60
            <EnumMember> <Description("Despegue")> DES = 61
            <EnumMember> <Description("Entregado")> ENT = 62
            <EnumMember> <Description("Ingreso a EUA")> IEU = 63
            <EnumMember> <Description("Inicio de ruta")> INR = 64
            <EnumMember> <Description("Llegada a destino")> LLD = 65
            <EnumMember> <Description("Pago de cuenta de gastos")> LFAC = 66
            <EnumMember> <Description("Pago de pedimento")> PAG = 67
            <EnumMember> <Description("Previo")> PRE = 68
            <EnumMember> <Description("Recepción de documentos")> DOC = 69
            <EnumMember> <Description("Recepción en bodega")> RBO = 70
            <EnumMember> <Description("Recolección de guía")> RGU = 71
            <EnumMember> <Description("Recolección/Posición en planta")> PPL = 72
            <EnumMember> <Description("Reconocimiento aduanero")> RAD = 73
            <EnumMember> <Description("Revalidación")> REV = 74
            <EnumMember> <Description("Selección automátizada rojo")> SAR = 75
            <EnumMember> <Description("Transbordo")> TBD = 76
            <EnumMember> <Description("Zarpe de buque")> ZAR = 77
            <EnumMember> <Description("Despacho aduanal")> DSP = 49
            <EnumMember> <Description("Emisión de cuenta de gastos")> FAC = 38
            <EnumMember> <Description("PENDIENTE DE FACTURAR NUEVAMENTE")> PFAC = 41
            <EnumMember> <Description("Alta de la referencia")> ALR = 27
            <EnumMember> <Description("Anticipo")> ANT = 5
        End Enum

#End Region

#Region "Attributes"

        Private _xmlSchemaObject As XMLSchemaObject

        Private _iOperations As IOperacionesCatalogo

        Private _system As New Organismo

        Public Shared _sesion As ISesion

        Private _sessionStatus As Boolean

        Private _dataAccessRulesList As List(Of DataAccessRules)

        Private _scriptOfRules As String

#End Region

#Region "Builders"

        Sub New()

            _xmlSchemaObject = New XMLSchemaObject

            _sesion = New SesionWcf

            _iOperations = New OperacionesCatalogo

            _dataAccessRulesList = New List(Of DataAccessRules)

            _scriptOfRules = Nothing

        End Sub

#End Region

#Region "Properties"

        Public Function GetAdvancedInformationSchemaInKromBase(ByVal MobileUserID As String, _
                                                               ByVal WebServiceUserID As String, _
                                                               ByVal WebServicePasswordID As String, _
 _
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
                                                                     Optional ByVal DisplayData_ As String = Nothing) As XMLSchemaObject Implements IWSReferences.GetAdvancedInformationSchemaInKromBase

            'Optional ByVal ModalityType As WSReferences.ModalityTypes = WSReferences.ModalityTypes.UND, _
            '                                                                     Optional ByVal CustomSection As WSReferences.CustomSections = WSReferences.CustomSections.UND, _
            '                                                                     Optional ByVal OperationType As WSReferences.OperationTypes = WSReferences.OperationTypes.UND, _
            '                                                                     Optional ByVal DisplayData As WSReferences.TypesDisplay = WSReferences.TypesDisplay.ShowAll) As XMLSchemaObject Implements IWSReferences.GetAdvancedInformationSchemaInKromBase

            Dim result_ As New TagWatcher

            Dim xmlSchemaObject_ As New XMLSchemaObject


            Dim ModalityType As WSReferences.ModalityTypes = WSReferences.ModalityTypes.UND
            Dim CustomSection As WSReferences.CustomSections = WSReferences.CustomSections.UND
            Dim OperationType As WSReferences.OperationTypes = WSReferences.OperationTypes.UND
            Dim DisplayData As WSReferences.TypesDisplay = WSReferences.TypesDisplay.ShowAll

            Integer.TryParse(ModalityType_, ModalityType)
            Integer.TryParse(CustomSection_, CustomSection)
            Integer.TryParse(OperationType_, OperationType)
            Integer.TryParse(DisplayData_, DisplayData)

            result_ = LogIn(MobileUserID, WebServiceUserID, WebServicePasswordID, 4, 1, False)

            If result_.Errors = ErrorTypes.WS000 Then

                Dim ContainterID As String = Nothing

                Dim CommercialInvoice As String = Nothing


                xmlSchemaObject_ = ReturnXMLSchema(SchemaTypes.Advanced1, _
                                                   MobileUserID, _
                                                   RFCOperationsClient, _
                                                   CustomerName_, _
                                                   SupplierName_, _
                                                   VesselName_, _
                                                   ReferenceNumber_, _
                                                   CustomsDocument, _
                                                   ContainterID, _
                                                   CommercialInvoice, _
                                                   ModalityType, _
                                                   CustomSection, _
                                                   OperationType, _
                                                   DisplayData)

            Else

                xmlSchemaObject_.TagWatcher = result_

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
                                                                      Optional ByVal ModalityType As String = Nothing, _
                                                                      Optional ByVal CustomSection As String = Nothing, _
                                                                      Optional ByVal OperationType As String = Nothing, _
 _
                                                                      Optional ByVal DisplayData As String = Nothing) As XMLSchemaObject Implements IWSReferences.GetBasicInformationSchemaInKromBase


            '                                                                                 Optional ByVal ModalityType As WSReferences.ModalityTypes = WSReferences.ModalityTypes.UND, _
            '                                                                     Optional ByVal CustomSection As WSReferences.CustomSections = Wma.WebServices.WSReferences.CustomSections.UND, _
            '                                                                     Optional ByVal OperationType As WSReferences.OperationTypes = Wma.WebServices.WSReferences.OperationTypes.UND, _
            '_
            '                                                                     Optional ByVal DisplayData As WSReferences.TypesDisplay = WSReferences.TypesDisplay.ShowAll) As XMLSchemaObject Implements IWSReferences.GetBasicInformationSchemaInKromBase


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

#End Region


#Region "Methods"

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

                    accessRules_ = accessRules_ & " and i_Cve_VinClienteOperacion =" & rule_.IDClienteEmpresa

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

                    accessRules_ = accessRules_ & " and i_Cve_ClienteEmpresa =" & rule_.IDClienteEmpresa

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

            _sesion.MininimoCaracteresUsuario = 7

            _sesion.MinimoNumerosUsuario = 0

            _sesion.MinimoCaracteresContrasena = 7

            _sesion.MinimoMayusculasContrasena = 2

            _sesion.MinimoMinusculasContrasena = 2

            _sesion.MinimoNumerosContrasena = 2

            _sesion.GrupoEmpresarial = 1

            _sesion.DivisionEmpresarial = 1

            _sesion.Aplicacion = 4

            _sesion.IdentificadorUsuario = Trim(user_)

            _sesion.ContraseniaUsuario = Trim(password_)

        End Sub


        Private Function LogIn(ByVal MobileUserID As String, _
                                    ByVal WebServiceUserID As String, _
                                    ByVal WebServicePasswordID As String, _
                                    ByVal IdRequiredApplication As Integer, _
                                    ByVal CorporateNumber As Integer, _
                                    Optional ByVal FullAuthentication As Boolean = False) As TagWatcher

            Dim results_ As New TagWatcher

            _sessionStatus = False

            SessionPrepare(WebServiceUserID, _
                           WebServicePasswordID)

            If _sesion.StatusArgumentos Then

                _sessionStatus = True

                _sesion.GrupoEmpresarial = CorporateNumber

                _sesion.DivisionEmpresarial = 1

                _sesion.Aplicacion = IdRequiredApplication

                _sesion.Idioma = ISesion.Idiomas.Espaniol

                If Not FullAuthentication Then

                    _iOperations = _system.EnsamblaModulo("Usuarios")

                    Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

                    temporalWorkSpace_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

                    _iOperations.EspacioTrabajo = temporalWorkSpace_

                    _iOperations.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                    _iOperations.ClausulasLibres = " and t_usuario = '" & MobileUserID & "'"

                    _iOperations.CantidadVisibleRegistros = 1000

                    _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                    If _iOperations.GenerarVista() = IOperacionesCatalogo.EstadoOperacion.COk Then

                        If _system.TieneResultados(_iOperations) Then

                            _iOperations = _system.EnsamblaModulo("AccesoUsuariosClavesCliente")

                            _iOperations.ClausulasLibres = " and t_Usuario = '" & MobileUserID & "'"

                            _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                            If _iOperations.GenerarVista() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                If _system.TieneResultados(_iOperations) Then

                                    If _iOperations.Vista.Tables(0).Columns.Contains("i_Cve_AccesoUsuarioClavesCliente") Then

                                        For Each row_ As DataRow In _iOperations.Vista.Tables(0).Rows

                                            Dim accessRule_ As New DataAccessRules

                                            accessRule_.IDAccesoUsuarioClavesCliente = row_("i_Cve_AccesoUsuarioClavesCliente")

                                            accessRule_.IDDivisionMiEmpresa = row_("i_Cve_DivisionMiEmpresa")

                                            If Not DBNull.Value.Equals(row_("t_RFC")) Then
                                                accessRule_.RFC = row_("t_RFC")
                                            Else : accessRule_.RFC = Nothing
                                            End If
                                            'accessRule_.RFC = row_("t_RFC")

                                            accessRule_.IDClienteEmpresa = row_("i_Cve_ClienteEmpresa")

                                            accessRule_.IDDivisionEmpresarialCliente = row_("i_Cve_DivisionEmpresarialCliente")

                                            accessRule_.Aplicacion = row_("i_Cve_Aplicacion")

                                            accessRule_.Accion = row_("i_Cve_Accion")

                                            accessRule_.Aduana = row_("i_Aduana")

                                            accessRule_.Patente = row_("i_Patente")

                                            accessRule_.TipoOperacion = row_("i_TipoOperacion")

                                            _dataAccessRulesList.Add(accessRule_)

                                        Next

                                        CovertRuleToScript(results_)
                                    Else
                                        results_.SetError(ErrorTypes.WS007, "4 This mobile user doesn't have client rules to access profile,  although was authenticated")
                                    End If

                                Else

                                    results_.SetError(ErrorTypes.WS007, "3 This mobile user doesn't have client rules to access profile,  although was authenticated")

                                End If

                            Else

                                results_.SetError(ErrorTypes.WS007, "2 This mobile user doesn't have client rules to access profile,  although was authenticated")

                            End If
                        Else
                            results_.SetError(ErrorTypes.WS007, "1 This mobile user doesn't have client rules to access profile,  although was authenticated")
                        End If

                        'Else
                        '    results_.SetError(ErrorTypes.WS007, " This mobile user doesn't have client rules to access profile,  although was authenticated")
                        'End If
                    Else

                        results_.SetError(ErrorTypes.WS001)

                    End If

                Else

                    _sessionStatus = _sesion.StatusArgumentos

                    If _sessionStatus Then

                        If _sesion.StatusArgumentos = True And _
                            _sesion.EspacioTrabajo IsNot Nothing Then

                            _iOperations = New OperacionesCatalogo

                            _iOperations = _system.ConsultaModulo(_sesion.EspacioTrabajo, "Usuarios", " and t_usuario = '" & MobileUserID & "'").Clone

                            If _system.TieneResultados(_iOperations) Then

                                results_.SetOK()

                            Else
                                results_.SetError(ErrorTypes.WS002)

                            End If

                        Else

                            results_.SetError(ErrorTypes.WS003)

                        End If


                    Else

                        results_.SetError(ErrorTypes.WS004)

                    End If

                End If
            Else

                results_.SetError(ErrorTypes.WS005)

            End If

            Return results_

        End Function

        Private Sub AddStatus(ByRef _statusList As List(Of StatusItem), _
                              ByVal _orderNumber As Int32, _
                              ByVal _idStatus As ReferenceStatusTypes, _
                              ByVal _lastDate As String, _
                             Optional ByVal _isDone As Boolean = False)

            'ByVal _lastDate As DateTime, _
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


        Private Function CheckDateStatus(ByRef tStatus As DataTable, ByVal status_ As String) As String

            'Dim resultado_ As Date = Nothing
            Dim resultado_ As String = Nothing

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

            'AddStatus(_statusList, 0, ReferenceStatusTypes.ALR, Now, _alr)

            _query = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " & _
                    "SELECT MP.i_Cve_MaestroOperaciones, MP.t_Referencia, ET.i_Cve_PlantillaTracking, AT.i_Cve_AccionTracking," & _
                        "AT.t_ClaveAccion,convert(varchar,cast(MAX(DT.f_Alta) as datetime) + cast(MAX(DT.H_Alta) as datetime),126) AS Fecha, Vi.i_cve_MedioTransporteArribo " & _
                    "FROM Reg009MaestroOperaciones MP  with (nolock) " & _
                        "INNER JOIN Enc008Tracking ET  with (nolock) ON (MP.i_Cve_MaestroOperaciones=ET.i_Cve_Operacion) " & _
                        "INNER JOIN Det008TrackingDetalles DT  with (nolock) ON (ET.i_Cve_Tracking=DT.i_Cve_EncabezadoTracking) " & _
                        "INNER JOIN Cat008AccionesTracking AT  with (nolock) ON (DT.i_Cve_AccionTracking=AT.i_Cve_AccionTracking) " & _
                        "INNER JOIN Vin003OperacionesAgenciasAduanales Vi  with (nolock) ON (vi.t_NumeroReferencia=MP.t_Referencia) " & _
                        "WHERE Dt.i_cve_estado=1 and ET.i_cve_estado=1 and MP.i_cve_estado=1 and MP.t_Referencia='" & reference_.ID & "' " & _
                    "GROUP BY MP.i_Cve_MaestroOperaciones, MP.t_Referencia, ET.i_Cve_PlantillaTracking, AT.i_Cve_AccionTracking,AT.t_ClaveAccion,Vi.i_cve_MedioTransporteArribo "


            'convert(datetime,GETDATE(), 126)
            '"AT.t_ClaveAccion,MAX(DT.f_Alta) AS Fecha, Vi.i_cve_MedioTransporteArribo " & _

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
            conexion_.Contrasena = "S0l1umF0rW"
            conexion_.ControladorBaseDatos = IConexiones.Controladores.SQLServer2008
            conexion_.Usuario = "sa"
            conexion_.NombreBaseDatos = "solium"
            conexion_.IpServidor = "10.66.1.19"
            conexion_.ObjetoDatos = IConexiones.TipoConexion.SqlCommand
            'conexion_.IniciaConexion()
            'conexion_.EjecutaConsulta("")
            'conexion_.TerminaConexion()
            conexion_.DataSetReciente.Tables.Clear()

            If conexion_.EjecutaConsultaIndividual(_query) Then
                If Not conexion_.DataSetReciente Is Nothing And
                   Not conexion_.DataSetReciente.Tables Is Nothing And conexion_.DataSetReciente.Tables.Count > 0 Then

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
                Case CustomSections.ALC, CustomSections.CEG, CustomSections.RKU, CustomSections.SAP, CustomSections.TUX

                    Select Case _operationType

                        Case OperationTypes.IMP

                            'DOC()
                            AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                            'REV()  
                            AddStatus(_statusList, 2, ReferenceStatusTypes.REV, CheckDateStatus(tablaStatus, ReferenceStatusTypes.REV.ToString), _rev)

                            'PRE()
                            AddStatus(_statusList, 3, ReferenceStatusTypes.PRE, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PRE.ToString), _pre)

                            'PAG()
                            AddStatus(_statusList, 4, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                            'SAR()                            
                            AddStatus(_statusList, 5, ReferenceStatusTypes.SAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SAR.ToString), _sar)

                            'RAD()
                            AddStatus(_statusList, 6, ReferenceStatusTypes.RAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RAD.ToString), _rad)

                            'DSP()
                            AddStatus(_statusList, 7, ReferenceStatusTypes.DSP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DSP.ToString), _dsp)

                            'FAC()
                            'AddStatus(_statusList, 8, ReferenceStatusTypes.FAC, _facDate, _fac)
                            AddStatus(_statusList, 8, ReferenceStatusTypes.FAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.FAC.ToString), _fac)

                            'LFAC()
                            'AddStatus(_statusList, 9, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)
                            AddStatus(_statusList, 9, ReferenceStatusTypes.LFAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LFAC.ToString), _lfac)

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
                Case CustomSections.DAI, CustomSections.TOL

                    Select Case _operationType

                        Case OperationTypes.IMP

                            'DOC()
                            AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                            'REV() Se comenta por no estar en Catalogo
                            'AddStatus(_statusList, 2, ReferenceStatusTypes.REV, CheckDateStatus(tablaStatus, ReferenceStatusTypes.REV.ToString), _rev)

                            'PRE() Se agregar por estar en catalogo
                            AddStatus(_statusList, 2, ReferenceStatusTypes.PRE, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PRE.ToString), _pre)

                            'RGU()
                            AddStatus(_statusList, 3, ReferenceStatusTypes.RGU, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RGU.ToString), _rgu)

                            'PAG()
                            AddStatus(_statusList, 4, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                            'SAR()                            
                            AddStatus(_statusList, 5, ReferenceStatusTypes.SAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SAR.ToString), _sar)

                            'RAD()
                            AddStatus(_statusList, 6, ReferenceStatusTypes.RAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RAD.ToString), _rad)

                            'DSP()
                            AddStatus(_statusList, 7, ReferenceStatusTypes.DSP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DSP.ToString), _dsp)

                            'FAC()
                            'AddStatus(_statusList, 8, ReferenceStatusTypes.FAC, _facDate, _fac)
                            AddStatus(_statusList, 8, ReferenceStatusTypes.FAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.FAC.ToString), _fac)

                            'LFAC()
                            'AddStatus(_statusList, 9, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)
                            AddStatus(_statusList, 9, ReferenceStatusTypes.LFAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LFAC.ToString), _lfac)

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
                                AddStatus(_statusList, 1, ReferenceStatusTypes.ATR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ATR.ToString), _atr)

                                'DOC()
                                AddStatus(_statusList, 2, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                                'PAG()
                                AddStatus(_statusList, 3, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                                'AMX()
                                AddStatus(_statusList, 4, ReferenceStatusTypes.AMX, CheckDateStatus(tablaStatus, ReferenceStatusTypes.AMX.ToString), _amx)

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
                                AddStatus(_statusList, 2, ReferenceStatusTypes.PRE, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PRE.ToString), _doc)

                                'DOC()
                                AddStatus(_statusList, 3, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                                'PAG()
                                AddStatus(_statusList, 4, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                                'CAM()
                                AddStatus(_statusList, 5, ReferenceStatusTypes.CAM, CheckDateStatus(tablaStatus, ReferenceStatusTypes.CAM.ToString), _cam)

                                'CRU()
                                AddStatus(_statusList, 6, ReferenceStatusTypes.CRU, CheckDateStatus(tablaStatus, ReferenceStatusTypes.CRU.ToString), _cru)

                                'SAR()                            
                                AddStatus(_statusList, 7, ReferenceStatusTypes.SAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SAR.ToString), _sar)

                                'RAD()
                                AddStatus(_statusList, 8, ReferenceStatusTypes.RAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RAD.ToString), _rad)

                                'DSP()
                                AddStatus(_statusList, 9, ReferenceStatusTypes.DSP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DSP.ToString), _dsp)

                                'FAC()
                                'AddStatus(_statusList, 12, ReferenceStatusTypes.FAC, _facDate, _fac)
                                AddStatus(_statusList, 10, ReferenceStatusTypes.FAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.FAC.ToString), _fac)

                                'LFAC()
                                'AddStatus(_statusList, 13, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)
                                AddStatus(_statusList, 11, ReferenceStatusTypes.LFAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LFAC.ToString), _lfac)

                            End If

                        Case OperationTypes.EXP

                            'Si el medio de arribo es 6 (Ferroviario) de lo contrario lo considera como carretero
                            If _MedioTransporteArribo = 6 Then

                                'DOC()
                                AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                                'ATR()
                                AddStatus(_statusList, 2, ReferenceStatusTypes.ATR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ATR.ToString), _atr)

                                'PAG()
                                AddStatus(_statusList, 3, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                                'SAR()                            
                                AddStatus(_statusList, 4, ReferenceStatusTypes.SAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SAR.ToString), _sar)

                                'RAD()
                                AddStatus(_statusList, 5, ReferenceStatusTypes.RAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RAD.ToString), _rad)

                                'DSP()
                                AddStatus(_statusList, 6, ReferenceStatusTypes.DSP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DSP.ToString), _dsp)

                                'IEU()
                                AddStatus(_statusList, 7, ReferenceStatusTypes.IEU, CheckDateStatus(tablaStatus, ReferenceStatusTypes.IEU.ToString), _ieu)

                                'FAC()
                                'AddStatus(_statusList, 10, ReferenceStatusTypes.FAC, _facDate, _fac)
                                AddStatus(_statusList, 8, ReferenceStatusTypes.FAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.FAC.ToString), _fac)

                                'LFAC()
                                'AddStatus(_statusList, 11, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)
                                AddStatus(_statusList, 9, ReferenceStatusTypes.LFAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LFAC.ToString), _lfac)

                            Else

                                'DOC()
                                AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DOC.ToString), _doc)

                                'AME
                                AddStatus(_statusList, 2, ReferenceStatusTypes.AME, CheckDateStatus(tablaStatus, ReferenceStatusTypes.AME.ToString), _cam)

                                'PAG()
                                AddStatus(_statusList, 3, ReferenceStatusTypes.PAG, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PAG.ToString), _pag)

                                'CRU()
                                AddStatus(_statusList, 4, ReferenceStatusTypes.CRU, CheckDateStatus(tablaStatus, ReferenceStatusTypes.CRU.ToString), _cru)

                                'SAR()                            
                                AddStatus(_statusList, 5, ReferenceStatusTypes.SAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.SAR.ToString), _sar)

                                'RAD()
                                AddStatus(_statusList, 6, ReferenceStatusTypes.RAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.RAD.ToString), _rad)

                                'DSP()
                                AddStatus(_statusList, 7, ReferenceStatusTypes.DSP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DSP.ToString), _dsp)

                                'IEU()
                                AddStatus(_statusList, 8, ReferenceStatusTypes.IEU, CheckDateStatus(tablaStatus, ReferenceStatusTypes.IEU.ToString), _ieu)

                                'FAC()
                                'AddStatus(_statusList, 10, ReferenceStatusTypes.FAC, _facDate, _fac)
                                AddStatus(_statusList, 9, ReferenceStatusTypes.FAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.FAC.ToString), _fac)

                                'LFAC()
                                'AddStatus(_statusList, 11, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)
                                AddStatus(_statusList, 10, ReferenceStatusTypes.LFAC, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LFAC.ToString), _lfac)

                            End If

                    End Select

                    'Logistica
                Case CustomSections.UND

                    Select Case _operationType

                        Case OperationTypes.IMP, OperationTypes.EXP, OperationTypes.UND

                            'BOO()
                            AddStatus(_statusList, 1, ReferenceStatusTypes.BOO, CheckDateStatus(tablaStatus, ReferenceStatusTypes.BOO.ToString), False)

                            'PPL()
                            AddStatus(_statusList, 2, ReferenceStatusTypes.PPL, CheckDateStatus(tablaStatus, ReferenceStatusTypes.PPL.ToString), False)

                            'ZAR()
                            AddStatus(_statusList, 3, ReferenceStatusTypes.ZAR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ZAR.ToString), False)

                            'INR()
                            AddStatus(_statusList, 4, ReferenceStatusTypes.INR, CheckDateStatus(tablaStatus, ReferenceStatusTypes.INR.ToString), False)

                            'DES()
                            AddStatus(_statusList, 5, ReferenceStatusTypes.DES, CheckDateStatus(tablaStatus, ReferenceStatusTypes.DES.ToString), False)

                            'CAT()
                            AddStatus(_statusList, 6, ReferenceStatusTypes.CAT, CheckDateStatus(tablaStatus, ReferenceStatusTypes.CAT.ToString), False)

                            'TBD()
                            AddStatus(_statusList, 7, ReferenceStatusTypes.TBD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.TBD.ToString), False)

                            'ABU()
                            AddStatus(_statusList, 8, ReferenceStatusTypes.ABU, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ABU.ToString), False)

                            'LLD()
                            AddStatus(_statusList, 9, ReferenceStatusTypes.LLD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.LLD.ToString), False)

                            'ATER()
                            AddStatus(_statusList, 10, ReferenceStatusTypes.ATER, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ATER.ToString), False)

                            'CAD()
                            AddStatus(_statusList, 11, ReferenceStatusTypes.CAD, CheckDateStatus(tablaStatus, ReferenceStatusTypes.CAD.ToString), False)

                            'ENP()
                            AddStatus(_statusList, 12, ReferenceStatusTypes.ENP, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ENP.ToString), False)

                            'ENT()
                            AddStatus(_statusList, 13, ReferenceStatusTypes.ENT, CheckDateStatus(tablaStatus, ReferenceStatusTypes.ENT.ToString), False)

                    End Select

            End Select

            Return _statusList

        End Function


        Private Function ReturnXMLSchema(ByVal schemaType_ As SchemaTypes, _
                                         ByVal MobileUserID As String, _
                                         Optional ByVal RFCOperationsClient As String = Nothing, _
 _
                                         Optional ByVal CustomerName_ As String = Nothing, _
                                         Optional ByVal SupplierName_ As String = Nothing, _
 _
                                         Optional ByVal VesselName_ As String = Nothing, _
 _
                                         Optional ByVal ReferenceNumber As String = Nothing, _
                                         Optional ByVal CustomsDocument As String = Nothing, _
                                         Optional ByVal ContainterID As String = Nothing, _
                                         Optional ByVal CommercialInvoice As String = Nothing, _
 _
                                         Optional ByVal ModalityType As WSReferences.ModalityTypes = WSReferences.ModalityTypes.UND, _
                                         Optional ByVal CustomSection As WSReferences.CustomSections = WSReferences.CustomSections.UND, _
                                         Optional ByVal OperationType As WSReferences.OperationTypes = WSReferences.OperationTypes.UND, _
                                         Optional ByVal DisplayData As WSReferences.TypesDisplay = WSReferences.TypesDisplay.ShowAll
                                         ) As XMLSchemaObject

            _xmlSchemaObject = New XMLSchemaObject

            Dim TagWatcher_ As New TagWatcher

            TagWatcher_.Errors = ErrorTypes.WS000

            TagWatcher_.Status = TypeStatus.Ok

            TagWatcher_.ResultsCount = 0

            Dim clausulasAdicionales_ As String = Nothing

            'If Not LastUpdate Is Nothing Then
            '    Dim FechaLastUpdate As DateTime = Nothing

            '    If Date.TryParse(LastUpdate, FechaLastUpdate) Then
            '        clausulasAdicionales_ = clausulasAdicionales_ & " and convert(datetime,Reference.f_FechaAltaOperacion,120)>=convert(datetime,'" + FechaLastUpdate.ToString("yyyy-MM-dd HH:mm:ss") + "',120)"
            '    End If

            'End If


            If Not ReferenceNumber Is Nothing Then
                clausulasAdicionales_ = clausulasAdicionales_ & " and t_NumeroReferencia = '" & ReferenceNumber & "'"
            End If

            If Not CustomsDocument Is Nothing Then
                clausulasAdicionales_ = clausulasAdicionales_ & " and i_NumeroPedimentoCompleto like '%" & CustomsDocument & "%'"
            End If

            Select Case DisplayData

                Case TypesDisplay.Delivered

                    clausulasAdicionales_ = clausulasAdicionales_ & " and i_Despachada = 1"

                Case TypesDisplay.NotDelivered

                    clausulasAdicionales_ = clausulasAdicionales_ & " and i_Despachada = 0"

            End Select

            If Not CustomerName_ Is Nothing Then
                clausulasAdicionales_ = clausulasAdicionales_ & " and t_NombreCliente = '" & CustomerName_ & "'"
            End If

            If Not VesselName_ Is Nothing Then
                clausulasAdicionales_ = clausulasAdicionales_ & " and t_DescripcionNaveBuque = '" & VesselName_ & "'"
            End If

            If Not ModalityType = ModalityTypes.UND Then
                Select Case ModalityType

                    Case ModalityTypes.AIR

                        clausulasAdicionales_ = clausulasAdicionales_ & " and i_AduanaSeccion in (470,650)"

                    Case ModalityTypes.SEA

                        clausulasAdicionales_ = clausulasAdicionales_ & " and i_AduanaSeccion not in (430,510,810,420,160) "  '(470,650,200)

                    Case ModalityTypes.ROA

                        clausulasAdicionales_ = clausulasAdicionales_ & " and i_AduanaSeccion in (200,240,800) " & CustomSection   '(200)

                End Select

            End If

            If Not CustomSection = CustomSections.UND Then
                clausulasAdicionales_ = clausulasAdicionales_ & " and i_AduanaSeccion = " & CustomSection
            End If

            If Not CustomsDocument Is Nothing Then
                clausulasAdicionales_ = clausulasAdicionales_ & " and i_NumeroPedimento like '%" & CustomsDocument & "%'"
            End If

            If Not OperationType = OperationTypes.UND Then
                clausulasAdicionales_ = clausulasAdicionales_ & " and i_TipoOperacion = " & OperationType
            End If

            If Not RFCOperationsClient Is Nothing Then
                'clausulasAdicionales_ = clausulasAdicionales_ & " and t_RFCExterno = '" & RFCOperationsClient & "'"
                clausulasAdicionales_ = clausulasAdicionales_ & " and contains (t_RFCExterno, '" & RFCOperationsClient & ")' "
            End If

            Select Case schemaType_

                Case SchemaTypes.OnlyHeaders

                    _iOperations = _system.EnsamblaModulo("OperacionesAgenciasAduanales")

                    Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

                    temporalWorkSpace_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

                    _iOperations.EspacioTrabajo = temporalWorkSpace_

                    _iOperations.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                    _iOperations.CantidadVisibleRegistros = 1000

                    _iOperations.ClausulasLibres = _scriptOfRules

                    _iOperations.ClausulasLibres = _iOperations.ClausulasLibres & _
                        clausulasAdicionales_

                    _iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                    _iOperations.GenerarVista()

                    If _system.TieneResultados(_iOperations) Then

                        For Each row_ As DataRow In _iOperations.Vista.Tables(0).Rows

                            Dim referencia_ As New Reference

                            referencia_.CustomsDeclararionKey = row_("t_Cve_Pedimento")

                            referencia_.CustomsDeclarationDocumentID = row_("i_NumeroPedimentoCompleto")

                            referencia_.CustomSection = row_("i_AduanaSeccion")

                            referencia_.DateLastStatus = Now

                            referencia_.Delivered = row_("i_Despachada")

                            referencia_.ID = row_("t_NumeroReferencia")

                            referencia_.ManagementCompanyId = row_("i_Cve_DivisionMiEmpresa")

                            Select Case row_("i_AduanaSeccion")

                                Case "430", "510", "810", "160", "420"
                                    referencia_.ModalityType = WSReferences.ModalityTypes.SEA

                                Case "470", "650"
                                    referencia_.ModalityType = WSReferences.ModalityTypes.AIR
                                Case "200", "240", "800"
                                    referencia_.ModalityType = WSReferences.ModalityTypes.ROA

                            End Select

                            Select Case row_("i_TipoOperacion")

                                Case "1" : referencia_.OperationType = WSReferences.OperationTypes.IMP
                                Case "2" : referencia_.OperationType = WSReferences.OperationTypes.EXP
                                Case Else
                                    referencia_.OperationType = WSReferences.OperationTypes.UND
                            End Select

                            referencia_.OperationYear = row_("i_Anio")

                            If row_("i_Despachada") = "1" Then

                                referencia_.StatusID = ReferenceStatusTypes.DSP
                                referencia_.StatusTag = ReferenceStatusTypes.DSP.ToString

                            Else

                                referencia_.StatusID = ReferenceStatusTypes.DOC
                                referencia_.StatusTag = ReferenceStatusTypes.DOC.ToString

                            End If

                            If Not DBNull.Value.Equals(row_("t_DescripcionNaveBuque")) Then

                                referencia_.VesselName = row_("t_DescripcionNaveBuque")

                            Else

                                referencia_.VesselName = Nothing

                            End If


                            _xmlSchemaObject.References.Add(referencia_)

                        Next

                    Else

                        TagWatcher_.Errors = ErrorTypes.WS006

                        TagWatcher_.Status = TypeStatus.Errors

                        TagWatcher_.ResultsCount = 0

                    End If

                    _xmlSchemaObject.TagWatcher = TagWatcher_

                Case SchemaTypes.Advanced1

                    Dim respuesta_ As Boolean = False

                    Dim _query As String = _
                "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " & _
                " DECLARE @Tipo int " & _
                " DECLARE @UsuarioExterno int " & _
                " DECLARE @NRegistros int " & _
                " DECLARE @NRegNoConcluidos int  " & _
                " DECLARE @NRegConcluidos int  " & _
                " " & _
                " " & _
                " SELECT top (1) @tipo=i_cve_tipoacceso, @UsuarioExterno=i_cve_cliente_externo FROM Reg021AccesoUsuariosClavesCliente " & _
                " where t_Usuario='" & MobileUserID & "' " & _
                " " & _
                " " & _
                " select @NRegistros = case @UsuarioExterno WHEN 1 THEN 2000 ELSE 1000 END;  " & _
                " " & _
                " " & _
                " Select @NRegNoConcluidos=count(Reference.t_NumeroReferencia) " & _
                " from Vin003OperacionesAgenciasAduanales as Reference with (nolock) " & _
                " INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones " & _
                " where  (not Reference.f_FechaUltimoEstatus is null) and Reference.f_FechaAltaOperacion>=convert(datetime,'2016-08-01',120)  " & _
                " and (OpMaster.i_TipoReferencia=1) " & _
                " and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)>=convert(date,DATEADD(day, -2, getdate()),120)) " & _
                "                   or (Reference.f_FechaLiquidacionFactura is null)))  " & _
                "   or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)>=convert(date,DATEADD(day, -2, getdate()),120)) or  " & _
                "           (Reference.f_FechaDespacho is null)))) " & _
                " and Reference.i_Cve_Estado =1 " & _scriptOfRules & " " & clausulasAdicionales_ & _
                " " & _
                " " & _
                " Select @NRegConcluidos = Case WHEN @NRegNoConcluidos>=@NRegistros THEN 0 Else @NRegistros-@NRegNoConcluidos END; " & _
                " " & _
                " " & _
                " Select top(@NRegistros) " & _
                " CASE WHEN i_AduanaSeccion in (430,160,810,510,420) THEN Reference.t_DescripcionNaveBuque " & _
                      " ELSE 'N/A'  " & _
                " END AS VesselName, " & _
                "Reference.t_CantidadBultos as PackagesAndPallets," & _
                "Reference.f_FechaEstimadaArribo as EstimatedTimeArrive," & _
                "CONVERT(VARCHAR(12),Reference.f_FechaAltaOperacion,106) as RefereceDate," & _
                "Reference.t_NombreCliente as CustomerName," & _
                "RTRIM(LTRIM(Reference.t_NumeroReferencia)) as ID," & _
                " " & _
                 " CASE WHEN i_AduanaSeccion in (430,160,810,510,420) THEN '1' " & _
                      " WHEN i_AduanaSeccion in (470,650) THEN '2'  " & _
                      " WHEN i_AduanaSeccion in (200,800,240) THEN '3'  " & _
                " END AS ModalityType, " & _
                "  " & _
                " Reference.i_TipoOperacion as OperationType, " & _
                " " & _
                " CASE WHEN i_AduanaSeccion in (430) THEN '1' " & _
                     " WHEN i_AduanaSeccion in (420) THEN '1' " & _
                     " WHEN i_AduanaSeccion in (160) THEN '8' " & _
                     " WHEN i_AduanaSeccion in (810) THEN '6' " & _
                     " WHEN i_AduanaSeccion in (510) THEN '9' " & _
                     " WHEN i_AduanaSeccion in (800) THEN '113' " & _
                     " WHEN i_AduanaSeccion in (240) THEN '113' " & _
                     " WHEN i_AduanaSeccion in (470) THEN '3' " & _
                     " WHEN i_AduanaSeccion in (650) THEN '7' " & _
                     " WHEN i_AduanaSeccion in (200) THEN '3' " & _
               " END AS ManagementCompanyId," & _
               " " & _
               " Reference.i_Cve_TrackingReciente as StatusID," & _
               " Reference.t_Cve_TrackingReciente as StatusTag," & _
               " case when not Reference.f_FechaUltimoEstatus is null then convert(datetime,Reference.f_FechaUltimoEstatus,126) " & _
               " else convert(datetime,GETDATE(), 126) end as DateLastStatus, " & _
               " Reference.i_AduanaSeccion as  CustomSection," & _
               " Reference.i_Despachada as Delivered," & _
               " case @tipo  " & _
               " when 1 then CAST(f_FechaDespacho AS DATETIME)  " & _
               " when 2 then CAST(f_FechaLiquidacionFactura AS DATETIME) " & _
               " else null end as HistoricDate, " & _
               " Reference.i_NumeroPedimentoCompleto as CustomsDeclarationDocumentID," & _
               " Reference.t_Cve_Pedimento as CustomsDeclararionKey," & _
               " Reference.i_Anio as OperationYear," & _
               " Reference.t_booking as Booking," & _
                " " & _
                " ( Select BillOfLading.t_NumeroGuia as IDGUID, BillOfLading.i_TipoGuia as TypeBL" & _
                " from Rep003RegGuiasInternacionales as BillOfLading with (nolock) " & _
                " where BillOfLading.i_cve_estado=1 and  Reference.i_Cve_VinOperacionesAgenciasAduanales = BillOfLading.i_Cve_VinOperacionesAgenciasAduanales" & _
                " FOR XML Path('BillOfLading'), Type " & _
                " ) as BillOfLadings," & _
                " " & _
                " ( Select CommercialInvoice.t_NumeroFactura as InvoiceNumber, CommercialInvoice.f_FechaFactura as InvoiceDate, t_Proveedor as SupplierName, '0' as TaxID," & _
                " " & _
                " ( Select InvoceItem.t_NumeroItem as PartNumber, InvoceItem.t_DescripcionMercancia  as Description,t_OrdenCompra as PurchaseOrder" & _
                " 		 from Rep003DetFacturasComerciales as InvoceItem with (nolock) " & _
                "         where InvoceItem.i_cve_estado=1 and CommercialInvoice.i_Cve_VinOperacionesAgenciasAduanales = InvoceItem.i_Cve_VinOperacionesAgenciasAduanales" & _
                " 		       and CommercialInvoice.i_Cve_FacturaComercial =  InvoceItem.i_Cve_FacturaComercial" & _
                " FOR XML Path('InvoceItem'), Type " & _
                " ) as InvoiceItems" & _
                " " & _
                "    from Rep003EncFacturasComerciales as CommercialInvoice with (nolock) " & _
                "         where CommercialInvoice.i_cve_estado=1 and Reference.i_Cve_VinOperacionesAgenciasAduanales = CommercialInvoice.i_Cve_VinOperacionesAgenciasAduanales" & _
                " FOR XML Path('CommercialInvoice'), Type " & _
                " ) as CommercialInvoices," & _
                " " & _
                " ( Select Container.t_NumeroContenedor as MarksOrNumbers,Container.i_Cve_idContenedorTipo as ContainerType,'' as ContainterSize" & _
                " from Rep003RegContenedores as Container with (nolock) " & _
                    " where Container.i_cve_estado=1 and Reference.i_Cve_VinOperacionesAgenciasAduanales = Container.i_Cve_VinOperacionesAgenciasAduanales" & _
                " FOR XML Path('Container'), Type " & _
                " ) as Containers" & _
                " " & _
                " from Vin003OperacionesAgenciasAduanales as Reference with (nolock) " & _
                " INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones " & _
                " where  (not Reference.f_FechaUltimoEstatus is null) and Reference.f_FechaAltaOperacion>=convert(datetime,'2016-08-01',120)  " & _
                " and (OpMaster.i_TipoReferencia=1) " & _
                " and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)>=convert(date,DATEADD(day, -2, getdate()),120)) " & _
                "                   or (Reference.f_FechaLiquidacionFactura is null)))  " & _
                "   or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)>=convert(date,DATEADD(day, -2, getdate()),120)) or  " & _
                "           (Reference.f_FechaDespacho is null)))) " & _
                " and Reference.i_Cve_Estado =1 " & _scriptOfRules & " " & clausulasAdicionales_ & _
                " " & _
                " " & _
                " union all  " & _
                " " & _
                " " & _
                " Select top(@NRegConcluidos) " & _
                " CASE WHEN i_AduanaSeccion in (430,160,810,510,420) THEN Reference.t_DescripcionNaveBuque " & _
                      " ELSE 'N/A'  " & _
                " END AS VesselName, " & _
                "Reference.t_CantidadBultos as PackagesAndPallets," & _
                "Reference.f_FechaEstimadaArribo as EstimatedTimeArrive," & _
                "CONVERT(VARCHAR(12),Reference.f_FechaAltaOperacion,106) as RefereceDate," & _
                "Reference.t_NombreCliente as CustomerName," & _
                "RTRIM(LTRIM(Reference.t_NumeroReferencia)) as ID," & _
                " " & _
                 " CASE WHEN i_AduanaSeccion in (430,160,810,510,420) THEN '1' " & _
                      " WHEN i_AduanaSeccion in (470,650) THEN '2'  " & _
                      " WHEN i_AduanaSeccion in (200,800,240) THEN '3'  " & _
                " END AS ModalityType, " & _
                "  " & _
                " Reference.i_TipoOperacion as OperationType, " & _
                " " & _
                " CASE WHEN i_AduanaSeccion in (430) THEN '1' " & _
                     " WHEN i_AduanaSeccion in (420) THEN '1' " & _
                     " WHEN i_AduanaSeccion in (160) THEN '8' " & _
                     " WHEN i_AduanaSeccion in (810) THEN '6' " & _
                     " WHEN i_AduanaSeccion in (510) THEN '9' " & _
                     " WHEN i_AduanaSeccion in (800) THEN '113' " & _
                     " WHEN i_AduanaSeccion in (240) THEN '113' " & _
                     " WHEN i_AduanaSeccion in (470) THEN '3' " & _
                     " WHEN i_AduanaSeccion in (650) THEN '7' " & _
                     " WHEN i_AduanaSeccion in (200) THEN '3' " & _
               " END AS ManagementCompanyId," & _
               " " & _
               " Reference.i_Cve_TrackingReciente as StatusID," & _
               " Reference.t_Cve_TrackingReciente as StatusTag," & _
               " case when not Reference.f_FechaUltimoEstatus is null then convert(datetime,Reference.f_FechaUltimoEstatus,126) " & _
               " else convert(datetime,GETDATE(), 126) end as DateLastStatus, " & _
               " Reference.i_AduanaSeccion as  CustomSection," & _
               " Reference.i_Despachada as Delivered," & _
               " case @tipo  " & _
               " when 1 then CAST(f_FechaDespacho AS DATETIME)  " & _
               " when 2 then CAST(f_FechaLiquidacionFactura AS DATETIME) " & _
               " else null end as HistoricDate, " & _
               " Reference.i_NumeroPedimentoCompleto as CustomsDeclarationDocumentID," & _
               " Reference.t_Cve_Pedimento as CustomsDeclararionKey," & _
               " Reference.i_Anio as OperationYear," & _
               " Reference.t_booking as Booking," & _
                " " & _
                " ( Select BillOfLading.t_NumeroGuia as IDGUID, BillOfLading.i_TipoGuia as TypeBL" & _
                " from Rep003RegGuiasInternacionales as BillOfLading with (nolock) " & _
                " where BillOfLading.i_cve_estado=1 and  Reference.i_Cve_VinOperacionesAgenciasAduanales = BillOfLading.i_Cve_VinOperacionesAgenciasAduanales" & _
                " FOR XML Path('BillOfLading'), Type " & _
                " ) as BillOfLadings," & _
                " " & _
                " ( Select CommercialInvoice.t_NumeroFactura as InvoiceNumber, CommercialInvoice.f_FechaFactura as InvoiceDate, t_Proveedor as SupplierName, '0' as TaxID," & _
                " " & _
                " ( Select InvoceItem.t_NumeroItem as PartNumber, InvoceItem.t_DescripcionMercancia  as Description,t_OrdenCompra as PurchaseOrder" & _
                " 		 from Rep003DetFacturasComerciales as InvoceItem with (nolock) " & _
                "         where InvoceItem.i_cve_estado=1 and CommercialInvoice.i_Cve_VinOperacionesAgenciasAduanales = InvoceItem.i_Cve_VinOperacionesAgenciasAduanales" & _
                " 		       and CommercialInvoice.i_Cve_FacturaComercial =  InvoceItem.i_Cve_FacturaComercial" & _
                " FOR XML Path('InvoceItem'), Type " & _
                " ) as InvoiceItems" & _
                " " & _
                "    from Rep003EncFacturasComerciales as CommercialInvoice with (nolock) " & _
                "         where CommercialInvoice.i_cve_estado=1 and Reference.i_Cve_VinOperacionesAgenciasAduanales = CommercialInvoice.i_Cve_VinOperacionesAgenciasAduanales" & _
                " FOR XML Path('CommercialInvoice'), Type " & _
                " ) as CommercialInvoices," & _
                " " & _
                " ( Select Container.t_NumeroContenedor as MarksOrNumbers,Container.i_Cve_idContenedorTipo as ContainerType,'' as ContainterSize" & _
                " from Rep003RegContenedores as Container with (nolock) " & _
                    " where Container.i_cve_estado=1 and Reference.i_Cve_VinOperacionesAgenciasAduanales = Container.i_Cve_VinOperacionesAgenciasAduanales" & _
                " FOR XML Path('Container'), Type " & _
                " ) as Containers" & _
                " " & _
                " from Vin003OperacionesAgenciasAduanales as Reference with (nolock) " & _
                " INNER JOIN Reg009MaestroOperaciones as OpMaster with (nolock) ON Reference.i_Cve_MaestroOperaciones=OpMaster.i_Cve_MaestroOperaciones " & _
                " where  (not Reference.f_FechaUltimoEstatus is null) and Reference.f_FechaAltaOperacion>=convert(datetime,'2016-08-01',120)  " & _
                " and (OpMaster.i_TipoReferencia=1) " & _
                " and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)<convert(date,DATEADD(day, -2, getdate()),120)) " & _
                "                   or (not Reference.f_FechaLiquidacionFactura is null)))  " & _
                "   or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)<convert(date,DATEADD(day, -2, getdate()),120)) and  " & _
                "           (not Reference.f_FechaDespacho is null)))) " & _
                " and Reference.i_Cve_Estado =1 " & _scriptOfRules & " " & clausulasAdicionales_ & _
                " " & _
                " " & _
                " ORDER by DateLastStatus desc " & _
                " FOR XML AUTO, ELEMENTS, ROOT ('References');"
                    '', ROOT ('References')
                    'ELEMENTS XSINIL,

                    ''''''Estas sentencias son para enviar solo las no concluidas cosiderando un tiempo de 2 dias posteriores''''
                    '" and ((@tipo=2 and ((convert(date,Reference.f_FechaLiquidacionFactura,120)>=convert(date,DATEADD(day, -2, getdate()),120)) " & _
                    '"                   or (Reference.f_FechaLiquidacionFactura is null)))  " & _
                    '"   or (@tipo=1 and ((convert(date,Reference.f_FechaDespacho,120)>=convert(date,DATEADD(day, -2, getdate()),120)) or  " & _
                    '"           (Reference.f_FechaDespacho is null)))) " & _


                    '     " when 1 then CAST(DATEADD(day, 2, f_FechaDespacho) AS DATETIME)  " & _
                    '" when 2 then CAST(DATEADD(day, 2, f_FechaLiquidacionFactura) AS DATETIME) " & _

                    '" case when not f_FechaRegistroHistorico is null then (CAST(Reference.f_FechaRegistroHistorico AS DATETIME) " & _
                    '"          + CAST(Reference.h_HoraRegistroHistorico AS DATETIME)) else CAST(Reference.f_FechaDespacho AS DATETIME) end as HistoricDate, " & _

                    'Reference.t_DescripcionNaveBuque as VesselName," & _
                    'convert(datetime,Reference.f_FechaAltaOperacion,126)
                    '" where Reference.f_FechaAltaOperacion>=convert(datetime,'2016-08-01',120) and " & _

                    '" Reference.f_FechaUltimoEstatus as DateLastStatus," & _
                    '" where convert(datetime,Reference.f_FechaAltaOperacion,102)>=convert(datetime,'" + LastUpdate.ToString("yyyy-MM-dd HH:mm:ss") + "',102) and" & _

                    '_system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()
                    'respuesta_ = _system.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_query)


                    Dim conexion_ As IConexiones = New Conexiones
                    conexion_.Contrasena = "S0l1umF0rW"
                    conexion_.ControladorBaseDatos = IConexiones.Controladores.SQLServer2008
                    conexion_.Usuario = "sa"
                    conexion_.NombreBaseDatos = "solium"
                    conexion_.IpServidor = "10.66.1.19"
                    conexion_.ObjetoDatos = IConexiones.TipoConexion.SqlCommand
                    'conexion_.IniciaConexion()
                    'conexion_.EjecutaConsulta("")
                    'conexion_.TerminaConexion()
                    conexion_.DataSetReciente.Tables.Clear()
                    respuesta_ = conexion_.EjecutaConsultaIndividual(_query)


                    If respuesta_ Then
                        'If Not _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente Is Nothing Then
                        '    If Not _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then
                        '        If _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count > 0 Then
                        If Not conexion_.DataSetReciente Is Nothing Then
                            If Not conexion_.DataSetReciente.Tables Is Nothing Then
                                If conexion_.DataSetReciente.Tables.Count > 0 Then
                                    'Dim object_ As String = _
                                    '    "<?xml version=" & Chr(34) & "1.0" & Chr(34) & "?>" & _
                                    '    "<GetAdvancedInformationSchemaInKromBaseResult xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & " xmlns:xsd=" & Chr(34) & "http://www.w3.org/2001/XMLSchema" & Chr(34) & ">" & _
                                    '    "  <SetXMLSchemaType>Advanced1</SetXMLSchemaType>" & _
                                    '      "<TagWatcher Errors=" & Chr(34) & "WS000" & Chr(34) & " ResultsCount=" & Chr(34) & "0" & Chr(34) & "> " & _
                                    '          "<Status>Ok</Status> " & _
                                    '          "<ErrorDescription></ErrorDescription> " & _
                                    '      "</TagWatcher> "


                                    'For Each registro_ As DataRow In _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows
                                    Dim objectXML_ As StringBuilder     'RIGG2
                                    objectXML_ = New StringBuilder()  'RIGG2

                                    objectXML_.Append("<?xml version=" & Chr(34) & "1.0" & Chr(34) & "?>" & _
                                        "<GetAdvancedInformationSchemaInKromBaseResult xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & " xmlns:xsd=" & Chr(34) & "http://www.w3.org/2001/XMLSchema" & Chr(34) & ">" & _
                                        "  <SetXMLSchemaType>Advanced1</SetXMLSchemaType>" & _
                                          "<TagWatcher Errors=" & Chr(34) & "WS000" & Chr(34) & " ResultsCount=" & Chr(34) & "0" & Chr(34) & "> " & _
                                              "<Status>Ok</Status> " & _
                                              "<ErrorDescription></ErrorDescription> " & _
                                          "</TagWatcher> ")

                                    For Each registro_ As DataRow In conexion_.DataSetReciente.Tables(0).Rows
                                        'object_ = object_ & registro_(0) 'RIGG2
                                        objectXML_.Append(registro_(0))
                                    Next

                                    'object_ = object_ & _  'RIGG2
                                    '    "</GetAdvancedInformationSchemaInKromBaseResult>"  'RIGG2

                                    objectXML_.Append("</GetAdvancedInformationSchemaInKromBaseResult>")

                                    Dim serializer As New XmlSerializer(GetType(XMLSchemaObject), New XmlRootAttribute("GetAdvancedInformationSchemaInKromBaseResult"))

                                    Dim a As String = serializer.ToString()

                                    Dim xmlSchemaObjectDeserializated_ As XMLSchemaObject = Nothing

                                    Dim string_reader As StringReader

                                    'Dim stream_reader As StreamReader = New StreamReader(objectXML_.ToString)

                                    'string_reader = New StringReader(object_) 'RIGG2
                                    string_reader = New StringReader(objectXML_.ToString)

                                    xmlSchemaObjectDeserializated_ = DirectCast(serializer.Deserialize(string_reader), XMLSchemaObject)
                                    'xmlSchemaObjectDeserializated_ = serializer.Deserialize(string_reader)

                                    _xmlSchemaObject.References = xmlSchemaObjectDeserializated_.References

                                    If Not ReferenceNumber Is Nothing Then

                                        If Not _xmlSchemaObject.References Is Nothing Then
                                            If _xmlSchemaObject.References.Count >= 1 Then

                                                _xmlSchemaObject.References(0).StatusCollection = GetStatusList(_xmlSchemaObject.References(0))

                                            End If
                                        End If

                                    End If

                                    _xmlSchemaObject.TagWatcher = xmlSchemaObjectDeserializated_.TagWatcher

                                End If

                            End If

                        End If
                    End If
                Case SchemaTypes.ReferenceType


            End Select

            _xmlSchemaObject.SetXMLSchemaType = schemaType_

            Return _xmlSchemaObject

        End Function

#End Region

        Public Function TestService(Optional ByVal YourName As String = "rafa", Optional ByVal LastName As String = "Gon") As String _
            Implements IWSReferences.TestService

            If IsNothing(YourName) Or YourName = "" Then
                YourName = "ALGODON"
            End If
            Return "Nombre: " & YourName & " - Apellido: " & LastName

        End Function


        Public Property XMLSchemaResult As XMLSchemaObject _
            Implements IWSReferences.XMLSchemaResult
            Get
                Return _xmlSchemaObject
            End Get
            Set(value As XMLSchemaObject)
                _xmlSchemaObject = value
            End Set
        End Property


    End Class


    '<XmlSerializerFormat>
    <DataContract()>
    Public Class XMLSchemaObject

#Region "Attributes"

        Private _references As List(Of Reference)

        Private _TagWatcher As TagWatcher

        Private _schemaType As SchemaTypes

#End Region

#Region "Builders"

        Sub New()

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
        Property References As List(Of Reference)
            Get
                Return _references
            End Get
            Set(value As List(Of Reference))
                _references = value
            End Set
        End Property



#End Region

#Region "Methods"

#End Region

    End Class


    '<XmlSerializerFormat>
    <DataContract()>
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

        '<XmlAttribute>
        <DataMember>
        Property IDAccesoUsuarioClavesCliente As Int32
            Get
                Return _i_Cve_AccesoUsuarioClavesCliente
            End Get
            Set(value As Int32)
                _i_Cve_AccesoUsuarioClavesCliente = value
            End Set
        End Property

        '<XmlAttribute>
        <DataMember>
        Property Accion As AccessTypes
            Get
                Return _i_Cve_Accion
            End Get
            Set(value As AccessTypes)
                _i_Cve_Accion = value
            End Set
        End Property

        '<XmlAttribute>
        <DataMember>
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

    '<XmlSerializerFormat>
    <DataContract()>
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

        '<XmlAttribute>
        <DataMember>
        Property IDGUID As String
            Get
                Return _id
            End Get
            Set(value As String)
                _id = value
            End Set

        End Property

        '<XmlAttribute>
        <DataMember>
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


    '<XmlSerializerFormat>
    <DataContract()>
    Public Class CommercialInvoice

#Region "Attributes"

        Private _invoiceNumber As String

        'Private _invoiceDate As Date
        Private _invoiceDate As String

        Private _supplierName As String

        Private _taxID As String

        Private _invoiceItems As List(Of InvoceItem)

#End Region

#Region "Enums"

#End Region

#Region "Builders"

        Sub New()

            _invoiceNumber = Nothing

            _invoiceDate = Nothing

            _supplierName = Nothing

            _taxID = Nothing

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

        '<XmlAttribute>
        <DataMember>
        Property InvoiceNumber As String
            Get
                Return _invoiceNumber
            End Get
            Set(value As String)
                _invoiceNumber = value
            End Set
        End Property

        '<XmlAttribute>
        <DataMember>
        Property InvoiceDate As String
            Get
                Return _invoiceDate
            End Get
            Set(value As String)
                _invoiceDate = value
            End Set
        End Property

        '<XmlAttribute>
        <DataMember>
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

    '<XmlSerializerFormat>
    <DataContract()>
    Public Class InvoceItem

#Region "Attributes"

        Private _partNumber As String

        Private _description As String

        Private _purchaseOrder As String

#End Region

#Region "Builders"

        Sub New()

            _partNumber = Nothing

            _description = Nothing

            _purchaseOrder = Nothing

        End Sub

#End Region

#Region "Properties"

        '<XmlAttribute>
        <DataMember>
        Property PartNumber As String
            Get
                Return _partNumber
            End Get
            Set(value As String)
                _partNumber = value
            End Set
        End Property

        '<XmlAttribute>
        <DataMember>
        Property Description As String
            Get
                Return _description
            End Get
            Set(value As String)
                _description = value
            End Set
        End Property

        '<XmlAttribute>
        <DataMember>
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


    '<XmlSerializerFormat>
    <DataContract()>
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

        '<XmlAttribute>
        <DataMember>
        Property MarksOrNumbers As String
            Get
                Return _marksOrNumber
            End Get
            Set(value As String)
                _marksOrNumber = value
            End Set
        End Property

        '<XmlAttribute>
        <DataMember>
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