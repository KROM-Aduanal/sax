<System.ComponentModel.RunInstaller(True)> Partial Class ProjectInstaller
    Inherits System.Configuration.Install.Installer

    'Installer reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de componentes
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de componentes requiere el siguiente procedimiento
    'Se puede modificar usando el Diseñador de componentes.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.spiAgenteKrom = New System.ServiceProcess.ServiceProcessInstaller()
        Me.srviAgenteKrom = New System.ServiceProcess.ServiceInstaller()
        '
        'spiAgenteKrom
        '
        Me.spiAgenteKrom.Account = System.ServiceProcess.ServiceAccount.LocalService
        Me.spiAgenteKrom.Password = Nothing
        Me.spiAgenteKrom.Username = Nothing
        '
        'srviAgenteKrom
        '
        Me.srviAgenteKrom.Description = "Agente para control e integridad de Kardex de equipo de cómputo Krom "
        Me.srviAgenteKrom.DisplayName = "Agente de inventario Krom"
        Me.srviAgenteKrom.ServiceName = "Agente Krom"
        '
        'ProjectInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.spiAgenteKrom, Me.srviAgenteKrom})

    End Sub
    Friend WithEvents spiAgenteKrom As System.ServiceProcess.ServiceProcessInstaller
    Friend WithEvents srviAgenteKrom As System.ServiceProcess.ServiceInstaller

End Class
