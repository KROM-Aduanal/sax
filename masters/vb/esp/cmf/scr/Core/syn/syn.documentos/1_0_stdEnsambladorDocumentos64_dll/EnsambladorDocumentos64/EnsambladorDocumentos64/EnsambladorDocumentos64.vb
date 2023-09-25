Imports MongoDB.Driver

Namespace Syn.Documento

    <Serializable()>
    Public Class EnsambladorDocumentos

        Public Sub Construye(ByVal documentoElectronico_ As DocumentoElectronico)

            documentoElectronico_.ConstruyeEncabezado()

            documentoElectronico_.ConstruyeCuerpo()

            documentoElectronico_.ConstruyeEncabezadoPaginasSecundarias()

            documentoElectronico_.ConstruyePiePagina()

        End Sub

    End Class

End Namespace
