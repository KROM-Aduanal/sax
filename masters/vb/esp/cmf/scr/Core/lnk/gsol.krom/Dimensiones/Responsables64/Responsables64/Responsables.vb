Imports gsol.krom
Imports Wma.Exceptions

Namespace gsol.krom

    Public Class Responsables
        Inherits EntidadDatos

#Region "Enums"

        Public Enum Vt026Responsables
            i_Cve_Responsable
            t_DescripcionEsp
            t_DescripcionIng
            i_TipoResponsable
            t_TipoResponsable
            i_Cve_Estatus
            t_Estatus
            i_Cve_Estado
        End Enum

#End Region

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.Responsables

        End Sub

#End Region

    End Class

End Namespace