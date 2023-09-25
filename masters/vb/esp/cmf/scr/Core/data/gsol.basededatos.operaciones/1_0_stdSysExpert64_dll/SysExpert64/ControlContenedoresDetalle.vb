Imports Gsol.BaseDatos.Operaciones
Imports Gsol
Public Class ControlContenedoresDetalle

#Region "Atributos"

    Private _IdControlContenedorDetalle As String

    Private _IdControlContenedor As String

    Private _IdRevalidacionBLContenedor As String

    Private _LLenoOVacio As String

    Private _FechaLiberacion As String

    Private _FechaEntrega As String

    Private _FechaCartaVacio As String

    Private _FechaDesconsolidacion As String

    Private _DesconsolidacionStatus As String

    Private _Imprimir As String

    Private _sentenciaInsert As String

    Private _sentenciaUpdate As String

    Private _PuedeActualizar As Boolean

    Private _PuedeInsertar As Boolean

    Private _ioperaciones As OperacionesCatalogo

    Private _sistema As Organismo

#End Region

#Region "Propiedades"

    Property IdControlContenedorDetalle As String

        Get

            Return _IdControlContenedorDetalle

        End Get

        Set(value As String)

            _IdControlContenedorDetalle = value

        End Set

    End Property

    Property IdControlContenedor As String

        Get

            Return _IdControlContenedor

        End Get

        Set(value As String)

            _IdControlContenedor = value

        End Set

    End Property

    Property IdRevalidacionBLContenedor As String

        Get

            Return _IdRevalidacionBLContenedor

        End Get

        Set(value As String)

            _IdRevalidacionBLContenedor = value

        End Set

    End Property

    Property LLenoOVacio As String

        Get

            Return _LLenoOVacio

        End Get

        Set(value As String)

            _LLenoOVacio = value

        End Set

    End Property

    Property FechaLiberacion As String

        Get

            Return _FechaLiberacion

        End Get

        Set(value As String)

            _FechaLiberacion = value

        End Set

    End Property

    Property FechaEntrega As String

        Get

            Return _FechaEntrega

        End Get

        Set(value As String)

            If value Is Nothing Then

                _FechaEntrega = value

            Else

                Dim fecha As DateTime = Convert.ToDateTime(value.Replace("p.m.", "PM").Replace("a.m.", "AM"))

                _FechaEntrega = fecha.ToString("dd/MM/yyyy HH:mm:ss")

            End If

        End Set

    End Property

    Property FechaCartaVacio As String

        Get

            Return _FechaCartaVacio

        End Get

        Set(value As String)

            If value Is Nothing Then

                _FechaCartaVacio = value

            Else

                Dim fecha As DateTime = Convert.ToDateTime(value.Replace("p.m.", "PM").Replace("a.m.", "AM"))

                _FechaCartaVacio = fecha.ToString("dd/MM/yyyy HH:mm:ss")

            End If

        End Set

    End Property

    Property FechaDesconsolidacion As String

        Get

            Return _FechaDesconsolidacion

        End Get

        Set(value As String)

            _FechaDesconsolidacion = value

        End Set

    End Property

    Property DesconsolidacionStatus As String

        Get

            Return _DesconsolidacionStatus

        End Get

        Set(value As String)

            _DesconsolidacionStatus = value

        End Set

    End Property

    Property Imprimir As String

        Get

            Return _Imprimir

        End Get

        Set(value As String)

            _Imprimir = value

        End Set

    End Property

    Property SentenciaInsert As String

        Get
            _sentenciaInsert = "INSERT INTO [SysExpert].[dbo].[ControlContenedoresDetalle] " &
                                                   "([IdControlContenedorDetalle] " &
                                                   ",[IdControlContenedor] " &
                                                   ",[IdRevalidacionBLContenedor] " &
                                                   ",[LLenoOVacio] " &
                                                   ",[FechaLiberacion] " &
                                                   ",[FechaEntrega] " &
                                                   ",[FechaCartaVacio] " &
                                                   ",[FechaDesconsolidacion] " &
                                                   ",[DesconsolidacionStatus] " &
                                                   ",[Imprimir]) " &
                                            "VALUES " &
                                                    "(" & obtenerIdNuevo() & ", " &
                                                   "" & (IIf(_IdControlContenedor Is Nothing, "NULL", _IdControlContenedor)) & ", " &
                                                   "" & (IIf(_IdRevalidacionBLContenedor Is Nothing, "NULL", _IdRevalidacionBLContenedor)) & ", " &
                                                   "" & (IIf(_LLenoOVacio Is Nothing, "NULL", _LLenoOVacio)) & ", " &
                                                   "" & (IIf(_FechaLiberacion Is Nothing, "NULL", _FechaLiberacion)) & ", " &
                                                   "" & (IIf(_FechaEntrega Is Nothing, "NULL", "'" + _FechaEntrega + "'")) & ", " &
                                                   "" & (IIf(_FechaCartaVacio Is Nothing, "NULL", "'" + _FechaCartaVacio + "'")) & ", " &
                                                   "" & (IIf(_FechaDesconsolidacion Is Nothing, "NULL", _FechaDesconsolidacion)) & ", " &
                                                   "" & (IIf(_DesconsolidacionStatus Is Nothing, "NULL", _DesconsolidacionStatus)) & ", " &
                                                   "" & (IIf(_Imprimir Is Nothing, "NULL", _Imprimir)) & ")"

            Return _sentenciaInsert

        End Get

        Set(value As String)

            _sentenciaInsert = value

        End Set

    End Property

    Property SentenciaUpdate As String

        Get
            _sentenciaUpdate = "UPDATE [SysExpert].[dbo].[ControlContenedoresDetalle] " &
                   "SET " &
                   "[LLenoOVacio]  = " & (IIf(_LLenoOVacio Is Nothing, "NULL", _LLenoOVacio)) & ", " &
                   "[FechaLiberacion]  = " & (IIf(_FechaLiberacion Is Nothing, "NULL", _FechaLiberacion)) & ", " &
                   "[FechaEntrega]  = " & (IIf(_FechaEntrega Is Nothing, "NULL", "'" + _FechaEntrega + "'")) & ", " &
                   "[FechaCartaVacio]  = " & (IIf(_FechaCartaVacio Is Nothing, "NULL", "'" + _FechaCartaVacio + "'")) & ", " &
                   "[FechaDesconsolidacion]  = " & (IIf(_FechaDesconsolidacion Is Nothing, "NULL", _FechaDesconsolidacion)) & ", " &
                   "[Imprimir]  = " & (IIf(_Imprimir Is Nothing, "NULL", _Imprimir)) & " " &
                   "WHERE IdControlContenedorDetalle = " & _IdControlContenedorDetalle & " and IdControlContenedor = " & _IdControlContenedor & " and IdRevalidacionBLContenedor = " & _IdRevalidacionBLContenedor

            Return _sentenciaUpdate

        End Get

        Set(value As String)

            _sentenciaUpdate = value

        End Set

    End Property

    Property PuedeActualizar As Boolean

        Get

            Return _PuedeActualizar

        End Get

        Set(value As Boolean)

            _PuedeActualizar = value

        End Set

    End Property

    Property PuedeInsertar As Boolean

        Get

            Return _PuedeInsertar

        End Get

        Set(value As Boolean)

            _PuedeInsertar = value

        End Set

    End Property

#End Region

#Region "Contructores"

    Sub New(ByVal ioperaciones_ As OperacionesCatalogo)

        _ioperaciones = ioperaciones_

        _sistema = New Organismo

    End Sub

#End Region

#Region "Metodos"

    Private Function obtenerIdNuevo() As Integer

        Dim _sql = "USE SysExpert; DECLARE @idNuevo INT EXEC getsemilla 'ControlContenedoresDetalle',1,@idNuevo OUTPUT  select @idNuevo  USE SOLIUM;"

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

        If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                    If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                        _IdControlContenedorDetalle = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)

                        Return _IdControlContenedorDetalle

                    End If

                End If

            End If

        End If

        Return _IdControlContenedorDetalle

    End Function

#End Region

End Class
