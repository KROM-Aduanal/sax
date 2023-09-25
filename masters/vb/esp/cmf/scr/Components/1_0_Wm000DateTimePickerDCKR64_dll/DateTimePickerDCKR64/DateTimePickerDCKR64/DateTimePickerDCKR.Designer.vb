<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DateTimePickerDCKR
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
        Me.dtpDate1 = New System.Windows.Forms.DateTimePicker()
        Me.dtpDate2 = New System.Windows.Forms.DateTimePicker()
        Me.chkIncluding = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.cbCriteria = New System.Windows.Forms.ComboBox()
        Me.lblSeparator = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dtpDate1
        '
        Me.dtpDate1.Enabled = False
        Me.dtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDate1.Location = New System.Drawing.Point(95, 26)
        Me.dtpDate1.Name = "dtpDate1"
        Me.dtpDate1.Size = New System.Drawing.Size(182, 20)
        Me.dtpDate1.TabIndex = 0
        '
        'dtpDate2
        '
        Me.dtpDate2.Enabled = False
        Me.dtpDate2.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDate2.Location = New System.Drawing.Point(198, 26)
        Me.dtpDate2.Name = "dtpDate2"
        Me.dtpDate2.Size = New System.Drawing.Size(80, 20)
        Me.dtpDate2.TabIndex = 1
        Me.dtpDate2.Visible = False
        '
        'chkIncluding
        '
        Me.chkIncluding.AutoSize = True
        Me.chkIncluding.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncluding.Location = New System.Drawing.Point(283, 29)
        Me.chkIncluding.Name = "chkIncluding"
        Me.chkIncluding.Size = New System.Drawing.Size(54, 17)
        Me.chkIncluding.TabIndex = 2
        Me.chkIncluding.Text = "Incluir"
        Me.chkIncluding.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.Controls.Add(Me.lblTitle)
        Me.Panel1.Controls.Add(Me.cbCriteria)
        Me.Panel1.Controls.Add(Me.lblSeparator)
        Me.Panel1.Controls.Add(Me.dtpDate1)
        Me.Panel1.Controls.Add(Me.chkIncluding)
        Me.Panel1.Controls.Add(Me.dtpDate2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.ForeColor = System.Drawing.Color.Black
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(348, 55)
        Me.Panel1.TabIndex = 3
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(8, 7)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(10, 13)
        Me.lblTitle.TabIndex = 5
        Me.lblTitle.Text = "."
        '
        'cbCriteria
        '
        Me.cbCriteria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCriteria.Enabled = False
        Me.cbCriteria.FormattingEnabled = True
        Me.cbCriteria.Location = New System.Drawing.Point(9, 25)
        Me.cbCriteria.Name = "cbCriteria"
        Me.cbCriteria.Size = New System.Drawing.Size(79, 21)
        Me.cbCriteria.TabIndex = 4
        '
        'lblSeparator
        '
        Me.lblSeparator.AutoSize = True
        Me.lblSeparator.Location = New System.Drawing.Point(181, 28)
        Me.lblSeparator.Name = "lblSeparator"
        Me.lblSeparator.Size = New System.Drawing.Size(12, 13)
        Me.lblSeparator.TabIndex = 3
        Me.lblSeparator.Text = "y"
        Me.lblSeparator.Visible = False
        '
        'DateTimePickerDCKR
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Panel1)
        Me.Name = "DateTimePickerDCKR"
        Me.Size = New System.Drawing.Size(348, 55)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dtpDate1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpDate2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents chkIncluding As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblSeparator As System.Windows.Forms.Label
    Friend WithEvents cbCriteria As System.Windows.Forms.ComboBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label

End Class
