Imports Gsol.BaseDatos.Operaciones
Imports Gsol
Public Class ControlContenedores

#Region "Atributos"

    Private _IdControlContenedor As String

    Private _IdReferencia As String

    Private _FechaEntrada As String

    Private _sentenciaInsert As String

    Private _sentenciaUpdate As String

    Private _PuedeActualizar As Boolean

    Private _PuedeInsertar As Boolean

    Private _ioperaciones As OperacionesCatalogo

    Private _sistema As Organismo

#End Region

#Region "Propiedades"

    Property IdControlContenedor As String

        Get

            Return _IdControlContenedor

        End Get

        Set(value As String)

            _IdControlContenedor = value

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

    Property FechaEntrada As String

        Get

            Return _FechaEntrada

        End Get

        Set(value As String)

            _FechaEntrada = value

        End Set

    End Property

    Property SentenciaInsert As String

        Get
            _sentenciaInsert = "INSERT INTO [SysExpert].[dbo].[ControlContenedores] " &
                                       "([IdControlContenedor] " &
                                       ",[IdReferencia] " &
                                       ",[FechaEntrada]) " &
                                "VALUES " &
                                       "(" & obtenerIdNuevo() & ", " &
                                       "" & (IIf(_IdReferencia Is Nothing, "NULL", _IdReferencia)) & ", " &
                                       "" & (IIf(_FechaEntrada Is Nothing, "NULL", "'" + _FechaEntrada + "'")) & ")"
            Return _sentenciaInsert

        End Get

        Set(value As String)

            _sentenciaInsert = value

        End Set

    End Property

    Property SentenciaUpdate As String

        Get
            _sentenciaUpdate = "UPDATE [SysExpert].[dbo].[ControlContenedores] " &
                   "SET " &
                   "[FechaEntrada]  = " & (IIf(_FechaEntrada Is Nothing, "NULL", _FechaEntrada)) & ", " &
                   "[IdReferencia]  = " & (IIf(_IdReferencia Is Nothing, "NULL", _IdReferencia)) & " " &
                   "WHERE IdControlContenedor = " & _IdControlContenedor

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

        Dim _sql = "USE SysExpert; DECLARE @idNuevo INT EXEC getsemilla 'ControlContenedores',1,@idNuevo OUTPUT  select @idNuevo  USE SOLIUM;"

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

        If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                    If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                        _IdControlContenedor = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)

                        Return _IdControlContenedor

                    End If

                End If

            End If

        End If

        Return _IdControlContenedor

    End Function

#End Region

End Class
