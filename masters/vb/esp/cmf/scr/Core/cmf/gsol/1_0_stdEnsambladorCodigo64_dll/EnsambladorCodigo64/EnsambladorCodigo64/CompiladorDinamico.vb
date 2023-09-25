Imports Microsoft.VisualBasic
Imports System.CodeDom.Compiler
Imports System.Configuration
Imports System.Reflection

Namespace gsol

    Public Class CompiladorDinamico

        Public Shared _sistema As New Organismo

        Public Shared _ensambladosPendientes As List(Of String)

#Region "Constructores"

        Sub New()

            _sistema = New Organismo

            _ensambladosPendientes = New List(Of String)

            _ensambladosPendientes.Clear()


        End Sub

#End Region

#Region "Métodos"

        Public Shared Function RectificaEnsamblados(ByVal ensamblados_ As String()) As String()
            Dim ensambladosRectificados_(1000) As String

            Dim indice_ As Int32 = 0
            _ensambladosPendientes = New List(Of String)

            'System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString() & "\Modulos\"'

            For Each item_ As String In ensamblados_
                'If System.IO.File.Exists(ConfigurationManager.AppSettings("RutaModulosEnsamblados") & item_) = True Then
                '"..\..\..\..\..\..\..\..\Libs\Bin\Apps\Solutions\kb\Modules\"
                'If System.IO.File.Exists(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString() & "\Modulos\" & item_) = True Then
                'If System.IO.File.Exists(Environment.GetEnvironmentVariable("saxpath") & "\projects\kb\libs\bin\modules\" & item_) = True Then
                If System.IO.File.Exists("..\modules\" & item_) = True Then
                    ensambladosRectificados_(indice_) = item_.Clone
                    indice_ += 1
                Else

                    _ensambladosPendientes.Add(item_)

                End If
            Next

            ReDim Preserve ensambladosRectificados_(indice_ - 1)


            If _ensambladosPendientes.Count > 0 Then

                Dim ensambladosPendientes_ As String = Nothing

                For Each string_ As String In _ensambladosPendientes

                    ensambladosPendientes_ = ensambladosPendientes_ & ChrW(13) & string_

                Next


                _sistema.GsDialogo("Krombase said:Los siguientes ensablados no fueron localizados en el directorio de la solución: " & _
                                    ensambladosPendientes_.ToString, Componentes.SistemaBase.GsDialogo.TipoDialogo.Alerta)

            End If


            Return ensambladosRectificados_

        End Function


        ''' <summary>
        ''' Evalua si un script es viable de ejecutarse
        ''' </summary>
        ''' <param name="ensamblados_">Lista de DLLs utilizadas por el script</param>
        ''' <param name="codigo_">Script a evaluar</param>
        ''' <returns>Objeto que contiene el resultado de la evaluación del script, así como el resultado de su ejecución</returns>
        Public Shared Function EvaluarCodigoWinForms(
         ByVal ensamblados_() As String,
         ByVal codigo_ As String,
         Optional ByVal codigo2_ As String = Nothing
        ) As IScript
            Dim script_ As IScript = Nothing

            Using proveedorCodigo_ As New VBCodeProvider()
                Dim ensambladoLlamador_ As Assembly = Assembly.GetCallingAssembly()
                Dim opcionesCompilacion_ As CompilerParameters = New CompilerParameters()
                Dim revisor_ As New List(Of String)


                ensamblados_ = RectificaEnsamblados(ensamblados_)

                'Se cambio esta linea por optimización, PBM 16022015
                'opcionesCompilacion_.GenerateInMemory = True

                opcionesCompilacion_.GenerateInMemory = False

                'opcionesCompilacion_.CompilerOptions = "/reference:" & String.Join(",", ensamblados_)
                opcionesCompilacion_.ReferencedAssemblies.Add("System.dll")
                opcionesCompilacion_.ReferencedAssemblies.Add("System.Windows.Forms.dll")
                opcionesCompilacion_.ReferencedAssemblies.Add(Assembly.Load("Gsol.IEnsambladorCodigo.1.0.0.0").Location)
                opcionesCompilacion_.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location)
                opcionesCompilacion_.ReferencedAssemblies.Add(ensambladoLlamador_.Location)

                'Dim _rutaModulos As String = "C:\SVN\SVN QA\Modulos\"

                'Dim _rutaModulos As String = System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString() & "\Modulos\" 'ConfigurationManager.AppSettings("RutaModulosEnsamblados")
                'Dim _rutaModulos As String = Environment.GetEnvironmentVariable("saxpath") & "\projects\kb\libs\bin\modules\" 'ConfigurationManager.AppSettings("RutaModulosEnsamblados")
                Dim _rutaModulos As String = "..\modules\" 'ConfigurationManager.AppSettings("RutaModulosEnsamblados")
                '"..\..\..\..\..\masters\vb\esp\sax2.0\bin\Foreign\

                revisor_.Clear()

                For Each ensamblado_ As String In ensamblados_

                    If Not revisor_.Contains(_rutaModulos & ensamblado_) Then

                        revisor_.Add(_rutaModulos & ensamblado_)

                        opcionesCompilacion_.ReferencedAssemblies.Add(_rutaModulos & ensamblado_)

                    End If


                Next

                'Dim parameters As CompilerParameters = New CompilerParameters()
                'parameters.OutputAssembly = AssemblyName
                'parameters.GenerateInMemory = False
                'parameters.GenerateExecutable = False
                'parameters.IncludeDebugInformation = True


                Dim sources_ As String

                sources_ = CompiladorDinamico.ObtenerScript(ensamblados_, codigo_, ensambladoLlamador_.FullName, codigo2_)

                Dim resultado_ As CompilerResults = proveedorCodigo_.CompileAssemblyFromSource(opcionesCompilacion_, sources_)

                If resultado_.Errors.HasErrors = True Then

                    Dim errores_ As String = Nothing

                    For error_ As Integer = 1 To resultado_.Errors.Count

                        If error_ = 1 Then

                            errores_ = vbNewLine & error_ & ".- " & resultado_.Errors.Item(error_ - 1).ErrorText

                        Else

                            errores_ = errores_ & vbNewLine & error_ & ".- " & resultado_.Errors.Item(error_ - 1).ErrorText

                        End If

                    Next

                    _sistema.GsDialogo("Krombase compiler: Ocurrieron algunos errores al momento de esamblar: " & Chr(13) & _
                             errores_, Componentes.SistemaBase.GsDialogo.TipoDialogo.AvisoGrande)

                    resultado_ = proveedorCodigo_.CompileAssemblyFromSource(opcionesCompilacion_, CompiladorDinamico.ObtenerScriptDefault())

                Else

                    'MsgBox("Carga exitosa!")
                    script_ = DirectCast(resultado_.CompiledAssembly.CreateInstance("gsol.Script"), IScript)

                End If

                ' script_ = DirectCast(resultado_.CompiledAssembly.CreateInstance("gsol.Script"), IScript)

            End Using

            Return script_

        End Function

        ''' <summary>
        ''' Contruye el script que será evualuado por el CompiladorDinamico
        ''' </summary>
        ''' <param name="ensamblados_">Lista de DLLs utilizadas por el script</param>
        ''' <param name="codigo_">Código a ejecutarse en el script</param>
        ''' <param name="espacioNombresLlamador_">Espacio de nombres del ensamblado donde se realiza la evaluación del script</param>
        ''' <returns>Script a evaluar</returns>
        Private Shared Function ObtenerScript(
         ByVal ensamblados_() As String,
         ByVal codigo_ As String,
         ByVal espacioNombresLlamador_ As String,
         Optional ByVal codigo2_ As String = Nothing
        ) As String
            Dim script_ As Text.StringBuilder = New Text.StringBuilder()
            Dim revisor_ As New List(Of String)
            'Dim ensabladosPendientes_ As String = Nothing

            script_.AppendLine("")
            script_.AppendLine("Imports System")
            script_.AppendLine("Imports System.Windows.Forms")
            script_.AppendLine("Imports gsol")
            script_.AppendLine("Imports " & espacioNombresLlamador_.Split(",")(0))

            revisor_.Clear()

            For Each ensamblado_ As String In ensamblados_
                Dim ensabladoNombre_ As String = Nothing

                Try

                    'ensabladoNombre_ = Assembly.LoadFrom(ConfigurationManager.AppSettings("RutaModulosEnsamblados") & ensamblado_).GetExportedTypes()(0).Namespace
                    '..\..\..\..\..\..\..\..\Libs\Bin\Apps\Solutions\kb\Modules\
                    'ensabladoNombre_ = Assembly.LoadFrom(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString() & "\Modulos\" & ensamblado_).GetExportedTypes()(0).Namespace
                    'ensabladoNombre_ = Assembly.LoadFrom(Environment.GetEnvironmentVariable("saxpath") & "\projects\kb\libs\bin\modules\" & ensamblado_).GetExportedTypes()(0).Namespace
                    ensabladoNombre_ = Assembly.LoadFrom("..\modules\" & ensamblado_).GetExportedTypes()(0).Namespace

                    '

                    If Not revisor_.Contains(ensabladoNombre_) Then

                        revisor_.Add(ensabladoNombre_)

                        script_.AppendLine("Imports " & ensabladoNombre_)

                    End If

                Catch ex_ As Exception

                    _ensambladosPendientes.Add(ensamblado_)

                    Console.WriteLine("[CompiladosDinamico Said]: No se encontro el ensamblado " & ensamblado_)

                    _sistema.GsDialogo("Krombase compiler: Error al virtualizar el módulo: " & ex_.Message & " source:" & ex_.Source, Componentes.SistemaBase.GsDialogo.TipoDialogo.Err)

                    'Throw

                End Try

            Next

            'script_.AppendLine("         Dim f As New frm000DatawareHouse6445() ")
            'script_.AppendLine("         f.Name=" & Chr(34) & "frm000DatawareHousefrm" & Chr(34) & "")
            'script_.AppendLine("         Me.Resultado = f ")

            script_.AppendLine("Namespace gsol")
            script_.AppendLine("    Public Class Script")
            script_.AppendLine("        Implements IScript")
            script_.AppendLine("        Sub New()")
            script_.AppendLine("            Me.Estatus = True")
            script_.AppendLine("            Me.Operacion = IScript.TipoOperacion.Agregar")
            script_.AppendLine("        End Sub")
            script_.AppendLine("        Public Property Estatus As Boolean Implements IScript.Estatus")
            script_.AppendLine("        Public Property Operacion As IScript.TipoOperacion Implements IScript.Operacion")
            script_.AppendLine("        Public Property Resultado As Object Implements IScript.Resultado")
            script_.AppendLine("        Public Sub Ejecutar2() Implements IScript.Ejecutar2")
            script_.Append(CompiladorDinamico.PrepararCodigo(codigo2_))
            script_.AppendLine("        End Sub")
            script_.AppendLine("        Public Sub Ejecutar(ByVal parametro As Object) Implements IScript.Ejecutar")
            script_.Append(CompiladorDinamico.PrepararCodigo(codigo_))
            script_.AppendLine("        End Sub")
            script_.AppendLine("    End Class")
            script_.AppendLine("End Namespace")

            If _ensambladosPendientes.Count > 0 Then

                'Dim ensambladosPendientes_ As String = Nothing

                'For Each string_ As String In _ensambladosPendientes

                '    ensambladosPendientes_ = ensambladosPendientes_ & ChrW(13) & string_

                'Next


                '_sistema.GsDialogo("Los siguientes ensablados no fueron localizados en el directorio de la solución: " & _
                '                    ensambladosPendientes_.ToString)

            End If

            'MsgBox(script_.ToString)

            Return script_.ToString()


        End Function



        ''' <summary>
        ''' Contruye un script (por omisión) que será evualuado por el CompiladorDinamico
        ''' </summary>
        Private Shared Function ObtenerScriptDefault(
        ) As String
            Dim script_ As Text.StringBuilder = New Text.StringBuilder()

            script_.AppendLine("")
            script_.AppendLine("Imports System")
            script_.AppendLine("Namespace gsol")
            script_.AppendLine("    Public Class Script")
            script_.AppendLine("        Implements IScript")
            script_.AppendLine("        Sub New()")
            script_.AppendLine("            Me.Operacion = IScript.TipoOperacion.NoDefinida")
            script_.AppendLine("        End Sub")
            script_.AppendLine("        Public Property Estatus As Boolean Implements IScript.Estatus")
            script_.AppendLine("        Public Property Operacion As IScript.TipoOperacion Implements IScript.Operacion")
            script_.AppendLine("        Public Property Resultado As Object Implements IScript.Resultado")
            script_.AppendLine("        Public Sub Ejecutar(ByVal parametro As Object) Implements IScript.Ejecutar")
            script_.AppendLine("        End Sub")
            script_.AppendLine("        Public Sub Ejecutar2(ByVal parametro As Object) Implements IScript.Ejecutar2")
            script_.AppendLine("        End Sub")
            script_.AppendLine("    End Class")
            script_.AppendLine("End Namespace")
            Return script_.ToString()
        End Function

        ''' <summary>
        ''' Prepara el script que será evualuado por el CompiladorDinamico
        ''' </summary>
        ''' <param name="codigo_">Código a ejecutarse en el script</param>
        ''' <returns>Script preparado para ser evaluado</returns>
        Private Shared Function PrepararCodigo(
         ByVal codigo_ As String
        )
            If Not codigo_ Is Nothing Then


                Dim codigoPreparado_ As System.Text.StringBuilder = New System.Text.StringBuilder()
                Dim indentacion_ As String = "            "
                Dim sentencias_() As String = codigo_.Split(";")

                For Each sentencia_ As String In sentencias_
                    codigoPreparado_.AppendLine(indentacion_ & sentencia_)
                Next

                Return codigoPreparado_.ToString()
            Else
                Return Nothing
            End If

        End Function

#End Region

    End Class

End Namespace
