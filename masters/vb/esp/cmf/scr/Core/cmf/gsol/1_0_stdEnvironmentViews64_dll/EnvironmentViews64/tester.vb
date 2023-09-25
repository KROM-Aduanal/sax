Imports Gsol.BaseDatos.Operaciones
Imports Wma.Operations
Imports Wma.Exceptions

Module tester


    Sub main()

        'MsgBox("hola mundo!!")

        Dim ev_ As New EnvironmentViews()

        'Test 2

        If ev_.ConvertEVTableToStatic("Ve025IUCantidadOperaciones") Then : MsgBox("done!") : End If
        If ev_.ConvertEVTableToStatic("Ve022IUConsultaOperacionesVivas") Then : MsgBox("done!") : End If
        If ev_.ConvertEVTableToStatic("Ve022IUConsultaOperaciones") Then : MsgBox("done!") : End If





        Dim messaje_ As New TagWatcher

        messaje_ = ev_.GetEnvironmentViewAsJSON("Ve009IUMaestroOperaciones")

        If messaje_.Status = TagWatcher.TypeStatus.Ok Then

            MsgBox("oki")

        End If



    End Sub

End Module
