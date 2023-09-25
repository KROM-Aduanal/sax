Imports Wma.Exceptions
Imports Gsol.BaseDatos.Operaciones
Imports Gsol

Public Class Reg012Documentos

#Region "Atributos"

    Private _i_Cve_Documento As Integer

    Private _t_RutaDocumentoOrigen As String

    Private _i_Cve_TipoDocumento As Integer

    Private _f_FechaRegistro As DateTime

    Private _i_Cve_EstatusDocumento As String

    Private _t_NombreDocumento As String

    Private _t_Extension As String

    Private _t_FolioDocumento As String

    Private _i_Cve_RepositorioDigital As Integer

    Private _t_Documento As String

    Private _i_Cve_Estado As Integer

    Private _i_Cve_DivisionMiEmpresa As Integer

    Private _t_VersionCFDi As Integer

    Private _i_Cve_EstatusLiquidacion As Integer

    Private _t_RutaGuardado As String

    Private _t_RutaExtension As String

    Private _t_RutaDocumento As String

    Private _TagWatcher As TagWatcher

    Private _Ext012Documentos As Ext012Documentos

    Private _TipoRegistro As TipoRegistro

    Private _operaciones As OperacionesCatalogo

#End Region

#Region "Enums"

    Enum TipoRegistro

        DocumentoNoIdentificado

        DocumentoEmail

        DocumentoGeneral

    End Enum

#End Region

#Region "Propiedades"

    Public Property i_Cve_Documento As Integer

        Get

            Return _i_Cve_Documento

        End Get

        Set(value As Integer)

            _i_Cve_Documento = value

            _Ext012Documentos.i_Cve_Documento = value

        End Set

    End Property

    Public Property t_RutaDocumentoOrigen As String

        Get

            Return _t_RutaDocumentoOrigen

        End Get

        Set(value As String)

            _t_RutaDocumentoOrigen = value

        End Set

    End Property

    Public Property t_RutaGuardado As String

        Get

            Return _t_RutaGuardado

        End Get

        Set(value As String)

            _t_RutaGuardado = value

        End Set

    End Property

    Public Property t_RutaExtension As String

        Get

            Return _t_RutaExtension

        End Get

        Set(value As String)

            _t_RutaExtension = value

        End Set

    End Property

    Public Property t_RutaDocumento As String

        Get

            Return _t_RutaDocumento

        End Get

        Set(value As String)

            _t_RutaDocumento = value

        End Set

    End Property

    Public Property i_Cve_TipoDocumento As Integer

        Get

            Return _i_Cve_TipoDocumento

        End Get

        Set(value As Integer)

            _i_Cve_TipoDocumento = value

        End Set

    End Property

    Public Property f_FechaRegistro As DateTime

        Get

            Return _f_FechaRegistro

        End Get

        Set(value As DateTime)

            _f_FechaRegistro = value

        End Set

    End Property

    Public Property i_Cve_EstatusDocumento As Integer

        Get

            Return _i_Cve_EstatusDocumento

        End Get

        Set(value As Integer)

            _i_Cve_EstatusDocumento = value

        End Set

    End Property

    Public Property t_NombreDocumento As String

        Get

            Return _t_NombreDocumento

        End Get

        Set(value As String)

            _t_NombreDocumento = value

            Ext012Documentos.t_NombreDocumento = value

            Dim delimiter_ = "."

            Dim substrings() As String = _t_NombreDocumento.Split(delimiter_)

            _t_Extension = "." & substrings(1)

        End Set

    End Property

    Public Property t_FolioDocumento As String

        Get

            Return _t_FolioDocumento

        End Get

        Set(value As String)

            _t_FolioDocumento = value

        End Set

    End Property

    Public Property i_Cve_RepositorioDigital As Integer

        Get

            Return _i_Cve_RepositorioDigital

        End Get

        Set(value As Integer)

            _i_Cve_RepositorioDigital = value

        End Set

    End Property

    Public Property t_Documento As String

        Get

            Return _t_Documento

        End Get

        Set(value As String)

            _t_Documento = value

        End Set

    End Property

    Public Property i_Cve_Estado As Integer

        Get

            Return _i_Cve_Estado

        End Get

        Set(value As Integer)

            _i_Cve_Estado = value

        End Set

    End Property

    Public Property i_Cve_DivisionMiEmpresa As Integer

        Get

            Return _i_Cve_DivisionMiEmpresa

        End Get

        Set(value As Integer)

            _i_Cve_DivisionMiEmpresa = value

            Ext012Documentos.i_Cve_DivisionMiEmpresa = value

        End Set

    End Property

    Public Property t_VersionCFDi As String

        Get

            Return _t_VersionCFDi

        End Get

        Set(value As String)

            _t_VersionCFDi = value

        End Set

    End Property

    Public Property i_Cve_EstatusLiquidacion As Integer

        Get

            Return _i_Cve_EstatusLiquidacion

        End Get

        Set(value As Integer)

            _i_Cve_EstatusLiquidacion = value

        End Set

    End Property

    Public Property TagWatcher As TagWatcher

        Get

            Return _TagWatcher

        End Get

        Set(value As TagWatcher)

            _TagWatcher = value

        End Set

    End Property

    Public ReadOnly Property TipoRegistroDocumento As TipoRegistro

        Get

            Return _TipoRegistro

        End Get

    End Property

    Public Property Ext012Documentos As Ext012Documentos

        Get

            Return _Ext012Documentos

        End Get

        Set(value As Ext012Documentos)

            _Ext012Documentos = value

        End Set

    End Property

#End Region

