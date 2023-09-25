<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TextBoxDCKR
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rbtnNo = New System.Windows.Forms.RadioButton()
        Me.rbtnSi = New System.Windows.Forms.RadioButton()
        Me.lblSeparator = New System.Windows.Forms.Label()
        Me.cbCriteria = New System.Windows.Forms.ComboBox()
        Me.chkIncluding = New System.Windows.Forms.CheckBox()
        Me.mtbValue2 = New System.Windows.Forms.MaskedTextBox()
        Me.mtbValue1 = New System.Windows.Forms.MaskedTextBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.Controls.Add(Me.lblTitle)
        Me.Panel1.Controls.Add(Me.rbtnNo)
        Me.Panel1.Controls.Add(Me.rbtnSi)
        Me.Panel1.Controls.Add(Me.lblSeparator)
        Me.Panel1.Controls.Add(Me.cbCriteria)
        Me.Panel1.Controls.Add(Me.chkIncluding)
        Me.Panel1.Controls.Add(Me.mtbValue2)
        Me.Panel1.Controls.Add(Me.mtbValue1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(348, 55)
        Me.Panel1.TabIndex = 0
        '
        'rbtnNo
        '
        Me.rbtnNo.AutoSize = True
        Me.rbtnNo.Enabled = False
        Me.rbtnNo.Location = New System.Drawing.Point(185, 26)
        Me.rbtnNo.Name = "rbtnNo"
        Me.rbtnNo.Size = New System.Drawing.Size(39, 17)
        Me.rbtnNo.TabIndex = 9
        Me.rbtnNo.TabStop = True
        Me.rbtnNo.Text = "No"
        Me.rbtnNo.UseVisualStyleBackColor = True
        Me.rbtnNo.Visible = False
        '
        'rbtnSi
        '
        Me.rbtnSi.AutoSize = True
        Me.rbtnSi.Enabled = False
        Me.rbtnSi.Location = New System.Drawing.Point(117, 27)
        Me.rbtnSi.Name = "rbtnSi"
        Me.rbtnSi.Size = New System.Drawing.Size(36, 17)
        Me.rbtnSi.TabIndex = 8
        Me.rbtnSi.TabStop = True
        Me.rbtnSi.Text = "Sí"
        Me.rbtnSi.UseVisualStyleBackColor = True
        Me.rbtnSi.Visible = False
        '
        'lblSeparator
        '
        Me.lblSeparator.AutoSize = True
        Me.lblSeparator.Enabled = False
        Me.lblSeparator.Location = New System.Drawing.Point(177, 29)
        Me.lblSeparator.Name = "lblSeparator"
        Me.lblSeparator.Size = New System.Drawing.Size(12, 13)
        Me.lblSeparator.TabIndex = 7
        Me.lblSeparator.Text = "y"
        Me.lblSeparator.Visible = False
        '
        'cbCriteria
        '
        Me.cbCriteria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCriteria.Enabled = False
        Me.cbCriteria.FormattingEnabled = True
        Me.cbCriteria.Items.AddRange(New Object() {"Es igual a...", "Es mayor que...", "Es menor que...", "Es diferente de...", "Valor entre..."})
        Me.cbCriteria.Location = New System.Drawing.Point(9, 25)
        Me.cbCriteria.Name = "cbCriteria"
        Me.cbCriteria.Size = New System.Drawing.Size(79, 21)
        Me.cbCriteria.TabIndex = 6
        '
        'chkIncluding
        '
        Me.chkIncluding.AutoSize = True
        Me.chkIncluding.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncluding.Location = New System.Drawing.Point(283, 29)
        Me.chkIncluding.Name = "chkIncluding"
        Me.chkIncluding.Size = New System.Drawing.Size(54, 17)
        Me.chkIncluding.TabIndex = 5
        Me.chkIncluding.Text = "Incluir"
        Me.chkIncluding.UseVisualStyleBackColor = True
        '
        'mtbValue2
        '
        Me.mtbValue2.Enabled = False
        Me.mtbValue2.Location = New System.Drawing.Point(197, 25)
        Me.mtbValue2.Mask = "99999"
        Me.mtbValue2.Name = "mtbValue2"
        Me.mtbValue2.Size = New System.Drawing.Size(69, 20)
        Me.mtbValue2.TabIndex = 1
        Me.mtbValue2.ValidatingType = GetType(Integer)
        Me.mtbValue2.Visible = False
        '
        'mtbValue1
        '
        Me.mtbValue1.Enabled = False
        Me.mtbValue1.Location = New System.Drawing.Point(95, 25)
        Me.mtbValue1.Mask = "99999"
        Me.mtbValue1.Name = "mtbValue1"
        Me.mtbValue1.Size = New System.Drawing.Size(171, 20)
        Me.mtbValue1.TabIndex = 0
        Me.mtbValue1.ValidatingType = GetType(Integer)
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(7, 7)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(10, 13)
        Me.lblTitle.TabIndex = 10
        Me.lblTitle.Text = "."
        '
        'TextBoxDCKR
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Panel1)
        Me.Name = "TextBoxDCKR"
        Me.Size = New System.Drawing.Size(348, 55)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblSeparator As System.Windows.Forms.Label
    Friend WithEvents cbCriteria As System.Windows.Forms.ComboBox
    Friend WithEvents chkIncluding As System.Windows.Forms.CheckBox
    Friend WithEvents mtbValue2 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents mtbValue1 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents rbtnNo As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnSi As System.Windows.Forms.RadioButton
    Friend WithEvents lblTitle As System.Windows.Forms.Label

End Class
