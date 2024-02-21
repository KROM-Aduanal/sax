Imports Wma.Exceptions

Public Class Cove : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property NumeroAcuseValor As String

    Property Vinculacion As String

    Property Incoterm As String

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