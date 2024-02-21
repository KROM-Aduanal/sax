Imports Wma.Exceptions

Public Class CartaInstruccionesSynapsis : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property NumeroPedimento As String

    Property AplicaPreferencias As Boolean

    Property AplicaCuentasAduaneras As Boolean

    Property AplicaPreciosEstimados As Boolean

    Property AplicaPreciosReferencia As Boolean

    Property AplicaCompensaciones As Boolean

    Property TieneCertificados As Boolean

    Property RequierePermisos As Boolean

    Property RequiereNormas As Boolean

    Property RequiereConversionUnidad As Boolean

    Property Certificados As List(Of Object)

    Property RegulacionesNoArancelarias As List(Of PermisoNorma)

    Property Compensaciones As List(Of Object)

    Property Conversiones As List(Of Object)

#End Region

#Region "Metodos"

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class