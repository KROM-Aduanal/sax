Imports gsol.krom

Namespace gsol.krom

    Public Class RegistroCausales
        Inherits EntidadDatos

#Region "Enums"

        Public Enum Vt026GeneralCausales
            i_Cve_RegistroCausal
            Auxiliar_t_Descripcion
            i_Cve_Causal
            Auxiliar_t_Clave
            Auxiliar_i_NivelCausal
            Auxiliar_i_Cve_TipoCausal
            i_Cve_Segmento
            Auxiliar_t_DescripcionResponsable
            Auxiliar_i_Cve_Responsable
            Auxiliar_i_TipoResponsable
            t_Referencia
            i_Cve_MaestroOperaciones
            i_Cve_DivisionMiEmpresa
            Oficina
            Estatus
            i_Cve_Estatus
            f_FechaRegistro
            f_FechaCierre
            f_FechaInicio
            t_ObservacionesCausal
            i_Cve_Estado
        End Enum

#End Region

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.RegistroCausales

        End Sub

#End Region



    End Class

End Namespace

