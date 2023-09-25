Imports Gsol
Imports Gsol.BaseDatos.Operaciones
Imports System.Text
Imports System.Reflection
Imports System.IO
Imports System.ComponentModel
Imports System.Threading

Public Class TagBarControl

#Region "Attributes"

    Private _labels As List(Of Label) = New List(Of Label)

    Private _indexPath As Integer = 0

    Private _xAxis As Integer = 4

    Private _yAxis As Integer = 12

    Public Event AfterSelectedRows(ByVal Values As Dictionary(Of String, String))

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

    'Private _showKeyBox As Boolean

    Private _fieldDisplayValue As String

    Private _fieldKeyValue As String

    'Private _keyValue As String

    Private _system As Organismo

    Private _module As Organismo.Modulos

    Private _quantityOfResults As Int32

    Private _finderModal As TextBoxFinderModals

    'Private _GUIDModule As String

    Private _moduleDirect As String

    Private _freeClausules As String

    Private _keepLaunch As Boolean

    'Private _permissionNumber As String

    'Private _dataType As ICaracteristica.TiposCaracteristica

    Private _modeKeyField As KeyFieldModes

    Private _controlMode As ModalTypes = ModalTypes.Undefined

    'Private _tbTextLeft As Int32 = 47

#End Region

#Region "Properties"

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
    <System.ComponentModel.Description("Write Display Field for TagBarControl, if null, we will use default values")> _
    Public Property DisplayField As String
        Set(value As String)

            _fieldDisplayValue = value

        End Set
        Get
            Return _fieldDisplayValue
        End Get
    End Property


    <System.ComponentModel.Category("Wma")> _
    <System.ComponentModel.Description("Write KeyValue Field for TagBarControl, if null, we will use default values")> _
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

#Region "Builders"

    Sub New()

        InitializeComponent()

        _iOperationsCatalog = New OperacionesCatalogo

        '_showKeyBox = False

        _fieldDisplayValue = Nothing

        _fieldKeyValue = Nothing

        '_keyValue = Nothing

        _system = New Organismo

        _module = Organismo.Modulos.SinDefinir

        _quantityOfResults = 0

        _finderModal = TextBoxFinderModals.AdvancedFinderReadOnlyKey

        '_GUIDModule = Nothing

        _moduleDirect = Nothing

        _freeClausules = Nothing

        _keepLaunch = True

        '_permissionNumber = Nothing

        '_dataType = ICaracteristica.TiposCaracteristica.cUndefined

        _modeKeyField = KeyFieldModes.AutoPresentationTableKeyField

    End Sub

#End Region

#Region "Methods"

    Private Sub GroupBoxButton_Click(sender As Object, e As EventArgs) Handles GroupBoxButton.Click

        'PrepareLabelInMemory("Krombase")

        'Exit Sub

        _controlMode = ModalTypes.BrowsingModal

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


                                        'tbTextBox.Clear()

                                        'tbKey.Clear()

                                        'tbTextBox.Text = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldDisplayValue, _
                                        'IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                                        Dim labelTitle_ As String = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldDisplayValue, _
                                                                                                    IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)
                                        Dim labelKey_ As String = Nothing

                                        Select Case _modeKeyField

                                            Case KeyFieldModes.AutoPresentationTableKeyField

                                                'tbKey.Text = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldKeyValue, _
                                                'IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                                                labelKey_ = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                                            Case KeyFieldModes.ManualTechnicalKeyField

                                                'tbKey.Text = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldKeyValue, _
                                                'IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                                                labelKey_ = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)


                                        End Select

                                        PrepareLabelInMemory(labelTitle_, labelKey_)

                                        '_keyValue = labelKey 'tbKey.Text

                                    Else
                                        'tbTextBox.Text = ""

                                        '_keyValue = Nothing

                                        'tbKey.Text = Nothing

                                    End If
                                Else
                                    'tbTextBox.Text = ""

                                    '_keyValue = Nothing

                                    'tbKey.Text = Nothing

                                End If
                            Else

                                'tbTextBox.Text = ""

                                '_keyValue = Nothing

                                'tbKey.Text = Nothing

                            End If


                        End If

                    Catch ex As Exception

                        'tbTextBox.Text = ""

                        '_keyValue = Nothing

                    End Try

                Else

                    'tbTextBox.Text = Nothing

                    'tbKey.Text = Nothing

                End If

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

                            _system.GsDialogo("Seleccione un módulo válido { número GUID ó Nombre Directo de módulo}")

                        End If



                End If

            End If

        Else
            _system.GsDialogo("Faltan algunos parámetros para completar la funcionalidad")

        End If

        Return status_

    End Function

    Private Sub PrepareLabelInMemory(ByVal title_ As String, ByVal key_ As String)

        Dim label_ As Label = New Label()

        label_.ForeColor = Color.White

        label_.BackColor = Color.DarkGreen

        label_.Padding = New Padding(8, 8, 8, 8)

        label_.AutoSize = True

        label_.Visible = True

        label_.Image = My.Resources.remove

        '7 spacios
        label_.Text = title_ & "       "

        label_.Name = key_

        label_.TextAlign = ContentAlignment.MiddleLeft

        label_.ImageAlign = ContentAlignment.MiddleRight

        label_.Cursor = Cursors.Hand

        AddHandler label_.Click, AddressOf removeLabel

        _labels.Add(label_)

        drawLabels()

    End Sub

    Private Sub drawLabels()

        Dim values_ As Dictionary(Of String, String) = New Dictionary(Of String, String)

        _xAxis = 4

        _yAxis = 12

        Me.Height = 60

        _indexPath = 0

        For Each Label As Label In _labels

            values_.Add(Label.Name, Label.Text)

            Label.TabIndex = _indexPath

            _indexPath += 1

            If GroupBoxContainer.Width <= (_xAxis + (Label.Width + 4)) Then

                _yAxis += 40

                _xAxis = 4

                Me.Height = Me.Height + 40

            End If

            Label.Location = New Point(_xAxis, _yAxis)

            GroupBoxContainer.Controls.Add(Label)

            _xAxis += Label.Width + 4

        Next

        RaiseEvent AfterSelectedRows(values_)

    End Sub

    Private Sub removeLabel(ByVal sender As Label, ByVal e As System.EventArgs)

        _labels.RemoveAt(sender.TabIndex)

        sender.Dispose()

        drawLabels()

    End Sub

#End Region

End Class
