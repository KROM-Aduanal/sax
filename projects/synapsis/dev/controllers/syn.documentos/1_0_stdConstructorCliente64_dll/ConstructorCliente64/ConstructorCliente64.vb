Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports gsol.krom
Imports MongoDB.Bson
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos.CamposClientes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Operaciones
Imports Wma.Exceptions

Namespace Syn.Documento
    <Serializable()>
    Public Class ConstructorCliente
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

#End Region

#Region "Builders"

        Sub New()

            Inicializa(Nothing,
                        TiposDocumentoElectronico.Clientes,
                        True)

        End Sub
        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.Clientes,
                       construir_)

        End Sub
        Public Sub New(ByVal folioDocumento_ As String,
                       ByVal referencia_ As String,
                       ByVal idCliente_ As Int32,
                       ByVal nombreCliente_ As String
                       )

            Inicializa(folioDocumento_,
                         referencia_,
                         idCliente_,
                         nombreCliente_,
                         TiposDocumentoElectronico.Clientes)

        End Sub

#End Region

#Region "Methods"

        Public Sub ConfiguracionNotificaciones()



        End Sub
        Public Overrides Sub ConstruyeEncabezado()

            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            ' Encabezado principal de la referencia
            ConstruyeSeccion(seccionEnum_:=SeccionesClientes.SCS1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)


            'ConfiguracionNotificaciones()

        End Sub
        Public Overrides Sub ConstruyeCuerpo()

            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ConstruyeSeccion(seccionEnum_:=SeccionesClientes.SCS2,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesClientes.SCS3,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesClientes.SCS4,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesClientes.SCS5,
            tipoBloque_:=TiposBloque.Cuerpo,
            conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesClientes.SCS6,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesClientes.SCS7,
            tipoBloque_:=TiposBloque.Cuerpo,
            conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesClientes.SCS8,
            tipoBloque_:=TiposBloque.Cuerpo,
            conCampos_:=False)

            '6,7,8
        End Sub


#End Region

