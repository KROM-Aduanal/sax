Imports System.Security.Permissions
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web
Imports Gsol.krom
Imports System.Web.Script.Serialization
Imports Rec.Globals.Utils

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
                        Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
                        Level:=AspNetHostingPermissionLevel.Minimal),
DefaultEvent("SelectedIndexChanged"),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class SelectControl
    Inherits UIInputControlDataConnector

#Region "Controles"

    Private _selectControl As DropDownList

    Private _selectControlText As TextBox

    Private _dropdownButton As LinkButton

    Private _dropdownList As HtmlGenericControl

    Private _unlokedControl As HtmlGenericControl

    Private _selectControlFinder As TextBox

    Private _tapGesture As LinkButton

    Private _validationsElements As HtmlGenericControl

    Private _tooltipControlText As TextBox

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventSelectedIndexChanged As New Object()

    Private Shared ReadOnly EventClick As New Object()

    Private Shared ReadOnly EventTextChanged As New Object()

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public ReadOnly Property Options As List(Of SelectOption) = New List(Of SelectOption)

    Property HasDetails As Boolean = False

    Property SearchBarEnabled As Boolean

        Get

            If ViewState("SearchBarEnabled") IsNot Nothing Then

                Return ViewState("SearchBarEnabled")

            End If

            Return True

        End Get

        Set(value As Boolean)

            ViewState("SearchBarEnabled") = value

        End Set

    End Property

    Property Template As String

    Property LocalSearch As Boolean

        Get

            If ViewState("LocalSearch") IsNot Nothing Then

                Return ViewState("LocalSearch")

            End If

            Return True

        End Get

        Set(value As Boolean)

            ViewState("LocalSearch") = value

        End Set

    End Property

    Public Property DataSource As List(Of SelectOption)

        Get

            EnsureChildControls()

            Return LocalStorage

        End Get

        Set(value As List(Of SelectOption))

            EnsureChildControls()

            LocalStorage = value

            SetSelectDataSource()

        End Set

    End Property
    Private Property LocalStorage As List(Of SelectOption)

        Get

            Try

                Return New JavaScriptSerializer().Deserialize(Of List(Of SelectOption))(ViewState("LocalStorage"))

            Catch ex As Exception

                Return Nothing

            End Try

        End Get

        Set(value As List(Of SelectOption))

            If value IsNot Nothing Then

                ViewState("LocalStorage") = New JavaScriptSerializer().Serialize(value)

            Else

                ViewState("LocalStorage") = False

            End If

        End Set

    End Property

    Public Overloads Property Value As String

        Get

            EnsureChildControls()

            Return _selectControl.Text

        End Get

        Set(value As String)

            EnsureChildControls()

            _selectControl.Text = value

            If value = Nothing Then

                _selectControlText.Text = String.Empty

            End If

        End Set

    End Property

    Public ReadOnly Property Text As String

        Get

            EnsureChildControls()

            If _selectControl.SelectedItem IsNot Nothing Then

                Return _selectControl.SelectedItem.Text

            End If

            Return Nothing

        End Get

    End Property

    Public Overrides Property Signature As String

        Get

            EnsureChildControls()

            Dim data_ = LocalStorage

            If data_ IsNot Nothing Then

                Dim item_ = data_.FirstOrDefault(Function(e) e.Value = _selectControl.SelectedValue)

                If item_ IsNot Nothing Then

                    Return item_.Signature

                End If

            End If

            Return Nothing

        End Get

        Set(value As String)

            MyBase.Signature = value

        End Set

    End Property


    Public ReadOnly Property SuggestedText As String

        Get

            EnsureChildControls()

            Return _selectControlFinder.Text

        End Get

    End Property

    Public Overrides Property Enabled As Boolean

        Get

            Return MyBase.Enabled

        End Get

        Set(value As Boolean)

            If _selectControl IsNot Nothing Then

                If value = True Then

                    _selectControl.Attributes.Remove("disabled")

                    _dropdownButton.Enabled = True

                Else

                    _selectControl.Attributes.Add("disabled", "disabled")

                    _dropdownButton.Enabled = False

                End If

            End If

            MyBase.Enabled = value

        End Set

    End Property

    Public Property Dropped As Boolean
        Get

            EnsureChildControls()

            Return _dropdownList.Visible

        End Get
        Set(value As Boolean)

            EnsureChildControls()

            _tapGesture.Visible = value

            _dropdownList.Visible = value

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

    Public Custom Event SelectedIndexChanged As EventHandler

        AddHandler(ByVal value As EventHandler)

            Events.AddHandler(EventSelectedIndexChanged, value)

        End AddHandler

        RemoveHandler(ByVal value As EventHandler)

            Events.RemoveHandler(EventSelectedIndexChanged, value)

        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As System.EventArgs)

            CType(Events(EventSelectedIndexChanged), EventHandler).Invoke(sender, e)

        End RaiseEvent

    End Event

    Protected Overridable Sub OnSelectedIndexChanged(ByVal e As EventArgs)

        Dim submitHandler As EventHandler = CType(Events(EventSelectedIndexChanged), EventHandler)

        If submitHandler IsNot Nothing Then

            submitHandler(Me, e)

        End If

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

    Private Sub SelectChangedIndex(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        _selectControlText.Text = Text

        _dropdownList.Visible = False

        _tapGesture.Visible = False

        OnSelectedIndexChanged(EventArgs.Empty)

    End Sub

    Private Sub OnDropdownChange(ByVal source As Object, ByVal E As EventArgs)

        EnsureChildControls()

        If _dropdownList.Visible = False Then

            OnClick(EventArgs.Empty)

            If Not Dimension = IEnlaceDatos.TiposDimension.SinDefinir And DataEntity IsNot Nothing Then

                Dim Form_ = New Template.FormularioGeneralWeb()

                With DataEntity

                    .cmp(KeyField)

                    .cmp(DisplayField)

                End With

                Dim dataTable_ = Form_.ConsultarEnlace(entidad_:=DataEntity,
                                                dimension_:=Dimension,
                                                granularidad_:=Granularity,
                                                clausulasLibres_:=FreeClauses)


                If dataTable_ IsNot Nothing Then

                    Dim rows_ = New List(Of SelectOption)

                    For Each row_ As DataRow In dataTable_.Rows

                        rows_.Add(New SelectOption With {.Text = row_.Item(DisplayField), .Value = row_.Item(KeyField)})

                    Next

                    LocalStorage = rows_

                End If

                SetSelectDataSource()

                _dropdownList.Visible = True

                _tapGesture.Visible = True

            Else

                If Options.Count Then

                    LocalStorage = Options

                End If

                SetSelectDataSource()

                _dropdownList.Visible = True

                _tapGesture.Visible = True

                'OnClick(EventArgs.Empty)

            End If

        Else

            _dropdownList.Visible = False

            _tapGesture.Visible = False

        End If

    End Sub

    Private Sub OnFinderTextChange(ByVal source As Object, ByVal E As EventArgs)

        EnsureChildControls()

        If Not String.IsNullOrEmpty(_selectControlFinder.Text) Then

            OnTextChanged(EventArgs.Empty)

        End If

    End Sub

    Private Sub SelectCloseDropdown(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        _dropdownList.Visible = False

        _tapGesture.Visible = False

    End Sub

#End Region

#Region "Metodos"

    Private Sub SetSelectDataSource()

        Dim localStorage_ = LocalStorage

        If localStorage_ Is Nothing Then

            If Options.Count > 0 Then

                LocalStorage = Options

            End If

        Else

            If localStorage_.Count = 1 Then

                If String.IsNullOrEmpty(localStorage_(0).Value) And String.IsNullOrEmpty(localStorage_(0).Text) Then

                    LocalStorage = Options

                End If

            End If

        End If

        If LocalStorage IsNot Nothing Then

            Dim lastValue_ = Value

            Dim lastText_ = Text

            _selectControl.DataTextField = "Text"

            _selectControl.DataValueField = "Value"

            _selectControl.DataSource = LocalStorage

            _selectControl.DataBind()

            If lastValue_ IsNot Nothing And Not String.IsNullOrEmpty(lastValue_) Then

                Try

                    Value = lastValue_

                Catch ex As Exception

                    Dim li = New ListItem()

                    li.Text = lastText_

                    li.Value = lastValue_

                    _selectControl.Items.Insert(0, li)

                    Value = lastValue_

                End Try

            Else

                Dim li = New ListItem()

                li.Text = String.Empty

                li.Value = String.Empty

                _selectControl.Items.Insert(0, li)

            End If

        Else

            _selectControl.Items.Clear()

            _selectControl.DataBind()

        End If

    End Sub

#End Region

#Region "Renderizado"

    Private Sub SettingSelectControls()

        _selectControl = New DropDownList

        With _selectControl

            .ID = ID

            .ClientIDMode = ClientIDMode.AutoID

            .EnableViewState = True

            .AutoPostBack = True

            .Attributes.Add("class", "d-none")

            .Attributes.Add("is", "wc-select")

            .Attributes.Add("template", _Template)

            .DataTextField = "Text"

            .DataValueField = "Value"

            .DataSource = Options

            .DataBind()

            .Items.Insert(0, New ListItem With {.Text = String.Empty, .Value = String.Empty})

            AddHandler .SelectedIndexChanged, AddressOf SelectChangedIndex

        End With

        _selectControlText = New TextBox

        With _selectControlText

            .Attributes.Add("class", "col")

            .ReadOnly = True

            .Attributes.Add("placeholder", Label)

        End With


        _dropdownButton = New LinkButton

        With _dropdownButton

            .Attributes.Add("class", "col-auto icon-arrow __down")

            .CausesValidation = False

            .ID = "down_"

            AddHandler .Click, AddressOf OnDropdownChange

        End With


        _dropdownList = New HtmlGenericControl("div")

        With _dropdownList

            .Attributes.Add("class", "row no-gutters __dropdown")

            .Visible = False

        End With

        _unlokedControl = New HtmlGenericControl("div")

        With _unlokedControl

            .Attributes.Add("class", "col-auto")

        End With


        _selectControlFinder = New TextBox

        With _selectControlFinder

            .Attributes.Add("class", "__search")

            .Attributes.Add("placeholder", "Buscar")

            .Attributes.Add("autocomplete", "off")

            If LocalSearch = False Then

                .Attributes.Add("server-mode", "true")

                .AutoPostBack = True

                AddHandler .TextChanged, AddressOf OnFinderTextChange

            End If

        End With

        _tapGesture = New LinkButton

        With _tapGesture

            .CausesValidation = False

            .Visible = False

            .Attributes.Add("class", "__gesture")

            AddHandler .Click, AddressOf SelectCloseDropdown

        End With

        _validationsElements = New HtmlGenericControl("div")

        With _validationsElements

            .Attributes.Add("class", "wc-select-valid")

        End With

        _tooltipControlText = New TextBox

        With _tooltipControlText

            .Attributes.Add("class", "__tooltip d-none")

            .Attributes.Add("is", "wc-tooltip")

        End With

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingSelectControls()

        Dim component_ = New HtmlGenericControl("div")

        With component_

            .Attributes.Add("class", CssClass)

            .Controls.Add(New LiteralControl("<div class='wc-select __component position-relative mb-3' " & ForeColor.HtmlPropertyColor & ">"))

            .Controls.Add(_selectControl)

            .Controls.Add(New LiteralControl("	<div class='row no-gutters align-items-center __control position-relative'>"))

            .Controls.Add(_selectControlText)

            .Controls.Add(New LiteralControl("		<div class='col-auto'>"))

            .Controls.Add(_dropdownButton)

            .Controls.Add(New LiteralControl("		</div>"))

            .Controls.Add(_unlokedControl)

            .Controls.Add(New LiteralControl("		<label for='' class='float-form-control m-0'>"))

            .Controls.Add(New LiteralControl("			<span>" & Label & "</span>"))

            .Controls.Add(New LiteralControl("	    </label>"))

            .Controls.Add(_validationsElements)

            .Controls.Add(New LiteralControl("	</div>"))

            With _dropdownList

                .Controls.Add(New LiteralControl("		<div class='col'>"))

                .Controls.Add(New LiteralControl("              <ul class='list-unstyled p-0 m-0 d-flex flex-wrap __items'></ul>"))

                If SearchBarEnabled = True Then

                    .Controls.Add(_selectControlFinder)

                End If

                If Not _Template Is Nothing Then

                    .Controls.Add(New LiteralControl("	        <a class='btn btn-link btn-block m-0 __add'>Agregar</a>"))

                    .Controls.Add(New LiteralControl("			<i class='d-block w-100 border-bottom'></i>"))

                End If

                If _HasDetails = True Then

                    .Controls.Add(New LiteralControl("			<a class='btn btn-link btn-block m-0 __find'>Buscar Más</a>"))

                End If

                .Controls.Add(New LiteralControl("		</div>"))

            End With

            .Controls.Add(_dropdownList)

            .Controls.Add(_tapGesture)

            .Controls.Add(_tooltipControlText)

            .Controls.Add(New LiteralControl("</div>"))

        End With

        SetInputRules(_validationsElements)

        Me.Controls.Add(component_)

    End Sub

    Protected Overrides Sub Render(ByVal component_ As HtmlTextWriter)

        If Locked = True Then

            _unlokedControl.Controls.Add(SetLockedControl())

        End If

        GetToolTipSetting(_tooltipControlText)

        Me.RenderContents(component_)

    End Sub

    Protected Overrides Sub RecreateChildControls()

        EnsureChildControls()

    End Sub

    Protected Overrides Sub AddAttributesToRender(writer As HtmlTextWriter)

        MyBase.AddAttributesToRender(writer)

    End Sub

#End Region

End Class
