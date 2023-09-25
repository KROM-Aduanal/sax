Imports Wma.Operations
Imports Wma.Components.ObjetoPregunta
'Imports Gsol.BaseDatos.Operaciones
Imports System.Windows.Forms
Imports System.Text
Imports Gsol.BaseDatos.Operaciones
Imports System.Drawing

Namespace Wma.Components

    Public Class ObjetoPregunta

#Region "Enums"

        Public Enum EstatusPregunta

            SinDefinir = 0

            EsCorrecta = 1

            EsIncorrecta = 2

            Otro = 3

        End Enum

#End Region

#Region "Atributos"

        'Mascara
        Private _mascara As String
        'Título
        Private _titulo As String
        'Valores
        Private _listaValores As List(Of String)
        'Obligatoria
        Private _obligatoria As String = Nothing

        'Validar
        Private _validar As String = Nothing

        'Controles

        Public WithEvents _textBox As TextBox

        Public WithEvents _maskedTextBox As MaskedTextBox

        Public WithEvents _comboBox As ComboBox

        'Paneles

        Private _panelEncabezado As Panel

        Private _panelContenido As Panel

        Public WithEvents _panelPie As Panel

        'Comentarios

        Public WithEvents _lblComentarios As Label

        'Otras propiedades

        Private requestedObject_ As New List(Of ComponentView)

        Private _formatoPregunta As String = Nothing

        Private _tiempoRespuesta As String = Nothing

        Private _valoresString As New List(Of ParValoresString)

        Private _validaciones As New List(Of ParValoresString)

        Private _vinculacion As String = Nothing

        Private _nameAsKey As String = Nothing

        Private _campoDesplegable As String = Nothing

        Private _campoLlave As String = Nothing

        Private _filtrosAvanzados As String = Nothing

        Private _estatus As EstatusPregunta

        Private _id As String

        Private _visible As String

        Private _numeroIntentos As String

        Private _idSiguienteAnidada As String


#End Region

#Region "Eventos"

        Public Event AlResponderCorrectamente()

#End Region

#Region "Constructor"

        Sub New(ByVal icaracteristica_ As ICaracteristica,
                ByRef panelHeader_ As Panel,
                ByRef panelContenido_ As Panel,
                ByRef panelPie_ As Panel,
                ByRef lblComentarios_ As Label)

            _panelContenido = panelContenido_

            _panelEncabezado = panelHeader_

            _panelPie = panelPie_

            _lblComentarios = lblComentarios_

            ProcesarInterfazCaracteristica(icaracteristica_)

        End Sub

#End Region

#Region "Propiedades"

        'Private _id As String
        Public Property IDSiguienteAnidada As String

            Get

                Return _idSiguienteAnidada

            End Get

            Set(value As String)

                _idSiguienteAnidada = value

            End Set

        End Property

        'Private _id As String
        Public Property ID As String

            Get

                Return _id

            End Get

            Set(value As String)

                _id = value

            End Set

        End Property

        'Private _visible As String
        Public Property Visible As String

            Get

                Return _visible

            End Get

            Set(value As String)

                _visible = value

            End Set

        End Property

        'Private _numeroIntentos As String
        Public Property NumeroIntentos As String

            Get

                Return _numeroIntentos

            End Get

            Set(value As String)

                _numeroIntentos = value

            End Set

        End Property

        Public Property FormatoPregunta As String

            Get

                Return _formatoPregunta

            End Get

            Set(value As String)

                _formatoPregunta = value

            End Set

        End Property

        Public Property Mascara As String

            Get

                Return _mascara

            End Get

            Set(value As String)

                _mascara = value

            End Set

        End Property

        Public Property Titulo As String

            Get

                Return _titulo

            End Get

            Set(value As String)

                _titulo = value

            End Set

        End Property

        Public Property ListaValores As List(Of String)

            Get

                Return _listaValores

            End Get

            Set(value As List(Of String))

                _listaValores = value

            End Set

        End Property

        Public Property Obligatoria As String

            Get

                Return _obligatoria

            End Get

            Set(value As String)

                _obligatoria = value

            End Set

        End Property

        Public Property Validar As String

            Get

                Return _validar

            End Get

            Set(value As String)

                _validar = value

            End Set

        End Property

        Property Estatus As EstatusPregunta
            Get
                Return _estatus
            End Get
            Set(value As EstatusPregunta)
                _estatus = value
            End Set

        End Property

#End Region

