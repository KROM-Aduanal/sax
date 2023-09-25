Imports Gsol.BaseDatos.Operaciones
Imports Gsol

Public Class RevalidacionBl

#Region "Atributos"

    Private _IdRevalidacionBL As String

    Private _IdAgencia As String

    Private _IdReferencia As String

    Private _Trasbordo As String

    Private _FechaETA As String

    Private _IdReexpedidora As String

    Private _IdPuerto As String

    Private _FechaEmbarque As String

    Private _FechaETALAX As String

    Private _ClaveCargaTipo As String

    Private _IdCargaClase As String

    Private _FechaRevalidacion As String

    Private _Observaciones As String

    Private _IdBuque As String

    Private _IdNaviera As String

    Private _sentenciaInsert As String

    Private _seInserto As Boolean

    Private _sentenciaUpdate As String

    Private _ioperaciones As OperacionesCatalogo

    Private _sistema As Organismo

    Private _PuedeActualizar As Boolean

    Private _PuedeInsertar As Boolean

    Private _ReexpedidoraKB As String

    Private _NavieraKB As String

    Private _CAATNavieraKB As String

    Private _NaveKB As String

    Private _ClavePuertoKB As String

    Private _NombrePuertoKB As String

    Private _RfcReexpedidoraKB As String

    Private _DomicilioReexpedidoraKB As String

    Private _i_Cve_RevalidacionImpoKB As String

    Private _EntradaRealKB As String

#End Region

