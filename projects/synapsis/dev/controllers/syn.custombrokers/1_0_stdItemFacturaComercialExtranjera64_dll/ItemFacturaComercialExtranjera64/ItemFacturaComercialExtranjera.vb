Imports Wma.Exceptions

Public Class ItemFacturaComercialExtranjera

#Region "Atributos"

    Private _totalIncrementables As Double
    Private _totalDecrementables As Double

#End Region

#Region "Propiedades"

    Property TotalIncrementables As Double

        Get

            Return _totalIncrementables

        End Get

        Set(value As Double)

            _totalIncrementables = value

        End Set

    End Property

    Property TotalDecrementables As Double

        Get

            Return _totalDecrementables

        End Get

        Set(value As Double)

            _totalDecrementables = value

        End Set

    End Property

#End Region

#Region "Metodos"

    Public Function ObtieneIncrementables() As TagWatcher

        Throw New NotImplementedException()

    End Function

    Public Function ObtieneDecrementables() As TagWatcher

        Throw New NotImplementedException()

    End Function

#End Region

End Class