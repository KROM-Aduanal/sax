Imports Wma.Exceptions

Public Class ItemExtraAutomoviles

#Region "Atributos"

    Private _kilometraje As Integer

#End Region

#Region "Propiedades"

    Property Kilometraje As Object

        Get

            Return _kilometraje

        End Get

        Set(value As Object)

            _kilometraje = value

        End Set

    End Property

#End Region

#Region "Metodos"

    Private Function VerificaKilometraje() As TagWatcher

        Throw New NotImplementedException()

    End Function

#End Region

End Class