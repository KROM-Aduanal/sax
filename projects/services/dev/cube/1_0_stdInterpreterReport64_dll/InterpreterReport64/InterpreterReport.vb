Imports MongoDB.Bson

Public Class InterpreterReport

#Region "Enum"
    Enum InterpreterErrorTypes

        Undefined = 0

        Syntax = 1

        Semantic = 2

        Logic = 3

        Round = 4

        Truncate = 5

        Precision = 6

        Overflow = 7

        UnderOverflow = 8

        DivisionByZero = 9

        UndefinedVariable = 10

        UndefinedFunction = 11

        DataType = 12

        InsufficientMemory = 13

    End Enum

#End Region

#Region "Properties"

    Property _id As ObjectId
    Property IntepreterErrors As InterpreterErrorTypes
    Property Report As Dictionary(Of String, Dictionary(Of String, String))
    Property RrrorTime As DateTime


#End Region

End Class
