Imports System.ComponentModel
Imports System.Runtime.Serialization
Imports System.Web.Script.Serialization
Imports Syn.Documento
Imports Wma.Exceptions

Public Interface IPrevalidador : Inherits ICloneable, IDisposable

#Region "Enums"

    Enum TiposProcesamiento

        SinDefinir = 0 'N0
        AsistirCaptura = 1 'N1
        GenerarPartidas = 2 'N2
        PrevalidarPedimento = 3 'N3
        GenerarArchivoValidación = 4 'N4

    End Enum

    Enum TiposValidacion

        SinDefinir = 0
        Legal = 1 'V1
        UsosCostumbres = 2 'V2
        Contigencia = 3 'V3
        Personalizada = 4 'V4
        Completa = 5 'V5

    End Enum

    Enum TiposRutaValidacion

        <EnumMember> <Description("Sin definir")> RUVA0 = 0
        <EnumMember> <Description("Normal importación por defecto")> RUVA1 = 1
        <EnumMember> <Description("Normal importación global complementario")> RUVA2 = 2
        <EnumMember> <Description("Normal importación pedimento consolidado")> RUVA3 = 3
        <EnumMember> <Description("Normal exportación por defecto")> RUVA4 = 4
        <EnumMember> <Description("Normal exportación pedimento consolidado")> RUVA5 = 5
        <EnumMember> <Description("Normal exportación global complementario")> RUVA6 = 6
        <EnumMember> <Description("Normal exportación pedimento complementario")> RUVA7 = 7
        <EnumMember> <Description("Normal tránsito por defecto")> RUVA8 = 8
        <EnumMember> <Description("Virtual exportación por defecto")> RUVA9 = 9
        <EnumMember> <Description("Virtual exportación pedimento consolidado")> RUVA10 = 10
        <EnumMember> <Description("Virtual importación por defecto")> RUVA11 = 11
        <EnumMember> <Description("Virtual importación pedimento consolidado")> RUVA12 = 12
        <EnumMember> <Description("Rectificación normal importación por defecto")> RUVA13 = 13
        <EnumMember> <Description("Rectificación normal importación pedimento consolidado")> RUVA14 = 14
        <EnumMember> <Description("Rectificación normal exportación por defecto")> RUVA15 = 15
        <EnumMember> <Description("Rectificación normal exportación pedimento complementario")> RUVA16 = 16
        <EnumMember> <Description("Rectificación normal exportación pedimento consolidado")> RUVA17 = 17
        <EnumMember> <Description("Rectificación normal tránsito por defecto")> RUVA18 = 18
        <EnumMember> <Description("Rectificación virtual importación")> RUVA19 = 19
        <EnumMember> <Description("Rectificación virtual exportación")> RUVA20 = 20

    End Enum

    Enum TiposProcesamientoElectronico

        SinDefinir = 0
        Prevalidacion = 1
        ValidacionPedimento = 2
        PagoPedimento = 3
        DesistimientoElectronico = 4
        BorradFirmaValidacion = 5
        CartaCupo = 6
        InformeIndustriaAutomotriz = 7
        PrevioConsolidado = 8

    End Enum

#End Region

#Region "Propiedades"

    Property TipoProcesamiento As TiposProcesamiento

    Property TipoValidacion As TiposValidacion

    Property TipoRutaValidacion As TiposRutaValidacion

    Property EstatusOperacion As TagWatcher

    Property Estatus As TagWatcher

#End Region

#Region "Metodos"

    Function AnalisisValidacion(ByVal tipoProcesamiento_ As TiposProcesamiento,
                                ByVal tipoValidacion_ As TiposValidacion,
                                ByVal rutaValidacion_ As TiposRutaValidacion,
                                Optional ByVal pedimento_ As DocumentoElectronico = Nothing) As TagWatcher

    Function AnalisisValidacion(ByVal tipoProcesamiento_ As TiposProcesamiento,
                                ByVal tipoValidacion_ As TiposValidacion,
                                ByVal rutaValidacion_ As TiposRutaValidacion,
                                Optional ByVal pedimento_ As JavaScriptSerializer() = Nothing) As TagWatcher

    'Cambiamos a Tagwatcher por que se vuelve ciclico pero en teoría dijimos que se debe devolver el InformePrevalidacion
    Function ObtenerInformeValidacion(ByVal folioOperacion_ As Int32,
                                      Optional ByVal pedimento_ As DocumentoElectronico = Nothing) As TagWatcher

    'Cambiamos a Tagwatcher por que se vuelve ciclico pero en teoría dijimos que se debe devolver el InformePrevalidacion
    Function ObtenerInformeValidacion(ByVal folioOperacion_ As Int32,
                                      Optional ByVal pedimento_ As JavaScriptSerializer() = Nothing) As TagWatcher

    Function ObtenerRutaValidacion(Optional ByVal pedimento_ As DocumentoElectronico = Nothing) As TagWatcher

    Function ObtenerRutaValidacion(Optional ByVal pedimento_ As JavaScriptSerializer() = Nothing) As TagWatcher

    Function ObtenerEstatusOperacion(ByVal folioOperacion_ As Int32) As TagWatcher

#End Region

End Interface