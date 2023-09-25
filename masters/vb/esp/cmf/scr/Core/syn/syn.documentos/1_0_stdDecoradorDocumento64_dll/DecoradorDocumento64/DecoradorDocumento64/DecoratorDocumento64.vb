Namespace Syn.Documento

    <Serializable()>
    Public MustInherit Class DecoratorDocumento
        Inherits Documento

#Region "Attributes"

        Protected _documento As Documento

#End Region

#Region "Builders"

        Public Sub New(ByVal documento_ As Documento)

            Me._documento = documento_

        End Sub

#End Region

#Region "Methods"
        Public Overrides Sub GeneraDocumento()

            _documento.GeneraDocumento()

        End Sub

#End Region

    End Class

End Namespace
