Imports Wma.Exceptions

Public Class Decrementables : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property Transporte As Integer

    Property Seguro As Integer

    Property Descarga As Integer

    Property Otros As Integer

#End Region

#Region "Metodos"

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class