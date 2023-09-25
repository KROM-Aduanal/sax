Imports gsol.krom

Namespace gsol.krom

    Public Class LDAHidrocarburos
        Inherits EntidadDatos

#Region "Enums"

        Public Enum ReporteIndicadoresLDAHidrocarburos
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
            t_Referencia
            t_Buque
            t_PedimentoCompleto
            t_Producto
            t_RazonSocial
            d_Total
            d_Saldo
            t_UnidadMedida
            f_FechaFolio
            t_FechaFolioNombreDia
            i_FechaFolioNumeroDia
            i_FechaFolioHora
            f_FechaSalida
            t_FechaSalidaNombreDia
            i_FechaSalidaNumeroDia
            i_FechaSalidaHora
            f_FechaEmisionCFDI
            t_FechaEmisionCFDINombreDia
            i_FechaEmisionCFDINumeroDia
            i_FechaEmisionCFDIHora
            d_TotalFolio
            d_SaldoRegistro
            i_Contador
            t_Folio
            t_FolioCFDI
            t_OrdenCarga
            t_LineaTransportista
            t_NumeroEconomico
            t_Destino
            t_DestinoPlanta
        End Enum

        Public Enum ReporteLDAHidrocarburos
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
            t_Referencia
            t_Buque
            t_PedimentoCompleto
            t_Producto
            t_RazonSocial
            d_Total
            d_Saldo
            t_UnidadMedida
            f_InicioCarga
            f_FinCarga
            i_Consecutivo
            t_Folio
            f_FechaFolio
            f_FechaSalida
            t_FolioCFDI
            f_FechaEmisionCFDI
            t_TanqueSiloCompartimiento
            t_OrdenCarga
            t_LineaTransportista
            t_Placa
            t_NumeroEconomico
            t_Destino
            t_DestinoPlanta
            d_TotalFolio
            d_SaldoRegistro
            i_Cve_DocumentoTicket
            i_Cve_DocumentoCFDITraslado
            i_Cve_DocumentoCFDITrasladoPDF
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

#End Region

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.LDAHidrocarburos

        End Sub

#End Region

    End Class

End Namespace