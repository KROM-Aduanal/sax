Imports gsol.krom
Imports MongoDB.Bson
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposProcesamientoElectDocumentos


Namespace Syn.Documento
    <Serializable()>
    Public Class ConstructorProcesamientoElectDocumentos
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

#End Region

#Region "Builders"

        Sub New()

            Inicializa(Nothing,
                        TiposDocumentoElectronico.ProcesamientoElectronicoDocumento,
                        True)

        End Sub
        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.ProcesamientoElectronicoDocumento,
                       construir_)

        End Sub
        'Public Sub New(ByVal folioDocumento_ As String,
        '               ByVal referencia_ As String,
        '               ByVal idCliente_ As Int32,
        '               ByVal nombreCliente_ As String
        '               )

        '    Inicializa(folioDocumento_,
        '                 referencia_,
        '                 idCliente_,
        '                 nombreCliente_,
        '                 TiposDocumentoElectronico.ProveedoresOperativos)

        'End Sub

        'NEW
        Public Sub New(ByVal folioDocumento_ As String,
                       ByVal referencia_ As String,
                       ByVal tipoPropietario_ As String,
                       ByVal nombrePropietario_ As String,
                       ByVal idPropietario_ As Int32,
                       ByVal objectIdPropietario_ As ObjectId,
                       ByVal metadatos_ As List(Of CampoGenerico)
                      )

            Inicializa(folioDocumento_,
                         referencia_,
                         tipoPropietario_,
                         nombrePropietario_,
                         idPropietario_,
                         objectIdPropietario_,
                         metadatos_,
                         TiposDocumentoElectronico.ProcesamientoElectronicoDocumento)

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

            ' Encabezado principal de la referencia
            ConstruyeSeccion(seccionEnum_:=SeccionesProcesamientoElectDocumentos.SPED1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeCuerpo()

            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ConstruyeSeccion(seccionEnum_:=SeccionesProcesamientoElectDocumentos.SPED2,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesProcesamientoElectDocumentos.SPED3,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesProcesamientoElectDocumentos.SPED4,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=False)
        End Sub

#End Region

#Region "Funciones"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)
            'NEW
            Select Case idSeccion_
                'Generales
                Case SeccionesProcesamientoElectDocumentos.SPED1
                    Return New List(Of Nodo) From {
                                                    Item(CP_PREREFERENCIA_DOCUMENTO_PROCESADO, Texto, useAsMetadata_:=True),
                                                    Item(CP_RAZON_SOCIAL_CLIENTE, Texto),
                                                    Item(CP_CLAVE_CLIENTE, Texto),
                                                    Item(CP_OBJECTID_CLIENTE, Texto),
                                                    Item(CP_TIPO_OPERACION),
                                                    Item(CP_ESTADO_DOCUMENTO_PROCESADO)
                                                }
                'Detalle proveedor
                Case SeccionesProvedorOperativo.SPRO2
                    Return New List(Of Nodo) From {
                                                    Item(CP_TIPO_DOCUMENTO_PROCESADO, Texto),
                                                    Item(CP_TIPO_PROCESAMIENTO, Entero)
                                                }

                'Domicilios físcales
                Case SeccionesProvedorOperativo.SPRO3
                    Return New List(Of Nodo) From {
                                                    Item(CP_OBJECTID_DOCUMENTO_PROCESADO, Texto),
                                                    Item(CP_CLAVE_DOCUMENTO_PROCESADO, Texto),
                                                    Item(CP_DETALLE_DOCUMENTO_PROCESADO, Texto),
                                                    Item(CP_TIPO_DETALLE_DOCUMENTO_PROCESADO, Texto),
                                                    Item(CP_ESTADO_DETALLE_DOCUMENTO_PROCESADO, Texto),
                                                    Item(CP_DOCUMENTO_PROCESADO, Texto),
                                                    Item(CP_DOCUMENTO_ELECTRONICO_GENERADO, Texto)
                                                }

                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function

#End Region

    End Class

End Namespace