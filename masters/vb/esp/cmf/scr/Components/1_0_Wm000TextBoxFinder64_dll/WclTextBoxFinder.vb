Imports gsol.BaseDatos.Operaciones
Imports System.Text
Imports System.Reflection
Imports System.IO
Imports System.ComponentModel
Imports System.Threading

Public Class WclTextBoxFinder

#Region "Attributes"

    Public Enum TextBoxFinderModals

        SimpleFinder = 0

        AdvancedFinder

        AdvancedFinderNoKey

        AdvancedFinderReadOnlyKey

    End Enum

    Public Enum KeyFieldModes

        AutoPresentationTableKeyField = 0

        ManualTechnicalKeyField = 2

    End Enum

    Public Enum ModalTypes

        Undefined = 0

        KeyModal = 1

        BrowsingModal = 2

    End Enum

    Private _iOperationsCatalog As IOperacionesCatalogo

    Private _showKeyBox As Boolean

    Private _fieldDisplayValue As String

    Private _fieldKeyValue As String

    Private _keyValue As String

    Private _system As Organismo

    Private _module As Organismo.Modulos

    Private _quantityOfResults As Int32

    Private _finderModal As TextBoxFinderModals

    Private _GUIDModule As String

    Private _moduleDirect As String

    Private _freeClausules As String

    Private _keepLaunch As Boolean

    Private _permissionNumber As String

    Private _dataType As ICaracteristica.TiposCaracteristica

    Private _modeKeyField As KeyFieldModes

    Private _controlMode As ModalTypes = ModalTypes.Undefined

    Private _tbTextLeft As Int32 = 47

#End Region

#Region "Events"

    Public Event WhenOperatorResponse() 'Implements IOperationsDynamicForm.AfterCloseDynamicForm

    Public Event AfterClickForAdvancedSearch() 'Implements IOperationsDynamicForm.AfterCloseDynamicForm

    Public Event AfterClickForSimpleSearch() 'Implements IOperationsDynamicForm.AfterCloseDynamicForm

    Public Event BeforeClickForAdvancedSearch() 'Implements IOperationsDynamicForm.OnClickAccept

    Public Event BeforeClickForSimpleSearch() 'Implements IOperationsDynamicForm.OnClickAccept

    Public Event tbfTextBox_TextChanged()

    Public Event tbfKey_TextChanged()

    Public Event tbfTextBox_Validated()

    Public Event tbfKey_Validated()

#End Region

#Region "Builders"

    Sub New()

        InitializeComponent()

        _iOperationsCatalog = New OperacionesCatalogo

        _showKeyBox = False

        _fieldDisplayValue = Nothing

        _fieldKeyValue = Nothing

        _keyValue = Nothing

        _system = New Organismo

        _module = Organismo.Modulos.SinDefinir

        _quantityOfResults = 0

        _finderModal = TextBoxFinderModals.AdvancedFinderReadOnlyKey

        tbTextBox.Left = _tbTextLeft

        btnSearch.Left = tbKey.Width + tbTextBox.Width + 4

        _GUIDModule = Nothing

        _moduleDirect = Nothing

        _freeClausules = Nothing

        _keepLaunch = True

        _permissionNumber = Nothing

        _dataType = ICaracteristica.TiposCaracteristica.cUndefined

        _modeKeyField = KeyFieldModes.AutoPresentationTableKeyField

    End Sub

    Private Sub ChangeUserInterface(ByVal finderModal_ As TextBoxFinderModals)

        Select Case finderModal_

            Case TextBoxFinderModals.AdvancedFinder
                tbKey.Visible = True
                tbKey.ReadOnly = False
                tbKey.Enabled = True
                tbKey.Show()

                tbKey.ForeColor = Color.Black
                btnSearch.Visible = True

                tbTextBox.Left = _tbTextLeft
                btnSearch.Left = tbKey.Width + tbTextBox.Width + 4

            Case TextBoxFinderModals.AdvancedFinderNoKey
                tbKey.Visible = False
                tbKey.ReadOnly = True
                tbKey.Enabled = False
                tbKey.Hide()
                tbKey.ForeColor = Color.Black
                btnSearch.Visible = True

                tbTextBox.Left = 2
                btnSearch.Left = tbTextBox.Width + 4

            Case TextBoxFinderModals.AdvancedFinderReadOnlyKey
                tbKey.Visible = True
                tbKey.ReadOnly = True
                tbKey.Enabled = False
                tbKey.Show()
                tbKey.ForeColor = Color.Black
                btnSearch.Visible = True
                tbTextBox.Left = _tbTextLeft
                btnSearch.Left = tbKey.Width + tbTextBox.Width + 4

            Case TextBoxFinderModals.SimpleFinder

                tbKey.Visible = True
                tbKey.ReadOnly = False
                tbKey.Enabled = True
                tbKey.Show()
                tbKey.ForeColor = Color.Black
                btnSearch.Visible = False

                tbTextBox.Left = _tbTextLeft
                btnSearch.Left = tbKey.Width + tbTextBox.Width + 4

            Case Else


        End Select

    End Sub

