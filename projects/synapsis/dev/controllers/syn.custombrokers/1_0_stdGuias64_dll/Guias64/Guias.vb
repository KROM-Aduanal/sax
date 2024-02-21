Imports Wma.Exceptions

Public Class Guias : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property Numero As String

    Property Tipo As String

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