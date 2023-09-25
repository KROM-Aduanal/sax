Imports System.Windows.Forms
Imports Gsol.BaseDatos.Operaciones
Imports System.Drawing
Imports System.Text

Namespace Wma.Components

    Public Class OperationsDynamicForm
        Implements IOperationsDynamicForm, 
            IDisposable

#Region "Attributes"

        Private _controlpairsdisplayedv As Int32
        Private WithEvents _mainform As Form
        Private _iopertionsCatalog As IOperacionesCatalogo
        Private _layoutcolumnsv As Int32

        'Template Elements
        Private _layoutcontent As TableLayoutPanel
        Private _fontstyle As FontStyle
        Private _font As Font

        'Form Elements (Controls)
        Private _contentpanel As Panel
        Private _answerpanel As Panel

        'Control footer
        Public WithEvents _btnAccept As Button

        Public WithEvents _btnCancel As Button

        Private _lblTitle As Label

        Private _lblMessage As Label

        'Collections 
        Private _collectionpaircontrols As Dictionary(Of Int32, OperationdPairControls)

        'Layout properties
        Private _numbercolumns As Int32

        Private _numberofrows As Int32

        'Mode
        Private _modedynamiccontrols As IOperationsDynamicForm.ModeDynamicControls

        Private _queryrules As StringBuilder

        Private _observations As StringBuilder

        'Launch query args
        Private _launchQuery As Boolean

        'Collection of questions
        Private _collectionQuestions As List(Of ObjetoPregunta)

        'Configuracion adicional
        Private _mostrarBarraTituloGeneral As Boolean

        Private _mostrarBotonesDeAccion As Boolean

        Private _modalidaPresentacionCuestionario As IOperationsDynamicForm.ModalidadesPresentacionCuestionario

        Private _numeroPreguntaActiva As Int32 = 0

#End Region

#Region "Events"

        Public Event AfterCloseDynamicForm() Implements IOperationsDynamicForm.AfterCloseDynamicForm

        Public Event OnClickAcceptEvent() Implements IOperationsDynamicForm.OnClickAccept

        Public Event OnClickCancelEvent() Implements IOperationsDynamicForm.OnClickCancel

        Public Event OnPaint() Implements IOperationsDynamicForm.OnPaint

        Public Event AtEndOfPoll() Implements IOperationsDynamicForm.AtEndOfPoll

#End Region

#Region "Builders"

        Sub New()

            _controlpairsdisplayedv = Nothing

            _mainform = New Form

            _mainform.KeyPreview = True

            'Config mainform
            _mainform.StartPosition = FormStartPosition.CenterScreen

            _iopertionsCatalog = New OperacionesCatalogo

            _layoutcolumnsv = Nothing

            _layoutcontent = New TableLayoutPanel

            'Template elements
            _fontstyle = FontStyle.Bold
            _font = New Font("Calibri", 18, _fontstyle)

            'Form Elements (Controls)
            _contentpanel = New Panel

            _answerpanel = New Panel
            _lblTitle = New Label

            'Control footer
            _btnAccept = New Button
            _btnCancel = New Button


            'Collections
            _collectionpaircontrols = New Dictionary(Of Int32, OperationdPairControls)


            _numbercolumns = 3
            _numberofrows = 10

            'Mode
            _modedynamiccontrols = IOperationsDynamicForm.ModeDynamicControls.AsSimpleInterface

            'AddHandler AfterCloseDynamicForm, AddressOf Me._mainform.FormClosed

            'Query rules, selected
            _queryrules = New StringBuilder

            _observations = New StringBuilder

            _lblMessage = New Label

            _launchQuery = False

            _collectionQuestions = New List(Of ObjetoPregunta)

            _mostrarBarraTituloGeneral = True

            _mostrarBotonesDeAccion = True

            _modalidaPresentacionCuestionario = IOperationsDynamicForm.ModalidadesPresentacionCuestionario.PresentaTodoActivo

        End Sub



#End Region

