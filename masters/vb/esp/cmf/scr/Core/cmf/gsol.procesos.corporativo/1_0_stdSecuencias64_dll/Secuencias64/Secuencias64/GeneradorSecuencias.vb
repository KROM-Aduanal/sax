Imports Gsol.BaseDatos.Operaciones
Imports Gsol
Imports System.ComponentModel

Public Enum eTipoSecuencia
    Referencia = 1
    Polizas = 2
    Facturas = 3
    HouseBL = 4
End Enum

Public Enum eSubTipo
    Ninguno = 0
    <Description("IG")>
    PolizaIngreso = 1
    <Description("EG")>
    PolizaEgreso = 2
    <Description("DR")>
    PolizaDiario = 3
End Enum


Public Class GeneradorSecuencias

    Private _sistema As Gsol.Organismo
    Private _espaciotrabajo As IEspacioTrabajo
    Private _aplicaMes, _aplicaAnio As Boolean
    Private _anio, _mes, _valorMaximo As Integer
    Private _mask, _valorEnmascarado As String
    Private _consideraGrupoEmpresa As Boolean = False
    Private _grupoEmpresarial, _divisionEmpresarial As Integer
    Private _omitirDivisionMiEmpresa As Boolean = False

    Public Property Mascara() As String
        Get
            Return _mask
        End Get
        Set(ByVal value As String)
            _mask = value
        End Set
    End Property

    Public ReadOnly Property ValorEnmascarado() As String
        Get
            Return _valorEnmascarado
        End Get
    End Property


    Public Property ConsiderarGrupoEmpresarial() As Boolean
        Get
            Return _consideraGrupoEmpresa
        End Get
        Set(ByVal value As Boolean)
            _consideraGrupoEmpresa = value
        End Set
    End Property



    Public Property OmitirDivisionMiEmpresa() As Boolean
        Get
            Return _omitirDivisionMiEmpresa
        End Get
        Set(ByVal value As Boolean)
            _omitirDivisionMiEmpresa = value
        End Set
    End Property



    Public Sub New(ByVal p_oOrganismo_ As Gsol.Organismo, ByVal p_oEspacioTrabajo_ As IEspacioTrabajo)

        _sistema = p_oOrganismo_

        _espaciotrabajo = p_oEspacioTrabajo_

    End Sub

    Public Function ObtenerSiguiente(ByVal p_eTipoSecuencia As eTipoSecuencia, ByVal p_eSubTipo As eSubTipo, Optional ByVal anio_ As Integer = 0, Optional ByVal mes_ As Integer = 0) As Integer

        _anio = anio_

        _mes = mes_

        ValidarVariables()

        ValidaInsertaRegistroSecuencia(p_eTipoSecuencia, p_eSubTipo)

        Return GenerarConsecutivo(p_eTipoSecuencia, p_eSubTipo)

    End Function

    Public Function ObtenerSiguiente(ByVal p_eTipoSecuencia As eTipoSecuencia, Optional ByVal anio_ As Integer = 0, Optional ByVal mes_ As Integer = 0, Optional ByVal valorMaximo_ As Int32 = 0) As Integer

        _anio = anio_

        _mes = mes_

        _valorMaximo = valorMaximo_

        ValidarVariables()

        ValidaInsertaRegistroSecuencia(p_eTipoSecuencia, eSubTipo.Ninguno)

        Return GenerarConsecutivo(p_eTipoSecuencia, eSubTipo.Ninguno)

    End Function

    Private Sub ValidarVariables()

        If _anio < 0 Then

            _anio = Now.Year

        End If

        If _mes > 0 And _anio = 0 Then

            _anio = Now.Year

        End If

        _grupoEmpresarial = CInt(IIf(_consideraGrupoEmpresa, CStr(_espaciotrabajo.MisCredenciales.GrupoEmpresarial), "-1"))

        _divisionEmpresarial = CInt(IIf(_omitirDivisionMiEmpresa, "-1", CStr(_espaciotrabajo.MisCredenciales.DivisionEmpresaria)))

        If _mes < 0 Or _mes > 12 Then

            Throw New Exception("El mes no es válido")

        End If

    End Sub

    Private Function GenerarConsecutivo(ByVal p_eTipoSecuencia As eTipoSecuencia, ByVal p_eSubTipo As eSubTipo) As Integer

        Dim resultSet_ As DataSet = New DataSet()

        _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

        Dim exec_ As String = GenerarCadenaEjecucion(p_eTipoSecuencia, p_eSubTipo)

        If (_sistema.ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(exec_)) Then

            resultSet_ = _sistema.ConexionSingleton.SQLServerSingletonConexion.DataSetReciente

            If Not resultSet_ Is Nothing Then

                If resultSet_.Tables.Count > 0 Then

                    If resultSet_.Tables(0).Rows.Count > 0 Then

                        If resultSet_.Tables(0).Columns.Contains("ErrorNumber") Then

                            Throw New Exception(CStr(resultSet_.Tables(0).Rows(0)("ErrorMessage")))

                        End If

                        Dim consecutivo_ As Integer = CInt(resultSet_.Tables(0).Rows(0)(0))

                        If Not String.IsNullOrWhiteSpace(_mask) Then

                            _valorEnmascarado = consecutivo_.ToString(_mask)

                        End If

                        Return consecutivo_

                    End If

                End If

            End If

        End If

        Return -1

    End Function

    Private Function GenerarCadenaEjecucion(ByVal p_eTipoSecuencia As eTipoSecuencia, ByVal p_eSubTipo As eSubTipo) As String

        Dim exec_ As String = "EXEC	Sp000GeneraConsecutivo"

        exec_ += " @i_Cve_MisEmpresas = " & _divisionEmpresarial
        exec_ += ",@i_TipoSecuencia = " & CInt(p_eTipoSecuencia)
        exec_ += ",@i_SubTipoSecuencia = " & CInt(p_eSubTipo)
        exec_ += ",@i_Cve_GrupoEmpresarial = " & _grupoEmpresarial
        exec_ += ",@i_Anio =" & If(CStr(_anio), "null")
        exec_ += ",@i_Mes =" & If(CStr(_mes), "null")
        exec_ += ",@i_ValorMaximo =" & If(CStr(_valorMaximo), "null")

        Return exec_

    End Function

    Private Sub ValidaInsertaRegistroSecuencia(ByVal p_eTipoSecuencia As eTipoSecuencia, ByVal p_eSubTipo As eSubTipo)

        If Not ValidaExistencia(p_eTipoSecuencia, p_eSubTipo) Then

            CrearRegistro(p_eTipoSecuencia, p_eSubTipo)

        End If

    End Sub

    Public Function CrearRegistroSecuencia(ByVal p_eTipoSecuencia As eTipoSecuencia, Optional ByVal anio_ As Integer = 0, Optional ByVal mes_ As Integer = 0, Optional ByVal p_Subtipo As eSubTipo = eSubTipo.Ninguno) As Integer

        _anio = anio_

        _mes = mes_

        ValidarVariables()

        Return CrearRegistro(p_eTipoSecuencia, p_Subtipo)

    End Function

    Private Function ValidaExistenciaRegistroSecuencia(ByVal p_eTipoSecuencia As eTipoSecuencia, Optional ByVal anio_ As Integer = 0, Optional ByVal mes_ As Integer = 0, Optional ByVal p_Subtipo As eSubTipo = eSubTipo.Ninguno) As Boolean

        _anio = anio_

        _mes = mes_

        ValidarVariables()

        Return ValidaExistenciaRegistroSecuencia(p_eTipoSecuencia)

    End Function

    Private Function ValidaExistencia(ByVal p_eTipoSecuencia As eTipoSecuencia, ByVal p_eSubTipo As eSubTipo) As Boolean

        Dim catalogo_ As IOperacionesCatalogo

        catalogo_ = _sistema.EnsamblaModulo("Secuencias")

        catalogo_.ClausulasLibres = "AND ISNULL(i_Cve_DivisionMiEmpresa,-1)= " & _divisionEmpresarial & _
            " AND i_TipoSecuencia =" & p_eTipoSecuencia & _
            " AND i_SubTipoSecuencia =" & p_eSubTipo & _
           " AND i_Anio =" & _anio & _
            " AND i_Mes = " & _mes & _
            " AND ISNULL(i_Cve_GrupoEmpresarial,-1) =" & _grupoEmpresarial

        catalogo_.EspacioTrabajo = _espaciotrabajo

        catalogo_.GenerarVista()

        Return _sistema.TieneResultados(catalogo_)

    End Function

    Private Function CrearRegistro(ByVal p_eTipoSecuencia As eTipoSecuencia, ByVal p_eSubTipo As eSubTipo) As Integer

        Dim Secuencias_ As IOperacionesCatalogo

        Secuencias_ = _sistema.EnsamblaModulo("Secuencias")

        Secuencias_.EspacioTrabajo = _espaciotrabajo

        Secuencias_.PreparaCatalogo()

        Secuencias_.CampoPorNombre("i_TipoSecuencia") = CStr(CInt(p_eTipoSecuencia))

        Secuencias_.CampoPorNombre("i_SubTipoSecuencia") = CStr(CInt(p_eSubTipo))

        Secuencias_.CampoPorNombre("i_Anio") = CStr(_anio)

        If _omitirDivisionMiEmpresa = False Then

            Secuencias_.CampoPorNombre("i_Cve_DivisionMiEmpresa") = CStr(_espaciotrabajo.MisCredenciales.DivisionEmpresaria)

        End If

        If _consideraGrupoEmpresa Then

            Secuencias_.CampoPorNombre("i_Cve_GrupoEmpresarial") = CStr(_espaciotrabajo.MisCredenciales.GrupoEmpresarial)

        End If

        Secuencias_.CampoPorNombre("i_Mes") = CStr(_mes)

        Secuencias_.CampoPorNombre("f_Alta") = Now.ToString("dd-MM-yyyy HH:mm:ss")

        Secuencias_.CampoPorNombre("i_Cve_Estado") = "1"

        Secuencias_.Agregar()

        Return CInt(Secuencias_.ValorIndice)

    End Function


End Class
