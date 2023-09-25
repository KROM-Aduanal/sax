Imports Gsol.BaseDatos.Operaciones
Imports Gsol

Public Class Trafico

#Region "Atributos"

    Private _idTrafico As String

    Private _idSituacion As String

    Private _idReferencia As String

    Private _idFactura As String

    Private _idGuia As String

    Private _FechaMovimiento As String

    Private _idUsuario As String

    Private _Dato As String

    Private _ProcesadoWeb As String

    Private _Observaciones As String

    Private _NumeroReferencia As String

    Private _GuiaMaster As String

    Private _GuiaHouse As String

    Private _NumeroFactura As String

    Private _idAgencia As String

    Private _HoraMovimiento As String

    Private _idRemesa As String

    Private _NumeroRemesa As String

    Private _Cancelado As String

    Private _FechaCancelacion As String

    Private _FechaAlta As String

    Private _sentenciaInsert As String

    Private _sentenciaUpdate As String

    Private _PuedeActualizar As Boolean

    Private _PuedeInsertar As Boolean

    Private _ioperaciones As OperacionesCatalogo

    Private _sistema As Organismo

#End Region

#Region "Propiedades"

    Property idTrafico As String

        Get

            Return _idTrafico

        End Get

        Set(value As String)

            _idTrafico = value

        End Set

    End Property

    Property idSituacion As String

        Get

            Return _idSituacion

        End Get

        Set(value As String)

            _idSituacion = value

        End Set

    End Property

    Property idReferencia As String

        Get

            Return _idReferencia

        End Get

        Set(value As String)

            _idReferencia = value

        End Set

    End Property

    Property idFactura As String

        Get

            Return _idFactura

        End Get

        Set(value As String)

            _idFactura = value

        End Set

    End Property

    Property idGuia As String

        Get

            Return _idGuia

        End Get

        Set(value As String)

            _idGuia = value

        End Set

    End Property

    Property FechaMovimiento As String

        Get

            Return _FechaMovimiento

        End Get

        Set(value As String)

            _FechaMovimiento = value

        End Set

    End Property

    Property idUsuario As String

        Get

            Return _idUsuario

        End Get

        Set(value As String)

            _idUsuario = value

        End Set

    End Property

    Property Dato As String

        Get

            Return _Dato

        End Get

        Set(value As String)

            _Dato = value

        End Set

    End Property

    Property ProcesadoWeb As String

        Get

            Return _ProcesadoWeb

        End Get

        Set(value As String)

            _ProcesadoWeb = value

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

    Property NumeroReferencia As String

        Get

            Return _NumeroReferencia

        End Get

        Set(value As String)

            _NumeroReferencia = value

        End Set

    End Property

    Property GuiaMaster As String

        Get

            Return _GuiaMaster

        End Get

        Set(value As String)

            _GuiaMaster = value

        End Set

    End Property

    Property GuiaHouse As String

        Get

            Return _GuiaHouse

        End Get

        Set(value As String)

            _GuiaHouse = value

        End Set

    End Property

    Property NumeroFactura As String

        Get

            Return _NumeroFactura

        End Get

        Set(value As String)

            _NumeroFactura = value

        End Set

    End Property

    Property idAgencia As String

        Get

            Return _idAgencia

        End Get

        Set(value As String)

            Select Case value

                Case 1 : _idSituacion = 8541
                Case 2 : _idSituacion = 8538
                Case 3 : _idSituacion = 8539
                Case 4 : _idSituacion = 8540
                Case 5 : _idSituacion = 8546
                Case 6 : _idSituacion = 8547
                Case 7 : _idSituacion = 8548
                Case 8 : _idSituacion = 8549
                Case 12 : _idSituacion = 8550
                Case 13 : _idSituacion = 8551
                Case 14 : _idSituacion = 8552
                Case 15 : _idSituacion = 8553
                Case 16 : _idSituacion = 8554
                Case 17 : _idSituacion = 8555
                Case 18 : _idSituacion = 8556
                Case 19 : _idSituacion = 8557
                Case 20 : _idSituacion = 8558
                Case 21 : _idSituacion = 8559
                Case 26 : _idSituacion = 8560
                Case 31 : _idSituacion = 8561
                Case 36 : _idSituacion = 9162

            End Select

            _idAgencia = value

        End Set

    End Property

    Property HoraMovimiento As String

        Get

            Return _HoraMovimiento

        End Get

        Set(value As String)

            _HoraMovimiento = value

        End Set

    End Property

    Property idRemesa As String

        Get

            Return _idRemesa

        End Get

        Set(value As String)

            _idRemesa = value

        End Set

    End Property

    Property NumeroRemesa As String

        Get

            Return _NumeroRemesa

        End Get

        Set(value As String)

            _NumeroRemesa = value

        End Set

    End Property

    Property Cancelado As String

        Get

            Return _Cancelado

        End Get

        Set(value As String)

            _Cancelado = value

        End Set

    End Property

    Property FechaCancelacion As String

        Get

            Return _FechaCancelacion

        End Get

        Set(value As String)

            _FechaCancelacion = value

        End Set

    End Property

    Property FechaAlta As String

        Get

            Return _FechaAlta

        End Get

        Set(value As String)

            _FechaAlta = value

        End Set

    End Property

    Property SentenciaInsert As String

        Get

            _sentenciaInsert = "INSERT INTO [SysExpert].[dbo].[Trafico] " &
                               "([idTrafico] " &
                               ",[idSituacion] " &
                               ",[idReferencia] " &
                               ",[idFactura] " &
                               ",[idGuia] " &
                               ",[FechaMovimiento] " &
                               ",[idUsuario] " &
                               ",[Dato] " &
                               ",[ProcesadoWeb] " &
                               ",[Observaciones] " &
                               ",[NumeroReferencia] " &
                               ",[GuiaMaster] " &
                               ",[GuiaHouse] " &
                               ",[NumeroFactura] " &
                               ",[idAgencia] " &
                               ",[HoraMovimiento] " &
                               ",[idRemesa] " &
                               ",[NumeroRemesa] " &
                               ",[Cancelado] " &
                               ",[FechaCancelacion] " &
                               ",[FechaAlta] )" &
                               " VALUES  " &
                               "(" & obtenerIdNuevo() & ", " &
                               "" & (IIf(_idSituacion Is Nothing, "NULL", _idSituacion)) & ", " &
                               "" & (IIf(_idReferencia Is Nothing, "NULL", _idReferencia)) & ", " &
                               "" & (IIf(_idFactura Is Nothing, "NULL", _idFactura)) & ", " &
                               "" & (IIf(_idGuia Is Nothing, "NULL", _idGuia)) & ", " &
                               "" & (IIf(_FechaMovimiento Is Nothing, "NULL", "'" & _FechaMovimiento & "'")) & ", " &
                               "" & (IIf(_idUsuario Is Nothing, "NULL", _idUsuario)) & ", " &
                               "" & (IIf(_Dato Is Nothing, "NULL", "'" & _Dato & "'")) & ", " &
                               "" & (IIf(_ProcesadoWeb Is Nothing, "NULL", _ProcesadoWeb)) & ", " &
                               "" & (IIf(_Observaciones Is Nothing, "NULL", "'" & _Observaciones & "'")) & ", " &
                               "" & (IIf(_NumeroReferencia Is Nothing, "NULL", "'" & _NumeroReferencia & "'")) & ", " &
                               "" & (IIf(_GuiaMaster Is Nothing, "NULL", "'" & _GuiaMaster & "'")) & ", " &
                               "" & (IIf(_GuiaHouse Is Nothing, "NULL", "'" & _GuiaHouse & "'")) & ", " &
                               "" & (IIf(_NumeroFactura Is Nothing, "NULL", "'" & _NumeroFactura & "'")) & ", " &
                               "" & (IIf(_idAgencia Is Nothing, "NULL", _idAgencia)) & ", " &
                               "" & (IIf(_HoraMovimiento Is Nothing, "NULL", "'" & _HoraMovimiento & "'")) & ", " &
                               "" & (IIf(_idRemesa Is Nothing, "NULL", _idRemesa)) & ", " &
                               "" & (IIf(_NumeroRemesa Is Nothing, "NULL", _NumeroRemesa)) & ", " &
                               "" & (IIf(_Cancelado Is Nothing, "NULL", _Cancelado)) & ", " &
                               "" & (IIf(_FechaCancelacion Is Nothing, "NULL", "'" & _FechaCancelacion & "'")) & ", " &
                               "" & (IIf(_FechaAlta Is Nothing, "NULL", "'" & _FechaAlta & "'")) & ") "

            Return _sentenciaInsert

        End Get

        Set(value As String)

            _sentenciaInsert = value

        End Set

    End Property

    Property SentenciaUpdate As String

        Get

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

        Dim _sql = "USE SysExpert; DECLARE @idNuevo INT EXEC getsemilla 'Trafico',1,@idNuevo OUTPUT  select @idNuevo  USE SOLIUM;"

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

        If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                    If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                        _idTrafico = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)

                        Return _idTrafico

                    End If

                End If

            End If

        End If

        Return _idTrafico

    End Function

#End Region



End Class