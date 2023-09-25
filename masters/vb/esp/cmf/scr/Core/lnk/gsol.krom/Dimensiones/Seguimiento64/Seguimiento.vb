Namespace gsol.krom

    Public Class Seguimiento
        Inherits EntidadDatos

#Region "Enums"

        Enum Vt025SeguimientoPendientesKBW
            t_EjecutivoRecepcion
            i_cve_seguimientoPendienteMostrarComentarios
            i_cve_seguimientoPendienteEditar
            i_Folio
            t_EjecutivoSolicitante
            t_EjecutivoResponsable
            i_cve_seguimientoPendientesEdicion
            i_cve_seguimientoPendientes
            t_Estatus
            t_Autorizado
            i_Prioridad
            i_Cve_EjecutivoResponsableInbox
            i_Cve_EjecutivoRecepcionInbox
            i_Cve_EjecutivoSolicitudInbox
            t_ResponsableInbox
            t_ReceptorInbox
            t_SolicitudInbox
            t_DireccionInbox
            f_FechaRegistro
            f_FechaUltimoMovimiento
            i_DiasNaturales
            t_NombreCompletoER
            i_Cve_EjecutivoRecepcion
            t_Descripcion
            t_Comentarios
            i_Cve_EjecutivoSolicitud
            t_NombreCompletoS
            t_NombreCompletoR
            i_Cve_EjecutivoResponsable
            f_FechaRecepcionSolicitud
            i_Cve_DivisionMiEmpresa
            i_Cve_Estado
            i_Cve_Estatus
            f_FechaInicio
            f_FechaFin
            i_Autorizacion
            f_FechaAutorizacion
            i_Cve_TipoSeguimiento
        End Enum

#End Region

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.Seguimiento

        End Sub

#End Region

    End Class

End Namespace