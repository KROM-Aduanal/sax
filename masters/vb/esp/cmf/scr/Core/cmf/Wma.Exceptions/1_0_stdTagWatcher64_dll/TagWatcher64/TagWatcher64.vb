Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.Reflection
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports IRecursosSistemas.RecursosCMF


Namespace Wma.Exceptions

    Public Class StackTrace
        Property StackObject As Object
        Property Message As String
        Property Resource As String
        Property Type As TagWatcher.TraceTypes
        Property Id As Int32

    End Class

    <DataContract()>
    <XmlSerializerFormat>
    Public Class TagWatcher
        'Implements IExceptions

        Enum TraceTypes
            Err = 0
            Warn = 1
            But = 2
            Info = 3
        End Enum

#Region "Attributes"

        Private _status As TypeStatus

        Private _error As ErrorTypes

        Private _resultsCont As Integer

        Private _description As String

        Private _object As Object

        Private _flag As String

        ' Private _recursoCMF As IRecursosSistemas.RecursosCMF

        Private _lastMessage As String

        Private _messagesList As New List(Of StackTrace)

#End Region

#Region "Enums"

        Enum InfoTypes
            <EnumMember> <Description("Info: No se encontró ningún valor para esta consulta")> G0_000_0000 = 0
            <EnumMember> <Description("Info:G0_000_0001: Not defined")> G0_000_0001 = 2
            <EnumMember> <Description("Info:G0_000_0002: Not defined")> G0_000_0002 = 3
            <EnumMember> <Description("Info:G0_000_0003: Not defined")> G0_000_0003 = 4
        End Enum

        <DataContract()>
        Enum ErrorTypes

            '#############################  OTROS SEGMENTOS DEL CONTROL DE EXCEPCIONES ##################################
            <EnumMember> <Description("")> WS000 = 0
            <EnumMember> <Description("Error:WS-001, Mobile user was not found")> WS001 = 1
            <EnumMember> <Description("Error:WS-002, Information not available")> WS002 = 2
            <EnumMember> <Description("Error:WS-003, Data request was rejected due to authentication errors")> WS003 = 3
            <EnumMember> <Description("Error:WS-004, Data request was rejected due to authentication errors")> WS004 = 4
            <EnumMember> <Description("Error:WS-005, Data request was rejected due to authentication errors")> WS005 = 5
            <EnumMember> <Description("Error:WS-006, Query returns 0 rows")> WS006 = 6
            <EnumMember> <Description("Error:WS-007: Mobile user do not have defined client resources")> WS007 = 7
            <EnumMember> <Description("Error:WS-008: Not enough rules to data access")> WS008 = 8
            <EnumMember> <Description("Error:WS-009: -")> WS009 = 9
            <EnumMember> <Description("Error:WS-010: Update was not commited")> WS010 = 10
            <EnumMember> <Description("Error:WS-011: Insert new record failed")> WS011 = 11
            <EnumMember> <Description("Error:WS-012: Athentication failed")> WS012 = 12
            <EnumMember> <Description("Error:WS-013: Mobile user does not exist")> WS013 = 13
            <EnumMember> <Description("Error:UNDEFINED: Not defined")> Undefined = 14
            <EnumMember> <Description("Error: Not defined,C1_000_0001")> C1_000_0001 = 101
            'Capa 1, Gestión de permisos, Construcción dinamica ej. ( IEspacioTrabajo, ICredenciales, ISesion)
            <EnumMember> <Description("Error: ID del error no implementado o asignado. 666")> C1_000_0666 = 666

            '<EnumMember> <Description("Error:UNDEFINED: Not defined")> Undefined = 0

            'Capa 1, Gestión de permisos, ej. ( IEspacioTrabajo, ICredenciales, ISesion)
            '<EnumMember> <Description("Error:C1_000_0001: No implementado")> C1_000_0001 = 1

            <EnumMember> <Description("Error:C1_000_1000: Not defined")> C1_000_1000 = 1000
            <EnumMember> <Description("Error:C1_000_1001: Not defined")> C1_000_1001 = 1001
            <EnumMember> <Description("Error:C1_000_1002: Not defined")> C1_000_1002 = 1002
            <EnumMember> <Description("Error:C1_000_1003: Not defined")> C1_000_1003 = 1003

            '#############################  L I N E A S   B A S E   #############################################
            'Capa2, lineasbase, ej. (LineaBaseOperaciones, LineaBaseBitacora, LineaBaseIniciaSesion, etc.)
            <EnumMember> <Description("Aviso: Esta modalidad no se encuentra implementeada en las condiciones solicitadas, C2_000_2000")> C2_000_2000 = 2000
            <EnumMember> <Description("Error: No se ha podido acceder al recurso singleton de Configuracion64, C2_000_2001")> C2_000_2001 = 2001
            <EnumMember> <Description("Error: Not defined, C2_000_2002")> C2_000_2002 = 2002
            <EnumMember> <Description("Error: Not defined, C2_000_2003")> C2_000_2003 = 2003
            <EnumMember> <Description("Error: Framework global configuration file has an error,C2_000_2004")> C2_000_2004 = 2004
            <EnumMember> <Description("Error: Not defined, C2_000_2005")> C2_000_2005 = 2005
            <EnumMember> <Description("Error: Not defined, C2_000_2006")> C2_000_2006 = 2006
            <EnumMember> <Description("Error: Not defined, C2_000_2007")> C2_000_2007 = 2007
            <EnumMember> <Description("Error: Not defined, C2_000_2008")> C2_000_2008 = 2008
            <EnumMember> <Description("Error: Not defined, C2_000_2009")> C2_000_2009 = 2009
            <EnumMember> <Description("Error: Not defined, C2_000_2010")> C2_000_2010 = 2010
            <EnumMember> <Description("Error: Not defined, C2_000_2010")> C2_000_2011 = 2011


            '#############################  I O P E R A C I O N E S   ###########################################
            'Capa3, IOperaciones
            <EnumMember> <Description("Error:C3_000_3000: Not defined")> C3_000_3000 = 3000
            <EnumMember> <Description("Error:C3_000_3001: Not defined")> C3_000_3001 = 3001
            <EnumMember> <Description("Error:C3_000_3002: Not defined")> C3_000_3002 = 3002
            <EnumMember> <Description("Error: No se encontró una columna en la Vista con el nombre solicitado ")> C3_000_3003 = 3003

            '#############################  I E N L A C E   D A T O S   ##########################################
            'Capa3, Sección especial extensión de IEnlace
            <EnumMember> <Description("Warning: Para este tipo de operación el módulo debe configurarse con el operador 'Sp000OperadorCatalogosModificacionesMaxMultipleValor', se ha cambiado por defecto. C3_001_3000")> C3_001_3000 = 13000
            <EnumMember> <Description("Error: Debe asignar un valor de llave primaria al menos para realizar esta acción. ")> C3_001_3001 = 13001
            <EnumMember> <Description("Error: Ocurrió un error al intentar modificar por los medios de IEnlace al momento de invocar SQL. ")> C3_001_3002 = 13002
            <EnumMember> <Description("Error: Ocurrió un error al intentar eliminar por los medios de IEnlcae al momento de invocar SQL. ")> C3_001_3003 = 13003
            <EnumMember> <Description("Error: No se encontraron caracteristicas disponibles para este recurso, verifique su vista de entorno o la configuración del recurso. ")> C3_001_3004 = 13004
            <EnumMember> <Description("Error: No ha proporcionado ningún objeto de datos para el IEnlace, la operación fue abortada. C3_001_3005")> C3_001_3005 = 13005
            <EnumMember> <Description("Error: No se envió una instancia correcta de IOperaciones por favor genere New OperacionesCatalogo y vuelva a intentar: ")> C3_001_3006 = 13006
            <EnumMember> <Description("Error: Atención no se ha encontrado el archivo con los recursos para el sistema, notifique al administrador ")> C3_001_3007 = 13007
            <EnumMember> <Description("Error: No se ha logrado realizar la replicación asignada a MongoDB porque no se encontró un indice primarykey SQL Server: ")> C3_001_3008 = 13008
            <EnumMember> <Description("Error: Ésta operación no ha sido implementada se desconoce su aplicabilidad. C3_001_3003")> C3_001_3009 = 13009
            <EnumMember> <Description("Error: No se encontraron datos en la lista. ")> C3_001_3010 = 13010
            <EnumMember> <Description("Warning: Not defined. ")> C3_001_3011 = 13011
            <EnumMember> <Description("Warning: Not defined. ")> C3_001_3012 = 13012
            <EnumMember> <Description("Warning: Not defined. ")> C3_001_3013 = 13013
            <EnumMember> <Description("Warning: Not defined. ")> C3_001_3014 = 13014
            <EnumMember> <Description("Warning: Not defined. ")> C3_001_3015 = 13015
            <EnumMember> <Description("Warning: Not defined. ")> C3_001_3016 = 13016
            <EnumMember> <Description("Warning: Not defined. ")> C3_001_3017 = 13017
            <EnumMember> <Description("Warning: Not defined. ")> C3_001_3018 = 13018
            <EnumMember> <Description("Warning: Not defined. ")> C3_001_3019 = 13019
            <EnumMember> <Description("Warning: Not defined. ")> C3_001_3020 = 13020
            <EnumMember> <Description("Warning: Not defined. ")> C3_001_3021 = 13021
            <EnumMember> <Description("Warning: Not defined. ")> C3_001_3022 = 13022
            <EnumMember> <Description("Warning: Not defined. ")> C3_001_3023 = 13023


            'Capa1, Excepciones de organismo
            <EnumMember> <Description("Error:C3_000_3900: No results")> C3_000_3900 = 3900
            <EnumMember> <Description("Error:C3_000_3901: Emerging exception")> C3_000_3901 = 3901

            '#############################  C O M P O N E N T E S   Y   C O N T R O L E S  ###########################
            'Capa4, User Interface, Controles de usuario, Ej. ( MaestroDetalle, FormulariosBase )
            <EnumMember> <Description("Error:C4_000_4000: Not defined")> C4_000_4000 = 4000
            <EnumMember> <Description("Error:C4_000_4001: Not defined")> C4_000_4001 = 4001
            <EnumMember> <Description("Error:C4_000_4002: Not defined")> C4_000_4002 = 4002
            <EnumMember> <Description("Error:C4_000_4003: Not defined")> C4_000_4003 = 4003


            '#############################  I M P L E M E N T A C I O N   D E   C M F  ##################################
            'Capa5, Implementación, ej. ( Frm014Facturación, Frm013GestorPagosHechos)
            <EnumMember> <Description("Error:C5_000_5000: No puede facturar/guardar proforma porque hay anticipos sin forma de pago/fecha recepción")> C5_000_5000 = 5000
            <EnumMember> <Description("Error:C5_000_5001: No se puede facturar/guardar proforma porque los anticipos ya estan aplicados")> C5_000_5001 = 5001
            <EnumMember> <Description("Error:C5_000_5002: No se puede facturar/guardar proforma porque los anticipos no estan disponibles")> C5_000_5002 = 5002
            <EnumMember> <Description("Error:C5_000_5003: No se puede facturar/guardar proforma porque los anticipos estan por liberar")> C5_000_5003 = 5003
            <EnumMember> <Description("Error:C5_000_5004: No se puede facturar/guardar proforma porque los anticipos estan no disponible por liberar")> C5_000_5004 = 5004
            <EnumMember> <Description("Error:C5_000_5005: Existe un error en las fluctuaciones cambiarias, favor de verificar")> C5_000_5005 = 5005
            <EnumMember> <Description("Error:C5_000_5006: No se puede facturar/guarda proforma debido a que el total de las facturas sobrepasa el monto de la nota de crédito")> C5_000_5006 = 5006
            <EnumMember> <Description("Error:C5_000_5007: No es posible facturar/guardar proforma para anticipos en dolares con facturas en pesos, comunicarse con TI")> C5_OOO_5007 = 5007
            'N 601: La expresión impresa proporcionada no es válida.
            'Este código de respuesta se presentará cuando la petición de validación no se haya respetado en
            'el formato definido.
            'N 602: Comprobante no encontrado.
            '   Lector de CFDi, excepciones
            <EnumMember> <Description("Error:C5_012_1000: XML corrupt")> C5_012_1000 = 5101
            <EnumMember> <Description("Error:C5_012_1001: (Sat.gob.mx 601) La expresión impresa proporcionada no es válida, comprobante no encontrado")> C5_012_1001 = 5102
            <EnumMember> <Description("Error:C5_012_1002: (Sat.gob.mx 602) Comprobante no encontrado, comprobante no encontrado, cancelado")> C5_012_1002 = 5103

            ' Controlador GeneraRepresentacionImpresa, excepciones
            <EnumMember> <Description("Error:C5_012_1101: La representación impresa no fue generada correctamente.")> C5_012_1101 = 5201
            <EnumMember> <Description("Error:C5_012_1102: No es posible generar una representación impresa del CFDI ya que el archivo fuente no tiene extensión XML.")> C5_012_1102 = 5202
            <EnumMember> <Description("Error:C5_012_1103: No es posible generar una representación impresa del XML para esta versión de CFDI.")> C5_012_1103 = 5203
            <EnumMember> <Description("Error:C5_012_1104: No existe el archivo fuente.")> C5_012_1104 = 5204

            'Capa5 A - ControladorEnlace
            <EnumMember> <Description("Error:C5_013_0001: No se encontraron credenciales para esta operación")> C5_013_00001 = 5501
            <EnumMember> <Description("Error:C5_013_0002: No se ha definido la estructura para la obtención de los datos ")> C5_013_00002 = 5502
            <EnumMember> <Description("Error:C5_013_0003: Dimension no implementada aún, por favor verifique sus parámetros ")> C5_013_00003 = 5503
            <EnumMember> <Description("Error:C5_013_0004: VOID ")> C5_013_00004 = 5504
            <EnumMember> <Description("Error:C5_013_0005: VOID ")> C5_013_00005 = 5505

            '#############################  C O N T R O L A D O R E S   E S P E C I A L E S  ##################################

            'Capa6, Controladores Extraordinarios, ej. ( FacturaObjeto64, Contabilidad64, Addendas )
            <EnumMember> <Description("Info:C6_014_6000: No tiene addenda disponible ")> C6_014_6000 = 6000
            <EnumMember> <Description("Error:C6_014_6001: No se ha cargado el CFDi para agregar addenda")> C6_014_6001 = 6001
            <EnumMember> <Description("Error:C6_014_6002: Este documento ya cuenta con una Addenda, operación cancelada")> C6_014_6002 = 6002
            <EnumMember> <Description("Error:C6_014_6003: El Archivo CFDi no existe.")> C6_014_6003 = 6003
            <EnumMember> <Description("Error:C6_014_6004: No existen datos disponibles de la referencia(s)")> C6_014_6004 = 6004
            <EnumMember> <Description("Error:C6_014_6005: Existen registros de pagos de terceros sin comprobantes asociados.")> C6_014_6005 = 6005
            <EnumMember> <Description("Error:C6_014_6006: El monto de alguno(s) registro(s) no coincide con el monto de su comprobante(s) asociado(s).")> C6_014_6006 = 6006
            <EnumMember> <Description("Error:C6_014_6007: Not defined")> C6_014_6007 = 6007

            'Capa6 Politicas Corporativas familia 10
            <EnumMember> <Description("Error:C6_010_7001: No cumplió con la(s) política")> C6_010_7001 = 7001
            <EnumMember> <Description("Error:C6_010_7002: No tiene politicas asignadas")> C6_010_7002 = 7002
            <EnumMember> <Description("Error:C6_010_7003: Parámetros no encontrados en la política")> C6_010_7003 = 7003
            <EnumMember> <Description("Error:C6_010_7004: No existe esa política")> C6_010_7004 = 7004
            <EnumMember> <Description("Error:C6_010_7005: Versión del módulo no encontrada, por favor actualice")> C6_010_7005 = 7005
            <EnumMember> <Description("Error:C6_010_7006: La referencia que se pretende modificar ya fue revalidada")> C6_010_7006 = 7006
            <EnumMember> <Description("Error:C6_010_7007: No se encontrarón valores en el objeto de la política")> C6_010_7007 = 7007
            <EnumMember> <Description("Error:C6_010_7008: No se encontro el tipo de contabilidad o es extranjera")> C6_010_7008 = 7008
            <EnumMember> <Description("Error:C6_010_7009: La oficina no tiene habilitado los costos extemporáneos")> C6_010_7009 = 7009
            <EnumMember> <Description("Error:C6_010_7010: La fecha de inicio de esta regla se contrapone con la fecha de inicio de la cuenta")> C6_010_7010 = 7010
            <EnumMember> <Description("Error:C6_010_7011: La fecha de fin de la cuenta se contrapone con alguna regla")> C6_010_7011 = 7011
            <EnumMember> <Description("Error:C6_010_7012: No se logro cerrar la cuenta ya que existen reglas pendientes de cerrar")> C6_010_7012 = 7012
            <EnumMember> <Description("Error:C6_010_7013: La división empresarial no tiene asignada un sistema externo activo.")> C6_010_7013 = 7013
            <EnumMember> <Description("Error:C6_010_7014: No se pudo cerrar la relación del usuario contra la cuenta de cliente ya que se encuentra cerrada.")> C6_010_7014 = 7014
            <EnumMember> <Description("Error:C6_010_7015: La fecha de inicio de la relación usuario - cuenta de cliente es menor que la fecha de inicio de la cuenta de cliente.")> C6_010_7015 = 7015
            <EnumMember> <Description("Error:C6_010_7016: Existen asignaciones de usuario - cuenta de cliente pendientes de cerrar.")> C6_010_7016 = 7016
            <EnumMember> <Description("Error:C6_010_7017: La fecha de inicio de esta asginación se contrapone con alguna otra relación.")> C6_010_7017 = 7017
            <EnumMember> <Description("Error:C6_010_7018: No se puede seleccionar el concepto debido a que algunos campos de la operación no se encuentran capturados.")> C6_010_7018 = 7018
            <EnumMember> <Description("Error:C6_010_7019: No se puede generar la factura debido a que existen documentos pendientes de asociar a los pagos de terceros.")> C6_010_7019 = 7019
            <EnumMember> <Description("Error:C6_010_7020: No se puede proceder con la operación debido a que el pedimento ya se encuentra pagado.")> C6_010_7020 = 7020
            <EnumMember> <Description("La referencia no corresponde a ningún ejecutivo del kardex de clientes. [C6_010_7021]")> C6_010_7021 = 7021
            <EnumMember> <Description("No existe un usuario asignado al ejecutivo al que le corresponde la referencia de acuerdo al kardex de clientes. [C6_010_7022]")> C6_010_7022 = 7022
            <EnumMember> <Description("La referencia no pertenece al ejecutivo que está realizando la solicitud. [C6_010_7023]")> C6_010_7023 = 7023
            <EnumMember> <Description("No se encontró la referencia en el sistema de pedimentos. [C6_010_7024]")> C6_010_7024 = 7024
            <EnumMember> <Description("No se encontró el monto de las contribuciones para la referencia. [C6_010_7025]")> C6_010_7025 = 7025
            <EnumMember> <Description("El valor aduana de alguna(s) de las partida(s) excede al valor permitido. [C6_010_7026]")> C6_010_7026 = 7026
            <EnumMember> <Description("El valor aduana del pedimento excede al valor permitido. [C6_010_7027]")> C6_010_7027 = 7027
            <EnumMember> <Description("El cliente no tiene un giro empresarial asinado. [C6_010_7028]")> C6_010_7028 = 7028
            <EnumMember> <Description("No se ha logrado cerrar la operación de hidrocarburos, los comentarios a continuación:  [C6_010_7029]" &
                                      vbCrLf & "")> C6_010_7029 = 7029
            <EnumMember> <Description("Fecha límite para aplicación de política 'Validación de Campos' aun no sobrepasada. [C6_010_7030]")> C6_010_7030 = 7030
            <EnumMember> <Description("Fecha límite para aplicación de política 'Validación de Campos' ha sido sobrepasada, no se puede guardar la Solicitud de Pago hasta que se cumplan los requisitos. [C6_010_7031]")> C6_010_7031 = 7031
            <EnumMember> <Description("La solicitud que intenta realizar es una solicitud para el pago de impuestos segun pedimento y a un no cuenta con la validación de pedimento. [C6_010_7032]")> C6_010_7032 = 7032

            'Capa6 Politicas Corporativas familia 13
            <EnumMember> <Description("Error:C6_013_8001: Existen documentos registrados en Emisión de Pagos, no es posible realizar la operación.")> C6_013_8001 = 8001
            <EnumMember> <Description("Error:C6_013_8002: El documento asociado está registrado en Emisión de Pagos, no es posible cancelar el pago por terceros.")> C6_013_8002 = 8002

            'Capa6 FactoraObjeto3364
            <EnumMember> <Description("Error:C6_014_8100: El tipo de cambio en el anticipo de fluctuación no es válido")> C6_014_8100 = 8100
            <EnumMember> <Description("Error:C6_014_8101: Hay mas de un tipo de cambio de fluctuación cambiaria en esta operación, la operación fue abortada")> C6_014_8101 = 8101
            <EnumMember> <Description("Error:C6_014_8102: No se ha registrado la fluctuación cambiaria de la referencia ")> C6_014_8102 = 8102
            <EnumMember> <Description("Error:C6_014_8103: No se ha logrado aplicar el sello local al CFDI del complemento de pago")> C6_014_8103 = 8103
            <EnumMember> <Description("Error:C6_014_8104: No se ha logrado timbrar el CFDI del complemento de pago ")> C6_014_8104 = 8104
            <EnumMember> <Description("Error:C6_014_8105: No se ha logrado agregar la información del complemento de pago a la poliza que originalmente tiene la factura")> C6_014_8105 = 8105
            <EnumMember> <Description("Error:C6_014_8106: No se ha logrado obtener el número de parcialidad del complemento de pago ")> C6_014_8106 = 8106
            <EnumMember> <Description("Error:C6_014_8107: No se ha logrado verificar los valores globales de la facturación")> C6_014_8107 = 8107
            <EnumMember> <Description("Error:C6_014_8108: No se ha logrado obtener los datos el Emisor o Factura para el Complemento de Pago")> C6_014_8108 = 8108
            <EnumMember> <Description("Error:C6_014_8109: No se ha logrado obtener los datos el Receptor para el Complemento de Pago")> C6_014_8109 = 8109
            <EnumMember> <Description("Error:C6_014_8110: No se ha logrado insertar la cabezerá del Complemento de Pago")> C6_014_8110 = 8110
            <EnumMember> <Description("Error:C6_014_8111: No se puede generar Complemento de Pago porque el método de pago no es PPD")> C6_014_8111 = 8111
            <EnumMember> <Description("Error:C6_014_8112: No se ha logrado insertar el nodo de pago del Complemento de Pago")> C6_014_8112 = 8112
            <EnumMember> <Description("Error:C6_014_8113: No se ha logrado insertar el nodo de Documento Relacionado de Complemento de Pago")> C6_014_8113 = 8113
            <EnumMember> <Description("Error:C6_014_8114: Existe un complemento de pago de este CFDi con fecha de pago posterior a este, por favor verifique las fechas")> C6_014_8114 = 8114
            <EnumMember> <Description("Error:C6_014_8115: Existe un complemento pendiente de sustituir, pero, no coincide con la parcialidad actual")> C6_014_8115 = 8115
            <EnumMember> <Description("Error:C6_014_8116: Hubo un error al consultar el documento a sustituir")> C6_014_8116 = 8116
            <EnumMember> <Description("Error:C6_014_8117: Error en la parcialidad del complemento de pago")> C6_014_8117 = 8117
            <EnumMember> <Description("Error:C6_014_8118: Parcialidad incorrecta, esta factura ya cuenta con liquidaciones")> C6_014_8118 = 8118
            <EnumMember> <Description("Error:C6_014_8119: Parcialidad incorrecta, esta factura aún no cuenta con liquidaciones")> C6_014_8119 = 8119
            <EnumMember> <Description("Error:C6_014_8120: Parcialidad incorrecta, no corresponde la parcialidad con la automática")> C6_014_8120 = 8120
            <EnumMember> <Description("Error:C6_014_8121: Existe un comprobante pendiente de sustituir en la base de datos")> C6_014_8121 = 8121
            <EnumMember> <Description("Error:C6_014_8122: No existe un comprobante pendiente de sustituir en la base de datos")> C6_014_8122 = 8122
            <EnumMember> <Description("Error:C6_014_8123: Existe un complemento pendiente de sustituir previo al complemento que se esta sustituyendo")> C6_014_8123 = 8123
            <EnumMember> <Description("Error:C6_014_8124: No se encontraron las liquidaciones del anticipo")> C6_014_8124 = 8124
            <EnumMember> <Description("No se encontraron los datos del complemento de pago.")> C6_014_8125 = 8125
            <EnumMember> <Description("No existe un complemento de pago para este movimiento.")> C6_014_8126 = 8126
            <EnumMember> <Description("El complemento de pago no se emitió al cliente y no se puede enviar")> C6_014_8127 = 8127
            <EnumMember> <Description("No se pudo enviar el complemento de pago al cliente. Favor de presionar el botón Enviar complemento en la sección de Anticipos")> C6_014_8128 = 8128
            <EnumMember> <Description("Existen complementos de pago que no se enviaron al cliente. Favor de presionar el botón Enviar complemento")> C6_014_8129 = 8129

            'Facturación 
            <EnumMember> <Description("La cuenta de gastos cuenta con complementos de pagos emitidos")> C6_014_8200 = 8200
            <EnumMember> <Description("Algunos conceptos no cuentan con el objeto de impuesto capturado")> C6_014_8201 = 8201
            <EnumMember> <Description("No es posible facturar conceptos con diferentes objetos de impuestos, favor de verificar")> C6_014_8202 = 8202

            'Capa 6 - Familia 014 - Complementos de pago 2.0
            <EnumMember> <Description("No se pudo guardar en base de datos los datos que corresponden al nodo Totales en el complemento de pago 2.0. [C6_014_8300]")> C6_014_8300 = 8300
            <EnumMember> <Description("No se pudo guardar en base de datos los datos que corresponden al nodo TrasladosP en el complemento de pago 2.0. [C6_014_8301]")> C6_014_8301 = 8301
            <EnumMember> <Description("No se pudo guardar en base de datos los datos que corresponden al nodo TrasladosDR en el complemento de pago 2.0. [C6_014_8302]")> C6_014_8302 = 8302
            <EnumMember> <Description("La cuenta de gastos que intentas liquidar cuenta con retenciones. Aún no se puede liquidar. [C6_014_8303]")> C6_014_8303 = 8303
            <EnumMember> <Description("Los conceptos de la cuenta de gastos a liquidar no tienen el mismo objeto de impuestos. Aún no se puede liquidar. [C6_014_8304]")> C6_014_8304 = 8304
            <EnumMember> <Description("La empresa de factoraje no tiene regimen fiscal asociado. Favor de vincular el regimen fiscal a la empresa. [C6_014_8305]")> C6_014_8305 = 8305
            <EnumMember> <Description("El cliente no tiene regimen fiscal asociado. Favor de vincular el regimen fiscal a la empresa del cliente. [C6_014_8306]")> C6_014_8306 = 8306
            <EnumMember> <Description("No se encontraron los conceptos de la factura a liquidar. [C6_014_8307]")> C6_014_8307 = 8307
            <EnumMember> <Description("No se encontró en ningún concepto de la cuenta de gastos el objeto de impuestos. Favor de agregarlo en el catálogo de productos/servicios. [C6_014_8308]")> C6_014_8308 = 8308

            'Capa6 SysExpert familia 003
            <EnumMember> <Description("Error:C6_003_9001: No existe ese puerto en SysExpert.")> C6_003_9001 = 9001
            <EnumMember> <Description("Error:C6_003_9002: No existe esa naviera en SysExpert.")> C6_003_9002 = 9002
            <EnumMember> <Description("Error:C6_003_9003: El viaje no contiene ningún registro.")> C6_003_9003 = 9003
            <EnumMember> <Description("Error:C6_003_9004: La sentencia de actualización no contiene WHERE.")> C6_003_9004 = 9004
            <EnumMember> <Description("Error:C6_003_9005: No se pudo actualizar tipo de cambio porque el pedimento esta pagado.")> C6_003_9005 = 9005
            <EnumMember> <Description("Error:C6_003_9006: No se pudo actualizar tipo de cambio error en SP spActualizaFechaEntrada.")> C6_003_9006 = 9006
            <EnumMember> <Description("Error:C6_003_9007: No se logro encontrar la referencia.")> C6_003_9007 = 9007
            <EnumMember> <Description("Error:C6_003_9008: Ocurrió un error al momento de eliminar las marcas/contenedores.")> C6_003_9008 = 9008
            <EnumMember> <Description("Error:C6_003_9009: No existe el Pais en SysExpert.")> C6_003_9009 = 9009
            <EnumMember> <Description("Error:C6_003_9010: No existe el Incoterm en SysExpert.")> C6_003_9010 = 9010
            <EnumMember> <Description("Error:C6_003_9011: No existe el Método de Valoración en SysExpert.")> C6_003_9011 = 9011
            <EnumMember> <Description("Error:C6_003_9012: No se pudo insertar el proveedor en SysExpert.")> C6_003_9012 = 9012
            <EnumMember> <Description("Error:C6_003_9013: No se pudo deshabilitar el proveedor importado en SysExpert.")> C6_003_9013 = 9013
            <EnumMember> <Description("Error:C6_003_9014: No existe la entidad federativa en SysExpert.")> C6_003_9014 = 9014
            <EnumMember> <Description("Error:C6_003_9015: No se encontró información para este registro.")> C6_003_9015 = 9015
            <EnumMember> <Description("Error:C6_003_9016: No se pudo actualizar el proveedor en SysExpert.")> C6_003_9016 = 9016
            <EnumMember> <Description("Error:C6_003_9017: No se pudo deshabilitar el proveedor en SysExpert.")> C6_003_9017 = 9017
            <EnumMember> <Description("Error:C6_003_9018: No se pudo habilitar el proveedor en SysExpert.")> C6_003_9018 = 9018
            <EnumMember> <Description("Error:C6_003_9019: No se pudo desactivar el proveedor que se sustituye.")> C6_003_9019 = 9019
            <EnumMember> <Description("Error:C6_003_9020: No fue posible eliminar las vinculaciones relacionadas al proveedor.")> C6_003_9020 = 9020
            <EnumMember> <Description("Error:C6_003_9021: No se pudo insertar la parte en SysExpert.")> C6_003_9021 = 9021
            <EnumMember> <Description("Error:C6_003_9022: No se pudo actualizar la parte en SysExpert.")> C6_003_9022 = 9022
            <EnumMember> <Description("No se encontró la situación de tráfico para la oficina, favor de contactar al administrador del sistema. [C6_003_9023] ")> C6_003_9023 = 9023
            <EnumMember> <Description("No se pudo actualizar la fecha de entrada, favor de contactar al administrador del sistema. [C6_003_9024] ")> C6_003_9024 = 9024
            <EnumMember> <Description("La referencia ya se encuentra validada. [C6_003_9025] ")> C6_003_9025 = 9025
            <EnumMember> <Description("Hubo un problema al insertar el contenedor en el pedimento, favor de contactar al administrador del sistema. [C6_003_9026] ")> C6_003_9026 = 9026
            <EnumMember> <Description("Hubo un problema al insertar la remesa en el pedimento, favor de contactar al administrador del sistema. [C6_003_9027] ")> C6_003_9027 = 9027
            <EnumMember> <Description("Hubo un problema al actualizar el contenedor en el pedimento, favor de contactar al administrador del sistema. [C6_003_9028] ")> C6_003_9028 = 9028
            <EnumMember> <Description("Hubo un problema al eliminar el contenedor en el pedimento, favor de contactar al administrador del sistema. [C6_003_9029] ")> C6_003_9029 = 9029
            <EnumMember> <Description("Hubo un problema al actualizar la remesa en el pedimento, favor de contactar al administrador del sistema. [C6_003_9030] ")> C6_003_9030 = 9030
            <EnumMember> <Description("Hubo un problema al eliminar la remesa en el pedimento, favor de contactar al administrador del sistema. [C6_003_9031] ")> C6_003_9031 = 9031

            'Capa6 Familia de correo Electrónico 028
            <EnumMember> <Description("Error:C6_028_1000: El correo electrónico no logro registrarse en la tabla Bit028CorreoElectronicoEntrada.")> C6_028_1000 = 9100
            <EnumMember> <Description("Error:C6_028_1001: El correo electrónico no logro registrarse en la tabla Vin028BandejaEntrada.")> C6_028_1001 = 9101
            <EnumMember> <Description("Error:C6_028_1002: El attachment no logro registrarse en la tabla de vinculación de documentos Vin028DocumentosCorreoElectronicoEntrada.")> C6_028_1002 = 9102
            <EnumMember> <Description("Error:C6_028_1003: No se logro enviar el correo electrónico de error al equipo de soporte.")> C6_028_1003 = 9103
            <EnumMember> <Description("Error:C6_028_1004: Excepción no controlada al momento de leer el correo electrónico.")> C6_028_1004 = 9104
            <EnumMember> <Description("Error:C6_028_1005: El attachment no logro encontrar el directorio.")> C6_028_1005 = 9105
            <EnumMember> <Description("Error:C6_028_1006: El correo electrónico no logro enviarse debido a que no existe un buzón de envio para este usuario.")> C6_028_1006 = 9106
            <EnumMember> <Description("Error:C6_028_1007: El correo electrónico no logro registrarse en la tabla Bit028CorreoElectronicoEnvios.")> C6_028_1007 = 9107
            <EnumMember> <Description("Error:C6_028_1008: El correo electrónico que se desea enviar sobrepaso la cantidad de intentos.")> C6_028_1008 = 9108
            <EnumMember> <Description("Error:C6_028_1009: El correo electrónico que se desea enviar no tiene adjuntos.")> C6_028_1009 = 9109
            <EnumMember> <Description("Error:C6_028_1010: El correo electrónico que se desea enviar no tiene adjuntos localizables.")> C6_028_1010 = 9110
            <EnumMember> <Description("Error:C6_028_1011: El adjunto no existe en el directorio proporcionado.")> C6_028_1011 = 9111
            <EnumMember> <Description("Error:C6_028_1012: El correo electrónico que se desea enviar no tiene destinatarios.")> C6_028_1012 = 9112
            <EnumMember> <Description("Error:C6_028_1013: El correo electrónico no logro registrarse en la tabla Ext028CorreoElectronicoEnviado.")> C6_028_1013 = 9113
            <EnumMember> <Description("Error:C6_028_1014: El correo electrónico no logro registrarse en la tabla Vin028DocumentosCorreoElectronicoEnviado.")> C6_028_1014 = 9114
            <EnumMember> <Description("Error:C6_028_1015: Excepción no controlada al momento de generar el envió de correo electrónico.")> C6_028_1015 = 9115
            <EnumMember> <Description("Error:C6_028_1016: No se logró cambiar el estatus del correo en la bandeja de entrada.")> C6_028_1016 = 9116
            <EnumMember> <Description("Error:C6_028_1017: No se logró insertar el registro en la bitácora de la lectura del correo electrónico.")> C6_028_1017 = 9117
            <EnumMember> <Description("Error:C6_028_1018: No se logró actualizar el registro en la bitácora de la lectura del correo electrónico.")> C6_028_1018 = 9118
            <EnumMember> <Description("Error:C6_028_1019: Error en el inicio de sesión del buzón.")> C6_028_1019 = 9119
            <EnumMember> <Description("Error:C6_028_1020: Error al generar la lista de usuarios a los que se les vincula el buzón de correo.")> C6_028_1020 = 9120
            <EnumMember> <Description("Error:C6_028_1021: No se encontró  el correo en el buzón.")> C6_028_1021 = 9121

            'Capa6 Controlador de Documentos
            <EnumMember> <Description("Error:C6_012_1000: El documento no se pudo registrar debido a la falta de asignación de tipo de documento.")> C6_012_1000 = 9200
            <EnumMember> <Description("Error:C6_012_1001: El documento no se pudo registrar debido a que el tipo de documento seleccionado no pertence al catálogo.")> C6_012_1001 = 9201
            <EnumMember> <Description("Error:C6_012_1002: El documento no se pudo registrar debido a la falta de asignación de un repositorio.")> C6_012_1002 = 9202
            <EnumMember> <Description("Error:C6_012_1003: El documento no se pudo registrar debido a que no existe relación entre el tipo de documento y la plantilla seleccionada.")> C6_012_1003 = 9203
            <EnumMember> <Description("Error:C6_012_1004: El documento no se pudo registrar debido a la falta de asignación de un directorio.")> C6_012_1004 = 9204
            <EnumMember> <Description("Error:C6_012_1005: El documento no se pudo registrar en la tabla Reg012Documentos.")> C6_012_1005 = 9205
            <EnumMember> <Description("Error:C6_012_1006: El documento no se pudo registrar en la tabla Ext012Documentos.")> C6_012_1006 = 9206
            <EnumMember> <Description("Error:C6_012_1007: El documento no se logro guardar en el repositorio de producción.")> C6_012_1007 = 9207
            <EnumMember> <Description("Error:C6_012_1008: El documento no se logro guardar en el repositorio de contingencia.")> C6_012_1008 = 9208
            <EnumMember> <Description("Error:C6_012_1009: El documento no se logro guardar debido a que no concuerda la extensión del tipo de documento contra el documento cargado.")> C6_012_1009 = 9209
            <EnumMember> <Description("Error:C6_012_1010: No se logro guardar una característica del documento.")> C6_012_1010 = 9210
            <EnumMember> <Description("Error:C6_012_1011: No se logro guardar una característica secundaria del documento.")> C6_012_1011 = 9211
            <EnumMember> <Description("Error:C6_012_1015: No se logro encontrar el documento.")> C6_012_1015 = 9215
            <EnumMember> <Description("Error:C6_012_1016: No se logro encontrar los soportes del documentos.")> C6_012_1016 = 9216
            <EnumMember> <Description("Error:C6_012_1017: El documento no se pudo registrar debido a que no fue capturada una característica que es requerida.")> C6_012_1017 = 9217
            <EnumMember> <Description("Error:C6_012_1018: El documento no se pudo registrar en la vinculación.")> C6_012_1018 = 9218
            <EnumMember> <Description("Error:C6_012_1019: No se lograron guardar las caracteriísticas en el apartado de visor.")> C6_012_1019 = 9219
            <EnumMember> <Description("Error:C6_012_1020: No se encontraron características con información de este documento.")> C6_012_1020 = 9220
            <EnumMember> <Description("Error:C6_012_1021: No se encontraron características con información de este documento en el visor.")> C6_012_1021 = 9221
            <EnumMember> <Description("Error:C6_012_1022: No se logró modificar el documento en la tabla de extensión.")> C6_012_1022 = 9222
            <EnumMember> <Description("Error:C6_012_1023: No se logró modificar una característica primaria.")> C6_012_1023 = 9223
            <EnumMember> <Description("Error:C6_012_1024: No se logró modificar una característica secundaria.")> C6_012_1024 = 9224
            <EnumMember> <Description("Error:C6_012_1025: No se lograron modificar las características en el visor.")> C6_012_1025 = 9225
            <EnumMember> <Description("Error:C6_012_1026: No se ha seleccionado ningún tipo de privilegio.")> C6_012_1026 = 9226
            <EnumMember> <Description("Error:C6_012_1027: No tienes privilegios para modificar el documento." &
                                      vbCrLf & "Favor de solicitar los permisos al departamento de TI. ")> C6_012_1027 = 9227
            <EnumMember> <Description("Error:C6_012_1028: No tienes privilegios para consultar el documento." &
                                      vbCrLf & "Favor de solicitar los permisos al departamento de TI. ")> C6_012_1028 = 9228
            <EnumMember> <Description("Error:C6_012_1029: No tienes privilegios para esta acción debido a que no eres administrador." &
                                      vbCrLf & "Favor de solicitar los permisos al departamento de TI. ")> C6_012_1029 = 9229
            <EnumMember> <Description("Error:C6_012_1030: No se logró insertar el documento, esto se debe a que ya se encuentra registrado en el maestro de documentos.")> C6_012_1030 = 9230
            <EnumMember> <Description("Error:C6_012_1031: No se puede abrir este documento ya que se encuentra eliminado.")> C6_012_1031 = 9231
            <EnumMember> <Description("Error:C6_012_1032: No se logro obtener información de esa operación considerando la referencia.")> C6_012_1032 = 9232
            <EnumMember> <Description("Error:C6_012_1033: No tienes privilegios para agregar el documento." &
                          vbCrLf & "Favor de solicitar los permisos al departamento de TI. ")> C6_012_1033 = 9233
            <EnumMember> <Description("Error:C6_012_1034: No se logro obtener información de esa operación considerando la razón social.")> C6_012_1034 = 9234
            <EnumMember> <Description("Error:C6_012_1035: El documento no se ha descargado por que ya existe en el directorio")> C6_012_1035 = 9235
            <EnumMember> <Description("Error:C6_012_1036: No se pudo convertir el documento Excel a PDF.")> C6_012_1036 = 9236
            <EnumMember> <Description("Error:C6_012_1037: No se logro modificar el documento.")> C6_012_1037 = 9237
            <EnumMember> <Description("Error:C6_012_1038: Error al eliminar la vinculación de la solicitud y el link.")> C6_012_1038 = 9238
            <EnumMember> <Description("Error:C6_012_1039: El documento por correo electrónico no tiene una plantilla configurada.")> C6_012_1039 = 9239
            <EnumMember> <Description("Error:C6_012_1040: No se logró cambiar el estatus del documentos.")> C6_012_1040 = 9240
            <EnumMember> <Description("Error:C6_012_1041: El documento no existe en el repositorio principal.")> C6_012_1041 = 9241
            <EnumMember> <Description("Error:C6_012_1042: Error no controlado, favor de notificar a TI.")> C6_012_1042 = 9242
            <EnumMember> <Description("Error:C6_012_1043: No se logró insertar el registro en la tabla de documentos divididos(Vin012DocumentosDivididos).")> C6_012_1043 = 9243
            <EnumMember> <Description("Error:C6_012_1044: No se logró insertar el registro en la tabla de edocuments(Vin012Edocuments).")> C6_012_1044 = 9244
            <EnumMember> <Description("Error:C6_012_1045: No se encuentra la clave de tipo de documento VUCEM.")> C6_012_1045 = 9245
            <EnumMember> <Description("Error:C6_012_1046: No se encontraron documentos asociados a la cuenta de gastos.")> C6_012_1046 = 9246
            <EnumMember> <Description("Error:C6_012_1047: No se encontraron documentos asociados a la operación.")> C6_012_1047 = 9247
            <EnumMember> <Description("Error:C6_012_1048: No se logró cambiar el valor de consultable por el cliente.")> C6_012_1048 = 9248
            <EnumMember> <Description("Error:C6_012_1049: No se logró eliminar el documento del directorio.")> C6_012_1049 = 9249
            <EnumMember> <Description("Error:C6_012_1050: No se logró convertir el archivo RAR a ZIP")> C6_012_1050 = 9250
            <EnumMember> <Description("Error:C6_012_1051: Error al procesar el documento temporal.")> C6_012_1051 = 9251
            <EnumMember> <Description("Error:C6_012_1052: No se logró crear los archivos temporales.")> C6_012_1052 = 9252
            <EnumMember> <Description("Error:C6_012_1053: Error al procesar el ZIP convertido.")> C6_012_1053 = 9253
            <EnumMember> <Description("Error:C6_012_1054: Error al descomprimir el ZIP.")> C6_012_1054 = 9254
            <EnumMember> <Description("Error:C6_012_1055: No se pudo convertir el documento Word a PDF.")> C6_012_1055 = 9255
            <EnumMember> <Description("Error:C6_012_1056: No se pudo convertir el documento XML a PDF.")> C6_012_1056 = 9256
            <EnumMember> <Description("Error:C6_012_1057: No se pudo convertir la imagen a PDF.")> C6_012_1057 = 9257
            <EnumMember> <Description("Error:C6_012_1058: No se logró generar el consecutivo del Edocument.")> C6_012_1058 = 9258
            <EnumMember> <Description("Error:C6_012_1059: No se encontró la clave de la plantilla del documento.")> C6_012_1059 = 9259
            <EnumMember> <Description("Error:C6_012_1060: No se logró eliminar los archivos temporales.")> C6_012_1060 = 9260

            'Capa6 Solicitudes de pago familia 013
            <EnumMember> <Description("Clave:C6_013_9401: La referenia operativa no tiene un ejecutivo de cuenta asignado, por lo cual se recomienda realizar lo siguiente: " &
                             vbCrLf & "1.-Es necesario ir al módulo de maestro de operaciones e ingresar el ejecutivo de la refencia operativa correcto" &
                             vbCrLf & "(en la solicitud de pago aparecerá el nombre de ese ejecutivo" &
                             vbCrLf & "2.-Es necesario acercarse al encargado de tráfico de su oficina y solicitar que actualice la cartera de clientes," &
                             vbCrLf & "esto con la finalidad de no incurrir nuevamente en este proceso")> C6_013_9001 = 9400
            <EnumMember> <Description("Clave:C6_013_9402: No se puedo guardar la vinculación de la solicitud con el link.")> C6_013_9402 = 9402
            <EnumMember> <Description("Clave:C6_013_9403: No se pudo cancelar el envío del correo para la solicitud.")> C6_013_9403 = 9403
            <EnumMember> <Description("Clave:C6_013_9404: Ocurrio un error al aprobar o cancelar la solicitud.")> C6_013_9404 = 9404
            <EnumMember> <Description("Clave:C6_013_9405: No hay un gerente vinculado a la(s) referencia(s) a cual enviar la solicitud, se debe vincular la referencia a un gerente en el kardex de clientes.")> C6_013_9405 = 9405
            <EnumMember> <Description("Clave:C6_013_9406: TagWatcher para uso de politica.")> C6_013_9406 = 9406
            <EnumMember> <Description("Clave:C6_013_9407: Ocurrio un error al leer la politica 'Verifica financiamiento cliente', comuniquese con el departamento de TI.")> C6_013_9407 = 9407
            <EnumMember> <Description("Clave:C6_013_9408: No se encontro el correo del autorizador, deberás reenviarle el correo.")> C6_013_9408 = 9408

            'Capa6 Flujo de operaciones familia 026
            <EnumMember> <Description("Error:C6_026_0000: No existe la plantilla seleccionada, favor de comunicarse con TI")> C6_026_0000 = 9300
            <EnumMember> <Description("Error:C6_026_0001: La plantilla seleccionada no contiene ninguna acción para el flujo, favor de comunicarse con TI")> C6_026_0001 = 9301
            <EnumMember> <Description("Error:C6_026_0002: La operación no paso la prueba de campos de la plantilla (campo no encontrado), favor de seleccionar una plantilla correcta")> C6_026_0002 = 9302
            <EnumMember> <Description("Error:C6_026_0003: La operación no paso la prueba de campos de la plantilla (el valor proporcionado no es el esperado), favor de seleccionar una plantilla correcta")> C6_026_0003 = 9303
            <EnumMember> <Description("Error:C6_026_0004: No se cuenta con la clave del maestro de operaciones, favor de indicarla")> C6_026_0004 = 9304
            <EnumMember> <Description("Error:C6_026_0005: No se cuenta con la clave de la plantilla, favor de indicarla")> C6_026_0005 = 9305
            <EnumMember> <Description("Error:C6_026_0006: ")> C6_026_0006 = 9306
            <EnumMember> <Description("Error:C6_026_0007: ")> C6_026_0007 = 9307
            <EnumMember> <Description("Error:C6_026_0008: ")> C6_026_0008 = 9308
            <EnumMember> <Description("Error:C6_026_0009: ")> C6_026_0009 = 9309
            <EnumMember> <Description("Error:C6_026_0010: ")> C6_026_0010 = 9310
            <EnumMember> <Description("Error:C6_026_0011: ")> C6_026_0011 = 9311
            <EnumMember> <Description("Error:C6_026_0012: ")> C6_026_0012 = 9312

            'Capa6 Recolector de documentos familia 030
            <EnumMember> <Description("Error:C6_030_0000: El nombre del archivo coincide con el patrón especificado en varios de documentos.")> C6_030_0000 = 9500
            <EnumMember> <Description("Error:C6_030_0001: El documento no corresponde a la referencia.")> C6_030_0001 = 9501
            <EnumMember> <Description("Error:C6_030_0002: El nombre del archivo no es válido.")> C6_030_0002 = 9502
            <EnumMember> <Description("Error:C6_030_0003: .")> C6_030_0003 = 9503
            <EnumMember> <Description("Error:C6_030_0004: .")> C6_030_0004 = 9504

            'Capa6 Grupos de destinatarios (Notificaciones)
            <EnumMember> <Description("No se encontraron destinatarios configurados para esta notificación, favor de contactar al administrador del sistema. [C6_046_9600] ")> C6_046_9600 = 9600

            'Capa 6 - Familia 029 - Generador de links
            <EnumMember> <Description("Clave:C6_029_0001: El tipo de link solicitado no es valido.")> C6_029_0001 = 9601
            <EnumMember> <Description("Clave:C6_029_0002: No se logro insertar el encabezado de los links.")> C6_029_0002 = 9602
            <EnumMember> <Description("Clave:C6_029_0003: No se logro insertar el detalle de los links.")> C6_029_0003 = 9603
            <EnumMember> <Description("Clave:C6_029_0004: No se logro encontrar el link.")> C6_029_0004 = 9604
            <EnumMember> <Description("Clave:C6_029_0005: El link que intestas consultar ya no esta disponible o la solicitud de pago ya caduco.")> C6_029_0005 = 9605
            <EnumMember> <Description("Clave:C6_029_0006: La lista de claves esta vacia.")> C6_029_0006 = 9606
            <EnumMember> <Description("Clave:C6_029_0007: La lista de nombres esta vacia.")> C6_029_0007 = 9607
            <EnumMember> <Description("Clave:C6_029_0008: Error al crear el link.")> C6_029_0008 = 9608
            <EnumMember> <Description("Clave:C6_029_0009: No se encontraron resultados.")> C6_029_0009 = 9609
            <EnumMember> <Description("Clave:C6_029_0010: No se pudo guardar la URL.")> C6_029_0010 = 9610
            <EnumMember> <Description("Clave:C6_029_0011: El documento que intenta visualizar es un XML y este tipo de documento no se puede visualizar.")> C6_029_0011 = 9611
            <EnumMember> <Description("Clave:C6_029_0012: No cuentas con el acceso para visualizar la documentación.")> C6_029_0012 = 9612

            'Capa6 Notificaciones
            <EnumMember> <Description("No existen destinatarios agregados en la notificación")> C6_029_0020 = 9620
            <EnumMember> <Description("Hubo un problema al obtener los destinatarios en la notificación.")> C6_029_0021 = 9621
            <EnumMember> <Description("No se encontraron las plantillas de documento agregadas en la notificación.")> C6_029_0022 = 9622
            <EnumMember> <Description("Hubo un problema al obtener las plantillas de documento en la noticación.")> C6_029_0023 = 9623
            <EnumMember> <Description("No se envío el documento ya que no corresponde al cliente.")> C6_029_0024 = 9624
            <EnumMember> <Description("Se necesita que al menos una plantilla de documento se encuentre agregada.")> C6_029_0025 = 9625
            <EnumMember> <Description("No se registró en bitacora el estatus RECIBIDO del cliente.")> C6_029_0026 = 9626
            <EnumMember> <Description("Ocurrió un error al guardar el estatus de la notificación en la bitácora.")> C6_029_0027 = 9627
            <EnumMember> <Description("No se encontró el registro en la bitácora de las notificaciones.")> C6_029_0028 = 9628
            <EnumMember> <Description("No se encontraron los destinatarios en el registro de la bitacora.")> C6_029_0029 = 9629

            ' Capa 6 - Familia 035 - Formularios FeedBack
            <EnumMember> <Description("No se pudo actualizar la fecha de modificación del cuestionario, favor de contactar al administrador del sistema. [C6_035_0001]")> C6_035_0001 = 9701
            <EnumMember> <Description("Hubo un problema al agregar la validación, vuelva a intentarlo. [C6_035_0002]")> C6_035_0002 = 9702
            <EnumMember> <Description("La pregunta se encuentra vinculada en validaciones del presente formulario, por favor verifique. [C6_035_0003]")> C6_035_0003 = 9703
            <EnumMember> <Description("No se pudo actualizar la revisión de la configuración, favor de contactar al administrador del sistema. [C6_035_0004]")> C6_035_0004 = 9704
            <EnumMember> <Description("Aviso: Después de la pregunta 'Establecer en las reglas' sólo está permitida si se comprobarán reglas.")> C6_035_0005 = 9705
            <EnumMember> <Description("Aviso: Se realizaron cambios en las reglas de validación es necesario guardar cambios en el registro.")> C6_035_0006 = 9706
            <EnumMember> <Description("Aviso: Indique {Unidad tiempo límite} y/o {Cantidad tiempo límite}.")> C6_035_0007 = 9707
            <EnumMember> <Description("No es posible aplicar la revisión, existen preguntas vinculadas sin capturar o inactivas.[C6_035_0008]")> C6_035_0008 = 9708
            <EnumMember> <Description("Hubo un problema al crear la vista de entorno, favor de contactar al administrador del sistema. [C6_035_0009]")> C6_035_0009 = 9709
            <EnumMember> <Description("Hubo un problema al registrar la vista de entorno, favor de contactar al administrador del sistema. [C6_035_0010]")> C6_035_0010 = 9710
            <EnumMember> <Description("Hubo un problema al actualizar el registro de la vista de entorno, favor de contactar al administrador del sistema. [C6_035_0011]")> C6_035_0011 = 9711
            <EnumMember> <Description("Hubo un problema al actualizar el registro en la bandeja de cuestionarios, favor de contactar al administrador del sistema. [C6_035_0012]")> C6_035_0012 = 9712
            <EnumMember> <Description("No es posible finalizar el cuestionario, existen preguntas pendientes de responder. [C6_035_0013]")> C6_035_0013 = 9713
            <EnumMember> <Description("Hubo un problema al registrar las preguntas en la bandeja de cuestionario, contacte al administrador del sistema.[C6_035_0014]")> C6_035_0014 = 9714
            <EnumMember> <Description("Hubo un problema al actualizar las preguntas en la bandeja de cuestionario, contacte al administrador del sistema.[C6_035_0015]")> C6_035_0015 = 9715

            'Capa 6 - Familia 003 - Control de viajes de importación
            <EnumMember> <Description("Error:C6_003_9800: No se encuentra configurado el puerto extranjero con la aduana correspondiente, por lo cual no se podrá calcular la ETA")> C6_003_9800 = 9800

            'Capa 6 - Familia 003 - Control de contenedores
            <EnumMember> <Description("Ocurrio un error al verificar si la operación se encuentra revalidada. [C6_003_9801]")> C6_003_9801 = 9801
            <EnumMember> <Description("No se pudieron actualizar las fechas EIR/Vacio, favor de contactar al administrador del sistema. [C6_003_9802]")> C6_003_9802 = 9802
            <EnumMember> <Description("No se encontraron contenedores. [C6_003_9803]")> C6_003_9803 = 9803
            <EnumMember> <Description("No ha seleccionado ningún contenedor. [C6_003_9804]")> C6_003_9804 = 9804
            <EnumMember> <Description("No se sincronizaron las fechas EIR/Vacio en SysExpert, favor de contactar al administrador del sistema. [C6_003_9805]")> C6_003_9805 = 9805

            'Capa 6 - Familia 003 - Control de viajes de exportación
            <EnumMember> <Description("No se insertaron las fechas de cierre documental o físico, favor de contactar al administrador del sistema. [C6_003_9850]")> C6_003_9850 = 9850
            <EnumMember> <Description("No se actualizaron las fechas de cierre documental o físico, favor de contactar al administrador del sistema. [C6_003_9851]")> C6_003_9851 = 9851
            <EnumMember> <Description("Existe al menos una referencia que se encuentra validada. [C6_003_9852]")> C6_003_9852 = 9852
            <EnumMember> <Description("Necesitas modificar la nave o buque.")> C6_003_9853 = 9853
            <EnumMember> <Description("Necesitas capturar el folio de capitanía del puerto.")> C6_003_9854 = 9854

            'Capa 6 - Familia 003 - Cuestionarios de Clasificación 
            <EnumMember> <Description("Registro desactivado, no cuenta con partidas. [C6_003_9900]")> C6_003_9900 = 9900
            <EnumMember> <Description("No se pudo crear una o más Bandejas de Cuestionario (Verifique los Cuestionarios). [C6_003_9901]")> C6_003_9901 = 9901
            <EnumMember> <Description("Se desasocio un cuestionario. Motivo: Fechas de Revisión Desactualizadas, (Verifique las Fechas de Última Revisión). [C6_003_9902]")> C6_003_9902 = 9902
            <EnumMember> <Description("¡Seleccionó partidas que no pertenecen a ese cuestionario, verifique su información! [C6_003_9903]")> C6_003_9903 = 9903
            <EnumMember> <Description("¡Seleccionó partidas que no requieren cuestionario! No puede realizar está acción. [C6_003_9904]")> C6_003_9904 = 9904
            <EnumMember> <Description("¡Seleccione un cuestionario! [C6_003_9905]")> C6_003_9905 = 9905
            <EnumMember> <Description("¡Seleccionó partidas que no tienen cuestionarios asociados! No puede realizar está acción. [C6_003_9906]")> C6_003_9906 = 9906
            <EnumMember> <Description("La referencia no tiene partidas que requieran cuestionario, no se inserto el registro. [C6_003_9907]")> C6_003_9907 = 9907
            <EnumMember> <Description("La referencia no tiene cuestionarios válidos, no se inserto el registro. [C6_003_9908]")> C6_003_9908 = 9908
            <EnumMember> <Description("La referencia no se encuentra en los Cuestionarios de Clasificación. [C6_003_9909]")> C6_003_9909 = 9909
            <EnumMember> <Description("Hubo un problema al actualizar los estatus de las partidas de los Cuestionarios de Clasificación. [C6_003_9910]")> C6_003_9910 = 9910
            <EnumMember> <Description("¡Seleccione partidas! No puede realizar está acción. [C6_003_9911]")> C6_003_9911 = 9911
            <EnumMember> <Description("¡Seleccione un cuestionario activo! [C6_003_9912]")> C6_003_9912 = 9912
            <EnumMember> <Description("¡Hubo un problema no existe ningún cuestionario relacionado a la fracción! [C6_003_9913]")> C6_003_9913 = 9913
            <EnumMember> <Description("No se pudo actualizar el estatus de una partida, favor de contactar al administrador del sistema. [C6_003_9914]")> C6_003_9914 = 9914
            <EnumMember> <Description("No se pudo actualizar el estatus de la referencia, favor de contactar al administrador del sistema. [C6_003_9915]")> C6_003_9915 = 9915
            <EnumMember> <Description("Hubo un problema al generar los Cuestionarios de Clasificación, favor de contactar al administrador del sistema. [C6_003_9916]")> C6_003_9916 = 9916


            '#############################  OTROS SEGMENTOS DEL CONTROL DE EXCEPCIONES ##################################

            'Krombase Web
            <EnumMember> <Description("No se ha completado su registro en el portal [KBW60400000]")> KBW_040_0000 = 10000
            <EnumMember> <Description("Los datos del usuario no son válidos [KBW60400001]")> KBW_040_0001 = 10001
            <EnumMember> <Description("Su perfil se encuentra en construcción, ya fué notificado [KBW60400002]")> KBW_040_0002 = 10002
            <EnumMember> <Description("Error:KBW6_040_0000: .")> KBW_040_0003 = 10003
            <EnumMember> <Description("Error:KBW6_040_0000: .")> KBW_040_0004 = 10004

            'Capa 6 - Familia 003 - Revalidación
            <EnumMember> <Description("Hubo un problema al verificar el tipo de pedimento, favor de contactar al administrador del sistema. [C6_003_0000]")> C6_003_0000 = 10100
            <EnumMember> <Description("La referencia ya fue registrada. [C6_003_0001]")> C6_003_0001 = 10101

            'Capa 6 - Familia 013 - Configuración de contabilidad
            <EnumMember> <Description("No existe la configuración de la cuenta contable para el tipo de retención ISR RESICO en esta empresa, consulte a sistemas [C6_013_1000].")> C6_013_1000 = 11000

            'Capa 6 - Familia 033 - Agrupaciones generales
            <EnumMember> <Description("Estas personas no pertenecen a la empresa seleccionada.")> C6_033_0000 = 11100
            <EnumMember> <Description("Se necesita que al menos un destinatario (persona, ejecutivo o departamento) se encuentre agregado.")> C6_033_0001 = 11101

            'Capa 5 - Familia 045 - Operaciones otros servicios
            <EnumMember> <Description("Error:C5_045_0000: Ocurrio un error en el registro de la información del previo, favor de comunicarse con TI")> C5_045_0000 = 11200
            <EnumMember> <Description("Error:C5_045_0001: Ocurrio un error en la actualización de la información del previo, favor de comunicarse con TI")> C5_045_0001 = 11201
            <EnumMember> <Description("Error:C5_045_0002: Ocurrio un error en el registro de la guía, favor de comunicarse con TI")> C5_045_0002 = 11202
            <EnumMember> <Description("Error:C5_045_0003: Ocurrio un error en la actualización de la guía, favor de comunicarse con TI")> C5_045_0003 = 11203
            <EnumMember> <Description("Error:C5_045_0004: Ocurrio un error en el registro de la orden de compra, favor de comunicarse con TI")> C5_045_0004 = 11204
            <EnumMember> <Description("Error:C5_045_0005: Ocurrio un error en la actualización de la orden de compra, favor de comunicarse con TI")> C5_045_0005 = 11205
            <EnumMember> <Description("Error:C5_045_0006: Ocurrio un error en la actualización de la fecha de inspección en los contenedores, favor de comunicarse con TI")> C5_045_0006 = 11206


            '#############################  SAX & SYN DEL CONTROL DE EXCEPCIONES ##################################
            <EnumMember> <Description("No se localizó alguno de los archivos: sax.projects.json o sax.settings.json")> C1_001_20000 = 20000
            <EnumMember> <Description("Settings of project was not found.")> C1_001_20001 = 20001
            <EnumMember> <Description("Error.")> C1_001_20002 = 20002
            <EnumMember> <Description("Error.")> C1_001_20003 = 20003
            <EnumMember> <Description("Error.")> C1_001_20004 = 20004
            <EnumMember> <Description("Error.")> C1_001_20005 = 20005
            <EnumMember> <Description("Error.")> C1_001_20006 = 20006
            <EnumMember> <Description("Error.")> C1_001_20007 = 20007
            <EnumMember> <Description("Error.")> C1_001_20008 = 20008
            <EnumMember> <Description("Error.")> C1_001_20009 = 20009
            <EnumMember> <Description("Error.")> C1_001_20010 = 20010
            <EnumMember> <Description("Error.")> C1_001_20011 = 20011
            <EnumMember> <Description("Error.")> C1_001_20012 = 20012
            <EnumMember> <Description("Error.")> C1_001_20013 = 20013

        End Enum

        <DataContract()>
        Enum TypeApplication
            <EnumMember> <Description("Undefined")> UND = 0
            <EnumMember> <Description("KromBase")> KBA = 1
            <EnumMember> <Description("MobileApp")> MAP = 2
            <EnumMember> <Description("Extranet")> EXT = 3
        End Enum

        <DataContract()>
        Enum ModuleID
            <EnumMember> <Description("Undefined")> UND = 0
        End Enum

        '<DataContract()>
        'Enum TypeStatus
        '    <EnumMember> Empty
        '    <EnumMember> Ok
        '    <EnumMember> Errors
        '    <EnumMember> Truncated
        'End Enum

        <DataContract()>
        Enum TypeStatus
            <EnumMember> Empty
            <EnumMember> Ok
            <EnumMember> OkBut
            <EnumMember> OkInfo
            <EnumMember> Errors
            <EnumMember> Truncated
            <EnumMember> Running
            <EnumMember> Paused
            <EnumMember> Stoped
            <EnumMember> Finished
        End Enum

