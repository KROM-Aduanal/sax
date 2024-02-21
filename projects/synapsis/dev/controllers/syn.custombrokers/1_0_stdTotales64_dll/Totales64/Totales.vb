Imports Wma.Exceptions

Public Class Totales : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property Efectivo As Integer

    Property Otros As Integer

    Property Total As Integer

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