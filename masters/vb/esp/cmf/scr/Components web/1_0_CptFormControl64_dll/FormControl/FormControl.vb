Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Drawing
Imports System.Security.Permissions
Imports System.Web
Imports System.ComponentModel
Imports Gsol.Web.Components.ButtonbarControl

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
    Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level:=AspNetHostingPermissionLevel.Minimal),
 DefaultEvent("CheckedChanged"),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class FormControl
    Inherits UIControl

#Region "Enum"

    Enum ButtonbarModality
        [Default] = 0
        Open = 1
        Draft = 2
        Closed = 3
        [Protected] = 4
        Reading = 5
        Writting = 6
    End Enum

#End Region

#Region "Controles"

    Private _importantmarkButton As CheckBox

    Private _autosaveButton As CheckBox

    Private _panelContexts As UpdatePanel

    Private _serverObserver As CheckBox

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventCheckedChange As New Object()

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public ReadOnly Property Fieldsets As List(Of FieldsetControl) = New List(Of FieldsetControl)

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public ReadOnly Property Buttonbar As ButtonbarControl = New ButtonbarControl

    Property SaveConfirm As Boolean = True

    Property MenuHidden As Boolean = False

    Property HasAutoSave As Boolean = False

    Public Property Capped As Boolean

        Get

            Return ViewState("Capped")

        End Get

        Set(value As Boolean)

            ViewState("Capped") = value

        End Set

    End Property

    Public Property AutoSave As Boolean

    Public Property IsEditing As Boolean

        Get

            If Not String.IsNullOrEmpty(ViewState("IsEditing")) Then

                Return Convert.ToBoolean(ViewState("IsEditing"))

            End If

            Return False

        End Get

        Set(value As Boolean)

            ViewState("IsEditing") = value

        End Set

    End Property

    Private ReadOnly Property IsPopup As Boolean
        Get

            If System.Web.HttpContext.Current.Request.QueryString("is_popup") = "true" Then

                Return True

            End If

            Return False

        End Get
    End Property

    Public WriteOnly Property Modality As ButtonbarModality

        Set(value As ButtonbarModality)

            'ViewState("Modality") = value

            Dim buttons_ = New List(Of ButtonItem)

            With buttons_

                .Add(New ButtonItem With {.Icon = "more.png", .Visible = False, .Color = "#43189a", .Enabled = True})

                .Add(New ButtonItem With {.Text = "Guardar", .Visible = False, .IsSubmit = True, .Color = "#43189a", .Enabled = True})

                .Add(New ButtonItem With {.Text = "Publicar", .Visible = False, .IsSubmit = True, .Color = "#13006a", .Enabled = True})

                .Add(New ButtonItem With {.Icon = "w_lock.png", .Text = "Seguir Editando", .Visible = False, .Color = "#43189a", .Enabled = True})

                .Add(New ButtonItem With {.Icon = "w_lock.png", .Text = "Publicado", .Visible = False, .Color = "#43189a", .Enabled = True})

                .Add(New ButtonItem With {.Icon = "undo.png", .Visible = False, .Color = "#331289", .Enabled = True})

                .Add(New ButtonItem With {.Icon = "delete.png", .Visible = False, .Color = "#331289", .Enabled = True})

            End With

            Select Case value

                Case ButtonbarModality.Default

                    buttons_(0).Visible = True

                    buttons_(1).Visible = False

                    buttons_(2).Visible = False

                    buttons_(3).Visible = False

                    buttons_(4).Visible = False

                    buttons_(5).Visible = False

                    buttons_(6).Visible = False

                Case ButtonbarModality.Open

                    buttons_(0).Enabled = False

                    buttons_(0).Visible = True

                    buttons_(1).Visible = True

                    buttons_(2).Visible = False

                    buttons_(3).Visible = False

                    buttons_(4).Visible = False

                    buttons_(5).Visible = True

                    buttons_(6).Visible = True

                Case ButtonbarModality.Draft

                    buttons_(0).Enabled = False

                    buttons_(0).Visible = True

                    buttons_(1).Visible = True

                    buttons_(2).Visible = True

                    buttons_(3).Visible = False

                    buttons_(4).Visible = False

                    buttons_(5).Visible = True

                    buttons_(6).Visible = True

                Case ButtonbarModality.Closed

                    buttons_(0).Visible = True

                    buttons_(1).Visible = False

                    buttons_(2).Visible = False

                    buttons_(3).Visible = True

                    buttons_(4).Visible = False

                    buttons_(5).Visible = False

                    buttons_(6).Visible = False

                Case ButtonbarModality.Protected

                    buttons_(0).Visible = True

                    buttons_(1).Visible = False

                    buttons_(2).Visible = False

                    buttons_(3).Visible = False

                    buttons_(4).Visible = True

                    buttons_(5).Visible = False

                    buttons_(6).Visible = False

                Case ButtonbarModality.Reading

                    buttons_(0).Enabled = False

                    buttons_(0).Visible = True

                    buttons_(1).Visible = False

                    buttons_(2).Visible = False

                    buttons_(3).Visible = True

                    buttons_(4).Visible = False

                    buttons_(5).Visible = False

                    buttons_(6).Visible = False

                Case ButtonbarModality.Writting

                    buttons_(0).Enabled = False

                    buttons_(0).Visible = True

                    buttons_(1).Visible = True

                    buttons_(2).Visible = False

                    buttons_(3).Visible = False

                    buttons_(4).Visible = False

                    buttons_(5).Visible = True

                    buttons_(6).Visible = False

            End Select

            Buttonbar.ButtonSource = buttons_

        End Set

    End Property

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

    Private Sub SetCappedModule(ByVal source As CheckBox, ByVal e As EventArgs)

        EnsureChildControls()

        Capped = source.Checked

        OnCheckedChanged(EventArgs.Empty)

    End Sub

    Private Sub SetAutoSave(ByVal source As CheckBox, ByVal e As EventArgs)

        EnsureChildControls()

        AutoSave = source.Checked

        OnCheckedChanged(EventArgs.Empty)

    End Sub

