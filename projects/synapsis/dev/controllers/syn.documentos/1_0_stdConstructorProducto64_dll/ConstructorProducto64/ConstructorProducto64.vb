Imports gsol.krom
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.RecursosComercioExterior

Namespace Syn.Documento
    <Serializable()>
    Public Class ConstructorProducto
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

#End Region

#Region "Builders"

        Sub New()

            Inicializa(Nothing,
                        TiposDocumentoElectronico.Productos,
                        True)

        End Sub
        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                       TiposDocumentoElectronico.Productos,
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
                         TiposDocumentoElectronico.Productos)

        End Sub

#End Region

#Region "Methods"
        Public Overrides Sub ConstruyeEncabezado()

            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            ' Encabezado principal de la referencia
            ConstruyeSeccion(seccionEnum_:=SeccionesProducto.SPTO1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeEncabezadoPaginasSecundarias()

        End Sub
        Public Overrides Sub ConstruyeCuerpo()

            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ConstruyeSeccion(seccionEnum_:=SeccionesProducto.SPTO2,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesProducto.SPTO3,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=True)

            ConstruyeSeccion(seccionEnum_:=SeccionesProducto.SPTO4,
                 tipoBloque_:=TiposBloque.Cuerpo,
                 conCampos_:=True)



        End Sub
        Public Overrides Sub ConstruyePiePagina()

        End Sub

#End Region

#Region "Funciones"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_
                'SeccionesProducto.SPTO1
                ''Encabezado principal del documento
                Case SeccionesProducto.SPTO1
                    Return New List(Of Nodo) From {
                                                    Item(CamposProducto.CP_NOMBRE_COMERCIAL, Texto, 120),
                                                    Item(CamposProducto.CP_HABILITADO, Booleano)
                                                  }


                Case SeccionesProducto.SPTO2

                    'Sección dos del documento

                    Return New List(Of Nodo) From {
                                                    Item(CamposProducto.CP_FRACCION_ARANCELARIA, Texto, 80),
                                                    Item(CamposProducto.CP_NICO, Texto),
                                                    Item(CamposProducto.CP_FECHA_REGISTRO, Fecha),
                                                    Item(CamposProducto.CP_ESTATUS, Texto, 30),
                                                    Item(CamposProducto.CP_OBSERVACION, Texto, 250)
                                                  }
                    '

                Case SeccionesProducto.SPTO3

                    'Sección tres del documento

                    Return New List(Of Nodo) From {
                                                    Item(CamposClientes.CA_RAZON_SOCIAL, Texto, 120),
                                                    Item(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, Texto, 120),
                                                    Item(CamposGlobales.CP_IDENTITY, Entero),
                                                    Item(SeccionesProducto.SPTO5, True)
                                                  }

                Case SeccionesProducto.SPTO4

                    'Seccion cuatro del documento
                    Return New List(Of Nodo) From {
                                                    Item(CamposProducto.CP_FRACCION_ARANCELARIA, Texto, 80),
                                                    Item(CamposProducto.CP_NICO, Texto, 80),
                                                    Item(CamposProducto.CP_FECHA_MODIFICACION, Fecha),
                                                    Item(CamposProducto.CP_MOTIVO, Texto, 250)
                                                  }

                Case SeccionesProducto.SPTO5

                    'Seccion cinco del documento
                    Return New List(Of Nodo) From {
                                                    Item(CamposProducto.CP_IDKROM, Entero),
                                                    Item(CamposProducto.CP_NUMERO_PARTE, Texto, 30),
                                                    Item(CamposProducto.CP_ALIAS, Texto, 80),
                                                    Item(CamposProducto.CP_TIPO_ALIAS, Entero),
                                                    Item(CamposProducto.CP_DESCRIPCION, Texto, 250),
                                                    Item(CamposProducto.CP_APLICACOVE, Booleano),
                                                    Item(CamposProducto.CP_DESCRIPCION_COVE, Texto, 250),
                                                    Item(CamposProducto.CP_FECHA_MODIFICACION, Fecha)
                                                  }



                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function



#End Region
    End Class

End Namespace