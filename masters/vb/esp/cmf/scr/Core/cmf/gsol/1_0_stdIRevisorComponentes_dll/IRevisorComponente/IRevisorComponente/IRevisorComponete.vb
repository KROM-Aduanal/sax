Imports System.ComponentModel

Namespace gsol

    Public Interface IRevisorComponente

#Region "Atributos"
        Enum TipoAplicacion
            NoDefinido = 0
            <Description("System.Windows.Forms")> Escritorio
            <Description("System.Web.UI")> Web
        End Enum
#End Region

#Region "Constructores"

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        ReadOnly Property Aplicacion As TipoAplicacion
        Property Control As Dictionary(Of Int32, String)
        Property DominioAdaptador As IAdaptador.DominiosAdptador

#End Region

#Region "Metodos"

        Sub ObtenerControles(ByVal controles_ As ICollection)

#End Region

    End Interface

End Namespace
