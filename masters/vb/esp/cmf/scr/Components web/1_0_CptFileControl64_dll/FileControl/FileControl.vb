Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports Gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization
Imports MongoDB.Driver
Imports Rec.Globals
Imports Rec.Globals.Controllers

<Assembly: TagPrefix("Gsol.Web.Components", "GWC")>
<
AspNetHostingPermission(SecurityAction.Demand,
    Level:=AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level:=AspNetHostingPermissionLevel.Minimal),
ToolboxData("<{0}:Register runat=""server""> </{0}:Register>")
>
Public Class FileControl
    Inherits UIInputControl

#Region "Enums"

    Enum FilecontrolModality
        [Default] = 0
        Reading = 1
        Writing = 2
    End Enum

#End Region

#Region "Controles"

    Private _fileInput As FileUpload

    Private _validationsElements As HtmlGenericControl

    Private _unlokedControl As HtmlGenericControl

    Private _textJsondata As TextBox

    Private _uploadActionButton As LinkButton

    Private _downloadActionButton As LinkButton

    Private _deleteActionButton As LinkButton

    Private _textInput As TextBox

    Private _valueInput As TextBox

#End Region

#Region "Propiedades"

    Public Event ChooseFile As EventHandler

    Public Overrides Property Enabled As Boolean

        Get

            Return MyBase.Enabled

        End Get

        Set(value As Boolean)

            If _fileInput IsNot Nothing Then

                If value = True Then

                    _fileInput.Attributes.Remove("disabled")

                Else

                    _fileInput.Attributes.Add("disabled", "disabled")

                End If

            End If

            MyBase.Enabled = value

        End Set

    End Property

    Public Property Modality As FilecontrolModality

    Public Property ShowDetails As Boolean = True

    Public Overloads Property Value As String

        Get

            EnsureChildControls()

            Return _valueInput.Text

        End Get

        Set(value As String)

            EnsureChildControls()

            _valueInput.Text = value

        End Set

    End Property

    Public Property Text As String

        Get

            EnsureChildControls()

            Return _textInput.Text

        End Get

        Set(value As String)

            EnsureChildControls()

            _textInput.Text = value

        End Set

    End Property

    'Public Property Multiple As Boolean = False

    Public Property ShowButtonsTitle As Boolean = True

    Public Property CanDelete As Boolean = True

    Public Property Dragable As Boolean = False

#End Region

#Region "Constructor"
#End Region

