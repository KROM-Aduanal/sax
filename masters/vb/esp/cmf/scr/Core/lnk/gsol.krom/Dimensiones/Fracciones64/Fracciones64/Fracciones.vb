Imports Wma.Exceptions

Namespace gsol.krom

    Public Class Fracciones
        Inherits EntidadDatos

#Region "Enums"

        Enum ReporteEstandarFracciones
            t_OrdenCompra
            i_Cve_Referencia
            i_Cve_VinFraccion
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
            spa_t_NumeroFraccion
            spa_t_DescipcionFraccion
            spa_i_UMC
            t_MedidaUMC
            spa_d_CantidadUMC
            spa_i_UMT
            t_MedidaUMT
            spa_d_CantidadUMT
            spa_t_PaisVendedorComprador
            spa_t_PaisOrigenDestino
            spa_i_ImporteIGIIGE
            spa_i_ImporteIVA
            spa_t_ClaveIdentificador
            spa_t_Complemento1
            spa_t_Complemento2
            spa_t_Complemento3
            i_Cve_AgenciaAduanal
            t_AgenciaAduanal
            i_Cve_Estado

        End Enum

#End Region

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.Fracciones

        End Sub

#End Region

    End Class

End Namespace