#Region "Contructores"

    Sub New()

        _i_Cve_Documento = 0

        _t_RutaDocumentoOrigen = Nothing

        _i_Cve_TipoDocumento = 0

        _f_FechaRegistro = Date.Now

        _i_Cve_EstatusDocumento = 1

        _t_NombreDocumento = Nothing

        _t_FolioDocumento = Nothing

        _i_Cve_RepositorioDigital = 0

        _t_Documento = Nothing

        _i_Cve_Estado = 0

        _i_Cve_DivisionMiEmpresa = 0

        _t_VersionCFDi = 0

        _i_Cve_EstatusLiquidacion = 0

        _TagWatcher = New TagWatcher

        _TipoRegistro = TipoRegistro.DocumentoNoIdentificado

        _Ext012Documentos = New Ext012Documentos

    End Sub

    Sub New(ByVal TipoRegistro_ As TipoRegistro,
            ByVal ioperaciones_ As IOperacionesCatalogo)

        _i_Cve_Documento = 0

        _t_RutaDocumentoOrigen = Nothing

        _i_Cve_TipoDocumento = 0

        _f_FechaRegistro = Date.Now

        _i_Cve_EstatusDocumento = 1

        _t_NombreDocumento = Nothing

        _t_FolioDocumento = Nothing

        _i_Cve_RepositorioDigital = 0

        _t_Documento = Nothing

        _i_Cve_Estado = 1

        _i_Cve_DivisionMiEmpresa = 0

        _t_VersionCFDi = Nothing

        _i_Cve_EstatusLiquidacion = 1

        _TagWatcher = New TagWatcher

        _TipoRegistro = TipoRegistro_

        _Ext012Documentos = New Ext012Documentos

        _operaciones = ioperaciones_


    End Sub

#End Region

#Region "Metodos"

    Public Sub CargarInformacion()

        Select Case _TipoRegistro

            Case TipoRegistro.DocumentoEmail

                CargarInformacionDocumentoEmail()

            Case TipoRegistro.DocumentoGeneral

                CargarInformacionDocumentoGeneral()

        End Select

        BuscarRepositorioDigital()

        If _i_Cve_TipoDocumento = 0 Then

            _TagWatcher.Status = TagWatcher.TypeStatus.Errors

            _TagWatcher.Errors = TagWatcher.ErrorTypes.C6_012_1000

        ElseIf _Ext012Documentos.i_Cve_TipoArchivo = 0 Then

            _TagWatcher.Status = TagWatcher.TypeStatus.Errors

            _TagWatcher.Errors = TagWatcher.ErrorTypes.C6_012_1001

        ElseIf _i_Cve_RepositorioDigital = 0 Then

            _TagWatcher.Status = TagWatcher.TypeStatus.Errors

            _TagWatcher.Errors = TagWatcher.ErrorTypes.C6_012_1002

        End If

    End Sub

    Private Sub CargarInformacionDocumentoEmail()

        BuscarTipoDocumento(" and i_Cve_TipoDocumento in (10,11,12,13,14,15,16,17,18,19,20,21,22,23,24)")

    End Sub

    Private Sub CargarInformacionDocumentoGeneral()

        BuscarTipoDocumento(" and i_Cve_TipoDocumento = " & _i_Cve_TipoDocumento)

    End Sub

    Public Sub BuscarTipoDocumento(ByVal SQLExtension_ As String)

        Dim sql_ = Nothing

        Dim TipoDocumento_ = New OperacionesCatalogo

        TipoDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

        Dim sistema_ = New Organismo

        TipoDocumento_.CantidadVisibleRegistros = 1

        sql_ = SQLExtension_ & " and t_Extension = '" & _t_Extension & "' " & ""

        TipoDocumento_ = sistema_.ConsultaModulo(TipoDocumento_.EspacioTrabajo,
                                                                "TiposDocumentos",
                                                                sql_)

        If sistema_.TieneResultados(TipoDocumento_) Then

            For Each fila_ As DataRow In TipoDocumento_.Vista.Tables(0).Rows

                _i_Cve_TipoDocumento = fila_.Item("Clave").ToString

                _Ext012Documentos.i_Cve_TipoDocumento = fila_.Item("Clave").ToString

                _Ext012Documentos.i_Cve_TipoArchivo = fila_.Item("Clave tipo de archivo").ToString

            Next

        Else

        End If

    End Sub

    Public Sub BuscarRepositorioDigital()

        Dim RepositorioDigital_ = New OperacionesCatalogo

        RepositorioDigital_.EspacioTrabajo = _operaciones.EspacioTrabajo

        Dim sistema_ = New Organismo

        RepositorioDigital_.CantidadVisibleRegistros = 1

        Dim sql_ = " and i_Cve_TipoDocumento = " & _i_Cve_TipoDocumento

        sql_ = sql_ & " and i_Cve_DivisionMiEmpresa = " & _i_Cve_DivisionMiEmpresa

        RepositorioDigital_ = sistema_.ConsultaModulo(RepositorioDigital_.EspacioTrabajo,
                                                      "RepositoriosDigitales",
                                                       sql_)

        If sistema_.TieneResultados(RepositorioDigital_) Then

            For Each fila_ As DataRow In RepositorioDigital_.Vista.Tables(0).Rows

                _i_Cve_RepositorioDigital = fila_.Item("Clave").ToString

                _t_RutaGuardado = fila_.Item("Ruta Guardado").ToString

                _t_RutaDocumento = _t_RutaGuardado & "" & t_RutaExtension

            Next

        Else

        End If

    End Sub

#End Region


End Class