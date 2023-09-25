Imports gsol.BaseDatos.Operaciones
Imports gsol

Public Class Proveedores

#Region "Atributos"

    ' Campos Tabla SysExpert

    Private _IdProveedor As String

    Private _ClaveProveedor As String

    Private _RazonSocial As String

    Private _TaxNumber As String

    Private _RFC As String

    Private _CURP As String

    Private _Calle As String

    Private _NumeroExterior As String

    Private _NumeroInterior As String

    Private _Colonia As String

    Private _CodigoPostal As String

    Private _Ciudad As String

    Private _IdEntidad As String

    Private _IdPais As String

    Private _Email As String

    Private _IdIncoterm As String

    Private _IdValoracion As String

    Private _FechaAlta As String

    Private _FechaModificacion As String

    Private _IdAgencia As String

    Private _IdCliente As String

    Private _Habilitado As String

    Private _ApellidoPaterno As String

    Private _ApellidoMaterno As String

    Private _Municipio As String

    Private _Nombre As String

    Private _CorreoElectronico As String

    Private _Telefono As String

    Private _NumExportadorConfiable As String

    Private _ClaveExternaExport As String

    Private _Glosado As String

    Private _sentenciaInsert As String

    Private _sentenciaUpdate As String

    Private _sentenciaDeshabilitarImportado As String

    Private _sentenciaDeshabilitar As String

    Private _sentenciaHabilitar As String

    Private _ioperaciones As OperacionesCatalogo

    Private _sistema As Organismo

    ' Campos Auxiliares

    Private _ClaveClienteKB As String

    Private _EntidadFederativaKB As String

    Private _ClavePaisKB As String

    Private _ClaveIncotermKB As String

    Private _ClaveValoracionKB As String

    Private _ClaveProveedorImportadoKB As String

    Private _TipoIdentificadorCOVEKB As String

#End Region

