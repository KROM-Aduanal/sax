Imports Gsol.BaseDatos.Operaciones
Imports Gsol

Public Class RevalidacionBLContenedor

#Region "Atributos"

    Private _IdRevalidacionBLContenedor As String

    Private _IdRevalidacionBL As String

    Private _IdContenedor As String

    Private _Cantidad As String

    Private _Peso As String

    Private _FechaLiberacion As String

    Private _NumeroContenedor As String

    Private _IdContenedorClase As String

    Private _IdContenedorTipo As String

    Private _Pies As String

    Private _IdContenedorRevalidacion As String

    Private _sentenciaInsert As String

    Private _sentenciaUpdate As String

    Private _PuedeActualizar As Boolean

    Private _PuedeInsertar As Boolean

    Private _ioperaciones As OperacionesCatalogo

    Private _sistema As Organismo

#End Region

#Region "Propiedades"

    Property IdRevalidacionBLContenedor As String

        Get

            Return _IdRevalidacionBLContenedor

        End Get

        Set(value As String)

            _IdRevalidacionBLContenedor = value

        End Set

    End Property

    Property IdRevalidacionBL As String

        Get

            Return _IdRevalidacionBL

        End Get

        Set(value As String)

            _IdRevalidacionBL = value

        End Set

    End Property

    Property IdContenedor As String

        Get

            Return _IdContenedor

        End Get

        Set(value As String)

            _IdContenedor = value

        End Set

    End Property

    Property Cantidad As String

        Get

            Return _Cantidad

        End Get

        Set(value As String)

            _Cantidad = value

        End Set

    End Property

    Property Peso As String

        Get

            Return _Peso

        End Get

        Set(value As String)

            _Peso = value

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

    Property NumeroContenedor As String

        Get

            Return _NumeroContenedor

        End Get

        Set(value As String)

            _NumeroContenedor = value

        End Set

    End Property

    Property IdContenedorClase As String

        Get

            Return _IdContenedorClase

        End Get

        Set(value As String)

            _IdContenedorClase = value

        End Set

    End Property

    Property IdContenedorTipo As String

        Get

            Return _IdContenedorTipo

        End Get

        Set(value As String)

            _IdContenedorTipo = value

        End Set

    End Property

    Property Pies As String

        Get

            Return _Pies

        End Get

        Set(value As String)

            _Pies = value

        End Set

    End Property

    Property IdContenedorRevalidacion As String

        Get

            Return _IdContenedorRevalidacion

        End Get

        Set(value As String)

            _IdContenedorRevalidacion = value

        End Set

    End Property

    Property SentenciaInsert As String

        Get

            _sentenciaInsert = "INSERT INTO [SysExpert].[dbo].[RevalidacionBLContenedor] " &
                               "([IdRevalidacionBLContenedor]" &
                               ",[IdRevalidacionBL] " &
                               ",[IdContenedor] " &
                               ",[Cantidad] " &
                               ",[Peso] " &
                               ",[FechaLiberacion] " &
                               ",[NumeroContenedor] " &
                               ",[IdContenedorClase] " &
                               ",[IdContenedorTipo] " &
                               ",[Pies] " &
                               ",[IdContenedorRevalidacion]) " &
                               " VALUES  " &
                               "(" & obtenerIdNuevo() & ", " &
                               "" & (IIf(_IdRevalidacionBL Is Nothing, "NULL", _IdRevalidacionBL)) & ", " &
                               "" & (IIf(_IdContenedor Is Nothing, "NULL", _IdContenedor)) & ", " &
                               "" & (IIf(_Cantidad Is Nothing, "NULL", "'" & _Cantidad & "'")) & ", " &
                               "" & (IIf(_Peso Is Nothing, "NULL", "'" & _Peso & "'")) & ", " &
                               "" & (IIf(_FechaLiberacion Is Nothing, "NULL", "'" & _FechaLiberacion & "'")) & ", " &
                               "" & (IIf(_NumeroContenedor Is Nothing, "NULL", "'" & _NumeroContenedor & "'")) & ", " &
                               "" & (IIf(_IdContenedorClase Is Nothing, "NULL", _IdContenedorClase)) & ", " &
                               "" & (IIf(_IdContenedorTipo Is Nothing, "NULL", _IdContenedorTipo)) & ", " &
                               "" & (IIf(_Pies Is Nothing, "NULL", "'" & _Pies & "'")) & ", " &
                               "" & (IIf(_IdContenedorRevalidacion Is Nothing, "NULL", _IdContenedorRevalidacion)) & ")"

            Return _sentenciaInsert

        End Get

        Set(value As String)

            _sentenciaInsert = value

        End Set

    End Property

    Property SentenciaUpdate As String

        Get
            _sentenciaUpdate = "update [SysExpert].[dbo].[RevalidacionBLContenedor] " &
                   "SET [IdContenedor]  = " & (IIf(_IdContenedor Is Nothing, "NULL", _IdContenedor)) & ", " &
                   "[Cantidad]  = " & (IIf(_Cantidad Is Nothing, "NULL", "'" & _Cantidad & "'")) & ", " &
                   "[Peso]  = " & (IIf(_Peso Is Nothing, "NULL", "'" & _Peso & "'")) & ", " &
                   "[FechaLiberacion]  = " & (IIf(_Cantidad Is Nothing, "NULL", "'" & _FechaLiberacion & "'")) & ", " &
                   "[NumeroContenedor]  = " & (IIf(_Cantidad Is Nothing, "NULL", "'" & _NumeroContenedor & "'")) & ", " &
                   "[IdContenedorClase]  = " & (IIf(_IdContenedorClase Is Nothing, "NULL", _IdContenedorClase)) & ", " &
                   "[IdContenedorTipo]  = " & (IIf(_IdContenedorTipo Is Nothing, "NULL", _IdContenedorTipo)) & ", " &
                   "[Pies]  = " & (IIf(_Pies Is Nothing, "NULL", "'" & _Pies & "'")) & ", " &
                   "[IdContenedorRevalidacion]  = " & (IIf(_IdContenedorRevalidacion Is Nothing, "NULL", "'" & _IdContenedorRevalidacion & "'")) & " " &
                   "WHERE IdRevalidacionBLContenedor = " & _IdRevalidacionBLContenedor & " and IdRevalidacionBL = " & _IdRevalidacionBL

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

        Dim _sql = "USE SysExpert; DECLARE @idNuevo INT EXEC getsemilla 'RevalidacionBLContenedor',1,@idNuevo OUTPUT  select @idNuevo  USE SOLIUM;"

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

        If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                    If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                        _IdRevalidacionBLContenedor = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)

                        Return _IdRevalidacionBLContenedor

                    End If

                End If

            End If

        End If

        Return _IdRevalidacionBLContenedor

    End Function

#End Region



End Class
