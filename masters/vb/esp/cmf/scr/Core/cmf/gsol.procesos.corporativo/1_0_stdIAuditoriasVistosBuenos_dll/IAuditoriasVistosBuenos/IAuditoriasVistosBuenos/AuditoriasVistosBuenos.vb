Imports Gsol.BaseDatos.Operaciones

Namespace gsol.procesos.corporativo
    Public Interface IAuditoriasVistosBuenos

#Region "Propiedades"

        ReadOnly Property MensajeError As String

#End Region

#Region "Métodos"

        Function GuardarInformacion(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                   ByVal i_Cve_Referencia As Integer, _
                                   ByVal i_Cve_Sistema As Integer) As Boolean

        Function CancelaAuditoria(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                 ByVal i_Cve_Referencia_ As Integer, _
                                 ByVal i_Cve_Sistema_ As Integer, _
                                 ByVal t_Observaciones_ As String) As Boolean

        Function ExisteAuditoria(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                    ByVal i_Cve_Referencia As Integer, _
                                    ByVal i_Cve_Sistema As Integer) As Boolean

        Function ConsultaInformacionTrafico(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                                ByVal i_Cve_Referencia As Integer, _
                                                ByVal i_Cve_Sistema As Integer, _
                                                ByVal modulo_ As String) As IOperacionesCatalogo

        Function GuardarInformacion(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                        ByVal datos_ As IOperacionesCatalogo) As Boolean

        Function ConsultaClaveRegistroAuditoriaVivaDesdeReferencia(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                                                       ByVal i_Cve_Referencia As Integer, _
                                                                       ByVal i_Cve_Sistema As Integer)

#End Region

    End Interface

End Namespace
