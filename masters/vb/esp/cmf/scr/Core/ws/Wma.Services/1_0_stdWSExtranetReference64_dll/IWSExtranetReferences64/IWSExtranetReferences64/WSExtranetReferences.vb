' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de clase "Service1" en el código y en el archivo de configuración a la vez.
Imports gsol.krom.Referencia.AtributosDimensionReferencias
Imports gsol.BaseDatos.Operaciones
Imports WSExtranetReferences
Imports gsol.EspacioTrabajo
Imports Wma.Exceptions
Imports System.Data
Imports System.IO
Imports gsol
Imports Wma.Exceptions.TagWatcher
Imports System.Text
Imports gsol.basededatos
Imports gsol.seguridad
Imports gsol.krom

Public Class WSExtranetReferences
    Implements IWSExtranetReferences

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
        <EnumMember> <Description("México ( Terrestre )")> PAN = 472
        <EnumMember> <Description("Veracruz ( Marítimo )")> RKU = 430
        <EnumMember> <Description("Lázaro Cárdenas ( Marítimo )")> ALC = 510
        <EnumMember> <Description("Toluca ( Áereo )")> TOL = 650
        <EnumMember> <Description("Altamira ( Marítimo )")> CEG = 810
        <EnumMember> <Description("Nuevo Laredo ( Marítimo )")> SFI = 240
        <EnumMember> <Description("Laredo TX ( Marítimo )")> SFC = 800
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

#End Region

#Region "Attributes"

    Private _xmlSchemaObject As XMLSchemaObject

    Private _iOperations As IOperacionesCatalogo

    Private _system As New Organismo

    Public Shared _sesion As ISesion

    Private _sessionStatus As Boolean

    Private _dataAccessRulesList As New List(Of DataAccessRules)

    Private _scriptOfRules As String

#End Region

#Region "Builders"

#End Region



