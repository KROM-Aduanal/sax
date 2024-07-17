Imports gsol.krom
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports MongoDB.Bson
Imports Wma.Exceptions

Namespace Syn.Documento
    <Serializable()>
    Public Class ConstructorReferencia
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

#End Region

#Region "Builders"

        Sub New()

            Inicializa(Nothing,
                        TiposDocumentoElectronico.Referencia,
                        True)

        End Sub

        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.Referencia,
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
                         TiposDocumentoElectronico.Referencia)

        End Sub

#End Region

#Region "Methods"

        Public Sub ConfiguracionDocumentosAsociados()

            DocumentosAsociados =
                New List(Of DocumentoAsociado) _
                From {
                    New DocumentoAsociado With {
                    .idcoleccion = "Glo007Clientes",
                    .identificadorrecurso = "ConstructorCliente",
                    .idcampo = CamposClientes.CA_RAZON_SOCIAL,
                    .idsection = SeccionesReferencias.SREF2
                    }
                }

            'DocumentosAsociados =
            '    New List(Of DocumentoAsociado) _
            '    From {
            '        New DocumentoAsociado With {
            '        .idcoleccion = "Glo007Clientes",
            '        .identificadorrecurso = "ConstructorCliente",
            '        .idcampo = CamposReferencia.CP_ID_IOE
            '        }
            '    }

        End Sub

        Public Sub ConfiguracionNotificaciones()

            subscriptionsgroup =
               New List(Of subscriptionsgroup) _
                  From {
                         New subscriptionsgroup With
                         {
                          .active = True,
                          .toresource = "ConstructorCliente",
                          ._foreignkeyname = "_id",
                          ._foreignkey = CamposClientes.CP_OBJECTID_CLIENTE,
                          .subscriptions =
                            New subscriptions With
                            {
                                .namespaces = New List(Of [namespace]) From
                                {
                                 nsp(1, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Cuerpo.1.Nodos.0.Nodos.2.Nodos.0")
                                },
                               .fields = New List(Of fieldInfo) From
                                {
                                 field(CamposClientes.CA_RFC_CLIENTE, nsp:=1, attr:="Valor")
                                }
                             }
                         }
                       }

        End Sub

        ',
        '                 New subscriptionsgroup With
        '                 {
        '                  .active = True,
        '                  .toresource = "[SynapsisN].[dbo].[Vt022AduanaSeccionA01]",
        '                  ._foreignkeyname = "i_Cve_AduanaSeccion",
        '                  ._foreignkey = CamposReferencia.CP_ADUANA_DESPACHO,
        '                  .subscriptions =
        '                    New subscriptions With
        '                    {
        '                        .namespaces = New List(Of [namespace]) From
        '                        {
        '                         nsp(1, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Cuerpo.0.Nodos.0.Nodos.13.Nodos.0")
        '                        },
        '                       .fields = New List(Of fieldInfo) From
        '                       {
        '                        field(CamposReferencia.CP_ADUANA_DESPACHO, nsp:=1, attr:="Valor")
        '                       }
        '                    }
        '                 }

        Public Overrides Sub ConstruyeEncabezado()

            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            ConstruyeSeccion(seccionEnum_:=SeccionesReferencias.SREF1,
                tipoBloque_:=TiposBloque.Encabezado,
                conCampos_:=True)

            ConfiguracionDocumentosAsociados()

        End Sub
        Public Overrides Sub ConstruyeCuerpo()

            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ConstruyeSeccion(seccionEnum_:=SeccionesReferencias.SREF2,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesReferencias.SREF3,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesReferencias.SREF4,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesReferencias.SREF5,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesReferencias.SREF6,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=False)

        End Sub

#End Region

#Region "Funciones"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            '_estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            Select Case idSeccion_

                'Generales'
                Case SeccionesReferencias.SREF1
                    Return New List(Of Nodo) From {
                                            Item(CamposReferencia.CP_REFERENCIA, Texto, longitud_:=16),
                                            Item(CamposPedimento.CA_NUMERO_PEDIMENTO_COMPLETO, Texto, longitud_:=21),
                                            Item(CamposPedimento.CA_TIPO_OPERACION, Entero, longitud_:=1),
                                            Item(CamposReferencia.CP_MATERIAL_PELIGROSO, Booleano),
                                            Item(CamposReferencia.CP_RECTIFICACION, Booleano),
                                            Item(CamposPedimento.CA_REGIMEN, Texto, longitud_:=3),
                                            Item(CamposPedimento.CA_CVE_PEDIMENTO, Texto, longitud_:=2),
                                            Item(CamposPedimento.CP_MODALIDAD_ADUANA_PATENTE, Entero, longitud_:=3),
                                            Item(CamposPedimento.CA_ADUANA_ENTRADA_SALIDA, Texto, longitud_:=3),
                                            Item(CamposPedimento.CA_PATENTE, Texto, longitud_:=4),
                                            Item(CamposReferencia.CA_TIPO_PEDIMENTO, Texto, longitud_:=100),
                                            Item(CamposReferencia.CP_DESADUANAMIENTO, Entero, longitud_:=5),
                                            Item(CamposPedimento.CP_EJECUTIVO_CUENTA, Entero, longitud_:=5),
                                            Item(CamposReferencia.CP_DESCRIPCION_MERCANCIA_COMPLETA, Texto, longitud_:=150),
                                            Item(CamposReferencia.CP_TIPO_CARGA_AGENCIA, Entero, longitud_:=3),
                                            Item(CamposReferencia.CP_TIPO_DESPACHO, Entero, longitud_:=3),
                                            Item(CamposReferencia.CP_PEDIMENTO_ORIGINAL, Texto, longitud_:=21)
                            }

                    'Cliente'
                Case SeccionesReferencias.SREF2

                    Return New List(Of Nodo) From {
                                             Item(CamposClientes.CP_OBJECTID_CLIENTE, IdObject),
                                             Item(CamposClientes.CA_RAZON_SOCIAL, Texto, longitud_:=120, useAsMetadata_:=True),
                                             Item(CamposClientes.CA_RFC_CLIENTE, Texto, longitud_:=13),
                                             Item(CamposClientes.CP_RFC_FACTURACION, Texto, longitud_:=13),
                                             Item(CamposClientes.CP_CVE_BANCO, Texto, longitud_:=200)
                              }

                    'Tracking'
                Case SeccionesReferencias.SREF3

                    Return New List(Of Nodo) From {
                                             Item(CamposReferencia.CP_FECHA_APERTURA, Fecha),
                                             Item(CamposPedimento.CA_FECHA_ENTRADA, Fecha),
                                             Item(CamposReferencia.CP_FECHA_PROFORMA, Fecha),
                                             Item(CamposReferencia.CP_FECHA_CIERRE, Fecha),
                                             Item(CamposReferencia.CP_FECHA_PAGO, Fecha),
                                             Item(CamposReferencia.CP_FECHA_ULTIMO_DESPACHO, Fecha)
                              }


                    'Fechas'
                Case SeccionesReferencias.SREF4

                    Return New List(Of Nodo) From {
                                            Item(CamposReferencia.CP_FECHA_PRESENTACION, Fecha),
                                            Item(CamposReferencia.CP_FECHA_SALIDA, Fecha),
                                            Item(CamposReferencia.CP_FECHA_PREVIO, Fecha),
                                            Item(CamposReferencia.CP_FECHA_ETD, Fecha),
                                            Item(CamposReferencia.CP_FECHA_CIERRE_FISICO, Fecha),
                                            Item(CamposReferencia.CP_FECHA_ETA, Fecha),
                                            Item(CamposReferencia.CP_FECHA_REVALIDACION, Fecha)
                              }

                    'Guias'
                Case SeccionesReferencias.SREF5

                    Return New List(Of Nodo) From {
                                            Item(CamposReferencia.CP_RECINTO_FISCAL, Entero, longitud_:=3),
                                            Item(CamposReferencia.CP_GUIA_MULTIPLE, Booleano),
                                            Item(SeccionesReferencias.SREF7, conCampos_:=False),
                                            Item(SeccionesReferencias.SREF8, conCampos_:=True)
                              }
                    'Documentos'
                Case SeccionesReferencias.SREF6

                    Return New List(Of Nodo) From {
                                            Item(CamposReferencia.CP_NOMBRE_DOCUMENTO, IdObject),
                                            Item(CamposReferencia.CP_TIPO_DOCUMENTO, Texto, longitud_:=100)
                    }
                      'Detalle guías simple'
                Case SeccionesReferencias.SREF8

                    Return New List(Of Nodo) From {
                                            Item(CamposReferencia.CP_NUMERO_GUIA, Texto, longitud_:=30),
                                            Item(CamposReferencia.CP_TRANSPORTISTA, Texto, longitud_:=100),
                                            Item(CamposReferencia.CP_PAIS, Texto, longitud_:=100),
                                            Item(CamposReferencia.CP_TIPO_CARGA, Texto, longitud_:=100),
                                            Item(CamposReferencia.CP_PESOBRUTO, Entero, longitud_:=10),
                                            Item(CamposReferencia.CP_UNIDADMEDIDA, Texto, longitud_:=50),
                                            Item(CamposReferencia.CP_TIPO_GUIA, Entero, longitud_:=3),
                                            Item(CamposReferencia.CP_FECHA_SALIDA_ORIGEN, Fecha),
                                            Item(CamposReferencia.CP_DESCRIPCION_MERCANCIA, Texto, longitud_:=150),
                                            Item(CamposReferencia.CP_CONSIGNATARIO, Texto, longitud_:=100)
                              }
                    'Detalle guías multi'
                Case SeccionesReferencias.SREF7

                    Return New List(Of Nodo) From {
                                            Item(CamposReferencia.CP_NUMERO_GUIA_MULTIPLE, Texto, longitud_:=30),
                                            Item(CamposReferencia.CP_TRANSPORTISTA_MULTIPLE, Texto, longitud_:=100),
                                            Item(CamposReferencia.CP_PAIS_MULTIPLE, Texto, longitud_:=100),
                                            Item(CamposReferencia.CP_TIPO_CARGA_MULTIPLE, Texto, longitud_:=100),
                                            Item(CamposReferencia.CP_PESOBRUTO_MULTIPLE, Entero, longitud_:=10),
                                            Item(CamposReferencia.CP_UNIDADMEDIDA_MULTIPLE, Texto, longitud_:=50),
                                            Item(CamposReferencia.CP_TIPO_GUIA_MULTIPLE, Entero, longitud_:=3),
                                            Item(CamposReferencia.CP_FECHA_SALIDA_ORIGEN_MULTIPLE, Fecha),
                                            Item(CamposReferencia.CP_DESCRIPCION_MERCANCIA_MULTIPLE, Texto, longitud_:=150),
                                            Item(CamposReferencia.CP_CONSIGNATARIO_MULTIPLE, Texto, longitud_:=100)
                              }
                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Throw New NotImplementedException()
        End Function

#End Region

    End Class

End Namespace
