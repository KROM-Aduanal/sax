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
Public Class CardControl
    Inherits UIControl

#Region "Enums"
#End Region

#Region "Controles"

    Private _closeButton As LinkButton

#End Region

#Region "Propiedades"

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property ListControls As List(Of WebControl) = New List(Of WebControl)

#End Region

#Region "Constructor"
#End Region

#Region "Eventos"

    Private Sub CloseCard(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        Visible = False

    End Sub

#End Region

#Region "Metodos"

    Private Sub SettingCardControls()

        _closeButton = New LinkButton

        With _closeButton

            .CausesValidation = False

            AddHandler .Click, AddressOf CloseCard

        End With

    End Sub

#End Region

#Region "Renderizado"

    Protected Overrides Sub CreateChildControls()

        SettingCardControls()

        Dim card_ = New HtmlGenericControl("div")

        With card_

            .Attributes.Add("class", "w-100")

            .Controls.Add(New LiteralControl("<div class='wc-card __component position-relative mt-4 mb-4' " & ForeColor.HtmlPropertyColor & ">"))

            .Controls.Add(New LiteralControl("<div>"))

            .Controls.Add(New LiteralControl("  <b>" & Label & "</b>"))

            .Controls.Add(_closeButton)

            .Controls.Add(New LiteralControl("</div>"))

            .Controls.Add(New LiteralControl("<div>"))

            If _ListControls.Count Then

                For Each control_ As Object In _ListControls

                    control_.Enabled = Enabled

                    .Controls.Add(control_)

                Next

            End If

            .Controls.Add(New LiteralControl("</div>"))

            .Controls.Add(New LiteralControl("</div>"))

        End With

        Me.Controls.Add(card_)

    End Sub

    Protected Overrides Sub Render(ByVal component_ As HtmlTextWriter)

        Me.RenderContents(component_)

    End Sub

    Protected Overrides Sub RecreateChildControls()

        EnsureChildControls()

    End Sub

#End Region

End Class


