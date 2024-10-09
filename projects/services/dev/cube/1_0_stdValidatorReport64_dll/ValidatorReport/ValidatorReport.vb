Imports System.Linq.Expressions
Imports Cube.Interpreters
Imports MongoDB.Bson
Imports Cube.Validators
Imports System.Web.UI
Imports Gsol.Web.Components
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports Wma.Exceptions

Public Class ValidatorReport
    Implements ICloneable, IDisposable

#Region "Enums"

    Enum TriggerSourceTypes

        Undefined = 0

        Interpreter = 1

        Cube = 2

        Route = 3

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
    Property result As Object
    Property resultTagWatcher As TagWatcher
    Property messages As List(Of String)

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

                If valuesource_ <> "" Then

                    AddDetailReport(adviceTypeError_,
                                             description_,
                                             valuesource_,
                                             errortype_)
                End If

            Case TriggerSourceTypes.Cube

                Dim errortype_ = ICubeController.CubeErrorTypes.ErrorRuleRules

                If valuesource_ = "" Then

                    errortype_ = ICubeController.CubeErrorTypes.Undefined

                End If

                title = title_

                datereport = date_

                sourcetype = triggerSourceType_

                scope = scope_

                _idreport = ObjectId.GenerateNewId




                If valuesource_ <> "" Then

                    AddDetailReport(adviceTypeError_,
                                             description_,
                                             valuesource_,
                                             errortype_)
                End If

        End Select

        Return Me

    End Function

    Public Sub AddDetailReport(adviceTypeError_ As AdviceTypesReport,
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

    Public Sub SetDetailReport(adviceTypeError_ As AdviceTypesReport,
                                     description_ As String,
                                     valuesource_ As String,
                                     errortype_ As [Enum]
                                     )

        details = New List(Of ItemReport) From {New ItemReport With {.advicetypeerror = adviceTypeError_,
                                                                          .descriptionerrortype = adviceTypeError_.ToString,
                                                                          .description = description_,
                                                                          .valuesourceitem = valuesource_,
                                                                          .errortype = errortype_,
                                                                          .numberitem = 1
                                                                          }}


    End Sub

    Sub ShowMessageError(Optional typeMessage_ As Int32 = 1, Optional formularioMessage_ As Gsol.Web.Template.FormularioGeneralWeb = Nothing)

        Select Case typeMessage_

            Case 0

            Case 1

                Dim mensaje_ As String = title & Chr(13)

                For Each itemreport_ In details

                    mensaje_ &= "No. " & itemreport_.numberitem & Chr(9) &
                                itemreport_.description & Chr(13) &
                                itemreport_.valuesourceitem & Chr(13) &
                                itemreport_.errortype.ToString & Chr(13) &
                                itemreport_.descriptionerrortype.ToString & Chr(13)

                Next

                MsgBox(mensaje_)

            Case 2

                Dim mensaje_ As String = title & Chr(13)

                For Each itemreport_ In details

                    mensaje_ &= "No. " & itemreport_.numberitem & Chr(9) &
                                itemreport_.description & Chr(9) &
                                itemreport_.valuesourceitem & Chr(9) &
                                itemreport_.errortype.ToString & Chr(9) &
                                itemreport_.descriptionerrortype.ToString

                Next

                formularioMessage_.DisplayMessage(mensaje_.Replace(Chr(13), " ").Replace("'", Chr(34)),
                                   Gsol.Web.Template.FormularioGeneralWeb.StatusMessage.Fail)

            Case 3

                Dim mensaje_ As String = title & Chr(13)

                Dim numberItem_ = 1

                For Each itemreport_ In details

                    mensaje_ &= If(itemreport_.valuesourceitem = "" And numberItem_ > 1,
                                    "",
                                    itemreport_.descriptionerrortype.ToString) &
                                Chr(13) &
                                "No. " & numberItem_ & Chr(9) &
                                itemreport_.description & Chr(13) &
                                itemreport_.valuesourceitem & Chr(13)

                    numberItem_ += 1

                Next

                MsgBox(mensaje_)

        End Select

    End Sub

    Sub ShowMessageError(listalabel_ As List(Of Object))

        If details IsNot Nothing Then

            Dim labelCount_ = 0

            For Each label_ As WebControls.Label In listalabel_

                label_.Text = ""

                labelCount_ += 1

            Next

            Dim title_ = details(0).valuesourceitem.Split(Chr(13))

            listalabel_(0).Text = title_(1)
            listalabel_(1).Text = title_(2)
            listalabel_(2).Text = details(0).descriptionerrortype.ToString


            For Each detail_ In details

                Dim descripcions_ As List(Of String) = detail_.description.Replace("'", Chr(34)).Split(Chr(13)).ToList

                Dim index_ = 3

                For Each description_ In descripcions_

                    If index_ < labelCount_ Then

                        If description_ <> "" Then

                            listalabel_(index_).Text = "#" & index_ - 2 & ": " & description_

                            index_ += 1

                        End If

                    End If

                Next

            Next

        End If


    End Sub

    Public Function GetTitle() As String

        Return title

    End Function

    Public Function GetDetails() As List(Of ItemReport)

        Return details

    End Function

#Region "Clone"

    Public Function Clone() As Object Implements ICloneable.Clone

        Dim validatorClone_ As ValidatorReport

        Dim formatter_ As New BinaryFormatter()

        Dim stream_ As New MemoryStream()

        formatter_.Serialize(stream_, Me)

        stream_.Position = 0

        validatorClone_ = formatter_.Deserialize(stream_)

        Return validatorClone_

    End Function

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' Para detectar llamadas redundantes

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not Me.disposedValue Then

            If disposing Then
                ' TODO: eliminar estado administrado (objetos administrados).
            End If

            'Propiedades no administradas

            With Me

                .datereport = Nothing

                .details = Nothing

                .messages = Nothing

                .result = Nothing

                .scope = Nothing

                .sourcetype = Nothing

                .title = Nothing

                ._idreport = Nothing

                ._iduser = Nothing

            End With

            ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
            ' TODO: Establecer campos grandes como Null.
        End If

        Me.disposedValue = True

    End Sub


    ' Visual Basic agregó este código para implementar correctamente el modelo descartable.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en Dispose(disposing As Boolean).
        Dispose(True)

        GC.SuppressFinalize(Me)

    End Sub

#End Region

#End Region

End Class

Public Class ItemReport


#Region "Propierties"
    Property numberitem As Int32
    Property valuesourceitem As String

    Property description As String

    Property descriptionerrortype As String

    Property advicetypeerror As ValidatorReport.AdviceTypesReport

    Property errortype As [Enum]


#End Region

End Class
