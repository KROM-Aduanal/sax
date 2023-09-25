Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Security.Permissions
Imports System.Web
Imports System.ComponentModel
Imports System.Web.UI.HtmlControls
Imports Gsol.krom
Imports Rec.Globals.Utils
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
Public Class ListboxControl
    Inherits UIInputControlDataConnector

#Region "Controles"

    Private _findBoxText As TextBox

    Private _findBoxValue As TextBox

    Private _suggestionsDropdown As HtmlGenericControl

    Private _suggestionData As HtmlGenericControl

    Private _tapGesture As LinkButton

    Private _findBoxItems As HtmlGenericControl

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventClick As New Object()

    Private Shared ReadOnly EventTextChanged As New Object()

    Public Overloads Property Value As List(Of SelectOption)

        Get

            EnsureChildControls()

            Return New JavaScriptSerializer().Deserialize(Of List(Of SelectOption))(_findBoxValue.Text)

        End Get

        Set(value As List(Of SelectOption))

            EnsureChildControls()

            _findBoxValue.Text = New JavaScriptSerializer().Serialize(value)

        End Set

    End Property

    Public Property Text As String

        Get

            EnsureChildControls()

            Return _findBoxText.Text

        End Get

        Set(value As String)

            EnsureChildControls()

            _findBoxText.Text = value

        End Set

    End Property

    Public WriteOnly Property DataSource As List(Of SelectOption)

        Set(value As List(Of SelectOption))

            EnsureChildControls()

            If value IsNot Nothing Then

                If value.Count Then

                    With _suggestionData

                        .Controls.Clear()

                        For Each item_ As SelectOption In value

                            If item_.Delete = False Then

                                .Controls.Add(New LiteralControl("<li><a class='link-unstyled d-block' index-path=" & item_.Indice & " id='" & item_.Value & "'>" & item_.Text & "</a></li>"))

                            End If

                        Next

                    End With

                    _suggestionsDropdown.Visible = True

                    _tapGesture.Visible = True

                Else

                    _suggestionsDropdown.Visible = False

                    _tapGesture.Visible = False

                End If

            Else

                _suggestionsDropdown.Visible = False

                _tapGesture.Visible = False

            End If

        End Set

    End Property

    Public Overrides Property Enabled As Boolean

        Get

            Return MyBase.Enabled

        End Get

        Set(value As Boolean)

            If _findBoxText IsNot Nothing Then

                If value = True Then

                    _findBoxText.Attributes.Remove("disabled")

                Else

                    _findBoxText.Attributes.Add("disabled", "disabled")

                End If

            End If

            MyBase.Enabled = value

        End Set

    End Property

#End Region

#Region "Constructor"
#End Region

