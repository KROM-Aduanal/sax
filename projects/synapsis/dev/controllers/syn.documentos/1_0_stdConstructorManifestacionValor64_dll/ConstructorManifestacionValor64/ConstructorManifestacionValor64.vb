
Imports gsol.krom
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior

Namespace Syn.Documento

    <Serializable()>
    Public Class ConstructorManifestacionValor
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

        'Aquí debes cambiar el tipo de documento, según el que usaras, parámetro que requiere el constructor.
        'Por ejemplo: TiposDocumentoElectronico.FacturaComercial
        Private _tipoDocumento As TiposDocumentoElectronico = TiposDocumentoElectronico.ManifestacionValor

#End Region

#Region "Builders"

        Sub New()

            Inicializa(Nothing,
                   _tipoDocumento,
                    True)

        End Sub

        Sub New(ByVal construir_ As Boolean,
            Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                   _tipoDocumento,
                   construir_)

        End Sub

        Public Sub New(ByVal folioDocumento_ As String,
                   ByVal folioOperacion_ As String,
                   ByVal propietario_ As Int32,
                   ByVal nombrePropietario_ As String
                   )

            Inicializa(folioDocumento_,
                   folioOperacion_,
                   propietario_,
                   nombrePropietario_,
                   _tipoDocumento)

        End Sub

#End Region

