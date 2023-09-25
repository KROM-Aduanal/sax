Imports gsol.krom

Namespace gsol.krom

    Public Class BancosSAT2
        Inherits EntidadDatos

#Region "Enums"

        Public Enum BancosSATTokenDimencion
            i_Cve_SATBancoRegistrado
            t_NombreCorto
            t_NombreRazonSocial
            i_Cve_Estado
        End Enum

#End Region

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.BancosSAT2

        End Sub

#End Region

    End Class

End Namespace
