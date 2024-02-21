Imports Wma.Exceptions

Public Class MediosTransporte : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property EntradaSalida As Integer

    Property Arribo As Integer

    Property Salida As Integer

#End Region

#Region "Metodos"

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class