Imports gsol.krom
Imports Wma.Exceptions
Imports MongoDB.Bson
Imports Syn.Documento
Imports Syn.CustomBrokers.Controllers
Imports System.IO
Imports MongoDB.Driver
Imports System.Drawing
Imports iText.IO.Image
Imports Ia.Pln
Imports MongoDB.Driver.Linq.Processors
Imports Wma.Exceptions.TagWatcher
Imports Syn.Utils
Imports Rec.Globals.Utils

Public Class ControladorReferencias
    Implements IControladorReferencias, ICloneable, IDisposable

    'Private _transformer As IControllerDocumentAnalyzer

    Private _controladorSecuencia As IControladorSecuencia

    Private _secuencia As ISecuencia

    Public Property Referencia As List(Of Referencia) Implements IControladorReferencias.Referencia

    Public Property Estado As TagWatcher Implements IControladorReferencias.Estado


    Sub New()

        _Estado = New TagWatcher

        _Referencia = New List(Of Referencia)

        '_transformer = New ControllerDocumentAnalyzer

        _controladorSecuencia = New ControladorSecuencia

    End Sub

    Public Function GeneraSecuencia(ByVal nombre_ As String,
                                       Optional ByVal enviroment_ As Int16 = 0,
                                       Optional ByVal anio_ As Int16 = 0,
                                       Optional ByVal mes_ As Int16 = 0,
                                       Optional ByVal tipoSecuencia_ As Integer = 0,
                                       Optional ByVal subTipoSecuencia_ As Integer = 0,
                                       Optional ByVal prefijo As String = Nothing
                                       ) As Int32 Implements IControladorReferencias.GeneraSecuencia

        _controladorSecuencia = New ControladorSecuencia

        _secuencia = New Secuencia With {.nombre = nombre_,
            .environment = enviroment_,
            .anio = anio_,
            .mes = mes_,
            .tiposecuencia = tipoSecuencia_,
            .subtiposecuencia = subTipoSecuencia_,
            .prefijo = prefijo
        }

        Dim respuesta_ = _controladorSecuencia.Generar(_secuencia)

        Dim sec_ As Int32 = 0

        Select Case respuesta_.Status

            Case TypeStatus.Ok

                sec_ = respuesta_.ObjectReturned.sec

            Case Else

        End Select

        Return sec_

    End Function

    Public Function CrearPrereferencias(referencia_ As List(Of Referencia)) As TagWatcher Implements IControladorReferencias.CrearPrereferencias
        Throw New NotImplementedException()
    End Function

    Public Function CrearPrereferencias(documento_ As List(Of MemoryStream)) As TagWatcher Implements IControladorReferencias.CrearPrereferencias

        If (documento_.Count > 0) Then

            '_Estado = _transformer.DocumentAnalyzer(Of Referencia)(documento_)

        Else

            _Estado.SetError()

            _Estado.ErrorDescription = "No se puede trabajar un objeto vacio"

        End If

    End Function

    Public Function CrearPrereferencia(referencia_ As Referencia) As TagWatcher Implements IControladorReferencias.CrearPrereferencia
        Throw New NotImplementedException()
    End Function

    Public Function CrearPrereferencia(documento_ As MemoryStream) As TagWatcher Implements IControladorReferencias.CrearPrereferencia

        If (documento_ IsNot Nothing) Then

            Dim x As New List(Of MemoryStream)

            x.Add(documento_)

            '_Estado = _transformer.DocumentAnalyzer(Of Object)(x)

        Else

            _Estado.SetError()

            _Estado.ErrorDescription = "No se puede trabajar un objeto vacio"

        End If

    End Function

    Public Function AgregarGuias(documento_ As List(Of MemoryStream), ByVal id_ As ObjectId, ByVal modalidad As IControladorReferencias.ModalidadesOperativas) As TagWatcher Implements IControladorReferencias.AgregarGuias

        If (documento_.Count > 0) Then

            'Dim x = _transformer.DocumentAnalyzer(Of Referencia)(documento_)




        Else

            _Estado.SetError()

            _Estado.ErrorDescription = "No se puede trabajar un objeto vacio"

        End If

    End Function

    Public Function AgregarGuia(guia_ As Guia, ByVal id_ As ObjectId, ByVal modalidad As IControladorReferencias.ModalidadesOperativas) As TagWatcher Implements IControladorReferencias.AgregarGuia
        Throw New NotImplementedException()
    End Function

    Public Function AgregarGuia(documento_ As MemoryStream, ByVal id_ As ObjectId, ByVal modalidad As IControladorReferencias.ModalidadesOperativas) As TagWatcher Implements IControladorReferencias.AgregarGuia
        Throw New NotImplementedException()
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        Throw New NotImplementedException()
    End Sub

End Class
