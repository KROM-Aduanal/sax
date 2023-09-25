


Public Class GsDialogo

#Region "Atributos"
    'Contestaciones
    Enum Contestacion
        Ok = 0
        Cancelar = 1
        Ignorar = 2
        Ayuda = 3
    End Enum


    Enum TipoDialogo
        Pregunta = 0
        Aviso = 1
        Alerta = 2
        Err = 3
        PreguntaGrande = 4
        AvisoGrande = 5
        AlertaGrande = 6
        ErrGrande = 7
        CajaChicaEntrada = 8
        CajaGrandeEntrada = 9
    End Enum

    Private _tipodialogo As TipoDialogo

    Private _contestacion As Contestacion

    Private _respuestaSimple As String

#End Region

#Region "Constructores"

    Sub New()

        ' Llamada necesaria para el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        _tipodialogo = TipoDialogo.Pregunta

        _contestacion = Contestacion.Cancelar

        _respuestaSimple = Nothing

        lblMensajeRE.Visible = False

    End Sub


#End Region

#Region "Propiedades"



    Public Property Respuesta As String

        Get

            Return _respuestaSimple

        End Get

        Set(value As String)

            _respuestaSimple = value

        End Set

    End Property

#End Region

#Region "Metodos"

    Public Function Invocar(ByRef respuesta_ As String,
                            ByRef richEditTextbox_ As RichTextBox, _
                            Optional ByVal tipodialogo_ As TipoDialogo = TipoDialogo.Aviso) As Contestacion

        Dim top_ As Integer = lblMensajeRE.Top
        Dim left_ As Integer = lblMensajeRE.Left
        Dim height_ As Integer = lblMensajeRE.Height
        Dim width_ As Integer = lblMensajeRE.Width


        Me.Controls.Add(richEditTextbox_)

        lblMensajeRE = richEditTextbox_

        lblMensajeRE.BorderStyle = BorderStyle.None
        lblMensajeRE.Top = top_
        lblMensajeRE.Left = left_
        lblMensajeRE.Height = height_
        lblMensajeRE.Width = width_

        lblMensajeRE.Anchor = AnchorStyles.Top
        lblMensajeRE.Anchor = AnchorStyles.Left

        richEditTextbox_.Visible = True

        LblMensaje.Visible = False
        lblMensajeRE.Visible = True
        LblMensaje.Clear()

        LblTipo.Text = "{" & tipodialogo_.ToString.Replace("Grande", Nothing) & "}"

        Select Case tipodialogo_

            Case TipoDialogo.Aviso

                btnAceptar.Visible = True
                'btnAceptar.Location = New Point(132, 139)
                btnAceptar.Location = New Point(191, 10)

                Me.Height = 260

                lblMensajeRE.Location = New Point(56, 43)
                lblMensajeRE.Width = 371
                lblMensajeRE.Height = 113

                'Location(56, 43)

                'Width(371)
                'Height(113)

                btnCancelar.Visible = False

            Case TipoDialogo.AvisoGrande

                LblTipo.Text = "{Información}"

                btnAceptar.Visible = True

                btnAceptar.Location = New Point(191, 10)

                Me.Height = 500

                '******************************************

                lblMensajeRE.Location = New Point(60, 44)
                lblMensajeRE.Width = 371
                lblMensajeRE.Height = 350

                'Location(56, 43)

                'Width(371)
                'Height(113)
                '******************************************

                btnCancelar.Visible = False

            Case TipoDialogo.CajaChicaEntrada

                'Not supported

            Case TipoDialogo.CajaGrandeEntrada

                'Not supported

            Case Else
                'btnAceptar.Location = New Point(72, 139)
                btnAceptar.Location = New Point(91, 10)


                btnAceptar.Visible = True
                btnCancelar.Visible = True

        End Select

        Me.ShowDialog()

        respuesta_ = _respuestaSimple

        Return _contestacion

    End Function

    Public Function Invocar(ByRef respuesta_ As String,
                            ByVal mensaje_ As String, _
                            Optional ByVal tipodialogo_ As TipoDialogo = TipoDialogo.Aviso) As Contestacion

        LblMensaje.Visible = True
        lblMensajeRE.Visible = False
        LblMensaje.Text = mensaje_
        lblMensajeRE.Clear()

        LblTipo.Text = "{" & tipodialogo_.ToString.Replace("Grande", Nothing) & "}"

        Select Case tipodialogo_

            Case TipoDialogo.Aviso

                btnAceptar.Visible = True
                'btnAceptar.Location = New Point(132, 139)
                btnAceptar.Location = New Point(191, 10)

                Me.Height = 260

                btnCancelar.Visible = False

                LblMensaje.SelectedText = Nothing

            Case TipoDialogo.AvisoGrande

                LblTipo.Text = "{Información}"

                btnAceptar.Visible = True
                'btnAceptar.Location = New Point(132, 139)
                btnAceptar.Location = New Point(191, 10)

                Me.Height = 500

                btnCancelar.Visible = False

            Case TipoDialogo.CajaChicaEntrada

                LblTipo.Text = mensaje_ ' "{Ingresar datos}"

                btnAceptar.Visible = True
                'btnAceptar.Location = New Point(132, 139)
                btnAceptar.Location = New Point(191, 10)

                Me.Height = 180


                LblMensaje.Multiline = False

                LblMensaje.BackColor = Color.LightYellow

                LblMensaje.PasswordChar = "*"

                LblMensaje.BorderStyle = BorderStyle.FixedSingle

                LblMensaje.Text = Nothing

                LblMensaje.Height = 25

                LblMensaje.ReadOnly = False


                btnCancelar.Visible = False

            Case TipoDialogo.CajaGrandeEntrada

                LblTipo.Text = "{Ingresar datos}" 'mensaje_

                LblTipo.AutoSize = True

                Me.Height = 180

                LblMensaje.Multiline = True

                LblMensaje.BackColor = Color.LightYellow

                LblMensaje.BorderStyle = BorderStyle.FixedSingle

                LblMensaje.Text = Nothing

                LblMensaje.Height = 65

                Dim miLabel_ As New Label()

                miLabel_.Text = mensaje_

                miLabel_.Location = New Point(68, 43)

                miLabel_.AutoSize = True

                Me.Controls.Add(miLabel_)

                LblMensaje.Location = New Point(68, 88)

                lblMensajeRE.Location = New Point(56, 88)

                LblMensaje.ReadOnly = False

            Case Else
                'btnAceptar.Location = New Point(72, 139)
                btnAceptar.Location = New Point(91, 10)


                btnAceptar.Visible = True
                btnCancelar.Visible = True

        End Select

        Me.ShowDialog()

        respuesta_ = _respuestaSimple

        Return _contestacion

    End Function

    Public Function Invocar(ByVal mensaje_ As String, _
                            Optional ByVal tipodialogo_ As TipoDialogo = TipoDialogo.Aviso) As Contestacion
        LblMensaje.Text = mensaje_

        LblTipo.Text = "{" & tipodialogo_.ToString.Replace("Grande", Nothing) & "}"

        Select Case tipodialogo_

            Case TipoDialogo.Aviso

                btnAceptar.Visible = True
                'btnAceptar.Location = New Point(132, 139)
                btnAceptar.Location = New Point(191, 10)

                Me.Height = 260

                btnCancelar.Visible = False

            Case TipoDialogo.AvisoGrande

                LblTipo.Text = "{Información}"

                btnAceptar.Visible = True
                'btnAceptar.Location = New Point(132, 139)
                btnAceptar.Location = New Point(191, 10)

                Me.Height = 500

                btnCancelar.Visible = False

            Case TipoDialogo.CajaChicaEntrada

                btnAceptar.Visible = True
                'btnAceptar.Location = New Point(132, 139)
                btnAceptar.Location = New Point(191, 10)

                Me.Height = 180

                LblMensaje.Multiline = False

                LblMensaje.BackColor = Color.Salmon

                LblMensaje.PasswordChar = "*"

                LblMensaje.ReadOnly = False

                btnCancelar.Visible = False

            Case TipoDialogo.CajaGrandeEntrada

                LblTipo.Text = "{Ingresar datos}" 'mensaje_

                LblTipo.AutoSize = True

                Me.Height = 180

                LblMensaje.Multiline = True

                LblMensaje.BackColor = Color.LightYellow

                LblMensaje.BorderStyle = BorderStyle.FixedSingle

                LblMensaje.Text = Nothing

                LblMensaje.Height = 65

                Dim miLabel_ As New Label()

                miLabel_.Text = mensaje_

                miLabel_.Location = New Point(68, 43)

                miLabel_.AutoSize = True

                Me.Controls.Add(miLabel_)

                LblMensaje.Location = New Point(68, 88)

                lblMensajeRE.Location = New Point(56, 88)

                LblMensaje.ReadOnly = False

            Case Else
                'btnAceptar.Location = New Point(72, 139)
                btnAceptar.Location = New Point(91, 10)


                btnAceptar.Visible = True
                btnCancelar.Visible = True

        End Select

        Me.ShowDialog()

        Return _contestacion

    End Function


    Private Sub BtnSalir_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        _contestacion = Contestacion.Cancelar
        Me.Close()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        _contestacion = Contestacion.Cancelar
        Me.Close()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        _contestacion = Contestacion.Ok

        _respuestaSimple = LblMensaje.Text

        Me.Close()
    End Sub
#End Region

    Private Sub GsDialogo_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyData
            Case Keys.Escape
                Me.Close()
            Case Keys.Enter
                btnAceptar.PerformClick()
        End Select
    End Sub



End Class


