<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MultiSelectorControl
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
        Me.MultiSelectorInput = New System.Windows.Forms.TextBox()
        Me.MultiSelectorDropDown = New System.Windows.Forms.CheckedListBox()
        Me.MultiSelectorButton = New System.Windows.Forms.Button()
        Me.MultiSelectorFinder = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'MultiSelectorInput
        '
        Me.MultiSelectorInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MultiSelectorInput.Location = New System.Drawing.Point(1, 0)
        Me.MultiSelectorInput.Margin = New System.Windows.Forms.Padding(0)
        Me.MultiSelectorInput.Name = "MultiSelectorInput"
        Me.MultiSelectorInput.ReadOnly = True
        Me.MultiSelectorInput.Size = New System.Drawing.Size(180, 20)
        Me.MultiSelectorInput.TabIndex = 0
        '
        'MultiSelectorDropDown
        '
        Me.MultiSelectorDropDown.BackColor = System.Drawing.SystemColors.Window
        Me.MultiSelectorDropDown.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.MultiSelectorDropDown.CheckOnClick = True
        Me.MultiSelectorDropDown.FormattingEnabled = True
        Me.MultiSelectorDropDown.Location = New System.Drawing.Point(1, 20)
        Me.MultiSelectorDropDown.Margin = New System.Windows.Forms.Padding(0)
        Me.MultiSelectorDropDown.Name = "MultiSelectorDropDown"
        Me.MultiSelectorDropDown.Size = New System.Drawing.Size(200, 90)
        Me.MultiSelectorDropDown.TabIndex = 1
        '
        'MultiSelectorButton
        '
        Me.MultiSelectorButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MultiSelectorButton.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.MultiSelectorButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.MultiSelectorButton.FlatAppearance.BorderSize = 0
        Me.MultiSelectorButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.MultiSelectorButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.MultiSelectorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.MultiSelectorButton.ForeColor = System.Drawing.SystemColors.Window
        Me.MultiSelectorButton.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.expand_more
        Me.MultiSelectorButton.Location = New System.Drawing.Point(181, 0)
        Me.MultiSelectorButton.Margin = New System.Windows.Forms.Padding(0)
        Me.MultiSelectorButton.Name = "MultiSelectorButton"
        Me.MultiSelectorButton.Size = New System.Drawing.Size(20, 20)
        Me.MultiSelectorButton.TabIndex = 2
        Me.MultiSelectorButton.UseVisualStyleBackColor = False
        '
        'MultiSelectorFinder
        '
        Me.MultiSelectorFinder.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.MultiSelectorFinder.ForeColor = System.Drawing.SystemColors.GrayText
        Me.MultiSelectorFinder.Location = New System.Drawing.Point(1, 114)
        Me.MultiSelectorFinder.Margin = New System.Windows.Forms.Padding(0)
        Me.MultiSelectorFinder.Name = "MultiSelectorFinder"
        Me.MultiSelectorFinder.Size = New System.Drawing.Size(200, 13)
        Me.MultiSelectorFinder.TabIndex = 3
        Me.MultiSelectorFinder.Text = "Buscar"
        '
        'MultiSelectorControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.MultiSelectorFinder)
        Me.Controls.Add(Me.MultiSelectorButton)
        Me.Controls.Add(Me.MultiSelectorDropDown)
        Me.Controls.Add(Me.MultiSelectorInput)
        Me.Name = "MultiSelectorControl"
        Me.Size = New System.Drawing.Size(202, 131)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MultiSelectorInput As System.Windows.Forms.TextBox
    Friend WithEvents MultiSelectorDropDown As System.Windows.Forms.CheckedListBox
    Friend WithEvents MultiSelectorButton As System.Windows.Forms.Button
    Friend WithEvents MultiSelectorFinder As System.Windows.Forms.TextBox

End Class
