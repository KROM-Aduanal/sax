Imports System.Security.Cryptography
Namespace gsol.seguridad

    Public Interface ICifrado

#Region "Atributos"

        Enum Metodos
            SHA1 = 0
            MD5 = 1
            Base64 = 2
        End Enum

        Enum TipoComportamientoCodigo
            MelekDay
            MelekDaySolicitudPagoReferencias
            MelekHour
            MelekDayFacturacion
        End Enum

#End Region

#Region "Contructores"

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Property Cadena As String

        'ReadOnly Property MetodoEncriptacion As SymmetricAlgorithm

        'ReadOnly Property LlaveCifrado As String

#End Region

#Region "Metodos"

        Function LlaveAccesoAutomatica(ByVal tipoComporamientoCodigo As TipoComportamientoCodigo) As String

        Function CifraCadena(ByVal cadena_ As String, ByVal metodocifrado_ As SymmetricAlgorithm, Optional ByVal llavecifrado As String = Nothing) As String

        Function CifraCadena(ByVal cadena_ As String, ByVal metodocifrado_ As Metodos) As String

        Function DescifraCadena(ByVal cadena_ As String, ByVal metodocifrado_ As SymmetricAlgorithm, ByVal llavecifrado As String) As String

#End Region

    End Interface

End Namespace
