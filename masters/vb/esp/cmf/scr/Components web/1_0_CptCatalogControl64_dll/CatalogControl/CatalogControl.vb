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

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
    Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level:=AspNetHostingPermissionLevel.Minimal),
DefaultEvent("Click"),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class CatalogControl
    Inherits UIControlDataConnector

#Region "Controles"

    Private _collapseButton As LinkButton

    Private _catalog As HtmlGenericControl

    Private _thead As HtmlGenericControl

    Private _tbody As HtmlGenericControl

    Private _textJsondata As TextBox

    Private _addButton As LinkButton

    Private _cloneButton As LinkButton

    Private _deleteButton As LinkButton

    Private _findBar As TextBox

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EventClick As New Object()

    Public Event RowChanged As EventHandler

    Private Property Headers As List(Of String) = New List(Of String)

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property Columns As List(Of CompositeControl) = New List(Of CompositeControl)

    Private Property DataRows As List(Of Object) = New List(Of Object)

    Public Property UserInteraction As Boolean

        Get

            If ViewState("UserInteraction") IsNot Nothing Then

                Return ViewState("UserInteraction")

            End If

            Return True

        End Get

        Set(value As Boolean)

            ViewState("UserInteraction") = value

        End Set

    End Property

    Public Property DeleteRowsId As List(Of Object)

        Get

            EnsureChildControls()

            Try

                Dim dataSource_ = New JavaScriptSerializer().Deserialize(Of Dictionary(Of Object, Object))(_textJsondata.Text)

                If dataSource_ IsNot Nothing Then

                    Dim dropItems_ As New List(Of Object)

                    If dataSource_.Item("DropItems") IsNot Nothing Then

                        For Each item_ As Object In dataSource_.Item("DropItems")

                            dropItems_.Add(item_)

                        Next

                        DeleteRowsId = dropItems_

                    End If

                    Return dropItems_

                End If

                Return Nothing

            Catch ex As Exception

                Return Nothing

            End Try

        End Get

        Set(value As List(Of Object))

            EnsureChildControls()

            Try

                Dim dataSource_ = New JavaScriptSerializer().Deserialize(Of Dictionary(Of Object, Object))(_textJsondata.Text)

                If dataSource_ IsNot Nothing Then

                    dataSource_.Item("DropItems") = value

                    _textJsondata.Text = New JavaScriptSerializer().Serialize(dataSource_)

                End If

            Catch ex As Exception


            End Try

        End Set

    End Property

    Public Property CanAdd As Boolean

        Get

            Return ViewState("CanAdd")

        End Get

        Set(value As Boolean)

            ViewState("CanAdd") = value

        End Set

    End Property

    Public Property CanClone As Boolean

        Get

            Return ViewState("CanClone")

        End Get

        Set(value As Boolean)

            ViewState("CanClone") = value

        End Set

    End Property

    Public Property CanDelete As Boolean

        Get

            Return ViewState("CanDelete")

        End Get

        Set(value As Boolean)

            ViewState("CanDelete") = value

        End Set

    End Property

    Public Property Collapsed As Boolean

        Get

            Return ViewState("Collapsed")

        End Get

        Set(value As Boolean)

            ViewState("Collapsed") = value

        End Set

    End Property

    Public Property DataSource As Object

        Get

            EnsureChildControls()

            Try

                Dim dataSource_ = New JavaScriptSerializer().Deserialize(Of Dictionary(Of Object, Object))(_textJsondata.Text)

                If dataSource_ IsNot Nothing Then

                    _lastrowinteraction = Convert.ToInt32(dataSource_.Item("LastRowInteraction"))

                    If _lastrowinteraction >= 0 Then

                        OnRowChanged(dataSource_.Item("Items")(_lastrowinteraction))

                        dataSource_.Item("LastRowInteraction") = -1

                        _textJsondata.Text = New JavaScriptSerializer().Serialize(dataSource_)

                    End If

                    Return dataSource_.Item("Items")

                End If

                Return Nothing

            Catch ex As Exception

                Return Nothing

            End Try

        End Get

        Set(value As Object)

            EnsureChildControls()

            If value IsNot Nothing Then

                If value.GetType = GetType(Dictionary(Of String, Object)) Then

                    value.Item("LastRowInteraction") = -1

                    _textJsondata.Text = New JavaScriptSerializer().Serialize(value)

                Else

                    Dim dataSource_ = New Dictionary(Of Object, Object)

                    dataSource_.Add("Items", value)

                    dataSource_.Add("DropItems", Nothing)

                    dataSource_.Add("SelectedItem", Nothing)

                    dataSource_.Add("LastRowInteraction", -1)

                    _textJsondata.Text = New JavaScriptSerializer().Serialize(dataSource_)

                End If

            Else

                _textJsondata.Text = Nothing

            End If

        End Set

    End Property

    Private Property _interaction As String

    Private Property _lastrowinteraction As Integer

