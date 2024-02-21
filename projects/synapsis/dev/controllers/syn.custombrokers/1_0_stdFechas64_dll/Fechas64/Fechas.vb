Imports Wma.Exceptions

Public Class Fechas : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property Entrada As Date

    Property Pago As Date

    Property Extraccion As Date

    Property Presentacion As Date

    Property Impeuacan As Date

    Property Original As Date

#End Region

#Region "Metodos"

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class