Namespace gsol.krom

    Public Class Facturas
        Inherits EntidadDatos

#Region "Enums"

        Enum ReporteEstandarFacturas
            t_OrdenCompra
            i_Cve_Referencia
            i_Cve_EncabezadoFactura
            i_Cve_MaestroOperaciones
            i_Cve_DivisionMiEmpresa
            t_RFCExterno
            i_Cve_VinClienteOperacion
            i_Cve_DivisionEmpresarialCliente
            i_AduanaSeccion
            i_Patente
            i_TipoOperacion
            t_NumeroReferencia
            t_Pedimento
            sp_i_TipoOperacion
            sp_t_TipoOperacion
            sdie_t_RFC
            sdie_t_RazonSocial
            sc_f_FechaPago
            sc_f_FechaEntrada
            i_Cve_Modalidad
            t_Modalidad
            f_FechaDespacho
            sdpc_t_NumeroFactura
            sdpc_f_FechaFactura
            sdpc_t_Incoterm
            sdpc_t_MonedaFactura
            sdpc_d_ValorMonedaFactura
            sdpc_d_FactorMonedaFactura
            sdpc_d_ValorDolares
            sdpc_t_RazonSocialProveedor
            sdpc_t_DomicilioProveedor
            sdpc_t_IdFiscal
            i_Cve_AgenciaAduanal
            t_AgenciaAduanal
            i_Cve_Estado

        End Enum

        Enum Vt022FacturaComercialOtrosCampos
            i_Cve_EncabezadoFactura
            i_Cve_OtrosCamposFactura
            i_Cve_Referencia
            t_NumeroReferencia
            i_Cve_DivisionMiEmpresa
            i_TipoReferencia
            t_RazonSocialCliente
            t_RFC
            i_Cve_ClienteOperativo
            i_Cve_EjecutivoCuenta
            t_Ejecutivo
            i_Cve_Estado
            i_Cve_FacturaExterna
            sdpc_t_NumeroFactura
            t_MasterBL
            t_HouseBL
        End Enum

#End Region

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.Facturas

        End Sub

#End Region

    End Class

End Namespace