Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web
Imports System.Security.Permissions
Imports System.ComponentModel
Imports System.Drawing

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
    Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level:=AspNetHostingPermissionLevel.Minimal),
DefaultEvent("Click"),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class DualityBarControl
    Inherits UIInputControl

#Region "Controles"

    Private _barButton As LinkButton

    Private _textValue As TextBox

    Private _textDetail As TextBox

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventClick As New Object()

    Public Property LabelDetail As String

    Public Property ValueDetail As String

        Get

            EnsureChildControls()

            Return _textDetail.Text

        End Get

        Set(value As String)

            EnsureChildControls()

            _textDetail.Text = value

            SetEstatusBar()

        End Set

    End Property

    Public Overloads Property Value As String

        Get

            EnsureChildControls()

            Return _textValue.Text

        End Get

        Set(value As String)

            EnsureChildControls()

            _textValue.Text = value

            SetEstatusBar()

        End Set

    End Property

    Public Overrides Property Enabled As Boolean

        Get

            Return MyBase.Enabled

        End Get

        Set(value As Boolean)

            If _textValue IsNot Nothing Then

                If value = True Then

                    _textValue.Attributes.Remove("disabled")

                Else

                    _textValue.Attributes.Add("disabled", "disabled")

                End If

                _textValue.Enabled = value

            Else

                MyBase.Enabled = value

            End If


        End Set

    End Property

    Public Property TintColor As Color

#End Region

#Region "Constructor"
#End Region

#Region "Eventos"

    Public Custom Event Click As EventHandler

        AddHandler(ByVal value As EventHandler)

            Events.AddHandler(EventClick, value)

        End AddHandler

        RemoveHandler(ByVal value As EventHandler)

            Events.RemoveHandler(EventClick, value)

        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As System.EventArgs)

            CType(Events(EventClick), EventHandler).Invoke(sender, e)

        End RaiseEvent

    End Event

    Protected Overridable Sub OnClick(ByVal e As EventArgs)

        Dim submitHandler As EventHandler = CType(Events(EventClick), EventHandler)

        If submitHandler IsNot Nothing Then

            submitHandler(Me, e)

        End If

    End Sub

    Private Sub DualityBarSetDetail(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        OnClick(EventArgs.Empty)

    End Sub

#End Region

#Region "Metodos"

    Private Sub SetEstatusBar()

        If Not String.IsNullOrEmpty(_textValue.Text) And Not String.IsNullOrEmpty(_textDetail.Text) Then

            _barButton.Attributes.Add("class", "full")

        Else

            If Not String.IsNullOrEmpty(_textValue.Text) Or Not String.IsNullOrEmpty(_textDetail.Text) Then

                _barButton.Attributes.Add("class", "semi")

            Else

                _barButton.Attributes.Remove("class")

            End If

        End If

    End Sub

#End Region

#Region "Renderizado"

    Private Sub SettingDualityBarControls()

        _textValue = New TextBox

        With _textValue

            .ID = ID

            .Attributes.Add("placeholder", Label)

            'If Value IsNot Nothing Then

            '    .Enabled = False

            'End If

        End With


        _textDetail = New TextBox

        With _textDetail

            .Enabled = False

            .Attributes.Add("placeholder", LabelDetail)

        End With


        _barButton = New LinkButton

        With _barButton

            .CausesValidation = False

            If Not TintColor.IsEmpty Then

                .Attributes.Add("style", "--ForeColor:" & TintColor.ToHex)

            End If

            AddHandler .Click, AddressOf DualityBarSetDetail

        End With

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingDualityBarControls()

        Dim component_ = New HtmlGenericControl("div")

        With component_

            .Attributes.Add("class", CssClass)

            .Controls.Add(New LiteralControl("<div class='wc-dualitybar row no-gutters align-items-center mb-3' " & ForeColor.HtmlPropertyColor & ">"))

            .Controls.Add(New LiteralControl("	<Label class='col'>"))

            .Controls.Add(New LiteralControl("        <span class='d-block'>" & Label & "</span>"))

            .Controls.Add(_textValue)

            .Controls.Add(New LiteralControl("	</label>"))

            .Controls.Add(New LiteralControl("	<Label class='col'>"))

            .Controls.Add(New LiteralControl("		<span class='d-block'>" & LabelDetail & "</span>"))

            .Controls.Add(New LiteralControl("	    <div class='w-100 d-flex'>"))

            .Controls.Add(New LiteralControl("	        <div class='col'>"))

            .Controls.Add(_textDetail)

            .Controls.Add(New LiteralControl("	        </div>"))

            .Controls.Add(New LiteralControl("	        <div class='col-auto'>"))

            .Controls.Add(_barButton)

            .Controls.Add(New LiteralControl("	        </div>"))

            .Controls.Add(New LiteralControl("	    </div>"))

            .Controls.Add(New LiteralControl("	</label>"))

            .Controls.Add(New LiteralControl("</div>"))

            SetEstatusBar()

        End With

        Me.Controls.Add(component_)

    End Sub

    Protected Overrides Sub Render(ByVal component_ As HtmlTextWriter)

        Me.RenderContents(component_)

    End Sub

    Protected Overrides Sub RecreateChildControls()

        EnsureChildControls()

    End Sub

#End Region

End Class


