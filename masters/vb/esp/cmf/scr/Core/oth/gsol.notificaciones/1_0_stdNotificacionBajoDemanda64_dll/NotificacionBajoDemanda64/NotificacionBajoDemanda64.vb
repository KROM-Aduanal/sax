Imports Wma.Exceptions
Imports Gsol.BaseDatos.Operaciones
Imports System.Text
Imports System.Collections.Generic
Imports System.Data
Imports Wma.Exceptions.TagWatcher
Imports Gsol.Controladores

Namespace gsol.notificaciones

    Public Class NotificacionBajoDemanda64
        Implements INotificaciones

#Region "Atributos"

        Private _estatusNotificacion As TagWatcher

        Private _ioperacionescatalogo As IOperacionesCatalogo

#End Region

#Region "Propiedades"

        Public ReadOnly Property EstatusNotificacion As Wma.Exceptions.TagWatcher Implements INotificaciones.EstatusNotificacion

            Get

                Return _estatusNotificacion

            End Get

        End Property

#End Region

#Region "Constructores"

        Sub New()

            _ioperacionescatalogo = New OperacionesCatalogo

            _estatusNotificacion = New TagWatcher

            _estatusNotificacion.SetOK()

        End Sub

        Sub New(ByVal ioperaciones_ As OperacionesCatalogo)

            Me.New()

            _ioperacionescatalogo = ioperaciones_

        End Sub

#End Region

#Region "Funciones"

        Public Function EnviarNotificacion(ByVal eventoNotificacion_ As IRecursosSistemas.EventosNotificacion,
                                    Optional ByVal asuntoNotificacion_ As String = "",
                                    Optional ByVal mensajeNotificacion_ As String = "") As TagWatcher Implements INotificaciones.EnviarNotificacion

            Dim procesarNotificacion_ As ProcesoNotificaciones = New ProcesoNotificaciones(_ioperacionescatalogo)

            With procesarNotificacion_

                .EventoNotificacion = eventoNotificacion_

                .TipoFolio = IRecursosSistemas.TipoDeFolio.MaestroOperaciones

                .TipoNotificacion = IRecursosSistemas.TipoNotificacion.BajoDemanda

                .Asunto = asuntoNotificacion_

                .Mensaje = mensajeNotificacion_

            End With

            _estatusNotificacion = procesarNotificacion_.ProcesarEnvioNotificacion()

            Return _estatusNotificacion

        End Function

        Public Function EnviarNotificacion(ByVal eventoNotificacion_ As IRecursosSistemas.EventosNotificacion,
                                           ByVal tipoDeFolio_ As IRecursosSistemas.TipoDeFolio,
                                           ByVal folio_ As Integer,
                                           Optional ByVal claveCliente_ As Integer = 0,
                                           Optional ByVal asuntoNotificacion_ As String = "",
                                           Optional ByVal mensajeNotificacion_ As String = "") As TagWatcher Implements INotificaciones.EnviarNotificacion

            Dim procesarNotificacion_ As ProcesoNotificaciones = New ProcesoNotificaciones(_ioperacionescatalogo)

            With procesarNotificacion_

                .EventoNotificacion = eventoNotificacion_

                .TipoFolio = tipoDeFolio_

                .TipoNotificacion = IRecursosSistemas.TipoNotificacion.BajoDemanda

                .Folio = folio_

                .ClaveCliente = claveCliente_

                .Asunto = asuntoNotificacion_

                .Mensaje = mensajeNotificacion_

            End With

            _estatusNotificacion = procesarNotificacion_.ProcesarEnvioNotificacion()

            Return _estatusNotificacion

        End Function

        Public Function EnviarNotificacion(ByVal eventoNotificacion_ As IRecursosSistemas.EventosNotificacion,
                                           ByVal claveCliente_ As Integer,
                                           Optional ByVal tipoDeFolio_ As IRecursosSistemas.TipoDeFolio = IRecursosSistemas.TipoDeFolio.MaestroOperaciones,
                                           Optional ByVal listaFolios_ As List(Of Integer) = Nothing,
                                           Optional ByVal asuntoNotificacion_ As String = "",
                                           Optional ByVal mensajeNotificacion_ As String = ""
                                        ) As TagWatcher Implements INotificaciones.EnviarNotificacion

            Dim procesarNotificacion_ As ProcesoNotificaciones

            For Each folio_ As Integer In listaFolios_

                procesarNotificacion_ = New ProcesoNotificaciones(_ioperacionescatalogo)

                With procesarNotificacion_

                    .EventoNotificacion = eventoNotificacion_

                    .TipoFolio = tipoDeFolio_

                    .Folio = folio_

                    .TipoNotificacion = IRecursosSistemas.TipoNotificacion.BajoDemanda

                    .ClaveCliente = claveCliente_

                    .Asunto = asuntoNotificacion_

                    .Mensaje = mensajeNotificacion_

                End With

                _estatusNotificacion = procesarNotificacion_.ProcesarEnvioNotificacion()

            Next

            Return _estatusNotificacion

        End Function

#End Region

    End Class

End Namespace