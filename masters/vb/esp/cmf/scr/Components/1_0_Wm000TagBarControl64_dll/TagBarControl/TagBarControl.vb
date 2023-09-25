Imports gsol
Imports gsol.BaseDatos.Operaciones
Imports System.Text
Imports System.Reflection
Imports System.IO
Imports System.ComponentModel
Imports System.Threading

Public Class TagBarControl

#Region "Enums"

    Public Enum KeyFieldModes

        AutoPresentationTableKeyField = 0

        ManualTechnicalKeyField = 2

    End Enum

    Public Enum ModalTypes

        Undefined = 0

        KeyModal = 1

        BrowsingModal = 2

    End Enum

    Public Enum ControlSize
        '48
        Small = 72

        Medium = 96

        Large = 120

    End Enum

#End Region

#Region "Attributes"

    Private _values As List(Of String)

    Private _labels As List(Of Label)

    Private _indexPath As Integer = 0

    Private _xAxis As Integer = 0

    Private _yAxis As Integer = 0

    Private _lHeight As Integer = 0

    Private _baseHeight As Integer = 0

    Private _iOperationsCatalog As IOperacionesCatalogo

    Private _fieldDisplayValue As String

    Private _fieldKeyValue As String

    Private _system As Organismo

    Private _module As Organismo.Modulos

    Private _quantityOfResults As Int32

    Private _moduleDirect As String

    Private _freeClausules As String

    Private _keepLaunch As Boolean

    Private _modeKeyField As KeyFieldModes

    Private _controlMode As ModalTypes = ModalTypes.Undefined

#End Region

