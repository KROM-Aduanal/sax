Imports gsol.krom

Namespace gsol.krom

    Public Class Anexo22
        Inherits EntidadDatos

        Enum Vt022AduanaSeccionA01
            i_Cve_AduanaSeccion
            t_Cve_Aduana
            t_Cve_Seccion
            t_AduanaSeccionDenominacion
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022ClavesPedimentoA02
            i_Cve_ClavePedimento
            t_Cve_Pedimento
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022MediosTransporteA03
            i_Cve_MedioTransporte
            t_Cve_MedioTransporte
            t_MedioTransporte
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022PaisesA04
            i_Cve_Pais
            t_Pais
            t_ClaveSAAIFIII
            t_ClaveSAAIM3
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022MonedasA05
            i_Cve_Moneda
            t_Pais
            t_Cve_Moneda
            t_NombreMoneda
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022RecintosFiscalizadosA06
            i_Cve_RecintoFiscalizado
            t_Aduana
            i_ClaveRecintoFiscalizado
            t_RecintoFiscalizado
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022UnidadesMedidaA07
            i_Cve_UnidadMedida
            i_ClaveUnidadMedida
            t_DescripcionUnidadMedida
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022IdentificadoresA08
            i_Cve_Identificador
            t_Cve_Identificador
            t_Identificador
            t_Nivel
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022RegulacionesRestriccionesNoArancelariasA09
            i_Cve_RegulacionRestriccionNoArancelaria
            t_Cve_RegulacionRestriccionNoArancelaria
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022TiposContenedoresVehiculosTransporteA10
            i_Cve_TipoContenedorVehiculoTransporte
            i_ClaveTipoContenedorVehiculoTransporte
            t_DescripcionTipoContenedorVehiculoTransporte
            i_EsContenedor
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022MetodosValoracionA11
            i_Cve_MetodoValoracion
            i_ClaveMetodoValoracion
            t_DescripcionMetodoValoracion
            t_DescripcionCorta
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022ContribucionesA12
            i_Cve_Contribucion
            i_ClaveContribucion
            t_Contribucion
            t_Abreviacion
            t_Nivel
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022FormasPagoA13
            i_Cve_FormaPago
            i_ClaveFormaPago
            t_DescripcionFormaPago
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022TerminosFacturacionA14
            i_Cve_TerminoFacturacion
            i_ClaveTerminoFacturacion
            t_Cve_TerminoFacturacion
            t_TerminoFacturacion
            t_DescripcionCorta
            t_ValorPresentacion
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022DestinosMercanciasA15
            i_Cve_DestinoMercancia
            i_ClaveDestinoMercancia
            t_DescripcionDestinoMercancia
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022RegimenesA16
            i_Cve_Regimen
            t_Cve_Regimen
            t_DescripcionRegimen
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022TiposTasasA18
            i_Cve_TipoTasa
            i_ClaveTipoTasa
            t_DescripcionTipoTasa
            f_FechaRegistro
            i_Cve_Estatus
            i_Cve_Estado
        End Enum

        Enum Vt022RecintosFiscalizadosEstrategicosA21
            i_Cve_RecintoFiscalizadoEstrategico
            t_Aduana
            t_Cve_RFE
            t_AdministradorRFE
            t_Cve_Operador
            t_OperadorRFE
            i_Cve_Estatus
            i_Cve_Estado
        End Enum


#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.Anexo22

        End Sub

#End Region

    End Class


End Namespace