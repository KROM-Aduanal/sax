Imports MongoDB.Bson
Imports Wma.Exceptions

Public Class InformePrevalidacion

#Region "Propiedades"

    Property tipoprocesamiento As IPrevalidador.TiposProcesamiento

    Property tipovalidacion As IPrevalidador.TiposValidacion

    Property numerooperacion As Integer

    Property _idpedimento As ObjectId

    Property rutavalidacion As IPrevalidador.TiposRutaValidacion

    Property numeropedimento As String

    Property tipooperacion As Integer

    Property clavepedimento As String

    Property regimen As String

    Property fechavalidacion As Date

    Property numerovalidacion As Integer

    Property aniovalidacion As String

    Property exepcionesencontradas As List(Of ExepcionValidador)

    Property archivado As Boolean

    Property estatusoperacion As TagWatcher

    Property estatus As TagWatcher

#End Region

#Region "Metodos"

    Public Function GenerarInformeDetallado() As List(Of ExepcionValidador)

        Return Nothing

    End Function

#End Region

End Class