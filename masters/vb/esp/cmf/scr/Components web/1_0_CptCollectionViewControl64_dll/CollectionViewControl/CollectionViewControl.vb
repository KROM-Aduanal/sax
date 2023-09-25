Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Drawing
Imports System.Web.UI.WebControls
Imports Gsol.krom
Imports Rec.Globals.Utils
Imports System.Web.Script.Serialization
'https://www.aspsnippets.com/Articles/Implement-Button-Click-event-in-ASPNet-Repeater-control.aspx
'https://learn.microsoft.com/en-us/previous-versions/aspnet/0e39s2ck(v=vs.100)

'falta solucionar detalle para los odf, no puedo jalar campos designados
<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<ToolboxData("<{0}:CollectionViewControl runat=server></{0}:CollectionViewControl>")>
Public Class CollectionViewControl
    Inherits UIControlDataConnector

#Region "Atributos"

    Private _editButton As LinkButton

    Private _deleteButton As LinkButton

    Private _fileButton As LinkButton

    Private _collapseButton As LinkButton

    Private _textJsondata As TextBox

#End Region

#Region "Propiedades"

    Private _viewControls As New List(Of Control)

    Private Property DataCollectionView = New List(Of Object)

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public ReadOnly Property ViewTemplate As Repeater = New Repeater

    Public Property PageItems As Integer

    Public Property OwnerField As String

    Public Property PublishDateField As String

    Public Property TitleField As String

    Public Property DetailField As String

    Public Property DataSource As Object

        Get

            EnsureChildControls()

            Try
                Dim dataSource_ = New JavaScriptSerializer().Deserialize(Of List(Of Object))(_textJsondata.Text)

                Return dataSource_

            Catch ex As Exception

                Return Nothing

            End Try

        End Get

        Set(value As Object)

            EnsureChildControls()

            If value IsNot Nothing Then

                _textJsondata.Text = New JavaScriptSerializer().Serialize(value)

                PrepareRepeaterData()

            Else

                _textJsondata.Text = Nothing

            End If

        End Set

    End Property

    Public Property Collapsed As Boolean

        Get

            If ViewState("Collapsed") IsNot Nothing Then

                Return ViewState("Collapsed")

            End If

            Return True

        End Get

        Set(value As Boolean)

            ViewState("Collapsed") = value

        End Set

    End Property

    Public Property CanEdite As Boolean = True

    Public Property CanDelete As Boolean = True

    Public Property CanFile As Boolean = False

    Public Property HasOwner As Boolean = False

    Public ReadOnly Property CollectionViewListControls As List(Of Control)

        Get

            EnsureChildControls()

            For Each c As Control In ViewTemplate.Controls

                FindControls(c)

            Next

            Return _viewControls

        End Get

    End Property

#End Region

#Region "Constructor"

    Sub New()

        AddHandler ViewTemplate.ItemDataBound, AddressOf OnItemDataBound

        AddHandler ViewTemplate.ItemCommand, AddressOf OnItemCommand

    End Sub

#End Region

