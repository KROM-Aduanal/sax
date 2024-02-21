Imports Wma.Exceptions

Public Class Pedimento : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property NumeroPedimento As String

    Property TipoOperacion As String

    Property CvePedimento As String

    Property Regimen As String

    Property Destino As Integer

    Property TipoCambio As Double

    Property PesoBruto As Double

    Property PrecioPagadoValorComercial As Double

    Property DatosImportadorExportador As DatosImportadorExportador

    Property Incrementables As Incrementables

    Property Decrementables As Decrementables

    Property CodigoAceptacion As String

    Property ClaveDespacho As String

    Property MarcasNumerosTotalesBultos As String

    Property Fecha As Fechas

    Property TasasNivelPedimento As TasasNivelPedimento

    Property CuadroLiquidacion As CuadroLiquidacion

    Property Totales As Totales

    Property DepositoReferenciado As DepositoReferenciado

    Property AgenteAgenciaApoderadoAduanal As AgenteAgenciaApoderadoAduanal

    Property EncabezadoPaginaSecundarias As EncabezadoPaginaSecundaria

    Property DatosProveedorComprador As DatosProveedorComprador

    Property Transportes As List(Of MediosTransporte)

    Property Candados As List(Of Candados)

    Property Guias As List(Of Guias)

    Property IdentificadoresGenerales As IdentificadoresGenerales

    Property Observaciones As String

    Property Partidas As List(Of Partidas)

    Property NumeroTotalPartidas As Integer

    Property ClavePrevalidador As String

    Property Estado As TagWatcher

#End Region

#Region "Metodos"

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class