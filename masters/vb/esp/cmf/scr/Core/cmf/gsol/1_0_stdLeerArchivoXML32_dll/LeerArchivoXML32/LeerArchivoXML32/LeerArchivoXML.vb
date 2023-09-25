Imports System.Xml

Namespace gsol

    Public Class LeerArchivoXML

        Implements ILeerArchivoXML

#Region "Atributos"

        Private _rutaarchivo As String
        Private _propiedades As Dictionary(Of String, String)


#End Region

#Region "Propiedades"

        WriteOnly Property RutaArchivo As String _
            Implements ILeerArchivoXML.RutaArchivo
            Set(ByVal value As String)
                _rutaarchivo = value
            End Set
        End Property

        Property Propiedades As Dictionary(Of String, String) _
            Implements ILeerArchivoXML.Propiedades
            Get
                Propiedades = _propiedades
            End Get
            Set(ByVal value As Dictionary(Of String, String))
                _propiedades = value
            End Set
        End Property
#End Region

#Region "Métodos"
        Public Sub LeerXML() _
            Implements ILeerArchivoXML.LeerXML

            Try
                Dim lectortextoxml As XmlTextReader
                lectortextoxml = New XmlTextReader(_rutaarchivo)
                lectortextoxml.WhitespaceHandling = WhitespaceHandling.None
                'read the xml declaration and advance to family tag
                'Load the Loop
                While (lectortextoxml.Read)
                    Select Case lectortextoxml.NodeType
                        Case XmlNodeType.Element
                            If lectortextoxml.HasAttributes Then
                                While lectortextoxml.MoveToNextAttribute
                                    _propiedades.Add(lectortextoxml.Name, lectortextoxml.Value)
                                End While
                            End If
                        Case XmlNodeType.Text

                    End Select

                End While
                lectortextoxml.Close()
            Catch ex As Exception
                Throw ex
            End Try

        End Sub
#End Region

#Region "Constructores"

        Sub New()

            _rutaarchivo = Environment.GetEnvironmentVariable("systemroot") & "\ConfiguracionGeneral.xml"
            _propiedades = New Dictionary(Of String, String)

        End Sub

#End Region


    End Class

End Namespace

