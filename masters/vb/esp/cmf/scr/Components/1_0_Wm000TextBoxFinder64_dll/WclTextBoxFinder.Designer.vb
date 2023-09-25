<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WclTextBoxFinder
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WclTextBoxFinder))
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.tbKey = New System.Windows.Forms.TextBox()
        Me.tbTextBox = New System.Windows.Forms.TextBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSearch
        '
        Me.btnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSearch.BackColor = System.Drawing.Color.Transparent
        Me.btnSearch.BackgroundImage = CType(resources.GetObject("btnSearch.BackgroundImage"), System.Drawing.Image)
        Me.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnSearch.FlatAppearance.BorderSize = 0
        Me.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearch.ForeColor = System.Drawing.Color.Transparent
        Me.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSearch.Location = New System.Drawing.Point(214, 2)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(0)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(17, 16)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'tbKey
        '
        Me.tbKey.BackColor = System.Drawing.Color.White
        Me.tbKey.Location = New System.Drawing.Point(2, 1)
        Me.tbKey.Name = "tbKey"
        Me.tbKey.Size = New System.Drawing.Size(44, 20)
        Me.tbKey.TabIndex = 1
        '
        'tbTextBox
        '
        Me.tbTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbTextBox.BackColor = System.Drawing.Color.WhiteSmoke
        Me.tbTextBox.Enabled = False
        Me.tbTextBox.Location = New System.Drawing.Point(47, 1)
        Me.tbTextBox.Multiline = True
        Me.tbTextBox.Name = "tbTextBox"
        Me.tbTextBox.ReadOnly = True
        Me.tbTextBox.Size = New System.Drawing.Size(164, 20)
        Me.tbTextBox.TabIndex = 3
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(68, 13)
        Me.lblTitle.TabIndex = 371
        Me.lblTitle.Text = "<Undefined>"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.tbKey)
        Me.Panel1.Controls.Add(Me.tbTextBox)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 13)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(234, 23)
        Me.Panel1.TabIndex = 372
        '
        'WclTextBoxFinder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "WclTextBoxFinder"
        Me.Size = New System.Drawing.Size(234, 36)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents btnSearch As System.Windows.Forms.Button
    Public WithEvents tbKey As System.Windows.Forms.TextBox
    Public WithEvents tbTextBox As System.Windows.Forms.TextBox
    Public WithEvents lblTitle As System.Windows.Forms.Label

End Class
