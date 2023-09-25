
Imports gsol.krom
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior


Namespace Syn.Documento
    <Serializable()>
    Public Class ConstructorFacturaComercial
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

#End Region

#Region "Builders"

        Sub New()

            Inicializa(Nothing,
                        TiposDocumentoElectronico.FacturaComercial,
                        True)

        End Sub
        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.FacturaComercial,
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
                         TiposDocumentoElectronico.FacturaComercial)

        End Sub

#End Region

#Region "Methods"
        Public Overrides Sub ConstruyeEncabezado()

            ' Encabezado principal de la factura comercial
            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            ' Construye las secciones 

            ConstruyeSeccion(seccionEnum_:=SeccionesFacturaComercial.SFAC1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesFacturaComercial.SFAC2,
                           tipoBloque_:=TiposBloque.Encabezado,
                           conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesFacturaComercial.SFAC3,
                           tipoBloque_:=TiposBloque.Encabezado,
                           conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeEncabezadoPaginasSecundarias()

            'Construir la parte encabezado para páginas secundarias
            '_estructuraDocumento(TiposBloque.EncabezadoPaginasSecundarias) = New List(Of Nodo)

            'Construir una sección
            'ConstruyeSeccion(seccionDocumento_:=SeccionesGenericas.SGS1,
            '                 tipoBloque_:=TiposBloque.EncabezadoPaginasSecundarias,
            '                 conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeCuerpo()

            'Construir la parte de cuerpo de la factura comercial
            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ' Construye las secciones 

            ConstruyeSeccion(seccionEnum_:=SeccionesFacturaComercial.SFAC4,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesFacturaComercial.SFAC5,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=True)


            ConstruyeSeccion(seccionEnum_:=SeccionesFacturaComercial.SFAC6,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=True)

            'ConstruyeSeccion(seccionEnum_:=SeccionesFacturaComercial.SFAC7,
            '                tipoBloque_:=TiposBloque.Cuerpo,
            '                conCampos_:=True)

            'ConstruyeSeccion(seccionEnum_:=SeccionesFacturaComercial.SFAC8,
            '                tipoBloque_:=TiposBloque.Cuerpo,
            '                conCampos_:=True)

            'ConstruyeSeccion(seccionEnum_:=SeccionesFacturaComercial.SFAC9,
            '                tipoBloque_:=TiposBloque.Cuerpo,
            '                conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyePiePagina()

            'Construir la parte pie de página
            '_estructuraDocumento(TiposBloque.PiePagina) = New List(Of Nodo)

            'Construir una sección
            'ConstruyeSeccion(seccionDocumento_:=SeccionesGenericas.SGS1,
            '                 tipoBloque_:=TiposBloque.PiePagina,
            '                 conCampos_:=True)

        End Sub

#End Region

#Region "Functions"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_

                ' Generales
                Case SeccionesFacturaComercial.SFAC1
                    Return New List(Of Nodo) From {
                                             Item(CamposFacturaComercial.CA_NUMERO_FACTURA, Texto, longitud_:=40),
                                             Item(CamposAcuseValor.CA_NUMERO_ACUSEVALOR, Texto, longitud_:=40),
                                             Item(CamposFacturaComercial.CA_FECHA_FACTURA, Fecha),
                                             Item(CamposAcuseValor.CA_FECHA_ACUSEVALOR, Fecha),
                                             Item(CamposClientes.CA_RAZON_SOCIAL, Texto, longitud_:=120),
                                             Item(CamposClientes.CA_TAX_ID, Texto, longitud_:=11),
                                             Item(CamposClientes.CA_RFC_CLIENTE, Texto, longitud_:=13),
                                             Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450),
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
                                             Item(CamposFacturaComercial.CA_CVE_PAIS_FACTURACION, Texto, longitud_:=3),
                                             Item(CamposFacturaComercial.CA_PAIS_FACTURACION, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CP_TIPO_OPERACION, Texto, longitud_:=11),
                                             Item(CamposFacturaComercial.CA_CVE_INCOTERM, Texto, longitud_:=3),
                                             Item(CamposFacturaComercial.CP_VALOR_FACTURA, Real, cantidadEnteros_:=14, cantidadDecimales_:=2),
                                             Item(CamposFacturaComercial.CA_MONEDA_FACTURACION, Texto, longitud_:=3),
                                             Item(CamposFacturaComercial.CP_VALOR_MERCANCIA, Real, cantidadEnteros_:=14, cantidadDecimales_:=2),
                                             Item(CamposFacturaComercial.CP_MONEDA_VALOR_MERCANCIA, Texto, longitud_:=3),
                                             Item(CamposFacturaComercial.CP_PESO_TOTAL, Real, cantidadEnteros_:=14, cantidadDecimales_:=3),
                                             Item(CamposFacturaComercial.CP_APLICA_ENAJENACION, Booleano),
                                             Item(CamposFacturaComercial.CA_APLICA_SUBDIVISION, Entero), ' Preguntar que significa el siguiente paramentro en los Enteros
                                             Item(CamposFacturaComercial.CP_SERIE_FOLIO_FACTURA, Texto, longitud_:=100)
                                             }

                ' Datos del proveedor
                Case SeccionesFacturaComercial.SFAC2
                    Return New List(Of Nodo) From {
                                             Item(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, Texto, longitud_:=120),
                                             Item(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR, Texto, longitud_:=11),
                                             Item(CamposProveedorOperativo.CA_RFC_PROVEEDOR, Texto, longitud_:=13),
                                             Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450),
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
                                             Item(CamposDomicilio.CA_CVE_PAIS, Texto, longitud_:=3),
                                             Item(CamposDomicilio.CA_PAIS, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CA_CVE_VINCULACION, Entero),
                                             Item(CamposFacturaComercial.CP_CVE_METODO_VALORACION, Entero),
                                             Item(CamposFacturaComercial.CA_APLICA_CERTIFICADO, Entero),
                                             Item(CamposFacturaComercial.CP_NOMBRE_CERTIFICADOR, Texto, longitud_:=120),
                                             Item(CamposFacturaComercial.CP_ORDEN_COMPRA, Texto, longitud_:=60)
                    }

                ' Datos del destinatario
                Case SeccionesFacturaComercial.SFAC3
                    Return New List(Of Nodo) From {
                                             Item(CamposDestinatario.CA_RAZON_SOCIAL, Texto, longitud_:=120),
                                             Item(CamposDestinatario.CA_TAX_ID, Texto, longitud_:=11),
                                             Item(CamposDestinatario.CA_RFC_DESTINATARIO, Texto, longitud_:=13),
                                             Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450),
                                             Item(CamposDomicilio.CA_CALLE, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CA_NUMERO_EXTERIOR, Texto, longitud_:=10),
                                             Item(CamposDomicilio.CA_NUMERO_INTERIOR, Texto, longitud_:=10),
                                             Item(CamposDomicilio.CA_CODIGO_POSTAL, Texto, longitud_:=10),
                                             Item(CamposDomicilio.CA_COLONIA, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CA_LOCALIDAD, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CA_CIUDAD, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CA_MUNICIPIO, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, Texto, longitud_:=3),
                                             Item(CamposDomicilio.CA_ENTIDAD_FEDERATIVA, Texto, longitud_:=80),
                                             Item(CamposDomicilio.CA_CVE_PAIS, Texto, longitud_:=3),
                                             Item(CamposDomicilio.CA_PAIS, Texto, longitud_:=80)
                    }

                ' Partidas
                Case SeccionesFacturaComercial.SFAC4
                    Return New List(Of Nodo) From {
                                             Item(CamposFacturaComercial.CP_NUMERO_PARTIDA, Entero),
                                             Item(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA, Texto, longitud_:=20),
                                             Item(CamposFacturaComercial.CA_VALOR_FACTURA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                             Item(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA, Texto, longitud_:=3),
                                             Item(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                             Item(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA, Texto, longitud_:=3),
                                             Item(CamposFacturaComercial.CA_CVE_METODO_VALORACION_PARTIDA, Texto, longitud_:=1),
                                             Item(CamposFacturaComercial.CA_PESO_NETO_PARTIDA, Real, cantidadEnteros_:=14, cantidadDecimales_:=3),
                                             Item(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=5),
                                             Item(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA, Texto, longitud_:=3),
                                             Item(CamposFacturaComercial.CP_CANTIDAD_FACTURA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                             Item(CamposFacturaComercial.CP_UNIDAD_MEDIDA_FACTURA_PARTIDA, Entero),
                                             Item(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA, Texto, longitud_:=250),
                                             Item(CamposFacturaComercial.CP_APLICA_DESCRIPCION_COVE_PARTIDA, Booleano),
                                             Item(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=3),
                                             Item(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA, Entero),
                                             Item(CamposFacturaComercial.CA_DESCRIPCION_COVE_PARTIDA, Texto, longitud_:=250),
                                             Item(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA, Texto, longitud_:=8),
                                             Item(CamposFacturaComercial.CA_CANTIDAD_TARIFA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                             Item(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA, Entero),
                                             Item(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA, Texto, longitud_:=2),
                                             Item(CamposFacturaComercial.CA_LOTE_PARTIDA, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA, Texto, longitud_:=25),
                                             Item(CamposFacturaComercial.CA_MARCA_PARTIDA, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CA_MODELO_PARTIDA, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CA_SUBMODELO_PARTIDA, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CA_KILOMETRAJE_PARTIDA, Entero),
                                             Item(CamposFacturaComercial.CP_ORDEN_COMPRA_PARTIDA, Texto, longitud_:=60),
                                             Item(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO, Texto, longitud_:=3),
                                             Item(CamposGlobales.CP_IDENTITY, Entero)
                    }

                ' Incrementables
                Case SeccionesFacturaComercial.SFAC5

                    Return New List(Of Nodo) From {
                                            Item(CamposFacturaComercial.CA_FLETES, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                            Item(CamposFacturaComercial.CA_MONEDA_FLETES, Texto, longitud_:=3),
                                            Item(CamposFacturaComercial.CA_SEGURO, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                            Item(CamposFacturaComercial.CA_MONEDA_SEGUROS, Texto, longitud_:=3),
                                            Item(CamposFacturaComercial.CA_EMBALAJES, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                            Item(CamposFacturaComercial.CA_MONEDA_EMBALAJES, Texto, longitud_:=3),
                                            Item(CamposFacturaComercial.CA_OTROS_INCREMENTABLES, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                            Item(CamposFacturaComercial.CA_MONEDA_OTROS_INCREMENTABLES, Texto, longitud_:=3),
                                            Item(CamposFacturaComercial.CA_DESCUENTOS, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                            Item(CamposFacturaComercial.CA_MONEDA_DESCUENTOS, Texto, longitud_:=3)
                    }

                    'Se comenta y conserva para en un futuro si es necesario se separe en secciones
                    ' Partidas - Factura
                    'Case SeccionesFacturaComercial.SFAC5
                    '    Return New List(Of Nodo) From {
                    '                             Item(CamposFacturaComercial.CP_NUMERO_PARTIDA, Entero),
                    '                             Item(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA, Texto, longitud_:=20),
                    '                             Item(CamposFacturaComercial.CA_VALOR_FACTURA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                    '                             Item(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA, Texto, longitud_:=3),
                    '                             Item(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                    '                             Item(CamposFacturaComercial.CA_MONEDA_MERCANCIA_PARTIDA, Texto, longitud_:=3),
                    '                             Item(CamposFacturaComercial.CA_CVE_METODO_VALORACION_PARTIDA, Texto, longitud_:=1),
                    '                             Item(CamposFacturaComercial.CA_PESO_NETO_PARTIDA, Real, cantidadEnteros_:=14, cantidadDecimales_:=3),
                    '                             Item(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=5),
                    '                             Item(CamposFacturaComercial.CA_PAIS_ORIGEN_PARTIDA, Texto, longitud_:=3),
                    '                             Item(CamposFacturaComercial.CP_CANTIDAD_FACTURA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                    '                             Item(CamposFacturaComercial.CP_UNIDAD_MEDIDA_FACTURA_PARTIDA, Entero),
                    '                             Item(CamposFacturaComercial.CA_DESCRIPCION_PARTE_PARTIDA, Texto, longitud_:=250),
                    '                             Item(CamposFacturaComercial.CP_APLICA_DESCRIPCION_COVE_PARTIDA, Booleano),
                    '                             Item(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=3),
                    '                             Item(CamposFacturaComercial.CA_UNIDAD_MEDIDA_COMERCIAL_PARTIDA, Entero),
                    '                             Item(CamposFacturaComercial.CA_DESCRIPCION_COVE_PARTIDA, Texto, longitud_:=250),
                    '                             Item(CamposFacturaComercial.CP_ORDEN_COMPRA_PARTIDA, Texto, longitud_:=60),
                    '                             Item(CamposFacturaComercial.CP_MONEDA_PRECIO_UNITARIO, Texto, longitud_:=3)
                    '    }

                    ' Partida - clasificación
                    'Case SeccionesFacturaComercial.SFAC6

                    '    Return New List(Of Nodo) From {
                    '                             Item(CamposFacturaComercial.CA_FRACCION_ARANCELARIA_PARTIDA, Texto, longitud_:=8),
                    '                             Item(CamposFacturaComercial.CA_CANTIDAD_TARIFA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                    '                             Item(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA, Entero),
                    '                             Item(CamposFacturaComercial.CA_FRACCION_NICO_PARTIDA, Texto, longitud_:=2)
                    '    }

                    ' Partida - detalle mercancía
                    'Case SeccionesFacturaComercial.SFAC7

                    '    Return New List(Of Nodo) From {
                    '                             Item(CamposFacturaComercial.CA_LOTE_PARTIDA, Texto, longitud_:=80),
                    '                             Item(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA, Texto, longitud_:=25),
                    '                             Item(CamposFacturaComercial.CA_MARCA_PARTIDA, Texto, longitud_:=80),
                    '                             Item(CamposFacturaComercial.CA_MODELO_PARTIDA, Texto, longitud_:=80),
                    '                             Item(CamposFacturaComercial.CA_SUBMODELO_PARTIDA, Texto, longitud_:=80),
                    '                             Item(CamposFacturaComercial.CA_KILOMETRAJE_PARTIDA, Entero)
                    '    }

                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function

#End Region

    End Class

End Namespace