
Imports gsol.krom
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior


Namespace Syn.Synapsis.Templates
    <Serializable()>
    Public Class ConstructorBase
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

#End Region

#Region "Builders"

        Sub New(ByVal tipoDocumento_ As TiposDocumentoElectronico,
                ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                        tipoDocumento_,
                        construir_)

        End Sub
        Public Sub New(ByVal tipoDocumento_ As TiposDocumentoElectronico,
                       ByVal folioDocumento_ As String,
                       ByVal folioOperacion_ As String,
                       ByVal idCliente_ As Int32,
                       ByVal nombreCliente_ As String
                       )

            Inicializa(folioDocumento_,
                         folioOperacion_,
                         idCliente_,
                         nombreCliente_,
                         tipoDocumento_)

        End Sub

#End Region

#Region "Methods"
        Public Overrides Sub ConstruyeEncabezado()

            'Construir la parte encabezado
            '_estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            'Construir una sección
            'ConstruyeSeccion(seccionDocumento_:=SeccionesGenericas.SGS1,
            '                 tipoBloque_:=TiposBloque.Encabezado,
            '                 conCampos_:=True)

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

            'Construir la parte de cuerpo
            '_estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            'Construir una sección
            'ConstruyeSeccion(seccionDocumento_:=SeccionesGenericas.SGS1,
            '                 tipoBloque_:=TiposBloque.Cuerpo,
            '                 conCampos_:=True)

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

            'Listado de relación sección - campo

            'Select Case idSeccion_

            '    Case SeccionesGenericas.SGS1
            '        Return New List(Of Nodo) From {
            '                                        Item(CamposReferencia.CP_REFERENCIA, Texto, 8),
            '                                        Item(CamposReferencia.CA_NUM_PEDIMENTO_COMPLETO, Texto, 8),
            '                                        Item(CamposReferencia.CA_TIPO_DOCUMENTO, Texto, 8),
            '                                        Item(SeccionesGenericas.SGS1, True)
            '                                      }

            '    Case SeccionesGenericas.SGS2

            '    Case SeccionesGenericas.SGS3

            '    Case SeccionesGenericas.SGS14


            '    Case Else

            '       _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            'End Select

            'Return Nothing

        End Function

#End Region

    End Class

End Namespace