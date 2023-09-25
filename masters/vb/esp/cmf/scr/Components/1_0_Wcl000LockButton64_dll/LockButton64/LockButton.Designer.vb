<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LockButton
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LockButton))
        Me.btnBloquear = New System.Windows.Forms.Button()
        Me.imageList = New System.Windows.Forms.ImageList(Me.components)
        Me.SuspendLayout()
        '
        'btnBloquear
        '
        Me.btnBloquear.BackColor = System.Drawing.Color.White
        Me.btnBloquear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnBloquear.FlatAppearance.BorderSize = 0
        Me.btnBloquear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBloquear.ImageIndex = 0
        Me.btnBloquear.ImageList = Me.imageList
        Me.btnBloquear.Location = New System.Drawing.Point(1, 2)
        Me.btnBloquear.Name = "btnBloquear"
        Me.btnBloquear.Size = New System.Drawing.Size(20, 23)
        Me.btnBloquear.TabIndex = 468
        Me.btnBloquear.UseVisualStyleBackColor = False
        '
        'imageList
        '
        Me.imageList.ImageStream = CType(resources.GetObject("imageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imageList.TransparentColor = System.Drawing.Color.Transparent
        Me.imageList.Images.SetKeyName(0, "lock.png")
        Me.imageList.Images.SetKeyName(1, "lock_unlock.png")
        '
        'LockButton
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnBloquear)
        Me.Name = "LockButton"
        Me.Size = New System.Drawing.Size(26, 26)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnBloquear As System.Windows.Forms.Button
    Friend WithEvents imageList As System.Windows.Forms.ImageList

End Class
