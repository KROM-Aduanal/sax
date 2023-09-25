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
Public Class PaginatorControl
    Inherits UIControl

#Region "Controles"

    Private _previewPagButton As LinkButton

    Private _nextPagButton As LinkButton

    Private _pagItems As List(Of RadioButton)

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventCheckedChange As New Object()

    Public Property PageIndex As Integer

        Get

            Return IIf(ViewState("PageIndex") IsNot Nothing, ViewState("PageIndex"), 1)

        End Get

        Set(value As Integer)

            ViewState("PageIndex") = value

        End Set

    End Property

    Private Property CurrentPage As Integer

        Get

            Return ViewState("CurrentPage")

        End Get

        Set(value As Integer)

            ViewState("CurrentPage") = value

        End Set

    End Property

    Public Property NumberItems As Integer = 1

    Public Property ItemsPage As Integer = 1

#End Region

#Region "Constructor"
#End Region

#Region "Eventos"

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

    Private Sub PaginatorPageSelected(ByVal source As RadioButton, ByVal e As EventArgs)

        EnsureChildControls()

        PageIndex = Integer.Parse(source.Text)

        OnCheckedChanged(EventArgs.Empty)

    End Sub

    Private Sub PaginatorToNextPage(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        Dim totalPages_ = Math.Ceiling(_NumberItems / _ItemsPage)

        Dim currentPage_ = If(CurrentPage = Nothing, 1, CurrentPage)

        If currentPage_ < totalPages_ Then

            CurrentPage = currentPage_ + 1

            For item_ As Integer = 1 To _ItemsPage

                Dim Page_ = (CurrentPage * _ItemsPage) - (_ItemsPage - item_)

                _pagItems(item_ - 1).Text = Page_

                _pagItems(item_ - 1).Checked = IIf(PageIndex = Page_, True, False)

            Next

        End If

    End Sub

    Private Sub PaginatorToPreviewPage(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        Dim currentPage_ = If(CurrentPage = Nothing, 1, CurrentPage)

        If currentPage_ > 1 Then

            CurrentPage = currentPage_ - 1

            For item_ As Integer = 1 To _ItemsPage

                Dim Page_ = (CurrentPage * _ItemsPage) - (_ItemsPage - item_)

                _pagItems(item_ - 1).Text = Page_

                _pagItems(item_ - 1).Checked = IIf(PageIndex = Page_, True, False)

            Next

        End If

    End Sub

#End Region

#Region "Metodos"

    Private Sub SettingPaginatorControls()

        _previewPagButton = New LinkButton

        With _previewPagButton

            .Attributes.Add("class", "__preview")

            .CausesValidation = False

            AddHandler .Click, AddressOf PaginatorToPreviewPage

        End With


        _nextPagButton = New LinkButton

        With _nextPagButton

            .Attributes.Add("class", "__next")

            .CausesValidation = False

            AddHandler .Click, AddressOf PaginatorToNextPage

        End With


        _pagItems = New List(Of RadioButton)

        Dim currentPage_ = If(CurrentPage = Nothing, 1, CurrentPage)

        For item_ As Integer = 1 To _ItemsPage

            Dim Page_ = (currentPage_ * _ItemsPage) - (_ItemsPage - item_)

            Dim pagButton_ = New RadioButton

            With pagButton_

                .Text = Page_

                .Checked = IIf(PageIndex = Page_, True, False)

                .GroupName = "pag-number"

                .AutoPostBack = True

                AddHandler .CheckedChanged, AddressOf PaginatorPageSelected

            End With

            _pagItems.Add(pagButton_)

        Next

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingPaginatorControls()

        Dim component_ = New HtmlGenericControl("div")

        With component_

            .Attributes.Add("class", CssClass)

            .Controls.Add(New LiteralControl("    <nav class='wc-paginator d-flex align-items-center'>"))

            For Each pagButton_ As RadioButton In _pagItems

                .Controls.Add(pagButton_)

            Next

            .Controls.Add(_previewPagButton)

            .Controls.Add(_nextPagButton)

            .Controls.Add(New LiteralControl("	</nav>"))

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


