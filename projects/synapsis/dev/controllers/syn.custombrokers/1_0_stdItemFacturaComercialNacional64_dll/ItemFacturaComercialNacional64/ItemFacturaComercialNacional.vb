Imports Wma.Exceptions

Public Class ItemFacturaComercialNacional

#Region "Atributos"

    Private _valorAgregado As Integer

#End Region

#Region "Propiedades"

    Property ValorAgregado As Integer

        Get

            Return _valorAgregado

        End Get

        Set(value As Integer)

            _valorAgregado = value

        End Set

    End Property

#End Region

#Region "Metodos"

    Public Function ObtieneValorAgregado() As TagWatcher

        Throw New NotImplementedException()

    End Function

#End Region

End Class