#Region "Events"

    Public Custom Event TextChanged As EventHandler

        AddHandler(ByVal value As EventHandler)

            Events.AddHandler(EventTextChanged, value)

        End AddHandler

        RemoveHandler(ByVal value As EventHandler)

            Events.RemoveHandler(EventTextChanged, value)

        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As System.EventArgs)

            CType(Events(EventTextChanged), EventHandler).Invoke(sender, e)

        End RaiseEvent

    End Event

    Protected Overridable Sub OnTextChanged(ByVal e As EventArgs)

        Dim submitHandler As EventHandler = CType(Events(EventTextChanged), EventHandler)

        If submitHandler IsNot Nothing Then

            submitHandler(Me, e)

        End If

    End Sub

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

    Private Sub FindboxChangeValue(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        _suggestionsDropdown.Visible = False

        _tapGesture.Visible = False

        _findBoxText.Text = Nothing

        If Not source.Text = Nothing Then

            'OnClick(EventArgs.Empty)

        End If

    End Sub

    Private Sub FindboxChangeText(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        If String.IsNullOrEmpty(_findBoxText.Text) Then

            _suggestionsDropdown.Visible = False

            _tapGesture.Visible = False

        Else

            If Not Dimension = IEnlaceDatos.TiposDimension.SinDefinir And DataEntity IsNot Nothing Then

                Dim Form_ = New Template.FormularioGeneralWeb()

                With DataEntity

                    .cmp(KeyField)

                    .cmp(DisplayField)

                End With

                FreeClauses = FreeClauses & " & " & DisplayField & " like %" & Text & "%"

                Dim dataTable_ As DataTable = Form_.ConsultarEnlace(entidad_:=DataEntity,
                                              dimension_:=Dimension,
                                              granularidad_:=Granularity,
                                              clausulasLibres_:=FreeClauses)

                With _suggestionData

                    .Controls.Clear()

                    For Each dataRow_ As DataRow In dataTable_.Rows

                        .Controls.Add(New LiteralControl("<li><a class='link-unstyled d-block' id='" & dataRow_.Item(KeyField) & "'>" & dataRow_.Item(DisplayField) & "</a></li>"))

                    Next

                End With

                _suggestionsDropdown.Visible = True

                _tapGesture.Visible = True

            Else

                OnTextChanged(EventArgs.Empty)

            End If

        End If

    End Sub

    Private Sub FindboxCloseDropdown(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        _suggestionsDropdown.Visible = False

        _tapGesture.Visible = False

        _findBoxText.Text = Nothing

    End Sub

#End Region

#Region "Metodos"

    Private Function GetData() As List(Of SelectOption)

        EnsureChildControls()

        Return New JavaScriptSerializer().Deserialize(Of List(Of SelectOption))(_findBoxValue.Text)

    End Function

    Private Sub SettingFindboxControls()

        _findBoxText = New TextBox

        With _findBoxText

            .Attributes.Add("placeholder", Label)

            .Attributes.Add("class", "__value")

            .Attributes.Add("autocomplete", "off")

            .AutoPostBack = True

            AddHandler .TextChanged, AddressOf FindboxChangeText

        End With


        _findBoxValue = New TextBox

        With _findBoxValue

            .ID = ID

            .AutoPostBack = True

            .Attributes.Add("class", "d-none __key")

            .Attributes.Add("is", "wc-listbox")

            AddHandler .TextChanged, AddressOf FindboxChangeValue

        End With

        _suggestionsDropdown = New HtmlGenericControl("div")

        With _suggestionsDropdown

            .Visible = False

            .Attributes.Add("class", "row no-gutters __dropdown")

        End With


        _suggestionData = New HtmlGenericControl("ul")

        With _suggestionData

            .Attributes.Add("class", "list-unstyled p-0 m-0 __content")

        End With


        _findBoxItems = New HtmlGenericControl("ul")

        With _findBoxItems

            .Attributes.Add("class", "__items")

        End With

        _tapGesture = New LinkButton

        With _tapGesture

            .CausesValidation = False

            .Visible = False

            AddHandler .Click, AddressOf FindboxCloseDropdown

        End With

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingFindboxControls()

        Dim component_ = New HtmlGenericControl("div")

        With component_

            .Attributes.Add("class", CssClass)

            .Controls.Add(New LiteralControl("<div class='wc-listbox __component position-relative mb-3' " & ForeColor.HtmlPropertyColor & ">"))

            .Controls.Add(New LiteralControl("	<div class='row no-gutters align-items-center'>"))

            .Controls.Add(New LiteralControl("		<div class='col-auto pl-2'>"))

            .Controls.Add(New LiteralControl("          <a class='col-auto icon-search pointer-events-none __loader'></a>"))

            .Controls.Add(New LiteralControl("		</div>"))

            .Controls.Add(New LiteralControl("		<div class='col position-relative'>"))

            .Controls.Add(_findBoxText)

            .Controls.Add(New LiteralControl("			<label class='float-form-control'>"))

            .Controls.Add(New LiteralControl("				<span>" & Label & "</span>"))

            .Controls.Add(New LiteralControl("		    </label>"))

            .Controls.Add(_findBoxValue)

            .Controls.Add(New LiteralControl("		</div>"))

            .Controls.Add(New LiteralControl("	</div>"))

            .Controls.Add(_findBoxItems)

            With _suggestionsDropdown

                .Controls.Add(New LiteralControl("		<div class='col'>"))

                .Controls.Add(_suggestionData)

                .Controls.Add(New LiteralControl("		</div>"))

            End With

            .Controls.Add(_suggestionsDropdown)

            .Controls.Add(_tapGesture)

            .Controls.Add(New LiteralControl("</div>"))

        End With

        Me.Controls.Add(component_)

    End Sub

    Public Sub PrepareItems()

        Dim items_ = GetData()

        If items_ IsNot Nothing Then

            For Each val As SelectOption In items_

                Dim disabledActions_ = IIf(Enabled = True, "", "pointer-events-none")

                Dim toggle_ = IIf(val.Delete = True, "class='d-none'", "")

                Dim item_ = New LiteralControl("<li " & toggle_ & " index-path=" & val.Indice & " id=" & val.Value & "><a class='" & disabledActions_ & "'></a>" & val.Text & "</li>")

                _findBoxItems.Controls.Add(item_)

            Next
        End If

    End Sub

    Protected Overrides Sub Render(ByVal component_ As HtmlTextWriter)

        PrepareItems()

        Me.RenderContents(component_)

    End Sub

    Protected Overrides Sub RecreateChildControls()

        EnsureChildControls()

    End Sub

#End Region

End Class

