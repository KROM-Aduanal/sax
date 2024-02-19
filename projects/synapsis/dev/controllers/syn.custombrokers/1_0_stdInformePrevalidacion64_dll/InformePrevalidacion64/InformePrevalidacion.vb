Imports MongoDB.Bson
Imports Wma.Exceptions

Public Class InformePrevalidacion : Implements ICloneable

#Region "Enums"

    Enum TiposProcesamiento
        SinDefinir = 0
    End Enum

    Enum TiposValidacion
        SinDefinir = 0
    End Enum

#End Region

#Region "Propiedades"

    Property TipoProcesamiento As TiposProcesamiento

    Property TipoValidacion As TiposValidacion

    Property IdPedimento As ObjectId

    Property NumeroPedimento As String

    Property TipoOperacion As [Enum]

    Property ClavePedimento As String

    Property Regimen As String

    Property FechaValidacion As Date

    Property NumeroValidacion As Integer

    Property AnioValidacion As String

    Property ExepcionesEncontradas As List(Of ExepcionValidador)

    Property Archivado As Boolean

    Property Estatus As TagWatcher


#End Region

#Region "Metodos"

    Public Function GenerarInformeDetallado() As List(Of ExepcionValidador)

        Return Nothing

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class