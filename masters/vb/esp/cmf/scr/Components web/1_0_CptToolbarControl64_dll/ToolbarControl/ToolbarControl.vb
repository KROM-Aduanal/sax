Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
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
Public Class ToolbarControl
    Inherits UIControl

#Region "Controles"

    Private _barnav As HtmlGenericControl

    Private _buttonBarListener As TextBox

    Private _currentPage As TextBox

    Private _numberPages As TextBox

    Private _previewPagButton As LinkButton

    Private _nextPagButton As LinkButton

    Private _paginatorNumberIndicator As LinkButton

#End Region

#Region "Propiedades"

    Private PageIndex As Integer = 1

    Private PagesNumber As Integer = 1

    Private Shared ReadOnly EventClick As New Object()

    Private Shared ReadOnly EventCheckedChange As New Object()

    Public WriteOnly Property SetPage As Integer

        Set(value As Integer)

            EnsureChildControls()

            PageIndex = value

            _currentPage.Text = PageIndex

        End Set

    End Property

    Public ReadOnly Property GetPage As Integer

        Get

            EnsureChildControls()

            PageIndex = Convert.ToInt32(_currentPage.Text)

            Return PageIndex

        End Get

    End Property

    Public Property NumberItems As Integer

        Get

            EnsureChildControls()

            PagesNumber = Convert.ToInt32(_numberPages.Text)

            Return PagesNumber

        End Get

        Set(value As Integer)

            If value Then

                EnsureChildControls()

                PagesNumber = value

                _numberPages.Text = PagesNumber

                _paginatorNumberIndicator.Text = PageIndex & " de " & PagesNumber

            End If

        End Set

    End Property

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property Buttons As List(Of ToolbarItem) = New List(Of ToolbarItem)

    Public Property IndexSelected As Integer = 0

    Public Property GetLastPage As Integer = 0

    Public Property ButtonSource As List(Of ToolbarItem)

        Get

            EnsureChildControls()

            Return LocalStorage

        End Get

        Set(value As List(Of ToolbarItem))

            EnsureChildControls()

            LocalStorage = value

            setButtonsBar()

        End Set

    End Property
    Private Property LocalStorage As List(Of ToolbarItem)

        Get

            Try

                Return New JavaScriptSerializer().Deserialize(Of List(Of ToolbarItem))(ViewState("LocalStorage"))

            Catch ex As Exception

                Return Nothing

            End Try

        End Get

        Set(value As List(Of ToolbarItem))

            If value IsNot Nothing Then

                If value.Count > 0 Then

                    ViewState("LocalStorage") = New JavaScriptSerializer().Serialize(value)

                End If

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

    Private Sub ButtonBarClick(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        IndexSelected = Integer.Parse(source.Attributes.Item("index-selected"))

        OnClick(EventArgs.Empty)

    End Sub

    Private Sub NavigationToNextPage(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        GetLastPage = Integer.Parse(_currentPage.Text)

        If GetLastPage < NumberItems Then

            PageIndex = GetLastPage + 1

            _currentPage.Text = PageIndex

            _paginatorNumberIndicator.Text = PageIndex & " de " & NumberItems

            OnCheckedChanged(EventArgs.Empty)

        End If

    End Sub

    Private Sub NavigationToPreviewPage(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        GetLastPage = Integer.Parse(_currentPage.Text)

        If GetLastPage > 1 Then

            PageIndex = GetLastPage - 1

            _currentPage.Text = PageIndex

            _paginatorNumberIndicator.Text = PageIndex & " de " & NumberItems

            OnCheckedChanged(EventArgs.Empty)

        End If

    End Sub

#End Region

#Region "Metodos"

    Private Sub setButtonsBar()

        If LocalStorage Is Nothing Then

            LocalStorage = Buttons

        End If

        Dim autoIcrement_ = 0

        With _barnav

            .Controls.Clear()

            If LocalStorage IsNot Nothing Then

                If LocalStorage.Count Then

                    For Each button_ As ToolbarItem In LocalStorage

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

            End If

            .Controls.Add(_previewPagButton)

            _paginatorNumberIndicator.Text = _currentPage.Text & " de " & _numberPages.Text

            .Controls.Add(_paginatorNumberIndicator)

            .Controls.Add(_nextPagButton)

        End With

    End Sub

    Private Sub SettingButtonbarControls()

        _barnav = New HtmlGenericControl("nav")

        With _barnav

            .Attributes.Add("class", "wc-buttonbar d-flex justify-content-between __toolbar")

            .Attributes.Add("style", "--tintColor: " & ForeColor.ToHex)

        End With


        _previewPagButton = New LinkButton

        With _previewPagButton

            .Attributes.Add("class", "__preview")

            .Attributes.Add("icon", "true")

            .Attributes.Add("style", "--icon:url(../../imgs/preview-arrow.png); --tintColor: #36215b")

            .CausesValidation = False

            AddHandler .Click, AddressOf NavigationToPreviewPage

        End With


        _nextPagButton = New LinkButton

        With _nextPagButton

            .Attributes.Add("class", "__next")

            .Attributes.Add("icon", "true")

            .Attributes.Add("style", "--icon:url(../../imgs/next-arrow.png); --tintColor: #36215b")

            .CausesValidation = False

            AddHandler .Click, AddressOf NavigationToNextPage

        End With


        _currentPage = New TextBox

        With _currentPage

            .Text = PageIndex

            .Attributes.Add("class", "d-none")

        End With

        _numberPages = New TextBox

        With _numberPages

            .Text = PagesNumber

            .Attributes.Add("class", "d-none")

        End With

        _paginatorNumberIndicator = New LinkButton

        With _paginatorNumberIndicator

            .CausesValidation = False

            .Attributes.Add("style", "--tintColor: #36215b")

        End With

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingButtonbarControls()

        Dim component_ = New HtmlGenericControl("div")

        With component_

            .Attributes.Add("class", CssClass)

            .Controls.Add(_barnav)

            .Controls.Add(_currentPage)

            .Controls.Add(_numberPages)

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

<Serializable>
Public Class ToolbarItem

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