Imports Wma.Exceptions

Public Class DatosProveedorComprador : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property IdFiscal As String

    Property NombreRazonSocial As String

    Property Domicilio As String

    Property Vinculacion As Double

    Property Facturas As List(Of Facturas)

    Property Coves As List(Of Cove)

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