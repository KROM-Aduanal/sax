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
Public Class NavigationControl
    Inherits UIControl

#Region "Controles"

    Private _componentContainer As HtmlGenericControl

    Private _rangeInputControl As TextBox

    Private _currentPage As TextBox

    Private _previewPagButton As LinkButton

    Private _nextPagButton As LinkButton

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventCheckedChange As New Object()

    Public Property PageIndex As Integer = 1

    Public Property NumberItems As Integer = 0

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

    Private Sub NavigationPageSelected(ByVal source As TextBox, ByVal e As EventArgs)

        EnsureChildControls()

        PageIndex = Integer.Parse(source.Text)

        _currentPage.Text = PageIndex

        SettingNavigationBar()

        OnCheckedChanged(EventArgs.Empty)

    End Sub

    Private Sub NavigationToNextPage(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        _rangeInputControl.Text = Integer.Parse(_rangeInputControl.Text) + 1

        PageIndex = Integer.Parse(_rangeInputControl.Text)

        _currentPage.Text = PageIndex

        SettingNavigationBar()

        OnCheckedChanged(EventArgs.Empty)

    End Sub

    Private Sub NavigationToPreviewPage(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        _rangeInputControl.Text = Integer.Parse(_rangeInputControl.Text) - 1

        PageIndex = Integer.Parse(_rangeInputControl.Text)

        _currentPage.Text = PageIndex

        SettingNavigationBar()

        OnCheckedChanged(EventArgs.Empty)

    End Sub

#End Region

#Region "Metodos"

    Private Sub SettingNavigationBar()

        Dim percent = Math.Round(((PageIndex / NumberItems) * 100), MidpointRounding.AwayFromZero)

        _componentContainer.Attributes.Add("style", "--barPercent:" & percent & "%")

    End Sub

    Private Sub SettingNavigationControls()

        _componentContainer = New HtmlGenericControl("div")

        With _componentContainer

            .Attributes.Add("class", "wc-navigation __component")

        End With

        SettingNavigationBar()


        _rangeInputControl = New TextBox

        With _rangeInputControl

            .Text = "1"

            .TextMode = TextBoxMode.Range

            .Attributes.Add("min", "1")

            .Attributes.Add("max", _NumberItems)

            .AutoPostBack = True

            AddHandler .TextChanged, AddressOf NavigationPageSelected

        End With


        _currentPage = New TextBox

        With _currentPage

            .ReadOnly = True

            .Text = PageIndex

        End With


        _previewPagButton = New LinkButton

        With _previewPagButton

            .Attributes.Add("class", "__preview")

            .CausesValidation = False

            AddHandler .Click, AddressOf NavigationToPreviewPage

        End With


        _nextPagButton = New LinkButton

        With _nextPagButton

            .Attributes.Add("class", "__next")

            .CausesValidation = False

            AddHandler .Click, AddressOf NavigationToNextPage

        End With

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingNavigationControls()

        Dim component_ = New HtmlGenericControl("div")

        component_.Attributes.Add("class", CssClass)

        With _componentContainer

            .Controls.Add(New LiteralControl("        <div class='row no-gutters'>"))

            .Controls.Add(New LiteralControl("            <div Class='col-auto'>"))

            .Controls.Add(_previewPagButton)

            .Controls.Add(New LiteralControl("            </div>"))

            .Controls.Add(New LiteralControl("            <div class='col text-center'>"))

            .Controls.Add(_currentPage)

            .Controls.Add(New LiteralControl("                <span>de " & NumberItems & "</span>"))

            .Controls.Add(New LiteralControl("            </div>"))

            .Controls.Add(New LiteralControl("            <div class='col-auto'>"))

            .Controls.Add(_nextPagButton)

            .Controls.Add(New LiteralControl("            </div>"))

            .Controls.Add(New LiteralControl("        </div>"))

            .Controls.Add(New LiteralControl("        <div class='row no-gutters'>"))

            .Controls.Add(_rangeInputControl)

            .Controls.Add(New LiteralControl("            <div></div>"))

            .Controls.Add(New LiteralControl("        </div>"))

        End With

        component_.Controls.Add(_componentContainer)

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


