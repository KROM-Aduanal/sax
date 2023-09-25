Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports gsol.krom
Imports MongoDB.Bson
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Operaciones
Imports Wma.Exceptions

Namespace Syn.Documento
    <Serializable()>
    Public Class ConstructorRevalidacion
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

#End Region

#Region "Builders"

        Sub New()

            Inicializa(Nothing,
                        TiposDocumentoElectronico.Revalidacion,
                        True)

        End Sub
        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.Revalidacion,
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
                         TiposDocumentoElectronico.Revalidacion)

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

            ' Encabezado principal de la revalidacion
            ConstruyeSeccion(seccionEnum_:=SeccionesRevalidacion.SREV1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeCuerpo()

            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ConstruyeSeccion(seccionEnum_:=SeccionesRevalidacion.SREV2,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesRevalidacion.SREV3,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesRevalidacion.SREV4,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=False)

        End Sub


#End Region

#Region "Funciones"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_

                'Generales
                Case SeccionesRevalidacion.SREV1
                    Return New List(Of Nodo) From {
                                              Item(CamposReferencia.CP_REFERENCIA, Texto, longitud_:=16),
                                              Item(CamposClientes.CA_RAZON_SOCIAL, Texto, longitud_:=120)
                              }

                'Datos revalidacion
                Case SeccionesRevalidacion.SREV2
                    Return New List(Of Nodo) From {
                                              Item(CamposRevalidacion.CP_NO_GUIA_MASTER, Texto, longitud_:=30),
                                              Item(CamposRevalidacion.CP_REVALIDADO, Booleano),
                                              Item(CamposRevalidacion.CP_FECHA_REVALIDACION, Fecha),
                                              Item(CamposRevalidacion.CP_TIPO_CARGA, Texto, longitud_:=30),
                                              Item(CamposRevalidacion.CP_ID_BLREVALIDADO, Texto, longitud_:=30)
                              }

                'Cargas
                Case SeccionesRevalidacion.SREV3
                    Return New List(Of Nodo) From {
                                              Item(CamposRevalidacion.CP_CLASE_CARGA, Texto, longitud_:=30),
                                              Item(CamposRevalidacion.CP_CANTIDAD_CARGA, Entero),
                                              Item(CamposRevalidacion.CP_PESO_CARGA, Entero)
                              }


                'Contenedores
                Case SeccionesRevalidacion.SREV4
                    Return New List(Of Nodo) From {
                                              Item(CamposRevalidacion.CP_CONTENEDOR, Texto, longitud_:=30),
                                              Item(CamposRevalidacion.CP_TAMANO_CONTENEDOR, Texto),
                                              Item(CamposRevalidacion.CP_PESO_CONTENEDOR, Entero)
                              }

                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function



#End Region

    End Class

End Namespace