#End Region

#Region "Constructor"

    Sub New()

        Collapsed = True

        CanAdd = True

        CanClone = True

        CanDelete = True

    End Sub

#End Region

#Region "Eventos"

    Public Overridable Sub OnRowChanged(ByRef row_ As Object)

        RaiseEvent RowChanged(row_, EventArgs.Empty)

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

    Private Sub CatalogCollapsed(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        Collapsed = Not Collapsed

    End Sub

#End Region

#Region "Metodos"

    Public Sub ForEach(func_ As Action(Of Object))

        Dim jsonData_ = GetData()

        If jsonData_ IsNot Nothing Then

            If jsonData_.Count Then

                For Each jsonRow_ As Object In jsonData_

                    Dim catalogRow_ As New CatalogRow

                    catalogRow_.Properties.Add(KeyField, jsonRow_.Item(KeyField))

                    For Each control_ As Object In _Columns

                        If control_.GetType = GetType(InputControl) Then

                            Dim temp_ = DirectCast(control_, InputControl)

                            catalogRow_.SetColumn(temp_, jsonRow_.Item(temp_.ID))

                        ElseIf control_.GetType = GetType(SwitchControl) Then

                            Dim temp_ = DirectCast(control_, SwitchControl)

                            catalogRow_.SetColumn(temp_, jsonRow_.Item(temp_.ID))

                        ElseIf control_.GetType = GetType(SelectControl) Then

                            Dim temp_ = DirectCast(control_, SelectControl)

                            catalogRow_.SetColumn(temp_, New SelectOption With {.Value = jsonRow_.Item(temp_.ID).Item("Value"), .Text = jsonRow_.Item(temp_.ID).Item("Text")})

                        End If

                    Next

                    If Not func_ Is Nothing Then

                        func_(catalogRow_)

                    End If

                Next

            End If

        End If

    End Sub

    Public Sub CatalogDataRefresh()

        EnsureChildControls()

        Dim dropItems_ = New List(Of Object)

        Dim dataRows_ As New List(Of Object)

        Dim dataSource_ = New JavaScriptSerializer().Deserialize(Of Dictionary(Of Object, Object))(_textJsondata.Text)

        If dataSource_ IsNot Nothing Then

            If dataSource_.Item("DropItems") IsNot Nothing Then

                For Each dropItem_ As Object In dataSource_.Item("DropItems")

                    dropItems_.Add(dropItem_)

                Next

                For Each row_ As Object In dataSource_.Item("Items")

                    If Not dropItems_.Contains(row_(KeyField)) Then

                        dataRows_.Add(row_)

                    End If

                Next

                dataSource_.Item("DropItems") = Nothing

                dataSource_.Item("Items") = dataRows_

                _textJsondata.Text = New JavaScriptSerializer().Serialize(dataSource_)

            End If

        End If

    End Sub

    Public Sub CatalogDataBinding()

        DataSource = DataRows

    End Sub

    Public Sub ClearRows()

        DataRows = New List(Of Object)

    End Sub

    Public Sub SetRow(func_ As Action(Of CatalogRow))

        If Not func_ Is Nothing Then

            Dim catalogRow_ = New CatalogRow

            func_(catalogRow_)

            DataRows.Add(catalogRow_.Properties)

        End If

    End Sub

    Public Sub EnabledToolBar(ByVal enabled_ As Boolean)

        EnsureChildControls()

        _interaction = IIf(enabled_ = False, " interaction-disabled", Nothing)

        _addButton.Attributes.Add("class", "__add" & _interaction)

        _cloneButton.Attributes.Add("class", "__clone" & _interaction)

        _deleteButton.Attributes.Add("class", "__delete" & _interaction)

    End Sub

    Public Function GetDataRows(Of T)() As List(Of T)

        Try

            Dim rows_ = New List(Of T)

            Dim jsonData_ = GetData()

            If jsonData_ IsNot Nothing Then

                If jsonData_.Count Then

                    Dim type_ As Type = GetType(T)

                    Dim properties_ As Reflection.PropertyInfo() = type_.GetProperties()

                    For Each jsonRow_ As Dictionary(Of String, Object) In jsonData_

                        Dim retObject_ As [Object] = type_.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, Nothing, type_, Nothing)

                        For Each propertyInfo As Reflection.PropertyInfo In properties_

                            If propertyInfo.CanWrite Then

                                If jsonRow_.Item(propertyInfo.Name) IsNot Nothing Then

                                    If jsonRow_.Item(propertyInfo.Name).GetType() = GetType(Dictionary(Of String, Object)) Then

                                        Dim selectOption_ = New SelectOption

                                        With selectOption_

                                            .Value = jsonRow_.Item(propertyInfo.Name).Item("Value")

                                            .Text = jsonRow_.Item(propertyInfo.Name).Item("Text")

                                        End With

                                        propertyInfo.SetValue(retObject_, selectOption_, Nothing)

                                    Else

                                        propertyInfo.SetValue(retObject_, jsonRow_.Item(propertyInfo.Name), Nothing)

                                    End If

                                End If

                            End If

                        Next

                        rows_.Add(retObject_)

                    Next

                End If

            End If

            Return rows_

        Catch ex As Exception

        End Try

        Return Nothing

    End Function

    Private Function GetData() As Object()

        Dim dataSource_ = DataSource

        If dataSource_ IsNot Nothing Then

            Return dataSource_

        Else

            If Not Dimension = IEnlaceDatos.TiposDimension.SinDefinir And DataEntity IsNot Nothing Then

                Dim Form_ = New Template.FormularioGeneralWeb()

                With DataEntity

                    .cmp(KeyField)

                    For Each field_ As String In _Headers

                        .cmp(field_)

                    Next

                End With

                Dim dataTable_ = Form_.ConsultarEnlace(entidad_:=DataEntity,
                                            dimension_:=Dimension,
                                            granularidad_:=Granularity,
                                            clausulasLibres_:=FreeClauses)

                dataSource_ = ObtenerListaResultados(dataTable_)

                DataSource = dataSource_

                Return DataSource

            End If

        End If

        Return Nothing

    End Function

    Private Sub AppendRowTemplate(ByRef control_ As HtmlGenericControl)

        control_.Controls.Add(New LiteralControl("       <tr class='d-none __template'>"))

        control_.Controls.Add(New LiteralControl("           <td id='" & KeyField & "'>"))

        If CanDelete Or CanClone Or CanAdd Then

            control_.Controls.Add(New LiteralControl("               <label class='wc-checkbox d-flex position-relative mb-3 ml-4' " & ForeColor.HtmlPropertyColor & ">"))

            control_.Controls.Add(New LiteralControl("	                <input id='id_' type='checkbox' class='d-none' value='0'/>"))

            control_.Controls.Add(New LiteralControl("	                <span class='d-flex align-items-center'></span>"))

            control_.Controls.Add(New LiteralControl("               </label>"))

        End If

        control_.Controls.Add(New LiteralControl("           </td>"))

        If _Columns.Count Then

            For Each column_ As Object In _Columns

                control_.Controls.Add(New LiteralControl("       <td id=" & column_.ID & ">"))

                control_.Controls.Add(column_)

                control_.Controls.Add(New LiteralControl("       </td>"))

            Next

        End If

        control_.Controls.Add(New LiteralControl("           <td>"))

        If CanDelete Then

            control_.Controls.Add(New LiteralControl("	            <a href='' class='__quit'></a>"))

        End If

        control_.Controls.Add(New LiteralControl("           </td>"))

        control_.Controls.Add(New LiteralControl("       </tr>"))

    End Sub

    Private Function SetDroppedState(controlId_ As String, rowIndex_ As Integer) As Boolean

        Try

            Dim dataSource_ = New JavaScriptSerializer().Deserialize(Of Dictionary(Of Object, Object))(_textJsondata.Text)

            If dataSource_ IsNot Nothing Then

                Dim selectedItem_ = dataSource_.Item("SelectedItem")

                If selectedItem_ IsNot Nothing Then

                    Dim itemId_ = selectedItem_.Item("Id")

                    If controlId_ = itemId_ Then

                        Dim c = selectedItem_.Item("RowId")

                        If Convert.ToInt32(c) = rowIndex_ Then

                            Dim b = Convert.ToBoolean(selectedItem_.Item("Drop"))

                            If b = False Then

                                dataSource_.Item("LastRowInteraction") = -1

                                dataSource_.Item("SelectedItem") = Nothing

                                _textJsondata.Text = New JavaScriptSerializer().Serialize(dataSource_)

                            End If

                            Return b

                        End If

                    End If

                End If

            End If

        Catch ex As Exception
        End Try

        Return False

    End Function

    Private Sub PrepareCatalogBody()

        Dim data_ = GetData()

        If data_ IsNot Nothing Then

            Dim rowIndex = 0

            For Each row_ As Object In data_

                Dim display_ = Nothing

                If DeleteRowsId.Contains(row_(KeyField)) Then

                    display_ = "d-none"

                End If

                _tbody.Controls.Add(New LiteralControl("       <tr class='__row " & display_ & "'>"))

                If UserInteraction = True Then

                    _tbody.Controls.Add(New LiteralControl("           <td id='" & KeyField & "'>"))

                    If CanDelete Or CanClone Then

                        _tbody.Controls.Add(New LiteralControl("               <label class='wc-checkbox d-flex position-relative mb-3 ml-4' " & ForeColor.HtmlPropertyColor & ">"))

                        _tbody.Controls.Add(New LiteralControl("	                <input id='id_' type='checkbox' class='d-none' value='" & row_(KeyField) & "'/>"))

                        _tbody.Controls.Add(New LiteralControl("	                <span class='d-flex align-items-center'></span>"))

                        _tbody.Controls.Add(New LiteralControl("               </label>"))

                    End If

                    _tbody.Controls.Add(New LiteralControl("           </td>"))

                End If

                If _Columns.Count Then

                    For Each column_ As Object In _Columns

                        _tbody.Controls.Add(New LiteralControl("       <td id=" & column_.ID & ">"))

                        If UserInteraction = True Then

                            Dim templateControl_ As Object

                            If column_.GetType = GetType(SelectControl) Then

                                column_.Dropped = False

                                templateControl_ = New SelectControl

                                With templateControl_

                                    .Enabled = column_.Enabled 'Enabled

                                    .ID = column_.ID '& rowIndex

                                    .Label = column_.Label

                                    .LocalSearch = column_.LocalSearch

                                    .SearchBarEnabled = column_.SearchBarEnabled

                                    .HasDetails = column_.HasDetails

                                    .Template = column_.Template

                                    .Dropped = SetDroppedState(column_.ID, rowIndex)

                                    If column_.DataSource IsNot Nothing Then

                                        .DataSource = column_.DataSource

                                        .Value = row_(column_.ID).Item("Value")

                                    Else

                                        .DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = row_(column_.ID).Item("Value"), .Text = row_(column_.ID).Item("Text")}}

                                        .Value = row_(column_.ID).Item("Value")

                                    End If

                                End With

                            ElseIf column_.GetType = GetType(InputControl) Then

                                templateControl_ = New InputControl

                                With templateControl_

                                    .Enabled = column_.Enabled 'Enabled

                                    .ID = column_.ID

                                    .Label = column_.Label

                                    .Type = column_.Type

                                    .Format = column_.Format

                                    .Value = row_(column_.ID)

                                End With

                            ElseIf column_.GetType = GetType(SwitchControl) Then

                                templateControl_ = New SwitchControl

                                With templateControl_

                                    .Enabled = column_.Enabled 'Enabled

                                    .ID = column_.ID

                                    .Label = column_.Label

                                    .OnText = column_.OnText

                                    .OffText = column_.OffText

                                    .Checked = row_(column_.ID)

                                End With

                            Else

                                templateControl_ = Nothing

                            End If

                            _tbody.Controls.Add(templateControl_)

                        Else

                            If column_.GetType = GetType(SwitchControl) Then

                                _tbody.Controls.Add(New LiteralControl("<p>" & IIf(row_(column_.ID) = True, column_.OnText, column_.OffText) & "</p>"))

                            ElseIf column_.GetType = GetType(SelectControl) Then

                                _tbody.Controls.Add(New LiteralControl("<p>" & row_(column_.ID).Item("Text").ToString & "</p>"))

                            Else

                                _tbody.Controls.Add(New LiteralControl("<p>" & row_(column_.ID).ToString & "</p>"))

                            End If

                        End If

                        _tbody.Controls.Add(New LiteralControl("       </td>"))

                    Next

                End If

                If UserInteraction = True Then

                    _tbody.Controls.Add(New LiteralControl("           <td>"))

                    If CanDelete Then

                        _tbody.Controls.Add(New LiteralControl("	            <a href='' class='__quit" & _interaction & "'></a>"))

                    End If

                    _tbody.Controls.Add(New LiteralControl("           </td>"))

                Else

                    _tbody.Controls.Add(New LiteralControl("           <td></td>"))

                End If

                _tbody.Controls.Add(New LiteralControl("       </tr>"))

                rowIndex += 1

            Next

        End If

    End Sub

    Private Sub PrepareCatalogHeader()

        _thead = New HtmlGenericControl("tr")

        If UserInteraction = True Then

            _thead.Controls.Add(New LiteralControl("       <th>"))

            If CanDelete Or CanClone Or CanAdd Then

                _thead.Controls.Add(New LiteralControl("           <label class='wc-checkbox d-flex position-relative mt-1 ml-4' " & ForeColor.HtmlPropertyColor & ">"))

                _thead.Controls.Add(New LiteralControl("	            <input type='checkbox' class='d-none __checks'/>"))

                _thead.Controls.Add(New LiteralControl("	            <span class='d-flex align-items-center'></span>"))

                _thead.Controls.Add(New LiteralControl("           </label>"))

            End If

            _thead.Controls.Add(New LiteralControl("       </th>"))

        End If

        For Each column_ As Object In _Columns

            _thead.Controls.Add(New LiteralControl("   <th>" & column_.Label & "</th>"))

            If column_.ID IsNot Nothing Then

                _Headers.Add(column_.ID)

            End If

        Next

        _thead.Controls.Add(New LiteralControl("       <th>"))

        _thead.Controls.Add(_collapseButton)

        _thead.Controls.Add(New LiteralControl("       </th>"))

        _tbody.Controls.Add(_thead)

    End Sub

    Private Function ObtenerListaResultados(ByRef tablaResultados_ As DataTable) As List(Of Object)

        Dim listaResultados_ As New List(Of Object)

        If tablaResultados_ IsNot Nothing Then

            For Each fila_ As DataRow In tablaResultados_.Rows

                Dim campo_ As New Dictionary(Of Object, Object)

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

                listaResultados_.Add(campo_)

            Next


        End If

        Return listaResultados_

    End Function

