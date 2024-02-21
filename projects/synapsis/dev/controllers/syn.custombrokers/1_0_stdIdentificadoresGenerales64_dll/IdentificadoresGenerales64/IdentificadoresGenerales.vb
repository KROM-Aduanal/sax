Imports Wma.Exceptions

Public Class IdentificadoresGenerales : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property Clave As String

    Property Complemento1 As String

    Property Complemento2 As String

    Property Complemento3 As String

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