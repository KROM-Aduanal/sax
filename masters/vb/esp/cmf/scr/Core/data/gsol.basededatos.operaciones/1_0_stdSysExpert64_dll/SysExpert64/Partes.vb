Imports Gsol.BaseDatos.Operaciones
Imports Gsol

Public Class Partes

#Region "Atributos"

    ' Campos Tabla SysExpert

    Private _IdParte As String

    Private _Clave As String

    Private _Pt_Descripcion As String

    Private _IdFraccion As String

    Private _IdUnidad As String

    Private _IdCliente As String

    Private _IdTipo As String

    Private _IdAgencia As String

    Private _IdIdentificador As String

    Private _IdComplemento As String

    Private _Commodity As String

    Private _Proyecto As String

    Private _Validado As String

    Private _DescripcionAdicional As String

    Private _TipoImpExp As String

    Private _Habilitado As String

    Private _Peso As String

    Private _Planta As String

    Private _DescripcionCOVE As String

    Private _IdDescripcionCODE As String

    Private _IdProveedor As String

    Private _RutaImagen As String

    Private _IdTipoMaterial As String

    Private _sentenciaInsert As String

    Private _sentenciaUpdate As String

    Private _sentenciaDeshabilitar As String

    Private _sentenciaHabilitar As String

    Private _ioperaciones As OperacionesCatalogo

    Private _sistema As Organismo

    ' Campos Auxiliares

    Private _ClaveFraccionKB As String

    Private _ClaveUnidadKB As String

    Private _ClaveTipoKB As String

#End Region