#End Region

#Region "Renderizado"

    Private Sub SettingCatalogControls()

        _tbody = New HtmlGenericControl("tbody")


        _collapseButton = New LinkButton

        With _collapseButton

            .CausesValidation = False

            .Attributes.Add("class", "__collapse")

            Dim icon_ = If(Collapsed = True, "-", "+")

            .Attributes.Add("icon", icon_)

            AddHandler .Click, AddressOf CatalogCollapsed

        End With


        _textJsondata = New TextBox

        With _textJsondata

            .Attributes.Add("class", "__data d-none")

            .AutoPostBack = True

            .ID = "__remove"

        End With

        _addButton = New LinkButton

        With _addButton

            .Attributes.Add("class", "__add")

        End With

        _cloneButton = New LinkButton

        With _cloneButton

            .Attributes.Add("class", "__clone")

        End With

        _deleteButton = New LinkButton

        With _deleteButton

            .Attributes.Add("class", "__delete")

        End With

        _findBar = New TextBox

        With _findBar

            .Attributes.Add("class", "__find")

            .Attributes.Add("placeholder", "Buscar")

        End With

    End Sub


    Protected Overrides Sub CreateChildControls()

        SettingCatalogControls()

        Dim component_ = New HtmlGenericControl("div")

        Dim simpleMode = If(UserInteraction = True, "", " user-iteraction-disabled")

        With component_

            .Attributes.Add("class", "wc-catalog __component position-relative " & CssClass & simpleMode)

            If Not ForeColor = Color.Empty Then

                .Attributes.Add("style", "--tintColor: " & ForeColor.ToHex)

            End If

            .Controls.Add(New LiteralControl(" <div>"))

            If UserInteraction = True Then

                If CanAdd Then

                    .Controls.Add(_addButton)

                End If

                If CanClone Then

                    .Controls.Add(_cloneButton)

                End If

                If CanDelete Then

                    .Controls.Add(_deleteButton)

                End If

            End If

            .Controls.Add(_findBar)

            .Controls.Add(New LiteralControl(" </div>"))

            .Controls.Add(New LiteralControl(" <table is='wc-catalog'>"))

            PrepareCatalogHeader()

            .Controls.Add(_tbody)

            If UserInteraction = True Then

                AppendRowTemplate(_tbody)

            End If

            .Controls.Add(New LiteralControl(" </table>"))

            .Controls.Add(_textJsondata)

        End With

        Me.Controls.Add(component_)

    End Sub

    Protected Overrides Sub Render(ByVal component_ As HtmlTextWriter)

        If Collapsed = True Then

            _collapseButton.Attributes.Add("icon", "-")

            PrepareCatalogBody()

        Else

            _collapseButton.Attributes.Add("icon", "+")

        End If

        _textJsondata.Attributes.Add("collapsed", Collapsed)

        Me.RenderContents(component_)

    End Sub

    Protected Overrides Sub RecreateChildControls()

        EnsureChildControls()

    End Sub

