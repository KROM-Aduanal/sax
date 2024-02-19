Imports Wma.Exceptions

Public Class DepositoReferenciado : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property CodigoBarras As String

    Property NumeroCodigoBarras As String

    Property Patente As String

    Property Pedimento As String

    Property Aduana As String

    Property Banco As String

    Property LineaCaptura As String

    Property ImportePagado As Integer

    Property FechaPago As Date

    Property NumeroOperacionBancaria As String

    Property NumeroTransaccional As String

    Property MedioPresentacion As String

    Property MedioRecepcionCobro As String

#End Region

#Region "Metodos"

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class