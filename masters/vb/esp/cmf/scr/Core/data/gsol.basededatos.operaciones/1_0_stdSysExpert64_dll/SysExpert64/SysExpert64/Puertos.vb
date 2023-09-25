Imports Gsol.BaseDatos.Operaciones
Imports Gsol

Public Class Puertos

#Region "Atributos"

    Private _IdPuerto As String

    Private _IdPais As String

    Private _Clave As String

    Private _Puerto As String

    Private _sentenciaInsert As String

    Private _sentenciaUpdate As String

    Private _PuedeActualizar As Boolean

    Private _PuedeInsertar As Boolean

    Private _ioperaciones As OperacionesCatalogo

    Private _sistema As Organismo

#End Region

#Region "Propiedades"

    Property IdPuerto As String

        Get

            Return _IdPuerto

        End Get

        Set(value As String)

            _IdPuerto = value

        End Set

    End Property

    Property IdPais As String

        Get

            Return _IdPais

        End Get

        Set(value As String)

            _IdPais = value

        End Set

    End Property

    Property Clave As String

        Get

            Return _Clave

        End Get

        Set(value As String)

            _Clave = value

        End Set

    End Property

    Property Puerto As String

        Get

            Return _Puerto

        End Get

        Set(value As String)

            _Puerto = value

        End Set

    End Property

    Property SentenciaInsert As String

        Get

            _sentenciaInsert = "INSERT INTO [SysExpert].[dbo].[Puertos] " &
                               "([idPuerto] " &
                               ",[idPais] " &
                               ",[Clave] " &
                               ",[Puerto] )" &
                               " VALUES  " &
                               "(" & obtenerIdNuevo() & ", " &
                               "" & (IIf(_IdPais Is Nothing, "NULL", _IdPais)) & ", " &
                               "" & (IIf(_Clave Is Nothing, "NULL", "'" & _Clave & "'")) & ", " &
                               "" & (IIf(_Puerto Is Nothing, "NULL", "'" & _Puerto & "'")) & ")"

            Return _sentenciaInsert

        End Get

        Set(value As String)

            _sentenciaInsert = value

        End Set

    End Property

    Property SentenciaUpdate As String

        Get
            _sentenciaUpdate = "update [SysExpert].[dbo].[Puertos] " &
                   "SET [idPais]  = " & (IIf(_IdPais Is Nothing, "NULL", _IdPais)) & ", " &
                   "[Clave]  = '" & (IIf(_Clave Is Nothing, "NULL", _Clave)) & "', " &
                   "[Puerto]  = '" & (IIf(_Puerto Is Nothing, "NULL", "'" & _Puerto & "'")) & "' " &
                   "WHERE idPuerto= " & _IdPuerto

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

        Dim _sql = "USE SysExpert; DECLARE @idNuevo INT EXEC getsemilla 'Puertos',1,@idNuevo OUTPUT  select @idNuevo  USE SOLIUM;"

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

        If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                    If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                        _IdPuerto = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)

                        Return _IdPuerto

                    End If

                End If

            End If

        End If

        Return _IdPuerto

    End Function

#End Region



End Class