#Region "Methods"

        Private Sub ProcessCollectionOfConditions()
            'Cleaning observations
            _observations.Clear()

            For Each ctrl_ As OperationdPairControls In _collectionpaircontrols.Values

                Select Case _modedynamiccontrols

                    Case IOperationsDynamicForm.ModeDynamicControls.AsSimpleInterface
                        'Simple control

                        Select Case ctrl_.TypeCharacteristic
                            Case ICaracteristica.TiposCaracteristica.cInt32,
                                 ICaracteristica.TiposCaracteristica.cReal,
                                 ICaracteristica.TiposCaracteristica.cString

                                MsgBox(ctrl_.ControlContent.Text)

                            Case ICaracteristica.TiposCaracteristica.cBoolean
                                MsgBox(ctrl_.ControlContent.Checked.ToString)
                            Case ICaracteristica.TiposCaracteristica.cDateTime
                                MsgBox(ctrl_.ControlContent.Text)
                        End Select


                    Case IOperationsDynamicForm.ModeDynamicControls.AsComparisons

                        'Only works with selected things!
                        If Not ctrl_.ControlContent.Controler.IsIncluded Then
                            Continue For
                        End If

                        If Not ctrl_.ControlContent.Controler.ControlValue1 Is Nothing And _
                            Trim(ctrl_.ControlContent.Controler.ControlValue1) <> "" Then

                            'MsgBox(ctrl_.ControlContent.Controler.ControlValue1)
                            AddCondition(_queryrules, _
                                    ctrl_.Characteristic, _
                                    ctrl_.ControlContent.Controler.Criterion, _
                                    ctrl_)
                        Else
                            _observations.AppendLine("    Coloque un valor para '" & ctrl_.Characteristic.NombreMostrar & "'. ")
                            'MsgBox(_observations.ToString)
                            _lblMessage.Text = _observations.ToString
                            _queryrules.Clear()

                            Exit For

                        End If


                End Select

            Next
        End Sub


        Private Function ReturnCriterion(ByVal criterion_ As ComponentCriteriaDCKR.TypesCriteria)
            Dim tokencriterion_ As String = "="

            Select Case criterion_
                Case ComponentCriteriaDCKR.TypesCriteria.BetweenValues : tokencriterion_ = "And"
                Case ComponentCriteriaDCKR.TypesCriteria.Contains : tokencriterion_ = "Like"
                Case ComponentCriteriaDCKR.TypesCriteria.EndsWith : tokencriterion_ = "Like"
                Case ComponentCriteriaDCKR.TypesCriteria.EqualsTo : tokencriterion_ = "="
                Case ComponentCriteriaDCKR.TypesCriteria.GreaterThan : tokencriterion_ = ">"
                Case ComponentCriteriaDCKR.TypesCriteria.IsDifferentOf : tokencriterion_ = "<>"
                Case ComponentCriteriaDCKR.TypesCriteria.MinorThan : tokencriterion_ = "<"
                Case ComponentCriteriaDCKR.TypesCriteria.StartWith : tokencriterion_ = "Like"
                Case ComponentCriteriaDCKR.TypesCriteria.Undefined : tokencriterion_ = Nothing
                Case Else
                    Return Nothing
            End Select

            Return tokencriterion_
        End Function

        Private Function ReturnBoolean(ByVal value_ As String) As String
            Dim _answer As String = "False"

            If value_ = "Sí" Then
                _answer = "True"
            Else
                _answer = "False"
            End If
            Return _answer
        End Function

        Private Sub AddCondition(ByRef conditions_ As StringBuilder, _
                                 ByVal field_ As ICaracteristica, _
                                 ByVal criterion_ As ComponentCriteriaDCKR.TypesCriteria, _
                                 ByVal control1_ As OperationdPairControls)

            Select Case criterion_
                Case ComponentCriteriaDCKR.TypesCriteria.BetweenValues ' Spetial Case, Number, Date

                    Select Case field_.TipoDato
                        Case ICaracteristica.TiposCaracteristica.cDateTime
                            conditions_.AppendLine(" and ( cast(" & field_.Nombre & " as date) between '" &
                                                   Trim(control1_.ControlContent.Controler.ControlValue1) & "' and '" & Trim(control1_.ControlContent.Controler.ControlValue2) & "') ")
                        Case ICaracteristica.TiposCaracteristica.cReal,
                            ICaracteristica.TiposCaracteristica.cInt32

                            If IsNumeric(control1_.ControlContent.Controler.ControlValue1) And
                                IsNumeric(control1_.ControlContent.Controler.ControlValue2) Then
                                conditions_.AppendLine(" and ( " & field_.Nombre & " between " & control1_.ControlContent.Controler.ControlValue1 & " and " & control1_.ControlContent.Controler.ControlValue2 & ") ")
                            End If


                    End Select

                Case ComponentCriteriaDCKR.TypesCriteria.Contains ' String
                    conditions_.AppendLine(" and (" & field_.Nombre & " " & ReturnCriterion(criterion_) & " '%" & Trim(control1_.ControlContent.Controler.ControlValue1) & "%') ")

                Case ComponentCriteriaDCKR.TypesCriteria.EndsWith 'String
                    conditions_.AppendLine(" and (" & field_.Nombre & " " & ReturnCriterion(criterion_) & " '%" & control1_.ControlContent.Controler.ControlValue1 & "') ")

                Case ComponentCriteriaDCKR.TypesCriteria.EqualsTo 'String, Number, Date
                    Select Case field_.TipoDato
                        Case ICaracteristica.TiposCaracteristica.cString, ICaracteristica.TiposCaracteristica.cBoolean
                            conditions_.AppendLine(" and (" & field_.Nombre & " " & ReturnCriterion(criterion_) & " '" & Trim(control1_.ControlContent.Controler.ControlValue1) & "') ")

                        Case ICaracteristica.TiposCaracteristica.cReal,
                            ICaracteristica.TiposCaracteristica.cInt32

                            If IsNumeric(control1_.ControlContent.Controler.ControlValue1) Then
                                conditions_.AppendLine(" and (" & field_.Nombre & " " & ReturnCriterion(criterion_) & " " & control1_.ControlContent.Controler.ControlValue1 & ") ")
                            End If


                        Case ICaracteristica.TiposCaracteristica.cDateTime
                            conditions_.AppendLine(" and ( cast(" & field_.Nombre & " as date) " & ReturnCriterion(criterion_) & " '" & Trim(control1_.ControlContent.Controler.ControlValue1) & "') ")

                    End Select


                Case ComponentCriteriaDCKR.TypesCriteria.GreaterThan ' Number, Date
                    Select Case field_.TipoDato
                        Case ICaracteristica.TiposCaracteristica.cDateTime
                            conditions_.AppendLine(" and ( cast(" & field_.Nombre & " as date) " & ReturnCriterion(criterion_) & " '" & Trim(control1_.ControlContent.Controler.ControlValue1) & "') ")
                        Case ICaracteristica.TiposCaracteristica.cInt32,
                            ICaracteristica.TiposCaracteristica.cReal

                            If IsNumeric(control1_.ControlContent.Controler.ControlValue1) Then
                                conditions_.AppendLine(" and (" & field_.Nombre & " " & ReturnCriterion(criterion_) & " " & control1_.ControlContent.Controler.ControlValue1 & ") ")
                            End If

                    End Select


                Case ComponentCriteriaDCKR.TypesCriteria.IsDifferentOf 'String, Number
                    Select Case field_.TipoDato
                        Case ICaracteristica.TiposCaracteristica.cString, ICaracteristica.TiposCaracteristica.cBoolean

                            conditions_.AppendLine(" and (" & field_.Nombre & " " & ReturnCriterion(criterion_) & " '" & Trim(control1_.ControlContent.Controler.ControlValue1) & "') ")

                        Case ICaracteristica.TiposCaracteristica.cInt32, ICaracteristica.TiposCaracteristica.cReal

                            If IsNumeric(control1_.ControlContent.Controler.ControlValue1) Then
                                conditions_.AppendLine(" and (" & field_.Nombre & " " & ReturnCriterion(criterion_) & " " & control1_.ControlContent.Controler.ControlValue1 & ") ")
                            End If


                        Case ICaracteristica.TiposCaracteristica.cDateTime
                            conditions_.AppendLine(" and ( cast(" & field_.Nombre & " as date ) " & ReturnCriterion(criterion_) & " '" & Trim(control1_.ControlContent.Controler.ControlValue1) & "') ")

                    End Select

                Case ComponentCriteriaDCKR.TypesCriteria.MinorThan ' Number, Date
                    Select Case field_.TipoDato
                        Case ICaracteristica.TiposCaracteristica.cDateTime

                            conditions_.AppendLine(" and (" & field_.Nombre & " " & ReturnCriterion(criterion_) & " '" & Trim(control1_.ControlContent.Controler.ControlValue1) & "') ")

                        Case ICaracteristica.TiposCaracteristica.cInt32,
                            ICaracteristica.TiposCaracteristica.cReal

                            If IsNumeric(control1_.ControlContent.Controler.ControlValue1) Then

                                conditions_.AppendLine(" and (" & field_.Nombre & " " & ReturnCriterion(criterion_) & " " & control1_.ControlContent.Controler.ControlValue1 & ") ")
                            End If

                    End Select

                Case ComponentCriteriaDCKR.TypesCriteria.StartWith 'String
                    conditions_.AppendLine(" and (" & field_.Nombre & " " & ReturnCriterion(criterion_) & " '" & control1_.ControlContent.Controler.ControlValue1 & "%') ")

                Case Else

            End Select

        End Sub

        Public Sub OnClickAccept(sender As Object, e As EventArgs) Handles _btnAccept.Click
            RaiseEvent OnClickAcceptEvent()

            ProcessCollectionOfConditions()
            If _observations.ToString Is Nothing Or _
                _observations.ToString = "" Then

                _launchQuery = True

                _mainform.Close()

            End If

        End Sub

        Public Sub OnClickCancel(sender As Object, e As EventArgs) Handles _btnCancel.Click
            RaiseEvent OnClickCancelEvent()

            _launchQuery = False

            _mainform.Close()


        End Sub


        Public Sub _mainform_FormClosed(sender As Object, e As FormClosedEventArgs) Handles _mainform.FormClosed

            RaiseEvent AfterCloseDynamicForm()

        End Sub


        Public Sub _mainform_FormPaint(sender As Object, e As PaintEventArgs) Handles _mainform.Paint

            RaiseEvent OnPaint()

        End Sub

        Private Sub _mainForm_KeyDown(sender As Object, e As KeyEventArgs) Handles _mainform.KeyDown

            Select Case e.KeyData

                Case Keys.Escape

                    _mainform.Close()

                Case Keys.Enter

                    _btnAccept.PerformClick()

            End Select

        End Sub

        Private Sub PreparingFooter()

            'Answer panel
            _answerpanel.BackColor = Color.ForestGreen

            _answerpanel.Dock = DockStyle.Bottom

            _answerpanel.Height = 70

            _btnAccept.Text = "Aceptar"
            _btnAccept.ForeColor = Color.White
            _btnAccept.BackColor = Color.ForestGreen
            _btnAccept.FlatStyle = FlatStyle.Flat
            _btnAccept.Height = 40
            _btnAccept.Width = 100
            _btnAccept.Top = 10
            _btnAccept.Left = 50


            _btnCancel.Text = "Cancelar"
            _btnCancel.ForeColor = Color.White
            _btnCancel.BackColor = Color.ForestGreen
            _btnCancel.FlatStyle = FlatStyle.Flat
            _btnCancel.Height = 40
            _btnCancel.Width = 100
            _btnCancel.Top = 10
            _btnCancel.Left = 230

            _answerpanel.Controls.Add(_btnAccept)

            _answerpanel.Controls.Add(_btnCancel)

        End Sub

        Private Sub PreparingHeader()
            'Main controls
            _contentpanel.BackColor = Color.White
            _contentpanel.AutoScroll = True

            Select Case _modedynamiccontrols
                Case IOperationsDynamicForm.ModeDynamicControls.AsSimpleInterface
                    _lblTitle.Text = "  Gestor:" & _iopertionsCatalog.Nombre

                Case IOperationsDynamicForm.ModeDynamicControls.AsComparisons
                    _lblTitle.Text = "  Búsqueda Avanzada:" & _iopertionsCatalog.Nombre

                Case IOperationsDynamicForm.ModeDynamicControls.AsQuestions
                    _lblTitle.Text = "  Cuestionario:" & _iopertionsCatalog.Nombre

                Case Else
                    'Adding dynamic controls, title
                    _lblTitle.Text = "  {Title}"


            End Select

            _lblMessage.ForeColor = Color.Red

            _lblMessage.Dock = DockStyle.Bottom

            _lblMessage.Font = New Font(_font.Name, 8, FontStyle.Regular)

            _lblTitle.Dock = DockStyle.Top

            _lblTitle.BackColor = Color.WhiteSmoke

            Dim point_ As New Point(10, 1)

            _lblTitle.Location = point_

            _lblTitle.Font = New Font(_font.Name, 14, FontStyle.Regular)

            _mainform.Controls.Add(_lblTitle)

            _lblMessage.Location = point_

            _lblMessage.Dock = DockStyle.Bottom

            _mainform.Controls.Add(_lblMessage)

        End Sub

        Public Sub ShowMyDynamicform() _
            Implements IOperationsDynamicForm.ShowMyDynamicForm

            If _mostrarBarraTituloGeneral Then : PreparingHeader() : End If

            If _mostrarBotonesDeAccion Then : PreparingFooter() : End If

            CreateDynamicForm()

            PreparingIULayout()

            _layoutcontent.AutoSize = True

            Dim point_ As New Point(10, 25)

            _layoutcontent.Location = point_

            'Adding ontrols
            _contentpanel.Controls.Add(_layoutcontent)

            _contentpanel.Dock = DockStyle.Fill

            _contentpanel.AutoScroll = True

            _mainform.Controls.Add(_contentpanel)

            If _mostrarBotonesDeAccion Then : _answerpanel.Dock = DockStyle.Bottom : _mainform.Controls.Add(_answerpanel) : End If


            If _modedynamiccontrols = IOperationsDynamicForm.ModeDynamicControls.AsQuestions Then

                _mainform.Show()

            Else

                _mainform.ShowDialog()

            End If


        End Sub



        Private Function NewHeight(ByVal quantitypairs_ As Int32) As Int32
            Dim newheight_ As Int32 = 200

            Select Case _modedynamiccontrols
                Case IOperationsDynamicForm.ModeDynamicControls.AsSimpleInterface
                    Select Case quantitypairs_
                        Case 1
                            newheight_ = 200
                        Case 2
                            newheight_ = 250
                        Case 3
                            newheight_ = 300
                        Case 4
                            newheight_ = 350
                        Case 5
                            newheight_ = 400
                        Case Else
                            newheight_ = 450

                    End Select

                    _mainform.Width = 300

                Case IOperationsDynamicForm.ModeDynamicControls.AsComparisons
                    Select Case quantitypairs_
                        Case 1
                            newheight_ = 300
                        Case 2
                            newheight_ = 360
                        Case 3
                            newheight_ = 420
                        Case 4
                            newheight_ = 480
                        Case 5
                            newheight_ = 540
                        Case Else
                            newheight_ = 600

                    End Select
                    _mainform.Width = 410

                Case IOperationsDynamicForm.ModeDynamicControls.AsQuestions
                    Select Case quantitypairs_
                        Case 1
                            newheight_ = 300
                        Case 2
                            newheight_ = 360
                        Case 3
                            newheight_ = 420
                        Case 4
                            newheight_ = 480
                        Case 5
                            newheight_ = 540
                        Case Else
                            newheight_ = 600

                    End Select
                    _mainform.Width = 410


            End Select

            Return newheight_

        End Function

        Private Sub PreparingIULayout()

            Dim contadorControles_ As Int32 = 0
            '----------espacio especial

            Dim cantidadControlesColumna_ As Int32 = 8

            Dim contadorControlesAuxiliar_ As Int32 = 0

            Dim posicionHorizontal_ As Int32 = 0

            '----------espacio especial

            'Config form, resizing
            If _collectionpaircontrols Is Nothing Then : Exit Sub : End If

            _mainform.Height = NewHeight(_collectionpaircontrols.Count)


            'Template layout
            '_layoutcontent.BackColor = Color.Pink
            '_layoutcontent.Dock = DockStyle.Fill

            'layoutcontent.Location = New System.Drawing.Point(26, 12)

            _layoutcontent.ColumnCount = _numbercolumns

            _layoutcontent.RowCount = _numberofrows

            'Setting config for margins in default columns ( Title )
            _layoutcontent.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30.0F))
            _layoutcontent.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30.0F))
            _layoutcontent.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30.0F))


            'Row styles I( Static )
            ''Top Magin
            '_layoutcontent.RowStyles.Add(New RowStyle(SizeType.Absolute, 10.0F))

            ''Title
            '_layoutcontent.RowStyles.Add(New RowStyle(SizeType.Absolute, 30.0F))

            'Rows
            'For row_ As Int32 = 1 To Convert.ToInt32(_numberofrows / 2)
            For row_ As Int32 = 1 To cantidadControlesColumna_
                '_layoutcontent.RowStyles.Add(New RowStyle(SizeType.Absolute, 15.0F))

                Select Case _modedynamiccontrols

                    Case IOperationsDynamicForm.ModeDynamicControls.AsSimpleInterface

                        _layoutcontent.RowStyles.Add(New RowStyle(SizeType.Absolute, 30.0F))

                    Case IOperationsDynamicForm.ModeDynamicControls.AsComparisons

                        _layoutcontent.RowStyles.Add(New RowStyle(SizeType.Absolute, 60.0F))

                    Case IOperationsDynamicForm.ModeDynamicControls.AsQuestions
                        'MOP 07/12/2020, derivada
                        '_layoutcontent.RowStyles.Add(New RowStyle(SizeType.Absolute, 30.0F))
                        '_layoutcontent.RowStyles.Add(New RowStyle(SizeType.Absolute, 100.0F))

                End Select

            Next
            '------------------------------------------------------------------------------

            'Adding controls in Layout
            Dim position_ As Int32 = 0

            Dim messageInstalled_ As Boolean = False

            For Each pairobjects_ As OperationdPairControls In _collectionpaircontrols.Values

                If Not _modedynamiccontrols = IOperationsDynamicForm.ModeDynamicControls.AsQuestions Then

                    pairobjects_.ControlTitleDirect = DirectCast(pairobjects_.ControlTitle, System.Windows.Forms.Label).Text ' pairobjects_.ControlTitle

                    '_layoutcontent.Controls.Add(pairobjects_.ControlContent, posicionHorizontal_, position_)

                End If

                _layoutcontent.Controls.Add(pairobjects_.ControlContent, posicionHorizontal_, position_)

                If contadorControlesAuxiliar_ > cantidadControlesColumna_ Then

                    'Message
                    '_layoutcontent.Controls.Add(_lblMessage, posicionHorizontal_, position_)

                    messageInstalled_ = True

                    'ancho_ += 220
                    'posx_ += 300
                    'posy_ = 90
                    contadorControlesAuxiliar_ = 0
                    'enUmbralDeControles_ = True
                    posicionHorizontal_ += 1

                    position_ = 0

                    _mainform.Width += 185

                Else

                    position_ += 1

                End If

                contadorControlesAuxiliar_ += 1

            Next

        End Sub

        Private Sub CollectionPairControls(ByVal position_ As Int32, _
            ByVal paircontrols_ As OperationdPairControls)

            _collectionpaircontrols.Add(position_, paircontrols_)

        End Sub

        Private Function PrepareDynamicControls(ByVal characteristic_ As ICaracteristica) As OperationdPairControls
            'Dim foundobject_ As Object = Nothing
            Dim newpair_ As New OperationdPairControls
            'Dim lblLabel_ As New Label

            newpair_.TypeCharacteristic = characteristic_.TipoDato
            newpair_.TechnicalName = characteristic_.Nombre
            'Title, usually Label
            newpair_.ControlTitle = New Label

            newpair_.ControlTitle.Autosize = True

            'Dock content
            'newpair_.ControlContent.Dock = DockStyle.Fill
            newpair_.Characteristic = characteristic_

            Select Case _modedynamiccontrols

                Case IOperationsDynamicForm.ModeDynamicControls.AsSimpleInterface

                    Select Case characteristic_.TipoDato

                        Case ICaracteristica.TiposCaracteristica.cBoolean
                            'newpair_.ControlTitle = New Label
                            newpair_.ControlTitle.Text = characteristic_.NombreMostrar

                            newpair_.ControlContent = New CheckBox
                            ' newpair_.ControlContent.Text = characteristic_.NombreMostrar
                            newpair_.ControlContent.Dock = DockStyle.Fill


                        Case ICaracteristica.TiposCaracteristica.cDateTime
                            'newpair_.ControlTitle = New Label
                            newpair_.ControlTitle.Text = characteristic_.NombreMostrar

                            newpair_.ControlContent = New DateTimePicker

                        Case ICaracteristica.TiposCaracteristica.cInt32

                            'newpair_.ControlTitle = New Label
                            newpair_.ControlTitle.Text = characteristic_.NombreMostrar

                            newpair_.ControlContent = New TextBox
                            newpair_.ControlContent.Text = characteristic_.Nombre
                            newpair_.ControlContent.Dock = DockStyle.Fill

                        Case ICaracteristica.TiposCaracteristica.cReal

                            'newpair_.ControlTitle = New Label
                            newpair_.ControlTitle.Text = characteristic_.NombreMostrar
                            newpair_.ControlContent.Dock = DockStyle.Fill

                            newpair_.ControlContent = New TextBox
                            newpair_.ControlContent.Text = characteristic_.Nombre
                            'newpair_.ControlContent.Dock = DockStyle.Fill

                        Case ICaracteristica.TiposCaracteristica.cString
                            'lblLabel_.Text = characteristic_.NombreMostrar
                            'Dim tbDescription As New TextBox

                            'newpair_.ControlTitle = New Label
                            newpair_.ControlTitle.Text = characteristic_.NombreMostrar

                            newpair_.ControlContent = New TextBox
                            newpair_.ControlContent.Text = characteristic_.ValorAsignado
                            newpair_.ControlContent.Dock = DockStyle.Fill

                            'newpair_.ControlContent = New TextBoxDCKR
                            'newpair_.ControlContent.Controler.ControlValue1 = characteristic_.ValorAsignado
                            'newpair_.ControlContent.Dock = DockStyle.Fill



                        Case ICaracteristica.TiposCaracteristica.cUndefined

                            MsgBox("This charactetistic doesn't have Type! '" & characteristic_.NombreMostrar & "'")
                            Return Nothing

                        Case Else
                            MsgBox("This charactetistic doesn't have Type! '" & characteristic_.NombreMostrar & "'")
                            Return Nothing

                    End Select

                Case IOperationsDynamicForm.ModeDynamicControls.AsComparisons

                    Select Case characteristic_.TipoDato

                        Case ICaracteristica.TiposCaracteristica.cBoolean
                            'newpair_.ControlTitle = New Label
                            newpair_.ControlTitle.Text = characteristic_.NombreMostrar

                            newpair_.ControlContent = New TextBoxDCKR(characteristic_.TipoDato)
                            newpair_.ControlContent.Controler.PrepareForDataType = ICaracteristica.TiposCaracteristica.cBoolean
                            'newpair_.ControlContent.Controler.ControlValue1 = 1
                            newpair_.ControlContent.Dock = DockStyle.Fill

                        Case ICaracteristica.TiposCaracteristica.cDateTime
                            'newpair_.ControlTitle = New Label
                            newpair_.ControlTitle.Text = characteristic_.NombreMostrar

                            'newpair_.ControlContent = New TextBoxDCKR
                            newpair_.ControlContent = New DateTimePickerDCKR
                            newpair_.ControlContent.Controler.PrepareForDataType = ICaracteristica.TiposCaracteristica.cDateTime
                            'newpair_.ControlContent.Controler.ControlValue1 = "<Empty>"
                            newpair_.ControlContent.Dock = DockStyle.Fill


                        Case ICaracteristica.TiposCaracteristica.cInt32

                            'newpair_.ControlTitle = New Label
                            newpair_.ControlTitle.Text = characteristic_.NombreMostrar

                            newpair_.ControlContent = New TextBoxDCKR(characteristic_.TipoDato)
                            newpair_.ControlContent.Controler.PrepareForDataType = ICaracteristica.TiposCaracteristica.cInt32
                            'newpair_.ControlContent.Controler.ControlValue1 = characteristic_.ValorDefault
                            newpair_.ControlContent.Dock = DockStyle.Fill

                        Case ICaracteristica.TiposCaracteristica.cReal

                            'newpair_.ControlTitle = New Label
                            newpair_.ControlTitle.Text = characteristic_.NombreMostrar

                            newpair_.ControlContent = New TextBoxDCKR(characteristic_.TipoDato)
                            newpair_.ControlContent.Controler.PrepareForDataType = ICaracteristica.TiposCaracteristica.cReal
                            'newpair_.ControlContent.Controler.ControlValue1 = characteristic_.ValorDefault
                            newpair_.ControlContent.Dock = DockStyle.Fill

                        Case ICaracteristica.TiposCaracteristica.cString
                            'newpair_.ControlTitle = New Label
                            newpair_.ControlTitle.Text = characteristic_.NombreMostrar

                            newpair_.ControlContent = New TextBoxDCKR(characteristic_.TipoDato)
                            newpair_.ControlContent.Controler.PrepareForDataType = ICaracteristica.TiposCaracteristica.cString
                            'newpair_.ControlContent.Controler.ControlValue1 = characteristic_.ValorDefault
                            newpair_.ControlContent.Dock = DockStyle.Fill


                        Case ICaracteristica.TiposCaracteristica.cUndefined

                            MsgBox("This charactetistic doesn't have Type! '" & characteristic_.NombreMostrar & "'")
                            Return Nothing
                        Case Else
                            MsgBox("This charactetistic doesn't have Type! '" & characteristic_.NombreMostrar & "'")
                            Return Nothing
                    End Select

                Case IOperationsDynamicForm.ModeDynamicControls.AsQuestions
                    '03/12/2020, nuevo método para aplicar los cuestionarios.

                    Dim wclPoll_ As New WclPoll(characteristic_)

                    'Asignación de caracteristica
                    newpair_.Characteristic = characteristic_

                    newpair_.ControlTitle.Text = wclPoll_.ConfiguracionPregunta.Titulo

                    newpair_.TypeCharacteristic = characteristic_.TipoDato

                    newpair_.ControlContent = wclPoll_



                    If wclPoll_.ConfiguracionPregunta.Visible = "0" Or
                        wclPoll_.ConfiguracionPregunta.Visible = "" Then

                        wclPoll_.Visible = False

                        newpair_.ControlContent.Dock = DockStyle.None

                    Else

                        newpair_.ControlContent.Dock = DockStyle.Fill

                    End If

                    _collectionQuestions.Add(wclPoll_.ConfiguracionPregunta)

                    Dim evento_ As New WclPoll.AlResponderCorrectamentePreguntaEventHandler(AddressOf CuandoRecibaRespuestaCorrecta)

                    AddHandler wclPoll_.AlResponderCorrectamentePregunta, evento_

                Case Else

                    MsgBox("Please select a valid mode before work!")

            End Select

            Return newpair_

        End Function


        Private Sub CuandoRecibaRespuestaCorrecta(ByRef componentePregunta_ As ObjetoPregunta)

                'Tomamos el ID de la pregunta actual
                _numeroPreguntaActiva = componentePregunta_.ID

                'Identificamos si hay ID para liberar como siguiente pregunta en el control actual
                If Not componentePregunta_.IDSiguienteAnidada Is Nothing Then

                    'Se inactiva el actual
                    Dim controlDeBateria_ As WclPoll

                    controlDeBateria_ = LocalizaControl(componentePregunta_, _collectionpaircontrols)

                    If Not controlDeBateria_ Is Nothing Then

                        controlDeBateria_.Enabled = False

                    Else

                        Exit Sub

                    End If

                    Dim _par As KeyValuePair(Of Integer, OperationdPairControls)

                    Dim encontrado_ As Boolean = False

                    For Each _par In _collectionpaircontrols

                        If DirectCast(_par.Value.ControlContent, WclPoll).ConfiguracionPregunta.ID = componentePregunta_.IDSiguienteAnidada Then

                            DirectCast(_par.Value.ControlContent, WclPoll).ConfiguracionPregunta.Visible = True

                            DirectCast(_par.Value.ControlContent, WclPoll).Visible = True

                            DirectCast(_par.Value.ControlContent, WclPoll).Enabled = True

                            encontrado_ = True

                            Exit For

                        End If

                    Next

                    If Not encontrado_ Then

                        If SeHaFinalizadoElCuestionario(_collectionpaircontrols) Then

                            RaiseEvent AtEndOfPoll()

                        End If

                    End If

                Else

                    Dim controDeBateria_ As WclPoll

                    controDeBateria_ = LocalizaControl(componentePregunta_, _collectionpaircontrols)

                    If Not controDeBateria_ Is Nothing Then

                        controDeBateria_.Enabled = False

                        If SeHaFinalizadoElCuestionario(_collectionpaircontrols) Then

                            RaiseEvent AtEndOfPoll()

                            Exit Sub

                        End If

                    Else

                        Exit Sub

                    End If

                    Dim _par As KeyValuePair(Of Integer, OperationdPairControls)

                    Dim seEncontro_ As Boolean = True

                    For Each _par In _collectionpaircontrols

                        Dim idSiguienteCalculado_ As Int32 = _numeroPreguntaActiva + 1

                        If Not DirectCast(_par.Value.ControlContent, WclPoll).ConfiguracionPregunta.ID = idSiguienteCalculado_.ToString Then

                            Continue For

                        End If

                        DirectCast(_par.Value.ControlContent, WclPoll).Enabled = True

                        Exit For

                    Next

                End If



        End Sub

        Public Function LocalizaControl(ByRef componentePregunta_ As ObjetoPregunta, ByRef _collectionpaircontrols As Dictionary(Of Integer, OperationdPairControls)) As WclPoll

            Dim _par As KeyValuePair(Of Integer, OperationdPairControls)

            For Each _par In _collectionpaircontrols

                If DirectCast(_par.Value.ControlContent, WclPoll).ConfiguracionPregunta.ID = componentePregunta_.ID Then

                    Return DirectCast(_par.Value.ControlContent, WclPoll)

                End If

            Next

        End Function

        Public Function SeHaFinalizadoElCuestionario(ByRef _collectionpaircontrols As Dictionary(Of Integer, OperationdPairControls)) As Boolean

            Dim respuesta_ As Boolean = True

            Dim _par As KeyValuePair(Of Integer, OperationdPairControls)

            For Each _par In _collectionpaircontrols

                If DirectCast(_par.Value.ControlContent, WclPoll).ConfiguracionPregunta.Estatus = ObjetoPregunta.EstatusPregunta.SinDefinir Then

                    respuesta_ = False

                    Return respuesta_

                End If

            Next

            Return respuesta_

        End Function

        Private Sub CreateDynamicForm()

            If Not _iopertionsCatalog Is Nothing Then

                If Not _iopertionsCatalog.Caracteristicas Is Nothing Then

                    Dim position_ As Int32 = 0

                    For Each characteristic_ As ICaracteristica In _iopertionsCatalog.Caracteristicas.Values

                        If Not _modedynamiccontrols = IOperationsDynamicForm.ModeDynamicControls.AsQuestions Then

                            If characteristic_.TipoFiltro = ICaracteristica.TiposFiltro.SinDefinir Then

                                Continue For

                            End If

                        End If

                        Dim newcontrol_ As Object = Nothing

                        newcontrol_ = PrepareDynamicControls(characteristic_)

                        If Not newcontrol_ Is Nothing Then

                            If _modedynamiccontrols = IOperationsDynamicForm.ModeDynamicControls.AsQuestions And
                                _modalidaPresentacionCuestionario = IOperationsDynamicForm.ModalidadesPresentacionCuestionario.PresentaUnaPreguntaActiva Then

                                If position_ = 0 Then

                                    newcontrol_.ControlContent.Enabled = True

                                Else

                                    newcontrol_.ControlContent.Enabled = False

                                End If

                            End If

                            CollectionPairControls(position_, newcontrol_)

                        Else

                            MsgBox("Found some problems with ICaracteristicas at, '" & characteristic_.NombreMostrar & "'")
                            Exit For

                        End If

                        position_ += 1

                    Next

                Else
                    MsgBox("There isn't characteristics loaded!")
                End If

            End If

        End Sub

        Public Sub SalirSinEnviarFormulario() Implements IOperationsDynamicForm.SalirSinEnviarFormulario

            _launchQuery = False

            _mainform.Close()

        End Sub

        Public Sub EnviarFormulario() Implements IOperationsDynamicForm.EnviarFormulario

            ProcessCollectionOfConditions()

            If _observations.ToString Is Nothing Or _
                _observations.ToString = "" Then

                _launchQuery = True

                _mainform.Close()

            End If

        End Sub