#Region "Propiedades"

    Property IdProveedor As String

        Get

            Return _IdProveedor

        End Get

        Set(value As String)

            _IdProveedor = value

        End Set

    End Property

    Property ClaveProveedor As String

        Get

            Return _ClaveProveedor

        End Get

        Set(value As String)

            _ClaveProveedor = value

        End Set

    End Property

    Property RazonSocial As String

        Get

            Return _RazonSocial

        End Get

        Set(value As String)

            _RazonSocial = value

        End Set

    End Property

    Property TaxNumber As String

        Get

            Return _TaxNumber

        End Get

        Set(value As String)

            _TaxNumber = value

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

    Property CURP As String

        Get

            Return _CURP

        End Get

        Set(value As String)

            _CURP = value

        End Set

    End Property

    Property Calle As String

        Get

            Return _Calle

        End Get

        Set(value As String)

            _Calle = value

        End Set

    End Property

    Property NumeroExterior As String

        Get

            Return _NumeroExterior

        End Get

        Set(value As String)

            _NumeroExterior = value

        End Set

    End Property

    Property NumeroInterior As String

        Get

            Return _NumeroInterior

        End Get

        Set(value As String)

            _NumeroInterior = value

        End Set

    End Property

    Property Colonia As String

        Get

            Return _Colonia

        End Get

        Set(value As String)

            _Colonia = value

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

    Property Ciudad As String

        Get

            Return _Ciudad

        End Get

        Set(value As String)

            _Ciudad = value

        End Set

    End Property

    Property IdEntidad As String

        Get

            Return _IdEntidad

        End Get

        Set(value As String)

            _IdEntidad = value

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

    Property Email As String

        Get

            Return _Email

        End Get

        Set(value As String)

            _Email = value

        End Set

    End Property

    Property IdIncoterm As String

        Get

            Return _IdIncoterm

        End Get

        Set(value As String)

            _IdIncoterm = value

        End Set

    End Property

    Property IdValoracion As String

        Get

            Return _IdValoracion

        End Get

        Set(value As String)

            _IdValoracion = value

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

    Property FechaModificacion As String

        Get

            Return _FechaModificacion

        End Get

        Set(value As String)

            _FechaModificacion = value

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

    Property IdCliente As String

        Get

            Return _IdCliente

        End Get

        Set(value As String)

            _IdCliente = value

        End Set

    End Property

    Property Habilitado As String

        Get

            Return _Habilitado

        End Get

        Set(value As String)

            _Habilitado = value

        End Set

    End Property

    Property ApellidoPaterno As String

        Get

            Return _ApellidoPaterno

        End Get

        Set(value As String)

            _ApellidoPaterno = value

        End Set

    End Property

    Property ApellidoMaterno As String

        Get

            Return _ApellidoMaterno

        End Get

        Set(value As String)

            _ApellidoMaterno = value

        End Set

    End Property

    Property Municipio As String

        Get

            Return _Municipio

        End Get

        Set(value As String)

            _Municipio = value

        End Set

    End Property

    Property Nombre As String

        Get

            Return _Nombre

        End Get

        Set(value As String)

            _Nombre = value

        End Set

    End Property

    Property CorreoElectronico As String

        Get

            Return _CorreoElectronico

        End Get

        Set(value As String)

            _CorreoElectronico = value

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

    Property NumExportadorConfiable As String

        Get

            Return _NumExportadorConfiable

        End Get

        Set(value As String)

            _NumExportadorConfiable = value

        End Set

    End Property

    Property ClaveExternaExport As String

        Get

            Return _ClaveExternaExport

        End Get

        Set(value As String)

            _ClaveExternaExport = value

        End Set

    End Property

    Property Glosado As String

        Get

            Return _Glosado

        End Get

        Set(value As String)

            _Glosado = value

        End Set

    End Property

    Property SentenciaInsert As String

        Get

            _sentenciaInsert = "INSERT INTO [SysExpert].[dbo].[Proveedores] " &
                               "([idProveedor] " &
                               ",[ClaveProveedor] " &
                               ",[RazonSocial] " &
                               ",[TaxNumber] " &
                               ",[RFC] " &
                               ",[CURP] " &
                               ",[Calle] " &
                               ",[NumeroExterior] " &
                               ",[NumeroInterior] " &
                               ",[Colonia] " &
                               ",[CodigoPostal] " &
                               ",[Ciudad] " &
                               ",[idEntidad] " &
                               ",[idPais] " &
                               ",[email] " &
                               ",[idIncoterm] " &
                               ",[idValoracion] " &
                               ",[FechaAlta] " &
                               ",[FechaModificacion] " &
                               ",[idAgencia] " &
                               ",[idCliente] " &
                               ",[habilitado] " &
                               ",[ApellidoPaterno] " &
                               ",[ApellidoMaterno] " &
                               ",[Municipio] " &
                               ",[Nombre] " &
                               ",[correoElectronico] " &
                               ",[telefono] " &
                               ",[NumExportadorConfiable] " &
                               ",[ClaveExternaExport] " &
                               ",[Glosado]) " &
                               " VALUES  " &
                               "(" & obtenerIdNuevo() & ", " &
                               "" & obtenerClaveNuevo() & ", " &
                               "" & (IIf(_RazonSocial Is Nothing, "''", "'" & _RazonSocial & "'")) & ", " &
                               "" & (IIf(_TaxNumber Is Nothing, "''", "'" & _TaxNumber & "'")) & ", " &
                               "" & (IIf(_RFC Is Nothing, "''", "'" & _RFC & "'")) & ", " &
                               "" & (IIf(_CURP Is Nothing, "''", "'" & _CURP & "'")) & ", " &
                               "" & (IIf(_Calle Is Nothing, "''", "'" & _Calle & "'")) & ", " &
                               "" & (IIf(_NumeroExterior Is Nothing, "''", "'" & _NumeroExterior & "'")) & ", " &
                               "" & (IIf(_NumeroInterior Is Nothing, "''", "'" & _NumeroInterior & "'")) & ", " &
                               "" & (IIf(_Colonia Is Nothing, "''", "'" & _Colonia & "'")) & ", " &
                               "" & (IIf(_CodigoPostal Is Nothing, "''", "'" & _CodigoPostal & "'")) & ", " &
                               "" & (IIf(_Ciudad Is Nothing, "''", "'" & _Ciudad & "'")) & ", " &
                               "" & (IIf(_IdEntidad Is Nothing, "NULL", _IdEntidad)) & ", " &
                               "" & (IIf(_IdPais Is Nothing, "NULL", _IdPais)) & ", " &
                               "" & (IIf(_Email Is Nothing, "''", "'" & _Email & "'")) & ", " &
                               "" & (IIf(_IdIncoterm Is Nothing, "NULL", _IdIncoterm)) & ", " &
                               "" & (IIf(_IdValoracion Is Nothing, "NULL", _IdValoracion)) & ", " &
                               "" & (IIf(_FechaAlta Is Nothing, "NULL", "'" & _FechaAlta & "'")) & ", " &
                               "" & (IIf(_FechaModificacion Is Nothing, "NULL", "'" & _FechaModificacion & "'")) & ", " &
                               "" & (IIf(_IdAgencia Is Nothing, "NULL", _IdAgencia)) & ", " &
                               "" & (IIf(_IdCliente Is Nothing, "NULL", _IdCliente)) & ", " &
                               "" & (IIf(_Habilitado Is Nothing, "NULL", _Habilitado)) & ", " &
                               "" & (IIf(_ApellidoPaterno Is Nothing, "NULL", "'" & _ApellidoPaterno & "'")) & ", " &
                               "" & (IIf(_ApellidoMaterno Is Nothing, "NULL", "'" & _ApellidoMaterno & "'")) & ", " &
                               "" & (IIf(_Municipio Is Nothing, "NULL", "'" & _Municipio & "'")) & ", " &
                               "" & (IIf(_Nombre Is Nothing, "NULL", "'" & _Nombre & "'")) & ", " &
                               "" & (IIf(_CorreoElectronico Is Nothing, "NULL", "'" & _CorreoElectronico & "'")) & ", " &
                               "" & (IIf(_Telefono Is Nothing, "NULL", "'" & _Telefono & "'")) & ", " &
                               "" & (IIf(_NumExportadorConfiable Is Nothing, "NULL", "'" & _NumExportadorConfiable & "'")) & ", " &
                               "" & (IIf(_ClaveExternaExport Is Nothing, "NULL", "'" & _ClaveExternaExport & "'")) & ", " &
                               "" & (IIf(_Glosado Is Nothing, "NULL", _Glosado)) & ")"

            Return _sentenciaInsert

        End Get

        Set(value As String)

            _sentenciaInsert = value

        End Set

    End Property

    Property SentenciaUpdate As String

        Get

            _sentenciaUpdate = "UPDATE [SysExpert].[dbo].[Proveedores] " &
                               "SET idIncoterm   = " & (IIf(_IdIncoterm Is Nothing, "NULL", _IdIncoterm)) & ", " &
                               " idValoracion = " & (IIf(_IdValoracion Is Nothing, "NULL", _IdValoracion)) & ", " &
                               " email = " & (IIf(_Email Is Nothing, "''", "'" & _Email & "'")) & ", " &
                               " correoElectronico = " & (IIf(_CorreoElectronico Is Nothing, "NULL", "'" & _CorreoElectronico & "'")) & ", " &
                               " FechaModificacion = " & (IIf(_FechaModificacion Is Nothing, "NULL", "'" & _FechaModificacion & "'")) & ", " &
                               " habilitado = " & (IIf(_Habilitado Is Nothing, "NULL", _Habilitado)) & ", " &
                               " telefono = " & (IIf(_Telefono Is Nothing, "NULL", "'" & _Telefono & "'")) & " " &
                               "WHERE idProveedor = " & _IdProveedor

            Return _sentenciaUpdate

        End Get

        Set(value As String)

            _sentenciaUpdate = value

        End Set

    End Property

    Property SentenciaDeshabilitarImportado As String

        Get
            _sentenciaDeshabilitarImportado = "UPDATE [SysExpert].[dbo].[Proveedores] " &
                                              " SET [habilitado] = 0 ," &
                                              " FechaModificacion = '" & System.DateTime.Now.ToString("dd/MM/yyyy") & "' " &
                                              "WHERE idProveedor = " & _ClaveProveedorImportadoKB

            Return _sentenciaDeshabilitarImportado

        End Get

        Set(value As String)

            _sentenciaDeshabilitarImportado = value

        End Set

    End Property

    Property SentenciaDeshabilitar As String

        Get
            _sentenciaDeshabilitar = "UPDATE [SysExpert].[dbo].[Proveedores] " &
                                     " SET [habilitado] = 0, " &
                                     " FechaModificacion = '" & System.DateTime.Now.ToString("dd/MM/yyyy") & "' " &
                                     "WHERE idProveedor = " & _IdProveedor

            Return _sentenciaDeshabilitar

        End Get

        Set(value As String)

            _sentenciaDeshabilitar = value

        End Set

    End Property

    Property SentenciaHabilitar As String

        Get
            _sentenciaHabilitar = "UPDATE [SysExpert].[dbo].[Proveedores] " &
                                  " SET [habilitado] = 1, " &
                                  " FechaModificacion = '" & System.DateTime.Now.ToString("dd/MM/yyyy") & "' " &
                                  "WHERE idProveedor = " & _IdProveedor

            Return _sentenciaHabilitar

        End Get

        Set(value As String)

            _sentenciaHabilitar = value

        End Set

    End Property

    Property ClaveClienteKB As String

        Get

            Return _ClaveClienteKB

        End Get

        Set(value As String)

            _ClaveClienteKB = value

        End Set

    End Property

    Property EntidadFederativaKB As String

        Get

            Return _EntidadFederativaKB

        End Get

        Set(value As String)

            _EntidadFederativaKB = value

        End Set

    End Property

    Property ClavePaisKB As String

        Get

            Return _ClavePaisKB

        End Get

        Set(value As String)

            _ClavePaisKB = value

        End Set

    End Property

    Property ClaveIncotermKB As String

        Get

            Return _ClaveIncotermKB

        End Get

        Set(value As String)

            _ClaveIncotermKB = value

        End Set

    End Property

    Property ClaveValoracionKB As String

        Get

            Return _ClaveValoracionKB

        End Get

        Set(value As String)

            _ClaveValoracionKB = value

        End Set

    End Property

    Property ClaveProveedorImportadoKB As String

        Get

            Return _ClaveProveedorImportadoKB

        End Get

        Set(value As String)

            _ClaveProveedorImportadoKB = value

        End Set

    End Property

    Property TipoIdentificadorCOVEKB As String

        Get

            Return _TipoIdentificadorCOVEKB

        End Get

        Set(value As String)

            _TipoIdentificadorCOVEKB = value

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
                   "EXEC getsemilla 'Proveedores',1,@idNuevo " &
                   "OUTPUT SELECT @idNuevo " &
                   "USE SOLIUM;"

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

        If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                    If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                        _IdProveedor = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)

                        Return _IdProveedor

                    End If

                End If

            End If

        End If

        Return _IdProveedor

    End Function

    Private Function obtenerClaveNuevo() As Integer

        Dim _sql = "USE SysExpert; " &
                   "DECLARE @ClaveNuevo INT " &
                   "SELECT @ClaveNuevo = (MAX(ClaveProveedor) +1) FROM Proveedores " &
                   "OUTPUT select @ClaveNuevo " &
                   "USE SOLIUM;"

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

        If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                    If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                        _ClaveProveedor = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)

                        Return _ClaveProveedor

                    End If

                End If

            End If

        End If

        Return _ClaveProveedor

    End Function

#End Region

End Class
