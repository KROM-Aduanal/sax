Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Drawing
Imports Gsol.krom

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<ToolboxData("<{0}:FilterbarControl runat=server></{0}:FilterbarControl>")>
Public Class FilterbarControl
    Inherits UIControlDataConnector

#Region "Atributos"

    Property _freeClauses As String

    Private _filters As New List(Of FilterItem)

#End Region

#Region "Propiedades"

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public ReadOnly Property Filters As List(Of FilterItem)
        Get

            If _filters Is Nothing Then
                _filters = New List(Of FilterItem)()
            End If

            Return _filters
        End Get
    End Property

#End Region

#Region "Constructor"
    Sub New()

        _filters = New List(Of FilterItem)

        CssClass = "w-100"

    End Sub
#End Region

#Region "Metodos"

    Protected Overrides Sub RenderContents(ByVal component_ As HtmlTextWriter)

        Dim control_ = New HtmlGenericControl("div")

        control_.Attributes.Add("class", "wc-filterbar position-relative " & CssClass)

        control_.Attributes.Add("id", ID)

        control_.Attributes.Add("is", "wc-filterbar")

        If Not ForeColor = Color.Empty Then

            control_.Attributes.Add("style", "--tintColor: " & ForeColor.ToHex)

        End If

        control_.Controls.Add(New LiteralControl("<div class='find-menu mr-4'>"))
        control_.Controls.Add(New LiteralControl("  <div Class='find-menu-title'>" & Label & "</div>"))

        AppendDefaultRows(control_)

        control_.Controls.Add(New LiteralControl("<label><a href=''>MOSTRAR MÁS</a></label>"))

        control_.Controls.Add(New LiteralControl("<div Class='find-menu-butonner'>"))
        control_.Controls.Add(New LiteralControl("  <div> <a href=''> Restaurar</a></div>"))
        control_.Controls.Add(New LiteralControl("  <div> <a href=''> Hecho</a></div>"))
        control_.Controls.Add(New LiteralControl("</div>"))

        control_.Controls.Add(New LiteralControl("</div>"))


        control_.RenderControl(component_)

    End Sub

    Protected Overrides Sub Render(ByVal component_ As HtmlTextWriter)

        Me.RenderContents(component_)

    End Sub

    Private Sub AppendDefaultRows(ByRef control_ As HtmlGenericControl)

        Dim index_ = 1

        For Each item_ As FilterItem In _filters

            control_.Controls.Add(New LiteralControl("<input id='dd" & index_ & "' type='checkbox' Class='d-none'/>"))
            control_.Controls.Add(New LiteralControl("<label for='dd" & index_ & "'><i></i>" & item_.Text & "<small>Cualquiera</small></label>"))

            If item_.Items.Count > 0 Then

                control_.Controls.Add(New LiteralControl("<nav>"))

                For Each option_ As FilterOption In item_.Items

                    control_.Controls.Add(New LiteralControl("<div Class='d-flex'>"))
                    control_.Controls.Add(New LiteralControl("    <a Class='col'>" & option_.Text & " <span>" & option_.Badge & "</span></a><input class='col-auto' type='checkbox'/>"))
                    control_.Controls.Add(New LiteralControl("</div>"))

                Next

                control_.Controls.Add(New LiteralControl("</nav>"))

            Else

                If item_.Dimension And item_.DataEntity IsNot Nothing Then

                    Dim results_ = item_.RealizarConsulta()

                    If Not results_ Is Nothing Then

                        control_.Controls.Add(New LiteralControl("<nav>"))

                        For Each row_ As DataRow In results_.Rows

                            control_.Controls.Add(New LiteralControl("<div Class='d-flex'>"))
                            control_.Controls.Add(New LiteralControl("    <a Class='col'>" & row_(item_.DisplayField) & " <span>0</span></a><input class='col-auto' type='checkbox'/>"))
                            control_.Controls.Add(New LiteralControl("</div>"))

                        Next row_

                        control_.Controls.Add(New LiteralControl("</nav>"))

                    End If

                End If

            End If

            index_ += 1

        Next

    End Sub

#End Region

End Class


Public Class FilterItem
    Inherits UIControlDataConnector

    Private _items As List(Of FilterOption)

    Public Sub New()

        MyBase.New()

        _items = New List(Of FilterOption)

    End Sub

    Public Property Text As String

    Public Property ID As String

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public ReadOnly Property Items As List(Of FilterOption)
        Get

            If _items Is Nothing Then
                _items = New List(Of FilterOption)()
            End If

            Return _items
        End Get
    End Property

    Public Function RealizarConsulta() As DataTable

        If Dimension And Not DataEntity Is Nothing Then

            Dim Form = New Template.FormularioGeneralWeb()

            With DataEntity

                .cmp(KeyField)

                .cmp(DisplayField)

            End With

            Return Form.ConsultarEnlace(entidad_:=DataEntity,
                                        dimension_:=Dimension,
                                        granularidad_:=Granularity,
                                        clausulasLibres_:=FreeClauses)

        End If

        Return Nothing

    End Function

End Class
Public Class FilterOption

    Public Sub New()
        MyBase.New()
    End Sub

    Public Property Text As String

    Public Property Badge As Integer

End Class