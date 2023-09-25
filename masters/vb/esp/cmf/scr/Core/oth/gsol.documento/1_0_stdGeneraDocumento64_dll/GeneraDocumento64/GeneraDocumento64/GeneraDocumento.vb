Imports gsol.Documento
Imports gsol.BaseDatos.Operaciones
Imports System.Xml
'Imports Ionic.Zip
Imports System.IO.Compression


Namespace Gsol.Documento

    Public Class GeneraDocumentoXMLCE
        Implements IGeneraDocumento

#Region "Atributos"

        Private _ioCatalogo As IOperacionesCatalogo

        ' Private _archivoGenerado As Object

        Private _fechaGenerado As DateTime

        Private _version As String

        Private _fechaGeneral As Date

        Private _RFC As String

        Private _ruta As String

        Private _rutaZip As String

        Private _rutaXML As String

#End Region

#Region "Propiedades"

        Public Property IOCatalogo As IOperacionesCatalogo _
            Implements IGeneraDocumento.IOCatalogo

            Get

                Return _ioCatalogo

            End Get

            Set(ByVal value As IOperacionesCatalogo)

                _ioCatalogo = value

            End Set

        End Property

        'Property ArchivoGenerado As Object Implements IGeneraDocumento.ArchivoGenerado
        '    Get
        '        Return _archivoGenerado
        '    End Get
        '    Set(value As Object)
        '        _archivoGenerado = value
        '    End Set
        'End Property

        Property FechaGenerado As Date _
            Implements IGeneraDocumento.FechaGenerado

            Get

                Return _fechaGenerado

            End Get

            Set(value As Date)

                _fechaGenerado = value

            End Set
        End Property

        Public Property Version As String _
            Implements IGeneraDocumento.Version

            Get

                Return _version

            End Get

            Set(ByVal value As String)

                _version = value

            End Set

        End Property


        Public Property FechaGeneral As Date _
            Implements IGeneraDocumento.FechaGeneral

            Get

                Return _fechaGeneral

            End Get

            Set(ByVal value As Date)

                _fechaGeneral = value

            End Set

        End Property


        Public Property Ruta As String _
            Implements IGeneraDocumento.Ruta

            Get
                Return _ruta
            End Get
            Set(ByVal value As String)
                _ruta = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Sub New()

            _ioCatalogo = New OperacionesCatalogo

            _ruta = ""

            _RFC = ""

            _fechaGenerado = System.DateTime.Now

            _fechaGeneral = System.DateTime.Now

            _version = ""

        End Sub

#End Region

