Imports System.ComponentModel
Imports System.Runtime.Serialization
Imports Wma.Exceptions

Public Interface IPrevalidadorAsistencia
    Inherits ICloneable, IDisposable

#Region "Enums"

    Enum TiposAsistenciaConsultar

        <EnumMember> <Description("SIN DEFINIR")> AS_PED0 = 0
        <EnumMember> <Description("PREV.AS_PED1")> AS_PED1 = 1
        <EnumMember> <Description("PREV.AS_PED2")> AS_PED2 = 2
        <EnumMember> <Description("PREV.AS_PED3")> AS_PED3 = 3
        <EnumMember> <Description("PREV.AS_PED4")> AS_PED4 = 4
        <EnumMember> <Description("PREV.AS_PED5")> AS_PED5 = 5
        <EnumMember> <Description("PREV.AS_PED6")> AS_PED6 = 6
        <EnumMember> <Description("PREV.AS_PED7")> AS_PED7 = 7
        <EnumMember> <Description("PREV.AS_PED8")> AS_PED8 = 8
        <EnumMember> <Description("PREV.AS_PED9")> AS_PED9 = 9
        <EnumMember> <Description("PREV.AS_PED10")> AS_PED10 = 10
        <EnumMember> <Description("PREV.AS_PED11")> AS_PED11 = 11
        <EnumMember> <Description("PREV.AS_PED12")> AS_PED12 = 12
        <EnumMember> <Description("PREV.AS_PED13")> AS_PED13 = 13
        <EnumMember> <Description("PREV.AS_PED14")> AS_PED14 = 14

    End Enum

    Enum ErroresAsistencia

        <EnumMember> <Description("SIN DEFINIR")> EAS_000 = 0
        <EnumMember> <Description("No existe condición para los parámetros recibidos.")> EAS_001 = 1
        <EnumMember> <Description("Sin filtros aplicables.")> EAS_002 = 2
        <EnumMember> <Description("Verifique su información capturada.")> EAS_003 = 3
        <EnumMember> <Description("Error al consultar la asistencia en el cubo.")> EAS_004 = 4
        <EnumMember> <Description("Se recibieron parámetros vacíos.")> EAS_005 = 5
        <EnumMember> <Description("Error al convertir los resultados del cubo a la Asistencia.")> EAS_006 = 6
        <EnumMember> <Description("La asistencia no regreso ningún resultado a procesar.")> EAS_007 = 7
        <EnumMember> <Description("SIN DEFINIR")> EAS_008 = 8
        <EnumMember> <Description("SIN DEFINIR")> EAS_009 = 9
        <EnumMember> <Description("SIN DEFINIR")> EAS_010 = 10

    End Enum

#End Region

#Region "Propiedades"

    Property TipoAsistenciaConsultada As TiposAsistenciaConsultar

    Property ResultadoAsistencia As Asistencia

    Property EstatusAsistencia As TagWatcher

#End Region

#Region "Metodos"

    Function ConsultarAsistencia(ByVal tipoProcesamiento_ As IPrevalidador.TiposProcesamiento,
                                        ByVal tipoValidacion_ As IPrevalidador.TiposValidacion,
                                        ByVal tipoAsistenciaConsultar_ As TiposAsistenciaConsultar,
                                        ByVal parametros_ As Dictionary(Of String, Object)) As TagWatcher

#End Region

End Interface