#Region "Methods"


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
                                                                 Optional ByVal ModalityType As WSExtranetReferences.ModalityTypes = WSExtranetReferences.ModalityTypes.UND, _
                                                                 Optional ByVal CustomSection As WSExtranetReferences.CustomSections = WSExtranetReferences.CustomSections.UND, _
                                                                 Optional ByVal OperationType As WSExtranetReferences.OperationTypes = WSExtranetReferences.OperationTypes.UND, _
                                                                 Optional ByVal DisplayData As WSExtranetReferences.TypesDisplay = WSExtranetReferences.TypesDisplay.ShowAll) As XMLSchemaObject Implements IWSExtranetReferences.GetAdvancedInformationSchemaInKromBase

        MobileUserID = "pbautista"
        WebServiceUserID = "brouniews@brounie.com"
        WebServicePasswordID = "AB24rsdAQ54"

        ModalityType = ModalityTypes.SEA
        CustomSection = CustomSections.RKU
        OperationType = OperationTypes.IMP
        DisplayData = TypesDisplay.Delivered

        Dim result_ As New TagWatcher

        Dim xmlSchemaObject_ As New XMLSchemaObject

        result_ = LogIn(MobileUserID, WebServiceUserID, WebServicePasswordID, 4, 1, False)

        If result_.Errors = ErrorTypes.WS000 Then

            Dim ContainterID As String = Nothing

            Dim CommercialInvoice As String = Nothing



            'ReturnXMLSchema(ByVal schemaType_ As SchemaTypes, _
            '                        Optional ByVal rfcOperationsClient_ As String = Nothing, _
            '                        Optional ByVal customerName_ As String = Nothing, _
            '                        Optional ByVal supplierName_ As String = Nothing, _

            '                        Optional ByVal vesselName_ As String = Nothing, _
            '                        Optional ByVal referenceNumber_ As String = Nothing, _
            '                        Optional ByVal customsDocument_ As String = Nothing, _

            '                        Optional ByVal containterID_ As String = Nothing, _
            '                        Optional ByVal commercialInvoice_ As String = Nothing, _
            '                        Optional ByVal modalityType_ As ModalityTypes = ModalityTypes.UND, _

            '                        Optional ByVal customSection_ As CustomSections = CustomSections.UND, _
            '                        Optional ByVal operationType_ As OperationTypes = OperationTypes.UND, _
            '                        Optional ByVal displayData_ As TypesDisplay = TypesDisplay.ShowAll _
            '                        )


            xmlSchemaObject_ = ReturnXMLSchema(SchemaTypes.Advanced1, _
 _
                                               RFCOperationsClient, _
                                               CustomerName_, _
                                               SupplierName_, _
 _
                                               VesselName_, _
                                               ReferenceNumber_, _
                                               CustomsDocument, _
 _
                                               ContainterID, _
                                               CommercialInvoice, _
                                               ModalityType, _
 _
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

        referencia1_.ModalityType = WSExtranetReferences.ModalityTypes.SEA

        referencia1_.OperationType = WSExtranetReferences.OperationTypes.IMP

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

    ''::::::::::::::::::::: Funcional Carlos:::::::::::::::::::::::


    Function TestWebService(Optional ByVal nombre_ As String = "") As String Implements IWSExtranetReferences.TestWebService

        If nombre_ = "" Then

            nombre_ = "extraño"

        End If

        Return "Hola " & nombre_ & " mi nombre es Evelynn y te doy la bienvenida al mundo del WCF."

    End Function

    Public Function GetBasicInformationSchemaInKromBase(ByVal mobileUserID_ As String _
                                                        , ByVal webServiceUserID_ As String _
                                                        , ByVal webServicePasswordID_ As String _
                                                        , Optional ByVal rfcCOperationsClient_ As String = Nothing _
                                                        , Optional ByVal customerName_ As String = Nothing _
                                                        , Optional ByVal supplierName_ As String = Nothing _
                                                        , Optional ByVal vesselName_ As String = Nothing _
                                                        , Optional ByVal modalityType_ As ModalityTypes = ModalityTypes.UND _
                                                        , Optional ByVal customSection_ As CustomSections = CustomSections.UND _
                                                        , Optional ByVal operationType_ As OperationTypes = OperationTypes.UND _
                                                        , Optional ByVal displayData_ As TypesDisplay = TypesDisplay.ShowAll) As XMLSchemaObject Implements IWSExtranetReferences.GetBasicInformationSchemaInKromBase

        mobileUserID_ = "pbautista"
        webServiceUserID_ = "brouniews@brounie.com"
        webServicePasswordID_ = "AB24rsdAQ54"

        modalityType_ = ModalityTypes.SEA
        customSection_ = CustomSections.RKU
        operationType_ = OperationTypes.IMP
        displayData_ = TypesDisplay.Delivered



        Dim result_ As New TagWatcher

        Dim xmlSchemaObject_ As New XMLSchemaObject

        result_ = LogIn(mobileUserID_, webServiceUserID_, webServicePasswordID_, 11, 1, False)

        If result_.Errors = ErrorTypes.WS000 Then

            xmlSchemaObject_ = ReturnXMLSchema(SchemaTypes.OnlyHeaders _
                                               , rfcCOperationsClient_ _
                                               , customerName_ _
                                               , supplierName_ _
                                               , vesselName_ _
                                                , _
                                                , _
                                                , _
                                                , _
                                               , modalityType_ _
                                               , customSection_ _
                                               , operationType_ _
                                               , displayData_)

        Else

            xmlSchemaObject_.TagWatcher = result_

        End If

        Return xmlSchemaObject_

    End Function

    Private Function LogIn(ByVal mobileUserID_ As String _
                            , ByVal webServiceUserID_ As String _
                            , ByVal webServicePasswordID_ As String _
                            , ByVal idRequiredApplication_ As Integer _
                            , ByVal corporateNumber_ As Integer _
                            , Optional ByVal fullAuthentication_ As Boolean = False) As TagWatcher

        Dim results_ As New TagWatcher

        _sessionStatus = False

        SessionPrepare(webServiceUserID_, webServicePasswordID_)

        If _sesion.StatusArgumentos Then

            _sessionStatus = True

            _sesion.GrupoEmpresarial = corporateNumber_

            _sesion.DivisionEmpresarial = 1

            _sesion.Aplicacion = idRequiredApplication_

            _sesion.Idioma = ISesion.Idiomas.Espaniol

            If Not fullAuthentication_ Then

                _iOperations = _system.EnsamblaModulo("Usuarios")

                Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

                temporalWorkSpace_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

                _iOperations.EspacioTrabajo = temporalWorkSpace_

                _iOperations.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                _iOperations.ClausulasLibres = " and t_usuario = '" & mobileUserID_ & "'"

                _iOperations.CantidadVisibleRegistros = 1000

                _iOperations.GenerarVista()

                If _system.TieneResultados(_iOperations) Then

                    _iOperations = _system.EnsamblaModulo("UsuariosAppMovil")

                    Dim temporalWorkSpace2_ As IEspacioTrabajo = New EspacioTrabajo

                    temporalWorkSpace2_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Produccion

                    _iOperations.EspacioTrabajo = temporalWorkSpace2_

                    _iOperations.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                    _iOperations.ClausulasLibres = " and t_usuario = '" & mobileUserID_ & "' and i_Cve_Estado = 1"

                    _iOperations.CantidadVisibleRegistros = 1000

                    _iOperations.GenerarVista()

                    If _system.TieneResultados(_iOperations) Then

                        '_dataAccessRulesList = New List(Of DataAccessRules)

                        For indice_ As Int32 = 0 To _iOperations.Vista.Tables(0).Rows.Count - 1

                            Dim accessRule_ As New DataAccessRules

                            accessRule_.IDAccesoUsuarioClavesCliente = _iOperations.Vista(indice_, "Clave", IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                            accessRule_.IDDivisionMiEmpresa = _iOperations.Vista(indice_, "i_Cve_DivisionMiEmpresa", IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                            accessRule_.RFC = _iOperations.Vista(indice_, "RFC", IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                            accessRule_.IDClienteEmpresa = _iOperations.Vista(indice_, "ID Cliente Empresa", IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                            accessRule_.IDDivisionEmpresarialCliente = _iOperations.Vista(indice_, "ID División Cliente", IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                            accessRule_.Aplicacion = _iOperations.Vista(indice_, "i_Cve_Aplicacion", IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                            accessRule_.Accion = _iOperations.Vista(indice_, "i_Cve_Accion", IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                            accessRule_.Aduana = _iOperations.Vista(indice_, "Aduana Sección", IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                            accessRule_.Patente = _iOperations.Vista(indice_, "Patente", IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                            accessRule_.TipoOperacion = _iOperations.Vista(indice_, "Tipo Operación", IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                            'MsgBox(_iOperations.Vista(indice_, "Clave", IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado) & ", " & _ _iOperations.Vista(indice_, "t_RFC", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico))

                            _dataAccessRulesList.Add(accessRule_)

                        Next

                        'For Each row_ As DataRow In _iOperations.Vista.Tables(0).Rows

                        '    Dim accessRule_ As New DataAccessRules

                        '    accessRule_.IDAccesoUsuarioClavesCliente = row_("Clave")

                        '    accessRule_.IDDivisionMiEmpresa = row_("i_Cve_DivisionMiEmpresa")

                        '    accessRule_.RFC = row_("t_RFC")

                        '    accessRule_.IDClienteEmpresa = row_("i_Cve_ClienteEmpresa")

                        '    accessRule_.IDDivisionEmpresarialCliente = row_("i_Cve_DivisionEmpresarialCliente")

                        '    accessRule_.Aplicacion = row_("i_Cve_Aplicacion")

                        '    accessRule_.Accion = row_("i_Cve_Accion")

                        '    accessRule_.Aduana = row_("i_Aduana")

                        '    accessRule_.Patente = row_("i_Patente")

                        '    accessRule_.TipoOperacion = row_("i_TipoOperacion")

                        '    _dataAccessRulesList.Add(accessRule_)

                        'Next

                        CovertRuleToScript(results_)

                    Else

                        results_.SetError(ErrorTypes.WS007, " This mobile user doesn't have client rules to access profile,  although was authenticated")

                    End If

                Else

                    results_.SetError(ErrorTypes.WS001)

                End If

            Else

                _sessionStatus = _sesion.StatusArgumentos

                If _sessionStatus Then

                    If _sesion.StatusArgumentos = True And _sesion.EspacioTrabajo IsNot Nothing Then

                        _iOperations = New OperacionesCatalogo

                        _iOperations = _system.ConsultaModulo(_sesion.EspacioTrabajo, "Usuarios", " and t_usuario = '" & mobileUserID_ & "'").Clone

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

    Private Sub SessionPrepare(ByVal user_ As String _
                                , ByVal password_ As String)

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

                accessRules_ = accessRules_ & " i_Cve_DivisionMiEmpresa =" & rule_.IDDivisionMiEmpresa

            End If

            If Not rule_.RFC Is Nothing Then

                'accessRules_ = accessRules_ & " and t_RfcExterno ='" & rule_.RFC & "'" 'NO esta

            End If

            If rule_.IDClienteEmpresa > 0 Then

                'accessRules_ = accessRules_ & " and i_Cve_VinClienteOperacion =" & rule_.IDClienteEmpresa 'NO esta

            End If

            If rule_.IDDivisionEmpresarialCliente > 0 Then

                'accessRules_ = accessRules_ & " and i_Cve_DivisionEmpresarialCliente =" & rule_.IDDivisionEmpresarialCliente 'NO esta

            End If

            If rule_.Aduana > 0 Then

                accessRules_ = accessRules_ & " and i_AduanaSeccion = " & rule_.Aduana

            End If

            If rule_.Patente > 0 Then

                accessRules_ = accessRules_ & " and i_Patente = " & rule_.Patente

            End If

            If rule_.TipoOperacion > 0 Then

                'accessRules_ = accessRules_ & " and i_TipoOperacion = " & rule_.TipoOperacion 'NO esta

            End If

            accessRules_ = accessRules_ & " ) "

            script_ = script_ & " " & tokenOr_ & " " & accessRules_

            counter_ += 1

        Next

        acessed_ = " ( " & script_ & ") "

        script_ = Nothing

        accessRules_ = Nothing

        counter_ = 1

        'For Each rule_ As DataAccessRules In _dataAccessRulesList

        '    If counter_ = 1 Then

        '        token_ = Nothing

        '        tokenOr_ = Nothing

        '    Else

        '        token_ = "and"

        '        tokenOr_ = "or"

        '    End If

        '    Select Case rule_.Accion

        '        Case AccessTypes.Access

        '            Continue For

        '        Case AccessTypes.Deny

        '            accessRules_ = " ( "

        '    End Select

        '    If rule_.IDDivisionMiEmpresa > 0 Then

        '        accessRules_ = accessRules_ & " i_Cve_DivisionMiEmpresa =" & rule_.IDDivisionMiEmpresa

        '    End If

        '    If Not rule_.RFC Is Nothing Then

        '        accessRules_ = accessRules_ & " and t_RfcExterno ='" & rule_.RFC & "'"

        '    End If

        '    If rule_.IDClienteEmpresa > 0 Then

        '        accessRules_ = accessRules_ & " and i_Cve_ClienteEmpresa =" & rule_.IDClienteEmpresa

        '    End If

        '    If rule_.IDDivisionEmpresarialCliente > 0 Then

        '        accessRules_ = accessRules_ & " and i_Cve_DivisionEmpresarialCliente =" & rule_.IDDivisionEmpresarialCliente

        '    End If

        '    If rule_.Aduana > 0 Then

        '        accessRules_ = accessRules_ & " and i_AduanaSeccion = " & rule_.Aduana

        '    End If

        '    If rule_.Patente > 0 Then

        '        accessRules_ = accessRules_ & " and i_Patente = " & rule_.Patente

        '    End If

        '    If rule_.TipoOperacion > 0 Then

        '        accessRules_ = accessRules_ & " and i_TipoOperacion = " & rule_.TipoOperacion

        '    End If

        '    accessRules_ = accessRules_ & " ) "

        '    script_ = script_ & " " & tokenOr_ & " " & accessRules_

        '    counter_ += 1

        'Next

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

    Private Function ReturnXMLSchema(ByVal schemaType_ As SchemaTypes, _
                                    Optional ByVal rfcOperationsClient_ As String = Nothing, _
                                    Optional ByVal customerName_ As String = Nothing, _
                                    Optional ByVal supplierName_ As String = Nothing, _
                                    Optional ByVal vesselName_ As String = Nothing, _
                                    Optional ByVal referenceNumber_ As String = Nothing, _
                                    Optional ByVal customsDocument_ As String = Nothing, _
                                    Optional ByVal containterID_ As String = Nothing, _
                                    Optional ByVal commercialInvoice_ As String = Nothing, _
                                    Optional ByVal modalityType_ As ModalityTypes = ModalityTypes.UND, _
                                    Optional ByVal customSection_ As CustomSections = CustomSections.UND, _
                                    Optional ByVal operationType_ As OperationTypes = OperationTypes.UND, _
                                    Optional ByVal displayData_ As TypesDisplay = TypesDisplay.ShowAll _
                                    ) As XMLSchemaObject

        _xmlSchemaObject = New XMLSchemaObject

        Dim TagWatcher_ As New TagWatcher

        TagWatcher_.Errors = ErrorTypes.WS000

        TagWatcher_.Status = TypeStatus.Ok

        TagWatcher_.ResultsCount = 0

        Dim clausulasAdicionales_ As String = Nothing

        If Not referenceNumber_ Is Nothing Then
            clausulasAdicionales_ = clausulasAdicionales_ & " and t_NumeroReferencia = '" & referenceNumber_ & "'"
        End If

        If Not customsDocument_ Is Nothing Then
            clausulasAdicionales_ = clausulasAdicionales_ & " and i_NumeroPedimentoCompleto like '%" & customsDocument_ & "%'"
        End If

        Select Case displayData_

            Case TypesDisplay.Delivered

                clausulasAdicionales_ = clausulasAdicionales_ & " and i_Despachada = 1"

            Case TypesDisplay.NotDelivered

                clausulasAdicionales_ = clausulasAdicionales_ & " and i_Despachada = 0"

        End Select

        If Not customerName_ Is Nothing Then
            clausulasAdicionales_ = clausulasAdicionales_ & " and t_NombreCliente = '" & customerName_ & "'"
        End If

        If Not vesselName_ Is Nothing Then
            clausulasAdicionales_ = clausulasAdicionales_ & " and t_DescripcionNaveBuque = '" & vesselName_ & "'"
        End If

        If Not modalityType_ = ModalityTypes.UND Then
            Select Case modalityType_

                Case ModalityTypes.AIR

                    clausulasAdicionales_ = clausulasAdicionales_ & " and i_AduanaSeccion in (470,650)"

                Case ModalityTypes.SEA

                    clausulasAdicionales_ = clausulasAdicionales_ & " and i_AduanaSeccion not in (470,650,200) "

                Case ModalityTypes.ROA

                    clausulasAdicionales_ = clausulasAdicionales_ & " and i_AduanaSeccion in (200) " & customSection_

            End Select

        End If

        If Not customSection_ = CustomSections.UND Then
            clausulasAdicionales_ = clausulasAdicionales_ & " and i_AduanaSeccion = " & customSection_
        End If

        If Not customsDocument_ Is Nothing Then
            clausulasAdicionales_ = clausulasAdicionales_ & " and i_NumeroPedimento like '%" & customsDocument_ & "%'"
        End If

        If Not operationType_ = OperationTypes.UND Then
            clausulasAdicionales_ = clausulasAdicionales_ & " and i_TipoOperacion = " & operationType_
        End If

        If Not rfcOperationsClient_ Is Nothing Then
            clausulasAdicionales_ = clausulasAdicionales_ & " and t_RFCExterno = '" & rfcOperationsClient_ & "'"
        End If

        Select Case schemaType_

            Case SchemaTypes.OnlyHeaders

                ':::::::::::::BLOQUE OBSOLETO:::::::::::::::::::

                '_iOperations = _system.EnsamblaModulo("OperacionesAgenciasAduanales")

                'Dim temporalWorkSpace_ As IEspacioTrabajo = New EspacioTrabajo

                'temporalWorkSpace_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

                '_iOperations.EspacioTrabajo = temporalWorkSpace_

                '_iOperations.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                '_iOperations.CantidadVisibleRegistros = 1000

                '_iOperations.ClausulasLibres = _scriptOfRules

                '_iOperations.ClausulasLibres = _iOperations.ClausulasLibres & _
                '    clausulasAdicionales_

                '_iOperations.ModalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.ConexionLibre

                '_iOperations.GenerarVista()

                '                If _system.TieneResultados(_iOperations) Then
                '::::::::::::::::: BLOQUE OBSOLETO :::::::::::::::::::::::

                Dim link_ As IEnlaceDatos = New EnlaceDatos

                With link_

                    .EspacioTrabajo = _iOperations.EspacioTrabajo

                    .LimiteResultados = 200

                    .Granularidad = IEnlaceDatos.TiposDimension.Referencias

                    .ModalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo

                    .TipoRespuestaRequerida = IEnlaceDatos.FormatosRespuesta.IOperaciones

                End With

                ''::::: Creamos la estructura solicitada

                Dim pckOperacionesExtranet_ As IEntidadDatos = New Referencia

                ':::: Especificamos los campos que necesitamos y el orden. :::::

                With pckOperacionesExtranet_
                    .cmp(t_Cve_Pedimento)
                    .cmp(i_NumeroPedimentoCompleto)
                    .cmp(i_AduanaSeccion)
                    .cmp(f_FechaUltimoEstatus)
                    .cmp(i_Despachada)
                    .cmp(t_NumeroReferencia)
                    .cmp(Referencia.AtributosDimensionReferencias.i_Cve_DivisionMiEmpresa)
                    .cmp(i_AduanaSeccion)
                    .cmp(i_TipoOperacion)
                    .cmp(t_DescripcionNaveBuque)
                    .cmp(t_NombreCliente)
                    .cmp(i_Anio)
                End With

                pckOperacionesExtranet_.Dimension = IEnlaceDatos.TiposDimension.ExtranetGeneralOperaciones

                'pckOperacionesExtranet_.GetFactura("·")

                'pckOperacionesExtranet_.Referencia("ffdf").Facturas.


                Dim registrosEncontrados_ As New List(Of IEntidadDatos)

                registrosEncontrados_ = link_.GeneraTransaccion(pckOperacionesExtranet_, Nothing)

                If link_.MensajeTagWatcher.Status = Wma.Exceptions.TagWatcher.TypeStatus.Ok Then

                    If registrosEncontrados_.Count > 0 Then

                        For Each row_ As DataRow In link_.IOperaciones.Vista.Tables(0).Rows

                            Dim referencia_ As New Reference

                            With referencia_

                                .CustomerName = row_(t_NombreCliente.ToString)

                                .OperationYear = row_(i_Anio.ToString)

                                .CustomsDeclararionKey = row_(t_Cve_Pedimento.ToString) 'Clave Pedimento

                                .CustomsDeclarationDocumentID = row_(i_NumeroPedimentoCompleto.ToString) 'NumeroPedimento

                                .CustomSection = row_(i_AduanaSeccion.ToString)

                                .DateLastStatus = row_(f_FechaUltimoEstatus.ToString)

                                .Delivered = row_(i_Despachada.ToString)

                                .ID = row_(t_NumeroReferencia.ToString)

                                .ManagementCompanyId = row_(i_Cve_DivisionMiEmpresa.ToString)

                                Select Case row_(i_AduanaSeccion.ToString)

                                    Case "430", "510", "810", "160", "420"

                                        .ModalityType = ModalityTypes.SEA

                                    Case "470", "650"

                                        .ModalityType = ModalityTypes.AIR

                                    Case "200", "240", "800"

                                        .ModalityType = ModalityTypes.ROA

                                End Select

                                Select Case row_(i_TipoOperacion.ToString)

                                    Case "1" : .OperationType = OperationTypes.IMP

                                    Case "2" : .OperationType = OperationTypes.EXP

                                    Case Else

                                        .OperationType = OperationTypes.UND

                                End Select

                                .OperationYear = row_(i_Anio.ToString)

                                If row_(i_Despachada.ToString) = "1" Then

                                    .StatusID = ReferenceStatusTypes.DSP

                                    .StatusTag = ReferenceStatusTypes.DSP.ToString

                                Else

                                    .StatusID = ReferenceStatusTypes.DOC

                                    .StatusTag = ReferenceStatusTypes.DOC.ToString

                                End If

                                If Not DBNull.Value.Equals(row_(t_DescripcionNaveBuque.ToString)) Then

                                    .VesselName = row_(t_DescripcionNaveBuque.ToString)

                                Else

                                    .VesselName = Nothing

                                End If

                            End With

                            _xmlSchemaObject.References.Add(referencia_)

                        Next

                    End If

                Else

                    TagWatcher_.Errors = ErrorTypes.WS006

                    TagWatcher_.Status = TypeStatus.Errors

                    TagWatcher_.ResultsCount = 0

                End If

                _xmlSchemaObject.TagWatcher = TagWatcher_

                ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

            Case SchemaTypes.Advanced1

                Dim respuesta_ As Boolean = False

                Dim _query As String = _
            "Select top(1000) " & _
            "Reference.t_DescripcionNaveBuque as VesselName," & _
            "Reference.t_CantidadBultos as PackagesAndPallets," & _
            "Reference.f_FechaEstimadaArribo as EstimatedTimeArrive," & _
            "Reference.f_FechaAltaOperacion as ReferenceDate," & _
            "Reference.t_NombreCliente as CustomerName," & _
            "Reference.t_NumeroReferencia as ID," & _
            " " & _
             " CASE WHEN i_AduanaSeccion in (430,160,810,510) THEN '1' " & _
                  " WHEN i_AduanaSeccion in (470,650) THEN '2'  " & _
                  " WHEN i_AduanaSeccion in (200,432) THEN '3'  " & _
            " END AS ModalityType, " & _
            "  " & _
            " Reference.i_TipoOperacion as OperationType, " & _
            " " & _
            " CASE WHEN i_AduanaSeccion in (430) THEN '1' " & _
                 " WHEN i_AduanaSeccion in (160) THEN '8' " & _
                 " WHEN i_AduanaSeccion in (810) THEN '6' " & _
                 " WHEN i_AduanaSeccion in (510) THEN '9' " & _
                 " WHEN i_AduanaSeccion in (800) THEN '113' " & _
                 " WHEN i_AduanaSeccion in (470) THEN '3' " & _
                 " WHEN i_AduanaSeccion in (650) THEN '7' " & _
                 " WHEN i_AduanaSeccion in (200) THEN '3' " & _
           " END AS ManagementCompanyId," & _
           " " & _
           " Reference.t_Cve_TrackingReciente as StatusID," & _
           " Reference.f_FechaUltimoEstatus as DateLastStatus," & _
           " Reference.i_AduanaSeccion as  CustomSection," & _
           " Reference.i_Despachada as Delivered," & _
           " Reference.i_NumeroPedimentoCompleto as CustomsDeclarationDocumentID," & _
           " Reference.t_Cve_Pedimento as CustomsDeclararionKey," & _
           " Reference.i_Anio as OperationYear," & _
            " " & _
            " ( Select BillOfLading.t_NumeroGuia as IDGUID, BillOfLading.i_TipoGuia as TypeBL" & _
            " from Rep003RegGuiasInternacionales as BillOfLading " & _
            " where Reference.i_Cve_VinOperacionesAgenciasAduanales = BillOfLading.i_Cve_VinOperacionesAgenciasAduanales" & _
            " FOR XML AUTO, TYPE" & _
            " ) as BillOfLadings," & _
            " " & _
            " ( Select CommercialInvoice.t_NumeroFactura as InvoiceNumber, CommercialInvoice.f_FechaFactura as InvoiceDate, t_Proveedor as SupplierName, '0' as TaxID," & _
            " " & _
            " ( Select InvoceItem.t_NumeroItem as PartNumber, InvoceItem.t_DescripcionMercancia  as Description,t_OrdenCompra as PurchaseOrder" & _
            " 		 from Rep003DetFacturasComerciales as InvoceItem " & _
            "         where CommercialInvoice.i_Cve_VinOperacionesAgenciasAduanales = InvoceItem.i_Cve_VinOperacionesAgenciasAduanales" & _
            " 		       and CommercialInvoice.i_Cve_FacturaComercial =  InvoceItem.i_Cve_FacturaComercial" & _
                    " FOR XML AUTO, TYPE" & _
            " ) as InvoiceItems" & _
            " " & _
            "    from Rep003EncFacturasComerciales as CommercialInvoice " & _
            "         where Reference.i_Cve_VinOperacionesAgenciasAduanales = CommercialInvoice.i_Cve_VinOperacionesAgenciasAduanales" & _
             " FOR XML AUTO, TYPE" & _
            " ) as CommercialInvoices," & _
            " " & _
               " ( Select Container.t_NumeroContenedor as MarksOrNumbers,Container.i_Cve_idContenedorTipo as ContainerType,'' as ContainterSize" & _
             " from Rep003RegContenedores as Container " & _
                " where Reference.i_Cve_VinOperacionesAgenciasAduanales = Container.i_Cve_VinOperacionesAgenciasAduanales" & _
             " FOR XML AUTO, TYPE" & _
            " ) as Containers" & _
            " " & _
            " from Vin003OperacionesAgenciasAduanales as Reference " & _
            " where Reference.i_Cve_Estado =1 " & _scriptOfRules & " " & clausulasAdicionales_ & _
            " FOR XML AUTO, ROOT ('References');"

                _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

                respuesta_ = _system.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_query)

                If Not _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente Is Nothing Then
                    If Not _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then
                        If _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count > 0 Then

                            Dim object_ As String = _
                                "<?xml version=" & Chr(34) & "1.0" & Chr(34) & "?>" & _
                                "<GetAdvancedInformationSchemaInKromBaseResult xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & " xmlns:xsd=" & Chr(34) & "http://www.w3.org/2001/XMLSchema" & Chr(34) & ">" & _
                                "  <SetXMLSchemaType>Advanced1</SetXMLSchemaType>" & _
                                  "<TagWatcher Errors=" & Chr(34) & "WS000" & Chr(34) & " ResultsCount=" & Chr(34) & "0" & Chr(34) & "> " & _
                                      "<Status>Ok</Status> " & _
                                      "<ErrorDescription></ErrorDescription> " & _
                                  "</TagWatcher> "


                            For Each registro_ As DataRow In _system.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows
                                object_ = object_ & registro_(0)
                            Next

                            'bautismen

                            object_ = object_ & _
                                "</GetAdvancedInformationSchemaInKromBaseResult>"

                            Dim serializer As New XmlSerializer(GetType(XMLSchemaObject), New XmlRootAttribute("GetAdvancedInformationSchemaInKromBaseResult"))

                            Dim xmlSchemaObjectDeserializated_ As XMLSchemaObject = Nothing

                            Dim string_reader As StringReader

                            string_reader = New StringReader(object_)

                            xmlSchemaObjectDeserializated_ = DirectCast(serializer.Deserialize(string_reader), XMLSchemaObject)

                            _xmlSchemaObject.References = xmlSchemaObjectDeserializated_.References

                            If Not referenceNumber_ Is Nothing Then

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

            Case SchemaTypes.ReferenceType


        End Select

        _xmlSchemaObject.SetXMLSchemaType = schemaType_

        Return _xmlSchemaObject

    End Function

    Private Function GetStatusList(ByVal reference_ As Reference) As List(Of StatusItem)

        Dim _query As String

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

        Dim _sar As Boolean = False

        Dim _rad As Boolean = False

        Dim _fac As Boolean = False

        Dim _facDate As New DateTime

        Dim _lfac As Boolean = False

        Dim _lfacDate As New DateTime

        'CheckInvoce(reference_.ID, _fac, _lfac, _facDate, _lfacDate)

        'If reference_.Delivered = "1" Then
        '    _fac = True
        'End If

        Dim _cam As Boolean = False : If reference_.Delivered = "1" Then : _cam = True : End If

        Dim _cru As Boolean = False : If reference_.Delivered = "1" Then : _cru = True : End If

        Dim _ieu As Boolean = False

        Dim _MedioTransporteArribo As Integer = 0

        'AddStatus(_statusList, 0, ReferenceStatusTypes.ALR, Now, _alr)

        _query = "" & _
            "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " & _
            "SELECT " & _
                "MP.i_Cve_MaestroOperaciones" & _
                ", MP.t_Referencia" & _
                ", ET.i_Cve_PlantillaTracking" & _
                ", AT.i_Cve_AccionTracking" & _
                ", AT.t_ClaveAccion" & _
                ", convert(varchar,cast(MAX(DT.f_Alta) as datetime) + cast(MAX(DT.H_Alta) as datetime),126) AS Fecha" & _
                ", Vi.i_cve_MedioTransporteArribo " & _
            "FROM " & _
                "Reg009MaestroOperaciones AS MP with (nolock) " & _
                "INNER JOIN Enc008Tracking AS ET with (nolock) ON (MP.i_Cve_MaestroOperaciones = ET.i_Cve_Operacion) " & _
                "INNER JOIN Det008TrackingDetalles AS DT with (nolock) ON (ET.i_Cve_Tracking = DT.i_Cve_EncabezadoTracking) " & _
                "INNER JOIN Cat008AccionesTracking AS AT with (nolock) ON (DT.i_Cve_AccionTracking = AT.i_Cve_AccionTracking) " & _
                "INNER JOIN Vin003OperacionesAgenciasAduanales AS Vi with (nolock) ON (vi.t_NumeroReferencia = MP.t_Referencia) " & _
            "WHERE " & _
                "Dt.i_cve_estado = 1 " & _
                "AND ET.i_cve_estado = 1 " & _
                "AND MP.i_cve_estado = 1 " & _
                "AND MP.t_Referencia = '" & reference_.ID & "' " & _
            "GROUP BY " & _
                "MP.i_Cve_MaestroOperaciones " & _
                ", MP.t_Referencia " & _
                ", ET.i_Cve_PlantillaTracking " & _
                ", AT.i_Cve_AccionTracking " & _
                ",AT.t_ClaveAccion " & _
                ",Vi.i_cve_MedioTransporteArribo "

        Select Case _operationModal

            Case CustomSections.ALC, CustomSections.CEG, CustomSections.RKU

                Select Case _operationType

                    Case OperationTypes.IMP

                        'DOC()
                        AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, Now, _doc)

                        'REV()
                        AddStatus(_statusList, 2, ReferenceStatusTypes.REV, Now, _rev)

                        'PRE()
                        AddStatus(_statusList, 3, ReferenceStatusTypes.PRE, Now, _pre)

                        'PAG()
                        AddStatus(_statusList, 4, ReferenceStatusTypes.PAG, Now, _pag)

                        'SAR()                            
                        AddStatus(_statusList, 5, ReferenceStatusTypes.SAR, Now, _sar)

                        'RAD()
                        AddStatus(_statusList, 6, ReferenceStatusTypes.RAD, Now, _rad)

                        'DSP()
                        AddStatus(_statusList, 7, ReferenceStatusTypes.DSP, Now, _dsp)

                        'FAC()
                        AddStatus(_statusList, 8, ReferenceStatusTypes.FAC, _facDate, _fac)

                        'LFAC()
                        AddStatus(_statusList, 9, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)

                    Case OperationTypes.EXP

                        'DOC()
                        AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, Now, _doc)

                        'PAG()
                        AddStatus(_statusList, 2, ReferenceStatusTypes.PAG, Now, _pag)

                        'SAR()                            
                        AddStatus(_statusList, 3, ReferenceStatusTypes.SAR, Now, _sar)

                        'RAD()
                        AddStatus(_statusList, 4, ReferenceStatusTypes.RAD, Now, _rad)

                        'DSP()
                        AddStatus(_statusList, 5, ReferenceStatusTypes.DSP, Now, _dsp)

                        'FAC()
                        AddStatus(_statusList, 6, ReferenceStatusTypes.FAC, _facDate, _fac)

                        'LFAC()
                        AddStatus(_statusList, 7, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)

                End Select

            Case CustomSections.DAI, CustomSections.TOL

                Select Case _operationType

                    Case OperationTypes.IMP

                        'DOC()
                        AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, Now, _doc)

                        'REV()
                        AddStatus(_statusList, 2, ReferenceStatusTypes.REV, Now, _rev)

                        'RGU()
                        AddStatus(_statusList, 3, ReferenceStatusTypes.RGU, Now, _rgu)

                        'PAG()
                        AddStatus(_statusList, 4, ReferenceStatusTypes.PAG, Now, _pag)

                        'SAR()                            
                        AddStatus(_statusList, 5, ReferenceStatusTypes.SAR, Now, _sar)

                        'RAD()
                        AddStatus(_statusList, 6, ReferenceStatusTypes.RAD, Now, _rad)

                        'DSP()
                        AddStatus(_statusList, 7, ReferenceStatusTypes.DSP, Now, _dsp)

                        'FAC()
                        AddStatus(_statusList, 8, ReferenceStatusTypes.FAC, _facDate, _fac)

                        'LFAC()
                        AddStatus(_statusList, 9, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)

                    Case OperationTypes.EXP

                        'DOC()
                        AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, Now, _doc)

                        'PAG()
                        AddStatus(_statusList, 2, ReferenceStatusTypes.PAG, Now, _pag)

                        'SAR()                            
                        AddStatus(_statusList, 3, ReferenceStatusTypes.SAR, Now, _sar)

                        'RAD()
                        AddStatus(_statusList, 4, ReferenceStatusTypes.RAD, Now, _rad)

                        'DSP()
                        AddStatus(_statusList, 5, ReferenceStatusTypes.DSP, Now, _dsp)

                        'FAC()
                        AddStatus(_statusList, 6, ReferenceStatusTypes.FAC, _facDate, _fac)

                        'LFAC()
                        AddStatus(_statusList, 7, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)

                End Select

            Case CustomSections.PAN, CustomSections.SFI, CustomSections.SFC

                Select Case _operationType

                    Case OperationTypes.IMP

                        'RBO()
                        AddStatus(_statusList, 1, ReferenceStatusTypes.RBO, Now, _doc)

                        'PRE()
                        AddStatus(_statusList, 2, ReferenceStatusTypes.PRE, Now, _doc)

                        'DOC()
                        AddStatus(_statusList, 3, ReferenceStatusTypes.DOC, Now, _doc)

                        'PAG()
                        AddStatus(_statusList, 4, ReferenceStatusTypes.PAG, Now, _pag)

                        'CAM()
                        AddStatus(_statusList, 5, ReferenceStatusTypes.CAM, Now, _cam)

                        'CRU()
                        AddStatus(_statusList, 6, ReferenceStatusTypes.CRU, Now, _cru)

                        'SAR()                            
                        AddStatus(_statusList, 7, ReferenceStatusTypes.SAR, Now, _sar)

                        'RAD()
                        AddStatus(_statusList, 8, ReferenceStatusTypes.RAD, Now, _rad)

                        'DSP()
                        AddStatus(_statusList, 9, ReferenceStatusTypes.DSP, Now, _dsp)

                        'FAC()
                        AddStatus(_statusList, 10, ReferenceStatusTypes.FAC, _facDate, _fac)

                        'LFAC()
                        AddStatus(_statusList, 11, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)


                    Case OperationTypes.EXP

                        'DOC()
                        AddStatus(_statusList, 1, ReferenceStatusTypes.DOC, Now, _doc)

                        'AME
                        AddStatus(_statusList, 2, ReferenceStatusTypes.AME, Now, _cam)

                        'PAG()
                        AddStatus(_statusList, 3, ReferenceStatusTypes.PAG, Now, _pag)

                        'CRU()
                        AddStatus(_statusList, 4, ReferenceStatusTypes.CRU, Now, _cru)

                        'SAR()                            
                        AddStatus(_statusList, 5, ReferenceStatusTypes.SAR, Now, _sar)

                        'RAD()
                        AddStatus(_statusList, 6, ReferenceStatusTypes.RAD, Now, _rad)

                        'DSP()
                        AddStatus(_statusList, 7, ReferenceStatusTypes.DSP, Now, _dsp)

                        'IEU()
                        AddStatus(_statusList, 8, ReferenceStatusTypes.IEU, Now, _ieu)

                        'FAC()
                        AddStatus(_statusList, 9, ReferenceStatusTypes.FAC, _facDate, _fac)

                        'LFAC()
                        AddStatus(_statusList, 10, ReferenceStatusTypes.LFAC, _lfacDate, _lfac)

                End Select

            Case CustomSections.UND

                Select Case _operationType

                    Case OperationTypes.IMP, OperationTypes.IMP, OperationTypes.UND

                        'BOO()
                        AddStatus(_statusList, 1, ReferenceStatusTypes.BOO, Now, False)

                        'PPL()
                        AddStatus(_statusList, 2, ReferenceStatusTypes.PPL, Now, False)

                        'ZAR()
                        AddStatus(_statusList, 3, ReferenceStatusTypes.ZAR, Now, False)

                        'INR()
                        AddStatus(_statusList, 4, ReferenceStatusTypes.INR, Now, False)

                        'DES()
                        AddStatus(_statusList, 5, ReferenceStatusTypes.DES, Now, False)

                        'CAT()
                        AddStatus(_statusList, 6, ReferenceStatusTypes.CAT, Now, False)

                        'TBD()
                        AddStatus(_statusList, 7, ReferenceStatusTypes.TBD, Now, False)

                        'ABU()
                        AddStatus(_statusList, 8, ReferenceStatusTypes.ABU, Now, False)

                        'LLD()
                        AddStatus(_statusList, 9, ReferenceStatusTypes.LLD, Now, False)

                        'ATER()
                        AddStatus(_statusList, 10, ReferenceStatusTypes.ATER, Now, False)

                        'CAD()
                        AddStatus(_statusList, 11, ReferenceStatusTypes.CAD, Now, False)

                        'ENP()
                        AddStatus(_statusList, 12, ReferenceStatusTypes.ENP, Now, False)

                        'ENT()
                        AddStatus(_statusList, 13, ReferenceStatusTypes.ENT, Now, False)

                End Select

        End Select

        Return _statusList

    End Function

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

    End Sub

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

#Region "Enums"

#End Region

#Region "Attributes"

    Private _id As String

    Private _typeBL As Int32

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

    <DataMember, XmlAttribute>
    Property SupplierName As String

        Get

            Return _supplierName

        End Get

        Set(value As String)

            _supplierName = value

        End Set

    End Property

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
Public Class StatusItem

#Region "Attributes"

    Private _status As WSExtranetReferences.ReferenceStatusTypes

    Private _done As Boolean

    Private _lastDate As DateTime

    Private _orderNumber As Int32

#End Region

#Region "Builders"

    Sub New()

        _status = New WSExtranetReferences.ReferenceStatusTypes

        _status = WSExtranetReferences.ReferenceStatusTypes.UND

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
    Public Property StatusType As WSExtranetReferences.ReferenceStatusTypes

        Get

            Return _status

        End Get

        Set(value As WSExtranetReferences.ReferenceStatusTypes)

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

<DataContract()>
<XmlSerializerFormat>
Public Class Reference

#Region "Attributes"

    Private _id As String

    Private _modalityType As ModalityTypes

    Private _operationType As OperationTypes

    Private _managementCompanyId As ManagementCompanies

    Private _statusID As ReferenceStatusTypes

    Private _statusTag As String

    'Private _dateLastStatus As DateTime
    Private _dateLastStatus As String

    Private _customSection As CustomSections

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

        _operationType = OperationTypes.UND

        _managementCompanyId = ManagementCompanies.UND

        _statusID = ReferenceStatusTypes.UND

        _statusTag = Nothing

        '_dateLastStatus = Now
        _dateLastStatus = Nothing

        _customSection = CustomSections.UND

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

'<DataContract()>

<DataContract()>
<XmlSerializerFormat>
Public Class XMLSchemaObject

#Region "Attributes"

    Private _references As List(Of Reference)

    Private _TagWatcher As TagWatcher

    Private _schemaType As WSExtranetReferences.SchemaTypes

#End Region

#Region "Builders"

    Sub New()

        _references = New List(Of Reference)

        _TagWatcher = New TagWatcher

    End Sub

#End Region

#Region "Properties"

    <DataMember>
    Property SetXMLSchemaType As WSExtranetReferences.SchemaTypes

        Get

            Return _schemaType

        End Get

        Set(value As WSExtranetReferences.SchemaTypes)

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