#Region "Funciones"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_

                'Generales
                Case SeccionesClientes.SCS1
                    Return New List(Of Nodo) From {
                                             Item(CamposClientes.CP_CVE_EMPRESA, Entero),
                                             Item(CamposClientes.CA_RAZON_SOCIAL, Texto, longitud_:=120),
                                             Item(CamposClientes.CP_CVE_DIVISION_MI_EMPRESA, Entero),
                                             Item(CamposClientes.CA_RFC_CLIENTE, Texto, longitud_:=13),
                                             Item(CamposClientes.CA_TAX_ID, Texto, longitud_:=11),
                                             Item(CamposClientes.CA_CURP_CLIENTE, Texto, longitud_:=18),
                                             Item(CamposClientes.CP_TIPO_PERSONA, Entero),
                                             Item(CamposClientes.CP_CLIENTE_EXTRANJERO, Entero),
                                             Item(CamposClientes.CP_CLIENTE_HABILITADO, Booleano),
                                             Item(CamposClientes.CP_ID_EMPRESA, IdObject),
 _
                                             Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=250),
                                             Item(CamposDomicilio.CA_CALLE, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CA_NUMERO_EXTERIOR, Texto, longitud_:=10),
                                             Item(CamposDomicilio.CA_NUMERO_INTERIOR, Texto, longitud_:=10),
                                             Item(CamposDomicilio.CA_NUMERO_EXT_INT, Texto, longitud_:=20),
                                             Item(CamposDomicilio.CA_CODIGO_POSTAL, Texto, longitud_:=10),
                                             Item(CamposDomicilio.CA_COLONIA, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CA_LOCALIDAD, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CA_CIUDAD, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CA_MUNICIPIO, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, Texto, longitud_:=3),
                                             Item(CamposDomicilio.CA_ENTIDAD_FEDERATIVA, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CA_ENTIDAD_MUNICIPIO, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CA_CVE_PAIS, Texto, longitud_:=3),
                                             Item(CamposDomicilio.CA_PAIS, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CP_ID_DOMICILIO, IdObject),
                                             Item(CamposDomicilio.CP_SEC_DOMICILIO, Entero, 8)
                              }

                'Aduana por defecto
                Case SeccionesClientes.SCS2

                    Return New List(Of Nodo) From {
                                             Item(CamposClientes.CP_CVE_ADUANA_SECCION, Entero),
                                             Item(CamposClientes.CP_MODALIDAD_ADUANA_SECCION, Texto, longitud_:=250),
                                             Item(CamposClientes.CP_CVE_PATENTE_ADUANAL, Entero),
                                             Item(CamposClientes.CP_PATENTE_ADUANAL, Texto, longitud_:=250),
                                             Item(CamposClientes.CP_CVE_TIPO_OPERACION, Entero),
                                             Item(CamposClientes.CP_TIPO_OPERACION, Texto, longitud_:=11)
                              }

                'Contactos del imp/exp
                Case SeccionesClientes.SCS3

                    Return New List(Of Nodo) From {
                                             Item(CamposClientes.CP_CVE_PERSONA, Entero),
                                             Item(CamposClientes.CP_NOMBRE_CONTACTO, Texto, longitud_:=250),
                                             Item(CamposClientes.CP_RFC_CONTACTO, Texto, longitud_:=13),
                                             Item(CamposClientes.CP_NOMBRE_PUESTO, Texto, longitud_:=100),
 _
                                             Item(CamposClientes.CP_INFO_CONTACTO, Texto, longitud_:=100),
                                             Item(CamposClientes.CP_TELEFONO1_CONTACTO, Texto, longitud_:=100),
                                             Item(CamposClientes.CP_MOVIL_CONTACTO, Texto, longitud_:=100),
                                             Item(CamposClientes.CP_EMAIL_CONTACTO, Texto, longitud_:=100)
                              }

                'Configuración
                Case SeccionesClientes.SCS4

                    Return New List(Of Nodo) From {
                                            Item(SeccionesClientes.SCS5, False),
                                            Item(SeccionesClientes.SCS6, False),
                                            Item(SeccionesClientes.SCS7, False),
                                            Item(SeccionesClientes.SCS8, False)
                              }

                'Sellos digitales
                Case SeccionesClientes.SCS5

                    Return New List(Of Nodo) From {
                                             Item(CamposClientes.CP_RUTA_ARCHIVO_SER_SELLOS, Texto, longitud_:=100),
                                             Item(CamposClientes.CP_RUTA_ARCHIVO_KEY_SELLOS, Texto, longitud_:=100),
                                             Item(CamposClientes.CP_FECHA_VIGENCIA_SELLOS, Fecha),
                                             Item(CamposClientes.CP_CONTRASENIA_SELLOS, Texto, longitud_:=50),
                                             Item(CamposClientes.CP_CVE_WEB_SERVICES_SELLOS, Texto, longitud_:=50)
                              }

                'Encargo conferido
                Case SeccionesClientes.SCS6

                    Return New List(Of Nodo) From {
                                             Item(CamposClientes.CP_CVE_PATENTE_ADUANAL_ENCARGO, Entero),
                                             Item(CamposClientes.CP_PATENTE_ADUANAL_ENCARGO, Texto),
                                             Item(CamposClientes.CP_ACTIVO_ENCARGO, Texto),
                                             Item(CamposClientes.CP_FECHA_INICIO_ENCARGO, Fecha),
                                             Item(CamposClientes.CP_FECHA_FIN_ENCARGO, Fecha)
                              }

                'Pago electrónico
                Case SeccionesClientes.SCS7

                    Return New List(Of Nodo) From {
                                             Item(CamposClientes.CP_CVE_TIPO_OPERACION, Entero),
                                             Item(CamposClientes.CP_TIPO_OPERACION_PAGO, Texto, longitud_:=11),
                                             Item(CamposClientes.CP_PATENTE_ADUANA_SECCION_PAGO, Texto, longitud_:=4),
                                             Item(CamposClientes.CP_CVE_DOCUMENTO, Entero),
                                             Item(CamposClientes.CP_CVE_DOCUMENTO_PAGO, Texto, longitud_:=250),
                                             Item(CamposClientes.CP_CVE_BANCO, Entero),
                                             Item(CamposClientes.CP_CVE_BANCO_PAGO, Texto, longitud_:=50),
                                             Item(CamposClientes.CP_ID_CUENTA_PAGO, Texto, longitud_:=50),
                                             Item(CamposClientes.CP_RANGO_PAGO, Texto),
                                             Item(CamposClientes.CP_RANGO_MINIMO_PAGO, Real, cantidadEnteros_:=18, cantidadDecimales_:=4),
                                             Item(CamposClientes.CP_RANGO_MAXIMO_PAGO, Real, cantidadEnteros_:=18, cantidadDecimales_:=4),
                                             Item(CamposClientes.CP_CUENTA_DEFAULT_PAGO, Texto),
                                             Item(CamposClientes.CP_ACTIVO_PAGO, Texto)
                              }


                    '.Attribute(CamposClientes.CP_CVE_TIPO_OPERACION).Valor = data_.scTipoOperacionPago.Value
                    '.Attribute(CamposClientes.CP_CVE_TIPO_OPERACION).ValorPresentacion = data_.scTipoOperacionPago.Text
                    ''.Attribute(CamposClientes.CP_TIPO_OPERACION_PAGO).Valor = data_.scTipoOperacionPago.Text

                    '.Attribute(CamposClientes.CP_PATENTE_ADUANA_SECCION_PAGO).Valor = data_.scPatentePago.Value
                    '.Attribute(CamposClientes.CP_PATENTE_ADUANA_SECCION_PAGO).ValorPresentacion = data_.scPatentePago.Text

                    ''.Attribute(CamposClientes.CP_CVE_DOCUMENTO).Valor = data_.scClaveDocumentoPago.Value
                    '.Attribute(CamposClientes.CP_CVE_DOCUMENTO_PAGO).Valor = data_.scClaveDocumentoPago.Value
                    '.Attribute(CamposClientes.CP_CVE_DOCUMENTO_PAGO).ValorPresentacion = data_.scClaveDocumentoPago.Text

                    ''.Attribute(CamposClientes.CP_CVE_BANCO).Valor = data_.scBancoPago.Value

                    '.Attribute(CamposClientes.CP_CVE_BANCO_PAGO).Valor = data_.scBancoPago.Value
                    '.Attribute(CamposClientes.CP_CVE_BANCO_PAGO).ValorPresentacion = data_.scBancoPago.Text

                    '.Attribute(CamposClientes.CP_ID_CUENTA_PAGO).Valor = data_.icCuentaPago
                    '.Attribute(CamposClientes.CP_RANGO_PAGO).Valor = data_.icRangoCuentaPago
                    '.Attribute(CamposClientes.CP_RANGO_MINIMO_PAGO).Valor = Nothing
                    '.Attribute(CamposClientes.CP_RANGO_MAXIMO_PAGO).Valor = Nothing
                    '.Attribute(CamposClientes.CP_CUENTA_DEFAULT_PAGO).Valor = Nothing 'data_.icCuentaPago

                'Expediente legal
                Case SeccionesClientes.SCS8

                    Return New List(Of Nodo) From {
                                             Item(CamposClientes.CP_CVE_NOMBRE_PLANTILLA, Entero),
                                             Item(CamposClientes.CP_NOMBRE_PLANTILLA, Texto),
                                             Item(CamposClientes.CP_RUTA_DOCUMENTO, Texto),
                                             Item(CamposClientes.CP_VIGENCIA_DOCUMENTO, Fecha)
                              }

                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function



#End Region

    End Class

End Namespace

