Imports Wma.Exceptions

Public Class ErrorValidacion : Implements ICloneable

#Region "Enums"

    Enum TiposCriterios
        SinDefinir = 0
    End Enum

    Enum Acciones
        SinDefinir = 0
    End Enum

    Enum TiposMensajes
        SinDefinir = 0
    End Enum

#End Region

#Region "Propiedades"

    Property Tipo As String

    Property Numero As Integer

    Property Descripcion As String

    Property TipoCriterio As TiposCriterios

    Property Accion As Acciones

    Property TipoMensaje As TiposMensajes

    Property Mensaje As String

#End Region

#Region "Metodos"

    Public Sub Add()

    End Sub

    Public Sub Delete()

    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class