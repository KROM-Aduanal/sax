Imports MongoDB.Bson
Imports Wma.Exceptions

Public Class InformacionAgrupacion

#Region "Atributos"

    Property _id As ObjectId

    Property _idpedimento As ObjectId

    Property agrupacionseleccionada As IGeneradorPartidasPedimento.TipoAgrupaciones

    Property tipocambio As Double

    Property fechatipocambio As Date

    Property fechageneracionagrupacion As Date

    Property numerototalpartidas As Integer

    Property tipooperacion As Integer

    Property pedimento As DatosPedimento

    Property recursossasociadas As List(Of DocumentoAsociado)

    Property itemsasociados As List(Of ItemPartida)

    Property firmaelectronica As String

    Property archivado As Boolean

    Property estado As Integer

#End Region

#Region "Metodos"

    Public Function MostrarItemsAsociados(ByVal secuenciaPartida_ As Integer) As TagWatcher

        Throw New NotImplementedException()

    End Function

    Public Function AsociarItemsPartida() As TagWatcher

        Throw New NotImplementedException()

    End Function

    Public Function MostrarAgrupacionSeleccionada() As IGeneradorPartidasPedimento.TipoAgrupaciones

        Throw New NotImplementedException()

    End Function

#End Region

End Class

Public Class DatosPedimento

    Property numeropedimentocompleto As String

    Property aduanaseccion As String

    Property patente As String

    Property anio As String

    Property pedimento As String

    Property cvepedimento As String

    Property regimen As String

    Property fechaentrada As Date

    Property fechapresentacion As Date

End Class

Public Class DocumentoAsociado

    Property _iddocumentoasociado As ObjectId

    Property identificadorrecurso As String

    Property idcoleccion As String

    Property firmaelectronica As String

    Property metadatos As IMetadatos

    Property analisisconsistencia As Integer

End Class

Public Interface IMetadatos

End Interface

Public Class InformacionAdicionalFactura
    Implements IMetadatos

    Property foliodocumento As String

    Property clavemoneda As String

    Property fechafactormoneda As Date

    Property factormoneda As Double

    Property totalincrementables As Double

    Property valorfactura As Double

End Class