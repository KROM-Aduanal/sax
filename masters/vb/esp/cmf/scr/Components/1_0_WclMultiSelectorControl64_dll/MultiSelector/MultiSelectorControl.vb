Imports gsol
Imports gsol.BaseDatos.Operaciones
Imports System.Text

Public Class MultiSelectorControl

#Region "Enums"

    Public Enum KeyFieldModes

        AutoPresentationTableKeyField = 0

        ManualTechnicalKeyField = 2

    End Enum

#End Region

#Region "Attributes"

    Private _form As ShadowedForm

    Private _selectedItems As List(Of String)

    Private _isDroppedDown As Boolean

    Private _iOperationsCatalog As IOperacionesCatalogo

    Private _fieldDisplayValue As String

    Private _fieldKeyValue As String

    Private _system As Organismo

    Private _module As Organismo.Modulos

    Private _moduleDirect As String

    Private _freeClausules As String

    Private _modeKeyField As KeyFieldModes

    Private _baseResults As DataTable

    Private _selectedKeys As List(Of String)

    Private _outFocus As Boolean

#End Region

#Region "Properties"

    <System.ComponentModel.Category("Wma")> _
    <System.ComponentModel.Description("Component selected items")> _
    Public WriteOnly Property SelectedItems() As List(Of String)
        Set(ByVal value As List(Of String))

            _selectedItems = value

            InitializeCatalog()

            MultiSelectorInput.Text = GetSelectedItems()

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
    <System.ComponentModel.Description("Free Clausules")> _
    Public WriteOnly Property FreeClausules As String

        Set(value As String)

            _freeClausules = value
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
    <System.ComponentModel.Description("Write Display Field for Component, if null, we will use default values")> _
    Public Property DisplayField As String
        Set(value As String)

            _fieldDisplayValue = value

        End Set
        Get
            Return _fieldDisplayValue
        End Get
    End Property


    <System.ComponentModel.Category("Wma")> _
    <System.ComponentModel.Description("Write KeyValue Field for Component, if null, we will use default values")> _
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

#End Region

#Region "Events"

    Public Event AfterSelectedItems(ByVal values As List(Of String))

#End Region

#Region "Builders"

    Public Sub New()

        InitializeComponent()

        _iOperationsCatalog = New OperacionesCatalogo

        _fieldDisplayValue = Nothing

        _fieldKeyValue = Nothing

        _system = New Organismo

        _module = Organismo.Modulos.SinDefinir

        _moduleDirect = Nothing

        _freeClausules = Nothing

        _modeKeyField = KeyFieldModes.AutoPresentationTableKeyField

        _selectedItems = New List(Of String)

        _selectedKeys = New List(Of String)

        _outFocus = False

        InitializeNew()

        AddHandler MultiSelectorFinder.LostFocus, AddressOf Multiselector_LostFocus

        AddHandler MultiSelectorDropDown.MouseLeave, AddressOf Multiselector_MouseLeave

        AddHandler MultiSelectorDropDown.MouseEnter, AddressOf Multiselector_MouseEnter

        AddHandler MultiSelectorButton.MouseEnter, AddressOf Multiselector_MouseEnter

        AddHandler MultiSelectorButton.MouseLeave, AddressOf Multiselector_MouseLeave


    End Sub

    Private Sub InitializeNew()

        _isDroppedDown = False

        MultiSelectorInput.Text = Nothing

        MultiSelectorDropDown.Hide()

        MultiSelectorFinder.Hide()

        _form = New ShadowedForm

        With _form

            .ShowInTaskbar = False

            .FormBorderStyle = FormBorderStyle.None

            .ControlBox = False

            .StartPosition = FormStartPosition.Manual

            .TopMost = True

            .Location = MultiSelectorDropDown.Location

            .Width = Me.Width

            .BackColor = Color.LightGray

            .Controls.Add(MultiSelectorDropDown)

            .Controls.Add(MultiSelectorFinder)

        End With

        MultiSelectorFinder.BackColor = Color.LightGray

        SetSize()

    End Sub

    Private Sub InitializeCatalog()

        If Me.VerifyParameters() Then

            Dim iOperacionAuxiliar_ As IOperacionesCatalogo = New OperacionesCatalogo

            If Not _module = Organismo.Modulos.SinDefinir Then

                iOperacionAuxiliar_ = _system.EnsamblaModulo(_module).Clone

            Else
                If Not _moduleDirect Is Nothing Then

                    iOperacionAuxiliar_ = _system.EnsamblaModulo(_moduleDirect).Clone

                Else
                    _system.GsDialogo("Debe eligir un módulo válido, coloque un valor apropiado para el campo NameAsKey")

                End If

            End If

            iOperacionAuxiliar_.EspacioTrabajo = _iOperationsCatalog.EspacioTrabajo

            iOperacionAuxiliar_.PreparaCatalogo()

            iOperacionAuxiliar_.ClausulasLibres = Nothing

            iOperacionAuxiliar_.ClausulasLibres = _freeClausules

            iOperacionAuxiliar_.GenerarVista()

            If _system.TieneResultados(iOperacionAuxiliar_) Then

                _baseResults = iOperacionAuxiliar_.Vista.Tables(0)

                _iOperationsCatalog = iOperacionAuxiliar_

                PrepareCheckList()

            End If

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

                    If Not _moduleDirect Is Nothing Then

                        Dim iOperacionAuxiliar_ As IOperacionesCatalogo = New OperacionesCatalogo

                        iOperacionAuxiliar_ = _system.EnsamblaModulo(_moduleDirect)

                        _fieldDisplayValue = Nothing

                        _fieldKeyValue = iOperacionAuxiliar_.IdentificadorCatalogo

                        status_ = True

                    Else

                        _system.GsDialogo("Seleccione un módulo válido { Nombre Directo de módulo}")

                    End If



                End If

            End If

        Else
            _system.GsDialogo("Faltan algunos parámetros para completar la funcionalidad")

        End If

        Return status_

    End Function

