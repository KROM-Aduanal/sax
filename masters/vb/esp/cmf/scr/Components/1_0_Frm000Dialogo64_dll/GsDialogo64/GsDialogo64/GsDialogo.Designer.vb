<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GsDialogo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GsDialogo))
        Me.BtnSalir = New System.Windows.Forms.Button()
        Me.LblTipo = New System.Windows.Forms.Label()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.RectangleShape2 = New Microsoft.VisualBasic.PowerPacks.RectangleShape()
        Me.LblMensaje = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnAceptar = New System.Windows.Forms.Button()
        Me.lblMensajeRE = New System.Windows.Forms.RichTextBox()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnSalir
        '
        Me.BtnSalir.BackgroundImage = CType(resources.GetObject("BtnSalir.BackgroundImage"), System.Drawing.Image)
        Me.BtnSalir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BtnSalir.FlatAppearance.BorderSize = 0
        Me.BtnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSalir.Location = New System.Drawing.Point(450, 6)
        Me.BtnSalir.Margin = New System.Windows.Forms.Padding(0)
        Me.BtnSalir.Name = "BtnSalir"
        Me.BtnSalir.Size = New System.Drawing.Size(22, 23)
        Me.BtnSalir.TabIndex = 240
        Me.BtnSalir.UseVisualStyleBackColor = True
        Me.BtnSalir.Visible = False
        '
        'LblTipo
        '
        Me.LblTipo.AutoSize = True
        Me.LblTipo.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTipo.ForeColor = System.Drawing.Color.Silver
        Me.LblTipo.Location = New System.Drawing.Point(62, 9)
        Me.LblTipo.Name = "LblTipo"
        Me.LblTipo.Size = New System.Drawing.Size(143, 31)
        Me.LblTipo.TabIndex = 237
        Me.LblTipo.Text = "{Acciones}"
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.RectangleShape2})
        Me.ShapeContainer1.Size = New System.Drawing.Size(480, 221)
        Me.ShapeContainer1.TabIndex = 241
        Me.ShapeContainer1.TabStop = False
        '
        'RectangleShape2
        '
        Me.RectangleShape2.BackgroundImage = CType(resources.GetObject("RectangleShape2.BackgroundImage"), System.Drawing.Image)
        Me.RectangleShape2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.RectangleShape2.BorderColor = System.Drawing.Color.Transparent
        Me.RectangleShape2.Location = New System.Drawing.Point(25, 7)
        Me.RectangleShape2.Name = "RectangleShape2"
        Me.RectangleShape2.Size = New System.Drawing.Size(38, 36)
        '
        'LblMensaje
        '
        Me.LblMensaje.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LblMensaje.BackColor = System.Drawing.Color.White
        Me.LblMensaje.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.LblMensaje.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMensaje.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.LblMensaje.Location = New System.Drawing.Point(68, 43)
        Me.LblMensaje.Multiline = True
        Me.LblMensaje.Name = "LblMensaje"
        Me.LblMensaje.ReadOnly = True
        Me.LblMensaje.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.LblMensaje.Size = New System.Drawing.Size(371, 114)
        Me.LblMensaje.TabIndex = 242
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.ForestGreen
        Me.Panel1.Controls.Add(Me.btnCancelar)
        Me.Panel1.Controls.Add(Me.btnAceptar)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 162)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(480, 59)
        Me.Panel1.TabIndex = 243
        '
        'btnCancelar
        '
        Me.btnCancelar.BackColor = System.Drawing.Color.ForestGreen
        Me.btnCancelar.BackgroundImage = CType(resources.GetObject("btnCancelar.BackgroundImage"), System.Drawing.Image)
        Me.btnCancelar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnCancelar.FlatAppearance.BorderSize = 0
        Me.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancelar.ForeColor = System.Drawing.Color.White
        Me.btnCancelar.Location = New System.Drawing.Point(274, 10)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(114, 39)
        Me.btnCancelar.TabIndex = 241
        Me.btnCancelar.Text = "&Cancelar"
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
        Me.btnAceptar.Location = New System.Drawing.Point(91, 10)
        Me.btnAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(114, 39)
        Me.btnAceptar.TabIndex = 240
        Me.btnAceptar.Text = "&Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = False
        '
        'lblMensajeRE
        '
        Me.lblMensajeRE.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMensajeRE.BackColor = System.Drawing.Color.White
        Me.lblMensajeRE.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblMensajeRE.Location = New System.Drawing.Point(56, 43)
        Me.lblMensajeRE.Name = "lblMensajeRE"
        Me.lblMensajeRE.Size = New System.Drawing.Size(371, 113)
        Me.lblMensajeRE.TabIndex = 244
        Me.lblMensajeRE.Text = ""
        '
        'GsDialogo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(480, 221)
        Me.Controls.Add(Me.lblMensajeRE)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.LblMensaje)
        Me.Controls.Add(Me.BtnSalir)
        Me.Controls.Add(Me.LblTipo)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "GsDialogo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Mensaje"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnSalir As System.Windows.Forms.Button
    Friend WithEvents LblTipo As System.Windows.Forms.Label
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents RectangleShape2 As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents LblMensaje As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents lblMensajeRE As System.Windows.Forms.RichTextBox

End Class
