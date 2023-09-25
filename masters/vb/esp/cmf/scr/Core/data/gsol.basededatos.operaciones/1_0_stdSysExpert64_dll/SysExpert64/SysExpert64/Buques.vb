Imports Gsol.BaseDatos.Operaciones
Imports Gsol

Public Class Buques

#Region "Atributos"

    Private _IdBuque As String

    Private _IdAgencia As String

    Private _ClaveBuque As String

    Private _Buque As String

    Private _sentenciaInsert As String

    Private _sentenciaUpdate As String

    Private _PuedeActualizar As Boolean

    Private _PuedeInsertar As Boolean

    Private _ioperaciones As OperacionesCatalogo

    Private _sistema As Organismo

#End Region

#Region "Propiedades"

    Property IdBuque As String

        Get

            Return _IdBuque

        End Get

        Set(value As String)

            _IdBuque = value

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

    Property ClaveBuque As String

        Get

            Return _ClaveBuque

        End Get

        Set(value As String)

            _ClaveBuque = value

        End Set

    End Property

    Property Buque As String

        Get

            Return _Buque

        End Get

        Set(value As String)

            _Buque = value

        End Set

    End Property

    Property SentenciaInsert As String

        Get

            _sentenciaInsert = "INSERT INTO [SysExpert].[dbo].[Buques] " &
                               "([IdBuque] " &
                               ",[IdAgencia] " &
                               ",[ClaveBuque] " &
                               ",[Buque] )" &
                               " VALUES  " &
                               "(" & obtenerIdNuevo() & ", " &
                               "" & (IIf(_IdAgencia Is Nothing, "NULL", _IdAgencia)) & ", " &
                               "" & (IIf(_ClaveBuque Is Nothing, "NULL", "'" & _ClaveBuque & "'")) & ", " &
                               "" & (IIf(_Buque Is Nothing, "NULL", "'" & _Buque & "'")) & ")"

            Return _sentenciaInsert

        End Get

        Set(value As String)

            _sentenciaInsert = value

        End Set

    End Property

    Property SentenciaUpdate As String

        Get
            _sentenciaUpdate = "update [SysExpert].[dbo].[Buques] " &
                   "SET [IdAgencia]  = " & (IIf(_IdAgencia Is Nothing, "NULL", _IdAgencia)) & ", " &
                   "[ClaveBuque]  = '" & (IIf(_ClaveBuque Is Nothing, "NULL", _ClaveBuque)) & "', " &
                   "[Buque]  = '" & (IIf(_Buque Is Nothing, "NULL", "'" & _Buque & "'")) & "' " &
                   "WHERE IdBuque = " & _IdBuque & " and IdAgencia = " & _IdAgencia

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

        Dim _sql = "USE SysExpert; DECLARE @idNuevo INT EXEC getsemilla 'Buques',1,@idNuevo OUTPUT  select @idNuevo  USE SOLIUM;"

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

        If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                    If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                        _IdBuque = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)

                        Return _IdBuque

                    End If

                End If

            End If

        End If

        Return _IdBuque

    End Function

#End Region



End Class