#End Region

#Region "Methods"

    Private Sub PrepareCheckList()

        Dim items_ As List(Of String) = New List(Of String)

        For indexPath_ As Integer = 0 To _baseResults.Rows.Count - 1

            Select Case _modeKeyField

                Case KeyFieldModes.AutoPresentationTableKeyField

                    Dim key_ As String = IOperations.Vista(indexPath_, _fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                    If _selectedKeys.Count > 0 Then

                        If _selectedKeys.Contains(key_) Then

                            items_.Add(IOperations.Vista(indexPath_, _fieldDisplayValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado))

                        End If

                    Else

                        items_.Add(IOperations.Vista(indexPath_, _fieldDisplayValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado))

                    End If

                Case KeyFieldModes.ManualTechnicalKeyField

                    Dim key_ As String = IOperations.Vista(indexPath_, _fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                    If _selectedKeys.Count > 0 Then

                        If _selectedKeys.Contains(key_) Then

                            items_.Add(IOperations.Vista(indexPath_, _fieldDisplayValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico))

                        End If

                    Else

                        items_.Add(IOperations.Vista(indexPath_, _fieldDisplayValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico))

                    End If

            End Select

        Next

        MultiSelectorDropDown.DataSource = items_

        For Index_ As Integer = 0 To MultiSelectorDropDown.Items.Count - 1

            Dim value_ As String = FindItemWithFieldValue(MultiSelectorDropDown.Items(Index_).ToString)

            If _selectedItems.Contains(value_) Then

                MultiSelectorDropDown.SetItemChecked(Index_, True)

            Else

                MultiSelectorDropDown.SetItemChecked(Index_, False)

            End If

        Next

    End Sub

    Private Sub MultiselectorDropDown_ItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs) Handles MultiSelectorDropDown.ItemCheck

        Dim selectedItems_ As List(Of String) = _selectedItems

        Dim value_ As String = FindItemWithFieldValue(MultiSelectorDropDown.Items(e.Index).ToString)

        If e.NewValue = 1 Then

            If Not selectedItems_.Contains(value_) Then

                selectedItems_.Add(value_)

            End If

        Else

            selectedItems_.Remove(value_)

        End If

        _selectedItems = selectedItems_

        MultiSelectorFinder.Focus()

        MultiSelectorInput.Text = GetSelectedItems()

    End Sub

    Private Sub MultiSelectorButton_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MultiSelectorButton.MouseDown

        ListButtonClick()

    End Sub

    Private Sub MultiselectorFinder_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MultiSelectorFinder.KeyPress

        If MultiSelectorFinder.Text.Length >= 2 Then

            Dim value_ As String = MultiSelectorFinder.Text

            For indexPath_ As Integer = 0 To _baseResults.Rows.Count - 1

                Dim keyValue_ As String = Nothing

                Dim fieldValue_ As String = Nothing

                Select Case _modeKeyField

                    Case KeyFieldModes.AutoPresentationTableKeyField

                        keyValue_ = IOperations.Vista(indexPath_, _fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                        fieldValue_ = IOperations.Vista(indexPath_, _fieldDisplayValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                    Case KeyFieldModes.ManualTechnicalKeyField

                        keyValue_ = IOperations.Vista(indexPath_, _fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        fieldValue_ = IOperations.Vista(indexPath_, _fieldDisplayValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                End Select

                If fieldValue_.ToLower().IndexOf(value_.ToLower()) >= 0 Then

                    _selectedKeys.Add(keyValue_)

                End If

            Next

            PrepareCheckList()

        Else

            _selectedKeys = New List(Of String)

            PrepareCheckList()

        End If

    End Sub

    Private Function FindItemWithFieldValue(ByVal findString_) As String

        For indexPath_ As Integer = 0 To _baseResults.Rows.Count - 1

            Dim key_ As String = Nothing

            Dim value_ As String = Nothing

            Select Case _modeKeyField

                Case KeyFieldModes.AutoPresentationTableKeyField

                    key_ = IOperations.Vista(indexPath_, _fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                    value_ = IOperations.Vista(indexPath_, _fieldDisplayValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)


                Case KeyFieldModes.ManualTechnicalKeyField

                    key_ = IOperations.Vista(indexPath_, _fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                    value_ = IOperations.Vista(indexPath_, _fieldDisplayValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

            End Select

            If findString_ = value_ Then

                Return key_

                Exit For

            End If

        Next

        Return Nothing

    End Function

    Private Sub ListButtonClick()

        If _isDroppedDown Then

            _isDroppedDown = False

            MultiSelectorDropDown.Hide()

            MultiSelectorFinder.Hide()

            _form.Hide()

            RaiseEvent AfterSelectedItems(_selectedItems)

        Else

            InitializeCatalog()

            _isDroppedDown = True

            SetSize()

            _form.Show()

            MultiSelectorDropDown.Show()

            MultiSelectorFinder.Show()

            MultiSelectorFinder.Focus()

        End If

    End Sub

    Private Sub Multiselector_MouseLeave(ByVal sender As Object, ByVal e As EventArgs)

        _outFocus = True

    End Sub

    Private Sub Multiselector_MouseEnter(ByVal sender As Object, ByVal e As EventArgs)

        _outFocus = False

    End Sub

    Private Sub Multiselector_LostFocus(ByVal sender As Object, ByVal e As EventArgs)

        If _outFocus = True Then

            MultiSelectorDropDown.Hide()

            MultiSelectorFinder.Hide()

            _form.Hide()

            RaiseEvent AfterSelectedItems(_selectedItems)

            _isDroppedDown = False

        End If

    End Sub

    Private Sub SetList()

        Dim oForm_ As Form

        Dim oRectangle_ As Rectangle

        Dim oPoint_ As Point

        If _form IsNot Nothing Then

            _form.Height = MultiSelectorDropDown.Height + MultiSelectorFinder.Height + 8

            MultiSelectorDropDown.Top = 0

            MultiSelectorFinder.Top = MultiSelectorDropDown.Height + 4

            oForm_ = Me.FindForm

            If oForm_ IsNot Nothing Then

                oPoint_ = Me.ParentForm.PointToClient(Me.PointToScreen(Point.Empty))

                oPoint_.Y = oPoint_.Y + Me.MultiSelectorInput.Height

                oRectangle_ = oForm_.RectangleToScreen(oForm_.ClientRectangle)

                oPoint_.X = oPoint_.X + oRectangle_.Left

                oPoint_.Y = oPoint_.Y + oRectangle_.Top

                _form.Location = oPoint_

            End If

            _form.Width = MultiSelectorDropDown.Width

        End If
    End Sub

    Private Sub SetSize()

        MultiSelectorInput.Width = Me.Width

        MultiSelectorButton.Left = MultiSelectorInput.Width - MultiSelectorButton.Width

        MultiSelectorDropDown.Width = Me.Width

        MultiSelectorFinder.Width = MultiSelectorDropDown.Width

        Me.Height = MultiSelectorInput.Height

        SetList()

    End Sub

    Private Function GetSelectedItems() As String

        Dim value_ As String = Nothing

        Dim key_ As String = Nothing

        Dim selectedItems_ As List(Of String) = New List(Of String)()

        If Not _baseResults Is Nothing Then

            For indexPath_ As Integer = 0 To _baseResults.Rows.Count - 1

                Select Case _modeKeyField

                    Case KeyFieldModes.AutoPresentationTableKeyField

                        value_ = IOperations.Vista(indexPath_, _fieldDisplayValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                        key_ = IOperations.Vista(indexPath_, _fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                    Case KeyFieldModes.ManualTechnicalKeyField

                        value_ = IOperations.Vista(indexPath_, _fieldDisplayValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                        key_ = IOperations.Vista(indexPath_, _fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                End Select

                If _selectedItems.Contains(key_) Then

                    selectedItems_.Add(value_)

                End If

            Next

            Return String.Join(",", selectedItems_)

        End If

    End Function

    Private Sub MultiSelectorFinder_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MultiSelectorFinder.GotFocus

        If MultiSelectorFinder.Text = "Buscar" Then
            MultiSelectorFinder.ForeColor = Color.Black
            MultiSelectorFinder.Text = ""
        End If

    End Sub

    Private Sub MultiSelectorFinder_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MultiSelectorFinder.LostFocus

        If MultiSelectorFinder.Text = Nothing Then
            MultiSelectorFinder.ForeColor = Color.Gray
            MultiSelectorFinder.Text = "Buscar"
        End If

    End Sub

#End Region

End Class

Public Class ShadowedForm
    Inherits Form

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Const CS_DROPSHADOW As Integer = &H20000
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ClassStyle = cp.ClassStyle Or CS_DROPSHADOW
            Return cp
        End Get
    End Property

End Class