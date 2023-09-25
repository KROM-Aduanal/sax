'RAFA
Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.Reflection
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System

Namespace Wma.Exceptions

    <DataContract>
    <XmlSerializerFormat>
    Public Class TagWatcher

#Region "Attributes"

        Private _status As TypeStatus

        Private _error As ErrorTypes

        Private _resultsCont As Integer

        Private _description As String

#End Region

#Region "Enums"

        <DataContract>
        Public Enum ErrorTypes
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

            <EnumMember> <Description("Error:UNDEFINED: Not defined")> Undefined = 0

            'Capa 1, Gestión de permisos, ej. ( IEspacioTrabajo, ICredenciales, ISesion)
            <EnumMember> <Description("Error:C1_000_0001: No implementado")> C1_000_0001 = 1
            <EnumMember> <Description("Error:C1_000_1000: Not defined")> C1_000_1000 = 1000
            <EnumMember> <Description("Error:C1_000_1001: Not defined")> C1_000_1001 = 1001
            <EnumMember> <Description("Error:C1_000_1002: Not defined")> C1_000_1002 = 1002
            <EnumMember> <Description("Error:C1_000_1003: Not defined")> C1_000_1003 = 1003

            'Capa2, Construcción dinamica, ej. ( Constructor visual, IEntidad, IEventos...)
            <EnumMember> <Description("Error:C2_000_2000: Not defined")> C2_000_2000 = 2000
            <EnumMember> <Description("Error:C2_000_2001: Not defined")> C2_000_2001 = 2001
            <EnumMember> <Description("Error:C2_000_2002: Not defined")> C2_000_2002 = 2002
            <EnumMember> <Description("Error:C2_000_2003: Not defined")> C2_000_2003 = 2003

            'Capa2, Errore en la lectura del archivo de configuracion global
            <EnumMember> <Description("Error:C2_000_2004: Framework global configuration file has an error.")> C2_000_2004 = 2004

            'Capa3, gestores de operacion, DAO, ej. familia IOperaciones
            <EnumMember> <Description("Error:C3_000_3000: Not defined")> C3_000_3000 = 3000
            <EnumMember> <Description("Error:C3_000_3001: Not defined")> C3_000_3001 = 3001
            <EnumMember> <Description("Error:C3_000_3002: Not defined")> C3_000_3002 = 3002
            <EnumMember> <Description("Error:C3_000_3003: Not defined")> C3_000_3003 = 3003

            'Capa3, Excepciones de organismo
            <EnumMember> <Description("Error:C3_000_3900: No results")> C3_000_3900 = 3900
            <EnumMember> <Description("Error:C3_000_3901: Emerging exception")> C3_000_3901 = 3901


            'Capa4, User Interface, Controles de usuario, Ej. ( MaestroDetalle, FormulariosBase )
            <EnumMember> <Description("Error:C4_000_4000: Not defined")> C4_000_4000 = 4000
            <EnumMember> <Description("Error:C4_000_4001: Not defined")> C4_000_4001 = 4001
            <EnumMember> <Description("Error:C4_000_4002: Not defined")> C4_000_4002 = 4002
            <EnumMember> <Description("Error:C4_000_4003: Not defined")> C4_000_4003 = 4003

            'Capa5, Implementación, ej. ( Frm014Facturación, Frm013GestorPagosHechos)
            <EnumMember> <Description("Error:C5_000_5000: No puede facturar/guardar proforma porque hay anticipos sin forma de pago/fecha recepción")> C5_000_5000 = 5000
            <EnumMember> <Description("Error:C5_000_5001: No se puede facturar/guardar proforma porque los anticipos ya estan aplicados")> C5_000_5001 = 5001
            <EnumMember> <Description("Error:C5_000_5002: No se puede facturar/guardar proforma porque los anticipos no estan disponibles")> C5_000_5002 = 5002
            <EnumMember> <Description("Error:C5_000_5003: No se puede facturar/guardar proforma porque los anticipos estan por liberar")> C5_000_5003 = 5003
            <EnumMember> <Description("Error:C5_000_5004: No se puede facturar/guardar proforma porque los anticipos estan no disponible por liberar")> C5_000_5004 = 5004
            <EnumMember> <Description("Error:C5_000_5005: Existe un error en las fluctuaciones cambiarias, favor de verificar")> C5_000_5005 = 5005
            <EnumMember> <Description("Error:C5_000_5006: No se puede facturar/guarda proforma debido a que el total de las facturas sobrepasa el monto de la nota de crédito")> C5_000_5006 = 5006
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

            'Capa5 B - Inventario de Dimensiones

            'Capa5 C - LineaBaseControladorEnlace
            'No se encontraron las credenciales apropiadas


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
            <EnumMember> <Description("Error:C6_010_7017: La fecha de inicio de esta asginación se contrapone con alguna ótra relación.")> C6_010_7017 = 7017

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

            'Capa6 Familia de correo Electrónico 028
            <EnumMember> <Description("Error:C6_028_1000: El correo electrónico no logro registrarse en la tabla Bit028CorreoElectronicoEntrada.")> C6_028_1000 = 9100
            <EnumMember> <Description("Error:C6_028_1001: El correo electrónico no logro registrarse en la tabla Vin028BandejaEntrada.")> C6_028_1001 = 9101
            <EnumMember> <Description("Error:C6_028_1002: El attachment no logro registrarse en la tabla de vinculación de documentos Vin028DocumentosCorreoElectronicoEntrada.")> C6_028_1002 = 9102
            <EnumMember> <Description("Error:C6_028_1003: No se logro enviar el correo electrónico de error al equipo de soporte.")> C6_028_1003 = 9103
            <EnumMember> <Description("Error:C6_028_1004: Excepción no controlada.")> C6_028_1004 = 9104
            <EnumMember> <Description("Error:C6_028_1005: El attachment no logro encontrar el directorio.")> C6_028_1005 = 9105
            <EnumMember> <Description("Error:C6_028_1006: El correo electrónico no logro enviarse debido a que no existe un buzón de envio para este usuario.")> C6_028_1006 = 9106
            <EnumMember> <Description("Error:C6_028_1007: El correo electrónico no logro registrarse en la tabla Bit028CorreoElectronicoEnvios.")> C6_028_1007 = 9107
            <EnumMember> <Description("Error:C6_028_1008: La cantidad de intentos de envio ha sido sobrepasada.")> C6_028_1008 = 9108
            <EnumMember> <Description("Error:C6_028_1009: El adjunto no tiene una clave de documento.")> C6_028_1009 = 9109
            <EnumMember> <Description("Error:C6_028_1010: El adjunto no contiene alguna ruta de donde se puede obtener el documento.")> C6_028_1010 = 9110
            <EnumMember> <Description("Error:C6_028_1011: El adjunto no existe en el directorio proporcionado.")> C6_028_1011 = 9111

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
            <EnumMember> <Description("Error:C6_012_1027: No tienes privilegios para modificar el documento." & _
                                      vbCrLf & "Favor de solicitar los permisos al departamento de TI. ")> C6_012_1027 = 9227
            <EnumMember> <Description("Error:C6_012_1028: No tienes privilegios para consultar el documento." & _
                                      vbCrLf & "Favor de solicitar los permisos al departamento de TI. ")> C6_012_1028 = 9228
            <EnumMember> <Description("Error:C6_012_1029: No tienes privilegios para esta acción debido a que no eres administrador." & _
                                      vbCrLf & "Favor de solicitar los permisos al departamento de TI. ")> C6_012_1029 = 9229
            <EnumMember> <Description("Error:C6_012_1030: No se logró insertar el documento, esto se debe a que ya se encuentra registrado en el maestro de documentos.")> C6_012_1030 = 9230
            <EnumMember> <Description("Error:C6_012_1031: No se puede abrir este documento ya que se encuentra eliminado.")> C6_012_1031 = 9231
            <EnumMember> <Description("Error:C6_012_1032: No se logro obtener información de esa operación considerando la referencia.")> C6_012_1032 = 9232
            <EnumMember> <Description("Error:C6_012_1033: No tienes privilegios para agregar el documento." & _
                          vbCrLf & "Favor de solicitar los permisos al departamento de TI. ")> C6_012_1033 = 9233
            <EnumMember> <Description("Error:C6_012_1034: No se logro obtener información de esa operación considerando la razón social.")> C6_012_1034 = 9234

            'Capa6 Solicitudes de pago familia 013
            <EnumMember> <Description("Error:C6_013_9001: La referenia operativa no tiene un ejecutivo de cuenta asignado, por lo cual se recomienda realizar lo siguiente: " & _
                             vbCrLf & "1.-Es necesario ir al módulo de maestro de operaciones e ingresar el ejecutivo de la refencia operativa correcto" & _
                             vbCrLf & "(en la solicitud de pago aparecerá el nombre de ese ejecutivo" & _
                             vbCrLf & "2.-Es necesario acercarse al encargado de tráfico de su oficina y solicitar que actualice la cartera de clientes," & _
                             vbCrLf & "esto con la finalidad de no incurrir nuevamente en este proceso")> C6_013_9001 = 9400

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

        End Enum

        <DataContract>
        Public Enum TypeApplication
            <EnumMember> <Description("Undefined")> UND = 0
            <EnumMember> <Description("KromBase")> KBA = 1
            <EnumMember> <Description("MobileApp")> MAP = 2
            <EnumMember> <Description("Extranet")> EXT = 3
        End Enum

        <DataContract>
        Public Enum ModuleID
            <EnumMember> <Description("Undefined")> UND = 0
        End Enum

        <DataContract>
        Public Enum TypeStatus
            <EnumMember> Empty
            <EnumMember> Ok
            <EnumMember> Errors
            <EnumMember> Truncated
            <EnumMember> Running
            <EnumMember> Paused
            <EnumMember> Stoped
            <EnumMember> Finished
        End Enum

#End Region

#Region "Builders"

        Public Sub New()

            _resultsCont = 0

            _status = TypeStatus.Empty

            _error = ErrorTypes.WS000

            _description = Nothing

        End Sub

#End Region

#Region "Properties"

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

        End Sub

        Sub SetError(Optional ByVal error_ As ErrorTypes = 0, _
                     Optional ByVal description_ As String = Nothing) 'Implements SetError

            _status = TypeStatus.Errors

            _resultsCont = 0

            _error = error_

            _description = description_

        End Sub

        Sub SetResults(ByVal result_ As TypeStatus, _
                       ByVal resultsCont_ As Integer, _
                       ByVal errorType_ As ErrorTypes) 'Implements SetResults

            _status = result_

            _resultsCont = resultsCont_

            _error = errorType_

        End Sub

        Public Function GetEnumDescription(ByVal EnumConstant As [Enum]) As String 'Implements GetEnumDescription
            Dim fi As FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
            Dim attr() As DescriptionAttribute = _
                          DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), _
                          False), DescriptionAttribute())

            If attr.Length > 0 Then
                Return attr(0).Description
            Else
                Return EnumConstant.ToString()
            End If
        End Function

#End Region

    End Class

End Namespace