#Region "Propiedades"

    Property IdRevalidacionBL As String

        Get

            Return _IdRevalidacionBL

        End Get

        Set(value As String)

            _IdRevalidacionBL = value

        End Set

    End Property

    Property IdAgencia As String

        Get

            Return _IdAgencia

        End Get

        Set(value As String)

            _IdAgencia = value

        End Set

    End Property

    Property IdReferencia As String

        Get

            Return _IdReferencia

        End Get

        Set(value As String)

            _IdReferencia = value

        End Set

    End Property

    Property Trasbordo As String

        Get

            Return _Trasbordo

        End Get

        Set(value As String)

            _Trasbordo = value

        End Set

    End Property

    Property FechaETA As String

        Get

            Return _FechaETA

        End Get

        Set(value As String)

            _FechaETA = value

        End Set

    End Property

    Property IdReexpedidora As String

        Get

            Return _IdReexpedidora

        End Get

        Set(value As String)

            _IdReexpedidora = value

        End Set

    End Property

    Property IdPuerto As String

        Get

            Return _IdPuerto

        End Get

        Set(value As String)

            _IdPuerto = value

        End Set

    End Property

    Property FechaEmbarque As String

        Get

            Return _FechaEmbarque

        End Get

        Set(value As String)

            _FechaEmbarque = value

        End Set

    End Property

    Property FechaETALAX As String

        Get

            Return _FechaETALAX

        End Get

        Set(value As String)

            _FechaETALAX = value

        End Set

    End Property

    Property ClaveCargaTipo As String

        Get

            Return _ClaveCargaTipo

        End Get

        Set(value As String)

            _ClaveCargaTipo = value

        End Set

    End Property

    Property IdCargaClase As String

        Get

            Return _IdCargaClase

        End Get

        Set(value As String)

            _IdCargaClase = value

        End Set

    End Property

    Property FechaRevalidacion As String

        Get

            Return _FechaRevalidacion

        End Get

        Set(value As String)

            If value Is Nothing Then

                _FechaRevalidacion = value

            Else

                Dim fecha As DateTime = Convert.ToDateTime(value.Replace("p.m.", "PM").Replace("a.m.", "AM"))

                _FechaRevalidacion = fecha.ToString("dd/MM/yyyy HH:mm:ss")

            End If

        End Set

    End Property

    Property Observaciones As String

        Get

            Return _Observaciones

        End Get

        Set(value As String)

            _Observaciones = value

        End Set

    End Property

    Property IdBuque As String

        Get

            Return _IdBuque

        End Get

        Set(value As String)

            _IdBuque = value

        End Set

    End Property

    Property IdNaviera As String

        Get

            Return _IdNaviera

        End Get

        Set(value As String)

            _IdNaviera = value

        End Set

    End Property

    Property SentenciaInsert As String

        Get

            _sentenciaInsert = "INSERT INTO [SysExpert].[dbo].[RevalidacionBL] " &
                               "([IdRevalidacionBL] " &
                               ",[IdAgencia] " &
                               ",[IdReferencia] " &
                               ",[Trasbordo] " &
                               ",[FechaETA] " &
                               ",[IdReexpedidora] " &
                               ",[IdPuerto] " &
                               ",[FechaEmbarque] " &
                               ",[FechaETALAX] " &
                               ",[ClaveCargaTipo] " &
                               ",[IdCargaClase] " &
                               ",[FechaRevalidacion] " &
                               ",[Observaciones] " &
                               ",[IdBuque] " &
                               ",[IdNaviera]) " &
                               " VALUES  " &
                               "(" & obtenerIdNuevo() & ", " &
                               "" & (IIf(_IdAgencia Is Nothing, "NULL", _IdAgencia)) & ", " &
                               "" & (IIf(_IdReferencia Is Nothing, "NULL", _IdReferencia)) & ", " &
                               "" & (IIf(_Trasbordo Is Nothing, "NULL", _Trasbordo)) & ", " &
                               "" & (IIf(_FechaETA Is Nothing, "NULL", "'" & _FechaETA & "'")) & ", " &
                               "" & (IIf(_IdReexpedidora Is Nothing, "NULL", _IdReexpedidora)) & ", " &
                               "" & (IIf(_IdPuerto Is Nothing, "NULL", _IdPuerto)) & ", " &
                               "" & (IIf(_FechaEmbarque Is Nothing, "NULL", "'" & _FechaEmbarque & "'")) & ", " &
                               "" & (IIf(_FechaETALAX Is Nothing, "NULL", "'" & _FechaETALAX & "'")) & ", " &
                               "" & (IIf(_ClaveCargaTipo Is Nothing, "NULL", "'" & _ClaveCargaTipo & "'")) & ", " &
                               "" & (IIf(_IdCargaClase Is Nothing, "NULL", _IdCargaClase)) & ", " &
                               "" & (IIf(_FechaRevalidacion Is Nothing, "NULL", "'" & _FechaRevalidacion & "'")) & ", " &
                               "" & (IIf(_Observaciones Is Nothing, "NULL", "'" & _Observaciones & "'")) & ", " &
                               "" & (IIf(_IdBuque Is Nothing, "NULL", _IdBuque)) & ", " &
                               "" & (IIf(_IdNaviera Is Nothing, "NULL", _IdNaviera)) & ")"

            Return _sentenciaInsert

        End Get

        Set(value As String)

            _sentenciaInsert = value

        End Set

    End Property

    Property SentenciaUpdate As String

        Get
            _sentenciaUpdate = "update [SysExpert].[dbo].[RevalidacionBL] " &
                   "SET [IdAgencia] = " & (IIf(_IdAgencia Is Nothing, "NULL", _IdAgencia)) & ", " &
                   "[Trasbordo]  = " & (IIf(_Trasbordo Is Nothing, "NULL", _Trasbordo)) & ", " &
                   "[FechaETA]  = " & (IIf(_FechaETA Is Nothing, "NULL", "'" & _FechaETA & "'")) & ", " &
                   "[IdReexpedidora]  = " & (IIf(_IdReexpedidora Is Nothing, "NULL", _IdReexpedidora)) & ", " &
                   "[IdPuerto]  = " & (IIf(_IdPuerto Is Nothing, "NULL", _IdPuerto)) & ", " &
                   "[FechaEmbarque]  = " & (IIf(_FechaEmbarque Is Nothing, "NULL", "'" & _FechaEmbarque & "'")) & ", " &
                   "[FechaETALAX]  = " & (IIf(_FechaETALAX Is Nothing, "NULL", "'" & _FechaETALAX & "'")) & ", " &
                   "[ClaveCargaTipo]  = " & (IIf(_ClaveCargaTipo Is Nothing, "NULL", "'" & _ClaveCargaTipo & "'")) & ", " &
                   "[IdCargaClase]  = " & (IIf(_IdCargaClase Is Nothing, "NULL", _IdCargaClase)) & ", " &
                   "[FechaRevalidacion]  = " & (IIf(_FechaRevalidacion Is Nothing, "NULL", "'" & _FechaRevalidacion & "'")) & ", " &
                   "[Observaciones]  = " & (IIf(_Observaciones Is Nothing, "NULL", "'" & _Observaciones & "'")) & ", " &
                   "[IdBuque]  = " & (IIf(_IdBuque Is Nothing, "NULL", _IdBuque)) & ", " &
                   "[IdNaviera]  = " & (IIf(_IdNaviera Is Nothing, "NULL", _IdNaviera)) & " " &
                   "WHERE IdRevalidacionBL = " & _IdRevalidacionBL & " and IdReferencia = " & _IdReferencia

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

    Property ReexpedidoraKB As String

        Get

            Return _ReexpedidoraKB


            Return Nothing

        End Get

        Set(value As String)

            _ReexpedidoraKB = value

        End Set

    End Property

    Property NavieraKB As String

        Get

            Return _NavieraKB


            Return Nothing

        End Get

        Set(value As String)

            _NavieraKB = value

        End Set

    End Property

    Property NaveKB As String

        Get

            Return _NaveKB


            Return Nothing

        End Get

        Set(value As String)

            _NaveKB = value

        End Set

    End Property

    Property ClavePuertoKB As String

        Get

            Return _ClavePuertoKB


            Return Nothing

        End Get

        Set(value As String)

            _ClavePuertoKB = value

        End Set

    End Property

    Property NombrePuertoKB As String

        Get

            Return _NombrePuertoKB


            Return Nothing

        End Get

        Set(value As String)

            _NombrePuertoKB = value

        End Set

    End Property

    Property RfcReexpedidoraKB As String

        Get

            Return _RfcReexpedidoraKB


            Return Nothing

        End Get

        Set(value As String)

            _RfcReexpedidoraKB = value

        End Set

    End Property

    Property DomicilioReexpedidoraKB As String

        Get

            Return _DomicilioReexpedidoraKB


            Return Nothing

        End Get

        Set(value As String)

            _DomicilioReexpedidoraKB = value

        End Set

    End Property

    Property i_Cve_RevalidacionImpoKB As String

        Get

            Return _i_Cve_RevalidacionImpoKB


            Return Nothing

        End Get

        Set(value As String)

            _i_Cve_RevalidacionImpoKB = value

        End Set

    End Property

    Property seInserto As Boolean

        Get

            Return _seInserto


            Return Nothing

        End Get

        Set(value As Boolean)

            _seInserto = value

        End Set

    End Property

    Property CAATNavieraKB As String

        Get

            Return _CAATNavieraKB


            Return Nothing

        End Get

        Set(value As String)

            _CAATNavieraKB = value

        End Set

    End Property

    Property EntradaRealKB As String

        Get

            Return _EntradaRealKB


            Return Nothing

        End Get

        Set(value As String)

            _EntradaRealKB = value

        End Set

    End Property



#End Region

#Region "Contructores"

    Sub New(ByVal ioperaciones_ As OperacionesCatalogo)

        _ioperaciones = ioperaciones_

        _sistema = New Organismo

        _seInserto = False

    End Sub


#End Region

#Region "Metodos"

    Private Function obtenerIdNuevo() As Integer

        Dim _sql = "USE SysExpert; DECLARE @idNuevo INT EXEC getsemilla 'Revalidacionbl',1,@idNuevo OUTPUT  select @idNuevo  USE SOLIUM;"

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

        If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                    If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                        _IdRevalidacionBL = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)

                        Return _IdRevalidacionBL

                    End If

                End If

            End If

        End If

        Return _IdRevalidacionBL

    End Function

#End Region



End Class
