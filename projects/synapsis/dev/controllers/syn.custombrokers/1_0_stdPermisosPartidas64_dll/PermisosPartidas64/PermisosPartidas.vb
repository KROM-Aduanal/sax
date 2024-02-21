Imports Wma.Exceptions

Public Class PermisosPartidas : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property Clave As String

    Property Numero As String

    Property FirmaDescargo As String

    Property ValorComercialDolares As Double

    Property CantidadUnidadMedida As Double

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