#Region "Eventos"

    Protected Sub OnItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)

        If e.Item.DataItem.Item("borrado") = False And e.Item.DataItem.Item("archivado") = False Then

            AppendItemBar(e.Item.Controls(1), e.Item.DataItem)

            For Each item As KeyValuePair(Of String, Object) In e.Item.DataItem

                Dim control_ = e.Item.FindControl(item.Key)

                If control_ IsNot Nothing Then

                    DirectCast(control_.Controls(0), HtmlGenericControl).Attributes.Add("fieldid", item.Key)

                    If e.Item.DataItem.Item("calapsado") = False Then

                        If control_.GetType = GetType(InputControl) Then

                            With DirectCast(control_, InputControl)

                                .Value = e.Item.DataItem.Item(item.Key)

                                .Enabled = e.Item.DataItem.Item("editando")

                            End With

                        ElseIf control_.GetType = GetType(SwitchControl) Then

                            With DirectCast(control_, SwitchControl)

                                .Checked = e.Item.DataItem.Item(item.Key)

                                .Enabled = e.Item.DataItem.Item("editando")

                            End With

                        ElseIf control_.GetType = GetType(SelectControl) Then

                            With DirectCast(control_, SelectControl)

                                '.DataSource = Nothing

                                .Value = Nothing

                                If Not String.IsNullOrEmpty(e.Item.DataItem.Item(item.Key).Item("Value")) And Not String.IsNullOrEmpty(e.Item.DataItem.Item(item.Key).Item("Text")) Then

                                    '.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = e.Item.DataItem.Item(item.Key).Item("Value"), .Text = e.Item.DataItem.Item(item.Key).Item("Text")}}

                                    .Value = e.Item.DataItem.Item(item.Key).Item("Value")




                                End If

                                .Enabled = e.Item.DataItem.Item("editando")

                                If Not e.Item.DataItem.Item(item.Key).Item("Dropdowm") = Nothing Then

                                    .Dropped = e.Item.DataItem.Item(item.Key).Item("Dropdowm")

                                End If

                            End With

                        ElseIf control_.GetType = GetType(CatalogControl) Then

                            With DirectCast(control_, CatalogControl)

                                If e.Item.DataItem.Item(item.Key) IsNot Nothing Then

                                    If e.Item.DataItem.Item(item.Key).GetType = GetType(String) Then

                                        Dim dataSource_ = New JavaScriptSerializer().Deserialize(Of Dictionary(Of String, Object))(e.Item.DataItem.Item(item.Key))

                                        .DataSource = dataSource_

                                    Else

                                        .DataSource = e.Item.DataItem.Item(item.Key)

                                    End If

                                End If

                            End With

                        ElseIf control_.GetType = GetType(ButtonControl) Then

                            With DirectCast(control_, ButtonControl)

                                .Enabled = e.Item.DataItem.Item("editando")

                                .Value = e.Item.DataItem.Item(item.Key)

                                .RowId = e.Item.ItemIndex

                            End With

                        End If

                    Else

                        control_.Visible = False

                    End If


                End If

            Next

        Else

            e.Item.Visible = False

        End If

    End Sub

    Protected Sub OnItemCommand(ByVal sender As Object, ByVal e As RepeaterCommandEventArgs)

        If e.CommandName = "borrado" Or e.CommandName = "archivado" Or e.CommandName = "calapsado" Or e.CommandName = "editando" Then

            Dim button_ As LinkButton = TryCast(e.CommandSource, LinkButton)

            If button_ IsNot Nothing Then
                'e.Item.ItemIndex
                Dim itemID_ = CStr(e.CommandArgument)

                Dim dataSource_ As List(Of Object) = DataSource

                Dim rowData_ = From item_ As Dictionary(Of String, Object) In dataSource_ Where item_.Item(KeyField) = itemID_ Select item_

                rowData_(0).Item(e.CommandName) = Not rowData_(0).Item(e.CommandName)

                'e.Item.DataItem.Item(e.CommandName) = Not rowData_(0).Item(e.CommandName)

                If e.CommandName = "editando" Then

                    If rowData_(0).Item("editando") = True Then

                        rowData_(0).Item("calapsado") = Not rowData_(0).Item("editando")

                    End If

                End If

                _textJsondata.Text = New JavaScriptSerializer().Serialize(dataSource_)

                'PrepareRepeaterData()

            End If

        End If

    End Sub

#End Region

