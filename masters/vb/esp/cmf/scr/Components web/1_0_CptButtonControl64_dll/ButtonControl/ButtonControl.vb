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
DefaultEvent("Click"),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class ButtonControl
    Inherits UIInputControl

#Region "Controls"

    Private _buttonControl As Button

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventClick As New Object()

    Public Property Icon As String

    Public Property RowId As Integer

    Public Overrides Property Enabled As Boolean

        Get

            Return MyBase.Enabled

        End Get

        Set(value As Boolean)

            If _buttonControl IsNot Nothing Then

                If value = True Then

                    _buttonControl.Attributes.Remove("disabled")

                Else

                    _buttonControl.Attributes.Add("disabled", "disabled")

                End If

            End If

            MyBase.Enabled = value

        End Set

    End Property

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

    Private Sub ButtonClick(ByVal source As Object, ByVal e As EventArgs)

        OnClick(EventArgs.Empty)

    End Sub

#End Region

#Region "Metodos"

    Private Sub SettingButtonControls()

        _buttonControl = New Button

        With _buttonControl

            .ID = ID

            .CausesValidation = False

            .Text = Label

            If Icon IsNot Nothing Then

                If Label IsNot Nothing Then

                    .Attributes.Add("class", "wc-button button-icontext")

                Else

                    .Attributes.Add("class", "wc-button button-icon")

                End If

                If ForeColor.IsEmpty = False Then

                    Attributes.Add("style", "background-image:url(/FrontEnd/Librerias/Krom/imgs/" & Icon & "); --tintColor: " & ForeColor.ToHex)

                Else

                    Attributes.Add("style", "background-image:url(/FrontEnd/Librerias/Krom/imgs/" & Icon & ");")

                End If

            Else

                If ForeColor.IsEmpty = False Then

                    .Attributes.Add("style", "--tintColor: " & ForeColor.ToHex)

                End If

                .Attributes.Add("class", "wc-button")

            End If

            AddHandler .Click, AddressOf ButtonClick

        End With

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingButtonControls()

        Dim component_ = New HtmlGenericControl("div")

        With component_

            .Attributes.Add("class", CssClass)

            .Controls.Add(_buttonControl)

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
