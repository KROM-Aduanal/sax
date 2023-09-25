Imports Wma.Exceptions

Namespace gsol.krom
    Public Class CuentaGastos
        Inherits EntidadDatos

        Enum ReporteEstandarCuentaGastosFreightForwarder
            i_Cve_TipoModalidadFactura
            i_Cve_Factura
            i_Cve_MaestroOperaciones
            i_Cve_DivisionMiEmpresa
            t_RFCExterno
            i_Cve_VinClienteOperacion
            i_Cve_DivisionEmpresarialCliente
            i_AduanaSeccion
            i_Patente
            i_TipoOperacion
            i_Cve_EstatusFactura
            t_EstatusFactura
            t_FolioFactura
            f_FechaEmision
            t_Descripcion
            i_SubTotal
            i_ImporteTotal
            i_TotalPagosHechos
            i_Honorarios
            i_ServiciosComplementarios
            i_CostosAduanales
            i_CostosDirectos
            i_PorcentajeIva
            i_OtrosIngresos
            f_FechaRecepcionFactura
            t_UUID
            i_Cve_MetodoPago
            t_DescMetodoPago
            t_TipoFactura
            i_Cve_ClienteOperacion
            t_RFCClienteOperacion
            t_ClienteOperacion
            t_Corresponsalia
            i_ImporteAnticipo
            i_SubtotalInformeGastos
            i_ImporteTotalInformeGastos
            t_ObservacionesRecepcionFactura
            i_Cve_ClienteComprobanteFiscal
            t_RFCClienteFacturacion
            t_ClienteComprobanteFiscal
            f_FechaCancelacionCFDi
            i_TipoOperacionImpExp
            t_TipoOperacion
            t_ReferenciasFacturas
            c_UsoCFDi
            t_UsoCFDi
            t_TipoComprobante
            i_Cve_AgenciaAduanal
            t_AgenciaAduanal
            i_Cve_EstadoFactura
            i_Cve_Estado

        End Enum

        Enum ConsultaCuentaGastosFreightForwarder

            t_ClienteOperacion

            i_Cve_Factura
            i_Cve_MaestroOperaciones
            i_Cve_DivisionMiEmpresa
            t_RFCExterno
            i_Cve_VinClienteOperacion
            i_Cve_DivisionEmpresarialCliente
            i_AduanaSeccion
            i_Patente
            i_TipoOperacion
            t_ReferenciasFactura
            t_FolioFactura
            f_FechaEmision
            t_UUID
            i_Cve_EstatusFactura
            t_EstatusFactura
            i_Cve_ClienteOperativo
            sdie_t_RFC
            sdie_t_RazonSocial
            i_Cve_ClienteComprobanteFiscal
            t_RFCComprobanteFiscal
            t_ClienteComprobanteFiscal
            i_Cve_AgenciaAduanal
            t_AgenciaAduanal
            i_Cve_Factura_PDF
            i_Cve_Factura_XML
            i_Cve_Factura_zip
            i_Cve_TipoComprobante
            t_TipoComprobante
            t_MetodoPago
            t_FormaPago
            t_Descripcion
            i_Cve_Estatus
            i_Cve_Estado

        End Enum

        Enum vt025EstadoCuentaOperaciones
            t_Aduana
            t_Oficina
            i_Cve_MaestroOperaciones
            i_Cve_DivisionMiEmpresa
            t_RFCExterno
            i_Cve_VinClienteOperacion
            i_Cve_DivisionEmpresarialCliente
            i_AduanaSeccion
            i_Patente
            i_TipoOperacion
            i_Cve_ClienteOperacion
            i_Cve_ClienteComprobanteFiscal
            t_RFCComprobanteFiscal
            Referencia
            t_RFCClienteOperacion
            t_ClienteOperacion
            Pedimento
            TOperacion
            Emision
            CG
            t_UUID
            Anticipos
            Gastos
            Saldo
            RecepcionFactura
            ClaveCliente
            ClienteFacturacion
            RFC_Facturacion
            Direccion
            i_Cve_Estado
        End Enum

        Enum Vt022ConsultaCuentaGastos
            t_Oficina
            t_Pedimento
            t_ClienteOperacion
            i_Cve_Factura
            i_Cve_MaestroOperaciones
            i_Cve_DivisionMiEmpresa
            t_RFCExterno
            i_Cve_VinClienteOperacion
            i_Cve_DivisionEmpresarialCliente
            i_AduanaSeccion
            i_Patente
            i_TipoOperacion
            t_ReferenciasFactura
            t_FolioFactura
            f_FechaEmision
            f_FechaRecepcion
            t_UUID
            i_Cve_EstatusFactura
            t_EstatusFactura
            i_Cve_ClienteOperativo
            sdie_t_RFC
            sdie_t_RazonSocial
            i_Cve_ClienteComprobanteFiscal
            t_RFCComprobanteFiscal
            t_ClienteComprobanteFiscal
            i_Cve_AgenciaAduanal
            t_AgenciaAduanal
            i_Cve_Factura_PDF
            i_Cve_Factura_XML
            i_Cve_Factura_zip
            i_Cve_Factura_cpg
            i_Cve_Factura_cpt
            i_Cve_TipoComprobante
            t_TipoComprobante
            t_MetodoPago
            t_FormaPago
            t_Descripcion
            i_Cve_Estatus
            i_Cve_Estado

        End Enum

        Enum ReporteEstandarCuentaGastos
            i_Cve_TipoModalidadFactura
            i_Cve_Factura
            i_Cve_MaestroOperaciones
            i_Cve_DivisionMiEmpresa
            t_RFCExterno
            i_Cve_VinClienteOperacion
            i_Cve_DivisionEmpresarialCliente
            i_AduanaSeccion
            i_Patente
            i_TipoOperacion
            i_Cve_EstatusFactura
            t_EstatusFactura
            t_FolioFactura
            f_FechaEmision
            f_FechaRecepcion
            t_Descripcion
            i_SubTotal
            i_ImporteTotal
            i_TotalPagosHechos
            i_Honorarios
            i_ServiciosComplementarios
            i_CostosAduanales
            i_CostosDirectos
            i_PorcentajeIva
            i_OtrosIngresos
            f_FechaRecepcionFactura
            t_UUID
            i_Cve_MetodoPago
            t_DescMetodoPago
            t_TipoFactura
            i_Cve_ClienteOperacion
            t_RFCClienteOperacion
            t_ClienteOperacion
            t_Corresponsalia
            i_ImporteAnticipo
            i_SubtotalInformeGastos
            i_ImporteTotalInformeGastos
            t_ObservacionesRecepcionFactura
            i_Cve_ClienteComprobanteFiscal
            t_RFCClienteFacturacion
            t_ClienteComprobanteFiscal
            f_FechaCancelacionCFDi
            i_TipoOperacionImpExp
            t_TipoOperacion
            t_ReferenciasFacturas
            c_UsoCFDi
            t_UsoCFDi
            t_TipoComprobante
            i_Cve_AgenciaAduanal
            t_AgenciaAduanal
            i_Cve_EstadoFactura
            i_Cve_Estado

        End Enum

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.Vt022ConsultaCuentaGastos

        End Sub

#End Region


    End Class

End Namespace
