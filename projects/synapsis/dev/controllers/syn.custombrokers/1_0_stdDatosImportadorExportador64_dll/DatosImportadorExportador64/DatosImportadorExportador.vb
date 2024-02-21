Imports Wma.Exceptions

Public Class DatosImportadorExportador : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property Nombre As String

    Property RFC As String

    Property CURP As String

    Property Domicilio As String

#End Region

#Region "Metodos"

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class