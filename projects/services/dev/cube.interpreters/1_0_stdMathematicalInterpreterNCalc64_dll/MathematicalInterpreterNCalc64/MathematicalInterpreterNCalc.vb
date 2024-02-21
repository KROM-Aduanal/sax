Imports System.Text.RegularExpressions
Imports Cube.ValidatorReport
Imports Cube.Validators
Imports gsol.krom
Imports MongoDB.Driver
Imports MongoDB.Driver.Linq
Imports NCalc
Imports Newtonsoft.Json
Imports Wma.Exceptions

Public Class MathematicalInterpreterNCalc
    Implements IMathematicalInterpreter, ICloneable, IDisposable


#Region "Attributes"

    Private _status As TagWatcher

    Private _interpreterType As IMathematicalInterpreter.InterpreterTypes

    Private _reports As ValidatorReport

    Private _operandsNCalc As List(Of String)

    Private _operandsTemp As List(Of String)

    Private _customFunctions As List(Of String)

    Private _operators As List(Of String)

    Private _expressionNCalc As NCalc.Expression

#End Region

#Region "Properties"

    Public Property Status As TagWatcher Implements IMathematicalInterpreter.status

        Get

            Return _status

        End Get

        Set(value As TagWatcher)

            _status = value

        End Set

    End Property

    Public Property InterpreterType As IMathematicalInterpreter.InterpreterTypes _
        Implements IMathematicalInterpreter.interpreterType
        Get

            Return _interpreterType

        End Get

        Set(value As IMathematicalInterpreter.InterpreterTypes)

            _interpreterType = value

        End Set

    End Property




#End Region