#Region "Métodos"

        Public Sub OnMaskedTextBoxChanged() Handles _maskedTextBox.TextChanged

            ValidaControl(_maskedTextBox)

        End Sub

        Public Sub OnLeaveTextBox() Handles _maskedTextBox.Leave

            ValidaControl(_maskedTextBox)

        End Sub

        Public Sub OnLostFocusTextBox() Handles _maskedTextBox.LostFocus

            ValidaControl(_maskedTextBox)

        End Sub

        Public Sub OnSelectedIndexChanged() Handles _comboBox.SelectedIndexChanged

            ValidaControl(_comboBox)

        End Sub

        Private Sub OnRadioButtonsChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            If CType(sender, RadioButton).Checked = True Then

                ValidaControl(CType(sender, RadioButton))

            End If

        End Sub

        Private Function ProcesarInterfazCaracteristica(ByVal icaracteristica_ As ICaracteristica) As Boolean

            Dim jsonFile_ As String = icaracteristica_.Interfaz

            Try

                If Not jsonFile_ Is Nothing Then

                    Dim serialJSON_ As New System.Web.Script.Serialization.JavaScriptSerializer

                    requestedObject_ = serialJSON_.Deserialize(Of List(Of ComponentView))(jsonFile_)

                    If Not requestedObject_ Is Nothing Then

                        _formatoPregunta = requestedObject_(0).Tipo

                        _mascara = requestedObject_(0).Mascara

                        _titulo = requestedObject_(0).Titulo

                        _valoresString = requestedObject_(0).ValoresString

                        _obligatoria = requestedObject_(0).Obligatoria

                        _tiempoRespuesta = requestedObject_(0).TiempoRespuesta

                        _validar = requestedObject_(0).Validar

                        _validaciones = requestedObject_(0).Validaciones

                        _vinculacion = requestedObject_(0).Vinculacion

                        _campoDesplegable = requestedObject_(0).CampoDesplegar

                        _campoLlave = requestedObject_(0).CampoLLave

                        _filtrosAvanzados = requestedObject_(0).ClausulasLibres

                        _nameAsKey = icaracteristica_.NameAsKey

                        _id = requestedObject_(0).Id

                        _visible = requestedObject_(0).Visible

                        _numeroIntentos = requestedObject_(0).NumeroIntentos

                    End If

                End If

                Select Case _formatoPregunta

                    Case "PreguntaTextBoxMask"

                        _maskedTextBox = New MaskedTextBox

                        _maskedTextBox.Mask = _mascara

                        _panelContenido.Controls.Add(_maskedTextBox)

                        'Dim evento_ As New EventHandler(AddressOf OnLeaveTextBox)
                        'Dim evento_ As New EventHandler(AddressOf OnLostFocusTextBox)
                        Dim evento_ As New EventHandler(AddressOf OnMaskedTextBoxChanged)

                        AddHandler _maskedTextBox.TextChanged, evento_
                        'AddHandler _maskedTextBox.Leave, evento_
                        'AddHandler _maskedTextBox.LostFocus, evento_

                    Case "PreguntaRadioButton"

                        Dim espacio_ As Int32 = 0

                        Dim indice_ As Int32 = 1

                        For Each parValores_ As ParValoresString In _valoresString

                            Dim radioButton_ As New RadioButton

                            radioButton_.Name = "rb" & indice_

                            radioButton_.Text = parValores_.Descripcion

                            radioButton_.Dock = DockStyle.Top

                            radioButton_.Left = espacio_

                            Dim point_ As New Point(10, 25 + espacio_)

                            radioButton_.Location = point_

                            espacio_ += 150

                            _panelContenido.Controls.Add(radioButton_)

                            Dim evento_ As New EventHandler(AddressOf OnRadioButtonsChanged)

                            AddHandler radioButton_.CheckedChanged, evento_

                            indice_ += 1

                        Next

                    Case "PreguntaComboBox"

                        _comboBox = New ComboBox

                        _comboBox.Height = 500

                        _comboBox.DropDownStyle = ComboBoxStyle.DropDownList

                        For Each parValores_ As ParValoresString In _valoresString

                            _comboBox.Items.Add(parValores_.Descripcion)

                        Next

                        _panelContenido.Controls.Add(_comboBox)

                    Case Else

                        'NOT IMPLEMENTED

                End Select

            Catch ex As Exception

                MsgBox("hugo un problema al momento de cargar la VE para este formulario, revise la configuración y vuelva a intentar, detalles: " & ex.Message)

            End Try

            Return True

        End Function

        Public Function ValidaControl(ByRef control_ As Object,
                                      Optional ByVal formatoPregunta_ As String = Nothing,
                                      Optional ByVal modalidad_ As Int32 = 0) As Boolean

            Dim respuesta_ As Boolean = False

            Dim comentarios_ As String = "Respuesta incorrecta."

            If formatoPregunta_ Is Nothing Then : formatoPregunta_ = _formatoPregunta : End If

            Select Case Trim(LCase(formatoPregunta_))

                Case "PreguntaTextBoxMask", "preguntatextboxmask"

                    If Not _validaciones Is Nothing Then

                        Dim validadorPregunta_ As New ValidadorPregunta(_validaciones)

                        With validadorPregunta_

                            .VerificaRespuesta(control_.Text)

                            If .Estatus = EstatusPregunta.EsCorrecta Then

                                comentarios_ = Nothing

                                _estatus = .Estatus

                                If Not .IDPreguntaSiguienteAnidada Is Nothing Then

                                    For Each _par As ParValoresString In _validaciones

                                        If _par.Descripcion = "IDSiguientePregunta" Then

                                            _idSiguienteAnidada = _par.Valor

                                            Exit For

                                        End If

                                    Next

                                End If

                                _idSiguienteAnidada = .IDPreguntaSiguienteAnidada

                            Else

                                comentarios_ = .ErrorPorDefecto

                                _estatus = .Estatus

                                _idSiguienteAnidada = Nothing

                            End If

                        End With

                    Else

                        comentarios_ = Nothing

                    End If

                Case "PreguntaRadioButton", "preguntaradiobutton"

                    If Not _validaciones Is Nothing Then

                        Dim validadorPregunta_ As New ValidadorPregunta(_validaciones)

                        With validadorPregunta_

                            .VerificaRespuesta(control_.Text)

                            If .Estatus = EstatusPregunta.EsCorrecta Then

                                comentarios_ = Nothing

                                _estatus = .Estatus

                                _idSiguienteAnidada = .IDPreguntaSiguienteAnidada

                            Else

                                comentarios_ = .ErrorPorDefecto

                                _estatus = .Estatus

                                _idSiguienteAnidada = Nothing

                            End If

                        End With

                    Else

                        comentarios_ = Nothing

                    End If

                Case "PreguntaComboBox", "preguntacombobox"

                    If Not _validaciones Is Nothing Then

                        Dim validadorPregunta_ As New ValidadorPregunta(_validaciones)

                        With validadorPregunta_

                            .VerificaRespuesta(control_.Text)

                            If .Estatus = EstatusPregunta.EsCorrecta Then

                                comentarios_ = Nothing

                                _estatus = .Estatus

                                _idSiguienteAnidada = .IDPreguntaSiguienteAnidada

                            Else

                                comentarios_ = .ErrorPorDefecto

                                _estatus = .Estatus

                                _idSiguienteAnidada = Nothing

                            End If

                        End With

                    Else

                        comentarios_ = Nothing

                    End If

                Case Else

            End Select

            If Not comentarios_ Is Nothing Then

                _lblComentarios.Text = comentarios_

                _lblComentarios.ForeColor = Color.Coral
            Else

                _lblComentarios.Text = "OK"

                _lblComentarios.ForeColor = Color.Teal

            End If

            If _estatus = EstatusPregunta.EsCorrecta Then

                RaiseEvent AlResponderCorrectamente()

            End If

            Return respuesta_

        End Function