#Region "Properties"

    <System.ComponentModel.Category("Wma")> _
    <System.ComponentModel.Description("Component Size")> _
    Public Property ComponentSize As ControlSize

        Get

            Return Me.Height

        End Get

        Set(value As ControlSize)

            Me.Height = value

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
    <System.ComponentModel.Description("Component Values")> _
    Public WriteOnly Property Values As List(Of String)

        Set(value As List(Of String))

            If value IsNot Nothing AndAlso value.Count > 0 Then

                If Me.VerifyParameters() Then

                    Dim freeClausules_ As String = " and " & _fieldKeyValue & " in(" & String.Join(",", value) & ")"

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

                    iOperacionAuxiliar_.ClausulasLibres = freeClausules_

                    iOperacionAuxiliar_.GenerarVista()

                    If _system.TieneResultados(iOperacionAuxiliar_) Then

                        Dim results_ = iOperacionAuxiliar_.Vista.Tables(0)

                        _iOperationsCatalog = iOperacionAuxiliar_

                        For indexPath As Integer = 0 To results_.Rows.Count - 1

                            Dim key_ As String = Nothing

                            Dim value_ As String = Nothing

                            Select Case _modeKeyField

                                Case KeyFieldModes.AutoPresentationTableKeyField

                                    key_ = IOperations.Vista(indexPath, _fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                                    value_ = IOperations.Vista(indexPath, _fieldDisplayValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                                Case KeyFieldModes.ManualTechnicalKeyField

                                    key_ = IOperations.Vista(indexPath, _fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                                    value_ = IOperations.Vista(indexPath, _fieldDisplayValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                            End Select

                            If value.Contains(key_) Then

                                PrepareLabelInMemory(value_, key_)

                            End If

                        Next

                    End If

                End If

            End If

        End Set

    End Property

    <System.ComponentModel.Category("Wma")> _
    <System.ComponentModel.Description("Component Legend")> _
    Public Property FieldSetLegend As String

        Get

            Return TagBarControlContainer.Text

        End Get
        Set(value As String)

            TagBarControlContainer.Text = value

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

    Public Event AfterSelectedRows(ByVal Values As List(Of String))

#End Region

#Region "Builders"

    Sub New()

        InitializeComponent()

        _iOperationsCatalog = New OperacionesCatalogo

        _fieldDisplayValue = Nothing

        _fieldKeyValue = Nothing

        _system = New Organismo

        _module = Organismo.Modulos.SinDefinir

        _quantityOfResults = 0

        _moduleDirect = Nothing

        _freeClausules = Nothing

        _keepLaunch = True

        _modeKeyField = KeyFieldModes.AutoPresentationTableKeyField

        _lHeight = 22

        _baseHeight = Me.Height

        _labels = New List(Of Label)

        _values = New List(Of String)

        ComponentSize = ControlSize.Small

        TagBarControlScrollView.HorizontalScroll.Maximum = 0

        TagBarControlScrollView.AutoScroll = False

        TagBarControlScrollView.VerticalScroll.Visible = False

        TagBarControlScrollView.AutoScroll = True

    End Sub

#End Region

#Region "Methods"

    Private Sub GroupBoxButton_Click(sender As Object, e As EventArgs) Handles TagBarControlButton.Click

        _controlMode = ModalTypes.BrowsingModal

        If _keepLaunch Then

            If Me.VerifyParameters() Then

                _iOperationsCatalog.PreparaCatalogo()

                _iOperationsCatalog.ClausulasLibres = Nothing

                _iOperationsCatalog.ClausulasLibres = _freeClausules

                If _values.Count > 0 Then

                    _iOperationsCatalog.ClausulasLibres = _freeClausules & " AND " & _fieldKeyValue & " NOT IN(" & String.Join(",", _values) & ")"

                End If

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


                                        Dim labelTitle_ As String = Nothing

                                        Dim labelKey_ As String = Nothing

                                        Select Case _modeKeyField

                                            Case KeyFieldModes.AutoPresentationTableKeyField

                                                labelKey_ = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                                                labelTitle_ = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldDisplayValue, _
                                                                                                    IOperacionesCatalogo.TiposAccesoCampo.NombreDesplegado)

                                            Case KeyFieldModes.ManualTechnicalKeyField

                                                labelKey_ = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldKeyValue, IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                                                labelTitle_ = _iOperationsCatalog.CampoPorNombreAvanzado(_fieldDisplayValue, _
                                                                                                    IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

                                        End Select

                                        If Not _values.Contains(labelKey_) Then

                                            PrepareLabelInMemory(labelTitle_, labelKey_)

                                        End If

                                    Else

                                    End If
                                Else


                                End If
                            Else


                            End If


                        End If

                    Catch ex As Exception

                    End Try

                Else

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

                        _system.GsDialogo("Seleccione un módulo válido { Nombre Directo de módulo}")

                    End If



                End If

            End If

        Else
            _system.GsDialogo("Faltan algunos parámetros para completar la funcionalidad")

        End If

        Return status_

    End Function

    Private Sub PrepareLabelInMemory(ByVal title_ As String, ByVal key_ As String)

        For Each tbcLabel_ As Label In TagBarControlScrollView.Controls

            If tbcLabel_.Name = key_ Then

                Exit Sub

            End If

        Next

        Dim label_ As Label = New Label()

        label_.ForeColor = Color.White

        label_.BackColor = Color.DarkGreen

        label_.Height = _lHeight

        label_.AutoSize = False

        label_.Visible = True

        label_.AutoEllipsis = True

        label_.Image = My.Resources.remove

        label_.Text = Trim(title_)

        label_.Name = Trim(key_)

        label_.TextAlign = ContentAlignment.MiddleLeft

        label_.ImageAlign = ContentAlignment.MiddleRight

        label_.Cursor = Cursors.Hand

        AddHandler label_.Click, AddressOf removeLabel

        _labels.Add(label_)

        drawLabels()

    End Sub

    Private Sub drawLabels()

        _xAxis = 1

        _yAxis = 0

        _indexPath = 0

        _values = New List(Of String)

        TagBarControlScrollView.VerticalScroll.Value = 0

        For Each label_ As Label In _labels

            _values.Add(label_.Name)

            label_.TabIndex = _indexPath

            _indexPath += 1

            Dim graphics_ As Graphics = label_.CreateGraphics

            Dim sizeF_ As SizeF

            sizeF_ = graphics_.MeasureString(label_.Text, label_.Font)

            Dim width_ = Math.Round(sizeF_.Width) + 22

            Dim margin_ As Integer = 24

            If width_ >= TagBarControlScrollView.Width - margin_ Then

                label_.Width = TagBarControlScrollView.Width - margin_

            Else

                label_.Width = width_

            End If

            If TagBarControlScrollView.Width < (_xAxis + (label_.Width + 4)) Then

                If _labels.Count > 1 Then

                    _yAxis += _lHeight + 1

                    _xAxis = 1

                End If

            End If

            label_.Location = New Point(_xAxis, _yAxis)

            TagBarControlScrollView.Controls.Add(label_)

            _xAxis += label_.Width + 1

        Next

        RaiseEvent AfterSelectedRows(_values)

    End Sub

    Private Sub removeLabel(ByVal sender As Label, ByVal e As System.EventArgs)

        _labels.RemoveAt(sender.TabIndex)

        sender.Dispose()

        drawLabels()

    End Sub

#End Region

End Class
