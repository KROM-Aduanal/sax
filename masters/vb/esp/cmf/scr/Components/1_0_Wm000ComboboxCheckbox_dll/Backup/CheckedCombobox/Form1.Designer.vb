<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.ListBox2 = New System.Windows.Forms.ListBox
        Me.CheckedCombobox1 = New WindowsApplication1.CheckedCombobox
        Me.SuspendLayout()
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(139, 12)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(133, 95)
        Me.ListBox1.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(336, 113)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ListBox2
        '
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.Location = New System.Drawing.Point(278, 12)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(133, 95)
        Me.ListBox2.TabIndex = 3
        '
        'CheckedCombobox1
        '
        Me.CheckedCombobox1.CheckedComboboxItems.Add("one")
        Me.CheckedCombobox1.CheckedComboboxItems.Add("two")
        Me.CheckedCombobox1.CheckedComboboxItems.Add("three")
        Me.CheckedCombobox1.CheckedComboboxItems.Add("four")
        Me.CheckedCombobox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.CheckedCombobox1.FormattingEnabled = True
        Me.CheckedCombobox1.Location = New System.Drawing.Point(12, 12)
        Me.CheckedCombobox1.Name = "CheckedCombobox1"
        Me.CheckedCombobox1.Size = New System.Drawing.Size(121, 21)
        Me.CheckedCombobox1.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(424, 287)
        Me.Controls.Add(Me.ListBox2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.CheckedCombobox1)
        Me.Name = "Form1"
        Me.Text = "CheckedCombobox Control"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CheckedCombobox1 As WindowsApplication1.CheckedCombobox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ListBox2 As System.Windows.Forms.ListBox

End Class
