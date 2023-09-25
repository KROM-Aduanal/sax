Imports Wma.Exceptions
Imports System.Collections.Generic

Namespace gsol.notificaciones

    Public Interface INotificaciones

#Region "Propiedades"

        ReadOnly Property EstatusNotificacion As TagWatcher

#End Region

#Region "Funciones"

        Function EnviarNotificacion(ByVal eventoNotificacion_ As IRecursosSistemas.EventosNotificacion,
                                    Optional ByVal asuntoNotificacion_ As String = "",
                                    Optional ByVal mensajeNotificacion_ As String = ""
                                    ) As TagWatcher

        Function EnviarNotificacion(ByVal eventoNotificacion_ As IRecursosSistemas.EventosNotificacion,
                                    ByVal claveCliente_ As Integer,
                                    Optional ByVal tipoDeFolio_ As IRecursosSistemas.TipoDeFolio = IRecursosSistemas.TipoDeFolio.MaestroOperaciones,
                                    Optional ByVal listaFolios_ As List(Of Integer) = Nothing,
                                    Optional ByVal asuntoNotificacion_ As String = "",
                                    Optional ByVal mensajeNotificacion_ As String = ""
                                    ) As TagWatcher

        Function EnviarNotificacion(ByVal eventoNotificacion_ As IRecursosSistemas.EventosNotificacion,
                                    ByVal tipoDeFolio_ As IRecursosSistemas.TipoDeFolio,
                                    ByVal folio_ As Integer,
                                    Optional ByVal claveCliente_ As Integer = 0,
                                    Optional ByVal asuntoNotificacion_ As String = "",
                                    Optional ByVal mensajeNotificacion_ As String = ""
                                    ) As TagWatcher

#End Region

    End Interface

End Namespace