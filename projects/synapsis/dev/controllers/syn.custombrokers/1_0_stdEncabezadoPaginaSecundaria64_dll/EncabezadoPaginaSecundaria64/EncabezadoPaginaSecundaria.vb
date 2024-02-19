Imports Wma.Exceptions

Public Class EncabezadoPaginaSecundaria : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property NumeroPedimento As String

    Property TipoOperacion As String

    Property ClavePedimento As String

    Property RFC As String

    Property CURP As String

#End Region

#Region "Metodos"

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class