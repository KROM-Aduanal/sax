Imports gsol.krom

Namespace gsol.krom

    Public Class CodigoAgrupadorSAT
        Inherits EntidadDatos

        Public Enum CodigoAgrupadorSATTokenDimencion
            i_Cve_SATBancoRegistrado
            t_NombreCorto
            t_NombreRazonSocial
            i_Cve_Estado
        End Enum


#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.CodigoAgrupadorSAT

        End Sub

#End Region

    End Class

End Namespace