#End Region

#Region "Properties"

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Get last control modal")> _
    Public Property ControlMode As ModalTypes
        Get
            Return _controlMode
        End Get

        Set(value As ModalTypes)

            _controlMode = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Set left value for tbKeyLeft")> _
    Public Property tbKeyLeft As Int32
        Get

            Return tbKey.Left
        End Get

        Set(value As Int32)

            tbKey.Left = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Set left value for tbText")> _
    Public Property tbTextLeft As Int32
        Get
            Return tbTextBox.Left
        End Get

        Set(value As Int32)

            _tbTextLeft = value
            tbTextBox.Left = _tbTextLeft

            'tbTextBox.Left = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Set Width value for tbKey")> _
    Public Property tbKeyWidth As Int32
        Get

            Return tbKey.Width

        End Get

        Set(value As Int32)

            tbKey.Width = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Set Width value for tbTextWidth")> _
    Public Property tbTextWidth As Int32
        Get
            Return tbTextBox.Width
        End Get

        Set(value As Int32)

            tbTextBox.Width = value

        End Set

    End Property





    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Set mode to use Queries throuh KeyField")> _
    Public Property ModeKeyField As KeyFieldModes
        Get
            Return _modeKeyField
        End Get

        Set(value As KeyFieldModes)

            _modeKeyField = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Set data type for KeyValue")> _
    Public Property KeyValueDataType As ICaracteristica.TiposCaracteristica
        Get
            Return _dataType
        End Get

        Set(value As ICaracteristica.TiposCaracteristica)

            _dataType = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Set the permission number to start queries")> _
    Public Property PermissionNumber As String
        Get
            Return _permissionNumber
        End Get

        Set(value As String)

            _permissionNumber = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Keep launch as boolean")> _
    Public Property KeepLaunch As String
        Get
            Return _keepLaunch
        End Get

        Set(value As String)

            _keepLaunch = value

        End Set

    End Property


    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Free Clausules")> _
    Public WriteOnly Property FreeClausules As String

        Set(value As String)

            _freeClausules = value
        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Module Title")> _
    Public Property Title As String
        Get

            Return lblTitle.Text

        End Get

        Set(value As String)

            If value Is Nothing Or value = "" Then
                lblTitle.Visible = False
                lblTitle.Hide()
                lblTitle.Text = value
            Else
                lblTitle.Visible = True
                lblTitle.Show()
                lblTitle.Text = value
            End If

        End Set
    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Module NameAsKey")> _
    Public Property NameAsKey As String
        Get
            Return _moduleDirect

        End Get
        Set(value As String)
            _moduleDirect = value

        End Set
    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Module GUID")> _
    Public Property GUID As String
        Get
            Return _GUIDModule

        End Get
        Set(value As String)
            _GUIDModule = value

        End Set
    End Property


    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Module Name")> _
    Public Property WorkingModule As Organismo.Modulos
        Get
            Return _module

        End Get
        Set(value As Organismo.Modulos)
            _module = value

        End Set
    End Property


    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Choose modal")> _
    Property SetModalFinder As TextBoxFinderModals
        Get
            Return _finderModal
        End Get

        Set(value As TextBoxFinderModals)
            ChangeUserInterface(value)
            _finderModal = value

        End Set
    End Property


    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Write Display Field for TextBox, if null, we will use default values")> _
    Public Property DisplayField As String
        Set(value As String)

            _fieldDisplayValue = value

        End Set
        Get
            Return _fieldDisplayValue
        End Get
    End Property


    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Write KeyValue Field for TextBox, if null, we will use default values")> _
    Public Property KeyField As String
        Set(value As String)

            _fieldKeyValue = value

        End Set

        Get
            Return _fieldKeyValue

        End Get
    End Property


    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Asign Current Session through IOperations")> _
    Public Property IOperations As IOperacionesCatalogo
        Get
            Return _iOperationsCatalog

        End Get
        Set(value As IOperacionesCatalogo)
            _iOperationsCatalog = value

        End Set
    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Get Current Key Value")> _
    Public ReadOnly Property GetKeyValue As String
        Get

            Return _keyValue

        End Get
    End Property

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Set new Key Value")> _
    Public WriteOnly Property SetKeyValue As String

        Set(value As String)

            _keyValue = value

            Me.tbKey.Text = _keyValue

            If Not _keyValue Is Nothing And Trim(_keyValue) <> "" Then

                _controlMode = ModalTypes.KeyModal

                RaiseEvent BeforeClickForSimpleSearch()

                FindMyKey()

                RaiseEvent WhenOperatorResponse()

                RaiseEvent AfterClickForSimpleSearch()

            End If

        End Set

    End Property

