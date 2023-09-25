
Public Class ProgressDialog
    Inherits Form

    ' Delegate to update the progress 
    ' of the ProgressBar widget
    Delegate Sub DelegateUpdate(ByVal progress As Integer)

    ' Delegate to handle the Close event 
    ' for this dialog
    Delegate Sub DelegateClose(ByRef dialog As Form)

    ' Default constructor which initializes 
    ' this control
    Public Sub New()
        InitializeComponent()
    End Sub



    ' Method to update the progress bar widget. This 
    ' uses the InvokeRequired and BeginInvoke methods
    Public Sub UpdateProgress(ByVal progress As Integer)
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.BeginInvoke(New DelegateUpdate(AddressOf UpdateDelegateImpl), progress)
        Else
            ProgressBar1.Value = progress
        End If
    End Sub

    ' Overloads the Close method
    Public Overloads Sub Close()
        If Me.InvokeRequired Then
            Me.BeginInvoke(New  _
                           DelegateClose(AddressOf CloseDelegateImpl), Me)
        Else
            Me.Close()
        End If
    End Sub

    ' Implementation of the DelegateUpdate
    Sub UpdateDelegateImpl(ByVal progress As Integer)
        ProgressBar1.Value = progress
        LabelProgress.Text = ""
        LabelProgress.Text = progress & CStr("%")
    End Sub

    ' Implmentation of the DelegateClose
    Sub CloseDelegateImpl(ByRef dialog As Form)
        dialog.Close()
    End Sub
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar

    Private Sub InitializeComponent()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.LabelProgress = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(57, 12)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(262, 12)
        Me.ProgressBar1.TabIndex = 0
        '
        'LabelProgress
        '
        Me.LabelProgress.AutoSize = True
        Me.LabelProgress.Location = New System.Drawing.Point(12, 9)
        Me.LabelProgress.Name = "LabelProgress"
        Me.LabelProgress.Size = New System.Drawing.Size(39, 13)
        Me.LabelProgress.TabIndex = 1
        Me.LabelProgress.Text = "Label1"
        '
        'ProgressDialog
        '
        Me.ClientSize = New System.Drawing.Size(402, 42)
        Me.Controls.Add(Me.LabelProgress)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Name = "ProgressDialog"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelProgress As System.Windows.Forms.Label
End Class