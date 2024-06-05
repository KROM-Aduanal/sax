Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports Gsol.krom
Imports System.Web
Imports System.Security.Permissions
Imports System.Drawing
Imports System.Reflection
Imports System.Web.Script.Serialization
Imports System.ComponentModel
Imports Rec.Globals.Utils
Imports Syn.Nucleo.Recursos

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
    Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level:=AspNetHostingPermissionLevel.Minimal),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>"),
DefaultEvent("CheckedChange")
>
Public Class PillboxControl
    Inherits UIControlDataConnector

#Region "Enums"

    Enum ToolbarModality
        [Default] = 0
        Simple = 1
        Advanced = 2
    End Enum

    Enum ToolbarActions
        SinDefinir = 0
        Nuevo = 1
        Clonar = 2
        Borrar = 3
        Archivar = 4
        Anterior = 5
        Siguiente = 6
    End Enum

#End Region

#Region "Controles"

    Private _bodyPillbox As HtmlGenericControl

    Private _toolBar As ToolbarControl

    Private _textJsondata As TextBox

    Private _keyInputControl As InputControl

    Private _identityInputControl As InputControl

    Private _jsonPillboxTemplate As TextBox

#End Region

#Region "Propiedades"

    Public Event BeforeClick As EventHandler

    Private Shared ReadOnly EventClick As New Object()

    Private Shared ReadOnly EventCheckedChange As New Object()

    Public Property PillboxListControls As New List(Of WebControl)

    Private Property DataPill As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property ListControls As List(Of WebControl) = New List(Of WebControl)

    Private _toolbarAction As ToolbarActions = ToolbarActions.SinDefinir

    Public WriteOnly Property Modality As ToolbarModality

        Set(value As ToolbarModality)

            EnsureChildControls()

            Dim buttons_ = New List(Of ToolbarItem)

            With buttons_

                .Add(New ToolbarItem With {.Icon = "more.png", .Visible = False})

                .Add(New ToolbarItem With {.Icon = "copiar.png", .Visible = False})

                .Add(New ToolbarItem With {.Icon = "delete.png", .Visible = False})

                .Add(New ToolbarItem With {.Icon = "archivar.png", .Visible = False})

            End With

            If value = ToolbarModality.Simple Then

                buttons_(0).Visible = True

                buttons_(1).Visible = True

                buttons_(2).Visible = True

                buttons_(3).Visible = False

            ElseIf value = ToolbarModality.Advanced Then

                buttons_(0).Visible = True

                buttons_(1).Visible = True

                buttons_(2).Visible = False

                buttons_(3).Visible = True

            End If

            With _toolBar

                .ButtonSource = buttons_

            End With

        End Set

    End Property

    Private ReadOnly Property NumberItems As Integer

        Get

            Try

                Return FilterVisibleData(New JavaScriptSerializer().Deserialize(Of List(Of Dictionary(Of String, Object)))(_textJsondata.Text)).Count

            Catch ex As Exception

                Return 0

            End Try

        End Get

    End Property

    Public Property DataSource As List(Of Dictionary(Of String, Object))

        Get

            EnsureChildControls()

            Try

                Return New JavaScriptSerializer().Deserialize(Of List(Of Dictionary(Of String, Object)))(_textJsondata.Text)

            Catch ex As Exception

                Return Nothing

            End Try

        End Get

        Set(value As List(Of Dictionary(Of String, Object)))

            EnsureChildControls()

            If value IsNot Nothing Then

                _textJsondata.Text = New JavaScriptSerializer().Serialize(value)

            Else

                _toolBar.SetPage = 1

                _toolBar.NumberItems = 1

                prepareControlsDefaultValues(New Dictionary(Of String, Object))

            End If

        End Set

    End Property

    Private Function FilterVisibleData(ByVal dataSource_ As List(Of Dictionary(Of String, Object))) As List(Of Dictionary(Of String, Object))

        Dim filteredDataSource_ As New List(Of Dictionary(Of String, Object))

        If dataSource_ IsNot Nothing Then

            If dataSource_.Count > 0 Then

                For Each dataRow_ As Dictionary(Of String, Object) In dataSource_

                    If dataRow_.Item("borrado") = False And dataRow_.Item("archivado") = False Then

                        filteredDataSource_.Add(dataRow_)

                    End If

                Next

            End If

        End If

        Return filteredDataSource_

    End Function

    Public ReadOnly Property PageIndex As Integer

        Get

            EnsureChildControls()

            Return _toolBar.GetPage

        End Get

    End Property

    Public ReadOnly Property ToolbarAction As ToolbarActions

        Get

            Return _toolbarAction

        End Get

    End Property

