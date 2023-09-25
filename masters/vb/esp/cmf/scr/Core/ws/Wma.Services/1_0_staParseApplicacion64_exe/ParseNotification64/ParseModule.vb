Imports Wma.WebServices
Imports System.Threading

Module ParseModule

    Private _referencia As String

    Private _notificacion As String

    'Sub Main()

    '    ' Dim notification2_ As New ParseNotification(reference_, message_)

    '    Dim thread_ As New Thread(
    '                              Sub()
    '                                  Dim notification_ As New ParseNotification(reference_, message_)

    '                                  Console.Write(notification_.GetStatus)

    '                              End Sub
    '                              )
    '    thread_.Start()
    '    thread_.Join()


    'End Sub


    Public Sub Main()

        ' Habilitamos los estilos visuales
        'Application.EnableVisualStyles()

        Dim argumentos As ObjectModel.ReadOnlyCollection(Of String) = _
              My.Application.CommandLineArgs()

        Dim referencia_ As String = "/referencia="

        Dim notificacion_ As String = "/notificacion="

        For Each arg As String In argumentos

            If (arg.ToLower().StartsWith(referencia_)) Then

                _referencia = arg.Remove(0, referencia_.Length)
                'DateTime.TryParse(fecha, m_Fecha)
                Continue For

            End If

            If (arg.ToLower().StartsWith(notificacion_)) Then

                _notificacion = arg.Remove(0, notificacion_.Length)

                Continue For

            End If

        Next

        ' Mostramos el formulario inicial de la aplicación
        '
        'Application.Run(New Form1())

        Dim notification_ As ParseNotification


        If _referencia Is Nothing Or _
           _notificacion Is Nothing Then

            Console.Write("Not enough parameters")

        Else

            Dim thread_ As New Thread(
                              Sub()

                                  notification_ = New ParseNotification(_referencia, _notificacion)

                              End Sub
                              )

            thread_.Start()

            thread_.Join()

            Console.Write("Done")

        End If

    End Sub

End Module
