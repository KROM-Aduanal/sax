Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
    Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level:=AspNetHostingPermissionLevel.Minimal),
    DefaultEvent("TextChanged"),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class InputControl
    Inherits UIInputControl

#Region "Enums"

    Enum InputTextAlign
        Left = 1
        Center = 2
        Right = 3
    End Enum

    Enum InputType
        Text = 1
        TextArea = 2
        Hide = 3
        Checkbox = 4
        Radio = 5
    End Enum

    Enum InputFormat
        SinDefinir = 0
        Calendar = 1
        Time = 2
        Security = 3
        Phone = 4
        Numeric = 5
        Money = 6
        Card = 7
        Real = 8
    End Enum

#End Region

#Region "Controles"

    Private _inputElement As Object

    Private _validationsElements As HtmlGenericControl

    Private _unlokedControl As HtmlGenericControl

    Private _tooltipControlText As TextBox

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventTextChanged As New Object()

    Private Shared ReadOnly EventCheckedChange As New Object()

    Public Property TextAlign As InputTextAlign = InputTextAlign.Left

    Public Property Type As InputType = InputType.Text

    Public Property Format As InputFormat = InputFormat.SinDefinir

    Public Property Icon As String

    Public Overloads Property Value As String

        Get

            EnsureChildControls()

            Return _inputElement.Text

        End Get

        Set(value As String)

            EnsureChildControls()

            _inputElement.Text = value

        End Set

    End Property

    Public Overrides Property Enabled As Boolean

        Get

            Return MyBase.Enabled

        End Get

        Set(value As Boolean)

            If _inputElement IsNot Nothing Then

                If value = True Then

                    If _inputElement.GetType = GetType(CheckBox) Or _inputElement.GetType = GetType(RadioButton) Then

                        _inputElement.InputAttributes.Remove("disabled")

                    Else

                        _inputElement.Attributes.Remove("disabled")

                    End If

                Else

                    If _inputElement.GetType = GetType(CheckBox) Or _inputElement.GetType = GetType(RadioButton) Then

                        _inputElement.InputAttributes.Add("disabled", "disabled")

                    Else

                        _inputElement.Attributes.Add("disabled", "disabled")

                    End If

                End If

            End If

            MyBase.Enabled = value

        End Set
    End Property

    Public Property UpperCase As Boolean = False

#End Region

#Region "Constructor"
#End Region

#Region "Eventos"

    Public Custom Event TextChanged As EventHandler

        AddHandler(ByVal value As EventHandler)

            Events.AddHandler(EventTextChanged, value)

        End AddHandler

        RemoveHandler(ByVal value As EventHandler)

            Events.RemoveHandler(EventTextChanged, value)

        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As System.EventArgs)

            CType(Events(EventTextChanged), EventHandler).Invoke(sender, e)

        End RaiseEvent

    End Event

    Protected Overridable Sub OnTextChanged(ByVal e As EventArgs)

        Dim submitHandler As EventHandler = CType(Events(EventTextChanged), EventHandler)

        If submitHandler IsNot Nothing Then

            submitHandler(Me, e)

        End If

    End Sub

    Public Custom Event CheckedChanged As EventHandler

        AddHandler(ByVal value As EventHandler)

            Events.AddHandler(EventCheckedChange, value)

        End AddHandler

        RemoveHandler(ByVal value As EventHandler)

            Events.RemoveHandler(EventCheckedChange, value)

        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As System.EventArgs)

            CType(Events(EventCheckedChange), EventHandler).Invoke(sender, e)

        End RaiseEvent

    End Event

    Protected Overridable Sub OnCheckedChanged(ByVal e As EventArgs)

        Dim submitHandler As EventHandler = CType(Events(EventCheckedChange), EventHandler)

        If submitHandler IsNot Nothing Then

            submitHandler(Me, e)

        End If

    End Sub

#End Region