#End Region

#Region "Constructor"
#End Region

#Region "Eventos"

    Public Overridable Sub OnBeforeClick()

        RaiseEvent BeforeClick(Me, EventArgs.Empty)

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

    Public Custom Event CheckedChange As EventHandler

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

    Protected Overridable Sub OnCheckedChange(ByVal e As EventArgs)

        Dim submitHandler As EventHandler = CType(Events(EventCheckedChange), EventHandler)

        If submitHandler IsNot Nothing Then

            submitHandler(Me, e)

        End If

    End Sub

    Private Sub NavigationToPage(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        PreparedDataPillbox()

        If _toolBar.GetLastPage > _toolBar.GetPage Then

            _toolbarAction = ToolbarActions.Anterior

        Else

            _toolbarAction = ToolbarActions.Siguiente

        End If

        OnCheckedChange(EventArgs.Empty)

    End Sub

    Private Sub ToolbarClickAction(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        Select Case _toolBar.IndexSelected

            Case 0

                _toolbarAction = ToolbarActions.Nuevo

                CreatePillbox()

            Case 1

                _toolbarAction = ToolbarActions.Clonar

                ClonePillbox()

            Case 2

                If NumberItems > 1 Then

                    _toolbarAction = ToolbarActions.Borrar

                    DeletePillbox()

                End If

            Case 3

                If NumberItems > 1 Then

                    _toolbarAction = ToolbarActions.Archivar

                    FiledPillbox()

                End If

            Case Else

        End Select

        OnClick(EventArgs.Empty)

    End Sub


#End Region

#Region "Metodos"

    Public Sub FiledPillbox()

        EnsureChildControls()

        Dim dataSource_ = New JavaScriptSerializer().Deserialize(Of List(Of Dictionary(Of String, Object)))(_textJsondata.Text)

        Dim filteredDataSource_ = FilterVisibleData(dataSource_)

        If filteredDataSource_ IsNot Nothing Then

            If filteredDataSource_.Count > 0 Then

                Dim page_ = _toolBar.GetPage - 1

                OnBeforeClick()

                'filteredDataSource_(page_).Item("archivado") = True

                dataSource_.ToList().ForEach(Sub(c As Dictionary(Of String, Object))

                                                 If c.Item("identidad") = filteredDataSource_(page_).Item("identidad") Then

                                                     c.Item("archivado") = True

                                                 End If

                                                 'Dim aux = From i As Dictionary(Of String, Object) In filteredDataSource_ Where i.Item("archivado") = True And i.Item("identidad") = c.Item("identidad") Select i.Item("archivado")

                                                 'If aux.Count > 0 Then c.Item("archivado") = aux(0)

                                             End Sub)

                _textJsondata.Text = New JavaScriptSerializer().Serialize(dataSource_)

                _toolBar.SetPage = IIf(page_ >= 1, page_, 1)

                PreparedDataPillbox()

            End If

        End If

    End Sub

    Public Sub DeletePillbox()

        EnsureChildControls()

        Dim dataSource_ = New JavaScriptSerializer().Deserialize(Of List(Of Dictionary(Of String, Object)))(_textJsondata.Text)

        Dim filteredDataSource_ = FilterVisibleData(dataSource_)

        If filteredDataSource_ IsNot Nothing Then

            If filteredDataSource_.Count > 0 Then

                Dim page_ = _toolBar.GetPage - 1

                OnBeforeClick()

                'filteredDataSource_(page_).Item("borrado") = True

                dataSource_.ToList().ForEach(Sub(c As Dictionary(Of String, Object))

                                                 If c.Item("identidad") = filteredDataSource_(page_).Item("identidad") Then

                                                     c.Item("borrado") = True

                                                 End If

                                                 'Dim aux = From i As Dictionary(Of String, Object) In filteredDataSource_ Where i.Item("borrado") = True And i.Item("identidad") = c.Item("identidad") Select i.Item("borrado")

                                                 'If aux.Count > 0 Then c.Item("borrado") = aux(0)

                                             End Sub)

                _textJsondata.Text = New JavaScriptSerializer().Serialize(dataSource_)

                _toolBar.SetPage = IIf(page_ >= 1, page_, 1)

                PreparedDataPillbox()

            End If

        End If

    End Sub

    Public Sub ClonePillbox()

        EnsureChildControls()

        Dim dataSource_ = New JavaScriptSerializer().Deserialize(Of List(Of Dictionary(Of String, Object)))(_textJsondata.Text)

        Dim filteredDataSource_ = FilterVisibleData(dataSource_)

        If filteredDataSource_ IsNot Nothing Then

            If filteredDataSource_.Count > 0 Then

                Dim page_ = _toolBar.GetPage - 1

                OnBeforeClick()

                Dim currentItem_ = filteredDataSource_(page_)

                Dim cloneItem_ = New Dictionary(Of String, Object)

                For Each control_ As Object In _PillboxListControls

                    If currentItem_.ContainsKey(control_.ID) Then

                        cloneItem_.Add(control_.ID, currentItem_.Item(control_.ID))

                    End If

                Next

                cloneItem_(KeyField) = 0

                cloneItem_("identidad") = dataSource_.Last.Item("identidad") + 1

                cloneItem_("borrado") = False

                cloneItem_("archivado") = False

                'filteredDataSource_.Add(cloneItem_)
                dataSource_.Add(cloneItem_)

                _textJsondata.Text = New JavaScriptSerializer().Serialize(dataSource_)

                PreparedDataPillbox(filteredDataSource_.Count + 1)

            End If

        End If

    End Sub

    Public Sub CreatePillbox()

        EnsureChildControls()

        Dim dataSource_ = New JavaScriptSerializer().Deserialize(Of List(Of Dictionary(Of String, Object)))(_textJsondata.Text)

        If dataSource_ IsNot Nothing Then

            If dataSource_.Count >= 0 Then

                OnBeforeClick()

                Dim newItem_ As New Dictionary(Of String, Object)

                For Each control_ As Object In _PillboxListControls

                    newItem_.Add(control_.ID, Nothing)

                    If control_.GetType = GetType(InputControl) Then

                        control_.Value = Nothing

                    ElseIf control_.GetType = GetType(SelectControl) Then

                        With control_

                            .DataSource = Nothing

                            .Value = Nothing

                        End With

                    ElseIf control_.GetType = GetType(SwitchControl) Then

                        control_.Checked = False

                    ElseIf control_.GetType = GetType(FindboxControl) Then

                        control_.Value = String.Empty

                        control_.Text = String.Empty

                    ElseIf control_.GetType = GetType(CatalogControl) Then

                        control_.DataSource = Nothing

                    End If

                Next

                newItem_.Add("borrado", False)

                newItem_.Add("archivado", False)

                newItem_("identidad") = dataSource_.Last.Item("identidad") + 1

                dataSource_.Add(newItem_)

                _textJsondata.Text = New JavaScriptSerializer().Serialize(dataSource_)

                PreparedDataPillbox(FilterVisibleData(dataSource_).Count)

            End If

        End If

    End Sub

    Public Sub PillBoxDataBinding()
        Dim i = 1
        For Each dataPill_ As Dictionary(Of String, Object) In DataPill

            If Not dataPill_.ContainsKey("borrado") Then

                dataPill_.Add("borrado", False)

            End If

            If Not dataPill_.ContainsKey("archivado") Then

                dataPill_.Add("archivado", False)

            End If

            'If Not dataPill_.ContainsKey("identidad") Then

            '    dataPill_.Add("identidad", i)

            'End If
            i += 1
        Next

        DataSource = DataPill

        PreparedDataPillbox()

    End Sub

    Public Sub ClearRows()

        DataPill = New List(Of Dictionary(Of String, Object))

    End Sub

    Public Sub ForEach(func_ As Action(Of PillBox))

        Dim jsonData_ = New JavaScriptSerializer().Deserialize(Of List(Of Dictionary(Of String, Object)))(_textJsondata.Text)

        If jsonData_ IsNot Nothing Then

            If jsonData_.Count Then

                For Each jsonRow_ As Object In jsonData_

                    Dim pillbox_ As New PillBox

                    pillbox_.Properties.Add(KeyField, jsonRow_.Item(KeyField))

                    For Each control_ As Object In _PillboxListControls

                        If control_.GetType = GetType(InputControl) Then

                            Dim temp_ = DirectCast(control_, InputControl)

                            If jsonRow_.ContainsKey(temp_.ID) Then

                                pillbox_.SetControlValue(temp_, jsonRow_.Item(temp_.ID))

                            End If

                        ElseIf control_.GetType = GetType(SwitchControl) Then

                            Dim temp_ = DirectCast(control_, SwitchControl)

                            If jsonRow_.ContainsKey(temp_.ID) Then

                                pillbox_.SetControlValue(temp_, jsonRow_.Item(temp_.ID))

                            End If

                        ElseIf control_.GetType = GetType(SelectControl) Then

                            Dim temp_ = DirectCast(control_, SelectControl)

                            If jsonRow_.ContainsKey(temp_.ID) Then

                                pillbox_.SetControlValue(temp_, New SelectOption With {.Value = jsonRow_.Item(temp_.ID).Item("Value"), .Text = jsonRow_.Item(temp_.ID).Item("Text")})

                            End If

                        ElseIf control_.GetType = GetType(FindboxControl) Then

                            Dim temp_ = DirectCast(control_, FindboxControl)

                            If jsonRow_.ContainsKey(temp_.ID) Then

                                pillbox_.SetControlValue(temp_, New SelectOption With {.Value = jsonRow_.Item(temp_.ID).Item("Value"), .Text = jsonRow_.Item(temp_.ID).Item("Text")})

                            End If

                        ElseIf control_.GetType = GetType(CatalogControl) Then

                            Dim temp_ = DirectCast(control_, CatalogControl)

                            If jsonRow_.ContainsKey(temp_.ID) Then

                                pillbox_.SetControlValue(temp_, jsonRow_.Item(temp_.ID))

                            End If

                        End If

                    Next

                    pillbox_.Properties.Add("borrado", jsonRow_.Item("borrado"))

                    pillbox_.Properties.Add("archivado", jsonRow_.Item("archivado"))

                    If Not func_ Is Nothing Then

                        func_(pillbox_)

                    End If

                Next

            End If

        End If

    End Sub

    Public Sub SetPillbox(func_ As Action(Of PillBox))

        If Not func_ Is Nothing Then

            Dim pillbox_ = New PillBox

            func_(pillbox_)

            DataPill.Add(pillbox_.Properties)

        End If

    End Sub

    Public Sub GetPillbox(func_ As Action(Of PillBox), Optional ByVal pagina_ As Integer = 0)

        If Not func_ Is Nothing Then

            If _PillboxListControls.Count Then

                Dim pillbox_ = New PillBox

                Dim dataSource_ = New JavaScriptSerializer().Deserialize(Of List(Of Dictionary(Of String, Object)))(_textJsondata.Text)

                Dim filteredDataSource_ = FilterVisibleData(dataSource_)

                If filteredDataSource_ IsNot Nothing Then

                    Dim row_ = If(pagina_ > 0, filteredDataSource_(pagina_ - 1), filteredDataSource_(PageIndex - 1))

                    For Each control_ As Object In _PillboxListControls

                        If control_.GetType = GetType(InputControl) Then

                            Dim temp_ = DirectCast(control_, InputControl)

                            pillbox_.SetControlValue(temp_, row_.Item(temp_.ID))

                        ElseIf control_.GetType = GetType(SwitchControl) Then

                            Dim temp_ = DirectCast(control_, SwitchControl)

                            pillbox_.SetControlValue(temp_, row_.Item(temp_.ID))

                        ElseIf control_.GetType = GetType(SelectControl) Then

                            Dim temp_ = DirectCast(control_, SelectControl)

                            If row_.Item(temp_.ID) IsNot Nothing Then

                                If Not String.IsNullOrEmpty(row_.Item(temp_.ID).Item("Value")) And Not String.IsNullOrEmpty(row_.Item(temp_.ID).Item("Text")) Then

                                    pillbox_.SetControlValue(temp_, New SelectOption With {.Value = row_.Item(temp_.ID).Item("Value"), .Text = row_.Item(temp_.ID).Item("Text")})

                                End If

                            End If

                        ElseIf control_.GetType = GetType(FindboxControl) Then

                            Dim temp_ = DirectCast(control_, FindboxControl)

                            If row_.Item(temp_.ID) IsNot Nothing Then

                                pillbox_.SetControlValue(temp_, New SelectOption With {.Value = row_.Item(temp_.ID).Item("Value"), .Text = row_.Item(temp_.ID).Item("Text")})

                            End If

                        ElseIf control_.GetType = GetType(CatalogControl) Then

                            Dim temp_ = DirectCast(control_, CatalogControl)

                            If row_.Item(temp_.ID) IsNot Nothing Then

                                pillbox_.SetControlValue(temp_, row_.Item(temp_.ID))

                            End If

                        End If

                    Next

                    pillbox_.Properties.Add("borrado", row_.Item("borrado"))

                    pillbox_.Properties.Add("archivado", row_.Item("archivado"))

                Else

                    filteredDataSource_ = New List(Of Dictionary(Of String, Object))

                    For Each control_ As Object In _PillboxListControls

                        If control_.GetType = GetType(InputControl) Then

                            Dim temp_ = DirectCast(control_, InputControl)

                            pillbox_.SetControlValue(temp_, temp_.Value)

                        ElseIf control_.GetType = GetType(SwitchControl) Then

                            Dim temp_ = DirectCast(control_, SwitchControl)

                            pillbox_.SetControlValue(temp_, temp_.Checked)

                        ElseIf control_.GetType = GetType(SelectControl) Then

                            Dim temp_ = DirectCast(control_, SelectControl)

                            pillbox_.SetControlValue(temp_, New SelectOption With {.Value = temp_.Value, .Text = temp_.Text})

                        ElseIf control_.GetType = GetType(FindboxControl) Then

                            Dim temp_ = DirectCast(control_, FindboxControl)

                            pillbox_.SetControlValue(temp_, New SelectOption With {.Value = temp_.Value, .Text = temp_.Text})

                        ElseIf control_.GetType = GetType(CatalogControl) Then

                            Dim temp_ = DirectCast(control_, CatalogControl)

                            pillbox_.SetControlValue(temp_, temp_.DataSource)

                        End If

                    Next

                    pillbox_.Properties.Add("borrado", False)

                    pillbox_.Properties.Add("archivado", False)

                End If

                func_(pillbox_)

                If filteredDataSource_.Count > 0 Then

                    Dim index_ = IIf(pagina_ > 0, pagina_ - 1, PageIndex - 1)

                    dataSource_.ToList().ForEach(Sub(c As Dictionary(Of String, Object))

                                                     If c.Item("identidad") = filteredDataSource_(index_).Item("identidad") Then

                                                         c = pillbox_.Properties

                                                     End If

                                                 End Sub)

                Else

                    dataSource_.Add(pillbox_.Properties)

                End If

                ClearRows()

                DataPill = dataSource_

                PillBoxDataBinding()

            End If

        End If

    End Sub

    Private Sub PreparedDataPillbox(Optional page_ As Integer = Nothing)

        If _PillboxListControls.Count Then

            Dim data_ = GetData()

            Dim row_ = New Dictionary(Of String, Object)

            If data_ IsNot Nothing Then

                If data_.Count >= 0 Then

                    _toolBar.NumberItems = data_.Count

                    If page_ > 0 Then

                        If data_.Count > 0 Then

                            row_ = data_(page_ - 1)

                        End If

                        _toolBar.SetPage = page_

                        _textJsondata.Attributes.Add("page", page_ - 1)

                    Else

                        If data_.Count > 0 Then

                            Dim currentPage_ = _toolBar.GetPage - 1

                            row_ = data_(currentPage_)

                        End If

                        _textJsondata.Attributes.Add("page", _toolBar.GetPage - 1)

                    End If

                End If

            End If

            prepareControlsDefaultValues(row_)

        End If

    End Sub

    Private Sub prepareControlsDefaultValues(ByRef row_ As Dictionary(Of String, Object))

        Dim data_ = New List(Of Dictionary(Of String, Object))

        Dim pillbox_ = New PillBox

        For Each control_ As Object In _PillboxListControls

            If row_.Count > 0 Then

                For Each item As KeyValuePair(Of String, Object) In row_

                    If item.Key = control_.ID Then

                        control_.CssClass = control_.CssClass & " " & control_.ID

                        If control_.GetType = GetType(InputControl) Then

                            control_.Value = item.Value

                        ElseIf control_.GetType = GetType(SelectControl) Then

                            If item.Value IsNot Nothing Then

                                With control_

                                    .DataSource = Nothing

                                    .Value = Nothing

                                    If Not String.IsNullOrEmpty(item.Value("Value")) And Not String.IsNullOrEmpty(item.Value("Text")) Then

                                        .DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = item.Value("Value"), .Text = item.Value("Text")}}

                                        .Value = item.Value("Value")

                                    End If

                                End With

                            End If

                        ElseIf control_.GetType = GetType(SwitchControl) Then

                            control_.Checked = item.Value

                        ElseIf control_.GetType = GetType(FindboxControl) Then

                            If item.Value IsNot Nothing Then

                                control_.Value = item.Value("Value")

                                control_.Text = item.Value("Text")

                            End If

                        ElseIf control_.GetType = GetType(CatalogControl) Then

                            control_.DataSource = item.Value

                        End If

                    End If

                Next

            Else

                control_.CssClass = control_.CssClass & " " & control_.ID

                If control_.GetType = GetType(InputControl) Then

                    If Not control_.ID = "identidad" Then

                        control_.Value = Nothing

                    End If

                    Dim temp_ = DirectCast(control_, InputControl)

                    pillbox_.SetControlValue(temp_, temp_.Value)

                ElseIf control_.GetType = GetType(SelectControl) Then

                    With control_

                        .DataSource = Nothing

                        .Value = Nothing

                    End With

                    Dim temp_ = DirectCast(control_, SelectControl)

                    pillbox_.SetControlValue(temp_, New SelectOption With {.Value = temp_.Value, .Text = temp_.Text})

                ElseIf control_.GetType = GetType(SwitchControl) Then

                    control_.Checked = False

                    Dim temp_ = DirectCast(control_, SwitchControl)

                    pillbox_.SetControlValue(temp_, temp_.Checked)

                ElseIf control_.GetType = GetType(FindboxControl) Then

                    Dim temp_ = DirectCast(control_, FindboxControl)

                    pillbox_.SetControlValue(temp_, New SelectOption With {.Value = temp_.Value, .Text = temp_.Text})

                ElseIf control_.GetType = GetType(CatalogControl) Then

                    Dim temp_ = DirectCast(control_, CatalogControl)

                    pillbox_.SetControlValue(temp_, temp_.DataSource)

                End If

            End If

        Next

        If row_.Count = 0 Then

            data_.Add(pillbox_.Properties)

            ClearRows()

            DataPill = data_

            PillBoxDataBinding()

        End If

    End Sub

    Private Function GetData() As List(Of Dictionary(Of String, Object))

        Dim dataSource_ = DataSource

        If dataSource_ IsNot Nothing Then

            If dataSource_.Count > 0 Then

                Return FilterVisibleData(dataSource_)

            End If

        Else

            If Not Dimension = IEnlaceDatos.TiposDimension.SinDefinir And DataEntity IsNot Nothing Then

                Dim Form_ = New Template.FormularioGeneralWeb()

                With DataEntity

                    .cmp(KeyField)

                    For Each control_ As Object In _PillboxListControls

                        .cmp(control_.ID)

                    Next

                End With

                Dim dataTable_ = Form_.ConsultarEnlace(entidad_:=DataEntity,
                                            dimension_:=Dimension,
                                            granularidad_:=Granularity,
                                            clausulasLibres_:=FreeClauses)

                dataSource_ = ObtenerListaResultados(dataTable_)

                DataSource = dataSource_

                Return dataSource_

            End If

        End If

        Return Nothing

    End Function

    Private Function ObtenerListaResultados(ByRef tablaResultados_ As DataTable) As List(Of Dictionary(Of String, Object))

        Dim listaResultados_ As New List(Of Dictionary(Of String, Object))

        If tablaResultados_ IsNot Nothing Then

            For Each fila_ As DataRow In tablaResultados_.Rows

                Dim campo_ As New Dictionary(Of String, Object)

                For Each columna_ As DataColumn In tablaResultados_.Columns

                    Dim tipoDato_ = fila_(columna_).GetType()

                    Select Case tipoDato_

                        Case System.Type.GetType("System.DateTime")

                            System.Type.GetType("System.Date")

                            campo_.Add(columna_.ColumnName, Date.Parse(fila_(columna_)).ToString("yyyy-MM-dd"))

                        Case Else

                            campo_.Add(columna_.ColumnName, fila_(columna_))

                    End Select

                Next

                campo_.Add("borrado", False)

                campo_.Add("archivado", False)

                listaResultados_.Add(campo_)

            Next


        End If

        Return listaResultados_

    End Function

