Imports gsol.krom
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.RecursosComercioExterior

Namespace Syn.Documento
    <Serializable()>
    Public Class ConstructorViajes
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

#End Region

#Region "Builders"

        Sub New()

            Inicializa(Nothing,
                        TiposDocumentoElectronico.Viajes,
                        True)

        End Sub
        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.Viajes,
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
                         TiposDocumentoElectronico.Viajes)

        End Sub

#End Region

#Region "Methods"

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
        Public Overrides Sub ConstruyeEncabezado()

            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            ' Encabezado principal de viajes
            ConstruyeSeccion(seccionEnum_:=SeccionesViajes.SVIA1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeCuerpo()

            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ConstruyeSeccion(seccionEnum_:=SeccionesViajes.SVIA2,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesViajes.SVIA3,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesViajes.SVIA4,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=True)

        End Sub


#End Region

#Region "Funciones"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_

                'Generales
                Case SeccionesViajes.SVIA1
                    Return New List(Of Nodo) From {
                                             Item(CamposViajes.CP_TIPO_TRANSPORTE, Texto, longitud_:=30),
                                             Item(CamposViajes.CP_TIPO_OPERACION, Booleano),
                                             Item(CamposViajes.CP_NAVE_BUQUE, Texto, longitud_:=30),
                                             Item(CamposViajes.CP_NAVIERA_AEREOLINEA, Texto, longitud_:=30),
                                             Item(CamposViajes.CP_REEXPEDIDORA_FORWARDING, Texto, longitud_:=30),
                                             Item(CamposViajes.CP_FOLIO_CAPITANIA, Texto, longitud_:=30),
                                             Item(CamposViajes.CP_NUMERO_VIAJE, Texto, longitud_:=30)
                              }

                'Datos operativos
                Case SeccionesViajes.SVIA2
                    Return New List(Of Nodo) From {
                                             Item(CamposViajes.CP_PUERTO_EXTRANGERO, Texto, longitud_:=30),
                                             Item(CamposViajes.CP_FECHA_SALIDA_ORIGEN, Fecha),
                                             Item(CamposViajes.CP_FECHA_ETA, Fecha),
                                             Item(CamposViajes.CP_FECHA_ETD, Fecha)
                              }

                'Datos adicionales
                Case SeccionesViajes.SVIA3
                    Return New List(Of Nodo) From {
                                             Item(CamposViajes.CP_FECHA_FONDEO, Fecha),
                                             Item(CamposViajes.CP_FECHA_ATRAQUE, Fecha),
                                             Item(CamposViajes.CP_FECHA_CIERRE_DOCUMENTO, Fecha),
                                             Item(CamposViajes.CP_FECHA_PRESENTACION, Fecha)
                              }
                'Referencias
                Case SeccionesViajes.SVIA4
                    Return New List(Of Nodo) From {
                                             Item(CamposReferencia.CP_REFERENCIA, Texto, longitud_:=16)
                              }

                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function

#End Region

    End Class

End Namespace

