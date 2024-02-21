Imports gsol.krom
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento

Namespace Syn.Documento

    <Serializable()>
    Public Class ConstructorPedimentoNormal
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"


#End Region

#Region "Builders"

        Sub New()

            Inicializa(Nothing,
                        TiposDocumentoElectronico.PedimentoNormal,
                        True)

        End Sub
        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.PedimentoNormal,
                       construir_)

        End Sub
        Public Sub New(ByVal folioDocumento_ As String,
                       ByVal folioOperacion_ As String,
                       ByVal idCliente_ As Int32,
                       ByVal nombreCliente_ As String
                       )

            Inicializa(folioDocumento_,
                         folioOperacion_,
                         idCliente_,
                         nombreCliente_,
                         TiposDocumentoElectronico.PedimentoNormal)

        End Sub

#End Region

#Region "Methods"
        Public Overrides Sub ConstruyeEncabezado()

            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            ' Encabezado principal del pedimento
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)

        End Sub

        Public Overrides Sub ConstruyeEncabezadoPaginasSecundarias()

            _estructuraDocumento(TiposBloque.EncabezadoPaginasSecundarias) = New List(Of Nodo)

            ' Encabezado para páginas secundarias del pedimento
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS2,
                             tipoBloque_:=TiposBloque.EncabezadoPaginasSecundarias,
                             conCampos_:=True)

        End Sub

        Public Overrides Sub ConstruyeCuerpo()

            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            'Datos del importador/exportador
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS3,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Tasas a nivel pedimento
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS6,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Cuadro de liquidación
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS7,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Deposito referenciado - línea de captura - información del pago
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS9,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Datos del proveedor o comprador
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS10,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Datos del destinatario
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS11,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Datos del transporte y transportista
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS12,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Fechas
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS14,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Candados
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS15,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Guias, Manifiestos, conocimientos de embarque o documentos de transporte
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS16,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Contenedores/Equipo ferrocarril/Número economico del vehiculo.
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS17,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)


            'Identificadores (Nivel pedimento)
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS18,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Cuentas aduaneras y cuentas aduaneras de garantia (Nivel pedimento)
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS19,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Descargos
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS20,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Compensaciones
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS21,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Documentos que amparan las formas de pago distintas a efectivo….
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS22,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Observaciones ( nivel pedimento)
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS23,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Partidas
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS24,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Fin del pedimento
            ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS43,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            '- - - - - JC - configuración anterior de la construcción del cuerpo - - - - - - - 

            ''Datos del importador/exportador
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS3,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=True)

            ''Tasas a nivel pedimento
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS6,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=False)

            ''Cuadro de liquidación
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS7,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=True)

            ''Deposito referenciado - línea de captura - información del pago
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS9,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=True)

            ''Datos del proveedor o comprador
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS10,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=False)

            ''Datos del destinatario
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS11,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=False)

            ''Datos del transporte y transportista
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS12,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=False)

            ''Fechas
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS14,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=True)

            ''Candados
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS15,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=False)

            ''Guias, Manifiestos, conocimientos de embarque o documentos de transporte
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS16,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=False)

            ''Contenedores/Equipo ferrocarril/Número economico del vehiculo.
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS17,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=False)


            ''Identificadores (Nivel pedimento)
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS18,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=False)

            ''Cuentas aduaneras y cuentas aduaneras de garantia (Nivel pedimento)
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS19,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=False)

            ''Descargos
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS20,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=False)

            ''Compensaciones
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS21,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=False)

            ''Documentos que amparan las formas de pago distintas a efectivo….
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS22,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=False)

            ''Observaciones ( nivel pedimento)
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS23,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=True)

            ''Partidas
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS24,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=False)

            ''Fin del pedimento
            'ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS43,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=True)

        End Sub

        Public Overrides Sub ConstruyePiePagina()

            _estructuraDocumento(TiposBloque.PiePagina) = New List(Of Nodo)

            With _estructuraDocumento(TiposBloque.PiePagina)

                'Pie de pagina del pedimento
                ConstruyeSeccion(seccionEnum_:=SeccionesPedimento.ANS44,
                                 tipoBloque_:=TiposBloque.PiePagina,
                                 conCampos_:=True)

            End With

        End Sub

#End Region