#End Region

#Region "Properties"

        Public Property ModalidadPresentacionCuestionario As IOperationsDynamicForm.ModalidadesPresentacionCuestionario _
            Implements IOperationsDynamicForm.ModalidadPresentacionCuestionario

            Get

                Return _modalidaPresentacionCuestionario

            End Get

            Set(value As IOperationsDynamicForm.ModalidadesPresentacionCuestionario)

                _modalidaPresentacionCuestionario = value

            End Set

        End Property


        Public Property MostrarBarraTituloGeneral As Boolean _
        Implements IOperationsDynamicForm.MostrarBarraTituloGeneral

            Get

                Return _mostrarBarraTituloGeneral

            End Get

            Set(value As Boolean)

                _mostrarBarraTituloGeneral = value

            End Set

        End Property

        Public Property MostrarBotonesDeAccion As Boolean Implements IOperationsDynamicForm.MostrarBotonesDeAccion

            Get

                Return _mostrarBotonesDeAccion

            End Get

            Set(value As Boolean)

                _mostrarBotonesDeAccion = value

            End Set

        End Property

        Public Property CollectionOfQuestions As List(Of ObjetoPregunta) _
         Implements IOperationsDynamicForm.CollectionOfQuestions
            Get
                Return _collectionQuestions
            End Get
            Set(value As List(Of ObjetoPregunta))
                _collectionQuestions = value
            End Set
        End Property


        Public Property LaunchQuery As Boolean _
            Implements IOperationsDynamicForm.LaunchQuery
            Get
                Return _launchQuery
            End Get
            Set(value As Boolean)
                _launchQuery = value
            End Set
        End Property


        ReadOnly Property GetQueryRules As StringBuilder _
            Implements IOperationsDynamicForm.GetQueryRules
            Get
                Return _queryrules
            End Get
        End Property

        Public Property ModeForm As IOperationsDynamicForm.ModeDynamicControls _
            Implements IOperationsDynamicForm.ModeForm
            Get
                Return _modedynamiccontrols
            End Get
            Set(value As IOperationsDynamicForm.ModeDynamicControls)
                _modedynamiccontrols = value
            End Set
        End Property


        Public ReadOnly Property CollectionPairsControls As IDictionary(Of Int32, OperationdPairControls) _
            Implements IOperationsDynamicForm.CollectionPairsControls
            Get
                Return _collectionpaircontrols
            End Get
        End Property

        Public Property ControlPairsDisplayed As Integer _
        Implements IOperationsDynamicForm.ControlPairsDisplayed
            Get
                Return _controlpairsdisplayedv
            End Get
            Set(value As Integer)
                _controlpairsdisplayedv = value
            End Set

        End Property



        Public Property DynamicForm As Form _
            Implements IOperationsDynamicForm.DynamicForm
            Get
                Return _mainform
            End Get
            Set(value As Form)
                _mainform = value
            End Set
        End Property

        Public Property IOperations As IOperacionesCatalogo _
            Implements IOperationsDynamicForm.IOperations
            Get
                Return _iopertionsCatalog
            End Get
            Set(value As IOperacionesCatalogo)
                _iopertionsCatalog = value
            End Set
        End Property


        Public Property LayoutColumns As Integer _
            Implements IOperationsDynamicForm.LayoutColumns
            Get
                Return _layoutcolumnsv
            End Get
            Set(value As Integer)
                _layoutcolumnsv = value
            End Set
        End Property


#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean ' Para detectar llamadas redundantes

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: eliminar estado administrado (objetos administrados).
                End If

                ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
                ' TODO: Establecer campos grandes como Null.
            End If

            Me.disposedValue = True

        End Sub

        ' Visual Basic agregó este código para implementar correctamente el modeo descartable.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' No cambie este código. Coloque el código de limpieza en Dispose(disposing As Boolean).
            Dispose(True)

            GC.SuppressFinalize(Me)

        End Sub

#End Region

    End Class

End Namespace