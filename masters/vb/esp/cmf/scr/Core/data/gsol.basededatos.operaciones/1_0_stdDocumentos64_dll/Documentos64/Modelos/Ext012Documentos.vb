Imports Wma.Exceptions
Imports Gsol.BaseDatos.Operaciones
Imports Gsol

Public Class Ext012Documentos

#Region "Atributos"

    Private _i_Cve_Documento As Integer

    Private _t_NombreDocumento As String

    Private _t_URLPublico As String

    Private _i_Cve_TipoDocumento As Integer

    Private _i_Cve_TipoArchivo As Integer

    Private _i_ConsultableCliente As Integer

    Private _i_Cve_OrigenDocumento As Integer

    Private _i_Cve_Estatus As Integer

    Private _i_Cve_Estado As Integer

    Private _i_Cve_DivisionMiEmpresa As Integer

    Private _TagWatcher As TagWatcher

#End Region

#Region "Propiedades"

    Public Property i_Cve_Documento As Integer

        Get

            Return _i_Cve_Documento

        End Get

        Set(value As Integer)

            _i_Cve_Documento = value

        End Set

    End Property

    Public Property t_NombreDocumento As String

        Get

            Return _t_NombreDocumento

        End Get

        Set(value As String)

            _t_NombreDocumento = value

        End Set

    End Property

    Public Property t_URLPublico As String

        Get

            Return _t_URLPublico

        End Get

        Set(value As String)

            _t_URLPublico = value

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

    Public Property i_Cve_TipoArchivo As Integer

        Get

            Return _i_Cve_TipoArchivo

        End Get

        Set(value As Integer)

            _i_Cve_TipoArchivo = value

        End Set

    End Property

    Public Property i_ConsultableCliente As Integer

        Get

            Return _i_ConsultableCliente

        End Get

        Set(value As Integer)

            _i_ConsultableCliente = value

        End Set

    End Property

    Public Property i_Cve_Estatus As Integer

        Get

            Return _i_Cve_Estatus

        End Get

        Set(value As Integer)

            _i_Cve_Estatus = value

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

        End Set

    End Property

    Public Property i_Cve_OrigenDocumento As Integer

        Get

            Return _i_Cve_OrigenDocumento

        End Get

        Set(value As Integer)

            _i_Cve_OrigenDocumento = value

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

#End Region

#Region "Contructores"

    Sub New()

        _i_Cve_Documento = 0

        _t_NombreDocumento = Nothing

        _t_URLPublico = Nothing

        _i_Cve_TipoDocumento = 0

        _i_Cve_TipoArchivo = 0

        _i_ConsultableCliente = 0

        _i_Cve_Estatus = 1

        _i_Cve_Estado = 1

        _i_Cve_DivisionMiEmpresa = 0

        _TagWatcher = New TagWatcher

    End Sub

#End Region

End Class