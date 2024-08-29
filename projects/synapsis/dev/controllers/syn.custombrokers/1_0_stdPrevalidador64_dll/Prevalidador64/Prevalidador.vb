Imports System.Web.Script.Serialization
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Wma.Exceptions

Public Class Prevalidador
    Implements IPrevalidador

#Region "Atributos"

    Private _tipoProcesamiento As IPrevalidador.TiposProcesamiento

    Private _tipoValidacion As IPrevalidador.TiposValidacion

    Private _tipoRutaValidacion As IPrevalidador.TiposRutaValidacion

    Private _estatusOperacion As TagWatcher

    Private _pedimento As JavaScriptSerializer()

    Private _listaClavesNormalImpo As New List(Of String)

    Private _listaClavesNormalExpo As New List(Of String)

    Private _listaClavesVirtualExpo As New List(Of String)

    Private _listaClavesVirtualImpo As New List(Of String)

    Private _listaClavesNormalImpoPC As New List(Of String)

    Private _listaClavesNormalTran As New List(Of String)

    Private _estatus As TagWatcher

    Private _disposedValue As Boolean

#End Region

#Region "Propiedades"

    Public Property TipoProcesamiento As IPrevalidador.TiposProcesamiento Implements IPrevalidador.TipoProcesamiento

        Get

            Return _tipoProcesamiento

        End Get

        Set(value As IPrevalidador.TiposProcesamiento)

            _tipoProcesamiento = value

        End Set

    End Property

    Public Property TipoValidacion As IPrevalidador.TiposValidacion Implements IPrevalidador.TipoValidacion

        Get

            Return _tipoValidacion

        End Get

        Set(value As IPrevalidador.TiposValidacion)

            _tipoValidacion = value

        End Set

    End Property

    Public Property TipoRutaValidacion As IPrevalidador.TiposRutaValidacion Implements IPrevalidador.TipoRutaValidacion

        Get

            Return _tipoRutaValidacion

        End Get

        Set(value As IPrevalidador.TiposRutaValidacion)

            _tipoRutaValidacion = value

        End Set

    End Property

    Public Property Estatus As TagWatcher Implements IPrevalidador.Estatus

        Get

            Return _estatus

        End Get

        Set(value As TagWatcher)

            _estatus = value

        End Set

    End Property

    Public Property EstatusOperacion As TagWatcher Implements IPrevalidador.EstatusOperacion

        Get

            Return _estatusOperacion

        End Get

        Set(value As TagWatcher)

            _estatusOperacion = value

        End Set

    End Property

#End Region

