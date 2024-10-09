Imports System.IO
Imports System.Linq.Expressions
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text.RegularExpressions
Imports Cube.ValidatorReport
Imports Cube.Validators
Imports gsol.krom
Imports MongoDB.Bson
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

    Private _validFields As List(Of String)

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

        _customFunctions = New List(Of String) From {"ABS",
                                                     "AHORA",
                                                     "ASIGNAR",
                                                     "BUSCARV",
                                                     "COINCIDIR",
                                                     "CONCATENAR",
                                                     "DENTRO",
                                                     "DICCIONARIO",
                                                     "EN",
                                                     "ENLISTAR",
                                                     "ESBLANCO",
                                                     "ESPACIOS",
                                                     "EXTRAE",
                                                     "HOY",
                                                     "LARGO",
                                                     "MEZCLAR",
                                                     "NO",
                                                     "O",
                                                     "OBTENERVALOR",
                                                     "RANGO",
                                                     "RED",
                                                     "REDONDEAR",
                                                     "ROOM",
                                                     "SECUENCIA",
                                                     "SETROOM",
                                                     "SI",
                                                     "SUMAR",
                                                     "SUMAR.SI",
                                                     "SUSTITUIR",
                                                     "TRUNC",
                                                     "TRUNCAR",
                                                     "Y"}

        _operandsNCalc = New List(Of String) From {"if",
                                                    "Round",
                                                    "Abs",
                                                    "N",
                                                    "Truncate",
                                                    "Sqrt",
                                                    "not",
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
                                                "&",
                                                " and ",
                                                " or ",
                                                "!",
                                                "not "
                                                }

        _operandsTemp = SetOperands()



        _reports = New ValidatorReport

    End Sub

    Sub SetValidField(field_ As String) Implements IMathematicalInterpreter.SetValidField

        _validFields.Add(field_)

    End Sub

    Public Function ExistFields(expresion_ As String, fields_ As List(Of String)) As Boolean Implements IMathematicalInterpreter.ExistFields

        Dim paramErrorName_ As New List(Of String)

        Dim result_ As Boolean = True

        If _validFields Is Nothing Then

            _validFields = New List(Of String)

        End If

        For Each fied_ In fields_

            Dim fieldName_ = fied_

            Dim pointIndex = fieldName_.LastIndexOf(".")

            If pointIndex <> -1 Then

                Dim integerParse_ As Int16

                If Integer.TryParse(fieldName_.Substring(pointIndex + 1), integerParse_) Then

                    fieldName_ = fieldName_.Substring(0, pointIndex)

                End If

                If fieldName_.IndexOf(".") <> -1 Then

                    If _validFields.IndexOf(fieldName_.ToUpper) = -1 Then

                        result_ = False

                        paramErrorName_.Add(fieldName_)

                    End If

                End If

            End If



        Next


        _reports = New ValidatorReport

        If paramErrorName_.Count > 0 Then

            Dim fieldErros_ As String = ""

            For Each field_ In paramErrorName_

                fieldErros_ &= field_ & Chr(13)

            Next

            _reports.SetHeaderReport("Nombre de campos inválidos",
                                                 DateTime.Now,
                                                AdviceTypesReport.Alert,
                                                 AdviceTypesReport.Alert,
                                                fieldErros_,
                                                fieldErros_,
                                                expresion_, "")

            _reports.ShowMessageError(0)


        End If

        Return result_

    End Function

    Public Function RunExpression(Of T)(expression_ As String,
                                         constantValues_ As Dictionary(Of String,
                                                                         T),
                                        Optional preferIndex_ As Int32? = Nothing) As T _
                                        Implements IMathematicalInterpreter.RunExpression

        If constantValues_ Is Nothing Then

            constantValues_ = New Dictionary(Of String, T)

        End If

        preferIndex_ = If(preferIndex_, 0)

        Dim newExpression_ = GetExpression(expression_.Replace(Chr(34), "'").Replace("\r", " "),
                                           constantValues_, preferIndex_)

        Dim paramValues_ As New Dictionary(Of String, Object)

        Dim paramErrorName_ As New List(Of String)

        For Each constanvalue_ In constantValues_

            Dim fieldName_ = constanvalue_.Key

            Dim pointIndex = fieldName_.LastIndexOf(".")

            If pointIndex <> -1 Then

                Dim integerParse_ As Int16

                If Integer.TryParse(fieldName_.Substring(pointIndex + 1), integerParse_) Then

                    fieldName_ = fieldName_.Substring(0, pointIndex)

                End If

                If _validFields.IndexOf(fieldName_.ToUpper) = -1 And fieldName_.IndexOf(".") <> -1 Then

                    paramErrorName_.Add(fieldName_)

                Else

                    Dim doubleParse_ As Double = 0

                    If constanvalue_.Value Is Nothing Then

                        paramValues_.Add(constanvalue_.Key, "")

                    Else

                        If TypeOf constanvalue_.Value Is List(Of String) Then

                            paramValues_.Add(constanvalue_.Key, constanvalue_.Value)

                        Else

                            If Double.TryParse(constanvalue_.Value.ToString, doubleParse_) Then

                                If doubleParse_ < 0 Or constanvalue_.Value.ToString.IndexOf(".") <> -1 Then

                                    paramValues_.Add(constanvalue_.Key, doubleParse_)

                                Else

                                    If constanvalue_.Value.ToString(0) = "0" Then

                                        paramValues_.Add(constanvalue_.Key, constanvalue_.Value)

                                    Else

                                        paramValues_.Add(constanvalue_.Key, doubleParse_)

                                    End If

                                End If

                            Else

                                If TypeOf constanvalue_.Value Is String Then

                                    paramValues_.Add(constanvalue_.Key, constanvalue_.Value.ToString.Replace(Chr(160), Chr(32)))

                                Else

                                    paramValues_.Add(constanvalue_.Key, constanvalue_.Value)

                                End If

                            End If

                        End If

                    End If


                End If

            End If

        Next

        Dim result_

        _reports = New ValidatorReport

        If paramErrorName_.Count > 0 Then

            Dim fieldErros_ As String = ""

            For Each field_ In paramErrorName_

                fieldErros_ &= field_ & Chr(13)

            Next

            _reports.SetHeaderReport("Nombre de campos inválidos",
                                                 DateTime.Now,
                                                AdviceTypesReport.Alert,
                                                 AdviceTypesReport.Alert,
                                                fieldErros_,
                                                fieldErros_,
                                                expression_, newExpression_)

            _reports.ShowMessageError(0)

            result_ = ""

        Else

            _expressionNCalc = New NCalc.Expression(newExpression_.Replace("&", " and ")) With {.Parameters = paramValues_}

            AddHandler _expressionNCalc.EvaluateFunction, RunFunctionHandler(paramValues_)

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

                    _reports.ShowMessageError(0)

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

                        _reports.ShowMessageError(0)

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

                _reports.ShowMessageError(0)

                'MsgBox(ex_.HResult & Chr(13) & _reports.title & Chr(13) & newExpression_)

                result_ = ""

            Catch ex_ As Exception

                If Array.FindAll(expression_.ToCharArray(), Function(ch) ch = "'").Count Mod 2 = 1 Then

                    _reports.SetHeaderReport("Falta una comilla(') de cierre",
                                             DateTime.Now,
                                             AdviceTypesReport.Alert,
                                             AdviceTypesReport.Alert,
                                             "Falta una comilla(') de cierre",
                                             "-2146232832",
                                             expression_,
                                             newExpression_)

                Else


                    _reports.SetHeaderReport(ex_.Message,
                                             DateTime.Now,
                                             AdviceTypesReport.Alert,
                                             AdviceTypesReport.Alert,
                                             ex_.Message,
                                             ex_.HResult,
                                             expression_,
                                             newExpression_)

                End If

                _reports.ShowMessageError(0)

                result_ = ""

            End Try

        End If


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

    Public Function GetReportFul() As ValidatorReport Implements IMathematicalInterpreter.GetReportFull

        Return _reports

    End Function

    Private Sub SetReport(ByRef report_ As ValidatorReport)

        report_ = CType(_reports.Clone(), ValidatorReport)

    End Sub

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

        Dim listOperand_ = GetListFormula(expression_.Replace("[13]", Chr(13)).Replace(Chr(34), "'"))

        listOperand_ = GetListOperandOperator(listOperand_)

        Dim numero_ As Double

        listOperand_.RemoveAll(Function(ch) Not ch.Contains(",SI") Or _customFunctions.Contains(ch.Substring(0, ch.Length - 3)) Or {"true", "false"}.Contains(ch.Substring(0, ch.Length - 3)) Or _operandsNCalc.Contains(ch.Substring(0, ch.Length - 3)) Or Double.TryParse(ch.Substring(0, ch.Length - 3), numero_))

        listOperand_ = listOperand_.Select(Function(e) If(e.Length > 3, e.Substring(0, e.Length - 3), e)).ToList

        listOperand_.RemoveAll(Function(ch) _operandsTemp.Contains(ch) And _operandsTemp.IndexOf(ch) Mod 2 = 0)

        Dim listOperadTemp_ As New List(Of String)

        For Each operand_ In listOperand_

            Dim index_ As Int16 = 0

            Dim number_ As Int32

            Dim found_ = False

            While index_ < operand_.Length And Not found_


                If Int32.TryParse(operand_(index_), number_) And index_ Mod 2 = 0 Then

                    found_ = True

                Else

                    index_ += 1

                End If


            End While

            If found_ Then

                Dim initial_ = operand_.Substring(index_, operand_.Length - index_)

                If Int32.TryParse(initial_, number_) Then

                    Dim position_ = _operandsTemp.IndexOf(operand_.Substring(0, index_))

                    If position_ > -1 Then

                        Dim operandNew_ = _operandsTemp(position_ + 1)

                        For index_ = 0 To initial_ - 1


                            If listOperadTemp_.IndexOf(operandNew_) = -1 Then

                                listOperadTemp_.Add(operandNew_ & "." & index_)

                            End If

                        Next

                    Else

                        found_ = False

                    End If

                Else

                    found_ = False

                End If


            End If

            If Not found_ Then

                operand_ = operand_.Replace(" ", "").Replace(vbCrLf, "").Replace(vbLf, "").Replace(Chr(34), "'").Replace("[13]", Chr(13))

                If Not listOperadTemp_.Contains(operand_) Then

                    If operand_.Length > 0 Then

                        If operand_(0) <> "'" And operand_ <> "SI" And operand_ <> "RED" Then 'And operand_.IndexOf(".") > 0 Then

                            listOperadTemp_.Add(operand_)

                        End If


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
                                  values_ As Dictionary(Of String, T),
                                  Optional prederIndex_ As Int32 = 0) _
                                  As String

        Dim customFunctions_ As New List(Of String)

        customFunctions_.AddRange(_operandsNCalc)

        customFunctions_.AddRange(_customFunctions)

        Dim finalExpression_ As String = ""

        Dim operandos_ = _operandsTemp

        Dim parenthesisCount_, parenthesisCountTruncate_ As New List(Of String)

        expression_ = SeparateSentences(expression_.Replace(Chr(34), "'").Replace("[13]", ""))

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
                                                                         prederIndex_,
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

                                Case "not"

                                    operand_ = "not "


                                Case Else

                                    operand_ = GetValueOperand(operand_).Replace(Chr(34), "'")

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

                                        If operand_.ToUpper = "NOT" Or operand_.ToUpper = " NOT" Or operand_.ToUpper = "NOT " Or operand_.ToUpper = " NOT " Then
                                            operand_ = "not "
                                            listOperand_.Add(operand_ & ",NO")

                                        Else
                                            listOperand_.Add(operand_ & ",SI")

                                        End If


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

                    If (Not character_ = vbLf And Not character_ = vbCrLf And Not character_ = " " And Not character_ = Chr(160) And Not character_ = Chr(13) And Not character_ = Chr(10)) Or quoteFound_ Then

                        'If character_ <> " " Or character_ <> lastCharacter_ Then

                        '    operand_ &= character_

                        '    lastCharacter_ = character_

                        'End If

                        operand_ &= character_

                    End If


            End Select

        Next

        If listOperand_.Count > 0 Then

            listOperand_.RemoveAt(listOperand_.Count - 1)

            If listOperand_.Count = 1 Then

                operand_ = listOperand_(0).Substring(0, listOperand_(0).Length - 3)


                If NotOperatorInQuotes(operand_, " or ") Then

                    Dim positionLogic_ = operand_.IndexOf(" or ")

                    If positionLogic_ <> -1 Then

                        listOperand_.RemoveAt(0)

                        If positionLogic_ = 0 Then

                            listOperand_.Add(" or ,NO")

                            listOperand_.Add(operand_.Substring(4) & ",SI")

                        Else


                            listOperand_.Add(operand_.Substring(0, positionLogic_) & ",SI")

                            listOperand_.Add(" or ,NO")

                            listOperand_.Add(operand_.Substring(positionLogic_ + 4) & ",SI")


                        End If

                    End If


                Else

                    If NotOperatorInQuotes(operand_, " and ") Then

                        Dim positionLogic_ = operand_.IndexOf(" and ")

                        If positionLogic_ <> -1 Then


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

        Dim quoteFound = False

        Dim word_ As String

        Dim functionOperands_ As New List(Of String)

        functionOperands_.AddRange(_operandsNCalc)

        functionOperands_.AddRange(_customFunctions)

        originalExpression_ = SeparateSentences(originalExpression_)
        For Each character_ In originalExpression_ & "?"

            word_ = character_

            If word_ = "'" Then

                quoteFound = Not quoteFound

            End If

            If quoteFound Then

                expression_ &= word_

            Else

                Select Case character_

                    Case " ", "+", "-", "*", "/", "^", "?", "(", ")"

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
                                If word_ = "(" Then

                                    expression_ &= word_

                                End If

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

                                    If {"*", "+", "-", "/", ")", "("}.Any(Function(ch_) operand_.Contains(ch_)) Then

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

            End If



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
                                                     NotOperatorInQuotes(expression_, ch_) And ch_.ToUpper <> "A" And ch_.ToUpper <> "N" And ch_.ToUpper <> "D" And ch_.ToUpper <> "O" And ch_.ToUpper <> "R" And ch_.ToUpper <> " ")

    End Function

    Function NotOperatorInQuotes(expresion_ As String, operator_ As String) As Boolean

        If expresion_.Length > 1 Then

            Dim posicionOperator_ = expresion_.IndexOf(operator_)

            Dim initialQuote_ = expresion_.IndexOf("'")

            Dim finalQuote_ = expresion_.Substring(1).IndexOf("'") + initialQuote_ + 1


            If (initialQuote_ < posicionOperator_ And finalQuote_ > posicionOperator_) Or posicionOperator_ = -1 Then

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

                Case " ", "+", "-", "*", "/", "^", "?", "(", ")"

                    Dim doubleTemp_ As Double

                    Dim isNumber_ = Double.TryParse(operand_,
                                                       doubleTemp_)


                    If Not isNumber_ Then

                        If operand_ <> "" Then

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
                        End If

                        If character_ <> "?" Then

                            listOperand_.Add(character_)

                        End If

                        If operand_ <> "" Then
                            Dim matches_ = values_.Keys.Where(Function(e) e.Contains(operand_)).Count

                            If matches_ > count_ Then

                                count_ = matches_

                            End If
                        End If

                    Else

                        listOperand_.Add(operand_)
                        If character_ <> "?" Then

                            listOperand_.Add(character_)

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

    Sub SetValidFields(campos_ As List(Of String)) Implements IMathematicalInterpreter.SetValidFields

        _validFields = campos_

    End Sub

    Function GetValidFields() As List(Of String) Implements IMathematicalInterpreter.GetValidFields

        Return _validFields

    End Function

    Function FormatToTwoDigits(number_ As String) As String

        If number_.Length = 1 Then

            Return "0" & number_

        Else

            Return number_.Substring(number_.Length - 2, 2)

        End If


    End Function

    Private Function RunFunctionHandler(values_ As Dictionary(Of String, Object)) As EvaluateFunctionHandler

        Dim procedure_ = Sub(functionName_ As String,
                                                    functionParameters_ As FunctionArgs)

                             Dim result_ As Double = 0

                             Dim resultIsDouble_ As Boolean = True

                             Select Case functionName_.ToUpper

                                 Case "ABS"

                                     Dim firstParameter_ As String = functionParameters_.
                                                            Parameters.
                                                            First.
                                                            Evaluate()

                                     result_ = Math.Abs(Double.Parse(firstParameter_))

                                 Case "AHORA"

                                     Dim countParameters_ = functionParameters_.
                                                          Parameters.Count


                                     Dim now_ = Date.Today.Year & "/" & FormatToTwoDigits(Date.Today.Month) & "/" & FormatToTwoDigits(Date.Today.Day) & " " &
                                               FormatToTwoDigits(DateTime.Now.Hour) & ":" & FormatToTwoDigits(DateTime.Now.Minute) & ":" & FormatToTwoDigits(DateTime.Now.Second)


                                     If countParameters_ > 0 Then

                                         now_ = now_.Replace("/", functionParameters_.
                                                          Parameters.First.Evaluate)


                                     End If


                                     functionParameters_.Result = now_

                                     resultIsDouble_ = False

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

                                     functionParameters_.Result = "VALOR ASIGNADO, " & Chr(34) & firstParameter_ & Chr(34) & " SE RECONOCE COMO " & Chr(34) & secondParameter_ & Chr(34)

                                     resultIsDouble_ = False

                                 Case "BUSCARV"

                                     Dim firstParameter_ = functionParameters_.
                                                            Parameters.
                                                            First.
                                                            Evaluate

                                     Dim secondParameter_ = functionParameters_.
                                                            Parameters(1).
                                                            Evaluate

                                     Dim thirdParameter_ = functionParameters_.
                                                            Parameters(2).
                                                            Evaluate
                                     Dim fourthParameter_ = functionParameters_.
                                                            Parameters(3).
                                                            Evaluate
                                     Dim search_ = functionParameters_.
                                                            Parameters.
                                                            Last.
                                                            Evaluate

                                     Dim value_, text_ As New List(Of String)

                                     Dim position_ As Int32 = 0

                                     For Each element_ In fourthParameter_

                                         If element_.Contains(firstParameter_) Then

                                             value_.Add(secondParameter_(position_))

                                             text_.Add(thirdParameter_(position_))

                                         End If

                                         position_ += 1

                                     Next

                                     Dim listresult_ As New List(Of List(Of String))

                                     listresult_.Add(value_)

                                     listresult_.Add(text_)

                                     functionParameters_.Result = listresult_

                                     resultIsDouble_ = False


                                 Case "COINCIDIR"

                                     Dim sentencesIn_ As String = ""

                                     Dim found_ As Boolean = False

                                     Dim paramterLast_ = functionParameters_.Parameters.Last.Evaluate

                                     Select Case paramterLast_

                                         Case 0

                                             For Each parameter_ In functionParameters_.Parameters.Take(functionParameters_.Parameters.Count - 1)

                                                 If TypeOf parameter_.Evaluate Is List(Of String) Then

                                                     For Each minilist_ In parameter_.Evaluate

                                                         sentencesIn_ &= "'" & minilist_ & "',"

                                                     Next

                                                 Else

                                                     sentencesIn_ &= "'" & parameter_.Evaluate & "',"

                                                 End If

                                             Next

                                             sentencesIn_ = "in(" & sentencesIn_.Substring(0, sentencesIn_.Length - 1) & ")"

                                         Case 1

                                             Dim first_ As String = functionParameters_.Parameters.First.Evaluate

                                             For Each parameter_ In functionParameters_.Parameters.Take(functionParameters_.Parameters.Count - 1)

                                                 If TypeOf parameter_.Evaluate Is List(Of String) Then

                                                     For Each minilist_ In parameter_.Evaluate

                                                         If first_ > minilist_ Then

                                                             found_ = True
                                                         End If

                                                     Next

                                                 Else


                                                     If first_ > parameter_.Evaluate Then

                                                         found_ = True

                                                     End If

                                                 End If

                                             Next

                                         Case -1


                                             Dim first_ As String = functionParameters_.Parameters.First.Evaluate

                                             For Each parameter_ In functionParameters_.Parameters.Take(functionParameters_.Parameters.Count - 1)

                                                 If TypeOf parameter_.Evaluate Is List(Of String) Then

                                                     For Each minilist_ In parameter_.Evaluate

                                                         If first_ < minilist_ Then

                                                             found_ = True
                                                         End If

                                                     Next

                                                 Else


                                                     If first_ < parameter_.Evaluate Then

                                                         found_ = True

                                                     End If

                                                 End If

                                             Next

                                         Case Else

                                             sentencesIn_ = "in(" & sentencesIn_.Substring(0, sentencesIn_.Length - 1) & ")"


                                     End Select

                                     If paramterLast_ = 0 OrElse paramterLast_ = 1 OrElse paramterLast_ = -1 Then

                                         Dim expression_ = New NCalc.Expression(sentencesIn_)


                                         expression_.Parameters = values_

                                         AddHandler expression_.EvaluateFunction, RunFunctionHandler(values_)


                                         functionParameters_.Result = expression_.Evaluate

                                     Else

                                         functionParameters_.Result = found_

                                     End If



                                     resultIsDouble_ = False

                                 Case "CONCATENAR"

                                     Dim sentencesIn_ As String = ""

                                     For Each parameter_ In functionParameters_.Parameters

                                         Dim valor_ = parameter_.Evaluate

                                         If TypeOf valor_ Is List(Of String) Then

                                             For Each minilist_ In valor_

                                                 sentencesIn_ &= minilist_

                                             Next

                                         Else

                                             sentencesIn_ &= parameter_.Evaluate

                                         End If

                                     Next


                                     functionParameters_.Result = sentencesIn_

                                     resultIsDouble_ = False

                                 Case "DENTRO"

                                     Dim encontrado_ As Boolean = False

                                     Dim sentencesIn_ As String = ""

                                     Dim elemento_ As String = functionParameters_.Parameters.First.Evaluate

                                     For Each parameter_ In functionParameters_.Parameters.Skip(1)

                                         Dim valor_ = parameter_.Evaluate

                                         If TypeOf valor_ Is List(Of String) Then

                                             Dim valorAux_ As List(Of String) = valor_

                                             'For Each minilist_ In parameter_.Evaluate

                                             '    sentencesIn_ &= "'" & minilist_ & "',"

                                             'Next
                                             If valorAux_.IndexOf(elemento_) > -1 Then

                                                 encontrado_ = True

                                                 Exit For

                                             End If


                                         Else

                                             sentencesIn_ &= "'" & parameter_.Evaluate & "',"

                                         End If

                                     Next

                                     If encontrado_ OrElse sentencesIn_ = "" Then

                                         functionParameters_.Result = encontrado_

                                     Else



                                         sentencesIn_ = "in('" & elemento_ & "'," & sentencesIn_.Substring(0, sentencesIn_.Length - 1) & ")"

                                         Dim expression_ = New NCalc.Expression(sentencesIn_)


                                         expression_.Parameters = values_

                                         AddHandler expression_.EvaluateFunction, RunFunctionHandler(values_)

                                         functionParameters_.Result = expression_.Evaluate

                                     End If

                                     resultIsDouble_ = False

                                 Case "DICCIONARIO"


                                     Dim elements_ As List(Of String) = functionParameters_.Parameters.First.Evaluate

                                     Dim elementSecond_ = functionParameters_.Parameters(1).Evaluate

                                     If functionParameters_.Parameters.Count = 3 Then

                                         Dim separatorList_ As String = functionParameters_.Parameters(2).Evaluate

                                         Dim dictionary_ As New Dictionary(Of String, List(Of String))

                                         If TypeOf elementSecond_ Is String Then

                                             Dim separator_ As String = elementSecond_

                                             For Each element_ In elements_

                                                 Dim keyValue_ = element_.Split(separator_)

                                                 Dim key_ = keyValue_(0).Trim(" ")

                                                 Dim value_ = keyValue_(1).Trim(" ")

                                                 dictionary_.Add(key_, value_.Split(separatorList_).ToList)

                                             Next

                                         Else

                                             Dim index_ = 0

                                             For Each element_ In elements_

                                                 Dim key_ = element_

                                                 Dim value_ As String = elementSecond_(index_)

                                                 dictionary_.Add(key_, value_.Split(separatorList_).ToList)

                                                 index_ += 1

                                             Next

                                         End If

                                         functionParameters_.Result = dictionary_

                                     Else


                                         Dim dictionary_ As New Dictionary(Of String, String)

                                         If TypeOf elementSecond_ Is String Then

                                             Dim separator_ As String = elementSecond_


                                             For Each element_ In elements_

                                                 Dim keyValue_ = element_.Split(separator_)

                                                 Dim key_ = keyValue_(0).Trim(" ")

                                                 Dim value_ = keyValue_(1).Trim(" ")

                                                 dictionary_.Add(key_, value_)

                                             Next

                                         Else

                                             Dim index_ = 0

                                             For Each element_ In elements_

                                                 Dim key_ = element_

                                                 dictionary_.Add(key_, elementSecond_(index_))

                                                 index_ += 1

                                             Next

                                         End If

                                         functionParameters_.Result = dictionary_

                                     End If


                                     resultIsDouble_ = False

                                 Case "EN"

                                     Dim sentencesIn_ As String = ""

                                     For Each parameter_ In functionParameters_.Parameters

                                         If TypeOf parameter_.Evaluate Is List(Of String) Then

                                             For Each minilist_ In parameter_.Evaluate

                                                 sentencesIn_ &= "'" & minilist_ & "',"

                                             Next

                                         Else

                                             sentencesIn_ &= "'" & parameter_.Evaluate & "',"

                                         End If

                                     Next

                                     sentencesIn_ = "in(" & sentencesIn_.Substring(0, sentencesIn_.Length - 1) & ")"

                                     Dim expression_ = New NCalc.Expression(sentencesIn_)


                                     expression_.Parameters = values_

                                     AddHandler expression_.EvaluateFunction, RunFunctionHandler(values_)


                                     functionParameters_.Result = expression_.Evaluate

                                     resultIsDouble_ = False

                                 Case "ENLISTAR"

                                     Dim list_ As New List(Of String)

                                     Dim separator_ As String

                                     Dim parameter_

                                     Dim total_ = functionParameters_.Parameters.Count - 1

                                     If total_ = -1 Then

                                         separator_ = ","

                                         parameter_ = ","

                                     Else


                                         parameter_ = functionParameters_.Parameters.Last.Evaluate

                                     End If

                                     If TypeOf parameter_ Is List(Of String) Then

                                         separator_ = ","

                                     Else

                                         separator_ = parameter_.ToString

                                         If separator_ <> "," And separator_ <> "|" And separator_ <> "{}" And separator_ <> "#" And separator_ <> ";" Then

                                             separator_ = ","

                                         Else

                                             total_ -= 1

                                         End If


                                     End If


                                     For index_ As Integer = 0 To total_

                                         Dim value_ = functionParameters_.Parameters(index_).Evaluate

                                         Dim wordList_ As New List(Of String)

                                         If TypeOf value_ Is List(Of String) Then

                                             wordList_ = value_

                                         Else

                                             wordList_ = functionParameters_.Parameters(index_).Evaluate.ToString.Split(separator_).ToList

                                         End If

                                         For Each word_ In wordList_

                                             list_.Add(word_.Trim(" ").Trim(Chr(160)).Replace("\r", ""))

                                         Next


                                     Next

                                     functionParameters_.Result = list_

                                     resultIsDouble_ = False

                                 Case "ESBLANCO"

                                     Dim firstParameter_ = functionParameters_.
                                                            Parameters.
                                                            First.
                                                            Evaluate

                                     If TypeOf firstParameter_ Is List(Of String) OrElse TypeOf firstParameter_ Is Dictionary(Of String, String) Then

                                         If firstParameter_ IsNot Nothing Then

                                             If firstParameter_.Count > 0 Then

                                                 functionParameters_.Result = False

                                             Else

                                                 functionParameters_.Result = True

                                             End If

                                         Else

                                             functionParameters_.Result = True

                                         End If

                                     Else

                                         If firstParameter_ Is Nothing Then

                                             functionParameters_.Result = False

                                         Else

                                             If firstParameter_.ToString <> "" Then

                                                 functionParameters_.Result = False

                                             Else

                                                 functionParameters_.Result = True

                                             End If

                                         End If


                                     End If





                                     resultIsDouble_ = False

                                 Case "ESPACIOS"

                                     Dim firstParameter_ As String = functionParameters_.
                                                            Parameters.
                                                            First.
                                                            Evaluate
                                     Dim sinEspcacios_ As String = ""

                                     Dim encontroCaracter_ = False

                                     For Each caracter_ In firstParameter_

                                         If encontroCaracter_ Then

                                             sinEspcacios_ &= caracter_

                                         Else

                                             If caracter_ <> " " And Asc(caracter_) <> 160 Then

                                                 encontroCaracter_ = True

                                                 sinEspcacios_ &= caracter_

                                             End If

                                         End If

                                     Next

                                     firstParameter_ = sinEspcacios_

                                     sinEspcacios_ = ""

                                     encontroCaracter_ = False

                                     For Each caracter_ In firstParameter_.Reverse

                                         If encontroCaracter_ Then

                                             sinEspcacios_ = caracter_ & sinEspcacios_

                                         Else

                                             If caracter_ <> " " And Asc(caracter_) <> 160 Then

                                                 encontroCaracter_ = True

                                                 sinEspcacios_ = caracter_ & sinEspcacios_

                                             End If

                                         End If

                                     Next

                                     functionParameters_.Result = sinEspcacios_

                                     resultIsDouble_ = False

                                 Case "EXTRAE"

                                     Dim firstParameter_ As String = functionParameters_.
                                                            Parameters.
                                                            First.
                                                            Evaluate

                                     Dim secondParameter_ = functionParameters_.
                                                            Parameters(1).
                                                            Evaluate

                                     Dim thirdParameter_ = functionParameters_.
                                                            Parameters.
                                                            Last.Evaluate


                                     functionParameters_.Result = firstParameter_.Substring(secondParameter_ - 1, thirdParameter_)

                                     resultIsDouble_ = False

                                 Case "HOY"

                                     Dim firstParameter_ As String

                                     If functionParameters_.Parameters.Count = 0 Then

                                         firstParameter_ = ""

                                     Else

                                         firstParameter_ = functionParameters_.
                                                          Parameters.
                                                          First.
                                                          Evaluate

                                     End If


                                     Dim date_ As String = DateTime.Now.ToString("yyyy/MM/dd")

                                     If firstParameter_ <> "" Then

                                         date_ = date_.Replace("/", firstParameter_)


                                     End If

                                     functionParameters_.Result = date_

                                     resultIsDouble_ = False


                                 Case "LARGO"

                                     Dim firstParameter_ = functionParameters_.
                                                            Parameters.
                                                            First.
                                                            Evaluate

                                     If TypeOf firstParameter_ Is List(Of String) Then

                                         result_ = firstParameter_.Count

                                     Else


                                         result_ = firstParameter_.ToString.Length

                                     End If

                                     resultIsDouble_ = True

                                 Case "MEZCLAR"

                                     Dim resultList_ As New List(Of String)

                                     Dim list1_ As List(Of String) = functionParameters_.Parameters(0).Evaluate

                                     Dim list2_ As List(Of String) = functionParameters_.Parameters(1).Evaluate

                                     Dim index_ = 0

                                     For Each element_ In list1_

                                         resultList_.Add(element_ & list2_(index_))

                                         index_ += 1

                                     Next

                                     functionParameters_.Result = resultList_

                                     resultIsDouble_ = False

                                 Case "NO"

                                     Dim firstParameter_ As Boolean = functionParameters_.
                                                            Parameters.
                                                            First.
                                                            Evaluate

                                     functionParameters_.Result = Not firstParameter_

                                     resultIsDouble_ = False

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

                                     AddHandler expression_.EvaluateFunction, RunFunctionHandler(values_)

                                     functionParameters_.Result = expression_.Evaluate

                                     resultIsDouble_ = False

                                 Case "OBTENERVALOR"

                                     Dim firstParameter_ = functionParameters_.
                                                            Parameters.
                                                            First.
                                                            Evaluate
                                     Dim secondParameter_ As String = functionParameters_.
                                                            Parameters(1).
                                                            Evaluate

                                     If TypeOf firstParameter_ Is Dictionary(Of String, String) Then

                                         Dim dictionary_ As Dictionary(Of String, String) = firstParameter_

                                         If dictionary_.ContainsKey(secondParameter_) Then

                                             functionParameters_.Result = dictionary_(secondParameter_)

                                         Else

                                             functionParameters_.Result = ""

                                         End If
                                     Else

                                         If TypeOf firstParameter_ Is Dictionary(Of String, List(Of String)) Then

                                             Dim dictionary_ As Dictionary(Of String, List(Of String)) = firstParameter_

                                             If dictionary_.ContainsKey(secondParameter_) Then

                                                 functionParameters_.Result = dictionary_(secondParameter_)

                                             Else

                                                 functionParameters_.Result = New List(Of String)

                                             End If

                                         Else

                                             Dim list_ As List(Of String) = firstParameter_

                                             If list_.Count < secondParameter_ Then

                                                 functionParameters_.Result = ""

                                             Else

                                                 functionParameters_.Result = list_(secondParameter_ - 1)

                                             End If

                                         End If

                                     End If

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

                                     Dim cube_ As ICubeController = New CubeController()

                                     Dim lastParameter_ As String = ""

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

                                                 Dim foundParameter_ = functionParameters_.Parameters.Where(Function(ch) ch.ParsedExpression.ToString = "[" & parametro_.ToString & ".0]")(0)



                                                 If values_.ContainsKey(parametro_.ToString & ".0") Then


                                                     'values_(parametro_.ToString & ".0") = functionParameters_.
                                                     '                                             Parameters(paramPosition_).
                                                     '                                             Evaluate

                                                     If foundParameter_ Is Nothing Then

                                                         values_(parametro_.ToString & ".0") = functionParameters_.
                                                                                                      Parameters(paramPosition_).
                                                                                                      Evaluate

                                                     Else

                                                         values_(parametro_.ToString & ".0") = foundParameter_.Evaluate

                                                     End If

                                                 Else

                                                     If foundParameter_ Is Nothing Then

                                                         'values_(parametro_.ToString & ".0") = functionParameters_.
                                                         '                                             Parameters(paramPosition_).
                                                         '                                             Evaluate

                                                         values_.Add(parametro_.ToString & ".0",
                                                                     functionParameters_.
                                                                     Parameters(paramPosition_).
                                                                     Evaluate)

                                                     Else

                                                         values_.Add(parametro_.ToString & ".0",
                                                                     foundParameter_.Evaluate)


                                                     End If

                                                 End If

                                             End If

                                             lastParameter_ = parametro_

                                         End If

                                         paramPosition_ += 1

                                     Next

                                     If paramPosition_ > 0 And paramPosition_ < functionParameters_.Parameters.Count - 1 Then



                                         For position_ = 1 To functionParameters_.Parameters.Count - paramPosition_

                                             Dim foundParameter_ = functionParameters_.Parameters.Where(Function(ch) ch.ParsedExpression.ToString = "[" & lastParameter_.ToString & "." & position_ & "]")(0)

                                             If foundParameter_ Is Nothing Then

                                                 values_(lastParameter_.ToString & "." & position_) = functionParameters_.
                                                                                                      Parameters(paramPosition_ + position_).
                                                                                                      Evaluate

                                             End If

                                             values_(lastParameter_.ToString & "." & position_) = foundParameter_.
                                                                                                  Evaluate

                                         Next

                                     End If

                                     Dim expressionRoom_ = New NCalc.Expression(GetExpression(operation_.Replace(Chr(13), "").Replace(vbCrLf, "").Replace(Chr(160), "").Replace("\r", ""),
                                                                                                  values_))

                                     expressionRoom_.Parameters = values_

                                     AddHandler expressionRoom_.EvaluateFunction, RunFunctionHandler(values_)

                                     Dim resultRoom_ = expressionRoom_.Evaluate

                                     If TypeOf resultRoom_ Is List(Of String) OrElse
                                        TypeOf resultRoom_ Is Dictionary(Of String, List(Of String)) OrElse
                                        TypeOf resultRoom_ Is Dictionary(Of String, String) Then

                                         functionParameters_.Result = resultRoom_

                                         resultIsDouble_ = False

                                     Else

                                         If Not Double.TryParse(resultRoom_, result_) Then

                                             functionParameters_.Result = resultRoom_

                                             resultIsDouble_ = False

                                         End If

                                     End If

                                 Case "SECUENCIA"


                                     Dim list_ As New List(Of String)

                                     If functionParameters_.Parameters.Count = 1 Then

                                         For secuencia_ = 1 To functionParameters_.Parameters.First.Evaluate

                                             list_.Add(secuencia_)

                                         Next

                                     Else

                                         For secuencia_ = functionParameters_.Parameters.First.Evaluate To functionParameters_.Parameters.Last.Evaluate

                                             list_.Add(secuencia_)

                                         Next


                                     End If

                                     functionParameters_.Result = list_

                                     resultIsDouble_ = False


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

                                         Dim operationsDB_ As IMongoCollection(Of room) =
                                             _enlaceDatos.GetMongoCollection(Of room)(New MongoClient("mongodb://" & endPoint_.ip & ":" & endPoint_.port), "CUBEROOMS")



                                         Dim updateDefinition_ = Builders(Of room).
                                                                             Update.
                                                                             Set(Function(e) e.rules, rules_).
                                                                             Set(Function(e) e.contenttype, contenttype_)

                                         Dim filter_ = Builders(Of room).
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

                                 Case "SUSTITUIR"



                                     Dim parameters_ = functionParameters_.Parameters

                                     Dim sentence_ As String = parameters_(0).Evaluate

                                     Dim charOld_ As String = parameters_(1).Evaluate

                                     Dim charNew_ As String = parameters_(2).Evaluate

                                     functionParameters_.Result = sentence_.Replace(charOld_, charNew_)

                                     resultIsDouble_ = False

                                 Case "TRUNC"


                                     Dim firstParameter_ = functionParameters_.Parameters.First

                                     Dim expression_ = New NCalc.Expression("Truncate((" &
                                                                              firstParameter_.
                                                                              Evaluate().
                                                                              ToString &
                                                                              ")*10000)/10000")


                                     expression_.Parameters = values_

                                     AddHandler expression_.EvaluateFunction, RunFunctionHandler(values_)

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

                                     expression_.Parameters = values_

                                     AddHandler expression_.EvaluateFunction, RunFunctionHandler(values_)

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

                .InterpreterType = Nothing

                ._status = Nothing

                ._validFields = Nothing

                ._customFunctions = Nothing

                ._expressionNCalc = Nothing

                ._operandsTemp = Nothing

                ._operators = Nothing

                ._reports.Dispose()

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

#Region "Clon"

    Private Function CloneTagWatcher() As TagWatcher

        Dim tagWatcherClone_ As TagWatcher

        Dim formatter_ As New BinaryFormatter()

        Dim stream_ As New MemoryStream()

        formatter_.Serialize(stream_, _status)

        stream_.Position = 0

        tagWatcherClone_ = formatter_.Deserialize(stream_)

        Return tagWatcherClone_

    End Function
    Public Function Clone() As Object Implements ICloneable.Clone, IMathematicalInterpreter.Clone

        Dim intepreterClone_ As IMathematicalInterpreter = New MathematicalInterpreterNCalc

        With intepreterClone_

            .interpreterType = Me.InterpreterType

            .status = Me.CloneTagWatcher()

        End With

        SetReport(intepreterClone_.GetReportFull)

        Return intepreterClone_

    End Function

#End Region

#End Region

End Class





