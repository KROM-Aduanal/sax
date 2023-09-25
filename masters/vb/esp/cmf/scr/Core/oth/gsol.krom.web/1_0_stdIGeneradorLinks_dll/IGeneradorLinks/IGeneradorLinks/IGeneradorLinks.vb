Imports Wma.Exceptions

Namespace gsol.krom.web

    Public Interface IGeneradorLinks

#Region "Enums"

        Enum TipoComportamiento
            Consultar
            Descargar
        End Enum

        Enum TipoDatoClave
            ClaveDocumento
            ClaveFactura
            MaestroOperaciones
        End Enum

        Enum TipoDatoNombre
            Referencia
            NombreDocumento
            FolioFactura
        End Enum

        Enum TipoDatoListaClaves
            ListaClaves
        End Enum

        Enum TipoDatoListaNombres
            ListaNombres
        End Enum

#End Region

#Region "Atributos"

#End Region

#Region "Propiedades"

#End Region

#Region "Constructores"

#End Region

#Region "Metodos"

        Function CrearLink(ByVal clave_ As Int64,
                           ByVal tipoDato_ As TipoDatoClave,
                           ByVal comportamiento_ As TipoComportamiento,
                           Optional ByVal rutaWeb_ As String = "CapaPresentacion/Ges025-001-GestorLinks.aspx/",
                           Optional ByVal parametros_ As String = "") As TagWatcher

        Function CrearLink(ByVal nombre_ As String,
                           ByVal tipoDato_ As TipoDatoNombre,
                           ByVal comportamiento_ As TipoComportamiento,
                           Optional ByVal rutaWeb_ As String = "CapaPresentacion/Ges025-001-GestorLinks.aspx/",
                           Optional ByVal parametros_ As String = "") As TagWatcher

        Function CrearLink(ByVal listaClaves_ As List(Of Int64),
                           ByVal tipoDato_ As TipoDatoListaClaves,
                           ByVal comportamiento_ As TipoComportamiento,
                           Optional ByVal rutaWeb_ As String = "CapaPresentacion/Ges025-001-GestorLinks.aspx/",
                           Optional ByVal parametros_ As String = "") As TagWatcher

        Function CrearLink(ByVal listaNombres_ As List(Of String),
                           ByVal tipoDato_ As TipoDatoListaNombres,
                           ByVal comportamiento_ As TipoComportamiento,
                           Optional ByVal rutaWeb_ As String = "CapaPresentacion/Ges025-001-GestorLinks.aspx/",
                           Optional ByVal parametros_ As String = "") As TagWatcher

#End Region

    End Interface

    Public Class Link

#Region "Atributos"

        Private _claveLink As Int64

        Private _link As String

        Private _tipoLink As IGeneradorLinks.TipoComportamiento

        Private _fechaVigencia As Date

        Private _listaDocumentos As List(Of Int64)

#End Region

#Region "Propiedades"

        Public Property ClaveLink As Int64

            Get

                Return _claveLink

            End Get

            Set(ByVal value As Int64)

                _claveLink = value

            End Set

        End Property

        Public Property Link As String

            Get

                Return _link

            End Get

            Set(ByVal value As String)

                _link = value

            End Set

        End Property

        Public Property TipoLink As IGeneradorLinks.TipoComportamiento

            Get

                Return _tipoLink

            End Get

            Set(ByVal value As IGeneradorLinks.TipoComportamiento)

                _tipoLink = value

            End Set

        End Property

        'Public Property Vigencia As Date

        '    Get

        '        Return _fechaVigencia

        '    End Get

        '    Set(ByVal value As Date)

        '        _fechaVigencia = value

        '    End Set

        'End Property

        'Public Property Documentos() As List(Of Int64)

        '    Get

        '        Return _listaDocumentos

        '    End Get

        '    Set(ByVal value As List(Of Int64))

        '        _listaDocumentos = value

        '    End Set

        'End Property

#End Region

#Region "Constructores"

        Sub New()

            _claveLink = 0

            _link = Nothing

            _tipoLink = Nothing

            _fechaVigencia = Nothing

            _listaDocumentos = New List(Of Int64)

        End Sub

#End Region

    End Class

End Namespace