Public Class Ejecutivos
    Inherits EntidadDatos

#Region "Enums"

    Public Enum EjecutivosMiEmpresa

        i_Cve_EjecutivosMisEmpresas = 0
        i_Cve_Personas = 1
        t_Nombre = 2
        t_ApellidoPaterno = 3
        t_ApellidoMaterno = 4
        t_NombreCompleto = 5
        i_Cve_Puestos = 6
        t_Puesto = 7
        i_Cve_Departamentos = 8
        t_Departamento = 9
        i_Cve_DivisionEmpresarialesMisEmpresas = 10
        t_Division = 11
        i_Cve_DivisionMiEmpresa = 12
        i_Cve_Estado = 13

    End Enum

#End Region

#Region "Constructores"

    Sub New()

        Dimension = IEnlaceDatos.TiposDimension.Ejecutivos

    End Sub

#End Region

End Class