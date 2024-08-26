Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports Gsol.krom
Imports Syn.Documento
Imports Wma.Exceptions

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
    Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level:=AspNetHostingPermissionLevel.Minimal),
DefaultEvent("Click"),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class FindbarControl
    Inherits UIInputControl

#Region "Controles"

    Private _findBarText As TextBox

    Private _findBarValue As TextBox

    Private _itemsContent As HtmlGenericControl

    Private _dropdown As HtmlGenericControl

    Private _clearButton As LinkButton

    Private _checkboxs As List(Of CheckBox)

    Private _tapGesture As LinkButton

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventClick As New Object()

    Private Shared ReadOnly EventTextChanged As New Object()

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public ReadOnly Property Filters As List(Of BarFilter) = New List(Of BarFilter)

    Property MinimumCharacters As Integer

    Public Property DataObject As Object

    Public Property MetadatosFilter As Dictionary(Of [Enum], String)

    Public Overloads Property Value As String

        Get

            EnsureChildControls()

            Return _findBarValue.Text

        End Get

        Set(value As String)

            EnsureChildControls()

            _findBarValue.Text = value

        End Set

    End Property

    Public Property Text As String

        Get

            EnsureChildControls()

            Return _findBarText.Text

        End Get

        Set(value As String)

            EnsureChildControls()

            _findBarText.Text = value

        End Set

    End Property

    Public Overrides Property Enabled As Boolean

        Get

            Return MyBase.Enabled

        End Get

        Set(value As Boolean)

            If _findBarText IsNot Nothing Then

                If value = True Then

                    _findBarText.Attributes.Remove("disabled")

                Else

                    _findBarText.Attributes.Add("disabled", "disabled")

                End If

            End If

            MyBase.Enabled = value

        End Set
    End Property

    Public WriteOnly Property DataSource As Dictionary(Of Object, Object)

        Set(value As Dictionary(Of Object, Object))

            EnsureChildControls()

            With _dropdown

                .Attributes.Remove("class")

                .Attributes.Add("class", "row no-gutters __dropdown")

                If value IsNot Nothing Then

                    With _itemsContent

                        For Each section As KeyValuePair(Of Object, Object) In value

                            .Controls.Add(New LiteralControl("<Label Class='d-block m-0'>"))

                            .Controls.Add(New LiteralControl("     <input type='checkbox' class='d-none'/>"))

                            .Controls.Add(New LiteralControl("     <span Class='font-weight-bold'>" & section.Key & "</span>"))

                            .Controls.Add(New LiteralControl("     <ul Class='list-unstyled pl-4 m-0 d-none'>"))

                            For Each item As Dictionary(Of Object, Object) In section.Value

                                Dim disableItem_ = Nothing

                                If item.Item("Value") = Me.Value Then

                                    disableItem_ = "disabled"

                                End If

                                .Controls.Add(New LiteralControl("<li>"))

                                .Controls.Add(New LiteralControl("     <a href='' " & disableItem_ & " id='" & item.Item("Value") & "' class='btn link-unstyled text-left p-1 m-0'>" & item.Item("Text") & "</a>"))

                                .Controls.Add(New LiteralControl("</li>"))

                            Next

                            .Controls.Add(New LiteralControl("     </ul>"))

                            .Controls.Add(New LiteralControl("</label>"))

                        Next

                    End With

                    _dropdown.Visible = True

                    _tapGesture.Visible = True

                End If

            End With

        End Set

    End Property


#End Region

#Region "Constructor"
#End Region

