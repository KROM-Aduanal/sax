<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WclPoll
    Inherits System.Windows.Forms.UserControl

    'UserControl1 reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pnlEncabezado = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblObligatoria = New System.Windows.Forms.Label()
        Me.pnlCuerpo = New System.Windows.Forms.Panel()
        Me.pnlInformacion = New System.Windows.Forms.Panel()
        Me.lblComentarios = New System.Windows.Forms.Label()
        Me.pnlSiguientePregunta = New System.Windows.Forms.Panel()
        Me.pnlEncabezado.SuspendLayout()
        Me.pnlInformacion.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlEncabezado
        '
        Me.pnlEncabezado.BackColor = System.Drawing.Color.White
        Me.pnlEncabezado.Controls.Add(Me.lblTitulo)
        Me.pnlEncabezado.Controls.Add(Me.lblObligatoria)
        Me.pnlEncabezado.Location = New System.Drawing.Point(7, 7)
        Me.pnlEncabezado.Margin = New System.Windows.Forms.Padding(7)
        Me.pnlEncabezado.Name = "pnlEncabezado"
        Me.pnlEncabezado.Size = New System.Drawing.Size(798, 54)
        Me.pnlEncabezado.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitulo.ForeColor = System.Drawing.Color.Teal
        Me.lblTitulo.Location = New System.Drawing.Point(40, 7)
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(7, 0, 7, 0)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(751, 45)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "Coloque su pregunta aquí..."
        '
        'lblObligatoria
        '
        Me.lblObligatoria.AutoSize = True
        Me.lblObligatoria.BackColor = System.Drawing.Color.Transparent
        Me.lblObligatoria.Font = New System.Drawing.Font("Segoe UI", 15.0!)
        Me.lblObligatoria.ForeColor = System.Drawing.Color.LightCoral
        Me.lblObligatoria.Location = New System.Drawing.Point(7, 0)
        Me.lblObligatoria.Margin = New System.Windows.Forms.Padding(7, 0, 7, 0)
        Me.lblObligatoria.Name = "lblObligatoria"
        Me.lblObligatoria.Size = New System.Drawing.Size(46, 61)
        Me.lblObligatoria.TabIndex = 1
        Me.lblObligatoria.Text = "*"
        '
        'pnlCuerpo
        '
        Me.pnlCuerpo.BackColor = System.Drawing.Color.White
        Me.pnlCuerpo.Location = New System.Drawing.Point(53, 65)
        Me.pnlCuerpo.Margin = New System.Windows.Forms.Padding(7)
        Me.pnlCuerpo.Name = "pnlCuerpo"
        Me.pnlCuerpo.Size = New System.Drawing.Size(751, 89)
        Me.pnlCuerpo.TabIndex = 1
        '
        'pnlInformacion
        '
        Me.pnlInformacion.BackColor = System.Drawing.Color.White
        Me.pnlInformacion.Controls.Add(Me.lblComentarios)
        Me.pnlInformacion.Location = New System.Drawing.Point(7, 155)
        Me.pnlInformacion.Margin = New System.Windows.Forms.Padding(7)
        Me.pnlInformacion.Name = "pnlInformacion"
        Me.pnlInformacion.Size = New System.Drawing.Size(798, 34)
        Me.pnlInformacion.TabIndex = 2
        '
        'lblComentarios
        '
        Me.lblComentarios.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComentarios.ForeColor = System.Drawing.Color.Coral
        Me.lblComentarios.Location = New System.Drawing.Point(38, 1)
        Me.lblComentarios.Margin = New System.Windows.Forms.Padding(7, 0, 7, 0)
        Me.lblComentarios.Name = "lblComentarios"
        Me.lblComentarios.Size = New System.Drawing.Size(735, 34)
        Me.lblComentarios.TabIndex = 1
        Me.lblComentarios.Text = "Confirmación de respuesta"
        Me.lblComentarios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlSiguientePregunta
        '
        Me.pnlSiguientePregunta.Location = New System.Drawing.Point(143, 188)
        Me.pnlSiguientePregunta.Name = "pnlSiguientePregunta"
        Me.pnlSiguientePregunta.Size = New System.Drawing.Size(769, 17)
        Me.pnlSiguientePregunta.TabIndex = 3
        '
        'WclPoll
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(14.0!, 29.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.pnlSiguientePregunta)
        Me.Controls.Add(Me.pnlInformacion)
        Me.Controls.Add(Me.pnlCuerpo)
        Me.Controls.Add(Me.pnlEncabezado)
        Me.Margin = New System.Windows.Forms.Padding(7)
        Me.Name = "WclPoll"
        Me.Size = New System.Drawing.Size(912, 207)
        Me.pnlEncabezado.ResumeLayout(False)
        Me.pnlEncabezado.PerformLayout()
        Me.pnlInformacion.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents pnlEncabezado As System.Windows.Forms.Panel
    Public WithEvents lblTitulo As System.Windows.Forms.Label
    Public WithEvents pnlCuerpo As System.Windows.Forms.Panel
    Public WithEvents pnlInformacion As System.Windows.Forms.Panel
    Public WithEvents lblComentarios As System.Windows.Forms.Label
    Public WithEvents lblObligatoria As System.Windows.Forms.Label
    Friend WithEvents pnlSiguientePregunta As System.Windows.Forms.Panel

End Class
