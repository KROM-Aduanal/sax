Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.UI.HtmlControls

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
    Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level:=AspNetHostingPermissionLevel.Minimal),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class ImageControl
    Inherits UIControl

#Region "Enums"

    Enum ImageAspect
        None = 0
        Fill = 1
        Contain = 2
        Cover = 3
    End Enum

    Enum ImageShape
        None = 0
        Rounded = 1
        Circle = 2
        Thumbnail = 3
    End Enum

    Enum ImagePosition
        None = 0
        Left = 1
        Center = 2
        Right = 3
        LeftTop = 4
        CenterTop = 5
        RightTop = 6
        LeftBottom = 7
        CenterBottom = 8
        RightBottom = 9
    End Enum

#End Region

#Region "Controles"

    Private _image As Image

    Private _imageTemplate As HtmlGenericControl

    Private _content As HtmlGenericControl

    Private _cssClassImg As String

#End Region

#Region "Propiedades"

    Property Source As String

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public ReadOnly Property Sources As List(Of Source) = New List(Of Source)

    Property Alt As String

    Property Aspect As ImageAspect = ImageAspect.None

    Property Shape As ImageShape = ImageShape.None

    Property Align As ImagePosition

    Public WriteOnly Property CssClassImg As String

        Set(value_ As String)

            _cssClassImg = value_

        End Set

    End Property

#End Region

#Region "Constructor"
#End Region

#Region "Metodos"
#End Region

#Region "Renderizado"

    Private Sub SettingImageControls()

        _content = New HtmlGenericControl("div")

        With _content

            .Attributes.Add("class", CssClass & " wc-image")

            .Attributes.Add("style", "--time:" & (_Sources.Count * 4) & "s")

            Dim aling_ = Align.ToString()

            .Attributes.Add("img-align", aling_)

        End With


        _imageTemplate = New HtmlGenericControl("div")

        With _imageTemplate

            .Attributes.Add("is", "wc-image")

            .Attributes.Add("img-aspect", _Aspect)

            .Attributes.Add("img-url", _Source)

            .Attributes.Add("class", "__component preload")

            .Attributes.Add("img-sources", _Sources.Count)

        End With


        _image = New Image

        With _image

            .AlternateText = Alt

            .Width = Width

            .Height = Height

            .CssClass = _cssClassImg

            Select Case _Shape
                Case ImageShape.Circle

                    .Attributes.Add("class", "img-circle")

                Case ImageShape.Rounded

                    .Attributes.Add("class", "img-rounded")

                Case ImageShape.Thumbnail

                    .Attributes.Add("class", "img-thumbnail")

            End Select

        End With

    End Sub

    Protected Overrides Sub CreateChildControls()

        SettingImageControls()

        _imageTemplate.Controls.Add(_image)

        If _Sources.Count Then

            With _imageTemplate

                .Controls.Add(New LiteralControl("<div>"))

                For Each source_ As Source In _Sources

                    .Controls.Add(New LiteralControl("<span img-url='" & source_.Path & "'></span>"))

                Next

                .Controls.Add(New LiteralControl("</div>"))

            End With

        End If

        _content.Controls.Add(_imageTemplate)

        Me.Controls.Add(_content)

    End Sub


    Protected Overrides Sub Render(ByVal component_ As HtmlTextWriter)

        Me.RenderContents(component_)

    End Sub

    Protected Overrides Sub RecreateChildControls()

        EnsureChildControls()

    End Sub

#End Region

End Class

Public Class Source
    Public Sub New()
        MyBase.New()
    End Sub

    Public Property Path As String

End Class