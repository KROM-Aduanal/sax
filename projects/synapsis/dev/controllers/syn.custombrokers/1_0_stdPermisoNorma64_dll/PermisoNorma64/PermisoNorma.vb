Imports Wma.Exceptions

Public Class PermisoNorma : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property Numero As Integer

    Property Clave As String

    Property Mercancia As String

    Property Proveedor As String

    Property Factura As String

    Property UnidadMedida As String

    Property Cantidad As Double

    Property FraccionArancelaria As String

    Property Nico As String

    Property PaisOrigen As String

    Property ValorTotal As Double

    Property PrecioUnitario As Double

    Property Descripcion As String

    Property FechaInicioVigencia As Date

    Property FechaFinVigencia As Date

#End Region

#Region "Metodos"

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class