#End Region

#Region "Builders"

        Sub New()

            _resultsCont = 0

            _status = TypeStatus.Empty

            _error = ErrorTypes.WS000

            _description = Nothing

        End Sub

        Sub New(ByVal statusNum_ As Int16,
                Optional ByVal resource_ As Object = Nothing,
                Optional ByVal info_ As String = Nothing,
                Optional ByVal errorType_ As ErrorTypes = ErrorTypes.Undefined)

            _resultsCont = 0

            _status = TypeStatus.Empty

            _error = ErrorTypes.WS000

            _description = Nothing

            Select Case statusNum_
                Case 0
                    SetErrors(resource_, errorType_) = info_
                Case 1
                    SetOK()
                Case 2
                    'Not implemented
            End Select


        End Sub

#End Region

#Region "Properties"

        Property SetErrors(ByVal resource_ As Object,
                                    Optional ByVal errorType_ As ErrorTypes = ErrorTypes.Undefined) As String 'Implements Status
            Set(value As String)

                SetError(resource_, value, errorType_)

            End Set

            Get
                Return Nothing

            End Get

        End Property

        Property FlagReturned As String 'Implements Status
            Get
                Return _flag
            End Get
            Set(value As String)
                _flag = value
            End Set
        End Property

        Property ObjectReturned As Object 'Implements Status
            Get
                Return _object
            End Get
            Set(value As Object)
                _object = value
            End Set
        End Property

        <DataMember>
        Property Status As TypeStatus 'Implements Status
            Get
                Return _status
            End Get
            Set(value As TypeStatus)
                _status = value
            End Set
        End Property

        <DataMember, XmlAttribute>
        Property Errors As ErrorTypes 'Implements Errors

            Get
                If _error = ErrorTypes.WS000 Then

                    Return Nothing

                Else

                    Return _error

                End If

            End Get
            Set(value As ErrorTypes)
                _error = value
            End Set

        End Property

        <DataMember, XmlAttribute>
        Property ResultsCount As Integer 'Implements ResultsCount
            Get
                Return _resultsCont
            End Get
            Set(value As Integer)
                _resultsCont = value
            End Set
        End Property


        <DataMember>
        Property ErrorDescription As String 'Implements ErrorDescription
            Get

                If _error = ErrorTypes.WS000 Then

                    Return Nothing

                Else
                    Return GetEnumDescription(DirectCast(Convert.ToInt32(_error), ErrorTypes)) & ":" & _description

                End If

            End Get
            Set(value As String)
                _description = value
            End Set
        End Property

        Public Property MessagesList As List(Of StackTrace)
            Get
                Return _messagesList
            End Get
            Set(value As List(Of StackTrace))
                _messagesList = value
            End Set
        End Property

        Public Property LastMessage As String
            Get
                Return _lastMessage
            End Get
            Set(value As String)
                _lastMessage = value
            End Set
        End Property

