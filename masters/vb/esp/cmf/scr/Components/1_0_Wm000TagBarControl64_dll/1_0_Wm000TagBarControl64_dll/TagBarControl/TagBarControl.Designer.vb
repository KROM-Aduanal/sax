<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TagBarControl
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
        Me.GroupBoxContainer = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBoxButton = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBoxContainer
        '
        Me.GroupBoxContainer.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBoxContainer.Location = New System.Drawing.Point(4, 4)
        Me.GroupBoxContainer.Name = "GroupBoxContainer"
        Me.GroupBoxContainer.Size = New System.Drawing.Size(508, 52)
        Me.GroupBoxContainer.TabIndex = 0
        Me.GroupBoxContainer.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GroupBoxButton)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel1.Location = New System.Drawing.Point(514, 4)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(32, 52)
        Me.Panel1.TabIndex = 1
        '
        'GroupBoxButton
        '
        Me.GroupBoxButton.BackColor = System.Drawing.Color.Transparent
        Me.GroupBoxButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.GroupBoxButton.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBoxButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.GroupBoxButton.FlatAppearance.BorderSize = 0
        Me.GroupBoxButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.GroupBoxButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.GroupBoxButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBoxButton.ForeColor = System.Drawing.Color.Transparent
        Me.GroupBoxButton.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.find
        Me.GroupBoxButton.Location = New System.Drawing.Point(0, 0)
        Me.GroupBoxButton.Name = "GroupBoxButton"
        Me.GroupBoxButton.Size = New System.Drawing.Size(32, 52)
        Me.GroupBoxButton.TabIndex = 1
        Me.GroupBoxButton.UseVisualStyleBackColor = False
        '
        'TagBarControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.GroupBoxContainer)
        Me.Name = "TagBarControl"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.Size = New System.Drawing.Size(550, 60)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBoxContainer As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBoxButton As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel

End Class
