Imports Gsol.BaseDatos.Operaciones
Imports Gsol

Public Class Reexpedidoras

#Region "Atributos"

    Private _IdReexpedidora As String

    Private _IdAgencia As String

    Private _ClaveReexpedidora As String

    Private _NombreReexpedidora As String

    Private _Domicilio As String

    Private _CodigoPostal As String

    Private _Telefono As String

    Private _RFC As String

    Private _ClaveForanea As String

    Private _sentenciaInsert As String

    Private _sentenciaUpdate As String

    Private _PuedeActualizar As Boolean

    Private _PuedeInsertar As Boolean

    Private _ioperaciones As OperacionesCatalogo

    Private _sistema As Organismo

#End Region

#Region "Propiedades"

    Property IdReexpedidora As String

        Get

            Return _IdReexpedidora

        End Get

        Set(value As String)

            _IdReexpedidora = value

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

    Property ClaveReexpedidora As String

        Get

            Return _ClaveReexpedidora

        End Get

        Set(value As String)

            _ClaveReexpedidora = value

        End Set

    End Property

    Property NombreReexpedidora As String

        Get

            Return _NombreReexpedidora

        End Get

        Set(value As String)

            _NombreReexpedidora = value

        End Set

    End Property

    Property Domicilio As String

        Get

            Return _Domicilio

        End Get

        Set(value As String)

            _Domicilio = value

        End Set

    End Property

    Property CodigoPostal As String

        Get

            Return _CodigoPostal

        End Get

        Set(value As String)

            _CodigoPostal = value

        End Set

    End Property

    Property Telefono As String

        Get

            Return _Telefono

        End Get

        Set(value As String)

            _Telefono = value

        End Set

    End Property

    Property RFC As String

        Get

            Return _RFC

        End Get

        Set(value As String)

            _RFC = value

        End Set

    End Property

    Property ClaveForanea As String

        Get

            Return _ClaveForanea

        End Get

        Set(value As String)

            _ClaveForanea = value

        End Set

    End Property

    Property SentenciaInsert As String

        Get

            _sentenciaInsert = "INSERT INTO [SysExpert].[dbo].[Reexpedidoras] " &
                               "([IdReexpedidora]" &
                               ",[IdAgencia] " &
                               ",[ClaveReexpedidora]" &
                               ",[NombreReexpedidora]" &
                               ",[Domicilio]" &
                               ",[CodigoPostal]" &
                               ",[Telefono]" &
                               ",[RFC]" &
                               ",[ClaveForanea] )" &
                               " VALUES  " &
                               "(" & obtenerIdNuevo() & ", " &
                               "" & (IIf(_IdAgencia Is Nothing, "NULL", _IdAgencia)) & ", " &
                               "" & (IIf(_ClaveReexpedidora Is Nothing, "NULL", "'" & _ClaveReexpedidora & "'")) & ", " &
                               "" & (IIf(_NombreReexpedidora Is Nothing, "NULL", "'" & _NombreReexpedidora & "'")) & ", " &
                               "" & (IIf(_Domicilio Is Nothing, "NULL", "'" & _Domicilio & "'")) & ", " &
                               "" & (IIf(_CodigoPostal Is Nothing, "NULL", "'" & _CodigoPostal & "'")) & ", " &
                               "" & (IIf(_Telefono Is Nothing, "NULL", "'" & _Telefono & "'")) & ", " &
                               "" & (IIf(_RFC Is Nothing, "NULL", "'" & _RFC & "'")) & ", " &
                               "" & (IIf(_ClaveForanea Is Nothing, "NULL", "'" & _ClaveForanea & "'")) & ")"

            Return _sentenciaInsert

        End Get

        Set(value As String)

            _sentenciaInsert = value

        End Set

    End Property

    Property SentenciaUpdate As String

        Get
            _sentenciaUpdate = "update [SysExpert].[dbo].[Reexpedidoras] " &
                   "SET [IdAgencia]   = " & (IIf(_IdAgencia Is Nothing, "NULL", _IdAgencia)) & ", " &
                   "[ClaveReexpedidora]  = " & (IIf(_ClaveReexpedidora Is Nothing, "NULL", "'" & _ClaveReexpedidora & "'")) & ", " &
                   "[NombreReexpedidora]  = " & (IIf(_NombreReexpedidora Is Nothing, "NULL", "'" & _NombreReexpedidora & "'")) & ", " &
                   "[Domicilio]  = " & (IIf(_Domicilio Is Nothing, "NULL", "'" & _Domicilio & "'")) & ", " &
                   "[CodigoPostal] = " & (IIf(_CodigoPostal Is Nothing, "NULL", "'" & _CodigoPostal & "'")) & ", " &
                   "[Telefono]  = " & (IIf(_Telefono Is Nothing, "NULL", "'" & _Telefono & "'")) & ", " &
                   "[RFC]  = " & (IIf(_RFC Is Nothing, "NULL", "'" & _RFC & "'")) & ", " &
                   "[ClaveForanea]  = " & (IIf(_ClaveForanea Is Nothing, "NULL", "'" & _ClaveForanea & "'")) & " " &
                   "WHERE NombreReexpedidora = '" & _NombreReexpedidora & "'"

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

        Dim _sql = "USE SysExpert; DECLARE @idNuevo INT EXEC getsemilla 'Reexpedidoras',1,@idNuevo OUTPUT  select @idNuevo  USE SOLIUM;"

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

        If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                    If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                        _IdReexpedidora = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)

                        Return _IdReexpedidora

                    End If

                End If

            End If

        End If

        Return _IdReexpedidora

    End Function

#End Region



End Class