Imports gsol.krom
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports MongoDB.Bson

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

        Public Sub ConfiguracionNotificaciones()

            SubscriptionsGroup =
               New List(Of subscriptionsgroup) _
                  From {
                         New subscriptionsgroup With
                         {
                          .active = True,
                          .toresource = "ConstructorCliente",
                          ._foreignkeyname = "_id",
                          ._foreignkey = CamposReferencia.CP_ID_IOE,
                          .subscriptions =
                            New subscriptions With
                            {
                                .namespaces = New List(Of [namespace]) From
                                {
                                 nsp(1, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Cuerpo.1.Nodos.0.Nodos.2.Nodos.0")
                                },
                               .fields = New List(Of fieldInfo) From
                                {
                                field(CamposReferencia.CA_RFC_DEL_IOE, nsp:=1, attr:="Valor")
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

            '_estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            '' Encabezado principal de la referencia
            'ConstruyeSeccion(seccionEnum_:=SeccionesReferencias.SREF1,
            '                 tipoBloque_:=TiposBloque.Encabezado,
            '                 conCampos_:=True)
            ConfiguracionNotificaciones()

        End Sub
        Public Overrides Sub ConstruyeCuerpo()

            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ConstruyeSeccion(seccionEnum_:=SeccionesReferencias.SREF1,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=True)

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

        End Sub

#End Region

#Region "Funciones"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            '_estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            Select Case idSeccion_

                    'Generales
                Case SeccionesReferencias.SREF1 'Prefijo, fecha de precio
                    Return New List(Of Nodo) From {
                                            Item(CamposReferencia.CP_REFERENCIA, Texto, longitud_:=16),
                                            Item(CamposReferencia.CP_PEDIMENTO, Texto, longitud_:=16),
                                            Item(CamposReferencia.CP_ORIGINAL, Texto, longitud_:=16),
                                            Item(CamposReferencia.CP_TIPO_OPERACION, Booleano),
                                            Item(CamposReferencia.CP_TIPO_REFERENCIA, Entero),
                                            Item(CamposReferencia.CP_MATERIAL_PELIROSO, Booleano),
                                            Item(CamposReferencia.CP_RECTIFICACION, Booleano),
                                            Item(CamposReferencia.CP_TIPO_CARGA, Texto, longitud_:=100),
                                            Item(CamposReferencia.CP_MODALIDAD_ADUANA_PATENTE2, Texto, longitud_:=100),
                                            Item(CamposReferencia.CP_REGIMEN, Texto, longitud_:=100),
                                            Item(CamposReferencia.CA_TIPO_DOCUMENTO, Texto, longitud_:=100),
                                            Item(CamposReferencia.CP_CLAVE_DOCUMENTO, Texto, longitud_:=100),
                                            Item(CamposReferencia.CP_ADUANA_ENTRADA_SALIDA, Texto, longitud_:=200),
                                            Item(CamposReferencia.CP_ADUANA_DESPACHO, Texto, longitud_:=200),
                                            Item(CamposReferencia.CP_DESTINO_MERCANCIA, Texto, longitud_:=200),
                                            Item(CamposReferencia.CP_EJECUTIVO_CUENTA, Texto, longitud_:=100)
                            }

                    'Cliente
                Case SeccionesReferencias.SREF2

                    Return New List(Of Nodo) From {
                                             Item(CamposReferencia.CP_ID_IOE, IdObject),
                                             Item(CamposReferencia.CA_RAZON_SOCIAL_IOE, Texto, longitud_:=250),
                                             Item(CamposReferencia.CA_RFC_DEL_IOE, Texto, longitud_:=13),
                                             Item(CamposReferencia.CA_CURP_DEL_IOE, Texto, longitud_:=25),
                                             Item(CamposReferencia.CP_RFC_FACTURACION_IOE, Texto, longitud_:=13),
                                             Item(CamposReferencia.CA_BANCO_PAGO_IOE, Texto, longitud_:=200),
                                             Item(SeccionesReferencias.SREF3, False)
                              }

                    'Detalles Cliente
                Case SeccionesReferencias.SREF3

                    Return New List(Of Nodo) From {
                                             Item(CamposReferencia.CP_DESCRIPCION_DETALLE, Texto, longitud_:=250),
                                             Item(CamposReferencia.CP_TIPO_DETALLE, Texto, longitud_:=250)
                              }


                    'Seguimiento
                Case SeccionesReferencias.SREF4

                    Return New List(Of Nodo) From {
                                             Item(CamposReferencia.CP_FECHA_ETA, Fecha),
                                             Item(CamposReferencia.CP_ES_ENTRADA, Entero, longitud_:=2),
                                             Item(CamposReferencia.CP_ES_PAGO_ANTICIPADO, Entero, longitud_:=2)
                              }

                    'Datos Adicionales
                Case SeccionesReferencias.SREF5

                    Return New List(Of Nodo) From {
                                            Item(CamposReferencia.CP_FECHA_RECEPCION_DOCUMENTOS, Fecha)
                              }

                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function

#End Region

    End Class

End Namespace
