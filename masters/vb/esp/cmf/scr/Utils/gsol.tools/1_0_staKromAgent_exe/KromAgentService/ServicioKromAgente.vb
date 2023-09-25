Imports KromAgent.svchostka

Public Class srvAgenteKrom

    Private _time As Timers.Timer

    Protected Overrides Sub OnPause()
        ' Agregue el código aquí para realizar cualquier anulación necesaria para detener el servicio.
        _time.Stop()

    End Sub

    Protected Overrides Sub OnContinue()
        ' Agregue el código aquí para realizar cualquier anulación necesaria para detener el servicio.
        _time.Start()

    End Sub

    Protected Overrides Sub OnStart(ByVal args() As String)

        MsgBox("yaaa2")
        ' Agregue el código aquí para iniciar el servicio. Este método debería poner
        ' en movimiento los elementos para que el servicio pueda funcionar.

        _time = New Timers.Timer

        AddHandler _time.Elapsed, AddressOf RunTaskEach5Seconds

        _time.Interval = 5000

        _time.Start()

    End Sub

    Private Function RunTaskEach5Seconds()
        'Hacer algo

    End Function

    Protected Overrides Sub OnStop()
        ' Agregue el código aquí para realizar cualquier anulación necesaria para detener el servicio.
        _time.Stop()

    End Sub



End Class
