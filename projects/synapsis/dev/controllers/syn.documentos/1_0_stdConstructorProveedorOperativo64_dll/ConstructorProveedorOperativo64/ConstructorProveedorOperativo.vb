Imports gsol.krom
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposProveedorOperativo

'Public Class ConstructorProveedoresOperativos
'End Class

Namespace Syn.Documento
    <Serializable()>
    Public Class ConstructorProveedoresOperativos
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

#End Region

#Region "Builders"

        Sub New()

            Inicializa(Nothing,
                        TiposDocumentoElectronico.ProveedoresOperativos,
                        True)

        End Sub
        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.ProveedoresOperativos,
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
                         TiposDocumentoElectronico.ProveedoresOperativos)

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
            ConstruyeSeccion(seccionEnum_:=SeccionesProvedorOperativo.SPRO1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeCuerpo()

            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ConstruyeSeccion(seccionEnum_:=SeccionesProvedorOperativo.SPRO2,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesProvedorOperativo.SPRO3,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesProvedorOperativo.SPRO4,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=False)

            ConstruyeSeccion(seccionEnum_:=SeccionesProvedorOperativo.SPRO5,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=False)

        End Sub

#End Region

#Region "Funciones"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_

                'Generales
                '_id ( ObjectID) = clave proveedor ( interna )
                'CP_CVE_PROVEEDOR = secuencia para usuarios
                Case SeccionesProvedorOperativo.SPRO1
                    Return New List(Of Nodo) From {
                                                    Item(CP_CVE_PROVEEDOR, Entero), 'Clave p/usuario, secuencia automática
 _
                                                    Item(CP_CVE_EMPRESA, Entero), 'Clave p/usuario, secuencia...
                                                    Item(CP_ID_EMPRESA, IdObject), 'Clave mongo ObjectID ( Para mantenimiento con sus dependencias)
                                                    Item(CA_RAZON_SOCIAL_PROVEEDOR, Texto, longitud_:=120),
 _
                                                    Item(CP_TIPO_USO, Entero)
                                                }

                'Detalle proveedor
                Case SeccionesProvedorOperativo.SPRO2
                    Return New List(Of Nodo) From {
                                                    Item(CP_SECUENCIA_PROVEEDOR, Entero),
                                                    Item(CA_RFC_PROVEEDOR, Texto, longitud_:=13),
                                                    Item(CA_TAX_ID_PROVEEDOR, Texto, longitud_:=30),
                                                    Item(CA_CURP_PROVEEDOR, Texto, longitud_:=18),
 _
                                                    Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=250),
 _
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
 _
                                                    Item(CamposDomicilio.CP_ID_DOMICILIO, IdObject),
                                                    Item(CamposDomicilio.CP_SEC_DOMICILIO, Entero),
                                                    Item(CamposGlobales.CP_IDENTITY, Entero)
                                                }

                'Domicilios físcales
                Case SeccionesProvedorOperativo.SPRO3
                    Return New List(Of Nodo) From {
                                                    Item(CP_SECUENCIA_PROVEEDOR_DOMICILIO, Entero),
                                                    Item(CP_TAX_ID_DOMICILIO, Texto, longitud_:=30),
                                                    Item(CP_RFC_PROVEEDOR_DOMICILIO, Texto, longitud_:=13),
                                                    Item(CA_DOMICILIO_FISCAL, Texto, longitud_:=250),
                                                    Item(CP_ARCHIVADO_DOMICILIO, Booleano)
                                                }

                'Vinculaciones con clientes
                Case SeccionesProvedorOperativo.SPRO4
                    Return New List(Of Nodo) From {
                                                    Item(CP_ID_CLIENTE_VINCULACION, IdObject),
                                                    Item(CP_TAX_ID_VINCULACION, Texto, longitud_:=30),
                                                    Item(CP_RFC_PROVEEDOR_VINCULACION, Texto, longitud_:=13),
                                                    Item(CA_CVE_VINCULACION, Texto, longitud_:=1),
                                                    Item(CP_VINCULACION, Texto, longitud_:=60),
                                                    Item(CP_PORCENTAJE_VINCULACION, Entero)
                                                }

                'Configuración adicional
                Case SeccionesProvedorOperativo.SPRO5
                    Return New List(Of Nodo) From {
                                                    Item(CP_ID_CLIENTE_CONFIGURACION, IdObject),
                                                    Item(CP_TAX_ID_CONFIGURACION, Texto, longitud_:=30),
                                                    Item(CP_RFC_PROVEEDOR_CONFIGURACION, Texto, longitud_:=13),
                                                    Item(CA_CVE_METODO_VALORACION, Entero),
                                                    Item(CP_METODO_VALORACION, Texto, longitud_:=60),
                                                    Item(CA_CVE_INCOTERM, Texto, longitud_:=3),
                                                    Item(CP_INCOTERM, Texto, longitud_:=70)
                                                }

                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function

#End Region

    End Class

End Namespace