#End Region

#Region "Metodos"

    Private Sub SettingFormControls()

        _importantmarkButton = New CheckBox

        With _importantmarkButton

            .AutoPostBack = True

            .InputAttributes.Add("class", "d-none")

            AddHandler .CheckedChanged, AddressOf SetCappedModule

        End With


        _autosaveButton = New CheckBox

        With _autosaveButton

            .InputAttributes.Add("class", "d-none __autosave")

            .AutoPostBack = True

            AddHandler .CheckedChanged, AddressOf SetAutoSave

        End With


        _panelContexts = New UpdatePanel

        With _panelContexts

            .UpdateMode = UpdatePanelUpdateMode.Conditional

            .Attributes.Add("class", "__contexs")

        End With


        _serverObserver = New CheckBox

        With _serverObserver

            .AutoPostBack = True

            .ID = ID & "_Observer"

            .InputAttributes.Add("class", "d-none __serverObserver")

        End With

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingFormControls()

        Dim control_ = New HtmlGenericControl("div")

        With control_

            .Attributes.Add("class", "wc-form w-100 align-items-stretch d-flex flex-colums position-relative __component")

            If Not ForeColor = Color.Empty Then

                .Attributes.Add("style", "--tintColor: " & ForeColor.ToHex.ToString)

            End If

            .Controls.Add(New LiteralControl("	<div class='row no-gutters align-items-center'>"))

            .Controls.Add(New LiteralControl("		<div class='col-auto p-4'>"))

            .Controls.Add(New LiteralControl("			<strong>"))

            If IsPopup = False Then

                .Controls.Add(New LiteralControl("			    <label>"))

                .Controls.Add(_importantmarkButton)

                .Controls.Add(New LiteralControl("			        <span></span>"))

                .Controls.Add(New LiteralControl("			    </label>"))

            End If

            .Controls.Add(New LiteralControl(Label))

            .Controls.Add(New LiteralControl("			</strong>"))

            If HasAutoSave = True And IsPopup = False Then

                .Controls.Add(New LiteralControl("&nbsp;"))

                .Controls.Add(New LiteralControl("				<label class='wc-checkbox'>"))

                .Controls.Add(_autosaveButton)

                .Controls.Add(New LiteralControl("				    <span text='Auto-Guardar' class='d-flex align-items-center'></span>"))

                .Controls.Add(New LiteralControl("				</label>"))

            End If


            .Controls.Add(New LiteralControl("		</div>"))

            .Controls.Add(New LiteralControl("		<div class='col d-flex justify-content-end p-4'>"))

            .Controls.Add(New LiteralControl("			<div class='col-auto flex-colums d-flex justify-content-end'>"))

            If IsPopup = False Then

                .Controls.Add(_Buttonbar)

            End If

            .Controls.Add(New LiteralControl("			</div>"))

            .Controls.Add(New LiteralControl("		</div>"))

            .Controls.Add(New LiteralControl("	</div>"))

            .Controls.Add(New LiteralControl("	<div class='row no-gutters flex-grow'>"))

            .Controls.Add(New LiteralControl("		<div class='col-auto p-4'>"))

            If IsPopup = False Then

                If MenuHidden = False Then

                    .Controls.Add(New LiteralControl("			<nav class='__navigation'>"))

                    If _Fieldsets.Count Then

                        Dim indexPath_ = 1

                        For Each fieldset_ As FieldsetControl In _Fieldsets

                            If fieldset_.Priority = True Then

                                Dim checked = If(indexPath_ = 1, "checked='checked'", "")

                                .Controls.Add(New LiteralControl("<Label Class='d-block position-relative'>"))

                                .Controls.Add(New LiteralControl("	<input " & checked & " type='radio' name='form_menu' class='d-none' value='" & indexPath_ & "' to-section='" & fieldset_.ID & "'/>"))

                                .Controls.Add(New LiteralControl("	<span>" & fieldset_.Label & " </span>"))

                                .Controls.Add(New LiteralControl("	<small></small>"))

                                .Controls.Add(New LiteralControl("</label>"))

                                indexPath_ += 1

                            End If

                        Next

                    End If

                End If

                .Controls.Add(New LiteralControl("			    </nav>"))

            Else

                .Controls.Add(New LiteralControl("          <a href='' class='d-block __back'></a>"))

            End If

            .Controls.Add(New LiteralControl("		</div>"))

            .Controls.Add(New LiteralControl("		<div class='col p-4 d-flex align-items-stretch position-relative __content'>"))

            If _Fieldsets.Count Then

                For Each fieldset_ As FieldsetControl In _Fieldsets

                    .Controls.Add(fieldset_)

                Next

            End If

            .Controls.Add(New LiteralControl("		</div>"))

            .Controls.Add(New LiteralControl("	</div>"))

            .Controls.Add(_panelContexts)

            .Controls.Add(_serverObserver)

        End With

        Dim updatePanel_ = New UpdatePanel

        With updatePanel_

            .UpdateMode = UpdatePanelUpdateMode.Always

            .Attributes.Add("class", "w-100")

            .ContentTemplateContainer.Controls.Add(control_)


        End With

        Me.Controls.Add(updatePanel_)

    End Sub

    Protected Overrides Sub Render(ByVal component_ As HtmlTextWriter)

        _importantmarkButton.Checked = Capped

        Me.RenderContents(component_)

    End Sub

    Protected Overrides Sub RecreateChildControls()

        EnsureChildControls()

    End Sub

#End Region

End Class