#Region "Propiedades"

    Property IdParte As String

        Get

            Return _IdParte

        End Get

        Set(value As String)

            _IdParte = value

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

    Property Pt_Descripcion As String

        Get

            Return _Pt_Descripcion

        End Get

        Set(value As String)

            _Pt_Descripcion = value

        End Set

    End Property

    Property IdFraccion As String

        Get

            Return _IdFraccion

        End Get

        Set(value As String)

            _IdFraccion = value

        End Set

    End Property

    Property IdUnidad As String

        Get

            Return _IdUnidad

        End Get

        Set(value As String)

            _IdUnidad = value

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

    Property IdTipo As String

        Get

            Return _IdTipo

        End Get

        Set(value As String)

            _IdTipo = value

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

    Property IdIdentificador As String

        Get

            Return _IdIdentificador

        End Get

        Set(value As String)

            _IdIdentificador = value

        End Set

    End Property

    Property IdComplemento As String

        Get

            Return _IdComplemento

        End Get

        Set(value As String)

            _IdComplemento = value

        End Set

    End Property

    Property Commodity As String

        Get

            Return _Commodity

        End Get

        Set(value As String)

            _Commodity = value

        End Set

    End Property

    Property Proyecto As String

        Get

            Return _Proyecto

        End Get

        Set(value As String)

            _Proyecto = value

        End Set

    End Property

    Property Validado As String

        Get

            Return _Validado

        End Get

        Set(value As String)

            _Validado = value

        End Set

    End Property

    Property DescripcionAdicional As String

        Get

            Return _DescripcionAdicional

        End Get

        Set(value As String)

            _DescripcionAdicional = value

        End Set

    End Property

    Property TipoImpExp As String

        Get

            Return _TipoImpExp

        End Get

        Set(value As String)

            _TipoImpExp = value

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

    Property Peso As String

        Get

            Return _Peso

        End Get

        Set(value As String)

            _Peso = value

        End Set

    End Property

    Property Planta As String

        Get

            Return _Planta

        End Get

        Set(value As String)

            _Planta = value

        End Set

    End Property

    Property DescripcionCOVE As String

        Get

            Return _DescripcionCOVE

        End Get

        Set(value As String)

            _DescripcionCOVE = value

        End Set

    End Property

    Property IdDescripcionCODE As String

        Get

            Return _IdDescripcionCODE

        End Get

        Set(value As String)

            _IdDescripcionCODE = value

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

    Property RutaImagen As String

        Get

            Return _RutaImagen

        End Get

        Set(value As String)

            _RutaImagen = value

        End Set

    End Property

    Property IdTipoMaterial As String

        Get

            Return _IdTipoMaterial

        End Get

        Set(value As String)

            _IdTipoMaterial = value

        End Set

    End Property

    Property SentenciaInsert As String

        Get
            _sentenciaInsert = "INSERT INTO [SysExpert].[dbo].[Partes] " &
                                "([idParte] " &
                                ",[Clave] " &
                                ",[pt_Descripcion] " &
                                ",[idFraccion] " &
                                ",[idUnidad] " &
                                ",[idCliente] " &
                                ",[idTipo] " &
                                ",[idAgencia] " &
                                ",[idIdentificador] " &
                                ",[idComplemento] " &
                                ",[Commodity] " &
                                ",[Proyecto] " &
                                ",[Validado] " &
                                ",[DescripcionAdicional] " &
                                ",[TipoImpExp] " &
                                ",[habilitado] " &
                                ",[Peso] " &
                                ",[Planta] " &
                                ",[DescripcionCOVE] " &
                                ",[idDescripcionCODE] " &
                                ",[idProveedor] " &
                                ",[RutaImagen] " &
                                ",[idTipoMaterial]) " &
                                " VALUES " &
                                "(" & obtenerIdNuevo() & ", " &
                                "" & (IIf(_Clave Is Nothing, "''", "'" & _Clave & "'")) & ", " &
                                "" & (IIf(_Pt_Descripcion Is Nothing, "''", "'" & _Pt_Descripcion & "'")) & ", " &
                                "" & (IIf(_IdFraccion Is Nothing, "NULL", _IdFraccion)) & ", " &
                                "" & (IIf(_IdUnidad Is Nothing, "NULL", _IdUnidad)) & ", " &
                                "" & (IIf(_IdCliente Is Nothing, "NULL", _IdCliente)) & ", " &
                                "" & (IIf(_IdTipo Is Nothing, "NULL", _IdTipo)) & ", " &
                                "" & (IIf(_IdAgencia Is Nothing, "NULL", _IdAgencia)) & ", " &
                                "" & (IIf(_IdIdentificador Is Nothing, "NULL", _IdIdentificador)) & ", " &
                                "" & (IIf(_IdComplemento Is Nothing, "NULL", _IdComplemento)) & ", " &
                                "" & (IIf(_Commodity Is Nothing, "''", "'" & _Commodity & "'")) & ", " &
                                "" & (IIf(_Proyecto Is Nothing, "''", "'" & _Proyecto & "'")) & ", " &
                                "" & (IIf(_Validado Is Nothing, "''", "'" & _Validado & "'")) & ", " &
                                "" & (IIf(_DescripcionAdicional Is Nothing, "''", "'" & _DescripcionAdicional & "'")) & ", " &
                                "" & (IIf(_TipoImpExp Is Nothing, "NULL", _TipoImpExp)) & ", " &
                                "" & (IIf(_Habilitado Is Nothing, "NULL", _Habilitado)) & ", " &
                                "" & (IIf(_Peso Is Nothing, "NULL", "'" & _Peso & "'")) & ", " &
                                "" & (IIf(_Planta Is Nothing, "NULL", "'" & _Planta & "'")) & ", " &
                                "" & (IIf(_DescripcionCOVE Is Nothing, "NULL", "'" & _DescripcionCOVE & "'")) & ", " &
                                "" & (IIf(_IdDescripcionCODE Is Nothing, "NULL", _IdDescripcionCODE)) & ", " &
                                "" & (IIf(_IdProveedor Is Nothing, "NULL", _IdProveedor)) & ", " &
                                "" & (IIf(_RutaImagen Is Nothing, "NULL", _RutaImagen)) & ", " &
                                "" & (IIf(_IdTipoMaterial Is Nothing, "NULL", _IdTipoMaterial)) & ")"

            Return _sentenciaInsert

        End Get

        Set(value As String)

            _sentenciaInsert = value

        End Set

    End Property

    Property SentenciaUpdate As String

        Get
            _sentenciaUpdate = "UPDATE [SysExpert].[dbo].[Partes] " &
                              "SET Clave = " & (IIf(_Clave Is Nothing, "''", "'" & _Clave & "'")) & ", " &
                              " pt_Descripcion = " & (IIf(_Pt_Descripcion Is Nothing, "''", "'" & _Pt_Descripcion & "'")) & ", " &
                              " idFraccion = " & (IIf(_IdFraccion Is Nothing, "NULL", _IdFraccion)) & ", " &
                              " idUnidad = " & (IIf(_IdUnidad Is Nothing, "NULL", _IdUnidad)) & ", " &
                              " idTipo = " & (IIf(_IdTipo Is Nothing, "NULL", _IdTipo)) & ", " &
                              " Validado = " & (IIf(_Validado Is Nothing, "''", "'" & _Validado & "'")) & ", " &
                              " DescripcionAdicional = " & (IIf(_DescripcionAdicional Is Nothing, "''", "'" & _DescripcionAdicional & "'")) & ", " &
                              " habilitado = " & (IIf(_Habilitado Is Nothing, "NULL", _Habilitado)) & ", " &
                              " DescripcionCOVE = " & (IIf(_DescripcionCOVE Is Nothing, "NULL", "'" & _DescripcionCOVE & "'")) & " " &
                              "WHERE idParte = " & _IdParte

            Return _sentenciaUpdate

        End Get

        Set(value As String)

            _sentenciaUpdate = value

        End Set

    End Property

    Property ClaveFraccionKB As String

        Get

            Return _ClaveFraccionKB

        End Get

        Set(value As String)

            _ClaveFraccionKB = value

        End Set

    End Property

    Property ClaveUnidadKB As String

        Get

            Return _ClaveUnidadKB

        End Get

        Set(value As String)

            _ClaveUnidadKB = value

        End Set

    End Property

    Property ClaveTipoKB As String

        Get

            Return _ClaveTipoKB

        End Get

        Set(value As String)

            _ClaveTipoKB = value

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
                   "EXEC getsemilla 'Partes',1,@idNuevo " &
                   "OUTPUT SELECT @idNuevo " &
                   "USE SOLIUM;"

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(_sql)

        If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables Is Nothing Then

            If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Count <= 0 Then

                If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows Is Nothing Then

                    If Not _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count <= 0 Then

                        _IdParte = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)(0)

                        Return _IdParte

                    End If

                End If

            End If

        End If

        Return _IdParte

    End Function

#End Region

End Class