#Region "Métodos"

        Public Sub ProcesarDocumento(ByVal tipoArchivo_ As IGeneraDocumento.TiposProcesable) _
            Implements IGeneraDocumento.ProcesarDocumento

            Select Case tipoArchivo_

                Case IGeneraDocumento.TiposProcesable.XMLCatalogo

                    GeneraXMLCECatalogo()

                Case IGeneraDocumento.TiposProcesable.XMLBalanza

                    GeneraXMLCEBalanza()

                Case IGeneraDocumento.TiposProcesable.XMLPolizas

                    GeneraXMLCEPolizas()
            End Select

        End Sub



        Private Sub GeneraXMLCECatalogo()

            Dim numeroRegistros_ As Int32

            numeroRegistros_ = _ioCatalogo.Vista.Tables(0).Rows.Count()

            _rutaZip = _ruta + "\" & _RFC & " " & _fechaGeneral.Year & " " & _fechaGeneral.ToString("MM") & " " & "CT\"

            System.IO.Directory.CreateDirectory(_rutaZip)
            'System.IO.Directory.CreateDirectory(_rutaZip & "\xml\")
            'System.IO.Directory.CreateDirectory(_rutaZip & "\zip\")

            _rutaXML = _rutaZip & _RFC & " " & _fechaGeneral.Year & " " & _fechaGeneral.ToString("MM") & " " & "CT.xml"

            Dim writer As New XmlTextWriter(_rutaXML, System.Text.Encoding.UTF8)

            writer.WriteStartDocument(True)

            writer.Formatting = Formatting.Indented

            writer.Indentation = 2
            '---Genera el primer elemento del XML "Cátalogo"

            writer.WriteStartElement("Catalogo")

            writer.WriteAttributeString(
                "Ano",
                _fechaGeneral.Year)

            writer.WriteAttributeString(
                "Mes",
                _fechaGeneral.ToString("MM"))

            writer.WriteAttributeString(
                "TotalCtas",
                numeroRegistros_)

            writer.WriteAttributeString(
                "RFC",
                _RFC)

            writer.WriteAttributeString(
                "Version",
                _version)

            '---- Genera un nodo "Ctas" por cada cuenta contable en el catálogo que se encuentre con estado 1 y status 1 (ACTIVA)

            For i = 0 To numeroRegistros_ - 1

                writer.WriteStartElement("Ctas")

                writer.WriteAttributeString(
                    "Natur",
                    _ioCatalogo.Vista.Tables(0).Rows(i)("Naturaleza").ToString.Substring(0, 1))

                writer.WriteAttributeString(
                    "Nivel",
                    _ioCatalogo.Vista.Tables(0).Rows(i)("Nivel"))

                If Not _ioCatalogo.Vista.Tables(0).Rows(i)("Subcuenta de") = "" Then

                    writer.WriteAttributeString(
                        "SubCtaDe",
                        _ioCatalogo.Vista.Tables(0).Rows(i)(2))

                End If

                writer.WriteAttributeString(
                    "Desc",
                    _ioCatalogo.Vista.Tables(0).Rows(i)(3))

                writer.WriteAttributeString(
                    "NumCta",
                    _ioCatalogo.Vista.Tables(0).Rows(i)(4))

                writer.WriteAttributeString(
                    "CodAgrup",
                    _ioCatalogo.Vista.Tables(0).Rows(i)(5))

                writer.WriteEndElement()

            Next
            ' -----Finaliza los nodos Ctas

            '---Cierra el elemento "Catálogo"
            writer.WriteEndElement()

            'Cierra el documento
            writer.WriteEndDocument()

            writer.Close()

            'Dim startPath As String = "c:\example\start"
            'Dim zipPath As String = "c:\example\result.zip"
            'Dim extractPath As String = "c:\example\extract"

            'ZipFile.CreateFromDirectory(_rutaZip, _rutaZip & "\zip\" & _RFC & " " & _fechaGeneral.Year & " " & _fechaGeneral.ToString("MM") & " " & "CT.zip")

            'ZipFile.ExtractToDirectory(zipPath, extractPath)




        End Sub

        Private Sub GeneraXMLCEBalanza()

        End Sub

        Private Sub GeneraXMLCEPolizas()

        End Sub

        Public Sub ConsultaDatosEmpresa() Implements IGeneraDocumento.ConsultaDatosEmpresa

            Dim ioperacionesEmpresa_ As IOperacionesCatalogo

            ioperacionesEmpresa_ = New OperacionesCatalogo

            ioperacionesEmpresa_.VistaEncabezados = "Ve000IUDivisionesEmpresariales"

            ioperacionesEmpresa_.OperadorCatalogoConsulta = "Vt000DivisionesEmpresariales"

            ioperacionesEmpresa_.ClausulasLibres = " AND i_Cve_DivisionEmpresarial ='" &
                _ioCatalogo.EspacioTrabajo.MisCredenciales.DivisionEmpresaria & "'"

            ioperacionesEmpresa_.CantidadVisibleRegistros = 1

            ioperacionesEmpresa_.PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Consulta)

            ioperacionesEmpresa_.GenerarVista()

            If ioperacionesEmpresa_.Vista.Tables(0).Rows.Count > 0 Then

                _RFC = ioperacionesEmpresa_.Vista.Tables(0).Rows(0)("RFC")

            End If

        End Sub
#End Region
    End Class
End Namespace