#End Region

End Class

Public Class CatalogRow

    Public Sub New()
        MyBase.New()
    End Sub

    Property Properties As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

    Public Sub SetColumn(ByRef control_ As InputControl, value_ As String)

        If Properties.ContainsKey(control_.ID) Then

            Properties(control_.ID) = value_

        Else

            Properties.Add(control_.ID, value_)

        End If

    End Sub

    Public Sub SetColumn(ByRef control_ As SelectControl, value_ As SelectOption)

        If Properties.ContainsKey(control_.ID) Then

            Properties(control_.ID) = value_

        Else

            Properties.Add(control_.ID, value_)

        End If

    End Sub

    Public Sub SetColumn(ByRef control_ As SwitchControl, value_ As Boolean)

        If Properties.ContainsKey(control_.ID) Then

            Properties(control_.ID) = value_

        Else

            Properties.Add(control_.ID, value_)

        End If

    End Sub

    Public Sub SetIndice(ByVal id_ As String, value_ As Integer)

        Properties.Add(id_, value_)

    End Sub

    Public Function GetColumn(ByRef control_ As SelectControl) As SelectOption

        Return New SelectOption With {.Text = Properties.Item(control_.ID).Text, .Value = Properties.Item(control_.ID).Value}

    End Function

    Public Function GetColumn(ByRef control_ As InputControl) As String

        Return Properties.Item(control_.ID)

    End Function

    Public Function GetColumn(ByRef control_ As SwitchControl) As Boolean

        Return Convert.ToBoolean(Properties.Item(control_.ID))

    End Function

    Public Function GetIndice(ByVal keyField_ As String) As Integer

        Return Convert.ToInt32(Properties.Item(keyField_))

    End Function

End Class
