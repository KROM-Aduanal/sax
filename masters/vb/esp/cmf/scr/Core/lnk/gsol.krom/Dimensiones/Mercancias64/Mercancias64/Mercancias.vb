Namespace gsol.krom

    Public Class Mercancias
        Inherits EntidadDatos

#Region "Enums"

        Enum ReporteEstandarMercancias
            i_Cve_Referencia
            i_Cve_EncabezadoFactura
            i_Cve_DetalleFactura
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
            sdpc_t_RazonSocialProveedor
            t_NumeroParte
            t_DescripcionMercancia
            t_CodigoProducto
            t_Marca
            t_Modelo
            t_NoSerie
            t_OrdenCompra
            i_CostoUnitario
            i_TotalValorMonedaExtranjera
            i_TotalValorMonedaDolares
            i_UMC
            i_CantidadUMC
            t_MedidaUMC
            i_Cve_AgenciaAduanal
            t_AgenciaAduanal
            i_Cve_Estado

        End Enum

        Enum Vt022MercanciasOtrosCampos
            i_Cve_DetalleFactura
            i_Cve_OtrosCamposMercancia
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
            i_Cve_DetalleFacturaExterna
            sdpc_t_NumeroFactura
            i_NumeroSecuencia
            t_ISDNumber
            i_Cartons
        End Enum

#End Region

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.Facturas

        End Sub

#End Region

    End Class

End Namespace