#Region "Methods"

        Public Overrides Sub ConstruyeEncabezado()

            ' Encabezado principal de la manifestación de valor
            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            ' Construye las secciones 

            ConstruyeSeccion(seccionEnum_:=SeccionesManifestacionValor.SMV1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)
            ConstruyeSeccion(seccionEnum_:=SeccionesManifestacionValor.SMV2,
                           tipoBloque_:=TiposBloque.Encabezado,
                           conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesManifestacionValor.SMV3,
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

            'Construir la parte de cuerpo de la manifestación de valor
            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ' Construye las secciones 

            ConstruyeSeccion(seccionEnum_:=SeccionesManifestacionValor.SMV4,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesManifestacionValor.SMV5,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesManifestacionValor.SMV6,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesManifestacionValor.SMV7,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesManifestacionValor.SMV8,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesManifestacionValor.SMV9,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesManifestacionValor.SMV10,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesManifestacionValor.SMV11,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=True)

        End Sub

        Public Overrides Sub ConstruyePiePagina()

            'Construir la parte pie de página
            '_estructuraDocumento(TiposBloque.PiePagina) = New List(Of Nodo)

            'Construir una sección
            'ConstruyeSeccion(seccionDocumento_:=SeccionesGenericas.SGS1,
            '                 tipoBloque_:=TiposBloque.PiePagina,
            '                 conCampos_:=True)

        End Sub

        'Será usado solo cuando aplique Followers and Friends (suscripciones).
        Public Sub ConfiguracionNotificaciones()

            'SubscriptionsGroup =
            '   New List(Of subscriptionsgroup) _
            '      From {
            '             New subscriptionsgroup With
            '             {
            '              .active = True,
            '              .toresource = "Vt002EjecutivosMiEmpresa",
            '              ._foreignkeyname = "_id",
            '              ._foreignkey = New ObjectId,
            '              .subscriptions =
            '                New subscriptions With
            '                  {
            '                    .namespaces = New List(Of [namespace]) From
            '                    {
            '                     nsp(1, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente"),
            '                     nsp(2, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.$[].Nodos.$[].Nodos.$[].Nodos.$[elem]")
            '                    },
            '                   .fields = New List(Of fieldInfo) From
            '                    {
            '                    field(CamposClientes.CP_CVE_EMPRESA, nsp:=2, attr:="Valor"),
            '                    field(CamposClientes.CA_RFC_CLIENTE, nsp:=1, attr:="Valor"),
            '                    field(CamposClientes.CA_CURP_CLIENTE, nsp:=1, attr:="Valor")
            '                    }
            '                 }
            '             },
            '             New subscriptionsgroup With
            '             {
            '              .active = True,
            '              .toresource = "[SynapsisN].[dbo].[Vt022AduanaSeccionA01]",
            '              ._foreignkeyname = "_id",
            '              ._foreignkey = New ObjectId,
            '              .subscriptions =
            '                New subscriptions With
            '                  {
            '                    .namespaces = New List(Of [namespace]) From
            '                    {
            '                     nsp(1, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente"),
            '                     nsp(2, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.$[].Nodos.$[].Nodos.$[].Nodos.$[elem]")
            '                    },
            '                   .fields = New List(Of fieldInfo) From
            '                   {
            '                    field(CamposClientes.CP_CVE_ADUANA_SECCION, nsp:=2, attr:="Valor")
            '                   }
            '                }
            '             }
            '           }

        End Sub

#End Region

#Region "Functions"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_

                ' Generales
                Case SeccionesManifestacionValor.SMV1
                    Return New List(Of Nodo) From {
                                             Item(CamposManifestacionValor.CA_PERIODICIDAD, Texto, longitud_:=40),
                                             Item(CamposManifestacionValor.CA_REPRESENTANTE_LEGAL, Texto, longitud_:=40),
                                             Item(CamposManifestacionValor.CA_FECHA_MANIFESTACION, Fecha)
}

                ' Datos del proveedor
                Case SeccionesManifestacionValor.SMV2
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
                                             Item(CamposFacturaComercial.CP_CVE_METODO_VALORACION, Entero)
}

                ' Datos del importador
                Case SeccionesManifestacionValor.SMV3
                    Return New List(Of Nodo) From {
                                             Item(CamposClientes.CA_RAZON_SOCIAL, Texto, longitud_:=120),
                                             Item(CamposClientes.CA_TAX_ID, Texto, longitud_:=11),
                                             Item(CamposClientes.CA_RFC_CLIENTE, Texto, longitud_:=13),
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

                ' Facturas
                Case SeccionesManifestacionValor.SMV4
                    Return New List(Of Nodo) From {
                                             Item(CamposFacturaComercial.CA_NUMERO_FACTURA, Texto, longitud_:=40),
                                             Item(CamposFacturaComercial.CA_FECHA_FACTURA, Fecha),
                                             Item(CamposFacturaComercial.CA_CVE_PAIS_FACTURACION, Texto, longitud_:=3),
                                             Item(CamposFacturaComercial.CA_PAIS_FACTURACION, Texto, longitud_:=80),
                                             Item(CamposPedimento.CA_NUM_PEDIMENTO_COMPLETO, Texto, longitud_:=80),
                                             Item(CamposPedimento.CA_FECHA_PEDIM_ORIGINAL, Fecha),
                                             Item(CamposGlobales.CP_IDENTITY, Entero)
}

                ' Incrementables
                Case SeccionesManifestacionValor.SMV5

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

                ' Anexos
                Case SeccionesManifestacionValor.SMV6

                    Return New List(Of Nodo) From {
                                            Item(CamposManifestacionValor.CA_ANEXA_DOC_66, Booleano),
                                            Item(SeccionesManifestacionValor.SMV8, False),
                                            Item(SeccionesManifestacionValor.SMV9, False),
                                            Item(CamposManifestacionValor.CA_PRECIO_COMPRENDE_ART_65, Booleano),
                                            Item(CamposManifestacionValor.CA_ACOMPANA_FACT, Booleano),
                                            Item(CamposManifestacionValor.CA_ANEXA_DOC_65, Booleano),
                                            Item(SeccionesManifestacionValor.SMV10, False),
                                            Item(SeccionesManifestacionValor.SMV11, False)
}

                ' Valor de tansacción
                Case SeccionesManifestacionValor.SMV7

                    Return New List(Of Nodo) From {
                                            Item(CamposManifestacionValor.CA_VALOR_PAGADO, Real, cantidadEnteros_:=10, cantidadDecimales_:=2),
                                            Item(CamposManifestacionValor.CA_CONCEPTOS_NO_VALOR, Texto, longitud_:=3)
}

                ' Anexa Doc art 66
                Case SeccionesManifestacionValor.SMV8

                    Return New List(Of Nodo) From {
                                            Item(CamposManifestacionValor.CA_NUM_ANEXO, Entero),
                                            Item(CamposManifestacionValor.CP_FACTURA_DOCUMENTO_66, Texto, longitud_:=80),
                                            Item(CamposGlobales.CP_IDENTITY, Entero)
}

                ' No anexa Doc art 66
                Case SeccionesManifestacionValor.SMV9

                    Return New List(Of Nodo) From {
                                            Item(CamposManifestacionValor.CA_MERCANCIA_66, Texto, longitud_:=40),
                                            Item(CamposManifestacionValor.CA_FAC_DOC_COMERCIAL_66, Texto, longitud_:=20),
                                            Item(CamposManifestacionValor.CA_IMPORTE_MONEDA_66, Texto, longitud_:=15),
                                            Item(CamposManifestacionValor.CA_CONCEPTO_CARGO_66, Texto, longitud_:=40),
                                            Item(CamposGlobales.CP_IDENTITY, Entero)
}

                ' Anexa Doc art 65
                Case SeccionesManifestacionValor.SMV10

                    Return New List(Of Nodo) From {
                                            Item(CamposManifestacionValor.CA_NUM_ANEXO_65, Entero),
                                            Item(CamposManifestacionValor.CP_FACTURA_DOCUMENTO_65, Texto, longitud_:=80),
                                            Item(CamposGlobales.CP_IDENTITY, Entero)
}

                ' No anexa Doc art 65
                Case SeccionesManifestacionValor.SMV11

                    Return New List(Of Nodo) From {
                                            Item(CamposManifestacionValor.CA_MERCANCIA_65, Texto, longitud_:=40),
                                            Item(CamposManifestacionValor.CA_FAC_DOC_COMERCIAL_65, Texto, longitud_:=20),
                                            Item(CamposManifestacionValor.CA_IMPORTE_MONEDA_65, Texto, longitud_:=15),
                                            Item(CamposManifestacionValor.CA_CONCEPTO_CARGO_65, Texto, longitud_:=40),
                                            Item(CamposGlobales.CP_IDENTITY, Entero)
}

                Case Else

                    _tagWatcher.SetError(Me,
                        "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function

#End Region

    End Class

End Namespace