#Region "Methods"

    Sub New()

        _customFunctions = New List(Of String) From {"SUMAR",
                                                           "SUMAR.SI",
                                                           "RANGO",
                                                           "RED",
                                                           "REDONDEAR",
                                                           "TRUNCAR",
                                                           "ROOM",
                                                           "SETROOM",
                                                           "ASIGNAR",
                                                           "TRUNC",
                                                           "SI",
                                                           "Y",
                                                           "O"}

        _operandsNCalc = New List(Of String) From {"if",
                                                    "Round",
                                                    "N",
                                                    "Truncate",
                                                    "Sqrt",
                                                    "Pow"}

        _operators = New List(Of String) From {"(",
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
                                                "~",
                                                " and ",
                                                " or "
                                                }

        _operandsTemp = SetOperands()

        _reports = New ValidatorReport

    End Sub

    Public Function RunExpression(Of T)(expression_ As String,
                                         constantValues_ As Dictionary(Of String,
                                                                         T)) As T _
                                        Implements IMathematicalInterpreter.RunExpression



        If constantValues_ Is Nothing Then

            constantValues_ = New Dictionary(Of String, T)

        End If

        Dim newExpression_ = GetExpression(expression_,
                                           constantValues_)

        Dim paramValues_ As New Dictionary(Of String, Object)

        For Each constanvalue_ In constantValues_

            Dim doubleParse_ As Double = 0

            If Double.TryParse(constanvalue_.Value.ToString, doubleParse_) Then

                paramValues_.Add(constanvalue_.Key, doubleParse_)

            Else

                paramValues_.Add(constanvalue_.Key, constanvalue_.Value)

            End If


        Next

        _expressionNCalc = New NCalc.Expression(newExpression_) With {.Parameters = paramValues_}

        AddHandler _expressionNCalc.EvaluateFunction, RunFunctionHandler(paramValues_)

        Dim result_

        Try

            result_ = _expressionNCalc.Evaluate

            If result_.ToString.Contains(ChrW(&H221E)) Then

                _reports.SetHeaderReport(result_.ToString & " División Entre 0 ",
                                     DateTime.Now,
                                    AdviceTypesReport.Alert,
                                     AdviceTypesReport.Alert,
                                    "Valor Infinito por División entre 0",
                                    "Infinito",
                                    expression_, newExpression_)

                _reports.ShowMessageError()

            Else

                If result_.ToString = "NaN" Then

                    Dim posicion_ = newExpression_.IndexOf("Sqrt")

                    If posicion_ > 0 Then

                        Dim expresionSqrt_ = New NCalc.Expression(GetExpressionInside(newExpression_.Substring(posicion_ + 4, newExpression_.Length - posicion_ - 4)))

                        AddHandler expresionSqrt_.EvaluateFunction, RunFunctionHandler(paramValues_)


                        If expresionSqrt_.Evaluate < 0 Then


                            _reports.SetHeaderReport(result_.ToString & " Número Imaginario ",
                                            DateTime.Now,
                                            AdviceTypesReport.Alert,
                                            AdviceTypesReport.Alert,
                                            "Número Imaginario por raiz de número negativo ",
                                            "Número Imaginario",
                                            expression_, newExpression_)

                        Else

                            _reports.SetHeaderReport(result_.ToString & " División Entre 0 ",
                             DateTime.Now,
                             AdviceTypesReport.Alert,
                             AdviceTypesReport.Alert,
                              "Intedeterminación por División entre 0",
                               "Indeterminación",
                              expression_, newExpression_)

                        End If

                    Else

                        _reports.SetHeaderReport(result_.ToString & " División Entre 0 ",
                                        DateTime.Now,
                                        AdviceTypesReport.Alert,
                                        AdviceTypesReport.Alert,
                                        "Intedeterminación por División entre 0",
                                        "Indeterminación",
                                         expression_, newExpression_)

                    End If

                    _reports.ShowMessageError()

                End If



            End If

        Catch ex_ As NCalc.EvaluationException

            If Array.FindAll(expression_.ToCharArray(), Function(ch) ch = "'").Count Mod 2 = 1 Then

                _reports.SetHeaderReport("Falta una comilla(') de cierre",
                                         DateTime.Now,
                                        AdviceTypesReport.Alert,
                                         AdviceTypesReport.Alert,
                                        "Falta una comilla(') de cierre",
                                        "-2146232832",
                                        expression_, newExpression_)
            Else
                _reports.SetHeaderReport(ex_.Message,
                DateTime.Now,
                AdviceTypesReport.Alert,
                AdviceTypesReport.Alert,
                ex_.Message,
                ex_.HResult,
                expression_, newExpression_)
            End If



            _reports.ShowMessageError()

            'MsgBox(ex_.HResult & Chr(13) & _reports.title & Chr(13) & newExpression_)

            result_ = ""

        Catch ex_ As Exception

            Dim algo_ = Array.FindAll(expression_.Split(""), Function(ch) ch = "'").Count

            If Array.FindAll(expression_.ToCharArray(), Function(ch) ch = "'").Count Mod 2 = 1 Then

                _reports.SetHeaderReport("Falta una comilla(') de cierre",
                                         DateTime.Now,
                                        AdviceTypesReport.Alert,
                                         AdviceTypesReport.Alert,
                                        "Falta una comilla(') de cierre",
                                        "-2146232832",
                                        expression_, newExpression_)
            Else


                _reports.SetHeaderReport(ex_.Message,
                                     DateTime.Now,
                                    AdviceTypesReport.Alert,
                                     AdviceTypesReport.Alert,
                                    ex_.Message,
                                    ex_.HResult,
                                    expression_, newExpression_)
            End If


            _reports.ShowMessageError()


            result_ = ""

        End Try

        Return result_

    End Function

    Public Function RunExpression(Of T)(expression_ As String,
                                        constantValues_ As Dictionary(Of String, T),
                                        interprete_ As IMathematicalInterpreter.InterpreterTypes) As T _
                                        Implements IMathematicalInterpreter.RunExpression

        If interprete_ = IMathematicalInterpreter.InterpreterTypes.NCALC Then

            Return RunExpression(expression_, constantValues_)

        Else

            Return Nothing

        End If

    End Function

    Public Function GetReportFull() As ValidatorReport Implements IMathematicalInterpreter.GetReportFull
        Throw New NotImplementedException()
    End Function

    Private Function GetExpressionInside(expression_ As String) As String

        Dim parenthicalCount_ = 0

        Dim position_ = 0

        For Each character_ In expression_

            position_ += 1

            Select Case character_

                Case "("

                    parenthicalCount_ += 1

                Case ")"

                    parenthicalCount_ -= 1


            End Select

            If parenthicalCount_ = 0 Then

                Return expression_.Substring(0, position_)

            End If

        Next

        Return expression_.Substring(0, position_)

    End Function

    Public Sub addOperands(operands_ As List(Of String)) Implements IMathematicalInterpreter.addOperands


        For operandsIndex_ = 0 To operands_.Count - 1 Step 2

            Dim indiceFind_ = _operandsTemp.IndexOf(operands_(operandsIndex_))

            If indiceFind_ > -1 Then

                _operandsTemp.RemoveAt(indiceFind_)
                _operandsTemp.RemoveAt(indiceFind_)

            End If

        Next

        _operandsTemp.AddRange(operands_)

    End Sub
    Private Function SetOperands() As List(Of String)

        Dim vector_ = GetListOperandAssignment("TASADESC=30")

        vector_.AddRange(GetListOperandAssignment("VAE=25"))

        vector_.AddRange(GetListOperandAssignment("ADVPRE=20"))

        vector_.AddRange(GetListOperandAssignment("TASAIGI=15"))

        vector_.AddRange(GetListOperandAssignment("TASAIGIPARTIDA=15"))

        vector_.AddRange(GetListOperandAssignment("TASAIVA=16"))

        vector_.AddRange(GetListOperandAssignment("DTAPP=1000"))

        vector_.AddRange(GetListOperandAssignment("IEPS=20"))

        vector_.AddRange(GetListOperandAssignment("CC=RED(TASACC*S24.CA_CANTIDAD_UMT_PARTIDA*S23.CA_TIPO_CAMBIO)"))

        vector_.AddRange(GetListOperandAssignment("ISAN= 100"))

        vector_.AddRange(GetListOperandAssignment("CIEPS=25.5"))

        vector_.AddRange(GetListOperandAssignment("CNR=53"))

        vector_.AddRange(GetListOperandAssignment("CUOESP=1.39"))

        vector_.AddRange(GetListOperandAssignment("PR=2.61"))

        vector_.AddRange(GetListOperandAssignment("VAU=(S24.CA_VALOR_ADUANA_PARTIDA/S23.CA_TIPO_CAMBIO) / S24.CA_CANTIDAD_UMT_PARTIDA"))

        vector_.AddRange(GetListOperandAssignment("TASACC=TRUNCAR(PR-VAU,4)"))

        vector_.AddRange(GetListOperandAssignment("TASADTA=0.008"))

        vector_.AddRange(GetListOperandAssignment("DTA=SI(S18.AplicaPD=True,SI(S18.AplicaAI=True,RED(S23.CA_VALOR_ADUANA*TASADTA)*0.92,RED(S23.CA_VALOR_ADUANA*TASADTA))+3*CFDTA,SI(S18.AplicaAI=True,RED(S23.CA_VALOR_ADUANA*TASADTA)*0.92,RED(S23.CA_VALOR_ADUANA*TASADTA)))"))

        vector_.AddRange(GetListOperandAssignment("IGI=RED(S23.CA_VALOR_ADUANA*TASAIGI/100)"))

        vector_.AddRange(GetListOperandAssignment("IGIPARTIDA=RED(S24.CA_VALOR_ADUANA_PARTIDA*TASAIGIPARTIDA/100)"))

        vector_.AddRange(GetListOperandAssignment("IVAPARTIDA=RED((S24.CA_VALOR_ADUANA_PARTIDA+IGIPARTIDA)*TASAIVA/100)"))

        vector_.AddRange(GetListOperandAssignment("CFDTAE=409"))

        vector_.AddRange(GetListOperandAssignment("TASADTAAF=0.00176"))

        vector_.AddRange(GetListOperandAssignment("DTAAF=RED(S23.CA_VALOR_ADUANA*TASADTAAF)"))


        Return vector_

    End Function
    Private Function GetListOperandAssignment(expression_ As String) As List(Of String)

        Return expression_.Split(New String() {"="}, 2, StringSplitOptions.None).ToList()

    End Function

    Public Function GetParams(expression_ As String) As List(Of String) Implements IMathematicalInterpreter.GetParams

        Dim listOperand_ = GetListFormula(expression_)

        listOperand_ = GetListOperandOperator(listOperand_)

        Dim numero_ As Double

        listOperand_.RemoveAll(Function(ch) Not ch.Contains(",SI") Or _customFunctions.Contains(ch.Substring(0, ch.Length - 3)) Or {"true", "false"}.Contains(ch.Substring(0, ch.Length - 3)) Or _operandsNCalc.Contains(ch.Substring(0, ch.Length - 3)) Or Double.TryParse(ch.Substring(0, ch.Length - 3), numero_))

        listOperand_ = listOperand_.Select(Function(e) If(e.Length > 3, e.Substring(0, e.Length - 3), e)).ToList

        listOperand_.RemoveAll(Function(ch) _operandsTemp.Contains(ch) And _operandsTemp.IndexOf(ch) Mod 2 = 0)

        Dim listOperadTemp_ As New List(Of String)

        For Each operand_ In listOperand_


            operand_ = operand_.Replace(" ", "").Replace(vbCrLf, "").Replace(vbLf, "")

            If Not listOperadTemp_.Contains(operand_) Then

                If operand_.Length > 0 Then

                    If operand_(0) <> "'" And operand_ <> "SI" And operand_ <> "RED" And operand_.IndexOf(".") > 0 Then

                        listOperadTemp_.Add(operand_)

                    End If


                End If

            End If

        Next

        listOperand_ = listOperadTemp_

        Return listOperand_

    End Function

    Private Function GetListOperandOperator(listElement_ As List(Of String)) As List(Of String)

        Dim auxListElement_ As New List(Of String)

        Dim polinomial_ = True

        While polinomial_

            Dim cuentaMonomial_ = 0

            For Each element_ In listElement_

                Dim positionLastComma_ = element_.LastIndexOf(",")

                Dim elementBeforeComma_ = element_.Substring(0, positionLastComma_)

                Dim elementAfterComma_ = element_.Substring(positionLastComma_)

                Dim auxiliary_ = GetValueOperand(elementBeforeComma_)

                If elementAfterComma_ = ",SI" AndAlso FindOperators(auxiliary_) Then

                    For Each auxElement_ In GetListFormula(auxiliary_)

                        auxListElement_.Add(auxElement_)

                    Next

                    auxiliary_ = ""

                Else

                    auxListElement_.Add(auxiliary_ & elementAfterComma_)

                End If

                If auxiliary_ <> elementBeforeComma_ Then

                    cuentaMonomial_ += 1

                End If


            Next

            listElement_ = auxListElement_.ToList

            auxListElement_ = New List(Of String)

            If cuentaMonomial_ = 0 Then

                polinomial_ = False

            End If

        End While

        Return listElement_

    End Function

    Private Function GetExpression(Of T)(expression_ As String,
                                  values_ As Dictionary(Of String, T)) _
                                  As String

        Dim customFunctions_ As New List(Of String)

        customFunctions_.AddRange(_operandsNCalc)

        customFunctions_.AddRange(_customFunctions)

        Dim finalExpression_ As String = ""

        Dim operandos_ = _operandsTemp

        Dim parenthesisCount_, parenthesisCountTruncate_ As New List(Of String)

        expression_ = SeparateSentences(expression_)

        Dim operandList_ = GetListFormula(expression_)

        operandList_ = GetListOperandOperator(operandList_)

        Dim found2Points_ = False

        For Each element_ In operandList_

            Dim curedElement_ = element_.Substring(0, element_.LastIndexOf(","))

            If element_(element_.Length - 1) = ";" Then

                Dim number_ As Integer

                Dim position_ = finalExpression_.LastIndexOf(",")

                Dim isNumber_ = True

                If position_ > -1 Then

                    isNumber_ = Integer.TryParse(finalExpression_.Substring(position_ + 1), number_)

                Else

                    isNumber_ = False

                End If

                If Not (position_ > -1 And isNumber_) Then

                    finalExpression_ &= ",0"

                End If

                finalExpression_ &= element_.Substring(0, element_.LastIndexOf(","))

            Else

                If element_(element_.Length - 1) = "?" Then

                    Dim position_ = finalExpression_.LastIndexOf(",")

                    If position_ = -1 Then

                        finalExpression_ &= element_.Substring(0, element_.LastIndexOf(","))

                    Else

                        Dim zeros_ = "1"

                        Dim lengthfinalExpression_ As Int32 = finalExpression_.Substring(position_ + 1)

                        For i = 1 To lengthfinalExpression_

                            zeros_ &= "0"

                        Next

                        Dim positionLastTruncate_ = finalExpression_.LastIndexOf("Truncate")

                        If positionLastTruncate_ = -1 Then

                            finalExpression_ &= element_.Substring(0,
                                                                   element_.LastIndexOf(","))

                        Else

                            finalExpression_ = finalExpression_.Substring(0,
                                                                    positionLastTruncate_ + 9) &
                                              "(" &
                                              finalExpression_.Substring(positionLastTruncate_ + 9,
                                                                          position_ - positionLastTruncate_ - 9) &
                                              ")*" &
                                              zeros_ &
                                              ")/" &
                                              zeros_

                        End If

                    End If

                Else

                    If element_(element_.Length - 1) = "}" Then

                        Dim lastRangePosition_ = finalExpression_.LastIndexOf("RANGO")

                        Dim range_ = finalExpression_.Substring(lastRangePosition_ + 6)

                        finalExpression_ = finalExpression_.Substring(0, lastRangePosition_)

                        lastRangePosition_ = range_.LastIndexOf(",")

                        Dim finalPosition_ As String = range_.Substring(lastRangePosition_ + 1)

                        range_ = range_.Substring(0, lastRangePosition_)

                        lastRangePosition_ = range_.LastIndexOf(",")

                        Dim initialPosition_ = range_.Substring(lastRangePosition_ + 1)

                        range_ = range_.Substring(0, lastRangePosition_)

                        range_ = range_.Replace("[", "").Replace("]", "")

                        Dim auxList_ = GetMatchesCount(range_, values_)

                        If finalPosition_ = "N" Then

                            finalPosition_ = auxList_(auxList_.Count - 1)

                        End If

                        range_ = ""

                        For Each elementoAux_ In auxList_.Take(auxList_.Count - 1)

                            range_ &= elementoAux_

                        Next

                        Dim mongoPosition_ As Int32

                        For mongoPosition_ = initialPosition_ - 1 To finalPosition_ - 1

                            finalExpression_ &= SetValuesOperands(range_,
                                                                     mongoPosition_,
                                                                     values_,
                                                                     False) &
                                                                     ","

                        Next

                        finalExpression_ = finalExpression_.Substring(0, finalExpression_.Length - 1)

                    Else

                        If element_.Substring(element_.LastIndexOf(",")) = ",SI" Then

                            If found2Points_ Then

                                found2Points_ = False

                                Dim lastRightParenthesisIndex_ = finalExpression_.LastIndexOf("(")

                                Dim last2PointIndex_ = finalExpression_.LastIndexOf(":")

                                Dim tempOperand_ = finalExpression_.Substring(lastRightParenthesisIndex_ + 1,
                                                                              last2PointIndex_ - lastRightParenthesisIndex_ - 1)

                                finalExpression_ = finalExpression_.Substring(0,
                                                                        lastRightParenthesisIndex_ + 1)

                                Dim initialRange_ As Int32

                                If Not Int32.TryParse(Regex.Match(tempOperand_,
                                                              "\d+$").
                                            Value,
                                            initialRange_) Then

                                    initialRange_ = -1

                                End If

                                tempOperand_ = tempOperand_.TrimEnd("0123456789".ToCharArray())

                                    Dim lengthFinalOperand = curedElement_.Length - 1

                                    While Char.IsNumber(CChar(Enumerable.ElementAt(CType(curedElement_, IEnumerable(Of Char)), CInt(lengthFinalOperand)))) Or
                                          Enumerable.ElementAt(CType(curedElement_, IEnumerable(Of Char)), CInt(lengthFinalOperand)) = "N"

                                        lengthFinalOperand -= 1

                                    End While

                                    Dim finalRange_ As Int32

                                    tempOperand_ = _operandsTemp(_operandsTemp.IndexOf(tempOperand_) + 1)

                                    Dim listaAux_ = GetMatchesCount(tempOperand_,
                                                                          values_)

                                    If curedElement_.Substring(CInt(lengthFinalOperand + 1),
                                                                CInt(curedElement_.Length - lengthFinalOperand - 1)) = "N" Then

                                        finalRange_ = listaAux_(listaAux_.Count - 1) - 1

                                    Else

                                        finalRange_ = curedElement_.Substring(CInt(lengthFinalOperand + 1),
                                                                              CInt(curedElement_.Length - 1)) - 1

                                    End If

                                    Dim mongoPosition_ As Int32

                                    For mongoPosition_ = initialRange_ - 1 To finalRange_

                                        finalExpression_ &= SetValuesOperands(tempOperand_,
                                                                             mongoPosition_,
                                                                             values_,
                                                                             False) &
                                                                             ","

                                    Next

                                    finalExpression_ = finalExpression_.Substring(0,
                                                                        finalExpression_.Length - 1)

                                Else

                                    finalExpression_ &= SetValuesOperands(curedElement_,
                                                                         0,
                                                                         values_,
                                                                         False)

                            End If

                        Else

                            If curedElement_ = ":" Then

                                found2Points_ = True

                            End If

                            finalExpression_ &= curedElement_

                        End If

                    End If

                End If

            End If

        Next

        Return finalExpression_

    End Function

    Function SeparateSentences(expression_ As String) As String

        Dim separationQuotes_ = expression_.Split("'")

        Dim result_ = ""

        Dim quotesCount_ = 0

        For Each separation_ In separationQuotes_

            If quotesCount_ Mod 2 = 0 Then

                result_ &= separation_.Replace("  ", " ").Replace(vbCrLf, "") & "'"

            Else

                result_ &= separation_ & "'"

            End If

            quotesCount_ += 1

        Next

        Return result_.Substring(0, result_.Length - 1)

    End Function

    Function GetListFormula(expression_ As String) _
                                   As List(Of String)

        Dim operand_ As String = ""

        Dim lastCharacter_ = ""

        Dim quoteFound_ = False

        Dim circumflexFound_ = False

        Dim listOperand_ As New List(Of String)

        Dim roundParenthesisCount_, truncateParenthesisCount_, rangeParenthesisCount_ As New List(Of Int32)

        Dim circumflexParenthesisCount_ As New List(Of Int32)

        For Each character_ In expression_ & ","

            Select Case character_

                Case "(", ",", ":", ")", "=", "<", ">", "+", "-", "*", "^", "/", "^", "~"

                    If Not quoteFound_ Then

                        If operand_ <> "" Then

                            Select Case operand_.ToUpper

                                Case "SI", "IF", " SI"

                                    operand_ = "if"

                                Case "REDONDEAR", " REDONDEAR"

                                    operand_ = "Round"

                                    roundParenthesisCount_.Insert(0, 0)

                                Case "TRUNCAR", " TRUNCAR"

                                    operand_ = "Truncate"

                                    truncateParenthesisCount_.Insert(0, 0)

                                Case "RANGO", " RANGO"

                                    rangeParenthesisCount_.Insert(0, 0)

                                Case "and"

                                    operand_ = " and "

                                Case "or"

                                    operand_ = " or "

                                Case Else

                                    operand_ = GetValueOperand(operand_)

                            End Select

                            If operand_ <> "" And operand_ <> " " Then

                                If operand_.ToUpper = "OR" Or operand_.ToUpper = " OR" Or operand_.ToUpper = "OR " Or operand_.ToUpper = " OR " Then
                                    operand_ = " or "
                                    listOperand_.Add(operand_ & ",NO")


                                Else

                                    If operand_.ToUpper = "AND" Or operand_.ToUpper = " AND" Or operand_.ToUpper = "AND " Or operand_.ToUpper = " AND " Then
                                        operand_ = " and "
                                        listOperand_.Add(operand_ & ",NO")

                                    Else

                                        listOperand_.Add(operand_ & ",SI")

                                    End If
                                End If



                            End If

                        End If

                        If circumflexFound_ Then

                            If character_ = "(" Then

                                If circumflexParenthesisCount_.Count = 0 Then

                                    circumflexParenthesisCount_.Insert(0, 0)

                                End If

                            Else

                                If circumflexParenthesisCount_.Count = 0 Then

                                    listOperand_.Add("),NO")

                                    circumflexFound_ = False

                                End If


                            End If

                        End If

                        If character_ = "^" Then

                            listOperand_(listOperand_.Count - 1) = "Pow,SI"

                            listOperand_.Add("(,NO")

                            listOperand_.Add(operand_ & ",SI")

                            listOperand_.Add(",,NO")

                            circumflexFound_ = True

                        Else

                            listOperand_.Add(character_ & ",NO")

                        End If

                        Select Case character_

                            Case "("

                                If roundParenthesisCount_.Count > 0 Then

                                    roundParenthesisCount_(0) += 1

                                End If

                                If truncateParenthesisCount_.Count > 0 Then

                                    truncateParenthesisCount_(0) += 1

                                End If

                                If rangeParenthesisCount_.Count > 0 Then

                                    rangeParenthesisCount_(0) += 1

                                End If

                                If circumflexParenthesisCount_.Count > 0 Then

                                    circumflexParenthesisCount_(0) += 1

                                End If

                            Case ")"

                                If roundParenthesisCount_.Count > 0 Then

                                    roundParenthesisCount_(0) -= 1

                                    If roundParenthesisCount_(0) = 0 Then

                                        listOperand_(listOperand_.Count - 1) &= ";"

                                        roundParenthesisCount_.RemoveAt(0)

                                    End If

                                End If

                                If truncateParenthesisCount_.Count > 0 Then

                                    truncateParenthesisCount_(0) -= 1

                                    If truncateParenthesisCount_(0) = 0 Then

                                        listOperand_(listOperand_.Count - 1) &= "?"

                                        truncateParenthesisCount_.RemoveAt(0)

                                    End If

                                End If

                                If rangeParenthesisCount_.Count > 0 Then

                                    rangeParenthesisCount_(0) -= 1

                                    If rangeParenthesisCount_(0) = 0 Then

                                        listOperand_(listOperand_.Count - 1) &= "}"

                                        rangeParenthesisCount_.RemoveAt(0)

                                    End If

                                End If

                                If circumflexParenthesisCount_.Count > 0 Then

                                    circumflexParenthesisCount_(0) -= 1

                                    If circumflexParenthesisCount_(0) = 0 Then

                                        circumflexParenthesisCount_.RemoveAt(0)

                                    End If

                                End If


                        End Select

                        operand_ = ""

                    Else

                        operand_ &= character_

                    End If


                Case Else

                    If character_ = "'" Then

                        quoteFound_ = Not quoteFound_

                    End If

                    If (Not character_ = vbLf And Not character_ = vbCrLf) Or quoteFound_ Then

                        If character_ <> " " Or character_ <> lastCharacter_ Then

                            operand_ &= character_

                            lastCharacter_ = character_

                        End If



                    End If


            End Select

        Next



        If listOperand_.Count > 0 Then

            listOperand_.RemoveAt(listOperand_.Count - 1)

            If listOperand_.Count = 1 Then

                operand_ = listOperand_(0).Substring(0, listOperand_(0).Length - 3)



                If NotOperatorInQuotes(operand_, " or ") Then

                    Dim positionLogic_ = operand_.IndexOf(" or ")

                    listOperand_.RemoveAt(0)

                    If positionLogic_ = 0 Then

                        listOperand_.Add(" or ,NO")

                        listOperand_.Add(operand_.Substring(4) & ",SI")

                    Else

                        listOperand_.Add(operand_.Substring(0, positionLogic_) & ",SI")

                        listOperand_.Add(" or ,NO")

                        listOperand_.Add(operand_.Substring(positionLogic_ + 4) & ",SI")

                    End If

                Else

                    If NotOperatorInQuotes(operand_, " and ") Then

                        Dim positionLogic_ = operand_.IndexOf(" and ")


                        listOperand_.RemoveAt(0)

                        If positionLogic_ = 0 Then

                            listOperand_.Add(" and ,NO")

                            listOperand_.Add(operand_.Substring(5) & ",SI")

                        Else

                            listOperand_.Add(operand_.Substring(0, positionLogic_) & ",SI")

                            listOperand_.Add(" and ,NO")

                            listOperand_.Add(operand_.Substring(positionLogic_ + 5) & ",SI")

                        End If


                    End If

                End If

            End If


        Else



                listOperand_.Add(operand_.Substring(0, operand_.Length - 1) & ",SI")

        End If

        Return listOperand_

    End Function

    Function SetValuesOperands(Of T)(originalExpression_ As String,
                                       position_ As Int32,
                                       values_ As Dictionary(Of String, T),
                                       skip_ As Boolean,
                                       Optional brackets_ As Boolean = True) _
                                       As String

        Dim expression_ As String = ""

        Dim operand_ As String = ""

        Dim word_ As String

        Dim functionOperands_ As New List(Of String)

        functionOperands_.AddRange(_operandsNCalc)

        functionOperands_.AddRange(_customFunctions)

        originalExpression_ = SeparateSentences(originalExpression_)

        For Each character_ In originalExpression_ & "?"

            word_ = character_

            Select Case character_

                Case " ", "+", "-", "*", "/", "^", "?"

                    If word_ = "?" Then

                        word_ = ""

                    End If

                    Dim doubleTemp_ As Double

                    Dim isNumber_ As Boolean

                    If Double.TryParse(operand_,
                                           doubleTemp_) OrElse
                           operand_ = "" OrElse
                           operand_ = "true" OrElse
                           operand_ = "false" Then

                        isNumber_ = True

                    Else

                        operand_ = GetValueOperand(operand_)

                        isNumber_ = Double.TryParse(operand_,
                                                       doubleTemp_)

                    End If

                    If isNumber_ Then

                        expression_ &= operand_ & word_

                    Else

                        If operand_(0) = "[" Then

                            operand_ = operand_.Substring(1,
                                                                operand_.Length - 2)

                        End If

                        Dim positionOperandFound_ = -1

                        For index_ = operand_.Length - 1 To 0 Step -1

                            If Not Char.IsNumber(operand_(index_)) OrElse
                                    operand_(index_) = "N" Then

                                positionOperandFound_ = index_

                                isNumber_ = True

                                Exit For

                            End If

                        Next

                        If isNumber_ Then

                            positionOperandFound_ = _operandsTemp.IndexOf(operand_.Substring(0,
                                                                                    positionOperandFound_ + 1))

                            If positionOperandFound_ Mod 2 <> 0 Then

                                positionOperandFound_ = -1

                            End If

                        Else

                            positionOperandFound_ = -1

                        End If

                        If positionOperandFound_ > -1 OrElse
                               functionOperands_.Contains(operand_) OrElse
                               (operand_(0) = "'" AndAlso
                               operand_(operand_.Length - 1) = "'") Then

                            expression_ &= If(operand_ = "SUMAR.SI",
                                                "[" & operand_ & "]",
                                                operand_)

                        Else

                            Dim maxencontrado_ As Boolean = False

                            Dim maxposicion_ As Int32

                            For maxposicion_ = position_ To 0 Step -1

                                If values_.ContainsKey(operand_ &
                                                            "." &
                                                            maxposicion_) Then

                                    maxencontrado_ = True

                                    Exit For

                                End If

                            Next

                            If maxencontrado_ OrElse skip_ Then

                                expression_ &= If(brackets_,
                                                     "[" &
                                                     operand_ &
                                                     "." &
                                                     maxposicion_ &
                                                     "]" &
                                                     word_,
                                                     operand_ &
                                                     "." &
                                                     maxposicion_ &
                                                     word_)

                            Else

                                If {"*", "+", "-", "/"}.Any(Function(ch_) operand_.Contains(ch_)) Then

                                    expression_ &= GetExpression(operand_,
                                                                       values_) &
                                                                       word_
                                Else
                                    expression_ &= operand_ & word_
                                End If

                            End If

                        End If

                    End If

                    operand_ = ""

                Case Else

                    operand_ &= word_

            End Select

        Next

        Return expression_

    End Function

    Function RemovePosicion(operand_ As String) As String

        Return operand_.Substring(0, operand_.LastIndexOf(".") - 1)

    End Function

    Function GetValueOperand(operand_ As String) As String

        Dim pos_ = _operandsTemp.IndexOf(operand_)

        If pos_ > -1 AndAlso
               pos_ < _operandsTemp.Count - 1 AndAlso
               pos_ Mod 2 = 0 Then

            Return _operandsTemp(pos_ + 1)

        Else

            Return operand_

        End If

    End Function

    Function FindOperators(expression_ As String) As Boolean

        Return _operators.Any(Function(ch_) expression_.Contains(ch_) And
                                                     NotOperatorInQuotes(expression_, ch_))

    End Function

    Function NotOperatorInQuotes(expresion_ As String, operator_ As String) As Boolean

        If expresion_.Length > 1 Then

            Dim posicionOperator_ = expresion_.IndexOf(operator_)

            If (expresion_.IndexOf("'") < posicionOperator_ And expresion_.Substring(1).IndexOf("'") > posicionOperator_) Or posicionOperator_ = -1 Then

                Return False

            Else

                Return True

            End If

        Else

            Return True

        End If

    End Function

    Function GetMatchesCount(Of T)(expression_ As String,
                                       values_ As Dictionary(Of String, T)) _
                                       As List(Of String)

        Dim count_ = 0

        Dim listOperand_ As New List(Of String)

        Dim operand_ = ""

        For Each character_ In expression_ & "?"

            Select Case character_

                Case " ", "+", "-", "*", "/", "^", "?"

                    Dim doubleTemp_ As Double

                    Dim isNumber_ = Double.TryParse(operand_,
                                                       doubleTemp_)

                    If Not isNumber_ Then

                        If operand_(0) = "[" Then

                            operand_ = operand_.Substring(1,
                                                                operand_.Length - 2)

                        End If

                        Dim pointLastPosition_ = operand_.LastIndexOf(".")

                        If pointLastPosition_ > -1 Then

                            Dim integerTemp_ As Int32

                            If Integer.TryParse(operand_.Substring(pointLastPosition_ + 1),
                                                    integerTemp_) Then

                                operand_ = operand_.Substring(0,
                                                                    pointLastPosition_)

                            End If

                        End If

                        listOperand_.Add(operand_)

                        If character_ <> "?" Then

                            listOperand_.Add(character_)

                        End If

                        Dim matches_ = values_.Keys.Where(Function(e) e.Contains(operand_)).Count

                        If matches_ > count_ Then

                            count_ = matches_

                        End If

                    End If

                    operand_ = ""

                Case Else

                    operand_ &= character_

            End Select

        Next

        listOperand_.Add(count_)

        Return listOperand_

    End Function

    Public Function ConvertCsvJson(csvFilePath_ As String) As String


        Dim csvData_ As String = System.IO.File.ReadAllText(csvFilePath_)

        Dim lines_ As String() = csvData_.Split(New String() {Environment.NewLine},
                                                    StringSplitOptions.RemoveEmptyEntries)

        Dim headers_ As String() = lines_(0).Split(","c)

        Dim result_ As New List(Of Dictionary(Of String, String))

        For fila_ As Integer = 1 To lines_.Length - 1

            Dim dictionaryLines_ As New Dictionary(Of String, String)

            Dim currentLine_ As String() = lines_(fila_).Split(","c)

            For column_ As Integer = 0 To headers_.Length - 1

                dictionaryLines_.Add(headers_(column_),
                                         currentLine_(column_))

            Next

            result_.Add(dictionaryLines_)

        Next

        Return JsonConvert.SerializeObject(result_)

    End Function

    Public Function DecodeAscii(exprescionAscii_ As String) As String

        Dim valorAscci_ As String = ""

        Dim expressionResultado_ = ""

        Dim comienzaAscci = False

        For Each caracter_ In exprescionAscii_

            If caracter_ = "[" Then

                valorAscci_ = ""

                comienzaAscci = True

            Else

                If caracter_ = "]" Then

                    expressionResultado_ &= Chr(valorAscci_)

                    comienzaAscci = False

                Else

                    If comienzaAscci Then

                        valorAscci_ &= caracter_

                    End If

                End If

            End If

        Next

        Return expressionResultado_

    End Function

    Private Function RunFunctionHandler(values_ As Dictionary(Of String, Object)) As EvaluateFunctionHandler

        Dim procedure_ = Sub(functionName_ As String,
                                                    functionParameters_ As FunctionArgs)

                             Dim result_ As Double = 0

                             Dim resultIsDouble_ As Boolean = True

                             Select Case functionName_.ToUpper

                                 Case "ASIGNAR"

                                     Dim firstParameter_ = functionParameters_.
                                                            Parameters.
                                                            First.
                                                            Evaluate

                                     Dim secondParameter_ = functionParameters_.
                                                            Parameters.
                                                            Last.
                                                            Evaluate

                                     _operandsTemp.Add(firstParameter_.replace("'", ""))

                                     _operandsTemp.Add(secondParameter_)

                                     result_ = 1

                                 Case "O"

                                     Dim resultO_ = ""

                                     For Each parameter_ In functionParameters_.
                                                                   Parameters

                                         resultO_ &= parameter_.
                                                          ParsedExpression.
                                                          ToString & " or "

                                     Next

                                     resultO_ = resultO_.Substring(0, resultO_.Length - 4)

                                     Dim expression_ = New NCalc.Expression(resultO_)

                                     expression_.Parameters = values_

                                     functionParameters_.Result = expression_.Evaluate

                                     resultIsDouble_ = False

                                 Case "RED"

                                     Dim firstParameter_ = functionParameters_.
                                                            Parameters.
                                                            First

                                     Dim expression_ = New NCalc.Expression("Round(" &
                                                                              (firstParameter_.Evaluate() - 0.01).
                                                                              ToString &
                                                                              ",0)")

                                     result_ = CDbl(expression_.Evaluate)

                                 Case "ROOM"

                                     Dim paramPosition_ = 0

                                     Dim firstParameter_ = functionParameters_.Parameters.First

                                     Dim list_ = firstParameter_.ParsedExpression.ToString.Split(New Char() {" "c})

                                     Dim cubeName_ = list_(0)

                                     Dim operation_ = ""

                                     Dim parameters_ As New List(Of String)

                                     Dim cube_ As ICubeController = New CubeController

                                     parameters_ = cube_.GetFormula(cubeName_).ObjectReturned

                                     operation_ = parameters_(0)

                                     parameters_.RemoveAt(0)

                                     For Each parametro_ In parameters_

                                         If paramPosition_ > 0 AndAlso
                                                    paramPosition_ < functionParameters_.Parameters.Count Then

                                             Dim fileCsv_ = functionParameters_.
                                                                       Parameters(paramPosition_).
                                                                       Evaluate.
                                                                       ToString

                                             If fileCsv_.IndexOf(".csv") >= 0 Then

                                                 'ConvertJsonCsv(operacion_,
                                                 '                   Archivocsv_)

                                                 Exit For

                                             Else

                                                 If values_.ContainsKey(parametro_.ToString & ".0") Then

                                                     values_(parametro_.ToString & ".0") = functionParameters_.
                                                                                                          Parameters(paramPosition_).
                                                                                                          Evaluate

                                                 Else

                                                     values_.Add(parametro_.ToString & ".0",
                                                                                functionParameters_.
                                                                                Parameters(paramPosition_).
                                                                                Evaluate)

                                                 End If

                                             End If


                                         End If

                                         paramPosition_ += 1

                                     Next

                                     Dim expressionRoom_ = New NCalc.Expression(GetExpression(operation_,
                                                                                                  values_))

                                     expressionRoom_.Parameters = values_

                                     AddHandler expressionRoom_.EvaluateFunction, RunFunctionHandler(values_)

                                     Dim resultRoom_ = expressionRoom_.Evaluate

                                     If Not Double.TryParse(resultRoom_, result_) Then

                                         functionParameters_.Result = resultRoom_

                                         resultIsDouble_ = False

                                     End If

                                 Case "SETROOM"

                                     Dim firstParameter_ = functionParameters_.
                                                            Parameters.
                                                            First

                                     Dim list_ = firstParameter_.
                                                          ParsedExpression.
                                                          ToString.
                                                          Split(New Char() {" "c})

                                     Dim cubeName_ = list_(0)

                                     Dim rules_ = functionParameters_.
                                                          Parameters(1).
                                                          Evaluate.
                                                          ToString

                                     Dim contenttype_ = "formulas"

                                     If rules_.IndexOf(".csv") > -1 Then

                                         contenttype_ = "csv"

                                         rules_ = ConvertCsvJson(functionParameters_.
                                                                         Parameters(1).
                                                                         Evaluate.
                                                                         ToString)

                                     End If

                                     Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
                                              {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                                         Dim endPoint_ = Sax.SaxStatements.GetInstance().GetEndPoint("project", 13)

                                         Dim operationsDB_ As IMongoCollection(Of Room) =
                                                     _enlaceDatos.GetMongoCollection(Of Room)(New MongoClient("mongodb://" & endPoint_.ip & ":" & endPoint_.port), "CUBEROOMS")



                                         Dim updateDefinition_ = Builders(Of Room).
                                                                                     Update.
                                                                                     Set(Function(e) e.rules, rules_).
                                                                                     Set(Function(e) e.contenttype, contenttype_)

                                         Dim filter_ = Builders(Of Room).
                                                               Filter.
                                                               Eq(Function(e) e.roomname,
                                                                              cubeName_.Substring(1,
                                                                                                cubeName_.Length - 2))

                                         operationsDB_.UpdateOne(filter_,
                                                                    updateDefinition_)

                                     End Using

                                     MsgBox("ROOM UPDATED")

                                 Case "SUMAR"

                                     For Each parametro_ In functionParameters_.Parameters

                                         result_ += CDbl(parametro_.Evaluate())

                                     Next

                                 Case "SUMAR.SI"

                                     Dim lastParameter_ = functionParameters_.
                                                           Parameters.
                                                           Last

                                     Dim listParameter_ = lastParameter_.
                                                          ParsedExpression.
                                                          ToString.
                                                          Split(New Char() {" "c})

                                     Dim operandName_ = listParameter_(0)

                                     If operandName_.IndexOf("[") > 0 Then

                                         operandName_ = listParameter_(0).
                                                           Substring(2,
                                                                     listParameter_(0).
                                                                     LastIndexOf(".") - 2)

                                     End If

                                     Dim valueOperand_ = listParameter_(2)

                                     If valueOperand_.IndexOf("[") > 0 Then

                                         valueOperand_ = listParameter_(2).
                                                          Substring(2,
                                                                    listParameter_(2).
                                                                    LastIndexOf(".") - 2)

                                     End If

                                     Dim operator_ = listParameter_(1)

                                     Dim doubleParse_ As Double

                                     operandName_ = IIf(Double.TryParse(operandName_,
                                                                           doubleParse_),
                                                           doubleParse_,
                                                           operandName_)

                                     valueOperand_ = IIf(Double.TryParse(valueOperand_,
                                                                          doubleParse_),
                                                          doubleParse_,
                                                          valueOperand_)

                                     Dim operandCount_ = 0

                                     For Each parameter_ In functionParameters_.
                                                                    Parameters.
                                                                    Take(functionParameters_.Parameters.Count - 1)

                                         Dim generalName_ = SetValuesOperands(operandName_,
                                                                                            operandCount_,
                                                                                            values_,
                                                                                            False)

                                         Dim generalValue = SetValuesOperands(valueOperand_,
                                                                                          operandCount_,
                                                                                          values_,
                                                                                          False)

                                         Dim expresionString = generalName_ &
                                                                       operator_ &
                                                                       generalValue

                                         Dim auxName_ = SetValuesOperands(operandName_,
                                                                                        operandCount_,
                                                                                        values_,
                                                                                        False,
                                                                                        False)

                                         Dim auxValue_ = SetValuesOperands(valueOperand_,
                                                                                       operandCount_,
                                                                                       values_,
                                                                                       False,
                                                                                       False)

                                         Dim evaluate_ As Boolean = False

                                         If values_.Keys.Contains(auxName_) Then

                                             evaluate_ = True

                                         Else

                                             auxName_ = SetValuesOperands(operandName_,
                                                                                        0,
                                                                                        values_,
                                                                                        False,
                                                                                        False)

                                             generalName_ = SetValuesOperands(operandName_,
                                                                                            0,
                                                                                            values_,
                                                                                            False)

                                             expresionString = generalName_ &
                                                                       operator_ &
                                                                       generalValue

                                             If values_.
                                                        Keys.
                                                        Contains(auxName_) Then

                                                 evaluate_ = True

                                             Else

                                                 If Double.TryParse(auxName_,
                                                                            doubleParse_) Then

                                                     evaluate_ = True

                                                 Else

                                                     evaluate_ = False

                                                 End If


                                             End If

                                         End If

                                         If values_.
                                                    Keys.
                                                    Contains(auxValue_) Then

                                             evaluate_ = True

                                         Else

                                             auxValue_ = SetValuesOperands(valueOperand_,
                                                                                       0,
                                                                                       values_,
                                                                                       False,
                                                                                       False)

                                             generalValue = SetValuesOperands(valueOperand_,
                                                                                          0,
                                                                                          values_,
                                                                                          False)

                                             expresionString = generalName_ &
                                                                       operator_ &
                                                                       generalValue

                                             If values_.
                                                        Keys.
                                                        Contains(auxValue_) Then

                                                 evaluate_ = True
                                             Else

                                                 If Double.TryParse(auxValue_,
                                                                            doubleParse_) Then

                                                     evaluate_ = True

                                                 Else

                                                     evaluate_ = False

                                                 End If


                                             End If

                                         End If


                                         If evaluate_ Then

                                             Dim expressionSI_ = New NCalc.Expression(expresionString)

                                             If Not Double.TryParse(auxName_,
                                                                            doubleParse_) Then

                                                 expressionSI_.Parameters(auxName_) = values_(auxName_)

                                             End If

                                             If Not Double.TryParse(auxValue_,
                                                                            doubleParse_) Then

                                                 expressionSI_.Parameters(auxValue_) = values_(auxValue_)

                                             End If

                                             evaluate_ = expressionSI_.Evaluate()

                                         End If


                                         If evaluate_ Then

                                             result_ += CDbl(parameter_.Evaluate())

                                         End If

                                         operandCount_ += 1

                                     Next

                                 Case "TRUNC"

                                     Dim firstParameter_ = functionParameters_.Parameters.First

                                     Dim expression_ = New NCalc.Expression("Truncate((" &
                                                                              firstParameter_.
                                                                              Evaluate().
                                                                              ToString &
                                                                              ")*10000)/10000")

                                     result_ = CDbl(expression_.Evaluate)


                                 Case "Y"

                                     Dim resultY_ = ""

                                     For Each parameter_ In functionParameters_.
                                                                   Parameters

                                         resultY_ &= parameter_.
                                                          ParsedExpression.
                                                          ToString & " and "

                                     Next

                                     resultY_ = resultY_.Substring(0, resultY_.Length - 5)

                                     Dim expression_ = New NCalc.Expression(resultY_)

                                     functionParameters_.Result = expression_.Evaluate

                                     resultIsDouble_ = False

                             End Select

                             If _customFunctions.
                                        Any(Function(ch) functionName_.ToUpper = ch) And resultIsDouble_ Then

                                 functionParameters_.Result = result_

                             End If

                         End Sub

        Return procedure_

    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        Throw New NotImplementedException()
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

#End Region

End Class





