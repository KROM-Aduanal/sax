<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm000Generico
    Inherits FormularioBaseMaestroDetalle

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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
        Me.ControlPanel = New System.Windows.Forms.Panel()
        Me.lblVersionModulo = New System.Windows.Forms.Label()
        Me.ControlPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'GsCatalogo1
        '
        Me.GsCatalogo1.Size = New System.Drawing.Size(564, 231)
        '
        'LblMensaje
        '
        Me.LblMensaje.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LblMensaje.Location = New System.Drawing.Point(24, 413)
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.BackColor = System.Drawing.Color.DarkCyan
        Me.btnCancelar.FlatAppearance.BorderSize = 0
        Me.btnCancelar.Location = New System.Drawing.Point(352, 23)
        Me.btnCancelar.Size = New System.Drawing.Size(108, 37)
        '
        'btnAceptar
        '
        Me.btnAceptar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAceptar.BackColor = System.Drawing.Color.DarkCyan
        Me.btnAceptar.FlatAppearance.BorderSize = 0
        Me.btnAceptar.Location = New System.Drawing.Point(133, 23)
        Me.btnAceptar.Size = New System.Drawing.Size(110, 37)
        '
        'ControlPanel
        '
        Me.ControlPanel.BackColor = System.Drawing.Color.DarkCyan
        Me.ControlPanel.Controls.Add(Me.lblVersionModulo)
        Me.ControlPanel.Controls.Add(Me.btnAceptar)
        Me.ControlPanel.Controls.Add(Me.btnCancelar)
        Me.ControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ControlPanel.Location = New System.Drawing.Point(0, 433)
        Me.ControlPanel.Name = "ControlPanel"
        Me.ControlPanel.Size = New System.Drawing.Size(608, 83)
        Me.ControlPanel.TabIndex = 360
        Me.ControlPanel.Controls.SetChildIndex(Me.btnCancelar, 0)
        Me.ControlPanel.Controls.SetChildIndex(Me.btnAceptar, 0)
        Me.ControlPanel.Controls.SetChildIndex(Me.lblVersionModulo, 0)
        '
        'lblVersionModulo
        '
        Me.lblVersionModulo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblVersionModulo.AutoSize = True
        Me.lblVersionModulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersionModulo.ForeColor = System.Drawing.Color.White
        Me.lblVersionModulo.Location = New System.Drawing.Point(545, 61)
        Me.lblVersionModulo.Name = "lblVersionModulo"
        Me.lblVersionModulo.Size = New System.Drawing.Size(58, 13)
        Me.lblVersionModulo.TabIndex = 357
        Me.lblVersionModulo.Text = "v.0.0.0.0"
        '
        'Frm000Generico
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(608, 516)
        Me.Controls.Add(Me.ControlPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "Frm000Generico"
        Me.Controls.SetChildIndex(Me.ControlPanel, 0)
        Me.Controls.SetChildIndex(Me.LblAccion, 0)
        Me.Controls.SetChildIndex(Me.Label6, 0)
        Me.Controls.SetChildIndex(Me.LblMensaje, 0)
        Me.Controls.SetChildIndex(Me.GsCatalogo1, 0)
        Me.ControlPanel.ResumeLayout(False)
        Me.ControlPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ControlPanel As System.Windows.Forms.Panel
    Friend WithEvents lblVersionModulo As System.Windows.Forms.Label

End Class
