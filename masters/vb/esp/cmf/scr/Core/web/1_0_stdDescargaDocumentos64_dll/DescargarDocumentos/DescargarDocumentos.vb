Imports Gsol.BaseDatos.Operaciones
Imports Gsol.krom.controladores
Imports Gsol.documento
Imports Wma.Reports
Imports System.IO
Imports gsol

Public Class DescargarDocumentos
    Inherits System.Web.UI.Page

#Region "Atributos"

    Public Shared _controladorWeb As ControladorWeb

#End Region

#Region "Metodos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim modulo_ As String = Request.QueryString("mod")

        Select Case modulo_

            Case "opeadmin"
                DocumentosOperativosAdministrativos()

            Case "repcom"
                ReportesCompletos()

            Case Nothing
                Response.Redirect("../Ges003-001-Consultas.Principal.aspx")

        End Select

    End Sub

    Public Function MensajeJavasScript(ByVal mensaje_ As String) As String
        'Dim javaScript As String = "<script type='text/javascript'>hideShowDiv();</script>"
        'Dim scriptKey As String = "UniqueKeyForThisScript"
        'Dim javaScript As String = "<script type='text/javascript'> alert(" & Chr(34) & mensaje_ & Chr(34) & "); </script>"


        Dim scriptKey As String = "UniqueKeyForThisScript2"
        'Dim javaScript As String = "<script type='text/javascript'> alert(" & Chr(34) & mensaje_ & Chr(34) & ");</script>"
        Dim javaScript As String = _
            "<script>" & _
            "$.KromMessage('danger', " & mensaje_ & "); " & _
            "</script>"

        ClientScript.RegisterStartupScript(Me.GetType(), scriptKey, javaScript)

        Return Nothing

    End Function

    Private Sub DocumentosOperativosAdministrativos()

        Try

            Dim espaciotrabajo_ As IEspacioTrabajo = New EspacioTrabajo

            Dim ioperaciones_ As IOperacionesCatalogo = New OperacionesCatalogo

            Dim documentoExtranet_ As New OperacionesDocumentoExtranet64

            Dim tipo_ As IOperacionesDocumentoExtranet.TiposDocumentoExtranet

            Dim extensionDocumento_ As IOperacionesDocumentoExtranet.TiposArchivosEsperado

            Dim tipoClaseDocumento_ As IOperacionesDocumentoExtranet.TipoClaseDocumento

            Dim redirec_ As String = "/Ges003-001-Consultas.Principal.aspx"

            Dim tipoDocumento_ As String = Request.QueryString("doc")

            Dim valorBusqueda_ As String = Request.QueryString("bus")

            Dim extension_ As String = Request.QueryString("ext")

            Dim claseDocumento_ As String = Request.QueryString("cla")

            Dim tipoDocumentosDescarga_ As String = Request.QueryString("des")

            espaciotrabajo_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

            ioperaciones_.EspacioTrabajo = espaciotrabajo_

            Select Case tipoDocumento_

                Case "ope"
                    tipo_ = IOperacionesDocumentoExtranet.TiposDocumentoExtranet.Operativo

                Case "admi"
                    tipo_ = IOperacionesDocumentoExtranet.TiposDocumentoExtranet.Administrativo

                Case "mdo"
                    tipo_ = IOperacionesDocumentoExtranet.TiposDocumentoExtranet.DocumentosMaestroDocumentos

                Case "mdz"
                    tipo_ = IOperacionesDocumentoExtranet.TiposDocumentoExtranet.DocumentosMaestroDocumentosZIP

                Case "cmd"
                    tipo_ = IOperacionesDocumentoExtranet.TiposDocumentoExtranet.ClaveDocumentosMaestroDocumentos

                Case Else
                    Response.Redirect(redirec_)

            End Select

            Select Case extension_

                Case "pdf"
                    extensionDocumento_ = IOperacionesDocumentoExtranet.TiposArchivosEsperado.PDF

                Case "xml"
                    extensionDocumento_ = IOperacionesDocumentoExtranet.TiposArchivosEsperado.XML

                Case "zip"
                    extensionDocumento_ = IOperacionesDocumentoExtranet.TiposArchivosEsperado.ZIP

                    'Case "pdfxml"
                    '    extensionDocumento_ = IOperacionesDocumentoExtranet.TiposArchivosEsperado.Todos

                Case Else
                    Response.Redirect(redirec_)

            End Select

            If Not claseDocumento_ Is Nothing Then

                Select Case claseDocumento_

                    Case "sop"
                        tipoClaseDocumento_ = IOperacionesDocumentoExtranet.TipoClaseDocumento.DocumentosSoporte

                    Case "cpg"
                        tipoClaseDocumento_ = IOperacionesDocumentoExtranet.TipoClaseDocumento.ComplementosPago

                    Case "cpt"
                        tipoClaseDocumento_ = IOperacionesDocumentoExtranet.TipoClaseDocumento.ComplementosPagoTerceros

                    Case "clv"
                        tipoClaseDocumento_ = IOperacionesDocumentoExtranet.TipoClaseDocumento.ClaveDocumento

                End Select

            Else

                tipoClaseDocumento_ = IOperacionesDocumentoExtranet.TipoClaseDocumento.SinDocumentosSoporte

            End If

            documentoExtranet_.ObtenerDocumento(New DocumentoExtranet With {.ValorBusqueda = valorBusqueda_,
                                                                            .TipoDocumentoExtranet = tipo_,
                                                                            .ConsultarTipoClaseDocumento = tipoClaseDocumento_,
                                                                            .TipoArchivoEsperado = extensionDocumento_,
                                                                            .TiposDocumentosDescargables = tipoDocumentosDescarga_}, ioperaciones_.Clone)

            'documentoExtranet_.DocumentosExtranet(0).RutaCompleta = "file:" & documentoExtranet_.DocumentosExtranet(0).RutaCompleta

            If documentoExtranet_.Estatus.Status = Wma.Exceptions.TagWatcher.TypeStatus.Ok Then

                If tipoDocumento_ = "ope" Then

                    If documentoExtranet_.DocumentosExtranet(0).DocumentoEncontrado = IOperacionesDocumentoExtranet.OperacionesDocumentoExtranetSiNo.Si Then

                        'MensajeJavasScript("INTENTANDO OPER.....................PRINCIPAL...(" & documentoExtranet_.DocumentosExtranet(0).RutaCompleta & ")")

                        ' Response.Redirect("http://www.google.com/errorOperativo33=" & documentoExtranet_.DocumentosExtranet(0).RutaCompleta)

                        'Response.Redirect("http://www.google.com?ok=1&ruta=" & documentoExtranet_.DocumentosExtranet(0).RutaCompleta)

                        'OK
                        KromComponentes.DescargaArchivo(documentoExtranet_.DocumentosExtranet(0).RutaCompleta, True)

                    Else

                        'MensajeJavasScript("REEENVIANDO.....................PRINCIPAL...(" & documentoExtranet_.DocumentosExtranet(0).RutaCompleta & ")")
                        '
                        'Response.Redirect("http://www.google.com?error=1&ruta=" & documentoExtranet_.DocumentosExtranet(0).RutaCompleta)
                        'OK
                        Response.Redirect(redirec_)

                    End If

                Else

                    'INTENTANDO ADMIN.....................PRINCIPAL...(//10.66.1.17/GRK/CompNalKROM/RKU18-GRK-44170.pdf)

                    ' MensajeJavasScript("INTENTANDO ADMIN.....................PRINCIPAL...(" & documentoExtranet_.DocumentosExtranet(0).RutaCompleta & ")")

                    'KromComponentes.DescargaArchivo("//10.66.1.17/GRK/CompNalKROM/RKU18-GRK-44170.pdf", True)


                    'KromComponentes.DescargaArchivo(documentoExtranet_.DocumentosExtranet(0).RutaCompleta, True, False)
                    KromComponentes.DescargaArchivo(documentoExtranet_.DocumentosExtranet(0).RutaCompleta, True)

                    'MensajeJavasScript("SALIMOS ADMIN.....................PRINCIPAL...(" & documentoExtranet_.DocumentosExtranet(0).RutaCompleta & ")")

                End If

            Else

                Response.Redirect(redirec_)
                'Response.Redirect("http://www.google.com?error=25&ruta=" & documentoExtranet_.Estatus.ToString & ").REVISIONWAN: " & documentoExtranet_.Estatus.ObjectReturned.ToString)

            End If

        Catch ex As Exception

            MensajeJavasScript("AVISO!: " & ex.Message)

        End Try


    End Sub

    Public Function ConsultaDocumentosViaLink(ByVal claves_ As List(Of Int64))

        Try

            Dim redirec_ As String = "/Ges025-001-GestorLinks.aspx"

            Dim espaciotrabajo_ As IEspacioTrabajo = New EspacioTrabajo

            Dim ioperaciones_ As IOperacionesCatalogo = New OperacionesCatalogo

            Dim documentoExtranet_ As New OperacionesDocumentoExtranet64

            espaciotrabajo_.ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas

            ioperaciones_.EspacioTrabajo = espaciotrabajo_

            documentoExtranet_.ObtenerDocumento(claves_)

            If documentoExtranet_.Estatus.Status = Wma.Exceptions.TagWatcher.TypeStatus.Ok Then

                'KromComponentes.DescargaArchivo(documentoExtranet_.DocumentosExtranet(0).RutaCompleta, True)

                Return documentoExtranet_.DocumentosExtranet(0).RutaCompleta

            Else

                Response.Redirect(redirec_)

            End If

        Catch ex As Exception

            MensajeJavasScript("AVISO!: " & ex.Message)

        End Try

    End Function

    Private Sub ReportesCompletos()

        Dim o_Stream_ As Byte() = Nothing

        If Not Session("ControladorWeb") Is Nothing Then

            _controladorWeb = Session("ControladorWeb")

            o_Stream_ = _controladorWeb.ExportarReporte(IReportw.Extensions.und)

            If Not o_Stream_ Is Nothing Then

                KromComponentes.DescargaReporte(o_Stream_, " KROM Extranet - " & _controladorWeb.EnlaceDatos.IOperaciones.Nombre, _controladorWeb.TipoReporte)

            End If

        End If

    End Sub

    Private Sub EscribirEnLog(ByVal t_Texto_ As String)

        Dim t_Ruta_ As String = "C:\logs\TestKBW JCCS.txt"

        Dim escritor_ As StreamWriter

        Try

            escritor_ = File.AppendText(t_Ruta_)

            escritor_.WriteLine(t_Texto_)

            escritor_.Flush()

            escritor_.Close()

        Catch ex As Exception

        End Try

    End Sub

#End Region

End Class