#End Region

#Region "Renderizado"

    Private Sub FindControls(ByVal root As Control)

        If root.GetType = GetType(InputControl) Then

            _PillboxListControls.Add(root)

        ElseIf root.GetType = GetType(SelectControl) Then

            _PillboxListControls.Add(root)

        ElseIf root.GetType = GetType(SwitchControl) Then

            _PillboxListControls.Add(root)

        ElseIf root.GetType = GetType(FindboxControl) Then

            _PillboxListControls.Add(root)

        ElseIf root.GetType = GetType(CatalogControl) Then

            _PillboxListControls.Add(root)

        Else

            For Each control_ As Control In root.Controls

                FindControls(control_)

            Next

        End If

    End Sub

    Private Sub SettingCatalogControls()

        _toolBar = New ToolbarControl()

        With _toolBar

            .ID = ID

            .CssClass = "col-auto ml-auto mb-4"

            AddHandler .CheckedChanged, AddressOf NavigationToPage

            AddHandler .Click, AddressOf ToolbarClickAction

        End With

        _bodyPillbox = New HtmlGenericControl("div")

        With _bodyPillbox

            .Attributes.Add("class", "row no-gutters fieldset w-100")

        End With

        _textJsondata = New TextBox

        With _textJsondata

            .Attributes.Add("class", "__data d-none")

            .Attributes.Add("page", 0)

        End With

        _keyInputControl = New InputControl

        With _keyInputControl

            .ID = KeyField

            .Type = InputControl.InputType.Hide

        End With

        _identityInputControl = New InputControl

        With _identityInputControl

            .ID = "identidad"

            .Type = InputControl.InputType.Hide

            .WorksWith = CamposGlobales.CP_IDENTITY

            .Value = 1

        End With

        For Each control_ As Object In ListControls

            FindControls(control_)

        Next

        _PillboxListControls.Add(_keyInputControl)

        _PillboxListControls.Add(_identityInputControl)

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingCatalogControls()

        Dim component_ = New HtmlGenericControl("div")

        With component_

            .Attributes.Add("class", "wc-pillbox __component position-relative d-flex flex-colums mb-3 nopadding " & CssClass)

            .Attributes.Add("is", "wc-pillbox")

            .Controls.Add(_toolBar)

            PreparedDataPillbox()

            If ListControls.Count Then

                For Each control_ As Object In ListControls

                    _bodyPillbox.Controls.Add(control_)

                Next

                _bodyPillbox.Controls.Add(_keyInputControl)

                _bodyPillbox.Controls.Add(_identityInputControl)

            End If

            .Controls.Add(_bodyPillbox)

            .Controls.Add(_textJsondata)

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