#End Region

    End Class

    Public Class ValidadorPregunta

#Region "Atributos"

        Private _respuestasCorrectas As List(Of ParValoresString)

        Private _errorPorDefecto As String

        Private _idSiguientePregunta As String

        Private _siguientePregunta As String

        Private _estatus As EstatusPregunta

        Private _idPreguntaSiguienteAnidada As String = Nothing

#End Region

#Region "Propiedades"

        Public Property IDPreguntaSiguienteAnidada As String

            Get

                Return _idPreguntaSiguienteAnidada

            End Get

            Set(value As String)

                _idPreguntaSiguienteAnidada = value

            End Set

        End Property

        Public Property RespuestasCorrectas As List(Of ParValoresString)

            Get

                Return _respuestasCorrectas

            End Get

            Set(value As List(Of ParValoresString))

                _respuestasCorrectas = value

            End Set

        End Property

        Public Property ErrorPorDefecto As String

            Get

                Return _errorPorDefecto

            End Get

            Set(value As String)

                _errorPorDefecto = value

            End Set

        End Property

        Public Property IDSiguientePregunta As String

            Get

                Return _idSiguientePregunta

            End Get

            Set(value As String)

                _idSiguientePregunta = value

            End Set

        End Property


        Public Property Estatus As EstatusPregunta

            Get

                Return _estatus

            End Get

            Set(value As EstatusPregunta)

                _estatus = value

            End Set

        End Property

#End Region

#Region "Constructores"

        Sub New(ByVal validaciones_ As List(Of ParValoresString))

            _respuestasCorrectas = New List(Of ParValoresString)

            With _respuestasCorrectas

                For Each parvalores_ As ParValoresString In validaciones_

                    If LCase(Trim(parvalores_.Descripcion)) = "equalsto" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "greaterthan" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "greaterorequalsthan" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "minorthan" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "isdifferentof" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "betweenvalues" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "startwith" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "endswith" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "contains" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "void" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "noequalsto" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "nogreaterthan" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "nominorthan" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "noisdifferentof" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "nobetweenvalues" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "nostartwith" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "noendswith" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "nocontains" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "novoid" Then

                        .Add(parvalores_)

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "error" Then

                        _errorPorDefecto = parvalores_.Valor

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "idsiguientepregunta" Then

                        _idSiguientePregunta = parvalores_.Valor

                    ElseIf LCase(Trim(parvalores_.Descripcion)) = "siguientepregunta" Then

                        _siguientePregunta = parvalores_.Valor

                    End If
                Next

            End With

            _estatus = EstatusPregunta.SinDefinir

        End Sub