#Region "Eventos"

    Public Sub addFilter(idSection_ As Integer, idField_ As Integer, Text_ As String)

        Dim barFilter_ As New BarFilter With {.IdField = idField_, .IdSection = idSection_, .Text = Text_}

        Filters.Add(barFilter_)

    End Sub

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

    Private Sub FindbarChangeText(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        If Not String.IsNullOrEmpty(Text) Then

            If DataObject Is Nothing And Filters.Count = 0 Then

                OnTextChanged(EventArgs.Empty)

            Else

                Dim dataSource_ = New Dictionary(Of Object, Object)

                Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                    Using _entidadDatos As IEntidadDatos = DataObject

                        Dim documentoElectronico_ As DocumentoElectronico = _entidadDatos

                        For Each checkbox_ As CheckBox In _checkboxs

                            If checkbox_.Checked = True Then

                                Dim idField = Convert.ToInt32(checkbox_.InputAttributes.Item("idfield"))

                                Dim idSection = Convert.ToInt32(checkbox_.InputAttributes.Item("idsection"))

                                Dim titlesection = checkbox_.InputAttributes.Item("titlesection")

                                Dim itemList = New List(Of Dictionary(Of Object, Object))

                                Dim status_ = Nothing

                                If MetadatosFilter Is Nothing Then

                                    status_ = _enlaceDatos.BusquedaGeneralDocumento(documentoElectronico_, idSection, idField, Text)

                                Else

                                    status_ = _enlaceDatos.BusquedaGeneralDocumento(documentoElectronico_, idSection, idField, Text, MetadatosFilter)

                                End If

                                If status_.Status = TagWatcher.TypeStatus.Ok Then

                                    For Each item_ As Dictionary(Of Object, Object) In status_.ObjectReturned

                                        Dim dataSourceItem_ = New Dictionary(Of Object, Object)

                                        dataSourceItem_.Add("Value", item_.Item("ID"))

                                        Dim titleItem_ = Nothing

                                        If item_.Item("valorOperacion") = item_.Item("folioOperacion") Then

                                            titleItem_ = item_.Item("valorOperacion")

                                        Else

                                            titleItem_ = item_.Item("valorOperacion") & " | " & item_.Item("folioOperacion")

                                        End If

                                        Dim indexFound_ = titleItem_.ToLower.IndexOf(Text.ToLower)

                                        If indexFound_ >= 0 Then

                                            Dim stringFound_ = titleItem_.Substring(indexFound_, Text.Length)

                                            titleItem_ = Replace(titleItem_, stringFound_, "<span>" & stringFound_ & "</span>")

                                        End If

                                        dataSourceItem_.Add("Text", titleItem_)

                                        itemList.Add(dataSourceItem_)

                                    Next

                                    Dim seccionName_ = titlesection

                                    If itemList.Count Then

                                        dataSource_.Add("(<b>" & itemList.Count & "</b>)" & seccionName_, itemList)

                                    End If

                                End If

                            End If

                        Next

                        If dataSource_.Count Then

                            DataSource = dataSource_

                        Else

                            With _dropdown

                                .Attributes.Remove("class")

                                .Attributes.Add("class", "row no-gutters __dropdown")

                                With _itemsContent

                                    .Controls.Add(New LiteralControl("<p class='p-4 text-center'>No se ha encontrado ningún resultado</p>"))

                                End With

                                _dropdown.Visible = True

                                _tapGesture.Visible = True

                            End With

                        End If

                    End Using

                End Using

            End If

        Else

            _dropdown.Visible = False

            _tapGesture.Visible = False

        End If

    End Sub

    Private Sub FindbarChangeValue(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        _dropdown.Visible = False

        _tapGesture.Visible = False

        If Not source.Text = Nothing And Not source.Text = "-1" Then

            OnClick(EventArgs.Empty)

        End If

    End Sub

    Private Sub FindbarClearInputText(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        _findBarText.Text = Nothing

        _findBarValue.Text = Nothing

        _dropdown.Visible = False

        _tapGesture.Visible = False

        OnClick(EventArgs.Empty)

    End Sub

    Private Sub FindbarCloseDropdown(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        _tapGesture.Visible = False

        _dropdown.Visible = False

    End Sub

#End Region

#Region "Metodos"

    Private Sub SettingFindboxControls()

        _findBarText = New TextBox

        With _findBarText

            .Attributes.Add("placeholder", Label)

            .Attributes.Add("class", "col __value")

            .Attributes.Add("autocomplete", "off")

            .AutoPostBack = True

            .ID = "Finder" & ID

            AddHandler .TextChanged, AddressOf FindbarChangeText

        End With


        _findBarValue = New TextBox

        With _findBarValue

            .ID = ID

            .Attributes.Add("class", "d-none __key")

            .Attributes.Add("minimum-characters", MinimumCharacters)

            .Attributes.Add("Is", "wc-findbar")

            .AutoPostBack = True

            AddHandler .TextChanged, AddressOf FindbarChangeValue

        End With


        _itemsContent = New HtmlGenericControl("div")

        With _itemsContent

            Dim border_ = If(Filters.Count > 0, " border-left ", "")

            .Attributes.Add("class", "col p-2 " & border_ & " __content")

        End With


        _dropdown = New HtmlGenericControl("div")

        With _dropdown

            .Attributes.Add("class", "row no-gutters d-none __dropdown")

            .Visible = False

        End With


        _clearButton = New LinkButton

        With _clearButton

            .Attributes.Add("class", "col-auto d-none __close")

            .CausesValidation = False

            AddHandler .Click, AddressOf FindbarClearInputText

        End With

        _checkboxs = New List(Of CheckBox)

        _tapGesture = New LinkButton

        With _tapGesture

            .CausesValidation = False

            .Visible = False

            AddHandler .Click, AddressOf FindbarCloseDropdown

        End With

    End Sub

    Public Sub GetFilters(ByRef component_ As HtmlGenericControl)

        With component_

            If Filters.Count Then

                .Controls.Add(New LiteralControl("<div class='col-auto p-2 __filters contracted'"))

                .Controls.Add(New LiteralControl("	<small class='text-muted text-uppercase font-weight-bold'>Filtros</small>"))

                Dim indexPath_ = 0

                For Each filter_ As BarFilter In _Filters

                    .Controls.Add(New LiteralControl("<label class='d-flex align-items-center m-0'>"))

                    Dim input_ = New CheckBox

                    With input_

                        .Checked = True

                        .InputAttributes.Add("idfield", _Filters(indexPath_).IdField)

                        .InputAttributes.Add("idsection", _Filters(indexPath_).IdSection)

                        .InputAttributes.Add("titlesection", _Filters(indexPath_).Text)

                    End With

                    _checkboxs.Add(input_)

                    .Controls.Add(_checkboxs(indexPath_))

                    .Controls.Add(New LiteralControl(filter_.Text))

                    .Controls.Add(New LiteralControl("</label>"))

                    indexPath_ += 1

                Next

                .Controls.Add(New LiteralControl("</div>"))

            End If

        End With

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingFindboxControls()

        Dim component_ = New HtmlGenericControl("div")

        With component_

            .Attributes.Add("class", CssClass)

            .Controls.Add(New LiteralControl("<div class='wc-findbar position-relative __component' " & ForeColor.HtmlPropertyColor & ">"))

            .Controls.Add(New LiteralControl("	<div class='row no-gutters align-items-center'>"))

            .Controls.Add(New LiteralControl("		<div class='col-auto pl-2'>"))

            .Controls.Add(New LiteralControl("          <a class='col-auto icon-search pointer-events-none __loader'></a>"))

            .Controls.Add(New LiteralControl("		</div>"))

            .Controls.Add(New LiteralControl("		<div class='col'>"))

            .Controls.Add(_findBarText)

            .Controls.Add(_findBarValue)

            .Controls.Add(New LiteralControl("		</div>"))

            .Controls.Add(New LiteralControl("		<div class='col-auto pr-2'>"))

            .Controls.Add(_clearButton)

            .Controls.Add(New LiteralControl("		</div>"))

            .Controls.Add(New LiteralControl("	</div>"))

            GetFilters(_dropdown)

            _dropdown.Controls.Add(_itemsContent)

            .Controls.Add(_dropdown)

            .Controls.Add(_tapGesture)

            .Controls.Add(New LiteralControl("</div>"))

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

Public Class BarFilter
    Public Sub New()
        MyBase.New()
    End Sub

    Public Property Text As String

    Public Property IdField As Integer

    Public Property IdSection As Integer

End Class

