Imports gsol.krom

Namespace gsol.krom

    Public Class ConfiguracionBaseDeDatos
        Inherits EntidadDatos

#Region "Enums"

        Public Enum VT000CamposBaseDatos
            i_Cve_CamposBaseDatos
            t_NombreTecnico
            t_NombreDescriptivo
            t_Descripcion
            i_Cve_Estado
            i_Cve_Estatus
        End Enum

#End Region

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.ConfiguracionBaseDeDatos

        End Sub

#End Region

    End Class

End Namespace