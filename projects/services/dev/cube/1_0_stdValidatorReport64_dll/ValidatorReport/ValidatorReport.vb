Imports System.Linq.Expressions
Imports Cube.ValidatorReport
Imports Cube.Interpreters
Imports MongoDB.Bson
Imports Cube.Validators

Public Class ValidatorReport

#Region "Enums"

    Enum TriggerSourceTypes

        Undefined = 0

        Interpreter = 1

        Cube = 2

    End Enum

    Enum AdviceTypesReport

        Undefined = 0

        Alert = 1

        Warning = 2

        Information = 3

    End Enum

#End Region

#Region "Propierties"
    Property _idreport As ObjectId

    Property title As String

    Property datereport As DateTime

    Property details As List(Of ItemReport)

    Property sourcetype As TriggerSourceTypes

    Property scope As AdviceTypesReport

    Property _iduser As ObjectId

#End Region


#Region "Methods"

    Public Function SetHeaderReport(title_ As String,
                                 date_ As DateTime,
                                 scope_ As AdviceTypesReport,
                                 adviceTypeError_ As AdviceTypesReport,
                                 description_ As String,
                                 sourceerrortype_ As String,
                                 valuesource_ As String, newExpression_ As String, Optional triggerSourceType_ As TriggerSourceTypes = TriggerSourceTypes.Interpreter) As ValidatorReport


        Select Case triggerSourceType_

            Case TriggerSourceTypes.Interpreter

                Dim errortype_ As IMathematicalInterpreter.InterpreterErrorTypes

                Dim operators = New List(Of String) From {"(",
                                                ",",
                                                ":",
                                                ")",
                                                "=",
                                                ">",
                                                "<",
                                                "+",
                                                "-",
                                                "*",
                                                "^",
                                                "/",
                                                "^",
                                                "~"}

                If sourceerrortype_ = "-2146232832" OrElse sourceerrortype_ = "-2146233033" Then

                    Dim palabras_ = title_.Split(" ")

                    If title_.Contains("extraneous") Then

                        If title_.Contains("<EOF>") Then

                            title_ = "Se encontró un " & palabras_(2) & " y se esperaba fin de al instrucción"

                        Else

                            title_ = "Se encontró un " & palabras_(2) & " y se esperaba un operador +,- ó un Operando"

                        End If

                    Else

                        If title_.Contains("mismatched") Then



                            title_ = "Falta el operando después del operador "

                        Else

                            If title_.Contains("missing") Then

                                If palabras_(1).Contains(")") Then

                                    title_ = "Falta  un ) para cerrar el ( "

                                Else

                                    title_ = "Falta  un " & palabras_(1)

                                End If



                            Else

                                If title_.Contains("token recognition") Then

                                    Dim indice_ = title_.IndexOf(vbCrLf)

                                    Dim indiceExpression_

                                    If indice_ = -1 Then

                                        indiceExpression_ = title_

                                    Else

                                        indiceExpression_ = title_.Substring(0, indice_)


                                    End If

                                    indiceExpression_ = indiceExpression_.Substring(indiceExpression_.LastIndexOf(":") + 1)

                                    Dim operando_ As String = ""

                                    For i As Int32 = indiceExpression_ To 0 Step -1

                                        If i < newExpression_.Length Then

                                            If operators.Any(Function(ch_) newExpression_(i) = ch_ Or newExpression_(i) = "'") Then

                                                Exit For

                                            Else

                                                operando_ = newExpression_(i) & operando_

                                            End If

                                        End If

                                    Next

                                    For i As Int32 = indiceExpression_ + 1 To newExpression_.Length - 1

                                        If operators.Any(Function(ch_) newExpression_(i) = ch_ Or newExpression_(i) = "'") Then

                                            Exit For

                                        Else

                                            operando_ &= newExpression_(i)

                                        End If

                                    Next

                                    title_ = " No se reconce el operando " & operando_

                                End If


                            End If


                        End If

                    End If

                    errortype_ = IMathematicalInterpreter.InterpreterErrorTypes.Syntax

                Else

                    If sourceerrortype_.Contains("División entre 0") Or sourceerrortype_.Contains("Infinito") Then

                        errortype_ = IMathematicalInterpreter.InterpreterErrorTypes.DivisionByZero

                    Else

                        If sourceerrortype_.Contains("Imaginario") Then

                            errortype_ = IMathematicalInterpreter.InterpreterErrorTypes.ImaginaryNumber

                        Else

                            If title_.Contains("Parameter was not defined") Then

                                title_ = "Un parámetro no está bien definido"

                                errortype_ = IMathematicalInterpreter.InterpreterErrorTypes.UndefinedVariable

                            Else

                                errortype_ = IMathematicalInterpreter.InterpreterErrorTypes.Semantic

                            End If



                        End If

                    End If


                End If


                title = title_
                datereport = date_
                sourcetype = triggerSourceType_
                scope = scope_
                _idreport = ObjectId.GenerateNewId
                AddDetailReport(adviceTypeError_,
                                             description_,
                                             valuesource_,
                                             errortype_)

            Case TriggerSourceTypes.Cube

                Dim errortype_ = ICubeController.CubeErrorTypes.RegimenClaveOperacion

                title = title_
                datereport = date_
                sourcetype = triggerSourceType_
                scope = scope_
                _idreport = ObjectId.GenerateNewId
                AddDetailReport(adviceTypeError_,
                                             description_,
                                             valuesource_,
                                             errortype_)



        End Select


        Return Me

    End Function


    Private Sub AddDetailReport(adviceTypeError_ As AdviceTypesReport,
                                     description_ As String,
                                     valuesource_ As String,
                                     errortype_ As [Enum]
                                     )

        If details Is Nothing Then

            details = New List(Of ItemReport) From {New ItemReport With {.advicetypeerror = adviceTypeError_,
                                                                              .descriptionerrortype = adviceTypeError_.ToString,
                                                                              .description = description_,
                                                                              .valuesourceitem = valuesource_,
                                                                              .errortype = errortype_,
                                                                              .numberitem = 1
                                                                              }}

        Else

            details.Add(New ItemReport With {.advicetypeerror = adviceTypeError_,
                                                                              .descriptionerrortype = adviceTypeError_.ToString,
                                                                              .description = description_,
                                                                              .valuesourceitem = valuesource_,
                                                                              .errortype = errortype_,
                                                                              .numberitem = details.Count + 1
                                                                              })

        End If

    End Sub

    Sub ShowMessageError()

        Dim mensaje_ As String = title & Chr(13)

        For Each itemreport_ In details

            mensaje_ &= "No. " & itemreport_.numberitem & Chr(9) &
                        itemreport_.description & Chr(9) &
                        itemreport_.valuesourceitem & Chr(9) &
                        itemreport_.errortype.ToString & Chr(9) &
                        itemreport_.descriptionerrortype.ToString

        Next

        MsgBox(mensaje_)

    End Sub

#End Region

End Class

Public Class ItemReport


#Region "Propierties"
    Property numberitem As Int32
    Property valuesourceitem As String

    Property description As String

    Property descriptionerrortype As String

    Property advicetypeerror As AdviceTypesReport

    Property errortype As [Enum]


#End Region

End Class
