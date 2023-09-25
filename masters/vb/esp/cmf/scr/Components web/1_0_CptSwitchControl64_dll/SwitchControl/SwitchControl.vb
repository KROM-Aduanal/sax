Imports System.Security.Permissions
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
                        Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
                        Level:=AspNetHostingPermissionLevel.Minimal),
DefaultEvent("CheckedChanged"),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class SwitchControl
    Inherits UIInputControl

#Region "Controls"

    Private _switchControl As CheckBox

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventCheckedChanged As New Object()

    Public Property OnText As String

    Public Property OffText As String

    Public Property Checked As Boolean

        Get

            EnsureChildControls()

            Return _switchControl.Checked

        End Get

        Set(ByVal Value As Boolean)

            EnsureChildControls()

            _switchControl.Checked = Value

        End Set

    End Property

    Public Overrides Property Enabled As Boolean

        Get

            Return MyBase.Enabled

        End Get

        Set(value As Boolean)

            If _switchControl IsNot Nothing Then

                If value = True Then

                    _switchControl.InputAttributes.Remove("disabled")

                Else

                    _switchControl.InputAttributes.Add("disabled", "disabled")

                End If

            End If

            MyBase.Enabled = value

        End Set

    End Property

#End Region

#Region "Constructor"
#End Region

#Region "Eventos"

    Public Custom Event CheckedChanged As EventHandler

        AddHandler(ByVal value As EventHandler)

            Events.AddHandler(EventCheckedChanged, value)

        End AddHandler

        RemoveHandler(ByVal value As EventHandler)

            Events.RemoveHandler(EventCheckedChanged, value)

        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As System.EventArgs)

            CType(Events(EventCheckedChanged), EventHandler).Invoke(sender, e)

        End RaiseEvent

    End Event

    Protected Overridable Sub OnCheckedChanged(ByVal e As EventArgs)

        Dim submitHandler As EventHandler = CType(Events(EventCheckedChanged), EventHandler)

        If submitHandler IsNot Nothing Then

            submitHandler(Me, e)

        End If

    End Sub

    Private Sub SwitchCheckedChanged(ByVal source As Object, ByVal e As EventArgs)

        OnCheckedChanged(EventArgs.Empty)

    End Sub

#End Region

#Region "Metodos"

    Private Sub SettingSwitchControls()

        _switchControl = New CheckBox

        With _switchControl

            .ID = ID

            .AutoPostBack = True

            AddHandler .CheckedChanged, AddressOf SwitchCheckedChanged

        End With

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingSwitchControls()

        Dim component_ = New HtmlGenericControl("div")

        With component_

            .Attributes.Add("class", CssClass)

            .Controls.Add(New LiteralControl("<label class='wc-switch __component position-relative mb-3' " & ForeColor.HtmlPropertyColor & ">"))

            .Controls.Add(_switchControl)

            .Controls.Add(New LiteralControl("	<span on-text='" & _OnText & "' off-text='" & _OffText & "' class='d-flex align-items-center'></span>"))

            .Controls.Add(New LiteralControl("	<small></small>"))

            .Controls.Add(New LiteralControl("	<span>" & Label & "</span>"))

            .Controls.Add(New LiteralControl("</label>"))

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
