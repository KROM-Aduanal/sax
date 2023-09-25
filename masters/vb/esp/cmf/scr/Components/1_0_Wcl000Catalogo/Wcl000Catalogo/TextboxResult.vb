
Option Explicit On

Imports gsol.BaseDatos.Operaciones

Public Class TextboxResult

    ' función PostMessage para cancelar el DropDown
    Private Const CB_SHOWDROPDOWN As Integer = &H14F
    Private Declare Function PostMessage _
        Lib "user32" _
        Alias "PostMessageA" ( _
            ByVal hwnd As IntPtr, _
            ByVal wMsg As Integer, _
            ByVal wParam As Integer, _
            ByVal lParam As Integer) As Integer

    ' Variables para los controles
    Private WithEvents m_TextBox As TextBox
    Private WithEvents m_DataGridView As DataGridView
    Private WithEvents m_frm_Container As Form
    Private WithEvents m_ButtonClose As Button

    ' variables
    Private m_Ancho_Lista As Integer = 500
    Private m_Alto_Lista As Integer = 130
    Private m_Columna As Integer = 0
    Private mShowToolTip As Boolean = False
    Private mShowDialog As Boolean = False

    Public ValorIndice As String

    Private iCaracteristicas_ As Dictionary(Of Integer, ICaracteristica)

    Private tooltip As ToolTip

    ' eventos
    Public Event onButtonClose()
    Public Event onCloseList()
    Public Event onOpenList()


    Public Property Caracteristicas As Dictionary(Of Integer, ICaracteristica)
        Get
            Return iCaracteristicas_

        End Get

        Set(value As Dictionary(Of Integer, ICaracteristica))
            iCaracteristicas_ = value
        End Set

    End Property

    ' Propiedades
    '
    ''' <summary>
    ''' Establece la Columna del DataGridView a mostrar en el combobox
    ''' </summary>
    Public Property ColumnaDefault() As Integer

        Get
            Return m_Columna
        End Get

        Set(ByVal value As Integer)
            m_Columna = value
        End Set

    End Property

    ''' <summary>
    ''' Establecer el alto de la lista desplegable
    ''' </summary>
    Public Property AltoLista() As Integer
        Get
            Return m_Alto_Lista
        End Get
        Set(ByVal value As Integer)
            If value < 100 Then
                m_Alto_Lista = 200
            Else
                m_Alto_Lista = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Establecer el Ancho de la lista desplegable
    ''' </summary>
    ''' 
    Public Property AnchoLista() As Integer
        Get
            Return m_Ancho_Lista
        End Get
        Set(ByVal value As Integer)
            ' si el ancho es menor al width del combo ...
            If value <= m_TextBox.Width Then
                '.. asignar el mismo ancho
                m_Ancho_Lista = m_TextBox.Width + 10
            Else
                m_Ancho_Lista = value
            End If

        End Set
    End Property
    ''' <summary>
    ''' ' Mostrar o no mostrar el objeto ToolTip para el ComboBox
    ''' </summary>

    Public Property ShowToolTip() As Boolean
        Get
            Return mShowToolTip
        End Get
        Set(ByVal value As Boolean)
            mShowToolTip = value
        End Set
    End Property

    ''' <summary>
    ''' ' Mostrar la lista desplegable como Modal o Normal
    ''' </summary>
    Public Property ShowDialog() As Boolean
        Get
            Return mShowDialog
        End Get
        Set(ByVal value As Boolean)
            mShowDialog = value
        End Set
    End Property

    ''' <summary>
    ''' ' ' Iniciar la instancia
    ''' </summary>

    Sub Iniciar( _
        ByVal textBox_ As TextBox, _
        ByVal DataGridView As DataGridView)

        If Not (m_TextBox Is Nothing) Or _
        Not (m_DataGridView) Is Nothing Then
            MsgBox("Ya se han indicado los controles")
            Exit Sub
        End If

        ' Configurar el combobox
        m_TextBox = textBox_
        With m_TextBox
            '.DropDownHeight = 1
            '.DropDownWidth = 1
            '.DropDownStyle = ComboBoxStyle.DropDownList
            '.Items.Clear()
            .Clear()

        End With

        ' Configurar el formulario contenedor
        m_frm_Container = New Form
        With m_frm_Container
            .FormBorderStyle = FormBorderStyle.None ' form sin borde
            .ShowInTaskbar = False ' Ocultarlo del Taskbar
            .Visible = False
        End With

        ' Configurar el dataGridView
        m_DataGridView = DataGridView
        With m_DataGridView
            .Parent = m_frm_Container ' asignar el contenedor del DataGridView
            .Visible = True
            ' Para seleccionar la fila completa
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End With

        ' botón para cerrar la lista
        m_ButtonClose = New Button
        With m_ButtonClose
            ' Asignar el formulario que lo contiene
            .Parent = m_frm_Container
        End With

        'iCaracteristicas_ = New Dictionary(Of Integer, ICaracteristica)


    End Sub

    'Private Sub m_TextBox_GotFocus( _
    '    ByVal sender As Object, _
    '    ByVal e As System.EventArgs) Handles m_TextBox.GotFocus
    '    With m_TextBox
    '        .BackColor = Color.White
    '        .ForeColor = Color.Black

    '    End With
    'End Sub

    ' Abir  la lista desplegable al presionar Enter
    'Private Sub m_TextBox_KeyDown( _
    '    ByVal sender As Object, _
    '    ByVal e As System.Windows.Forms.KeyEventArgs) Handles m_TextBox.KeyDown
    '    If (e.KeyCode = Keys.Enter) And _
    '       (m_TextBox.Focused) And _
    '       (m_frm_Container.Visible = False) Then

    '        showList()
    '    End If
    'End Sub



    ' Cancelar el DropDown con PostMessage, y mostrar o cerrar la lista
    Private Sub m_TextBox_MouseDown( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.MouseEventArgs) _
            Handles m_TextBox.MouseDown

        PostMessage(m_TextBox.Handle, CB_SHOWDROPDOWN, 0, 0)

        showList()
    End Sub

    ''' <summary>
    ''' Evento para cerrar la lista desplegable
    ''' </summary>
    Sub closeList()
        ' Establecer los datos para la info sobre herramientas
        MostrarToolTip()
        RaiseEvent onCloseList() ' Lanzar evento
        ' ocultar el formulario
        If m_frm_Container.Visible Then
            m_frm_Container.Visible = False

            Dim indice_ As String

            Dim campoBusquedaPrincipal_ As String = ObtieneNombreTecnicoCampoLlave()

            If Not campoBusquedaPrincipal_ Is Nothing Then

                indice_ = Me.m_DataGridView.CurrentRow.Cells.Item(campoBusquedaPrincipal_).Value.ToString()

            Else

                'MsgBox("No está definido el campo de búsqueda principal, reporte a informática por favor")

            End If

            'MsgBox("Referencia:" & indice_)

            m_TextBox.Text = indice_

            ValorIndice = indice_

            With m_TextBox
                .Focus()
            End With
        End If
    End Sub

    Private Function ObtieneNombreTecnicoCampoLlave() As String

        For Each parDatos_ As KeyValuePair(Of Int32, ICaracteristica) In iCaracteristicas_

            If parDatos_.Value.TipoFiltro = ICaracteristica.TiposFiltro.PorDefecto Then

                'ca = parDatos_.Value

                Return parDatos_.Value.Nombre

                Exit For

            End If

        Next

        Return Nothing

    End Function

    ' al desactivarse el formulario que hace de lista, ocultarlo 
    Private Sub m_frm_Container_Deactivate( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles m_frm_Container.Deactivate
        If m_frm_Container.Visible Then m_frm_Container.Visible = False

    End Sub

    ''' <summary>
    ''' Evento para Abrir la lista desplegable
    ''' </summary>
    Public Sub showList()

        ' si el form de lista está visible sale
        If m_frm_Container.Visible Then Exit Sub

        ' obtener la posición Left y Top del combo en la pantalla
        Dim Posicion As System.Drawing.Point
        Dim point_CBox As Point = m_TextBox.PointToScreen(Posicion)
        Dim new_point_combo As Point = m_frm_Container.PointToClient(point_CBox)

        ' posicionar el formulario en la posición del combo
        With m_frm_Container
            .SetBounds( _
                      (point_CBox.X), _
                      (point_CBox.Y + m_TextBox.Height), _
                      (m_Ancho_Lista), _
                      (m_Alto_Lista + 25))

            .BringToFront() ' traerlo al frente
        End With

        ' propiedaedes para el comboBox
        With m_TextBox
            .BackColor = Color.AliceBlue
            .ForeColor = Color.Black
        End With

        ' Propiedades del botón
        With m_ButtonClose

            .Text = "Seleccionar y salir"

            ' para obtener el ancho del texto del botón con MeasureString
            Dim g As Graphics = m_frm_Container.CreateGraphics
            Dim WFont As New System.Drawing.SizeF

            WFont = g.MeasureString(.Text, .Font)
            g.Dispose()

            ' asignar el tamaño y la posición del button
            .Size = New Size(CInt(WFont.Width * 1.21), 22)
            .Location = New Point(m_frm_Container.Width - (.Width + 2), _
                                 m_frm_Container.Height - (.Height) - 2)


        End With

        ' Configurar el DataGridView ( el tamaño, la posición, establecerle el foco)
        With m_DataGridView

            If .Parent Is Nothing Then
                .Parent = m_frm_Container
            End If

            .SetBounds(2, 3, (.Parent.Width - 5), (.Parent.Height - 34))
        End With
        ' lanzar evento 
        RaiseEvent onOpenList()

        ' hacer visible la lista desplegable  ( modal o normal )
        If mShowDialog Then ' modal 
            m_DataGridView.Focus()
            m_frm_Container.ShowDialog()
            m_TextBox.Focus()
        Else
            m_frm_Container.Show() ' normal
            m_DataGridView.Focus()
        End If
    End Sub

    ' lanzar evento 
    Private Sub m_button_Close_Click( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles m_ButtonClose.Click

        RaiseEvent onButtonClose()

        closeList()


    End Sub

    Private Sub m_DataGridView_CellEndEdit( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
            Handles m_DataGridView.CellEndEdit

        ' Al editar la celda actualizar el texto del Combobox
        If e.ColumnIndex = m_Columna Then
            With m_TextBox
                '.Items.Clear()
                '.Items.Add(ValueColumn)
                '.Text = ValueColumn()
                .Clear()
                .Text = ValueColumn()
            End With
        End If
    End Sub

    Private Sub m_DataGridView_KeyDown( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyEventArgs) Handles m_DataGridView.KeyDown
        'si se presiona la tecla escape, cerrar la lista
        'If (e.KeyCode = Keys.Escape) Then

        If (e.KeyCode = Keys.Enter) Then
            Dim indice_ As String = Nothing

            '            indice_ = Me.m_DataGridView.CurrentRow.Cells.Item("t_Referencia").Value.ToString()

            Dim campoBusquedaPrincipal_ As String = ObtieneNombreTecnicoCampoLlave()

            If Not campoBusquedaPrincipal_ Is Nothing Then

                indice_ = Me.m_DataGridView.CurrentRow.Cells.Item(campoBusquedaPrincipal_).Value.ToString()

            Else

                'MsgBox("No está definido el campo de búsqueda principal, reporte a informática por favor")

            End If

            m_TextBox.Text = indice_

            ValorIndice = indice_

            closeList()

        ElseIf (e.KeyCode = Keys.Escape) Then

            closeList()

        End If

    End Sub

    ' Cambiar el valor del combo al cambiar el valor seleccionado del  DataGridView
    Private Sub m_DataGridView_SelectionChanged( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs) _
            Handles m_DataGridView.SelectionChanged

        'Dim indice_ As String = Nothing

        'indice_ = Me.m_DataGridView.CurrentRow.Cells.Item("t_Referencia").Value.ToString()

        Dim indice_ As String

        Dim campoBusquedaPrincipal_ As String = ObtieneNombreTecnicoCampoLlave()

        If Not campoBusquedaPrincipal_ Is Nothing Then

            indice_ = Me.m_DataGridView.CurrentRow.Cells.Item(campoBusquedaPrincipal_).Value.ToString()

        Else

            'MsgBox("No está definido el campo de búsqueda principal, reporte a informática por favor")

        End If

        'MsgBox("Referencia:" & indice_)


        'With m_TextBox
        '    ' .Items.Clear()
        '    .Clear()

        '    Dim valor As Object = ValueColumn()

        '    If Not valor Is Nothing Then
        '        '.Items.Add(ValueColumn)

        '        .Text = ValueColumn()
        '    End If
        'End With

        'Extra

        m_TextBox.Text = indice_

        ValorIndice = indice_

    End Sub


    ''' <summary>
    ''' función que retorna el valor de la celda de la columna default
    ''' </summary>
    Function ValueColumn() As String

        If m_DataGridView Is Nothing Or _
           m_DataGridView.CurrentRow Is Nothing Then
            Return Nothing
        Else
            With m_DataGridView
                Try
                    If Not .Item( _
                            m_Columna, _
                            .CurrentRow.Index).Value Is Nothing Then

                        Return m_DataGridView.Item( _
                                             m_Columna, _
                                             .CurrentRow.Index).Value.ToString


                    End If
                Catch ex As Exception
                    MsgBox(ex.Message.ToString)
                End Try
            End With
        End If
        Return Nothing
    End Function

    ' Rutina que recorre las columnas de la fila del datagridView para _
    ' obtener el texto y asignarlo al objeto toolTip para el combobox
    Private Sub MostrarToolTip(Optional ByVal delay As Integer = 3000)

        ' si la fila actual es Nothing o mShowToolTip es Falso ..salir
        If (m_DataGridView.CurrentRow Is Nothing) Or _
               (mShowToolTip = False) Then
            Exit Sub
        End If

        Dim texto As String = String.Empty

        With m_DataGridView
            ' recorrer las columnas
            For i As Integer = 0 To .ColumnCount - 1
                Dim Header_text As String = .Columns(i).HeaderText

                ' verificar que el valor no sea Nothing
                If Not (.Item(i, .CurrentRow.Index).Value) Is Nothing Then
                    If .Item(i, .CurrentRow.Index).Value.ToString <> String.Empty Then

                        'Usar la colección Item para obtener el valor de la celda
                        Dim Celda As String = .Item(i, .CurrentRow.Index).Value.ToString
                        ' guardar el caption del header + el texto de la celda
                        texto = texto & _
                                Header_text & ": " & _
                                Celda & vbCrLf
                    End If
                End If

            Next
        End With

        If Not tooltip Is Nothing Then tooltip.Dispose()

        ' nuevo tooltip
        tooltip = New ToolTip

        ' configurarlo ( el delay, el tipo, el título y el texto anterior )
        With tooltip
            .AutomaticDelay = delay
            .IsBalloon = True
            .ToolTipIcon = ToolTipIcon.Info

            .ToolTipTitle = m_DataGridView.Item( _
                                            m_Columna, _
                                            m_DataGridView.CurrentRow.Index).Value.ToString

            .SetToolTip(m_TextBox, texto)
        End With
    End Sub

    ' al repintar el formulario de lista, dibujar un rectángulo del
    'tamaño del form con la función DrawRectangle
    Private Sub m_frm_Container_Paint( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.PaintEventArgs) Handles m_frm_Container.Paint
        'System.Drawing.SystemPens.GradientInactiveCaption()

        e.Graphics.DrawRectangle(Pens.Silver, _
                New Rectangle(0, 1, _
                    m_frm_Container.Width - 2, m_frm_Container.Height - 2))
    End Sub

    Sub Dispose()
        Finalize()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        Me.m_TextBox = Nothing
        Me.m_ButtonClose = Nothing
        Me.m_DataGridView = Nothing
        Me.m_frm_Container = Nothing
    End Sub


End Class
