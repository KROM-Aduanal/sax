Imports System.IO
Imports System.Text.RegularExpressions
Imports Wma.Exceptions


Public Interface IMathematicalInterpreter : Inherits IDisposable

#Region "Enums"
    Enum InterpreterTypes

        Undefined = 0

        NCALC = 1

    End Enum

    Enum InterpreterErrorTypes

        Undefined = 0

        Syntax = 1

        Semantic = 2

        Logic = 3

        Round = 4

        Truncate = 5

        Precision = 6

        OverFlow = 7

        UnderOverFlow = 8

        DivisionByZero = 9

        UndefinedVariable = 10

        UndefinedFunction = 11

        DataType = 12

        InsufficientMemory = 13

        ImaginaryNumber = 14

    End Enum

#End Region

#Region "Properties"

    Property interpreterType As InterpreterTypes
    Property status As TagWatcher

#End Region

#Region "Methods"

    Function RunExpression(Of T)(expression_ As String, constantValues_ As Dictionary(Of String, T), Optional indexM As Int32 = 0) As T

    Function RunExpression(Of T)(expression_ As String, constantValues_ As Dictionary(Of String, T), interprete_ As InterpreterTypes) As T

    Function GetReportFull() As ValidatorReport
    Function GetParams(expression_ As String) As List(Of String)

    Sub SetValidFields(campos_ As List(Of String))

    Sub SetValidField(field_ As String)


    Function GetValidFields() As List(Of String)

    Function ExistFields(expresion_ As String, fields_ As List(Of String)) As Boolean

    Sub addOperands(operands_ As List(Of String))

#End Region

End Interface