Public Class PillBox

    Public Sub New()
        MyBase.New()
    End Sub

    Property Properties As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

    Public Sub SetControlValue(ByRef control_ As InputControl, value_ As String)

        If Properties.ContainsKey(control_.ID) Then

            Properties(control_.ID) = value_

        Else

            Properties.Add(control_.ID, value_)

        End If

    End Sub

    Public Sub SetControlValue(ByRef control_ As SelectControl, value_ As SelectOption)

        If Properties.ContainsKey(control_.ID) Then

            Properties(control_.ID) = value_

        Else

            Properties.Add(control_.ID, value_)

        End If

    End Sub

    Public Sub SetControlValue(ByRef control_ As SwitchControl, value_ As Boolean)

        If Properties.ContainsKey(control_.ID) Then

            Properties(control_.ID) = value_

        Else

            Properties.Add(control_.ID, value_)

        End If

    End Sub

    Public Sub SetControlValue(ByRef control_ As FindboxControl, value_ As SelectOption)

        If Properties.ContainsKey(control_.ID) Then

            Properties(control_.ID) = value_

        Else

            Properties.Add(control_.ID, value_)

        End If

    End Sub

    Public Sub SetControlValue(ByRef control_ As CatalogControl, value_ As Object)

        If Properties.ContainsKey(control_.ID) Then

            Properties(control_.ID) = value_

        Else

            Properties.Add(control_.ID, value_)

        End If

    End Sub

    Public Sub SetIndice(ByVal id_ As String, value_ As Integer)

        If Properties.ContainsKey(id_) Then

            Properties(id_) = value_

        Else

            Properties.Add(id_, value_)

        End If

    End Sub

    Public Sub SetIdentity(value_ As Integer)

        If Properties.ContainsKey("identidad") Then

            Properties("identidad") = value_

        Else

            Properties.Add("identidad", value_)

        End If

    End Sub

    Public Sub SetFiled(value_ As Boolean)

        If Properties.ContainsKey("archivado") Then

            Properties("archivado") = value_

        Else

            Properties.Add("archivado", value_)

        End If

    End Sub

    Public Function GetControlValue(ByRef control_ As SelectControl) As SelectOption

        If Properties.ContainsKey(control_.ID) = True Then

            If Properties.Item(control_.ID) IsNot Nothing Then

                Return New SelectOption With {.Text = Properties.Item(control_.ID).Text, .Value = Properties.Item(control_.ID).Value}

            End If

        End If

        Return Nothing

    End Function

    Public Function GetControlValue(ByRef control_ As InputControl) As String

        If Properties.ContainsKey(control_.ID) = True Then

            Return Properties.Item(control_.ID)

        End If

        Return Nothing

    End Function

    Public Function GetControlValue(ByRef control_ As SwitchControl) As Boolean

        If Properties.ContainsKey(control_.ID) = True Then

            Convert.ToBoolean(Properties.Item(control_.ID))

        End If

        Return control_.Checked

    End Function

    Public Function GetControlValue(ByRef control_ As FindboxControl) As SelectOption

        If Properties.ContainsKey(control_.ID) = True Then

            If Properties.Item(control_.ID) IsNot Nothing Then

                Return New SelectOption With {.Text = Properties.Item(control_.ID).Text, .Value = Properties.Item(control_.ID).Value}

            End If

        End If

        Return Nothing

    End Function

    Public Function GetControlValue(ByRef control_ As CatalogControl) As Object

        If Properties.ContainsKey(control_.ID) = True Then

            Return Properties.Item(control_.ID)

        End If

        Return Nothing

    End Function

    Public Function GetIndice(ByVal keyField_ As String) As Integer

        If Properties.Item(keyField_) IsNot Nothing Then

            If IsNumeric(Properties.Item(keyField_)) Then

                Return Convert.ToInt32(Properties.Item(keyField_))

            ElseIf Not String.IsNullOrEmpty(Properties.Item(keyField_)) Then

                Return Convert.ToInt32(Properties.Item(keyField_))

            End If

        End If

        Return 0

    End Function

    Public Function GetIdentity() As Integer

        If Properties.Item("identidad") IsNot Nothing Then

            If IsNumeric(Properties.Item("identidad")) Then

                Return Convert.ToInt32(Properties.Item("identidad"))

            ElseIf Not String.IsNullOrEmpty(Properties.Item("identidad")) Then

                Return Convert.ToInt32(Properties.Item("identidad"))

            End If

        End If

        Return 0

    End Function

    Public Function IsFiled() As Boolean

        Return Convert.ToBoolean(Properties.Item("archivado"))

    End Function

    Public Function IsDeleted() As Boolean

        Return Convert.ToBoolean(Properties.Item("borrado"))

    End Function

End Class