#Region "Funciones"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_

                'Encabezado principal del pedimento
                Case SeccionesPedimento.ANS1

                    'NodoPedimento
                    Return New List(Of Nodo) From {
                                   Item(CA_NUMERO_PEDIMENTO_COMPLETO, Texto, longitud_:=21),
                                   Item(CA_TIPO_OPERACION, Entero, longitud_:=3),
                                   Item(CA_CVE_PEDIMENTO, Texto, longitud_:=2),
                                   Item(CA_REGIMEN, Texto, longitud_:=3),
                                   Item(CA_DESTINO_ORIGEN, Entero, longitud_:=2),
                                   Item(CA_TIPO_CAMBIO, Real, cantidadEnteros_:=4, cantidadDecimales_:=5),
                                   Item(CA_PESO_BRUTO, Real, cantidadEnteros_:=11, cantidadDecimales_:=3),
                                   Item(CA_ADUANA_ENTRADA_SALIDA, Texto, longitud_:=3),
                                   Item(CA_MEDIO_TRANSPORTE, Entero, longitud_:=2),
                                   Item(CA_MEDIO_TRANSPORTE_ARRIBO, Entero, longitud_:=2),
                                   Item(CA_MEDIO_TRANSPORTE_SALIDA, Entero, longitud_:=2),
                                   Item(CA_VALOR_DOLARES, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                                   Item(CA_VALOR_ADUANA, Entero, longitud_:=12),
                                   Item(CA_PRECIO_PAGADO_VALOR_COMERCIAL, Entero, longitud_:=12),
                                   Item(CA_VALOR_SEGUROS, Entero, longitud_:=12),
                                   Item(CA_SEGUROS, Entero, longitud_:=12),
                                   Item(CA_FLETES, Entero, longitud_:=12),
                                   Item(CA_EMBALAJES, Entero, longitud_:=12),
                                   Item(CA_OTROS_INCREMENTABLES, Entero, longitud_:=12),
                                   Item(CA_TRANSPORTE_DECREMENTABLES, Entero, longitud_:=12),
                                   Item(CA_SEGURO_DECREMENTABLES, Entero, longitud_:=12),
                                   Item(CA_CARGA_DECREMENTABLES, Entero, longitud_:=12),
                                   Item(CA_DESCARGA_DECREMENTABLES, Entero, longitud_:=12),
                                   Item(CA_OTROS_DECREMENTABLES, Entero, longitud_:=12),
                                   Item(CA_ACUSE_ELECTRONICO_VALIDACION, Texto, longitud_:=8),
                                   Item(CA_CODIGO_BARRAS, Texto, longitud_:=6), 'Revisar captura dice el ConstructorCampoPedimento64
                                   Item(CA_CLAVE_SAD, Texto, longitud_:=3),
                                   Item(CA_MARCAS_NUMEROS_TOTAL_BULTOS, Texto), 'No tiene longitud en ConstructorCampoPedimento64
                                   Item(CA_CERTIFICACION, Texto, longitud_:=6), 'Revisar captura dice el ConstructorCampoPedimento64
                                   Item(CA_CLAVE_SAD, Texto, longitud_:=3),
                                   Item(CA_ANIO_VALIDACION, Texto, longitud_:=2),
                                   Item(CA_ADUANA_SIN_SECCION, Texto, longitud_:=2),
                                   Item(CA_CVE_TIPO_OPERACION, Entero, longitud_:=1),
                                   Item(CA_FECHA_VALIDACION, Fecha),
                                   Item(CP_REFERENCIA, Texto, longitud_:=8),
                                   Item(CP_MODALIDAD_ADUANA_PATENTE, Entero, longitud_:=3),
                                   Item(CP_MODALIDAD, Entero, longitud_:=1),
                                   Item(CP_EJECUTIVO_CUENTA, Entero, longitud_:=5),
                                   Item(CP_NUMERO_CLIENTE, Entero, longitud_:=6),
                                   Item(CA_CVE_PREVALIDADOR, Entero, longitud_:=5),
                                   Item(CA_VALIDADOR_DESIGNADO, Texto, longitud_:=30),
                                   Item(CA_NUMERO_SEMANA, Entero, longitud_:=3),
                                   Item(CA_ARCHIVO_VALIDACION, Texto, longitud_:=16),
                                   Item(CA_ACUSE_ELECTRONICO_PAGO, Texto, longitud_:=8),
                                   Item(CA_ARCHIVO_PAGO, Texto, longitud_:=16)
                                   }

                ' Encabezado para páginas secundarias del pedimento
                Case SeccionesPedimento.ANS2

                    Return New List(Of Nodo) From {
                               Item(CA_TIPO_CAMBIO, Real, cantidadEnteros_:=4, cantidadDecimales_:=5),
                               Item(CA_NUMERO_PEDIMENTO_COMPLETO, Texto, longitud_:=21),
                               Item(CA_TIPO_OPERACION, Entero, longitud_:=3),
                               Item(CA_CVE_PEDIMENTO, Texto, longitud_:=2),
                               Item(CA_RFC_IOE, Texto, longitud_:=13),
                               Item(CA_CURP_IOE, Texto, longitud_:=18)
                         }

                ' Datos del importador/exportador
                Case SeccionesPedimento.ANS3

                    Return New List(Of Nodo) From {
                               Item(CA_RFC_IOE, Texto, longitud_:=13),
                               Item(CA_CURP_IOE, Texto, longitud_:=18),
                               Item(CA_RAZON_SOCIAL_IOE, Texto, longitud_:=120),
                               Item(CA_DOMICILIO_IOE, Texto, longitud_:=196),
                               Item(CA_CALLE_IOE, Texto, longitud_:=80),
                               Item(CA_NUMERO_INTERIOR_IOE, Texto, longitud_:=10),
                               Item(CA_NUMERO_EXTERIOR_IOE, Texto, longitud_:=10),
                               Item(CA_CODIGO_POSTAL_IOE, Texto, longitud_:=10),
                               Item(CA_MUNICIPIO_CIUDAD_IOE, Texto, longitud_:=80),
                               Item(CA_ENTIDAD_FEDERATIVA_IOE, Texto, longitud_:=3),
                               Item(CA_PAIS_IOE, Texto, longitud_:=3)
                         }

                ' Datos generales del pedimento complementario
                Case SeccionesPedimento.ANS4

                    Return New List(Of Nodo)

                        ' Prueba suficiente - JC: Esto es otra sección, no es prueba suficiente
                Case SeccionesPedimento.ANS5

                    Return New List(Of Nodo) From {
                               Item(CA_CONCEPTO, Texto, longitud_:=7),
                               Item(CA_FORMA_PAGO, Entero, longitud_:=3),
                               Item(CA_IMPORTE, Entero, longitud_:=12),
                               Item(CA_CVE_CONCEPTO_NIVEL_PEDIMENTO, Entero, longitud_:=2),
                               Item(CA_CVE_PAIS_EXPORTACION, Texto, longitud_:=3)
                         }

                        ' Tasas a nivel pedimento
                Case SeccionesPedimento.ANS6

                    Return New List(Of Nodo) From {
                               Item(CA_CONTRIBUCION, Texto, longitud_:=7),
                               Item(CA_CVE_TIPO_TASA, Entero, longitud_:=2),
                               Item(CA_TASA, Real, cantidadEnteros_:=5, cantidadDecimales_:=1),
                               Item(CA_CVE_CONCEPTO_NIVEL_PEDIMENTO, Entero, longitud_:=2)
                         }

                        ' Cuadro de liquidación
                Case SeccionesPedimento.ANS7

                    Return New List(Of Nodo) From {
                               Item(CA_EFECTIVO, Entero, longitud_:=12),
                               Item(CA_OTROS, Entero, longitud_:=12),
                               Item(CA_TOTAL, Entero, longitud_:=12),
                               Item(SeccionesPedimento.ANS55, False)
                         }

                    'Partidas del cuadro de liquidación
                Case SeccionesPedimento.ANS55

                    Return New List(Of Nodo) From {
                               Item(CA_CONCEPTO, Texto, longitud_:=7),
                               Item(CA_DESCRIPCION_CONCEPTO, Texto, longitud_:=50),
                               Item(CA_FORMA_PAGO, Entero, longitud_:=3),
                               Item(CA_IMPORTE, Entero, longitud_:=12),
                               Item(CA_CVE_CONCEPTO_NIVEL_PEDIMENTO, Entero, longitud_:=2)
                         }

                        ' Informe de la industria automotriz
                Case SeccionesPedimento.ANS8

                    Return New List(Of Nodo)

                        ' Deposito referenciado - línea de captura - información del pago
                Case SeccionesPedimento.ANS9

                    Return New List(Of Nodo) From {
                               Item(CA_DEPOSITO_REFERENCIADO, Texto), 'No tiene longitud en ConstructorCampoPedimento64
                               Item(CA_NUMERO_PEDIMENTO, Texto, longitud_:=7),
                               Item(CA_PATENTE, Texto, longitud_:=4),
                               Item(CA_PAGO_ELECTRONICO, Texto, longitud_:=22),
                               Item(CA_NOMBRE_INSTITUCION_BANCARIA, Texto, longitud_:=30),
                               Item(CA_LINEA_CAPTURA, Texto, longitud_:=20),
                               Item(CA_NUMERO_OPERACION_BANCARIA, Texto, longitud_:=20),
                               Item(CA_NUMERO_TRANSACCION_SAT, Texto, longitud_:=25),
                               Item(CA_MEDIO_PRESENTACION, Texto, longitud_:=23),
                               Item(CA_MEDIO_RECEPCION_COBRO, Texto, longitud_:=26),
                               Item(CA_CLAVE_SAD, Texto, longitud_:=3),
                               Item(CA_EFECTIVO, Entero, longitud_:=12),
                               Item(CA_FECHA_PAGO, Fecha)
                         }

                        ' Datos del proveedor o comprador
                Case SeccionesPedimento.ANS10



                    Return New List(Of Nodo) From {
                               Item(CA_ID_FISCAL_PROVEEDOR, Texto, longitud_:=30),
                               Item(CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC, Texto, longitud_:=120),
                               Item(CA_DOMICILIO_POC, Texto, longitud_:=190),
                               Item(CA_VINCULACION, Texto, longitud_:=2),
                               Item(CA_CVE_VINCULACION, Entero, longitud_:=1),
                                                                              _
                               Item(CA_CALLE_POC, Texto, longitud_:=80),
                               Item(CA_NUMERO_INTERIOR_POC, Texto, longitud_:=10),
                               Item(CA_NUMERO_EXTERIOR_POC, Texto, longitud_:=10),
                               Item(CA_CODIGO_POSTAL_POC, Texto, longitud_:=10),
                               Item(CA_MUNICIPIO_CIUDAD_POC, Texto, longitud_:=80),
                               Item(CA_ENTIDAD_FEDERATIVA_POC, Texto, longitud_:=3),
                               Item(CA_PAIS_POC, Texto, 3),
                                                           _
                               Item(SeccionesPedimento.ANS13, False),
                               Item(CamposGlobales.CP_IDENTITY, Entero)
                         }

                        ' Datos del destinatario
                Case SeccionesPedimento.ANS11

                    Return New List(Of Nodo) From {
                               Item(CA_ID_FISCAL_DESTINATARIO, Texto, longitud_:=17),
                               Item(CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO, Texto, longitud_:=120),
                               Item(CA_DOMICILIO_DESTINATARIO, Texto, longitud_:=193),
                               Item(CA_CALLE_DESTINATARIO, Texto, longitud_:=80),
                               Item(CA_NUMERO_INTERIOR_DESTINATARIO, Texto, longitud_:=10),
                               Item(CA_NUMERO_EXTERIOR_DESTINATARIO, Texto, longitud_:=10),
                               Item(CA_CODIGO_POSTAL_DESTINATARIO, Texto, longitud_:=10),
                               Item(CA_MUNICIPIO_CIUDAD_DESTINATARIO, Texto, longitud_:=80),
                               Item(CA_PAIS_DESTINATARIO, Texto, longitud_:=3)
                         }

                        ' Datos del transporte y transportista
                Case SeccionesPedimento.ANS12

                    Return New List(Of Nodo) From {
                               Item(CA_ID_TRANSPORTE, Texto, longitud_:=30),
                               Item(CA_CVE_PAIS_TRANSPORTE, Texto, longitud_:=3),
                               Item(CA_NOMBRE_RAZON_SOCIAL_TRANSPORTE, Texto, longitud_:=120),
                               Item(CA_CVE_RFC_TRANSPORTE, Texto, longitud_:=13),
                               Item(CA_CURP_TRANSPORTE_PERSONA_FISICA, Texto, longitud_:=18),
                               Item(CA_DOMICILIO_TRANSPORTE, Texto, longitud_:=150),
                               Item(CA_TOTAL_BULTOS, Entero, longitud_:=5),
                               Item(CamposGlobales.CP_IDENTITY, Entero)
                         }

                        ' CFDi o ducumento equivalente
                Case SeccionesPedimento.ANS13

                    Return New List(Of Nodo) From {
                               Item(CA_CFDI_FACTURA, Texto, longitud_:=40),
                               Item(CA_FECHA_FACTURA, Fecha),
                               Item(CA_INCOTERM, Texto, longitud_:=3),
                               Item(CA_CVE_MONEDA_FACTURA, Texto, longitud_:=3),
                               Item(CA_MONTO_MONEDA_FACTURA, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                               Item(CA_FACTOR_MONEDA, Real, cantidadEnteros_:=2, cantidadDecimales_:=5),
                               Item(CA_MONTO_USD, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                               Item(CA_NUMERO_ACUSE_DE_VALOR, Texto, longitud_:=40),
                               Item(CA_ACUSE_VALOR, Texto, longitud_:=20)
                         }

                        ' Fechas
                Case SeccionesPedimento.ANS14

                    Return New List(Of Nodo) From {
                               Item(CA_FECHA_ENTRADA, Fecha),
                               Item(CA_FECHA_PAGO, Fecha),
                               Item(CA_FECHA_EXTRACCION, Fecha),
                               Item(CA_FECHA_PRESENTACION, Fecha),
                               Item(CA_FECHA_IMPORTACION_EUA_CAN, Fecha),
                               Item(CA_FECHA_ORIGINAL, Fecha),
                               Item(CA_FECHA_REGISTRO, Fecha),
                               Item(CA_FECHA_REVALIDACION, Fecha),
                               Item(CA_FECHA_ZARPE, Fecha),
                               Item(CA_FECHA_PREVIO, Fecha),
                               Item(CA_FECHA_FONDEO, Fecha),
                               Item(CA_FECHA_ATRAQUE, Fecha),
                               Item(CA_FECHA_DESPACHO, Fecha),
                               Item(CA_FECHA_ARRIBO, Fecha),
                               Item(CA_FECHA_ENTREGA, Fecha),
                               Item(CA_FECHA_FACTURACION, Fecha)
                         }

                        ' Candados
                Case SeccionesPedimento.ANS15

                    Return New List(Of Nodo) From {
                               Item(CA_NUMERO_CANDADO, Texto, longitud_:=21),
                               Item(CA_NUMERO_CANDADO_1RA, Texto), 'Revisar captura dice el ConstructorCampoPedimento64
                               Item(CA_NUMERO_CANDADO_2DA, Texto)  'Revisar captura dice el ConstructorCampoPedimento64
                         }

                        ' Guias, Manifiestos, conocimientos de embarque o documentos de transporte
                Case SeccionesPedimento.ANS16

                    Return New List(Of Nodo) From {
                               Item(CA_GUIA_MANIFIESTO_BL, Texto, longitud_:=20),
                               Item(CA_MASTER_HOUSE, Texto, longitud_:=1)
                         }



                        ' Contenedores/Equipo ferrocarril/Número economico del vehiculo
                Case SeccionesPedimento.ANS17

                    Return New List(Of Nodo) From {
                               Item(CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO, Texto, longitud_:=12),
                               Item(CA_CVE_TIPO_CONTENEDOR, Entero, longitud_:=2)
                         }

                        ' Identificadores (Nivel pedimento)
                Case SeccionesPedimento.ANS18

                    Return New List(Of Nodo) From {
                               Item(CA_CVE_IDENTIFICADOR, Texto, longitud_:=2),
                               Item(CA_COMPLEMENTO_1, Texto, longitud_:=20),
                               Item(CA_COMPLEMENTO_2, Texto, longitud_:=30),
                               Item(CA_COMPLEMENTO_3, Texto, longitud_:=40)
                         }


                        ' Cuentas aduaneras y cuentas aduaneras de garantia (Nivel pedimento)
                Case SeccionesPedimento.ANS19

                    Return New List(Of Nodo) From {
                               Item(CA_CVE_CUENTA_ADUANERA, Entero, longitud_:=1),
                               Item(CA_CVE_TIPO_GARANTIA, Entero, longitud_:=1),
                               Item(CA_INSTITUCION_EMISORA_GARANTIA, Texto, longitud_:=120),
                               Item(CA_NUMERO_CONTRATO, Entero, longitud_:=17),
                               Item(CA_FOLIO_CONSTANCIA, Entero, longitud_:=17),
                               Item(CA_IMPORTE_TOTAL_CONSTANCIA, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                               Item(CA_FECHA_EMISION_CONSTANCIA, Fecha),
                               Item(CA_CANTIDAD_UMT_PRECIO_ESTIMADO_PEDIMENTO, Real, cantidadEnteros_:=10, cantidadDecimales_:=4),
                               Item(CA_TITULOS_ASIGNADOS_PEDIMENTO, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                               Item(CA_VALOR_UNITARIO_TITULO_PEDIMENTO, Real, cantidadEnteros_:=10, cantidadDecimales_:=4)
                         }

                        ' Descargos
                Case SeccionesPedimento.ANS20

                    Return New List(Of Nodo) From {
                               Item(CA_NUMERO_PEDIMENTO_ORIGINAL_COMPLETO, Texto, longitud_:=21),
                               Item(CA_FECHA_PEDIMENTO_ORIGINAL, Fecha),
                               Item(CA_CVE_PEDIMENTO_ORIGINAL, Texto, longitud_:=2),
                               Item(CA_ANIO_VALIDACION_ORIGINAL, Texto, longitud_:=4),
                               Item(CA_ANIO_VALIDACION_ORIGINAL, Texto, longitud_:=2),
                               Item(CA_PATENTE_ORIGINAL, Texto, longitud_:=4),
                               Item(CA_ADUANA_DESPACHO_ORIGINAL, Texto, longitud_:=3),
                               Item(CA_ADUANA_DESPACHO_ORIGINAL_2, Texto, longitud_:=2),
                               Item(CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS, Texto, longitud_:=7),
                               Item(CA_FRACCION_ORIGINAL, Texto, longitud_:=8),
                               Item(CA_UM_ORIGINAL, Entero, longitud_:=2),
                               Item(CA_CANTIDAD_MERCANCIA_UMT_DESCARGO, Real, cantidadEnteros_:=12, cantidadDecimales_:=5)
                         }

                        ' Compensaciones
                Case SeccionesPedimento.ANS21

                    Return New List(Of Nodo) From {
                               Item(CA_COMPENSACION_CONTRIBUCION, Texto, longitud_:=7),
                               Item(CA_COMPENSACION_NUMERO_PEDIMENTO_ORIGINAL_COMPLETO, Texto, longitud_:=21),
                               Item(CA_FECHA_PAGO_ORIGINAL_PARA_COMPENSACION, Fecha),
                               Item(CA_IMPORTE_GRAVAMEN_COMPENSACION, Entero, longitud_:=12),
                               Item(CA_CVE_CONCEPTO_COMPENSACION, Entero, longitud_:=2),
                               Item(CA_COMPENSACION_ANIO_VALIDACION_ORIGINAL, Texto, longitud_:=4),
                               Item(CA_COMPENSACION_ANIO_VALIDACION_2_ORIGINAL, Texto, longitud_:=2),
                               Item(CA_COMPENSACION_PATENTE_ORIGINAL, Texto, longitud_:=4),
                               Item(CA_COMPENSACION_ADUANA_DESPACHO_ORIGINAL, Texto, longitud_:=3),
                               Item(CA_COMPENSACION_ADUANA_DESPACHO_ORIGINAL_2, Texto, longitud_:=2),
                               Item(CA_COMPENSACION_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS, Texto, longitud_:=7)
                         }


                        ' Documentos que amparan las formas de pago distintas a efectivo
                Case SeccionesPedimento.ANS22

                    Return New List(Of Nodo) From {
                               Item(CA_PAGOS_VIRTUALES_FORMA_PAGO, Entero, 3),
                               Item(CA_NOMBRE_INSTITUCION_EMISORA_DOCUMENTO, Texto, longitud_:=120),
                               Item(CA_NUMERO_DOCUMENTO, Texto, longitud_:=40),
                               Item(CA_FECHA_EXPOCICION_DOCUMENTO, Fecha),
                               Item(CA_IMPORTE_DOCUMENTO, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                               Item(CA_SALDO_DISPONIBLE_DOCUMENTO, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                               Item(CA_IMPORTE_PAGADO_PEDIMENTO, Entero, longitud_:=14)
                         }

                        ' Observaciones ( nivel pedimento)
                Case SeccionesPedimento.ANS23

                    Return New List(Of Nodo) From {
                               Item(CA_OBSERVACIONES_PEDIMENTO, Texto)
                         }

                        ' Partidas
                Case SeccionesPedimento.ANS24

                    Return New List(Of Nodo) From {
                               Item(CA_SECUENCIA_PARTIDA, Entero, longitud_:=5),
                               Item(CA_FRACCION_ARANCELARIA_PARTIDA, Texto, longitud_:=8),
                               Item(CA_NICO_PARTIDA, Texto, longitud_:=2),
                               Item(CA_CVE_METODO_VALORACION_PARTIDA, Entero, longitud_:=2),
                               Item(CA_CVE_UMC_PARTIDA, Entero, longitud_:=2),
                               Item(CA_CANTIDAD_UMC_PARTIDA, Real, cantidadEnteros_:=12, cantidadDecimales_:=3),
                               Item(CA_CVE_UMT_PARTIDA, Entero, longitud_:=2),
                               Item(CA_CANTIDAD_UMT_PARTIDA, Real, cantidadEnteros_:=13, cantidadDecimales_:=5),
                               Item(CA_CVE_PAIS_VENDEDOR_PARTIDA, Texto, longitud_:=3),
                               Item(CA_CVE_PAIS_ORIGEN_PARTIDA, Texto, longitud_:=3),
                               Item(CA_CVE_PAIS_COMPRADOR_PARTIDA, Texto, longitud_:=3),
                               Item(CA_CVE_PAIS_DESTINO_PARTIDA, Texto, longitud_:=3),
                               Item(CA_DESCRIPCION_MERCANCIA_PARTIDA, Texto, longitud_:=250),
                               Item(CA_VALOR_ADUANA_PARTIDA, Entero, longitud_:=12),
                               Item(CA_PRECIO_PAGADO_PARTIDA, Entero, longitud_:=12),
                               Item(CA_VALOR_COMERCIAL_PARTIDA, Entero, longitud_:=12),
                               Item(CA_PRECIO_UNITARIO_PARTIDA, Real, cantidadEnteros_:=10, cantidadDecimales_:=5),
                               Item(CA_VALOR_AGREGADO_PARTIDA, Entero, longitud_:=12),
                               Item(CA_MARCA_PARTIDA, Texto, longitud_:=80),
                               Item(CA_MODELO_PARTIDA, Texto, longitud_:=80),
                               Item(CA_CODIGO_PRODUCTO_PARTIDA, Texto, longitud_:=20),
                               Item(CA_VINCULACION, Texto, longitud_:=2),
                               Item(CA_CVE_VINCULACION, Entero, longitud_:=1),
                               Item(CA_ENTIDAD_FEDERATIVA_ORIGEN, Texto, longitud_:=3),
                               Item(CA_ENTIDAD_FEDERATIVA_DESTINO, Texto, longitud_:=3),
                               Item(CA_ENTIDAD_FEDERATIVA_COMPRADOR, Texto, longitud_:=3),
                               Item(CA_ENTIDAD_FEDERATIVA_VENDEDOR, Texto, longitud_:=3),
                               Item(CamposGlobales.CP_IDENTITY, Entero),
                               Item(SeccionesPedimento.ANS25, True),
                               Item(SeccionesPedimento.ANS26, True),
                               Item(SeccionesPedimento.ANS27, True),
                               Item(SeccionesPedimento.ANS28, True),
                               Item(SeccionesPedimento.ANS29, True),
                               Item(SeccionesPedimento.ANS32, True),
                               Item(SeccionesPedimento.ANS33, True),
                               Item(SeccionesPedimento.ANS34, True),
                               Item(SeccionesPedimento.ANS35, True),
                               Item(SeccionesPedimento.ANS36, True)
                         }

                        ' Mercancias
                Case SeccionesPedimento.ANS25

                    Return New List(Of Nodo) From {
                               Item(CA_VINCULACION_NUMERO_SERIE_PARTIDA, Texto, longitud_:=25),
                               Item(CA_KILOMETRAJE_PARTIDA, Entero, longitud_:=6)
                         }

                        ' Regulaciones y restricciones no arancelarias
                Case SeccionesPedimento.ANS26

                    Return New List(Of Nodo) From {
                               Item(CA_CVE_PERMISO, Texto, longitud_:=3),
                               Item(CA_NUMERO_PERMISO, Texto, longitud_:=50),
                               Item(CA_FIRMA_ELECTRONICA_PERMISO, Texto, longitud_:=32),
                               Item(CA_VALOR_USD_PARTIDA, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                               Item(CA_CANTIDAD_UMT_UMC, Real, cantidadEnteros_:=13, cantidadDecimales_:=5)
                         }

                        ' Identificadores ( Nivel partida )
                Case SeccionesPedimento.ANS27

                    Return New List(Of Nodo) From {
                               Item(CA_CVE_IDENTIFICADOR_PARTIDA, Texto, longitud_:=2),
                               Item(CA_COMPLEMENTO_1_PARTIDA, Texto, longitud_:=20),
                               Item(CA_COMPLEMENTO_2_PARTIDA, Texto, longitud_:=50),
                               Item(CA_COMPLEMENTO_3_PARTIDA, Texto, longitud_:=40)
                         }

                        ' Cuentas aduaneras de garantia ( Nivel partida)
                Case SeccionesPedimento.ANS28

                    Return New List(Of Nodo) From {
                               Item(CA_CVE_TIPO_GARANTIA_PARTIDA, Entero, longitud_:=2),
                               Item(CA_INSTITUCION_EMISORA_GARANTIA_PARTIDA, Texto, longitud_:=120),
                               Item(CA_FECHA_EXPIRACION_CONSTANCIA_PARTIDA, Fecha),
                               Item(CA_NUMERO_CUENTA_GARANTIA_PARTIDA, Entero, longitud_:=17),
                               Item(CA_FOLIO_CONSTANCIA_PARTIDA, Texto, longitud_:=17),
                               Item(CA_MONTO_TOTAL_CONSTANCIA_PARTIDA, Real, cantidadEnteros_:=12, cantidadDecimales_:=2),
                               Item(CA_PRECIO_ESTIMADO_PARTIDA, Real, cantidadEnteros_:=10, cantidadDecimales_:=4),
                               Item(CA_CANTIDAD_UMT_PRECIO_ESTIMADO_PARTIDA, Real, cantidadEnteros_:=10, cantidadDecimales_:=4)
                         }

                        ' Tasas y contribuciones a nivel partida
                Case SeccionesPedimento.ANS29

                    Return New List(Of Nodo) From {
                               Item(CA_CVE_CONTRIBUCION_PARTIDA, Texto, longitud_:=7),
                               Item(CA_TASA_PARTIDA, Real),
                               Item(CA_CVE_TIPO_TASA_PARTIDA, Entero, longitud_:=2),
                               Item(CA_FORMA_PAGO_PARTIDA, Entero, longitud_:=3),
                               Item(CA_IMPORTE_PARTIDA, Entero, longitud_:=12),
                               Item(CA_CVE_CONTRIBUCION_NIVEL_PARTIDA, Entero, longitud_:=2)
                         }

                        ' Contribuciones a nivel partida
                Case SeccionesPedimento.ANS30

                    Return New List(Of Nodo)

                        ' Partidas del informe de la industria automotriz
                Case SeccionesPedimento.ANS31

                    Return New List(Of Nodo)

                        ' Determinación de contribuciones a nivel partida al amparo del Art 2.5 del T-MEC
                Case SeccionesPedimento.ANS32

                    Return New List(Of Nodo) From {
                               Item(CA_MONTO_MERCANCIAS_NO_ORIGINARIAS_PARTIDA, Entero, longitud_:=12),
                               Item(CA_MONTO_IGI_PARTIDA, Entero, longitud_:=12)
                         }

                        ' Detalle de importación a EUA/CAN al amparo del Art. 2.5 del T-MEC
                Case SeccionesPedimento.ANS33

                    Return New List(Of Nodo) From {
                               Item(CA_SECUENCIA_PARTIDA, Entero, longitud_:=5),
                               Item(CA_FRACCION_ARANCELARIA_PARTIDA, Texto, longitud_:=8),
                               Item(CA_NUMERO_PEDIMENTO, Texto, longitud_:=7),
                               Item(CA_CVE_PAIS_EXPORTACION, Texto, longitud_:=3)
                         }

                        ' Determinación y/o pago de contribuciones por aplicación del art 2.5 del TMEC en el pedimento de exporación(Retorno)
                Case SeccionesPedimento.ANS34

                    Return New List(Of Nodo) From {
                               Item(CA_SECUENCIA_PARTIDA, Entero, longitud_:=5),
                               Item(CA_FRACCION_ARANCELARIA_PARTIDA, Texto, longitud_:=8),
                               Item(CA_MONTO_MERCANCIAS_NO_ORIGINARIAS_PARTIDA, Entero, longitud_:=12),
                               Item(CA_MONTO_IGI_PARTIDA, Entero, longitud_:=12),
                               Item(CA_NUMERO_PEDIMENTO, Texto, longitud_:=7)
                         }

                        ' Pago de contribuciones a nivel partida por aplicación del Art. 2.5 del T-MEC
                Case SeccionesPedimento.ANS35

                    Return New List(Of Nodo) From {
                                Item(CA_SECUENCIA_PARTIDA, Entero, longitud_:=5),
                                Item(CA_FRACCION_ARANCELARIA_PARTIDA, Texto, longitud_:=8),
                                Item(CA_CVE_CONTRIBUCION_PARTIDA, Texto, longitud_:=7),
                                Item(CA_FORMA_PAGO_PARTIDA, Entero, longitud_:=3),
                                Item(CA_IMPORTE_PARTIDA, Entero, longitud_:=12),
                                Item(CA_NUMERO_PEDIMENTO, Texto, longitud_:=7),
                                Item(CA_CVE_PAIS_EXPORTACION, Texto, longitud_:=3)
                        }

                         ' Observaciones ( Nivel partida )
                Case SeccionesPedimento.ANS36

                    Return New List(Of Nodo) From {
                               Item(CA_OBSERVACIONES_PARTIDA, Texto) 'No tiene longitud en ConstructorCampoPedimento64
                         }

                        ' Rectificaciones
                Case SeccionesPedimento.ANS37

                    Return New List(Of Nodo) From {
                               Item(CA_RECTIFICACION_FECHA_PEDIMENTO_ORIGINAL, Fecha),
                               Item(CA_RECTIFICACION_CVE_PEDIMENTO_ORIGINAL, Texto, longitud_:=2),
                               Item(CA_RECTIFICACION_PATENTE_ORIGINAL, Texto, longitud_:=4),
                               Item(CA_RECTIFICACION_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS, Texto, longitud_:=7),
                               Item(CA_RECTIFICACION_ANIO_VALIDACION_2_ORIGINAL, Texto, longitud_:=2),
                               Item(CA_RECTIFICACION_ANIO_VALIDACION_ORIGINAL, Texto, longitud_:=4),
                               Item(CA_RECTIFICACION_ADUANA_DESPACHO_ORIGINAL, Texto, longitud_:=3),
                               Item(CA_RECTIFICACION_ADUANA_DESPACHO_ORIGINAL_2, Texto, longitud_:=2),
                               Item(CA_CVE_PEDIMENTO_RECTIFICACION, Texto, longitud_:=72),
                               Item(CA_FECHA_PAGO_PEDIMENTO_RECTIFICACION, Fecha)
                    }

                        ' Diferencias de contribuciones ( Nivel pedimento )
                Case SeccionesPedimento.ANS38

                    Return New List(Of Nodo) From {
                               Item(CA_DIFERENCIA, Entero, longitud_:=7),
                               Item(CA_DIFERENCIA_EFECTIVO, Entero, longitud_:=7),
                               Item(CA_DIFERENCIA_OTROS, Entero, longitud_:=7),
                               Item(CA_DIFERENCIA_TOTAL, Entero, longitud_:=7),
                               Item(CA_CONCEPTO_DIFERENCIA_CONTRIBUCION, Texto, longitud_:=7),
                               Item(CA_CVE_CONCEPTO_DIFERENCIA_CONTRIBUCION, Entero, longitud_:=2),
                               Item(CA_FORMA_PAGO_DIFERENCIAS, Entero, longitud_:=2)
                    }

                        ' Prueba suficiente
                Case SeccionesPedimento.ANS39

                    Return New List(Of Nodo) From {
                               Item(CA_CVE_PAIS_EXPORTACION, Entero, longitud_:=7),
                               Item(CA_NUMERO_DOCUMENTO_IMPORTACION_EU_CAN, Entero, longitud_:=7),
                               Item(CA_CVE_PRUEBA, Entero, longitud_:=7)
                    }

                        ' Encabezado para determinacion de contribuciones a nivel partdida para pedimentos complementarios al amparo del art. T-Mec
                Case SeccionesPedimento.ANS40

                    Return New List(Of Nodo)

                        ' Encabezado para determinación de contribuciones a nivel partida para pedimentos complementarios al amparo del los articulos 14 de la decision o 15 del TLCAELC
                Case SeccionesPedimento.ANS41

                    Return New List(Of Nodo)

                        ' Instructivo de llenado del pedimento de tránsito para el transbordo
                Case SeccionesPedimento.ANS42

                    Return New List(Of Nodo)

                        ' Fin de pedimento
                Case SeccionesPedimento.ANS43

                    Return New List(Of Nodo) From {
                               Item(CA_FIN_PEDIMENTO, Texto, longitud_:=17),
                               Item(CA_NUMERO_TOTAL_PARTIDAS, Entero, longitud_:=4),
                               Item(CA_CVE_PREVALIDADOR, Texto, longitud_:=3)
                         }

                        ' Pie de pagina del pedimento
                Case SeccionesPedimento.ANS44

                    Return New List(Of Nodo) From {
                               Item(CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA, Texto, longitud_:=120),
                               Item(CA_RFC_AA, Texto, longitud_:=13),
                               Item(CA_CURP_AA_REPRESENTANTE_LEGAL, Texto, longitud_:=18),
                               Item(CA_NOMBRE_MANDATARIO_REPRESENTANTE_AA, Texto, longitud_:=120),
                               Item(CA_RFC_MANDATARIO_AA_REPRESENTANTE_ALMACEN, Texto, longitud_:=13),
                               Item(CA_CURP_MANDATARIO_AA_REPRESENTANTE_ALMACEN, Texto, longitud_:=18),
                               Item(CA_PATENTE, Texto, longitud_:=4),
                               Item(CA_EFIRMA, Texto, longitud_:=360),
                               Item(CA_CERTIFICADO_FIRMA, Texto, longitud_:=25),
                               Item(CA_TIPO_FIGURA, Entero, longitud_:=1)
                         }

                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select


            Return Nothing

        End Function

#End Region


    End Class

End Namespace