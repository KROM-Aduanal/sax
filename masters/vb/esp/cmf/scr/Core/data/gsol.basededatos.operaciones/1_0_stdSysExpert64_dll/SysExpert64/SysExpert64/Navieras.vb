Imports Gsol.BaseDatos.Operaciones
Imports Gsol

Public Class Navieras

#Region "Atributos"

    Private _IdNaviera As String

    Private _CAAT As String

    Private _Naviera As String

    Private _sentenciaInsert As String

    Private _sentenciaUpdate As String

    Private _PuedeActualizar As Boolean

    Private _PuedeInsertar As Boolean

    Private _ioperaciones As OperacionesCatalogo

    Private _sistema As Organismo

#End Region

#Region "Propiedades"

    Property IdNaviera As String

        Get

            Return _IdNaviera

        End Get

        Set(value As String)

            _IdNaviera = value

        End Set

    End Property

    Property CAAT As String

        Get

            Return _CAAT

        End Get

        Set(value As String)

            _CAAT = value

        End Set

    End Property

    Property Naviera As String

        Get

            Return _Naviera

        End Get

        Set(value As String)

            _Naviera = value

        End Set

    End Property

    Property SentenciaInsert As String

        Get

            _sentenciaInsert = "INSERT INTO [SysExpert].[dbo].[Navieras] " &
                               "([IdNaviera] " &
                               ",[CAAT] " &
                               ",[Naviera] )" &
                               " VALUES  " &
                               "(" & obtenerIdNuevo() & ", " &
                               "" & (IIf(_CAAT Is Nothing, "NULL", "'" & _CAAT & "'")) & ", " &
                               "" & (IIf(_Naviera Is Nothing, "NULL", "'" & _Naviera & "'")) & ")"

            Return _sentenciaInsert

        End Get

        Set(value As String)

            _sentenciaInsert = value

        End Set

    End Property

    Property SentenciaUpdate As String

        Get
            _sentenciaUpdate = "update [SysExpert].[dbo].[Navieras] " &
                   "SET [CAAT]  = '" & (IIf(_CAAT Is Nothing, "NULL", _CAAT)) & "', " &
                   "[Naviera]  = '" & (IIf(_Naviera Is Nothing, "NULL", _Naviera)) & "' " &
                   "WHERE Naviera = '" & _Naviera & "'"


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

        Dim _sql = "USE SysExpert; DECLARE @idNuevo INT EXEC getsemilla 'Navieras',1,@idNuevo OUTPUT  select @idNuevo  USE SOLIUM;"

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

        If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                    If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                        _IdNaviera = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)

                        Return _IdNaviera

                    End If

                End If

            End If

        End If

        Return _IdNaviera

    End Function

#End Region

End Class