#Region "Metodos"

    Private Sub InputSelected(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        OnCheckedChanged(EventArgs.Empty)

    End Sub

    Private Sub TextInputChanged(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        OnTextChanged(EventArgs.Empty)

    End Sub

    Private Sub RenderTextField(ByRef component_ As HtmlGenericControl)

        _validationsElements = New HtmlGenericControl("div")

        _inputElement = New TextBox

        _unlokedControl = New HtmlGenericControl("div")

        Dim extraCss_ = Nothing

        With _inputElement

            .ID = ID

            .Text = Value

            .Attributes.Add("is", "wc-input")

            If UpperCase Then

                .Attributes.Add("class", "form-control text-uppercase text-align-" & TextAlign.ToString.ToLower)

            Else

                .Attributes.Add("class", "form-control text-align-" & TextAlign.ToString.ToLower)

            End If

            If _Format = InputFormat.Calendar Then

                .Attributes.Add("class", "form-control datepicker input-date-icon")

                .Attributes.Add("data-inputmask-alias", "datetime")

                .Attributes.Add("data-inputmask-inputformat", "dd/mm/yyyy")

                .Attributes.Add("im-insert", "false")

            ElseIf _Format = InputFormat.Time Then

                extraCss_ = " bootstrap-timepicker"

                .Attributes.Add("class", "form-control timepicker input-time-icon")

            ElseIf _Format = InputFormat.Numeric Then

                .Attributes.Add("class", "form-control numeric")

            ElseIf _Format = InputFormat.Real Then

                .Attributes.Add("class", "form-control real")

            ElseIf _Format = InputFormat.Money Then

                .Attributes.Add("class", "form-control currency")

            ElseIf _Format = InputFormat.Phone Then

                .Attributes.Add("data-inputmask", "'mask': '(999) 999-9999'")

                .Attributes.Add("data-mask", "")

            ElseIf _Format = InputFormat.Card Then

                .Attributes.Add("data-inputmask", "'mask': '9999-9999-9999-9999'")

                .Attributes.Add("data-mask", "")

            ElseIf _Format = InputFormat.Security Then

                .Attributes.Add("type", "password")

            Else

                If Icon IsNot Nothing Then

                    .Attributes.Add("icon style", "background-image:url(/FrontEnd/Librerias/Krom/imgs/" & Icon & ");")

                End If

            End If

            .Attributes.Add("placeholder", Label)

            .Attributes.Add("autocomplete", "off")

            .AutoPostBack = False ' hacer pruebas a ver que se quito con eso jeje

        End With

        AddHandler DirectCast(_inputElement, TextBox).TextChanged, AddressOf TextInputChanged

        With component_

            .Controls.Add(New LiteralControl("<div class='wc-input __component form-group position-relative w-100 mb-3 " & extraCss_ & "' " & ForeColor.HtmlPropertyColor & ">"))

            .Controls.Add(_inputElement)

            .Controls.Add(_unlokedControl)

            .Controls.Add(_validationsElements)

            .Controls.Add(New LiteralControl("	<label for='' class='float-form-control m-0'>"))

            .Controls.Add(New LiteralControl("		<span>" & Label & "</span>"))

            .Controls.Add(New LiteralControl("	</label>"))

            .Controls.Add(_tooltipControlText)

            .Controls.Add(New LiteralControl("</div>"))

        End With

        SetInputRules(_validationsElements)

    End Sub

    Private Sub RenderTextAreaField(ByRef component_ As HtmlGenericControl)

        _validationsElements = New HtmlGenericControl("div")

        _inputElement = New TextBox

        _unlokedControl = New HtmlGenericControl("div")

        With _inputElement

            .ID = ID

            .Text = Value

            If UpperCase Then

                .Attributes.Add("class", "form-control text-uppercase text-align-" & TextAlign.ToString.ToLower)

            Else

                .Attributes.Add("class", "form-control text-align-" & TextAlign.ToString.ToLower)

            End If

            .TextMode = TextBoxMode.MultiLine

            .Wrap = True

            If Icon IsNot Nothing Then

                .Attributes.Add("icon style", "background-image:url(/FrontEnd/Librerias/Krom/imgs/" & Icon & ");")

            End If

            .Attributes.Add("placeholder", Label)

            .AutoPostBack = True

        End With

        AddHandler DirectCast(_inputElement, TextBox).TextChanged, AddressOf TextInputChanged

        With component_

            .Controls.Add(New LiteralControl("<div class='wc-input __component form-group position-relative mb-3' " & ForeColor.HtmlPropertyColor & ">"))

            .Controls.Add(_inputElement)

            .Controls.Add(_unlokedControl)

            .Controls.Add(_validationsElements)

            .Controls.Add(New LiteralControl("	<label for='' class='float-form-control m-0'>"))

            .Controls.Add(New LiteralControl("		<span>" & Label & "</span>"))

            .Controls.Add(New LiteralControl("	</label>"))

            .Controls.Add(New LiteralControl("</div>"))

        End With

    End Sub

    Private Sub RenderHideField(ByRef component_ As HtmlGenericControl)

        _inputElement = New TextBox

        With _inputElement

            .ID = ID

            .Text = Value

            '.Attributes.Add("class", "d-none")

        End With

        With component_

            .Controls.Add(New LiteralControl("<div class='wc-input __component form-group position-relative d-none'>"))

            .Controls.Add(_inputElement)

            .Controls.Add(New LiteralControl("</div>"))

        End With


    End Sub

    Private Sub RenderCheckboxField(ByRef component_ As HtmlGenericControl)

        _inputElement = New CheckBox

        With _inputElement

            .ID = ID

            .InputAttributes.Add("class", "d-none")

            .AutoPostBack = True

        End With

        AddHandler DirectCast(_inputElement, CheckBox).CheckedChanged, AddressOf InputSelected

        With component_

            .Controls.Add(New LiteralControl("<label class='wc-checkbox __component d-flex position-relative mb-3' " & ForeColor.HtmlPropertyColor & ">"))

            If Icon IsNot Nothing Then

                .Controls.Add(New LiteralControl("<i style='background-image:url(/FrontEnd/Librerias/Krom/imgs/" & Icon & "'></i>"))

            End If

            .Controls.Add(_inputElement)

            .Controls.Add(New LiteralControl("    <span text='" & Label & "' class='d-flex align-items-center'></span>"))

            .Controls.Add(New LiteralControl("</label>"))

        End With

    End Sub

    Private Sub RenderRadioField(ByRef component_ As HtmlGenericControl)

        _inputElement = New RadioButton

        With _inputElement

            .ID = ID

            .InputAttributes.Add("class", "d-none")

            .AutoPostBack = True

        End With

        AddHandler DirectCast(_inputElement, CheckBox).CheckedChanged, AddressOf InputSelected

        With component_

            .Controls.Add(New LiteralControl("<label class='wc-radio __component d-flex position-relative mb-3' " & ForeColor.HtmlPropertyColor & ">"))

            If Icon IsNot Nothing Then

                .Controls.Add(New LiteralControl("<i style='background-image:url(/FrontEnd/Librerias/Krom/imgs/" & Icon & "'></i>"))

            End If

            .Controls.Add(_inputElement)

            .Controls.Add(New LiteralControl("    <span text='" & Label & "' class='d-flex align-items-center'></span>"))

            .Controls.Add(New LiteralControl("</label>"))

        End With

    End Sub

#End Region

#Region "Renderizado"

    Protected Overrides Sub CreateChildControls()

        _tooltipControlText = New TextBox

        With _tooltipControlText

            .Attributes.Add("class", "__tooltip d-none")

            .Attributes.Add("is", "wc-tooltip")

        End With

        Dim component_ = New HtmlGenericControl("div")

        component_.Attributes.Add("class", CssClass)

        Select Case _Type

            Case InputType.Text

                RenderTextField(component_)

            Case InputType.TextArea

                RenderTextAreaField(component_)

            Case InputType.Checkbox

                RenderCheckboxField(component_)

            Case InputType.Radio

                RenderRadioField(component_)

            Case InputType.Hide

                RenderHideField(component_)

        End Select

        Me.Controls.Add(component_)

    End Sub

    Protected Overrides Sub Render(ByVal component_ As HtmlTextWriter)

        If Locked = True Then

            _unlokedControl.Controls.Add(SetLockedControl())

        End If

        GetToolTipSetting(_tooltipControlText)

        Me.RenderContents(component_)

    End Sub

    Protected Overrides Sub RecreateChildControls()

        EnsureChildControls()

    End Sub

#End Region

End Class


