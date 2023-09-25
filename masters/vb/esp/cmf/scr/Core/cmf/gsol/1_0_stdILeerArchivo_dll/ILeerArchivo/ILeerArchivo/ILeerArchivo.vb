Imports System.Security.Cryptography
Namespace gsol

    Public Interface ILeerArchivo

#Region "Atributos"

        Enum TiposCifrado
            SinDefinir
            RijndaelManaged
        End Enum

        Enum StatusArchivos
            SinDefinir = 0
            Cargado = 1
            ConError = 2
        End Enum

        Enum TiposAutomaticos
            ConfiguracionModulos
            ConfiguracionCifradoInstancia
            OtroTipo
        End Enum

#End Region

#Region "Propiedades"

        Property TipoArchivoAutomatico As TiposAutomaticos

        WriteOnly Property RutaArchivo As String

        Property Encriptado As TiposCifrado

        Property Llave As String

        ReadOnly Property StatusArchivo As StatusArchivos

        ReadOnly Property Propiedades As Dictionary(Of String, String)


#End Region

#Region "Métodos"

        Sub LeerXML()
        Sub LeerXML(ByVal tipoArchivoAutomatico_ As TiposAutomaticos)
        Function RegresaValor(ByVal atributo_ As String) As String

#End Region

    End Interface

End Namespace
