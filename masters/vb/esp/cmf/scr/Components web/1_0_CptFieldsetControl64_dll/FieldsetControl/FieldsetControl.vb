Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web
Imports System.Security.Permissions

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
    Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level:=AspNetHostingPermissionLevel.Minimal),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class FieldsetControl
    Inherits UIControl

#Region "Enums"
#End Region

#Region "Controles"

    Private _collapseButton As HtmlGenericControl

    Private _legend As HtmlGenericControl

    Private _collapseTrigger As CheckBox

    Private _fieldsetContainer As HtmlGenericControl

#End Region

#Region "Propiedades"

    Property Priority() As Boolean = True

    Property Detail() As String

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property ListControls As List(Of WebControl) = New List(Of WebControl)

    Public Property Collapsed As Boolean = True

#End Region

#Region "Constructor"
#End Region

#Region "Eventos"

    Private Sub FieldsetCollapsed(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        With _fieldsetContainer

            .Attributes.Remove("class")

            If _collapseTrigger.Checked = True Then

                .Attributes.Add("class", "row no-gutters")

                _collapseButton.Attributes.Add("icon", "-")

            Else

                .Attributes.Add("class", "row no-gutters d-none")

                _collapseButton.Attributes.Add("icon", "+")

            End If

        End With

    End Sub

#End Region

#Region "Metodos"

    Private Sub SettingFieldsetControls()

        _legend = New HtmlGenericControl("legend")

        With _legend

            Dim legendText_ = If(Detail Is Nothing, Label, Detail)

            .Controls.Add(New LiteralControl(legendText_))

        End With


        _collapseTrigger = New CheckBox

        With _collapseTrigger

            .Checked = Collapsed

            .AutoPostBack = True

            .ID = ID

            .InputAttributes.Add("class", "d-none")

            AddHandler .CheckedChanged, AddressOf FieldsetCollapsed

        End With


        _collapseButton = New HtmlGenericControl("label")

        With _collapseButton

            .Attributes.Add("class", "__collapse")

            Dim icon_ = If(Collapsed = True, "-", "+")

            .Attributes.Add("icon", icon_)

        End With


        _fieldsetContainer = New HtmlGenericControl("div")

        With _fieldsetContainer

            Dim visible_ = If(Collapsed = True, "", " d-none")

            .Attributes.Add("class", "row no-gutters w-100" & visible_)

        End With

    End Sub

#End Region

#Region "Renderizado"

    Protected Overrides Sub CreateChildControls()

        SettingFieldsetControls()

        Dim fieldset_ = New HtmlGenericControl("fieldset")

        With fieldset_

            .Attributes.Add("class", "row no-gutters " & CssClass)

            .Attributes.Add("label", Label)

            .Attributes.Add("section-id", ID)

            If _Priority = False Then

                .Attributes.Add("is-sub", Not _Priority)

            End If

            If Not Label Is Nothing Then

                _legend.Controls.Add(_collapseButton)

                .Controls.Add(_legend)

            End If

            .Controls.Add(_collapseTrigger)

            If _ListControls.Count Then

                For Each control_ As Object In _ListControls

                    control_.Enabled = Enabled

                    _fieldsetContainer.Controls.Add(control_)

                Next

            End If

            .Controls.Add(_fieldsetContainer)

        End With

        Me.Controls.Add(fieldset_)

    End Sub

    Protected Overrides Sub Render(ByVal component_ As HtmlTextWriter)

        Me.RenderContents(component_)

    End Sub

    Protected Overrides Sub RecreateChildControls()

        EnsureChildControls()

    End Sub

#End Region

End Class