#Region "Metodos"

    Public Sub SetItem(func_ As Action(Of Object))

        If Not func_ Is Nothing Then

            Dim collectionItem = New CollectionItem()

            func_(collectionItem)

            Dim data_ = collectionItem.Properties

            data_.Add("borrado", False)

            data_.Add("archivado", False)

            data_.Add("calapsado", Collapsed)

            data_.Add("editando", False)

            DataCollectionView.Add(data_)

        End If

    End Sub

    Public Sub ForEach(func_ As Action(Of Object))

        Dim data_ = GetData()

        If data_ IsNot Nothing Then

            For Each item_ As Dictionary(Of String, Object) In data_

                Dim collectionItem = New CollectionItem()

                For Each attribute As KeyValuePair(Of String, Object) In item_

                    collectionItem.SetControlValue(attribute.Key, attribute.Value)

                Next

                If Not func_ Is Nothing Then

                    func_(collectionItem)

                End If

            Next

        End If

        'For Each item_ As RepeaterItem In ViewTemplate.Items

        '    Dim collectionItem = New CollectionItem(item_)

        '    If Not func_ Is Nothing Then

        '        func_(collectionItem)

        '    End If

        'Next

    End Sub

    Public Sub GetItem(ByVal index_ As Integer, func_ As Action(Of Object))

        Dim data_ = GetData()

        If data_ IsNot Nothing Then

            Dim item_ As Dictionary(Of String, Object) = data_(index_)

            If item_ IsNot Nothing Then

                Dim collectionItem = New CollectionItem()

                For Each attribute As KeyValuePair(Of String, Object) In item_

                    collectionItem.SetControlValue(attribute.Key, attribute.Value)

                Next

                If Not func_ Is Nothing Then

                    func_(collectionItem)

                End If

                data_(index_) = collectionItem.Properties

                DataSource = data_

            End If

        End If

        'Dim collectionItem = New CollectionItem(ViewTemplate.Items(index_))

        'If Not func_ Is Nothing Then

        '    func_(collectionItem)

        'End If

    End Sub

    Private Function GetData() As List(Of Object)

        Dim dataSource_ As Object = DataSource

        If dataSource_ IsNot Nothing Then

            Return dataSource_

        Else

            If Not Dimension = IEnlaceDatos.TiposDimension.SinDefinir And DataEntity IsNot Nothing Then

                Dim Form_ = New Template.FormularioGeneralWeb()

                With DataEntity

                    .cmp(KeyField)

                    'For Each id_ As Object In ids_
                    '    .cmp(id_)
                    'Next

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

    Private Sub AppendItemBar(ByRef container_ As Control, ByRef rowData_ As Object)

        _editButton = New LinkButton

        With _editButton

            .Attributes.Add("class", IIf(rowData_.Item("editando") = False, "__edit", "__cancel"))

            .CausesValidation = False

            .CommandName = "editando"

            .CommandArgument = rowData_.Item(KeyField)

        End With

        _deleteButton = New LinkButton

        With _deleteButton

            .Attributes.Add("class", "__delete")

            .CausesValidation = False

            .CommandName = "borrado"

            .CommandArgument = rowData_.Item(KeyField)

        End With

        _fileButton = New LinkButton

        With _fileButton

            .Attributes.Add("class", "__delete")

            .CausesValidation = False

            .CommandName = "archivado"

            .CommandArgument = rowData_.Item(KeyField)

        End With

        _collapseButton = New LinkButton

        With _collapseButton

            .Attributes.Add("class", IIf(rowData_.Item("calapsado") = False, "__up", "__down"))

            .CausesValidation = False

            .CommandName = "calapsado"

            .CommandArgument = rowData_.Item(KeyField)

        End With

        Dim bar_ = New HtmlGenericControl("div")

        With bar_

            .Attributes.Add("class", "__item")

            .Attributes.Add("itemid", rowData_.Item(KeyField))

            Dim margin_ = IIf(rowData_.Item("calapsado") = False, " mb-4", "")

            .Controls.Add(New LiteralControl("<div class='row no-gutters align-items-center" & margin_ & "'>"))

            .Controls.Add(New LiteralControl("  <div class='col'>"))

            If HasOwner Then

                .Controls.Add(New LiteralControl("      <div class='owner-item'>"))

                .Controls.Add(New LiteralControl("          <img src=''/>"))

                .Controls.Add(New LiteralControl("          <label>" & rowData_.Item(OwnerField) & " <small>" & elapsedTime(DateTime.Parse(rowData_.Item(PublishDateField))) & "</small></label>"))

                .Controls.Add(New LiteralControl("      </div>"))

            End If

            .Controls.Add(New LiteralControl("      <p><span>" & rowData_.Item(DetailField) & "</span>" & rowData_.Item(TitleField) & "</p>"))

            .Controls.Add(New LiteralControl("  </div>"))

            .Controls.Add(New LiteralControl("  <div class='col-auto d-flex'>"))

            If CanEdite Then

                .Controls.Add(_editButton)

            End If

            If CanDelete Then

                .Controls.Add(_deleteButton)

            End If

            If CanFile Then

                .Controls.Add(_fileButton)

            End If

            .Controls.Add(_collapseButton)

            .Controls.Add(New LiteralControl("  </div>"))

            .Controls.Add(New LiteralControl("</div>"))

        End With

        container_.Controls.AddAt(0, bar_)

    End Sub

    Public Sub CollectionViewDataBinding()

        DataSource = DataCollectionView

    End Sub

    Private Sub FindControls(ByVal root As Control)

        Dim types = New List(Of Type) From {GetType(InputControl), GetType(SelectControl), GetType(SwitchControl), GetType(FindboxControl)}

        If types.Contains(root.GetType) Then

            _viewControls.Add(root)

        Else

            For Each cntrl As Control In root.Controls

                FindControls(cntrl)

            Next

        End If

    End Sub

    Private Function ObtenerListaResultados(ByRef tablaResultados_ As DataTable) As List(Of Object)

        Dim listaResultados_ As New List(Of Object)

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

                campo_.Add("calapsado", Collapsed)

                campo_.Add("editando", False)

                listaResultados_.Add(campo_)

            Next


        End If

        Return listaResultados_

    End Function

    Private Function elapsedTime(sd_ As Date) As String

        Dim ed_ = DateTime.Now

        Dim d3 = ed_ - sd_

        If d3.Days = 0 Then

            If d3.Hours > 0 Then

                Return "Hace " & d3.Hours & " horas"

            Else

                Return "Hace un momento"

            End If

        Else

            Dim years_, months_, days_, m As Integer

            years_ = Year(ed_) - Year(sd_)

            If Month(sd_) > Month(ed_) Then

                years_ = years_ - 1

            End If

            If Month(ed_) < Month(sd_) Then

                months_ = 12 - Month(sd_) + Month(ed_)

            Else

                months_ = Month(ed_) - Month(sd_)

            End If

            If Day(ed_) < Day(sd_) Then

                months_ = months_ - 1

                If Month(ed_) = Month(sd_) Then

                    years_ = years_ - 1

                    months_ = 11

                End If

            End If

            days_ = Day(ed_) - Day(sd_)

            If days_ < 0 Then

                m = CInt(Month(ed_)) - 1

                If m = 0 Then

                    m = 12

                End If

                Select Case m

                    Case 1, 3, 5, 7, 8, 10, 12

                        days_ = 31 + days_

                    Case 4, 6, 9, 11

                        days_ = 30 + days_

                    Case 2

                        If (Year(ed_) Mod 4 = 0 And Year(ed_) Mod 100 <> 0) Or Year(ed_) Mod 400 = 0 Then

                            days_ = 29 + days_

                        Else

                            days_ = 28 + days_

                        End If

                End Select

            End If

            Dim str_ As New List(Of String)

            If years_ Then

                str_.Add(CStr(years_) + " años")

            End If

            If months_ Then

                str_.Add(CStr(months_) + " meses")

            End If

            If days_ Then

                str_.Add(CStr(days_) + " días")

            End If

            Return "Hace " & String.Join(", ", str_)

            'Dim anosMesesDias = CStr(years_) + " años, " + CStr(months_) + " meses, " + CStr(days_) + " días "
            'Return anosMesesDias

        End If

    End Function