#End Region

#Region "Methods"

        Sub Clear()

            _resultsCont = 0

            _status = TypeStatus.Empty

            _error = ErrorTypes.WS000

            _description = Nothing


        End Sub

        Sub Initialize() 'Implements SetOK

            _resultsCont = 0

            _status = TypeStatus.Empty

            _description = Nothing

            _error = ErrorTypes.Undefined

        End Sub

        Sub SetOKBut(ByVal resource_ As Object,
                     ByVal message_ As String,
                     Optional ByVal id_ As Int32 = 0,
                     Optional ByVal stackObject_ As Object = Nothing) 'Implements SetOK

            _resultsCont = 0

            _status = TypeStatus.OkBut

            _description = Nothing

            _lastMessage = message_

            _error = ErrorTypes.Undefined

            _messagesList.Add(New StackTrace With {.Id = id_,
                                                  .Message = message_,
                                                  .Resource = resource_.GetType.Name,
                                                  .Type = TraceTypes.But,
                                                  .StackObject = stackObject_})

        End Sub

        Sub SetOKInfo(ByVal resource_ As Object,
                     ByVal message_ As String,
                     Optional ByVal id_ As Int32 = 0,
                     Optional ByVal stackObject_ As Object = Nothing) 'Implements SetOK

            _resultsCont = 0

            _status = TypeStatus.OkInfo

            _description = Nothing

            _lastMessage = message_

            _error = ErrorTypes.Undefined

            _object = stackObject_

            _messagesList.Add(New StackTrace With {.Id = id_,
                                                  .Message = message_,
                                                  .Resource = resource_.GetType.Name,
                                                  .Type = TraceTypes.Info,
                                                  .StackObject = stackObject_})

        End Sub

        Sub SetOK(Optional ByVal resultsCont_ As Integer = 0) 'Implements SetOK

            _resultsCont = 0

            _status = TypeStatus.Ok

            _description = Nothing

            _error = ErrorTypes.WS000

        End Sub

        Sub SetOK(ByVal resultsCont_ As Integer,
                  ByVal flagValue_ As String) 'Implements SetOK

            _resultsCont = resultsCont_

            _status = TypeStatus.Ok

            _description = Nothing

            _error = ErrorTypes.WS000

            _flag = flagValue_

        End Sub

        'G0_000_0000
        'Sub SetInfo(Optional ByVal info_ As InfoTypes = 0,
        '            Optional ByVal description_ As String = Nothing) 'Implements SetError

        '    _resultsCont = 0

        '    _info = info_

        '    _description = description_

        '    '·········································
        '    Dim tagWatcherCopy_ As New TagWatcher

        '    With tagWatcherCopy_
        '        ._status = _status
        '        ._resultsCont = 0
        '        ._info = info_
        '        ._error = ErrorTypes.C1_000_0001
        '        ._description = description_
        '    End With

        '    Dim eventMonitorTagWatcher_ As TagWatcherEventMonitor

        '    eventMonitorTagWatcher_ = TagWatcherEventMonitor.ObtainInstance()

        '    'eventMonitorTagWatcher_.Add(tagWatcherCopy_)
        '    eventMonitorTagWatcher_.Add(_status,
        '                                _resultsCont,
        '                                _error,
        '                                description_,
        '                                TagWatcherEventMonitor.CMFLayers.L01Core)


        '    ' ObtainInstance.

        'End Sub

        Sub SetError(ByVal error_ As Int32) 'Implements SetError

            _status = TypeStatus.Errors

            _resultsCont = 0

            _error = error_

            _description = Nothing

            '·········································

            Dim tagWatcherCopy_ As New TagWatcher

            With tagWatcherCopy_
                ._status = _status
                ._resultsCont = 0
                ._error = error_
                ._description = Nothing
            End With

            Dim eventMonitorTagWatcher_ As TagWatcherEventMonitor

            eventMonitorTagWatcher_ = TagWatcherEventMonitor.ObtainInstance()

            'eventMonitorTagWatcher_.Add(tagWatcherCopy_)
            eventMonitorTagWatcher_.Add(_status,
                                        _resultsCont,
                                        _error,
                                        Nothing,
                                        TagWatcherEventMonitor.CMFLayers.L01Core)


            ' ObtainInstance.

        End Sub

        Sub SetError(Optional ByVal error_ As ErrorTypes = 0,
                     Optional ByVal description_ As String = Nothing) 'Implements SetError

            _status = TypeStatus.Errors

            _resultsCont = 0

            _error = error_

            _description = description_

            '·········································

            Dim tagWatcherCopy_ As New TagWatcher

            With tagWatcherCopy_
                ._status = _status
                ._resultsCont = 0
                ._error = error_
                ._description = description_
            End With

            Dim eventMonitorTagWatcher_ As TagWatcherEventMonitor

            eventMonitorTagWatcher_ = TagWatcherEventMonitor.ObtainInstance()

            'eventMonitorTagWatcher_.Add(tagWatcherCopy_)
            eventMonitorTagWatcher_.Add(_status,
                                        _resultsCont,
                                        _error,
                                        description_,
                                        TagWatcherEventMonitor.CMFLayers.L01Core)


            ' ObtainInstance.

        End Sub

        Sub SetError(ByVal CMFresource_ As IRecursosSistemas.RecursosCMF,
                     Optional ByVal error_ As ErrorTypes = 0,
                     Optional ByVal description_ As String = Nothing) 'Implements SetError

            _status = TypeStatus.Errors

            _resultsCont = 0

            _error = error_

            _description = description_

            '·········································

            Dim tagWatcherCopy_ As New TagWatcher

            With tagWatcherCopy_
                ._status = _status
                ._resultsCont = 0
                ._error = error_
                ._description = description_
            End With

            Dim eventMonitorTagWatcher_ As TagWatcherEventMonitor

            eventMonitorTagWatcher_ = TagWatcherEventMonitor.ObtainInstance()

            Dim typeLayer_ As TagWatcherEventMonitor.CMFLayers = TagWatcherEventMonitor.CMFLayers.L01Core

            typeLayer_ = ReturnLayer(CMFresource_)

            eventMonitorTagWatcher_.Add(_status,
                                        _resultsCont,
                                        _error,
                                        description_,
                                        typeLayer_,
                                        CMFresource_)


        End Sub

        Sub SetError(ByVal resource_ As Object,
                     ByVal description_ As String,
                           Optional ByVal error_ As ErrorTypes = ErrorTypes.Undefined)

            _status = TypeStatus.Errors

            _error = error_

            _description = description_

            '·········································

            Dim tagWatcherCopy_ As New TagWatcher

            With tagWatcherCopy_
                ._status = _status
                ._error = error_
                ._description = description_
            End With

            Dim eventMonitorTagWatcher_ As TagWatcherEventMonitor

            eventMonitorTagWatcher_ = TagWatcherEventMonitor.ObtainInstance()

            'Dim typeLayer_ As TagWatcherEventMonitor.CMFLayers = TagWatcherEventMonitor.CMFLayers.L01Core

            'typeLayer_ = ReturnLayer(CMFresource_)

            'Dim name_ As List(Of T)

            eventMonitorTagWatcher_.Add(_status,
                                        _error,
                                        description_,
                                        TagWatcherEventMonitor.CMFLayers.L00Undefined,
                                        resource_.GetType.Name)


        End Sub

        Sub SetError(ByVal CMFresourceName_ As String,
                     Optional ByVal error_ As ErrorTypes = ErrorTypes.Undefined,
                     Optional ByVal description_ As String = Nothing) 'Implements SetError

            _status = TypeStatus.Errors

            _error = error_

            _description = description_

            '·········································

            Dim tagWatcherCopy_ As New TagWatcher

            With tagWatcherCopy_
                ._status = _status
                ._error = error_
                ._description = description_
            End With

            Dim eventMonitorTagWatcher_ As TagWatcherEventMonitor

            eventMonitorTagWatcher_ = TagWatcherEventMonitor.ObtainInstance()

            'Dim typeLayer_ As TagWatcherEventMonitor.CMFLayers = TagWatcherEventMonitor.CMFLayers.L01Core

            'typeLayer_ = ReturnLayer(CMFresource_)

            eventMonitorTagWatcher_.Add(_status,
                                        _error,
                                        description_,
                                        TagWatcherEventMonitor.CMFLayers.L00Undefined,
                                        CMFresourceName_)


        End Sub

        Private Function ReturnLayer(ByVal CMFResource_ As IRecursosSistemas.RecursosCMF) As TagWatcherEventMonitor.CMFLayers

            Dim returnType_ As TagWatcherEventMonitor.CMFLayers = TagWatcherEventMonitor.CMFLayers.L00Undefined

            Select Case CMFResource_
                'Capa 1, componentes
                Case Gsol_Organismo64,
                     Gsol_ConstructorVisual64,
                     Gsol_IConstructorVisual,
                     Gsol_IIniciaSesion,
                     Gsol_IniciaSesion64,
                     Gsol_ISectorEntorno,
                     Gsol_ISesion,
                     Gsol_ISesionWcf,
                     Gsol_EspacioTrabajo32,
                     Gsol_IEspacioTrabajo,
                     Gsol_ISectorEntorno,
                     Gsol_SectorEntorno32,
                     Gsol_EnsambladorCodigo64,
                     Gsol_IEnsambladorCodigo,
                     Wma_Reports_DesktopReports64,
                     Wma_Reports_ExcelApplicationReports64,
                     Wma_Reports_IReport

                    returnType_ = TagWatcherEventMonitor.CMFLayers.L01Core

                    'Capa 2
                Case Gsol_BaseDatos_Operaciones_IOperacionesCatalogo,
                     Gsol_BaseDatos_Operaciones_CaracteristicaCatlogo64,
                     Gsol_BaseDatos_Operaciones_ICaracteristicas,
                     Gsol_BaseDatos_Operaciones_OperacionesCatalogo64,
                     ICatalogo

                    returnType_ = TagWatcherEventMonitor.CMFLayers.L02DBOperations

                    'Capa 3
                Case Gsol_BaseDatos_Operaciones_LineaBaseCatalogos64,
                    gsol_krom_LineaBaseEnlaceDatos64,
                    Gsol_LineaBaseIniciaSesion64,
                    Gsol_Monitoreo_LineaBaseBitacora64

                    returnType_ = TagWatcherEventMonitor.CMFLayers.L03BaseLines

                    'Capa 4
                Case FormularioMaestroDetalle,
                     FormularioSencillo64,
                     GsDialogo64,
                     WclTextBoxFinder64,
                     Wma_Components_TextBoxDCKR64,
                     WclComboBoxFinder64,
                     Gsol_FormularioBase64,
                     Gsol_FormularioBaseCatalogo64,
                     Gsol_Modulos_GenericCatalog64,
                     Gsol_Modulos_GenericOperators64,
                     Wcl000Catalogo,
                     Wma_Components_WclPoll64,
                     gsol_Componentes_LockButton64

                    returnType_ = TagWatcherEventMonitor.CMFLayers.L04VisualComponents

                    'Capa 5
                Case Gsol_Clientes_GesAddendaGroupeSeb64,
                    GesMetas64,
                    GestionContabilidadElectronica64,
                    GestionDocumentosPagosHechos64,
                    GestionGruposImpuestos,
                    GestionMaestroOperaciones64,
                    GestionMaestroOperaciones64

                    returnType_ = TagWatcherEventMonitor.CMFLayers.L05Development

                    'Capa 6
                Case Gsol_BaseDatos_SysExpert64,
                    Gsol_Controladores_Contabilidad64,
                    gsol_krom_ControladorPeticiones64,
                    gsol_krom_ControladorWeb64,
                    gsol_krom_IControladorPeticiones64,
                    gsol_Procesos_Controladores_Auditorias64

                    returnType_ = TagWatcherEventMonitor.CMFLayers.L06ExtraControllers

                Case Else

                    returnType_ = TagWatcherEventMonitor.CMFLayers.L00Undefined

            End Select

            Return returnType_

        End Function

        Sub SetResults(ByVal result_ As TypeStatus,
                       ByVal resultsCont_ As Integer,
                       ByVal errorType_ As ErrorTypes) 'Implements SetResults

            _status = result_

            _resultsCont = resultsCont_

            _error = errorType_

        End Sub

        Public Function GetEnumDescription(ByVal EnumConstant As [Enum]) As String 'Implements GetEnumDescription
            Dim fi As FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
            Dim attr() As DescriptionAttribute =
                          DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute),
                          False), DescriptionAttribute())

            If attr.Length > 0 Then
                Return attr(0).Description
            Else
                Return EnumConstant.ToString()
            End If
        End Function

        Public Shared Widening Operator CType(v As TagWatcher) As Task(Of Object)
            Throw New NotImplementedException()
        End Operator

#End Region

    End Class

End Namespace