Imports Wma.Exceptions

Public Class AgenteAgenciaApoderadoAduanal : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property NombreRazonSocial As String

    Property RFC As String

    Property CURP As String

    Property NombreMandatarioPersona As String

    Property RfcMandatarioPersona As String

    Property CurpMandatarioPersona As String

    Property NumeroSerieCertificado As String

    Property Efirma As String

    Property Patente As String

#End Region

#Region "Metodos"

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class