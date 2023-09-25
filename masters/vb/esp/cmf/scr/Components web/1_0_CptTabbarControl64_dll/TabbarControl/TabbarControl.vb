Imports System.Security.Permissions
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web
Imports System.Drawing

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
                        Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
                        Level:=AspNetHostingPermissionLevel.Minimal),
DefaultEvent("CheckedChanged"),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class TabbarControl
    Inherits UIControl

#Region "Controls"

    Private _tabbarSection As HtmlGenericControl

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventCheckedChanged As New Object()

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public ReadOnly Property Tabs As List(Of TabItem) = New List(Of TabItem)

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public ReadOnly Property TabsSections As List(Of FieldsetControl) = New List(Of FieldsetControl)

    Public Property TabId As Integer

        Get

            Return IIf(ViewState("TabId"), ViewState("TabId"), 1)

        End Get

        Set(value As Integer)

            ViewState("TabId") = value

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

    Private Sub TabbarCheckedChanged(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        TabId = Convert.ToInt32(source.InputAttributes.Item("value"))

        SetTabbarSections()

        OnCheckedChanged(EventArgs.Empty)

    End Sub

#End Region

#Region "Metodos"

    Private Sub SetTabbarSections()

        Dim indice_ = 1

        For Each section_ As FieldsetControl In TabsSections

            With section_

                .Priority = False

                .Label = Nothing

                .Detail = Nothing

                If TabId = indice_ Then

                    .Visible = True

                Else

                    .Visible = False

                End If

            End With

            _tabbarSection.Controls.Add(section_)

            indice_ += 1

        Next

    End Sub

    Private Sub SettingTabbarControls()

        _tabbarSection = New HtmlGenericControl("div")

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingTabbarControls()

        Dim component_ = New HtmlGenericControl("div")

        With component_

            .Attributes.Add("class", "wc-tabbar w-100 position-relative mb-5 __component")

            If Not ForeColor = Color.Empty Then

                .Attributes.Add("style", "--tintColor: " & ForeColor.ToHex & "")

            End If

            .Controls.Add(New LiteralControl("<div class='row no-gutters pl-4 pr-4 mb-4'>"))
            .Controls.Add(New LiteralControl("  <div class='col-xs-6 d-flex'>"))

            Dim indice_ = 1

            For Each tab_ As TabItem In Tabs

                .Controls.Add(New LiteralControl("<Label Class='flex-grow m-0'>"))
                .Controls.Add(New LiteralControl(tab_.Text))

                Dim input_ = New RadioButton

                With input_

                    .AutoPostBack = True

                    .InputAttributes.Add("value", indice_)

                    .GroupName = ID

                    If indice_ = 1 Then

                        input_.Checked = True

                    End If

                    AddHandler .CheckedChanged, AddressOf TabbarCheckedChanged

                End With

                .Controls.Add(input_)

                .Controls.Add(New LiteralControl("   <span></span>"))
                .Controls.Add(New LiteralControl("</label>"))

                indice_ += 1

            Next

            .Controls.Add(New LiteralControl("  </div>"))
            .Controls.Add(New LiteralControl("</div>"))

            SetTabbarSections()

            .Controls.Add(_tabbarSection)

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

Public Class TabItem

    Public Sub New()
        MyBase.New()
    End Sub

    Public Property Text As String

End Class
