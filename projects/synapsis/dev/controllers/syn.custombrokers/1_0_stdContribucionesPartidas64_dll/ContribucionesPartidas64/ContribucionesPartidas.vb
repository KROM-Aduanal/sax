Imports Wma.Exceptions

Public Class ContribucionesPartidas : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property Contribucion As String

    Property Tasa As Integer

    Property TipoTasa As Integer

    Property FormaPago As String

    Property Importe As Double

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