Imports System.Web
Imports System.Web.Services
Imports System.Web.Script.Services
Imports System.Web.Script.Serialization

Imports AltaReferencia
Imports Ejecutivos
Imports MisEmpresasTargets
Imports Clientes
Imports Validador

Public Class alta_referencia
    Inherits System.Web.UI.Page

    Dim _listaEjecutivos As List(Of Dictionary(Of String, Object))

    Dim _listaEmpresas As List(Of Dictionary(Of String, Object))

    Dim _listaClientes As List(Of Dictionary(Of String, Object))

    Public ReadOnly Property listaEjecutivos As List(Of Dictionary(Of String, Object))

        Get

            Return _listaEjecutivos

        End Get

    End Property

    Public ReadOnly Property listaEmpresas As List(Of Dictionary(Of String, Object))

        Get

            Return _listaEmpresas

        End Get

    End Property

    Public ReadOnly Property listaClientes As List(Of Dictionary(Of String, Object))

        Get

            Return _listaClientes

        End Get

    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Ejecutivos_ = New Ejecutivos

        '_listaEjecutivos = Ejecutivos_.TodosLosQue(New Dictionary(Of String, String) From {{"i_Cve_DivisionMiEmpresa =", "1"}})

        Dim Empresas_ = New MisEmpresasTargets

        '_listaEmpresas = Empresas_.Todos()

        Dim Clientes_ = New Clientes

        '_listaClientes = Clientes_.TodosLosQue(New Dictionary(Of String, String) From {{"i_Cve_DivisionMiEmpresa =", "1"}, {"t_Nombre !=", ""}})

    End Sub

    Public Function customRule(ByVal value_ As String) As String

        If value_ = "01/20/2021" Then

            Return "El campo {c} no tiene una fecha válida"

        End If

        Return Nothing

    End Function

    <WebMethod>
    Public Shared Function GuardarDatos(ByVal Data_ As Dictionary(Of String, Object)) As Object

        Dim Response_ As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

        Dim Validador_ = New Validador(Data_)

        Validador_.CondicionarCampo("d_FechaRegistro", "Fecha de Registro", "required|callback_customRule")

        If Validador_.VerificarReglas() Then

            Dim Referencias_ = New AltaReferencia

            Dim Operacion_ As Boolean

            If Not String.IsNullOrEmpty(Data_("i_Cve_Referencia")) Then

                Operacion_ = Referencias_.Modificar(Integer.Parse(Data_("i_Cve_Referencia").ToString()), Data_)

            Else

                Operacion_ = Referencias_.Insertar(Data_)

            End If

            If Operacion_ Then

                Response_.Add("code", "200")
                Response_.Add("message", "Registro Procesado Correctamente")

            Else

                Response_.Add("code", "400")
                Response_.Add("message", "Ocurrio un Error al Procesar")

            End If

        Else

            Response_.Add("code", "400")
            Response_.Add("message", "Error de Validación")
            Response_.Add("errors", Validador_.ErroresValidacion)


        End If

        Return Response_

    End Function

    <WebMethod>
    Public Shared Function BorrarRegistro() As Object

        Dim Response_ As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

        Response_.Add("code", "200")
        Response_.Add("message", "Elemento borrado correctamente")

        Return Response_

    End Function

    <WebMethod>
    Public Shared Function ObtenerAltasReferencias() As Object

        Dim Referencias_ = New AltaReferencia

        Dim listaReferencias_ = Referencias_.Todos()

        Return listaReferencias_

    End Function

    <WebMethod>
    Public Shared Function ObtenerCliente(ByVal i_Cve_Cliente As Integer) As Object

        Dim clientes_ = New Clientes

        Dim cliente_ = clientes_.Encontrar(i_Cve_Cliente)

        Return cliente_

    End Function

    <WebMethod>
    Public Shared Function GuardarCliente(ByVal Data_ As Object) As Object

        Dim Response_ As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

        Dim Validador_ = New Validador(Data_)

        Validador_.CondicionarCampo("bussines_name", "Razón Social", "required|alphanumeric|match[rfc]")
        Validador_.CondicionarCampo("rfc", "RFC", "required")

        If Validador_.VerificarReglas() Then

            'Dim Referencias_ = New AltaReferencia

            Dim Operacion_ As Boolean = True

            'Operacion_ = Referencias_.Insertar(Data_)

            If Operacion_ Then

                Response_.Add("code", "200")
                Response_.Add("response", New Dictionary(Of String, String) From {{"key", "1"}, {"value", Data_("bussines_name").ToString()}})
                Response_.Add("message", "Registro Procesado Correctamente")

            Else

                Response_.Add("code", "400")
                Response_.Add("message", "Ocurrio un Error al Procesar")

            End If

        Else

            Response_.Add("code", "400")
            Response_.Add("message", "Error de Validación")
            Response_.Add("errors", Validador_.ErroresValidacion)


        End If

        Return Response_

    End Function

    <WebMethod>
    Public Shared Function GuardarEjecutivo(ByVal Data_ As Object) As Object

        Dim Response_ As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

        Dim Validador_ = New Validador(Data_)

        Validador_.CondicionarCampo("name", "Nombre Ejecutivo", "required|alphanumeric")

        If Validador_.VerificarReglas() Then

            'Dim Referencias_ = New AltaReferencia

            Dim Operacion_ As Boolean = True

            'Operacion_ = Referencias_.Insertar(Data_)

            If Operacion_ Then

                Response_.Add("code", "200")
                Response_.Add("response", New Dictionary(Of String, String) From {{"key", "1"}, {"value", Data_("name").ToString()}})
                Response_.Add("message", "Registro Procesado Correctamente")

            Else

                Response_.Add("code", "400")
                Response_.Add("message", "Ocurrio un Error al Procesar")

            End If

        Else

            Response_.Add("code", "400")
            Response_.Add("message", "Error de Validación")
            Response_.Add("errors", Validador_.ErroresValidacion)


        End If

        Return Response_

    End Function

End Class

