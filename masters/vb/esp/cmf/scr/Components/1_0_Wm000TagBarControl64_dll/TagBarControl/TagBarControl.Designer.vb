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
        Me.TagBarControlContainer = New System.Windows.Forms.GroupBox()
        Me.TagBarControlScrollView = New System.Windows.Forms.Panel()
        Me.TagBarControlButton = New System.Windows.Forms.Button()
        Me.TagBarControlContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'TagBarControlContainer
        '
        Me.TagBarControlContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TagBarControlContainer.BackColor = System.Drawing.Color.White
        Me.TagBarControlContainer.Controls.Add(Me.TagBarControlScrollView)
        Me.TagBarControlContainer.Location = New System.Drawing.Point(3, 3)
        Me.TagBarControlContainer.Name = "TagBarControlContainer"
        Me.TagBarControlContainer.Size = New System.Drawing.Size(230, 50)
        Me.TagBarControlContainer.TabIndex = 0
        Me.TagBarControlContainer.TabStop = False
        '
        'TagBarControlScrollView
        '
        Me.TagBarControlScrollView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TagBarControlScrollView.BackColor = System.Drawing.Color.Transparent
        Me.TagBarControlScrollView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.TagBarControlScrollView.Location = New System.Drawing.Point(3, 16)
        Me.TagBarControlScrollView.Margin = New System.Windows.Forms.Padding(0)
        Me.TagBarControlScrollView.Name = "TagBarControlScrollView"
        Me.TagBarControlScrollView.Size = New System.Drawing.Size(224, 30)
        Me.TagBarControlScrollView.TabIndex = 0
        '
        'TagBarControlButton
        '
        Me.TagBarControlButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TagBarControlButton.BackColor = System.Drawing.Color.Transparent
        Me.TagBarControlButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.TagBarControlButton.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.TagBarControlButton.FlatAppearance.BorderSize = 0
        Me.TagBarControlButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.TagBarControlButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.TagBarControlButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.TagBarControlButton.ForeColor = System.Drawing.Color.Transparent
        Me.TagBarControlButton.Image = Global.gsol.Componentes.SistemaBase.My.Resources.Resources.find
        Me.TagBarControlButton.Location = New System.Drawing.Point(239, 3)
        Me.TagBarControlButton.Name = "TagBarControlButton"
        Me.TagBarControlButton.Size = New System.Drawing.Size(32, 32)
        Me.TagBarControlButton.TabIndex = 1
        Me.TagBarControlButton.UseVisualStyleBackColor = False
        '
        'TagBarControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.TagBarControlButton)
        Me.Controls.Add(Me.TagBarControlContainer)
        Me.Name = "TagBarControl"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.Size = New System.Drawing.Size(274, 54)
        Me.TagBarControlContainer.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TagBarControlContainer As System.Windows.Forms.GroupBox
    Friend WithEvents TagBarControlButton As System.Windows.Forms.Button
    Friend WithEvents TagBarControlScrollView As System.Windows.Forms.Panel

End Class
