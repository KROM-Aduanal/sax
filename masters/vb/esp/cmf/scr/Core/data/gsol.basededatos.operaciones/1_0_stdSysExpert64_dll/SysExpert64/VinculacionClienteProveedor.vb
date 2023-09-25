Imports Gsol.BaseDatos.Operaciones
Imports Gsol

Public Class VinculacionClienteProveedor

#Region "Atributos"

    ' Campos Tabla SysExpert

    Private _IdVincCliPro As String

    Private _IdProveedor As String

    Private _IdCliente As String

    Private _IdVinculacion As String

    Private _Porcentaje As String

    Private _sentenciaInsert As String

    Private _sentenciaDelete As String

    Private _ioperaciones As OperacionesCatalogo

    Private _sistema As Organismo

    ' Campos Auxiliares

    Private _IdProveedorClienteVinculadoKB As String

    Private _ClaveVinculacionKB As String

#End Region

#Region "Propiedades"

    Property IdVincCliPro As String

        Get

            Return _IdVincCliPro

        End Get

        Set(value As String)

            _IdVincCliPro = value

        End Set

    End Property

    Property IdProveedor As String

        Get

            Return _IdProveedor

        End Get

        Set(value As String)

            _IdProveedor = value

        End Set

    End Property

    Property IdCliente As String

        Get

            Return _IdCliente

        End Get

        Set(value As String)

            _IdCliente = value

        End Set

    End Property

    Property IdVinculacion As String

        Get

            Return _IdVinculacion

        End Get

        Set(value As String)

            _IdVinculacion = value

        End Set

    End Property

    Property Porcentaje As String

        Get

            Return _Porcentaje

        End Get

        Set(value As String)

            _Porcentaje = value

        End Set

    End Property

    Property ClaveVinculacionKB As String

        Get

            Return _ClaveVinculacionKB

        End Get

        Set(value As String)

            _ClaveVinculacionKB = value

        End Set

    End Property

    Property IdProveedorClienteVinculadoKB As String

        Get

            Return _IdProveedorClienteVinculadoKB

        End Get

        Set(value As String)

            _IdProveedorClienteVinculadoKB = value

        End Set

    End Property

    Property SentenciaInsert As String

        Get

            _sentenciaInsert = "INSERT INTO [SysExpert].[dbo].[VinculacionCliPro] " &
                               "([idVincCliPro], " &
                               "[idProveedor], " &
                               "[idCliente], " &
                               "[idVinculacion], " &
                               "[Porcentaje]) " &
                               "VALUES  " &
                               "(" & obtenerIdNuevo() & ", " &
                               "" & (IIf(_IdProveedor Is Nothing, "NULL", _IdProveedor)) & ", " &
                               "" & (IIf(_IdCliente Is Nothing, "NULL", _IdCliente)) & ", " &
                               "" & (IIf(_IdVinculacion Is Nothing, "NULL", _IdVinculacion)) & ", " &
                               "" & (IIf(_Porcentaje Is Nothing, "NULL", _Porcentaje)) & ")"

            Return _sentenciaInsert

        End Get

        Set(value As String)

            _sentenciaInsert = value

        End Set

    End Property

    Property SentenciaDelete As String

        Get

            _sentenciaDelete = "DELETE [SysExpert].[dbo].[VinculacionCliPro] WHERE idProveedor = "

            Return _sentenciaDelete

        End Get


        Set(value As String)

            _sentenciaDelete = value

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

        Dim _sql = "USE SysExpert; " &
                   "DECLARE @idNuevo INT " &
                   "EXEC getsemilla 'VinculacionCliPro',1,@idNuevo " &
                   "OUTPUT SELECT @idNuevo " &
                   "USE SOLIUM;"

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

        If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                    If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                        _IdVincCliPro = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)

                        Return _IdVincCliPro

                    End If

                End If

            End If

        End If

        Return _IdVincCliPro

    End Function

#End Region

End Class
