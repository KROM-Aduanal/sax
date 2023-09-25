Imports gsol.krom
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.RecursosComercioExterior

Namespace Syn.Documento
    <Serializable()>
    Public Class ConstructorTIGIE
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

#End Region

#Region "Builders"

        Sub New()

            Inicializa(Nothing,
                        TiposDocumentoElectronico.TarifaArancelaria,
                        True)

        End Sub
        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.TarifaArancelaria,
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
                         TiposDocumentoElectronico.TarifaArancelaria)

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

            ' Encabezado principal de TIGIE
            ConstruyeSeccion(seccionEnum_:=SeccionesTarifaArancelaria.TIGIE1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeCuerpo()

            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ConstruyeSeccion(seccionEnum_:=SeccionesTarifaArancelaria.TIGIE2,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=True)


            ConstruyeSeccion(seccionEnum_:=SeccionesTarifaArancelaria.TIGIE3,
                tipoBloque_:=TiposBloque.Cuerpo,
                conCampos_:=True)

        End Sub


#End Region

#Region "Funciones"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_

                'Generales
                Case SeccionesTarifaArancelaria.TIGIE1
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_NUMERO_FRACCION_ARANCELARIA, Texto, longitud_:=10),
                                             Item(CamposTarifaArancelaria.CA_NUMERO_NICO, Texto, longitud_:=2),
                                             Item(CamposTarifaArancelaria.CA_FRACCION_ARANCELARIA, Texto, longitud_:=255),
                                             Item(CamposTarifaArancelaria.CA_NICO, Texto, longitud_:=255),
                                             Item(CamposTarifaArancelaria.CA_MATERIAL_PELIGROSO, Booleano),
                                             Item(CamposTarifaArancelaria.CA_MATERIAL_VULNERABLE, Booleano),
                                             Item(CamposTarifaArancelaria.CA_MATERIAL_SENSIBLE, Booleano),
                                             Item(CamposTarifaArancelaria.CA_SECCION, Texto, longitud_:=255),
                                             Item(CamposTarifaArancelaria.CA_CAPITULO, Texto, longitud_:=255),
                                             Item(CamposTarifaArancelaria.CA_PARTIDA, Texto, longitud_:=255),
                                             Item(CamposTarifaArancelaria.CA_SUBPARTIDA, Texto, longitud_:=255),
                                             Item(CamposTarifaArancelaria.CA_FECHA_PUBLICACION, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_ENTRADA_VIGOR, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_FIN, Fecha),
                                             Item(CamposTarifaArancelaria.PA_ACTUALIZACION, Fecha)
                              }

                'Importación
                Case SeccionesTarifaArancelaria.TIGIE2
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_UNIDAD_MEDIDA, Texto, longitud_:=30),
                                             Item(SeccionesTarifaArancelaria.TIGIE19, True),
                                             Item(SeccionesTarifaArancelaria.TIGIE4, True),
                                             Item(SeccionesTarifaArancelaria.TIGIE5, True)
                              }


                'Exportación
                Case SeccionesTarifaArancelaria.TIGIE3
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_UNIDAD_MEDIDA, Texto, longitud_:=30),
                                             Item(SeccionesTarifaArancelaria.TIGIE19, True),
                                             Item(SeccionesTarifaArancelaria.TIGIE4, True),
                                             Item(SeccionesTarifaArancelaria.TIGIE5, True)
                              }

                'Impuestos
                Case SeccionesTarifaArancelaria.TIGIE19
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_NOMBRE_IMPUESTO, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_VALOR_IMPUESTO, Texto, longitud_:=20),
                                             Item(CamposTarifaArancelaria.CA_FECHA_PUBLICACION, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_ENTRADA_VIGOR, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_FIN, Fecha)
                              }

                'Regulaciones arancelarias
                Case SeccionesTarifaArancelaria.TIGIE4
                    Return New List(Of Nodo) From {
                                             Item(SeccionesTarifaArancelaria.TIGIE6, True),
                                             Item(SeccionesTarifaArancelaria.TIGIE8, True),
                                             Item(SeccionesTarifaArancelaria.TIGIE9, True),
                                             Item(SeccionesTarifaArancelaria.TIGIE10, True),
                                             Item(SeccionesTarifaArancelaria.TIGIE11, True)
                              }

                'Regulaciones no arancelarias
                Case SeccionesTarifaArancelaria.TIGIE5
                    Return New List(Of Nodo) From {
                                             Item(SeccionesTarifaArancelaria.TIGIE13, True), '12
                                             Item(SeccionesTarifaArancelaria.TIGIE14, True),
                                             Item(SeccionesTarifaArancelaria.TIGIE15, True),
                                             Item(SeccionesTarifaArancelaria.TIGIE16, True),
                                             Item(SeccionesTarifaArancelaria.TIGIE17, True),
                                             Item(SeccionesTarifaArancelaria.TIGIE18, True)
                              }


                'Tratados comerciales
                Case SeccionesTarifaArancelaria.TIGIE6
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_NOMBRE_TRATADO, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_NOMBRE_CORTO_TRATADO, Texto, longitud_:=75),
                                             Item(SeccionesTarifaArancelaria.TIGIE7, True)
                              }

                'Paises afiliados
                Case SeccionesTarifaArancelaria.TIGIE7
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_PAIS, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_SECTOR, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_ARANCEL, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_PREFERENCIA, Texto, longitud_:=75),
                                             Item(CamposTarifaArancelaria.CA_OBSERVACION, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_FECHA_PUBLICACION, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_ENTRADA_VIGOR, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_FIN, Fecha)
                              }

                'Cupos arancel
                Case SeccionesTarifaArancelaria.TIGIE8
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_PAIS, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_ICONO_PAIS, IdObject),
                                             Item(CamposTarifaArancelaria.CA_TOTAL_CUPO, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_ARANCEL, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_ARANCEL_FUERA, Texto, longitud_:=1050),
                                             Item(CamposTarifaArancelaria.CA_UNIDAD_MEDIDA, Texto, longitud_:=20),
                                             Item(CamposTarifaArancelaria.CA_NOTA, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_FECHA_PUBLICACION, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_ENTRADA_VIGOR, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_FIN, Fecha)
                              }

                'IEPS
                Case SeccionesTarifaArancelaria.TIGIE9
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_CATEGORIA, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_TIPO, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_TASA, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_CUOTA, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_UNIDAD_MEDIDA, Texto, longitud_:=20),
                                             Item(CamposTarifaArancelaria.CA_OBSERVACION, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_FECHA_PUBLICACION, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_ENTRADA_VIGOR, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_FIN, Fecha)
                              }

                'Cuotas compensatorias
                Case SeccionesTarifaArancelaria.TIGIE10
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_EMPRESA, Texto, longitud_:=100),
                                             Item(CamposTarifaArancelaria.CA_PAIS, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_CUOTA, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_TIPO, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_ACOTACION, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_FECHA_PUBLICACION, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_ENTRADA_VIGOR, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_FIN, Fecha)
                              }

                'Precios estimados
                Case SeccionesTarifaArancelaria.TIGIE11
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_PRECIO, Texto, longitud_:=20),
                                             Item(CamposTarifaArancelaria.CA_UNIDAD_MEDIDA, Texto, longitud_:=20),
                                             Item(CamposTarifaArancelaria.CA_DESCRIPCION, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_FECHA_PUBLICACION, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_ENTRADA_VIGOR, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_FIN, Fecha)
                              }

                'Permisos institucionales
                'Case SeccionesTarifaArancelaria.TIGIE12
                '    Return New List(Of Nodo) From {
                '                             Item(CamposTarifaArancelaria.CA_NOMBRE_INSTITUCION, Texto, longitud_:=100),
                '                             Item(CamposTarifaArancelaria.CA_NOMBRE_CORTO_INSTITUCION, Texto, longitud_:=50),
                '                             Item(SeccionesTarifaArancelaria.TIGIE13)
                '              }

                'Permisos
                Case SeccionesTarifaArancelaria.TIGIE13
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_CLAVE, Texto, longitud_:=100),
                                             Item(CamposTarifaArancelaria.CA_PERMISO, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_ACOTACION, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_FECHA_PUBLICACION, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_ENTRADA_VIGOR, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_FIN, Fecha)
                              }

                'Normas oficiales mexicana
                Case SeccionesTarifaArancelaria.TIGIE14
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_NORMA, Texto, longitud_:=100),
                                             Item(CamposTarifaArancelaria.CA_DESCRIPCION, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_DATO_OMITIDO_INEXACTO, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_ACOTACION, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_FECHA_PUBLICACION, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_ENTRADA_VIGOR, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_FIN, Fecha)
                              }

                'Anexos
                Case SeccionesTarifaArancelaria.TIGIE15
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_NUMERO, Texto, longitud_:=20),
                                             Item(CamposTarifaArancelaria.CA_NOMBRE, Texto, longitud_:=75),
                                             Item(CamposTarifaArancelaria.CA_DESCRIPCION, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_ACOTACION, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_FECHA_PUBLICACION, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_ENTRADA_VIGOR, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_FIN, Fecha)
                              }

                'Embargos
                Case SeccionesTarifaArancelaria.TIGIE16
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_PAIS, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_ICONO_PAIS, IdObject),
                                             Item(CamposTarifaArancelaria.CA_APLICACION, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_ACOTACION, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_MERCANCIA, Texto, longitud_:=100),
                                             Item(CamposTarifaArancelaria.CA_FECHA_PUBLICACION, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_ENTRADA_VIGOR, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_FIN, Fecha)
                              }

                'Cupos minimos
                Case SeccionesTarifaArancelaria.TIGIE17
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_PAIS, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_ICONO_PAIS, IdObject),
                                             Item(CamposTarifaArancelaria.CA_UNIDAD_MEDIDA, Texto, longitud_:=20),
                                             Item(CamposTarifaArancelaria.CA_CUPO, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_DESCRIPCION, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_FECHA_PUBLICACION, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_ENTRADA_VIGOR, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_FIN, Fecha)
                              }

                'Padron sectorial
                Case SeccionesTarifaArancelaria.TIGIE18
                    Return New List(Of Nodo) From {
                                             Item(CamposTarifaArancelaria.CA_SECTOR, Texto, longitud_:=50),
                                             Item(CamposTarifaArancelaria.CA_ANEXO, IdObject),
                                             Item(CamposTarifaArancelaria.CA_ACOTACION, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_DESCRIPCION, Texto, longitud_:=150),
                                             Item(CamposTarifaArancelaria.CA_FECHA_PUBLICACION, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_ENTRADA_VIGOR, Fecha),
                                             Item(CamposTarifaArancelaria.CA_FECHA_FIN, Fecha)
                              }

                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function

#End Region

    End Class

End Namespace

