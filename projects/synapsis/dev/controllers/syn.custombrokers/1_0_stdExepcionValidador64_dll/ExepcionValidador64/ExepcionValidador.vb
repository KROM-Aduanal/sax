Imports Syn.Nucleo.RecursosComercioExterior
Imports Wma.Exceptions

Public Class ExepcionValidador : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property nombrecampo As String

    Property nombrepresentacion As String

    Property idcampounico As Integer

    Property numeroregistro As Integer

    Property descripcionregistro As String

    Property erroresvalidacion As List(Of ErrorValidacion)

    Property estatus As TagWatcher

#End Region

#Region "Metodos"

    Public Sub Add()

        Dim prueba_ As New Dictionary(Of CamposPedimento, String) From {{CamposPedimento.CA_ACUSE_VALOR, "Impo"}}

    End Sub

    Public Sub Delete()



    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class