#End Region

#Region "Methods"

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        _controlMode = ModalTypes.BrowsingModal

        RaiseEvent BeforeClickForAdvancedSearch()

        If _keepLaunch Then

            If Me.VerifyParameters() Then

                _iOperationsCatalog.PreparaCatalogo()

                _iOperationsCatalog.ClausulasLibres = Nothing

                _iOperationsCatalog.ClausulasLibres = _freeClausules

                Dim clausulalibre_ As StringBuilder = New StringBuilder()

                If _module = Organismo.Modulos.SinDefinir Then

                    _iOperationsCatalog = _system.CargaModuloVirtual(_moduleDirect, _
                                                                    _iOperationsCatalog)
                Else

                    _iOperationsCatalog = _system.CargaModuloVirtual(_module.ToString, _
                                                                    _iOperationsCatalog)

                End If

                If _iOperationsCatalog.Caracteristicas.Count > 0 Then

                    _quantityOfResults = _iOperationsCatalog.Caracteristicas.Count

                    Try
                        If Not _fieldDisplayValue Is Nothing Then

                            If Not _iOperationsCatalog.Vista.Tables Is Nothing Then
                                If _iOperationsCatalog.Vista.Tables.Count >= 1 Then
                                    If _iOperationsCatalog.Vista.Tables(0).Rows.Count >= 1 Then


                                        tbTextBox.Clear()

                                        tbKey.Clear()

                                        tbTextBox.Text = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldDisplayValue, _
                                                                                                    IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                                        Select Case _modeKeyField

                                            Case KeyFieldModes.AutoPresentationTableKeyField

                                                tbKey.Text = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldKeyValue, _
                                                                    IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                                            Case KeyFieldModes.ManualTechnicalKeyField

                                                tbKey.Text = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldKeyValue, _
                                                                            IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                                        End Select

                                        _keyValue = tbKey.Text

                                    Else
                                        tbTextBox.Text = ""

                                        _keyValue = Nothing

                                        tbKey.Text = Nothing

                                    End If
                                Else
                                    tbTextBox.Text = ""

                                    _keyValue = Nothing

                                    tbKey.Text = Nothing

                                End If
                            Else

                                tbTextBox.Text = ""

                                _keyValue = Nothing

                                tbKey.Text = Nothing

                            End If



                        End If

                    Catch ex As Exception

                        tbTextBox.Text = ""

                        _keyValue = Nothing





                    End Try

                Else

                    tbTextBox.Text = Nothing

                    '08/09/2017
                    tbKey.Text = Nothing

                End If

            Else

                _system.GsDialogo("Faltan algunos parámetros para completar la funcionalidad")

            End If

            RaiseEvent WhenOperatorResponse()

            RaiseEvent AfterClickForAdvancedSearch()

        End If


    End Sub

    Private Sub FindMyKey()

        'MsgBox(".")

        If Me.VerifyParameters() Then

            Dim iOperacionAuxiliar_ As IOperacionesCatalogo = New OperacionesCatalogo

            If Not _module = Organismo.Modulos.SinDefinir Then

                iOperacionAuxiliar_ = _system.EnsamblaModulo(_module)

            Else
                If Not _moduleDirect Is Nothing Then

                    iOperacionAuxiliar_ = _system.EnsamblaModulo(_moduleDirect)

                    'If iOperacionAuxiliar_.IdentificadorEmpresa = "1" Then
                    '    FreeClausules = " and i_Cve_DivisionMiEmpresa = " & iOperacionAuxiliar_.EspacioTrabajo.MisCredenciales.DivisionEmpresaria

                    'End If

                Else
                    _system.GsDialogo("Debe eligir un módulo válido, coloque un valor apropiado para el campo NameAsKey")

                End If

            End If

            iOperacionAuxiliar_.EspacioTrabajo = _iOperationsCatalog.EspacioTrabajo

            iOperacionAuxiliar_.PreparaCatalogo()

            iOperacionAuxiliar_.ClausulasLibres = Nothing

            'iOperacionAuxiliar_.ClausulasLibres = " and " & iOperacionAuxiliar_.IdentificadorCatalogo & " = " & Me.tbKey.Text & _freeClausules

            Select Case _modeKeyField

                Case KeyFieldModes.AutoPresentationTableKeyField

                    Select Case _dataType
                        Case ICaracteristica.TiposCaracteristica.cDateTime, ICaracteristica.TiposCaracteristica.cString

                            iOperacionAuxiliar_.ClausulasLibres = " and " & iOperacionAuxiliar_.IdentificadorCatalogo & " = '" & Trim(Me.tbKey.Text) & "' " & _freeClausules

                        Case Else

                            iOperacionAuxiliar_.ClausulasLibres = " and " & iOperacionAuxiliar_.IdentificadorCatalogo & " = " & Me.tbKey.Text & _freeClausules

                    End Select


                Case KeyFieldModes.ManualTechnicalKeyField

                    Select Case _dataType

                        Case ICaracteristica.TiposCaracteristica.cDateTime, ICaracteristica.TiposCaracteristica.cString

                            iOperacionAuxiliar_.ClausulasLibres = " and " & _fieldKeyValue & " = '" & Trim(Me.tbKey.Text) & "' " & _freeClausules

                            'tbKey.Text = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                        Case Else

                            iOperacionAuxiliar_.ClausulasLibres = " and " & _fieldKeyValue & " = " & Trim(Me.tbKey.Text) & " " & _freeClausules

                    End Select

            End Select

            iOperacionAuxiliar_.GenerarVista()

            'If _system.TieneResultados(iOperacionAuxiliar_) Then

            If iOperacionAuxiliar_.Vista.Tables.Count > 0 Then

                If iOperacionAuxiliar_.Vista.Tables(0).Rows.Count > 0 Then

                    tbTextBox.Text = iOperacionAuxiliar_.Vista.Tables(0).Rows(0)(_fieldDisplayValue)

                    '_keyValue = iOperacionAuxiliar_.Vista.Tables(0).Rows(0)(_fieldKeyValue)
                    Select Case _modeKeyField

                        Case KeyFieldModes.AutoPresentationTableKeyField

                            _keyValue = iOperacionAuxiliar_.Vista(0, _fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                        Case KeyFieldModes.ManualTechnicalKeyField

                            _keyValue = iOperacionAuxiliar_.Vista(0, _fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                    End Select

                    _iOperationsCatalog = iOperacionAuxiliar_

                Else

                    tbTextBox.Clear()

                    tbKey.Clear()

                    _keyValue = Nothing

                    _iOperationsCatalog = iOperacionAuxiliar_

                End If

            Else

                tbTextBox.Text = Nothing

                tbKey.Clear()

                _keyValue = Nothing

            End If

            'Else

            '    tbTextBox.Text = Nothing

            '    tbKey.Clear()

            '    _keyValue = Nothing

            'End If

        Else

            _system.GsDialogo("Faltan algunos parámetros para completar la funcionalidad")

        End If

    End Sub



    Private Function VerifyParameters() As Boolean

        Dim status_ As Boolean = False

        If Not _iOperationsCatalog.EspacioTrabajo.MisCredenciales Is Nothing Then

            If Not (_fieldDisplayValue Is Nothing And _fieldKeyValue Is Nothing) Then

                status_ = True

            Else

                If _module <> Organismo.Modulos.SinDefinir Then

                    Dim iOperacionAuxiliar_ As IOperacionesCatalogo = New OperacionesCatalogo

                    iOperacionAuxiliar_ = _system.EnsamblaModulo(_module)

                    _fieldDisplayValue = Nothing

                    _fieldKeyValue = iOperacionAuxiliar_.IdentificadorCatalogo

                    status_ = True

                Else

                    If Not _GUIDModule Is Nothing Then
                        'NOT IMPLEMENTED YET

                        status_ = True

                    Else

                        If Not _moduleDirect Is Nothing Then

                            Dim iOperacionAuxiliar_ As IOperacionesCatalogo = New OperacionesCatalogo

                            iOperacionAuxiliar_ = _system.EnsamblaModulo(_moduleDirect)

                            _fieldDisplayValue = Nothing

                            _fieldKeyValue = iOperacionAuxiliar_.IdentificadorCatalogo

                            status_ = True

                        Else

                            _system.GsDialogo("Seleccione un módulo válido { número GUID ó Nombre Directo de módulo}")

                        End If

                    End If

                End If

            End If

        Else
            _system.GsDialogo("Faltan algunos parámetros para completar la funcionalidad")

        End If

        Return status_

    End Function

#End Region

    Private Sub tbKey_Leave(sender As Object, e As EventArgs) Handles tbKey.Leave

        _controlMode = ModalTypes.KeyModal

        If Trim(tbKey.Text) <> "" And Not tbKey.Text Is Nothing Then

            If Not _permissionNumber Is Nothing Then

                If _iOperationsCatalog.EspacioTrabajo.BuscaPermiso(_permissionNumber, IEspacioTrabajo.TipoModulo.Abstracto) Then

                    RaiseEvent BeforeClickForSimpleSearch()


                    If _keepLaunch Then

                        FindMyKey()

                    End If

                    RaiseEvent WhenOperatorResponse()

                    RaiseEvent AfterClickForSimpleSearch()

                Else

                    _system.GsDialogo("Permission number {" & _permissionNumber & "}, does not exist in current user session")

                End If

            Else

                _system.GsDialogo("Please set a permission number to try launch queries, value can't be void")

            End If


        Else

            tbTextBox.Clear()

        End If

    End Sub

    Private Sub tbTextBox_TextChanged(sender As Object, e As EventArgs) Handles tbTextBox.TextChanged
        RaiseEvent tbfTextBox_TextChanged()
    End Sub


    Private Sub tbKey_TextChanged(sender As Object, e As EventArgs) Handles tbKey.TextChanged
        RaiseEvent tbfKey_TextChanged()
    End Sub

    Private Sub tbTextBox_Validated(sender As Object, e As EventArgs) Handles tbTextBox.Validated
        RaiseEvent tbfTextBox_Validated()
    End Sub

    Private Sub tbKey_Validated(sender As Object, e As EventArgs) Handles tbKey.Validated
        RaiseEvent tbfKey_Validated()
    End Sub
End Class


<TypeConverter(GetType(ExpandableObjectConverter))> _
Public Class CharacteristicsField

    Private _iCharacteristic As ICaracteristica

    Sub New()

        _iCharacteristic = New CaracteristicaCatalogo

    End Sub

    Public ReadOnly Property Characteristic As ICaracteristica
        Get
            Return _iCharacteristic
        End Get
    End Property

    <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
    Property [KeyField]() As String
        Get
            Return _iCharacteristic.Nombre
        End Get
        Set(ByVal Value As String)
            _iCharacteristic.Nombre = Value
        End Set
    End Property

    <DefaultValue(""), NotifyParentProperty(True), RefreshProperties(RefreshProperties.Repaint)> _
    Property [DisplayField]() As String
        Get
            Return _iCharacteristic.NombreMostrar
        End Get
        Set(ByVal Value As String)
            _iCharacteristic.NombreMostrar = Value
        End Set
    End Property

End Class

