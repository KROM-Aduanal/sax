Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Drawing
Imports System.Security.Permissions
Imports System.Web
Imports System.ComponentModel
Imports System.Web.Script.Serialization

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
    Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level:=AspNetHostingPermissionLevel.Minimal),
DefaultEvent("Click"),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class ButtonbarControl
    Inherits UIControl


#Region "Controles"

    Private _barnav As HtmlGenericControl

    Private _dropdown As HtmlGenericControl

    Private _buttonBar As LinkButton

    Private _buttonBarListener As TextBox

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventClick As New Object()

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public ReadOnly Property Buttons As List(Of ButtonItem) = New List(Of ButtonItem)

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public ReadOnly Property DropdownButtons As List(Of ButtonItem) = New List(Of ButtonItem)

    Public Property IndexSelected As Integer = 0

    Public Property ButtonSource As List(Of ButtonItem)

        Get

            EnsureChildControls()

            Return LocalStorage.Buttons

        End Get

        Set(value As List(Of ButtonItem))

            EnsureChildControls()

            LocalStorage = New ButtonsStorage With {.Buttons = value, .Dropdown = DropdownButtons}

            setButtonsBar()

        End Set

    End Property
    Private Property LocalStorage As ButtonsStorage

        Get

            Try

                Return New JavaScriptSerializer().Deserialize(Of ButtonsStorage)(ViewState("LocalStorage"))

            Catch ex As Exception

                Return Nothing

            End Try

        End Get

        Set(value As ButtonsStorage)

            If value IsNot Nothing Then

                ViewState("LocalStorage") = New JavaScriptSerializer().Serialize(value)

            End If

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

    Private Sub ButtonBarClick(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        _IndexSelected = Integer.Parse(source.Attributes.Item("index-selected"))

        _dropdown.Visible = False

        OnClick(EventArgs.Empty)

    End Sub

    Private Sub ButtonBarDropdown(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        _dropdown.Visible = Not _dropdown.Visible

    End Sub

#End Region

#Region "Metodos"

    Private Sub setButtonsBar()

        If LocalStorage Is Nothing Then

            LocalStorage = New ButtonsStorage With {.Buttons = Buttons, .Dropdown = DropdownButtons}

        End If

        Dim autoIcrement_ = 0

        With _barnav

            .Controls.Clear()

            If LocalStorage.Buttons.Count Then

                For Each button_ As ButtonItem In LocalStorage.Buttons

                    If button_.Visible = True Then

                        Dim buttonBar_ = New LinkButton

                        With buttonBar_

                            .Text = button_.Text

                            .CausesValidation = button_.IsSubmit

                            Dim cssSettings_ As New List(Of String)

                            .Attributes.Add("index-selected", autoIcrement_)

                            If button_.Icon IsNot Nothing Then

                                .Attributes.Add("icon", "true")

                                cssSettings_.Add("--icon:url(../../imgs/" & button_.Icon & ")")

                            End If

                            If button_.Enabled = False Then

                                .Attributes.Add("disabled", "disabled")

                            End If

                            If Not button_.Color = Nothing Then

                                cssSettings_.Add("--tintColor: " & button_.Color)

                            End If

                            If cssSettings_.Count Then

                                .Attributes.Add("style", Strings.Join(cssSettings_.ToArray(), "; "))

                            End If

                            AddHandler .Click, AddressOf ButtonBarClick

                        End With

                        .Controls.Add(buttonBar_)

                    End If

                    autoIcrement_ += 1

                Next

            End If

            If LocalStorage.Dropdown.Count Then

                _dropdown.Controls.Clear()

                .Controls.Add(New LiteralControl("			<label class='m-0 position-relative __tree'>"))

                .Controls.Add(_buttonBar)

                For Each button_ As ButtonItem In LocalStorage.Dropdown

                    If button_.Visible = True Then

                        Dim buttonBar_ = New LinkButton

                        With buttonBar_

                            .CausesValidation = button_.IsSubmit

                            Dim cssSettings_ As New List(Of String)

                            .Text = button_.Text

                            .Attributes.Add("index-selected", autoIcrement_)

                            If button_.Icon IsNot Nothing Then

                                .Attributes.Add("icon", "true")

                                cssSettings_.Add("--icon:url(../../imgs/" & button_.Icon & ")")

                            End If

                            If button_.Enabled = False Then

                                .Attributes.Add("disabled", "disabled")

                            End If

                            If Not button_.Color = Nothing Then

                                cssSettings_.Add("--tintColor: " & button_.Color)

                            End If

                            If cssSettings_.Count Then

                                .Attributes.Add("style", Strings.Join(cssSettings_.ToArray(), "; "))

                            End If

                            AddHandler .Click, AddressOf ButtonBarClick

                        End With

                        _dropdown.Controls.Add(buttonBar_)

                    End If

                    autoIcrement_ += 1

                Next

                .Controls.Add(_dropdown)

                .Controls.Add(New LiteralControl("			</label>"))

            End If

        End With

    End Sub

    Private Sub SettingButtonbarControls()

        _barnav = New HtmlGenericControl("nav")

        With _barnav

            .Attributes.Add("class", "wc-buttonbar d-flex justify-content-between __toolbar")

            .Attributes.Add("style", "--tintColor: " & ForeColor.ToHex)

        End With

        _buttonBar = New LinkButton

        With _buttonBar

            .Text = "Acciones"

            .CausesValidation = False

            AddHandler .Click, AddressOf ButtonBarDropdown

        End With


        _dropdown = New HtmlGenericControl("nav")

        With _dropdown

            .Attributes.Add("class", "__dropdown")

            .Visible = False

        End With

    End Sub


    Protected Overrides Sub CreateChildControls()

        SettingButtonbarControls()

        Dim component_ = New HtmlGenericControl("div")

        With component_

            .Attributes.Add("class", CssClass)

            .Controls.Add(_barnav)

            setButtonsBar()

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

Public Class ButtonsStorage

    Public Sub New()
        MyBase.New()
    End Sub

    Public Property Buttons As List(Of ButtonItem)

    Public Property Dropdown As List(Of ButtonItem)

End Class

Public Class ButtonItem

    Public Sub New()
        MyBase.New()
    End Sub

    Public Property Icon As String

    Public Property Text As String

    Public Property Enabled As Boolean = True

    Public Property Visible As Boolean = True

    Public Property Color As String

    Public Property IsSubmit As Boolean = False

    Public Property ID As String

End Class