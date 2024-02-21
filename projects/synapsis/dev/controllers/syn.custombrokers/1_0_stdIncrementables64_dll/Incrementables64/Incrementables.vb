Imports Wma.Exceptions

Public Class Incrementables : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property ValorSeguros As Integer

    Property Seguros As Integer

    Property Fletes As Integer

    Property Embalajes As Integer

    Property Otros As Integer

#End Region

#Region "Metodos"

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class