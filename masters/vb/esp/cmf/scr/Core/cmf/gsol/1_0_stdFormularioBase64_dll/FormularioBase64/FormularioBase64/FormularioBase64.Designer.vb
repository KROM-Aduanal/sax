<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormularioBase64
    Inherits System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormularioBase64))
        Me.LblMensaje = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnAceptar = New System.Windows.Forms.Button()
        Me.LblAccion = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'LblMensaje
        '
        Me.LblMensaje.AutoSize = True
        Me.LblMensaje.ForeColor = System.Drawing.Color.Maroon
        Me.LblMensaje.Location = New System.Drawing.Point(55, 401)
        Me.LblMensaje.Name = "LblMensaje"
        Me.LblMensaje.Size = New System.Drawing.Size(10, 13)
        Me.LblMensaje.TabIndex = 358
        Me.LblMensaje.Text = "-"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.DimGray
        Me.Label6.Location = New System.Drawing.Point(21, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(120, 31)
        Me.Label6.TabIndex = 357
        Me.Label6.Text = "{Módulo}"
        '
        'btnCancelar
        '
        Me.btnCancelar.BackColor = System.Drawing.Color.ForestGreen
        Me.btnCancelar.BackgroundImage = CType(resources.GetObject("btnCancelar.BackgroundImage"), System.Drawing.Image)
        Me.btnCancelar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnCancelar.FlatAppearance.BorderSize = 0
        Me.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancelar.ForeColor = System.Drawing.Color.White
        Me.btnCancelar.Location = New System.Drawing.Point(303, 440)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(110, 37)
        Me.btnCancelar.TabIndex = 356
        Me.btnCancelar.Text = "&Cerrar"
        Me.btnCancelar.UseVisualStyleBackColor = False
        '
        'btnAceptar
        '
        Me.btnAceptar.BackColor = System.Drawing.Color.ForestGreen
        Me.btnAceptar.BackgroundImage = CType(resources.GetObject("btnAceptar.BackgroundImage"), System.Drawing.Image)
        Me.btnAceptar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAceptar.FlatAppearance.BorderSize = 0
        Me.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAceptar.ForeColor = System.Drawing.Color.Transparent
        Me.btnAceptar.Location = New System.Drawing.Point(115, 440)
        Me.btnAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(112, 37)
        Me.btnAceptar.TabIndex = 355
        Me.btnAceptar.Text = "&Guardar"
        Me.btnAceptar.UseVisualStyleBackColor = False
        '
        'LblAccion
        '
        Me.LblAccion.AutoSize = True
        Me.LblAccion.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAccion.ForeColor = System.Drawing.Color.DarkGray
        Me.LblAccion.Location = New System.Drawing.Point(22, 41)
        Me.LblAccion.Name = "LblAccion"
        Me.LblAccion.Size = New System.Drawing.Size(166, 29)
        Me.LblAccion.TabIndex = 354
        Me.LblAccion.Text = "nuevo registro"
        '
        'FormularioBase64
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(531, 493)
        Me.Controls.Add(Me.LblMensaje)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.LblAccion)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "FormularioBase64"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "{Sin Definir}"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents LblMensaje As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents btnCancelar As System.Windows.Forms.Button
    Public WithEvents btnAceptar As System.Windows.Forms.Button
    Public WithEvents LblAccion As System.Windows.Forms.Label
End Class
