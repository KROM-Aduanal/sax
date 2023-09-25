Imports Wma.Exceptions

Namespace gsol.krom

    Public Class Clientes
        Inherits EntidadDatos

        Enum Vt001ClientesEmpresaOtrosCampos
            i_Cve_ClienteEmpresa
            i_Cve_ClientesEmpresaOtrosCampos
            i_Cve_ClienteExterno
            t_RFCCliente
            i_Cve_DivisionMiEmpresa
            t_Division
            t_DivisionCliente
            t_GrupoEmpresarial
            t_NombreGiro
            i_VoBoOperativo
            t_VoBoOperativo
            i_VoBoAdministrativo
            t_VoBoAdministrativo
            i_Cve_Estado
        End Enum

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.Clientes

        End Sub

#End Region

    End Class


End Namespace