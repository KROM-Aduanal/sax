Imports Wma.Exceptions

Public Class CuadroLiquidacion : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property Concepto As String

    Property FormaPago As String

    Property Importe As Integer

#End Region

#Region "Metodos"

    Public Sub Add()

    End Sub

    Public Sub Delete()

    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class