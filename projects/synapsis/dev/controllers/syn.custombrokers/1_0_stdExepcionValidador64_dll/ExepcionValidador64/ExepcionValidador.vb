Imports Wma.Exceptions

Public Class ExepcionValidador : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property NombreCampo As String

    Property NombrePresentacion As String

    Property IdCampoUnico As Integer

    Property NumeroRegistro As Integer

    Property DescripcionRegistro As String

    Property ErroresValidacion As List(Of ErrorValidacion)

    Property Estatus As TagWatcher


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