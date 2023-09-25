Public Class Form1


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        'hoy es  sabado 05 de mayo 2018

        Dim primerCaracter_ As String = Now.DayOfWeek  ' día de la semana, numero natural sin ceros a la izquierda = 6
        Dim segundoCaracter_ As String = UCase(Mid(Now.ToString("dddd"), Now.ToString("dddd").Length - 1, 2))  ' dos últimos ´digitos del día, mayuscula ( lunES)  = DO
        Dim tercerCaracter_ As String = (Now.DayOfWeek * Now.Day).ToString ' día de la semana multiplicado por día del mes = 6*5 = 30
        Dim cuartoCaracter_ As String = LCase(Mid(MonthName(Now.Month, True), MonthName(Now.Month, True).Length - 1, 1)) ' ultima letra minuscula del mes ej. Mayo, = o
        Dim quintoCaracter_ As String = Now.DayOfWeek ' ultima letra minuscula del mes ej. Mayo, = o
        Dim sextoCaracter_ As String = LCase(Mid(Now.ToString("dddd"), 1, 1)) & LCase(Mid(MonthName(Now.Month, True), MonthName(Now.Month, True).Length - 1, 1)) 'primera letra del dia y penultima letra del mes minusculas ej. Sábado y Mayo = so

        'password= 5ES10090oso

        primerCaracter_ = Now.Day.ToString

        'MsgBox(primerCaracter_ & "," & _
        '       segundoCaracter_ & "," & _
        '       tercerCaracter_ & "," & _
        '       cuartoCaracter_ & "," & _
        '       quintoCaracter_ & "," & _
        '       sextoCaracter_
        '       )

        TextBox1.Text = primerCaracter_ &
               segundoCaracter_ &
               tercerCaracter_ &
               cuartoCaracter_ &
               quintoCaracter_ &
               sextoCaracter_


    End Sub

End Class