#Region "Metodos"

    Public Function AnalisisValidacion(tipoProcesamiento_ As IPrevalidador.TiposProcesamiento,
                                       tipoValidacion_ As IPrevalidador.TiposValidacion,
                                       rutaValidacion_ As IPrevalidador.TiposRutaValidacion,
                                       Optional pedimento_ As DocumentoElectronico = Nothing) As TagWatcher Implements IPrevalidador.AnalisisValidacion

        Throw New NotImplementedException()

    End Function

    Public Function AnalisisValidacion(tipoProcesamiento_ As IPrevalidador.TiposProcesamiento,
                                       tipoValidacion_ As IPrevalidador.TiposValidacion,
                                       rutaValidacion_ As IPrevalidador.TiposRutaValidacion,
                                       Optional pedimento_ As JavaScriptSerializer() = Nothing) As TagWatcher Implements IPrevalidador.AnalisisValidacion

        Throw New NotImplementedException()

    End Function

    Public Function ObtenerInformeValidacion(folioOperacion_ As Integer,
                                             Optional pedimento_ As DocumentoElectronico = Nothing) As TagWatcher Implements IPrevalidador.ObtenerInformeValidacion

        Throw New NotImplementedException()

    End Function

    Public Function ObtenerInformeValidacion(folioOperacion_ As Integer,
                                             Optional pedimento_ As JavaScriptSerializer() = Nothing) As TagWatcher Implements IPrevalidador.ObtenerInformeValidacion

        Throw New NotImplementedException()

    End Function

    Public Function ObtenerRutaValidacion(Optional pedimento_ As DocumentoElectronico = Nothing) As TagWatcher Implements IPrevalidador.ObtenerRutaValidacion

        _estatus = New TagWatcher()
        _estatus.SetOK()

        If pedimento_ IsNot Nothing Then

            Dim cvePedimento_ As String = Nothing
            Dim tipoOperacion_ As Integer = 0
            Dim tipoPedimento_ As Integer = 0
            Dim regimen_ As String = Nothing
            Dim identificadorPC_ As Boolean = False
            Dim cvePedimentoOriginal_ As String = Nothing 'Se deja para rectis pero se debe ver de donde obtener el valor correcto

            If pedimento_.Attribute(CamposPedimento.CA_CVE_PEDIMENTO).Valor <> Nothing Then

                cvePedimento_ = pedimento_.Attribute(CamposPedimento.CA_CVE_PEDIMENTO).Valor

            Else

                _estatus.SetError(Me, "No se tiene la clave de pedimento para definir la ruta de validación correspondiente, verifique su información.")

            End If

            If pedimento_.Attribute(CamposPedimento.CA_TIPO_OPERACION).Valor <> 0 Then

                tipoOperacion_ = pedimento_.Attribute(CamposPedimento.CA_TIPO_OPERACION).Valor

            Else

                _estatus.SetError(Me, "No se tiene el tipo de operación para definir la ruta de validación correspondiente, verifique su información.")

            End If

            If pedimento_.Attribute(CamposPedimento.CP_TIPO_PEDIMENTO).Valor <> 0 Then

                tipoPedimento_ = pedimento_.Attribute(CamposPedimento.CP_TIPO_PEDIMENTO).Valor

            Else

                _estatus.SetError(Me, "No se tiene el tipo de pedimento para definir la ruta de validación correspondiente, verifique su información.")

            End If

            If pedimento_.Attribute(CamposPedimento.CA_REGIMEN).Valor <> Nothing Then

                regimen_ = pedimento_.Attribute(CamposPedimento.CA_REGIMEN).Valor

            Else

                _estatus.SetError(Me, "No se tiene el regimen de pedimento para definir la ruta de validación correspondiente, verifique su información.")

            End If

            If pedimento_.Seccion(SeccionesPedimento.ANS18) IsNot Nothing Then

                If pedimento_.Seccion(SeccionesPedimento.ANS18).CantidadPartidas > 0 Then

                    Dim listadoIdentificadores_ = From identificadores_ In pedimento_.Seccion(SeccionesPedimento.ANS18).Nodos
                                                  Select identificadores_
                                                  Where identificadores_.estado = 1

                    If listadoIdentificadores_.Count >= 1 Then

                        For Each identificador_ In listadoIdentificadores_

                            If identificador_.Attribute(CamposPedimento.CA_CVE_IDENTIFICADOR).Valor = "PC" Then

                                identificadorPC_ = True

                                Exit For

                            Else

                                _estatus.SetOK()

                            End If

                        Next

                    Else

                        _estatus.SetError(Me, "No se tienen identificadores de pedimento cargados correctamente para definir la ruta de validación correspondiente, verifique su información.")

                    End If

                Else

                    _estatus.SetError(Me, "No se tienen identificadores de pedimento para definir la ruta de validación correspondiente, verifique su información.")

                End If

            Else

                _estatus.SetError(Me, "No se tiene la sección de identificadores nivel pedimento para definir la ruta de validación correspondiente, verifique su información.")

            End If

            If tipoPedimento_ = 4 Then

                If pedimento_.Attribute(CamposPedimento.CA_RECTIFICACION_CVE_PEDIMENTO_ORIGINAL).Valor <> Nothing Then

                    cvePedimentoOriginal_ = pedimento_.Attribute(CamposPedimento.CA_RECTIFICACION_CVE_PEDIMENTO_ORIGINAL).Valor

                Else

                    _estatus.SetError(Me, "No se tiene la clave de pedimento original para definir la ruta de validación correspondiente, verifique su información.")

                End If

            End If

            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                CrearListasClaves()

                _estatus.ObjectReturned = ComparacionRutasValidacion(cvePedimento_, tipoOperacion_, tipoPedimento_, regimen_, identificadorPC_, cvePedimentoOriginal_)

            Else

                _estatus.SetError(Me, _estatus.ErrorDescription)

            End If

        Else

            _estatus.SetError(Me, "No se tiene pedimento para detectar la ruta de validación correspondiente.")

        End If

        Return _estatus

    End Function

    Private Function ComparacionRutasValidacion(cvePedimento_ As String,
                                           tipoOperacion_ As Integer,
                                           Optional tipoPedimento_ As Integer = 0,
                                           Optional regimen_ As String = Nothing,
                                           Optional identificadorPC_ As Boolean = False,
                                           Optional cvePedimentoOriginal_ As String = Nothing) As IPrevalidador.TiposRutaValidacion

        'Comenzamos la validación
        If tipoPedimento_ = 1 And tipoOperacion_ = 1 And _listaClavesNormalImpo.Contains(cvePedimento_) And identificadorPC_ = False Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA1

        ElseIf tipoPedimento_ = 1 And tipoOperacion_ = 1 And cvePedimento_ = "GC" Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA2

        ElseIf tipoPedimento_ = 1 And tipoOperacion_ = 1 And _listaClavesNormalImpoPC.Contains(cvePedimento_) And identificadorPC_ = True Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA3

        ElseIf tipoPedimento_ = 1 And tipoOperacion_ = 2 And _listaClavesNormalExpo.Contains(cvePedimento_) And identificadorPC_ = False Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA4

        ElseIf tipoPedimento_ = 1 And tipoOperacion_ = 2 And _listaClavesNormalExpo.Contains(cvePedimento_) And identificadorPC_ = True Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA5

        ElseIf tipoPedimento_ = 1 And tipoOperacion_ = 2 And cvePedimento_ = "GC" Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA6

        ElseIf tipoPedimento_ = 1 And tipoOperacion_ = 2 And cvePedimento_ = "CT" Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA7

        ElseIf tipoPedimento_ = 1 And _listaClavesNormalTran.Contains(cvePedimento_) And identificadorPC_ = False Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA8

        ElseIf tipoOperacion_ = 2 And _listaClavesVirtualExpo.Contains(cvePedimento_) And identificadorPC_ = False Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA9

        ElseIf tipoOperacion_ = 2 And _listaClavesVirtualExpo.Contains(cvePedimento_) And identificadorPC_ = True Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA10

        ElseIf tipoOperacion_ = 1 And _listaClavesVirtualImpo.Contains(cvePedimento_) And identificadorPC_ = False Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA11

        ElseIf tipoOperacion_ = 1 And _listaClavesVirtualImpo.Contains(cvePedimento_) And identificadorPC_ = True Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA12

        ElseIf tipoPedimento_ = 4 And tipoOperacion_ = 1 And _listaClavesNormalImpo.Contains(cvePedimentoOriginal_) And identificadorPC_ = False Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA13

        ElseIf tipoPedimento_ = 4 And tipoOperacion_ = 1 And _listaClavesNormalImpoPC.Contains(cvePedimentoOriginal_) And identificadorPC_ = True Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA14

        ElseIf tipoPedimento_ = 4 And tipoOperacion_ = 2 And _listaClavesNormalExpo.Contains(cvePedimentoOriginal_) And identificadorPC_ = False Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA15

        ElseIf tipoPedimento_ = 4 And tipoOperacion_ = 2 And cvePedimentoOriginal_ = "CT" Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA16

        ElseIf tipoPedimento_ = 4 And tipoOperacion_ = 2 And _listaClavesNormalExpo.Contains(cvePedimentoOriginal_) And identificadorPC_ = True Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA17

        ElseIf tipoPedimento_ = 4 And _listaClavesNormalTran.Contains(cvePedimentoOriginal_) And identificadorPC_ = False Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA18

        ElseIf tipoPedimento_ = 4 And tipoOperacion_ = 1 And _listaClavesVirtualImpo.Contains(cvePedimentoOriginal_) And identificadorPC_ = False Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA19

        ElseIf tipoPedimento_ = 4 And tipoOperacion_ = 2 And _listaClavesVirtualExpo.Contains(cvePedimentoOriginal_) And identificadorPC_ = False Then

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA20

        Else

            _estatus.SetError(Me, "No se tiene combinación correcta de parámetros para detectar la ruta de validación, verifique la información.")

            _tipoRutaValidacion = IPrevalidador.TiposRutaValidacion.RUVA0 'Sin definir ya que no se detecto ninguna

        End If

        Return _tipoRutaValidacion

    End Function

    Sub CrearListasClaves()

        'Creamos las listas de las claves que le tocan a ciertos tipos para hacer la comparación
        _listaClavesNormalImpo = New List(Of String) From {"P1", "VF", "AJ", "G1", "A1", "BA", "BR", "K1", "K2", "L1", "E2", "AD",
            "T1", "S2", "BP", "BH", "BO", "G2", "F3", "D1", "C1", "H1", "IN", "F5", "AF", "VU", "A5", "G8", "I1", "C3", "BD",
            "BI", "H8", "M2", "M1", "F8", "F9", "A4", "BE", "M3", "BC", "E3", "E1", "F2", "M4", "E4", "M5", "F4", "A3"}

        _listaClavesNormalExpo = New List(Of String) From {"BF", "AJ", "G1", "A1", "BA", "BR", "K1", "L1", "T1", "BM",
            "S2", "BP", "BO", "RT", "K3", "J3", "D1", "H1", "F4", "I1", "BD", "H8", "M2", "M1", "F8", "F9", "A4",
            "BE", "M3", "G6", "G7", "K2", "J4", "M5"}

        _listaClavesVirtualExpo = New List(Of String) From {"BB", "V1", "V2", "V3", "V4", "V6", "V7", "V9", "VD", "G9"}

        _listaClavesVirtualImpo = New List(Of String) From {"BB", "V1", "V2", "V3", "V4", "V5", "V6", "V7", "V8", "V9", "G9"}

        _listaClavesNormalImpoPC = New List(Of String) From {"IN", "AF"}

        _listaClavesNormalTran = New List(Of String) From {"T3", "T7", "T9", "T6"}

    End Sub

    Public Function ObtenerRutaValidacion(Optional pedimento_ As JavaScriptSerializer() = Nothing) As TagWatcher Implements IPrevalidador.ObtenerRutaValidacion

        Throw New NotImplementedException()

    End Function

    Public Function ObtenerEstatusOperacion(folioOperacion_ As Integer) As TagWatcher Implements IPrevalidador.ObtenerEstatusOperacion

        Throw New NotImplementedException()

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not _disposedValue Then

            If disposing Then

                _tipoProcesamiento = Nothing

                _tipoValidacion = Nothing

                _tipoRutaValidacion = Nothing

                _estatusOperacion = Nothing

                _pedimento = Nothing

                _estatus = Nothing

            End If

            _disposedValue = True

        End If

    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose

        Dispose(disposing:=True)

        GC.SuppressFinalize(Me)

    End Sub

#End Region

End Class