#End Region

#Region "Renderizado"

    Private Sub SettingCollectionViewControls()

        _textJsondata = New TextBox

        With _textJsondata

            .Attributes.Add("class", "__data d-none")

            .ID = "__data"

        End With

        'PrepareRepeaterData()

    End Sub

    Private Sub PrepareRepeaterData()

        Dim data_ = GetData()

        If data_ IsNot Nothing Then

            With ViewTemplate

                .DataSource = data_

                .DataBind()

            End With

        End If

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingCollectionViewControls()

        Dim component_ = New HtmlGenericControl("div")

        With component_

            .Attributes.Add("class", "wc-collectionview position-relative " & CssClass)

            .Attributes.Add("is", "wc-collectionview")

            .Attributes.Add("keyfield", KeyField)

            If Not ForeColor = Color.Empty Then

                .Attributes.Add("style", "--tintColor: " & ForeColor.ToHex)

            End If

            .Controls.Add(ViewTemplate)

            .Controls.Add(_textJsondata)


        End With

        Me.Controls.Add(component_)

    End Sub

    Protected Overrides Sub Render(ByVal component_ As HtmlTextWriter)

        PrepareRepeaterData()

        Me.RenderContents(component_)

    End Sub

    Protected Overrides Sub RecreateChildControls()

        EnsureChildControls()

    End Sub

