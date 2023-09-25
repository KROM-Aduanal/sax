
Imports gsol.krom
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports MongoDB.Driver.Core.Configuration
Imports System.Collections.ObjectModel
Imports System.Web.UI
Imports System.Text.RegularExpressions
Imports MongoDB.Bson.Serialization.Attributes

Namespace Syn.Documento
    <Serializable()>
    Public Class ConstructorAcuseValor
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

#End Region

#Region "Builders"

        Sub New()

            Inicializa(Nothing,
                        TiposDocumentoElectronico.AcuseValor,
                        True)

        End Sub
        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.AcuseValor,
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
                         TiposDocumentoElectronico.AcuseValor)

        End Sub

#End Region

#Region "Methods"
        Public Overrides Sub ConstruyeEncabezado()

            ' Encabezado principal del COIVE
            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            ' Construye las secciones 

            ConstruyeSeccion(seccionEnum_:=SeccionesAcuseValor.SAcuseValor1,
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

            'Construir la parte de cuerpo del COVEE
            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ' Construye las secciones 

            ConstruyeSeccion(seccionEnum_:=SeccionesAcuseValor.SAcuseValor2,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesAcuseValor.SAcuseValor3,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesAcuseValor.SAcuseValor4,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesAcuseValor.SAcuseValor5,
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

#End Region

#Region "Functions"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_

                ' Generales
                Case SeccionesAcuseValor.SAcuseValor1
                    Return New List(Of Nodo) From {
                                             Item(CamposAcuseValor.CP_ID_FACTURA_ACUSEVALOR, IdObject),
                                             Item(CamposFacturaComercial.CA_NUMERO_FACTURA, Texto, longitud_:=40),
                                             Item(CamposAcuseValor.CP_NUMERO_SYSTEM_ACUSEVALOR, Texto, longitud_:=40),
                                             Item(CamposAcuseValor.CA_NUMERO_ACUSEVALOR, Texto, longitud_:=40),
                                             Item(CamposFacturaComercial.CP_TIPO_OPERACION, Texto, longitud_:=14),
                                             Item(CamposAcuseValor.CP_TIPO_DOCUMENTO_ACUSEVALOR, Texto, longitud_:=40),
                                             Item(CamposFacturaComercial.CA_MONEDA_FACTURACION, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CA_FECHA_FACTURA, Fecha),
                                             Item(CamposAcuseValor.CA_FECHA_ACUSEVALOR, Fecha),
                                             Item(CamposFacturaComercial.CA_APLICA_SUBDIVISION, Texto, longitud_:=3),
                                             Item(CamposAcuseValor.CA_RELACION_FACTURA_ACUSEVALOR, Texto, longitud_:=3),
                                             Item(CamposFacturaComercial.CA_APLICA_CERTIFICADO, Texto, longitud_:=3),
                                             Item(CamposAcuseValor.CA_NUMERO_EXPORTADOR_ACUSEVALOR, Texto, longitud_:=100),
                                             Item(CamposAcuseValor.CA_OBSERVACIONES_ACUSEVALOR, Texto, longitud_:=450)
                                             }

                ' Datos del proveedor
                Case SeccionesAcuseValor.SAcuseValor2
                    Return New List(Of Nodo) From {
                                             Item(CamposAcuseValor.CP_ID_Proveedor_ACUSEVALOR, IdObject),
                                             Item(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR, Texto, longitud_:=15),
                                             Item(CamposProveedorOperativo.CA_RFC_PROVEEDOR, Texto, longitud_:=13),
                                             Item(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, Texto, longitud_:=120),
                                             Item(CamposProveedorOperativo.CA_CURP_PROVEEDOR, Texto, longitud_:=120),
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
                                             Item(CamposDomicilio.CA_PAIS, Texto, longitud_:=80)
                    }

                ' Datos del destinatario
                Case SeccionesAcuseValor.SAcuseValor3
                    Return New List(Of Nodo) From {
                                             Item(CamposAcuseValor.CP_ID_Destinatario_ACUSEVALOR, IdObject),
                                             Item(CamposDestinatario.CA_TAX_ID, Texto, longitud_:=11),
                                             Item(CamposDestinatario.CA_RFC_DESTINATARIO, Texto, longitud_:=13),
                                             Item(CamposDestinatario.CA_RAZON_SOCIAL, Texto, longitud_:=120),
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
                                             Item(CamposDomicilio.CA_PAIS, Texto, longitud_:=80)
                    }

                ' Partidas
                Case SeccionesAcuseValor.SAcuseValor4
                    Return New List(Of Nodo) From {
                                             Item(CamposFacturaComercial.CP_NUMERO_PARTIDA, Entero),
                                             Item(CamposFacturaComercial.CA_NUMERO_PARTE_PARTIDA, Texto, longitud_:=20),
                                             Item(CamposAcuseValor.CA_DESCRIPCION_PARTIDA_ACUSEVALOR, Texto, longitud_:=250),
                                             Item(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=5),
                                             Item(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                             Item(CamposAcuseValor.CA_UNIDAD_MEDIDA_FACTURA_PARTIDA_ACUSEVALOR, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                             Item(CamposAcuseValor.CA_VALOR_MERCANCIA_PARTIDA_DOLARES_ACUSEVALOR, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                             Item(CamposFacturaComercial.CA_MARCA_PARTIDA, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CA_MODELO_PARTIDA, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CA_SUBMODELO_PARTIDA, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA, Texto, longitud_:=80),
                                             Item(CamposGlobales.CP_IDENTITY, Entero)
                    }

                'DetallesPartida
                'Configuración
                Case SeccionesAcuseValor.SAcuseValor5

                    Return New List(Of Nodo) From {
                                            Item(CamposAcuseValor.CA_SELLO_ACUSEVALOR, Texto, longitud_:=255),
                                            Item(CamposAcuseValor.CA_PATENTE_ACUSEVALOR, Texto, longitud_:=5),
                                            Item(CamposAcuseValor.CA_RFC_CONSULTA_ACUSEVALOR, Texto, longitud_:=15),
                                            Item(CamposAcuseValor.CP_EMAIL_CONSULTA_ACUSEVALOR, Texto, longitud_:=15)
                    }



                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function


#End Region

    End Class




End Namespace