#End Region

#Region "Métodos"

        Public Function VerificaRespuesta(ByVal respuesta_ As String) As EstatusPregunta

            _estatus = EstatusPregunta.SinDefinir

            For Each parCorrecto_ As ParValoresString In _respuestasCorrectas

                Dim respuestaCorrectaConfigurada_ As String = parCorrecto_.Valor

                Dim segmentos_ As String() = Split(respuestaCorrectaConfigurada_, ":")

                If UBound(segmentos_) = 0 Then

                    respuestaCorrectaConfigurada_ = segmentos_(0)

                Else

                    respuestaCorrectaConfigurada_ = segmentos_(0)

                    _idPreguntaSiguienteAnidada = segmentos_(1)

                End If

                Select Case Trim(LCase(parCorrecto_.Descripcion))

                    Case "equalsto"

                        If Trim(LCase(respuestaCorrectaConfigurada_)) = LCase(Trim(respuesta_)) Then

                            _estatus = EstatusPregunta.EsCorrecta

                            Exit For

                            Return _estatus

                        Else

                            _estatus = EstatusPregunta.EsIncorrecta

                        End If

                    Case "greaterthan"

                        Dim valorCorrecto_ As Double = 0

                        Dim valorPropuesto_ As Double = 0

                        If IsNumeric(respuesta_) Then

                            valorPropuesto_ = Convert.ToDouble(LTrim(respuesta_))

                            If IsNumeric(respuestaCorrectaConfigurada_) Then

                                valorCorrecto_ = Convert.ToDouble(LTrim(respuestaCorrectaConfigurada_))

                                If valorPropuesto_ > valorCorrecto_ Then

                                    _estatus = EstatusPregunta.EsCorrecta

                                    Exit For

                                    Return _estatus

                                Else

                                    _estatus = EstatusPregunta.EsIncorrecta

                                End If

                            End If

                        End If

                    Case "greaterorequalsthan"

                        Dim valorCorrecto_ As Double = 0

                        Dim valorPropuesto_ As Double = 0

                        If IsNumeric(respuesta_) Then

                            valorPropuesto_ = Convert.ToDouble(LTrim(respuesta_))

                            If IsNumeric(respuestaCorrectaConfigurada_) Then

                                valorCorrecto_ = Convert.ToDouble(LTrim(respuestaCorrectaConfigurada_))

                                If valorPropuesto_ >= valorCorrecto_ Then

                                    _estatus = EstatusPregunta.EsCorrecta

                                    Exit For

                                    Return _estatus

                                Else

                                    _estatus = EstatusPregunta.EsIncorrecta

                                End If

                            End If

                        End If

                    Case "minorthan" 'NOT IMPLEMENTED
                    Case "isdifferentof" 'NOT IMPLEMENTED
                    Case "betweenvalues" 'NOT IMPLEMENTED
                    Case "startwith" 'NOT IMPLEMENTED 
                    Case "endswith" 'NOT IMPLEMENTED 
                    Case "contains" 'NOT IMPLEMENTED 
                    Case "void" 'NOT IMPLEMENTED 
                    Case "noequalsto" 'NOT IMPLEMENTED 
                    Case "nogreaterthan" 'NOT IMPLEMENTED 
                    Case "greaterorequalsthan"
                    Case "nominorthan" 'NOT IMPLEMENTED 
                    Case "noisdifferentof" 'NOT IMPLEMENTED 
                    Case "nobetweenvalues" 'NOT IMPLEMENTED 
                    Case "nostartwith" 'NOT IMPLEMENTED 
                    Case "noendswith" 'NOT IMPLEMENTED 
                    Case "nocontains" 'NOT IMPLEMENTED 
                    Case "novoid" 'NOT IMPLEMENTED 
                    Case Else
                        'NOT IMPLEMENTED
                        _estatus = EstatusPregunta.Otro

                End Select

            Next

            'Mostramos el formulario oculto

            'If _estatus = EstatusPregunta.EsCorrecta And _idPreguntaSiguienteAnidada <> Nothing Then

            '    MuestraPreguntaID(_idPreguntaSiguienteAnidada)

            'End If

            Return _estatus

        End Function

        'Public Sub MuestraPreguntaID(ByVal id_ As String)


        'End Sub

#End Region

    End Class


End Namespace



