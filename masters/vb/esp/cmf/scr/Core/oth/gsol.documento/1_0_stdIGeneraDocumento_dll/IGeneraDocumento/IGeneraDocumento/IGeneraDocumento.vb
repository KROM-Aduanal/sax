Imports Gsol.BaseDatos.Operaciones

Namespace Gsol.Documento


    Public Interface IGeneraDocumento

#Region "Atributos"

        Enum TiposProcesable

            INDEFINIDO = 0
            XMLCatalogo = 1
            XMLBalanza = 2
            XMLPolizas = 3

        End Enum

#End Region

#Region "Propiedades"

        Property IOCatalogo() As IOperacionesCatalogo

        ' Property ArchivoGenerado() As Object

        Property FechaGenerado() As Date

        Property Version() As String

        'Sub ProcesarDocumento(ByVal tipoProcesable_ As TiposProcesable)

        Property FechaGeneral() As Date

        Property Ruta() As String

#End Region

#Region "Métodos"

        Sub ProcesarDocumento(ByVal tipoProcesable_ As TiposProcesable)

        Sub ConsultaDatosEmpresa()

#End Region

    End Interface
End Namespace
