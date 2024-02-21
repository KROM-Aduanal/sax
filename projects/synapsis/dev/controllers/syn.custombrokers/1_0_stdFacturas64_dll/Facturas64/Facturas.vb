Imports Wma.Exceptions

Public Class Facturas : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property NumeroFactura As String

    Property Fecha As Date

    Property Incoterm As String

    Property Moneda As String

    Property ValorMoneda As Double

    Property FactorMoneda As Double

    Property ValorDolares As Double

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