#Region "Eventos"


    Private Sub BeforeSelectedFile(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        Dim documentodigital_ = New PropiedadesDocumento

        RaiseEvent ChooseFile(documentodigital_, EventArgs.Empty)

        Dim data_ = New Dictionary(Of String, Object) From {{"type", "upload"}, {"data", documentodigital_}}

        _textJsondata.Text = New JavaScriptSerializer().Serialize(data_)

        _fileInput.Attributes.Add("accept", GetEnumDescription(documentodigital_.formatoarchivo))

        'checar quen onda con el nommbre de archivo me da anciedad lo coloquen los usuarios pero es un rollo mandarlo al controlador

    End Sub

    Private Sub DownloadFile(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        Dim data_ = New Dictionary(Of String, Object) From {{"type", "download"}, {"data", New Dictionary(Of String, Object) From {{"fileId", Value}, {"fileName", Text}}}}

        _textJsondata.Text = New JavaScriptSerializer().Serialize(data_)

    End Sub

    Private Sub DeleteFile(ByVal source As Object, ByVal e As EventArgs)

        EnsureChildControls()

        Dim data_ = New Dictionary(Of String, Object) From {{"type", "delete"}, {"data", New Dictionary(Of String, Object) From {{"fileId", Value}}}}

        _textJsondata.Text = New JavaScriptSerializer().Serialize(data_)

    End Sub

#End Region

#Region "Metodos"

    Public Sub EnabledButton(ByVal enabled_ As Boolean)

        EnsureChildControls()

        If enabled_ = False Then

            _uploadActionButton.Attributes.Add("class", "__upload disabled")

            _deleteActionButton.Attributes.Add("class", "__delete disabled")

        Else

            _uploadActionButton.Attributes.Add("class", "__upload")

            _deleteActionButton.Attributes.Add("class", "__delete")

        End If

    End Sub

    Private Function GetEnumDescription(Of TEnum)(enumObj_ As TEnum) As String

        Dim fi_ As FieldInfo = enumObj_.GetType().GetField(enumObj_.ToString())

        Dim attributes_ As DescriptionAttribute() = fi_.GetCustomAttributes(GetType(DescriptionAttribute), False)

        If attributes_ IsNot Nothing AndAlso attributes_.Length > 0 Then

            Return attributes_(0).Description

        Else

            Return enumObj_.ToString()

        End If

    End Function

    Private Sub SettingFileControls()

        _textInput = New TextBox

        With _textInput

            '.ReadOnly = True

            .Attributes.Add("class", "__text")

            .Attributes.Add("placeholder", Label & " (Seleccione un archivo)")

        End With

        _valueInput = New TextBox

        With _valueInput

            .Attributes.Add("class", "d-none __value")

        End With

        _fileInput = New FileUpload

        With _fileInput

            .ID = ID

            .Attributes.Add("class", "d-none")

            .Attributes.Add("is", "wc-file")

            'If Multiple = True Then

            '    .Attributes.Add("limit", 20)

            '    '.AllowMultiple = True

            'End If

        End With

        _textJsondata = New TextBox

        With _textJsondata

            .Attributes.Add("class", "__data d-none")

        End With

        _uploadActionButton = New LinkButton

        With _uploadActionButton

            AddHandler .Click, AddressOf BeforeSelectedFile

            .CausesValidation = False

            If ShowButtonsTitle = True Then

                .Text = "Cargar"

            End If

            .Attributes.Add("class", "__upload")

            'If Modality = FilecontrolModality.Default Then

            '    .Attributes.Add("split", "true")

            'End If

        End With

        _downloadActionButton = New LinkButton

        With _downloadActionButton

            AddHandler .Click, AddressOf DownloadFile

            .CausesValidation = False

            If ShowButtonsTitle = True Then

                .Text = "Descargar"

            End If

            .Attributes.Add("class", "__download")

            'If Modality = FilecontrolModality.Default Then

            '    .Attributes.Add("split", "true")

            'End If

        End With

        _deleteActionButton = New LinkButton

        With _deleteActionButton

            AddHandler .Click, AddressOf DeleteFile

            .CausesValidation = False

            If ShowButtonsTitle = True Then

                .Text = "Eliminar"

            End If

            .Attributes.Add("class", "__delete")

            'If Modality = FilecontrolModality.Default Then

            '    .Attributes.Add("split", "true")

            'End If

        End With



    End Sub

#End Region

#Region "Renderizado"

    Protected Overrides Sub CreateChildControls()

        SettingFileControls()

        Dim component_ = New HtmlGenericControl("div")

        component_.Attributes.Add("class", CssClass)

        With component_

            If Dragable = False Then

                .Controls.Add(New LiteralControl("<div class='row no-gutters wc-file mb-5 " & IIf(ShowDetails = True, "", "only-buttons") & " __component' " & ForeColor.HtmlPropertyColor & ">"))

                If ShowDetails = True Then

                    .Controls.Add(New LiteralControl("  <div class='col d-flex align-items-center'>"))

                    .Controls.Add(_textInput)

                    .Controls.Add(New LiteralControl("  </div>"))

                End If

                .Controls.Add(New LiteralControl("  <div class='col-auto d-flex align-items-center'>"))

                If Modality = FilecontrolModality.Default Then

                    .Controls.Add(_uploadActionButton)

                    .Controls.Add(_downloadActionButton)

                ElseIf Modality = FilecontrolModality.Reading Then

                    .Controls.Add(_downloadActionButton)

                ElseIf Modality = FilecontrolModality.Writing Then

                    .Controls.Add(_uploadActionButton)

                End If

                If CanDelete Then

                    .Controls.Add(_deleteActionButton)

                End If

                .Controls.Add(New LiteralControl("  </div>"))

                .Controls.Add(_fileInput)

                .Controls.Add(_valueInput)

                .Controls.Add(_textJsondata)

                .Controls.Add(New LiteralControl("</div>"))

            Else


                Dim documentodigital_ = New PropiedadesDocumento

                RaiseEvent ChooseFile(documentodigital_, EventArgs.Empty)

                Dim data_ = New Dictionary(Of String, Object) From {{"type", "dragable"}, {"data", documentodigital_}}

                _textJsondata.Text = New JavaScriptSerializer().Serialize(data_)

                _fileInput.Attributes.Add("accept", GetEnumDescription(documentodigital_.formatoarchivo))


                .Controls.Add(New LiteralControl("<div class='row no-gutters wc-file wc-file-dragable mb-5 __component' " & ForeColor.HtmlPropertyColor & ">"))

                .Controls.Add(New LiteralControl("<div data-placeholder='" & Label & "'>"))

                .Controls.Add(New LiteralControl("<div class='__details'><svg xmlns='http: //www.w3.org/2000/svg' width='50' height='42' style='position: absolute; bottom: 6px; right:48%; opacity:0.9' viewBox='0 0 640 512'><path fill='#9c27b0' d='M144 480C64.5 480 0 415.5 0 336c0-62.8 40.2-116.2 96.2-135.9c-.1-2.7-.2-5.4-.2-8.1c0-88.4 71.6-160 160-160c59.3 0 111 32.2 138.7 80.2C409.9 102 428.3 96 448 96c53 0 96 43 96 96c0 12.2-2.3 23.8-6.4 34.6C596 238.4 640 290.1 640 352c0 70.7-57.3 128-128 128l-368 0zm79-217c-9.4 9.4-9.4 24.6 0 33.9s24.6 9.4 33.9 0l39-39L296 392c0 13.3 10.7 24 24 24s24-10.7 24-24l0-134.1 39 39c9.4 9.4 24.6 9.4 33.9 0s9.4-24.6 0-33.9l-80-80c-9.4-9.4-24.6-9.4-33.9 0l-80 80z'/></svg></div>"))

                .Controls.Add(New LiteralControl("<div class='__items'></div>"))

                .Controls.Add(New LiteralControl("</div>"))

                .Controls.Add(_fileInput)

                .Controls.Add(_valueInput)

                .Controls.Add(_textJsondata)

                .Controls.Add(New LiteralControl("</div>"))

            End If

        End With

        Me.Controls.Add(component_)

    End Sub

    Protected Overrides Sub Render(ByVal component_ As HtmlTextWriter)

        If String.IsNullOrEmpty(Value) Then

            _downloadActionButton.Attributes.Add("class", "__download disabled")

            _deleteActionButton.Attributes.Add("class", "__delete disabled")

        End If

        If Not String.IsNullOrEmpty(Text) Then

            _textInput.Text = Text

        End If

        If Locked = True Then

            _unlokedControl.Controls.Add(SetLockedControl())

        End If

        Me.RenderContents(component_)

    End Sub

    Protected Overrides Sub RecreateChildControls()

        EnsureChildControls()

    End Sub

#End Region

End Class


