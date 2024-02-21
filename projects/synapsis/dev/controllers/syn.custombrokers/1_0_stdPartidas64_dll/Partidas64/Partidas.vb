Imports Wma.Exceptions

Public Class Partidas : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property Secuencia As Integer

    Property Fraccion As String

    Property Nico As String

    Property Vinculacion As String

    Property MetodoValoracion As String

    Property UnidadMedidaComercial As Integer

    Property CantidadUmt As Double

    Property PaisVendedorComprador As String

    Property PaisOrigenDestino As String

    Property Descripcion As String

    Property ValorAduanaDolares As Double

    Property ImportePrecioPagado As Double

    Property PrecioUnitario As Double

    Property ValorAgregado As Double

    Property Marca As String

    Property Modelo As String

    Property CodigoProducto As String

    Property IdentificadoresPartidas As List(Of IdentificadoresPartidas)

    Property PermisosPartidas As List(Of PermisosPartidas)

    Property Observaciones As String
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