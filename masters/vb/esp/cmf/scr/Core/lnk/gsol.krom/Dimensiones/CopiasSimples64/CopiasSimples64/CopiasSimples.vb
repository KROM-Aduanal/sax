Imports gsol.krom

Namespace gsol.krom

    Public Class CopiasSimples
        Inherits EntidadDatos

#Region "Enums"

        Public Enum Vt022ReporteEstandarHidrocarburos
            h_HoraLiberacion
            i_Cve_MaestroOperaciones
            i_Cve_DivisionMiEmpresa
            t_RFCExterno
            i_Cve_VinClienteOperacion
            i_Cve_DivisionEmpresarialCliente
            i_AduanaSeccion
            i_Patente
            i_TipoOperacion
            i_Cve_ClienteOperativo
            i_Cve_ClienteComprobanteFiscal
            t_RFCComprobanteFiscal
            t_NumeroReferencia
            i_NumeroPedimento
            t_NombreCliente
            f_FechaPago
            i_Fraccion
            t_Mercancia
            i_PesoBrutoPedimento
            i_Consecutivo
            t_Tipo
            t_NoLoad
            t_Ticket
            f_FechaTicket
            t_Placas
            t_Placasct
            t_Transportista
            t_PlacaJaula
            i_PesoBrutoPartida
            i_PesoTara
            i_PesoNeto
            t_Operador
            t_Destino
            f_FechaDocumento
            t_Recinto
            t_Sellos
            t_SellosE
            t_Candados
            t_TanqueVopak
        End Enum

#End Region

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.CopiasSimples

        End Sub

#End Region

    End Class

End Namespace