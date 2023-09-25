
Public Class Estructuras

    'Enum enmRequerimientosAuditoria
    '    No = 0
    '    Si
    '    Condicional
    'End Enum

    Public Structure Referencia

        Public i_Cve_Sistema As Int32

        Public i_Cve_Referencia As Int32

        Public t_Cve_Referencia As String

    End Structure

End Class

Public Class AuditoriasPrevias

#Region "Atributos"

    Public i_Cve_Catalogo As Integer

    Public NombreCorto As String

    Public i_Cve_CatalogoAuditoriaPrevia As Integer

    Public NombreCortoAuditoriaPrevia As String

    Public tieneRegistroAuditoriaViva As Boolean

    Public i_Cve_Politica As Integer

    Public requiereAuditoriaSegunPolitica As Boolean

    'Public tieneAuditoriaSegunPolitica As Boolean ' Estructuras.enmRequerimientosAuditoria
    Public mensaje As String

#End Region


End Class

Public Class CatalogoAuditorias

    Public i_Cve_RegistroAuditoria As Integer

    Private i_Cve_CatalogoAuditoria As Integer

    Private i_Cve_MaestroOperaciones As Integer


End Class

Public Class CamposClausulasLibres

    Public _nombreCampo As String

    Public _comparacion As String

    Public _valorCampo As String

    Public _tipoDato As AdministracionAuditorias.enmTipoDato

    Public Sub New(ByVal nombreCampo_ As String, ByVal comparacion_ As String, ByVal valorCampo_ As String, _
                   ByVal tipoDato_ As AdministracionAuditorias.enmTipoDato)

        _nombreCampo = nombreCampo_

        _comparacion = comparacion_

        _valorCampo = valorCampo_

        _tipoDato = tipoDato_

    End Sub

End Class

Public Class ModuloClausulaLibreArmada

    Public _token As String

    Public _clausula As String

End Class