#End Region

End Class


Public Class CollectionItem

    Private Property Item As RepeaterItem

    Property Properties As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

    Public Sub New(ByRef item_ As RepeaterItem)
        MyBase.New()
        Item = item_
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Function tester(ByVal a As String) As Object

        Dim b = Item
        Dim c = b.DataItem

        Return Nothing

    End Function

    Public Function GetControlValue(ByRef id_ As String) As Object

        If Item Is Nothing Then

            Return Properties.Item(id_)

        Else

            Dim control_ = Item.FindControl(id_)

            Select Case control_.GetType()

                Case GetType(InputControl)

                    Return DirectCast(control_, InputControl).Value

                Case GetType(SelectControl)

                    Dim selectControl_ = DirectCast(control_, SelectControl)

                    Return New SelectOption() With {.Value = selectControl_.Value, .Text = selectControl_.Text}

                Case GetType(SwitchControl)

                    Return DirectCast(control_, SwitchControl).Checked

                Case GetType(FindboxControl)

                    Dim findboxControl_ = DirectCast(control_, SelectControl)

                    Return New SelectOption() With {.Value = findboxControl_.Value, .Text = findboxControl_.Text}

                Case GetType(CollectionViewControl)

                    Dim collectionviewControl_ = DirectCast(control_, CollectionViewControl)

                    Return collectionviewControl_.DataSource

                Case Else

                    Return Nothing

            End Select

        End If

    End Function

    Public Sub SetControlValue(ByRef id_ As String, ByVal value_ As Object)

        If Item Is Nothing Then
            If Properties.ContainsKey(id_) Then
                Properties(id_) = value_
            Else
                Properties.Add(id_, value_)
            End If


        Else

            Dim control_ = Item.FindControl(id_)

            Select Case control_.GetType()

                Case GetType(InputControl)

                    DirectCast(control_, InputControl).Value = value_

                Case GetType(SelectControl)

                    Dim selectControl_ = DirectCast(control_, SelectControl)

                    With selectControl_

                        .Value = Nothing

                        .DataSource = New List(Of SelectOption) From {value_}

                    End With

                Case GetType(SwitchControl)

                    DirectCast(control_, SwitchControl).Checked = value_

                Case GetType(FindboxControl)

                    Dim findboxControl_ = DirectCast(control_, FindboxControl)

                    With findboxControl_

                        .Value = value_.Value

                        .Text = value_.Text

                    End With

                Case GetType(CollectionViewControl)

                    Dim collectionviewControl_ = DirectCast(control_, CollectionViewControl)

                    With collectionviewControl_

                        .DataSource = value_

                    End With

                Case Else

            End Select

        End If

    End Sub

    Public Sub SetIndice(ByVal id_ As String, value_ As Integer)

        If Properties.ContainsKey(id_) Then

            Properties(id_) = value_

        Else

            Properties.Add(id_, value_)

        End If

    End Sub

    Public Sub SetFiled(value_ As Boolean)

        If Properties.ContainsKey("archivado") Then

            Properties("archivado") = value_

        Else

            Properties.Add("archivado", value_)

        End If

    End Sub

    Public Function GetIndice(ByVal keyField_ As String)

        Return Properties.Item(keyField_)

    End Function

    Public Function IsFiled() As Boolean

        Return Convert.ToBoolean(Properties.Item("archivado"))

    End Function

    Public Function IsDeleted() As Boolean

        Return Convert.ToBoolean(Properties.Item("borrado"))

    End Function

End Class
