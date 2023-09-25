Imports gsol.krom
Imports Wma.Exceptions

Namespace gsol.krom

    Public Class Referencia
        Inherits EntidadDatos

        Enum ConsultaGlobalSynapsis
            i_Cve_MaestroOperaciones
            t_NumeroReferencia
            t_rutaBusqueda
            t_Granularidad
            i_Cve_Estado
        End Enum

        Public Enum Vt009MaestroOperaciones

            i_Cve_MaestroOperaciones
            i_Cve_EstatusTracking
            t_ClaveAccionTracking
            t_EstatusReferencia
            t_Referencia
            t_TipoReferencia
            i_Cve_DivisionMiEmpresa
            t_Empresa
            f_FechaRegistro
            i_Cve_VinClienteOperacion
            t_ClienteOperacion
            i_Cve_VinClienteFacturacion
            t_ClienteFacturacion
            i_Cve_UsuarioRegistro
            t_NombrePersona
            i_Cve_Ejecutivo
            t_NombreCompleto
            f_FechaModificacion
            i_TipoReferencia
            i_TipoReferenciaTracking
            t_ClavesAnticipos
            i_Cve_Estado
            i_Cve_Estatus
            t_Estatus
            i_Cve_EstadoOperacion
            t_EstadoOperacion

        End Enum

        Public Enum Vt009MaestroOperacionesOtrosCampos

            i_Cve_OtrosCampos
            i_Cve_MaestroOperaciones
            i_Cve_DivisionMiEmpresa
            i_Cve_ClienteOperativo
            t_ReferenciaCliente
            t_NumeroLegajo
            t_OrdenCompra
            i_Cve_Estado

            'BASF
            t_Planeador
            t_CentroGastos
            t_Dnote
            t_ObservacionesRectificacion
            t_SBU
            t_MaterialPeligroso
            i_MaterialPeligroso

            t_Naviera
            t_TransportistaNacional
            i_Modalidad
            t_Reexpedidora
            f_FechaContenedorPiso
            f_fechaUVA
            f_fechaEntregaPlanta
            t_Planta

            'BASF
            i_NCM
            t_DG
            i_EfectividadAA
            i_EfectividadBASF
            i_EfectividadTransporte

            t_NCM
            t_EfectividadAA
            t_EfectividadBASF
            t_EfectividadTransporte

            t_TipoRevision

            i_CodigoPlanta
            t_DestinoLugarEntrega
            t_TipoUnidad
            i_TipoRevision
            f_EntregaEnDestino
            i_MaterialDanado
            i_CantidadDanioKG
            i_MontoDaniadoUSD
            i_EmbarqueUrgente

        End Enum

        Enum ReporteEstandarPedimentos
            i_CantidadPartesII
            i_CantidadCopiasSimples
            i_CantidadRemesas
            i_CantidadContenedores
            t_OrdenCompra
            t_Pedimento
            t_NumerosContenedor
            i_Cve_Referencia
            t_NumeroReferencia
            sp_i_Patente
            t_AgenteAduanal
            sp_i_Aduana
            sp_i_Seccion
            sp_i_AduanaES
            t_NombreAduana
            sp_i_NumeroPedimento
            sp_i_TipoOperacion
            sp_t_TipoOperacion
            sp_t_ClavePedimento
            sp_t_ClavePedimentoOriginal
            sp_i_Anio
            sp_d_TipoCambio
            sp_d_PesoBruto
            sp_d_ValorDolares
            sp_i_ValorAduana
            sp_d_PrecioPagadoValorComercia
            sdie_t_RFC
            sdie_t_RazonSocial
            sdie_d_Seguros
            sdie_d_Fletes
            sdie_d_Embalajes
            sdie_d_OtrosIncrementables
            sdie_t_MarcasNumerosContenedor
            sc_f_FechaPago
            sc_f_FechaEntrada
            scl_dta_i_Importe
            scl_prv_i_Importe
            scl_cnt_i_Importe
            scl_iva_i_Importe
            scl_igi_i_Importe
            scl_cc_i_Importe
            scl_isan_i_Importe
            scl_ieps_i_Importe
            scl_rec_i_Importe
            scl_otros_i_Importe
            scl_rt_i_Importe
            scl_bss_i_Importe
            scl_eur_i_Importe
            scl_reu_i_Importe
            scl_eci_i_Importe
            scl_dfc_i_Importe
            scl_mt_i_Importe
            scl_itv_i_Importe
            scl_ivaprv_i_Importe

            scl_ieps_gas_i_Importe
            scl_2ib_i_importe
            scl_2ia2_i_importe
            scl_2ia1_i_importe
            scl_2ic_i_importe
            scl_2if_i_importe
            scl_2ig_i_importe
            scl_2ij_i_importe
            scl_2ii_i_importe
            scl_icf_i_importe
            scl_iepsdie_i_importe
            scl_icnf_i_importe
            scl_lieps_i_importe

            scl_total_importe
            t_Modalidad
            f_FechaDespacho
            f_RegistroReferencia
            f_UltimaModificación
            t_Semaforo
            t_EstatusOperacion
            t_DivisionMiEmpresa
            t_SemaforoVR
            i_Cve_Modalidad
        End Enum

        Enum Ext022ReferenciasAgenciasAduanales
            i_Cve_Referencia
            t_NumeroReferencia
            sp_i_Patente
            sp_i_Aduana
            sp_i_Seccion
            sp_i_AduanaES
            sp_i_NumeroPedimento
            sp_i_TipoOperacion
            sp_t_TipoOperacion
            sp_t_ClavePedimento
            sp_t_ClavePedimentoOriginal
            sp_i_Anio
            sp_d_TipoCambio
            sp_d_PesoBruto
            sp_d_ValorDolares
            sp_i_ValorAduana
            sp_d_PrecioPagadoValorComercia
            sdie_t_RFC
            sdie_t_RazonSocial
            sdie_d_Seguros
            sdie_d_Fletes
            sdie_d_Embalajes
            sdie_d_OtrosIncrementables
            sdie_t_MarcasNumerosContenedor
            sc_f_FechaPago
            sc_f_FechaEntrada
            scl_dta_i_Importe
            scl_prv_i_Importe
            scl_cnt_i_Importe
            scl_iva_i_Importe
            scl_igi_i_Importe
            i_Cve_SistemaTrafico
            i_Cve_Modalidad
            f_FechaDespacho
            f_RegistroReferencia
            f_UltimaModificación
            i_Cve_EjecutivoCuenta
            i_Cve_Tracking
            t_SemaforoVR
            b_OperacionDespachada
            i_Cve_DivisionMiEmpresa
            i_Cve_Estado
            i_Cve_Estatus
        End Enum

        Enum AtributosDimensionReferencias
            i_Cve_Referencia = 0
            t_NumeroReferencia = 1
            i_Cve_MaestroOperaciones
            i_Cve_ClienteOperacion
            i_Cve_ClienteComprobanteFiscal
            i_Cve_Tracking
            i_Cve_TipoOperacion
            i_Cve_Modalidad
            t_NombreClienteOperacion
            f_FechaAlta
            f_FechaDespacho
            f_FechaPagoPedimento
            f_EjecutivoCuenta
            t_Estatus
            i_Pago
            f_FechaEntrada
            i_Patente
            i_ClaveAduana
            i_ValorAduana
            t_Mercancia

            i_Cve_VinOperacionesAgenciasAduanales
            i_Cve_ReferenciaExterna
            i_Anio
            i_AduanaSeccion
            i_NumeroPedimento
            i_NumeroPedimentoCompleto
            i_TipoOperacion
            i_Cve_Estado
            i_Cve_Estatus
            i_Cve_DivisionMiEmpresa
            f_FechaRegistroVinculacion
            i_Cve_Sistema
            f_FechaUltimoEstatus
            i_Cve_TrackingReciente
            t_Cve_TrackingReciente
            i_Despachada
            i_Cve_Cliente_Externo
            t_RfcExterno
            t_Cve_Pedimento
            i_Cve_DivisionEmpresarialCliente
            i_Cve_VinClienteOperacion
            t_NombreCliente
            f_FechaAltaOperacion
            f_FechaEstimadaArribo
            t_DescripcionNaveBuque
            t_CantidadBultos
            f_FechaRegistroHistorico
            h_HoraRegistroHistorico
            i_cve_MedioTransporteArribo
            f_FechaLiquidacionFactura
            t_Booking
        End Enum

        Enum Vt022ConsultaOperaciones
            i_CantidadPartesII
            i_CantidadCopiasSimples
            i_CantidadRemesas
            i_CantidadContenedores
            t_OrdenCompra
            t_NumerosContenedor
            i_RutaDocumentos
            t_Semaforo
            t_Ejecutivo
            t_NombreAduana
            i_EntVsDes
            i_Cve_Referencia
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
            t_NumeroReferencia_PDF
            t_Pedimento
            sp_t_TipoOperacion
            sp_t_ClavePedimento
            sdie_t_RazonSocial
            sdie_t_MarcasNumerosContenedor
            sc_f_FechaPago
            sc_f_FechaEntrada
            f_FechaDespacho
            i_TipoModalidad
            t_TipoModalidad
            i_Cve_AgenciaAduanal
            t_AgenciaAduanal
            t_SemaforoVR
            b_FechaDespacho
            i_Cve_Estado
        End Enum

        Enum ReporteEstandarOperacionesFreightForwarder
            i_Cve_MaestroOperaciones
            i_Cve_DivisionMiEmpresa
            t_RFCExterno
            i_Cve_ClienteOperativo
            i_Cve_DivisionEmpresarialCliente
            i_AduanaSeccion
            i_Patente
            i_TipoOperacion
            i_Cve_ClienteComprobanteFiscal
            t_RFCComprobanteFiscal
            i_Cve_ReferenciasFreightForwarder
            t_Referencia
            f_FechaAlta
            i_CveOficina
            t_Oficina
            t_TipoOperacion
            i_Cve_TipoModalidad
            t_Modalidad
            t_Aduanaseccion
            t_NombreAduanaseccion
            t_ReferenciaAgenciaAduanal
            t_ReferenciaCliente
            i_Cliente
            t_Cliente
            t_Mercancia
            i_TipoCarga
            t_TipoEmbalaje
            d_PesoBruto
            i_UnidadMedida
            t_UnidadMedida
            d_TotalPiezas
            i_PaisCarga
            t_PaisCarga
            i_PuertoCarga
            t_Cve_PuertoCarga
            t_PuertoCarga
            i_PaisDescarga
            t_PaisDescarga
            i_PuertoDescarga
            t_Cve_PuertoDescarga
            t_PuertoDescarga
            f_FechaZarpe
            t_MasterBL
            t_Nave
            t_Observaciones
            t_Division
            i_Cve_Naviera
            t_Naviera
            t_Caat
            t_NumeroViaje
            f_ETA
            i_TipoMovimiento
            t_TipoMovimiento
            t_LugarRecepcion
            i_TipoPrecarriage
            t_TipoPreCarriage
            t_LugarEntrega
            i_TipoInland
            t_TipoInland
            t_Booking
            f_FechaBooking
            f_FechaCanjeDocumentos
            f_FechaLLegadaDestino
            f_ETS
            t_NumeroTracking
            i_TipoMaterial
            t_NumeroFactura
            t_NombreProveedor
            t_PO
            i_Cve_EstatusOperacion
            t_EstatusOperacion
            t_PaisCargaIngles
            t_PaisDescargaIngles
            d_ValorCompra
            d_ValorVenta
            f_FechaPosicionamiento
            f_FechaInicioRuta
            f_FechaEntregado
        End Enum

        Enum ConsultaOperacionesFreightForwarder
            t_Cve_TrackingReciente
            f_FechaUltimoEstatus
            t_Ejecutivo
            i_Cve_MaestroOperaciones
            i_Cve_DivisionMiEmpresa
            t_RFCExterno
            i_Cve_ClienteOperativo
            i_Cve_DivisionEmpresarialCliente
            i_AduanaSeccion
            i_Patente
            i_TipoOperacion
            i_Cve_ClienteComprobanteFiscal
            t_RFCComprobanteFiscal
            i_Cliente
            t_Cliente
            i_Cve_ReferenciasFreightForwarder
            t_Referencia
            f_FechaAlta
            i_CveOficina
            t_Oficina
            t_TipoOperacion
            t_MasterBL
            t_Mercancia
            i_Cve_TipoModalidad
            t_Modalidad
            t_Nave
            i_Cve_EstatusOperacion
            t_EstatusOperacion
            t_ReferenciaAgenciaAduanal
            i_RutaDocumentos
            t_NumeroReferencia_PDF
            i_ClaveMaestro
        End Enum

        Enum KPIOperaciones
            i_TipoReferencia
            i_Cve_DivisionMiEmpresa
            t_RFCExterno
            t_NombreCliente
            i_Cve_VinClienteOperacion
            i_Cve_DivisionEmpresarialCliente
            i_AduanaSeccion
            t_NombreAduana
            i_Patente
            i_Cve_ClienteOperativoExterno
            i_Cve_ClienteOperativo
            i_Cve_ClienteComprobanteFiscal
            t_RFCComprobanteFiscal
            i_Dia
            t_Dia
            t_Semana
            i_Mes
            t_Mes
            i_Anio
            sc_f_FechaPago
            i_TotalOperaciones
            i_OperacionesImpo
            i_OperacionesExpo
            i_OperacionesMaritimas
            i_OperacionesTerrestres
            i_OperacionesAereas
            i_OperacionesMultimodal
            i_OperacionesVivas
            i_OperacionesSinPagar
            i_OperacionesPagadas
            i_OperacionesDespachadas
            i_OperacionesSemVerde
            i_OperacionesSemRojo
        End Enum

        Enum ConsultaOperacionesVivas
            t_OrdenCompra
            t_Cve_TrackingReciente
            f_FechaUltimoEstatus
            t_Ejecutivo
            i_Cve_Referencia
            i_Cve_MaestroOperaciones
            i_Cve_DivisionMiEmpresa
            t_RFCExterno
            i_Cve_VinClienteOperacion
            i_Cve_DivisionEmpresarialCliente
            i_AduanaSeccion
            i_Patente
            i_TipoOperacion
            i_Cve_ClienteOperativoExterno
            i_Cve_ClienteOperativo
            i_Cve_ClienteComprobanteFiscal
            t_RFCComprobanteFiscal
            i_Cve_ReferenciaExterna
            t_NumeroReferencia
            sp_i_Patente
            t_AgenteAduanal
            sp_i_Aduana
            sp_i_Seccion
            sp_i_AduanaES
            t_NombreAduana
            sp_i_NumeroPedimento
            t_Pedimento
            sp_i_TipoOperacion
            sp_t_TipoOperacion
            sp_t_ClavePedimento
            sp_t_ClavePedimentoOriginal
            sp_i_Anio
            sp_d_TipoCambio
            sp_d_PesoBruto
            sp_d_ValorDolares
            sp_i_ValorAduana
            sp_d_PrecioPagadoValorComercia
            sdie_t_RFC
            sdie_t_RazonSocial
            sdie_d_Seguros
            sdie_d_Fletes
            sdie_d_Embalajes
            sdie_d_OtrosIncrementables
            sdie_t_MarcasNumerosContenedor
            sc_f_FechaPago
            sc_f_FechaEntrada
            scl_dta_i_Importe
            scl_prv_i_Importe
            scl_cnt_i_Importe
            scl_iva_i_Importe
            scl_igi_i_Importe
            i_Cve_SistemaTrafico
            i_TipoModalidad
            t_TipoModalidad
            f_RegistroReferencia
            f_UltimaModificacion
            i_Cve_EjecutivoCuenta
            i_Cve_Tracking
            t_Estatus
            i_DiferenciaDias
            t_DivisionMiEmpresa
            i_Cve_Estatus
            i_Cve_Estado
            i_Cve_AgenciaAduanal
            t_AgenciaAduanal
        End Enum

        Enum Vt000Domicilios
            i_Cve_Domicilio
            t_Calle
            t_NumeroExterior
            t_NumeroInterior
            t_Colonia
            t_CodigoPostal
            t_Municipio
            i_Cve_EstadoPais
            t_Cve_Pais
            i_Cve_Estado
            i_Cve_Tipo
            i_Cve_Ciudad
            t_Localidad
            i_Cve_Estatus
        End Enum

        Enum Vt000Personas
            i_Cve_Persona
            t_Nombre
            t_ApellidoPaterno
            t_ApellidoMaterno
            i_Cve_Estado
            i_Cve_Tipo
            t_RFC
            t_CURP
            i_Cve_Estatus
            t_Estatus
        End Enum

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.Referencias

        End Sub

#End Region

    End Class


End Namespace