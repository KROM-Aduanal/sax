Imports Cube.Validators
Imports Wma.Exceptions
Imports Syn.CustomBrokers.Controllers.Asistencia
Imports Wma.Exceptions.TagWatcher

Public Class PrevalidadorAsistencia : Inherits Prevalidador
    Implements IPrevalidadorAsistencia

#Region "Atributos"

    Private _tipoAsistenciaConsultada As IPrevalidadorAsistencia.TiposAsistenciaConsultar

    Private _resultadoAsistencia As Asistencia

    Private _estatusAsistencia As TagWatcher

    Private _disposedValue As Boolean

#End Region

#Region "Constructor"

    Sub New()

        _resultadoAsistencia = New Asistencia

        _estatusAsistencia = New TagWatcher

    End Sub

#End Region

#Region "Propiedades"

    Public Property TipoAsistenciaConsultada As IPrevalidadorAsistencia.TiposAsistenciaConsultar Implements IPrevalidadorAsistencia.TipoAsistenciaConsultada

        Get

            Return _tipoAsistenciaConsultada

        End Get

        Set(value As IPrevalidadorAsistencia.TiposAsistenciaConsultar)

            _tipoAsistenciaConsultada = value

        End Set

    End Property

    Public Property ResultadoAsistencia As Asistencia Implements IPrevalidadorAsistencia.ResultadoAsistencia

        Get

            Return _resultadoAsistencia

        End Get

        Set(value As Asistencia)

            _resultadoAsistencia = value

        End Set

    End Property

    Public Property EstatusAsistencia As TagWatcher Implements IPrevalidadorAsistencia.EstatusAsistencia

        Get

            Return _estatusAsistencia

        End Get

        Set(value As TagWatcher)

            _estatusAsistencia = value

        End Set

    End Property

#End Region

#Region "Metodos"

    Public Function ConsultarAsistencia(tipoProcesamiento_ As IPrevalidador.TiposProcesamiento,
                                        tipoValidacion_ As IPrevalidador.TiposValidacion,
                                        tipoAsistenciaConsultar_ As IPrevalidadorAsistencia.TiposAsistenciaConsultar,
                                        parametros_ As Dictionary(Of String, Object)) _
                                        As TagWatcher Implements IPrevalidadorAsistencia.ConsultarAsistencia

        _tipoAsistenciaConsultada = tipoAsistenciaConsultar_

        If tipoProcesamiento_ = IPrevalidador.TiposProcesamiento.AsistirCaptura Then

            If parametros_ IsNot Nothing Then

                Dim controladorCubo_ As ICubeController = New CubeController()

                controladorCubo_.RunRoom(Nucleo.Recursos.GetEnumDescription(_tipoAsistenciaConsultada), parametros_)

                If controladorCubo_.status.Status = TypeStatus.Ok Then

                    ConvertirDiccionarioAsistencia(controladorCubo_.status.ObjectReturned)

                    If _estatusAsistencia.Status = TypeStatus.Ok Then

                        _estatusAsistencia.ObjectReturned = _resultadoAsistencia

                        Return _estatusAsistencia

                    Else

                        _estatusAsistencia.SetError(Me, IPrevalidadorAsistencia.ErroresAsistencia.EAS_006)

                    End If

                Else

                    If controladorCubo_.status.Status = TypeStatus.OkBut Then

                            _estatusAsistencia.SetOKBut(Me, IPrevalidadorAsistencia.ErroresAsistencia.EAS_003)

                        ElseIf controladorCubo_.status.Status = TypeStatus.OkInfo Then

                            _estatusAsistencia.SetOKInfo(Me, IPrevalidadorAsistencia.ErroresAsistencia.EAS_002)

                        Else

                            _estatusAsistencia.SetError(Me, IPrevalidadorAsistencia.ErroresAsistencia.EAS_004)

                        End If

                End If

            Else

                _estatusAsistencia.SetError(Me, IPrevalidadorAsistencia.ErroresAsistencia.EAS_005)

            End If

        End If

        Return _estatusAsistencia

    End Function

    Sub ConvertirDiccionarioAsistencia(ByVal resultadoRoom_ As Dictionary(Of String, String))

        If resultadoRoom_ IsNot Nothing Then

            Dim tipoAsistencia_ As Integer = resultadoRoom_.Item("TIPO_ASISTENCIA")

            Select Case tipoAsistencia_

                Case 1

                    _resultadoAsistencia.TipoAsistencia = TiposAsistencia.Acotacion

                    Dim _lista As New List(Of String) From {
                        resultadoRoom_.ElementAt(1).Value
                    }

                    If _lista.Count > 0 Then

                        _resultadoAsistencia.CamposAfectados = New Dictionary(Of String, Array) From {
                            {resultadoRoom_.ElementAtOrDefault(1).Key, _lista.ToArray}}

                        _estatusAsistencia.SetOK()

                    Else

                        _estatusAsistencia.SetOKInfo(Me, IPrevalidadorAsistencia.ErroresAsistencia.EAS_002)

                    End If

                Case 2

                    _resultadoAsistencia.TipoAsistencia = TiposAsistencia.Advertencia

                Case 3

                    _resultadoAsistencia.TipoAsistencia = TiposAsistencia.ValorFijo

                Case 4

                    _resultadoAsistencia.TipoAsistencia = TiposAsistencia.Sugerencia

                Case Else

                    _resultadoAsistencia.TipoAsistencia = TiposAsistencia.SinDefinir

            End Select

        Else

            _estatusAsistencia.SetError(Me, IPrevalidadorAsistencia.ErroresAsistencia.EAS_004)

        End If

    End Sub

    Sub ConvertirDiccionarioAsistencia(ByVal resultadoRoom_ As Dictionary(Of String, List(Of String)))

        If resultadoRoom_ IsNot Nothing Then

            Dim tipoAsistencia_ As Integer = resultadoRoom_.Item("TIPO_ASISTENCIA").Item(0)

            Select Case tipoAsistencia_

                Case 1

                    _resultadoAsistencia.TipoAsistencia = TiposAsistencia.Acotacion

                    _resultadoAsistencia.CamposAfectados = New Dictionary(Of String, Array)

                    If resultadoRoom_.Count > 0 Then

                        For Each elementList_ In resultadoRoom_

                            If elementList_.Key <> "TIPO_ASISTENCIA" Then

                                _resultadoAsistencia.CamposAfectados.Add(elementList_.Key, elementList_.Value.ToArray)

                                _estatusAsistencia.SetOK()

                            End If

                        Next

                    Else

                        _estatusAsistencia.SetOKInfo(Me, IPrevalidadorAsistencia.ErroresAsistencia.EAS_002)

                    End If

                Case 2

                    _resultadoAsistencia.TipoAsistencia = TiposAsistencia.Advertencia

                Case 3

                    _resultadoAsistencia.TipoAsistencia = TiposAsistencia.ValorFijo

                Case 4

                    _resultadoAsistencia.TipoAsistencia = TiposAsistencia.Sugerencia

                Case Else

                    _resultadoAsistencia.TipoAsistencia = TiposAsistencia.SinDefinir

            End Select

        Else

            _estatusAsistencia.SetError(Me, IPrevalidadorAsistencia.ErroresAsistencia.EAS_004)

        End If

    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not _disposedValue Then

            If disposing Then

                _estatusAsistencia = Nothing

                _resultadoAsistencia = Nothing

                _tipoAsistenciaConsultada = Nothing

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